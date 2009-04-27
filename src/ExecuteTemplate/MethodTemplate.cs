using System;
using System.Configuration;
using System.Web;
using System.Threading;
using Common.MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Serialization;
using log4net;
using MySqlHelper=MySql.Data.MySqlClient.MySqlHelper;

namespace ExecuteTemplate
{
	public delegate R MethodDelegate<T, R>(T e);

	public class ExecuteArgs : EventArgs
	{
		public ulong ClientCode { get; set; }

		[XmlIgnore]
		public MySqlDataAdapter DataAdapter { get; set; }
	}

	public delegate ulong GetClientCodeDelegate(MySqlConnection connection, string userName);

	public delegate void BeforeSleepDelegate<T>(T e, MySqlException ex) where T : ExecuteArgs;

	public sealed class MethodTemplate
	{
		/// <summary>
		/// Ўаблон метода дл€ выполнени€ действий к базе данных, которые повтор€ютс€ в случае возникновени€ ошибок типа deadlock
		/// </summary>
		/// <typeparam name="T">“ип передаваевых параметров, наследников от ExecuteArgs</typeparam>
		/// <typeparam name="R">“ип возвращаемого значени€</typeparam>
		/// <param name="e">ѕередаваемые параметры</param>
		/// <param name="d">ƒелегат, который будет отрабатывать внутри метода</param>
		/// <param name="defaultValue">значение по-умолчанию дл€ возращаемого значени€</param>
		/// <param name="connection">—оединение с базой данных, если имеет значение null, то будет создано внутри метода</param>
		/// <param name="raiseException">Ќеобходимо ли вызывать исключение наверх или гасить его в методе</param>
		/// <param name="getClientCode">ƒелегат дл€ получени€ кода клиента, если null, то код клиента устанавливаетс€ в 0</param>
		/// <param name="closeConnection">“ребуетс€ ли закрывать соединение после отработки метода?</param>
		/// <param name="beforeSleep">ƒанный делегат вызываетс€ перед тем, как метод сделает Sleep после ошибки MySql</param>
		/// <returns></returns>
		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue, GetClientCodeDelegate getClientCode, bool canUseSlave) where T : ExecuteArgs
		{
			var log = LogManager.GetLogger(typeof(MethodTemplate));

			MySqlTransaction transaction = null;
			MySqlDataAdapter dataAdapter = null;

			R result = defaultValue;
			var Quit = false;
			var userName = String.Empty;
				do
				{
					using (var connection = GetConnection(canUseSlave))
					{
						try
						{
							connection.Open();
							transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

							try
							{
								userName = HttpContext.Current.User.Identity.Name;
							}
							catch (Exception)
							{
								userName = Environment.UserName;
							}
							

							if (userName.IndexOf("ANALIT\\") == 0)
								userName = userName.Substring(7, userName.Length - 7);

							dataAdapter = new MySqlDataAdapter();
							dataAdapter.SelectCommand = new MySqlCommand();
							dataAdapter.SelectCommand.Connection = connection;
							dataAdapter.SelectCommand.Transaction = transaction;

							e.DataAdapter = dataAdapter;

							if (getClientCode != null)
								e.ClientCode = getClientCode(connection, userName);
							else
								e.ClientCode = 0;

							result = d(e);

							transaction.Commit();

							Quit = true;
						}
						catch (Exception ex)
						{
							if (transaction != null)
								transaction.Rollback();

							if (ExceptionHelper.IsDeadLockOrSimilarExceptionInChain(ex))
							{
								Thread.Sleep(10000);
							}
							else
							{
								Quit = true;
								log.Error(Utils.FormatException(ex, e, userName));
							}
						}
					}
				} while (!Quit);

			return result;
		}

		private static MySqlConnection GetConnection(bool canUseSlave)
		{
			if (canUseSlave)
				return new ConnectionManager().GetConnection();
			return new MySqlConnection(ConfigurationManager.ConnectionStrings["Main"].ConnectionString);
		}

		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue) where T : ExecuteArgs
		{
			return ExecuteMethod(e, d, defaultValue, GetClientCode, true);
		}

		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue, GetClientCodeDelegate getClientCode) where T : ExecuteArgs
		{
			return ExecuteMethod(e, d, defaultValue, getClientCode, true);
		}

		public static ulong GetClientCode(MySqlConnection connection, string userName)
		{
			return Convert.ToUInt64(MySqlHelper.ExecuteScalar(connection, @"
SELECT 
    osuseraccessright.clientcode 
FROM clientsdata, 
    osuseraccessright 
WHERE osuseraccessright.clientcode = clientsdata.firmcode 
    AND firmstatus = 1 
	AND allowGetData = 1
    AND billingstatus = 1 
    AND OSUserName = ?UserName", new[] { new MySqlParameter("?UserName", userName) }));
		}

	}

}
