using System;
using System.Web;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Xml.Serialization;
using log4net;

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
		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue,
			MySqlConnection connection, bool raiseException, GetClientCodeDelegate getClientCode, bool closeConnection, BeforeSleepDelegate<T> beforeSleep) where T : ExecuteArgs
		{
			var log = LogManager.GetLogger(typeof(MethodTemplate));

			MySqlTransaction transaction = null;
			MySqlDataAdapter dataAdapter = null;

			R result = defaultValue;
			var Quit = false;
			var userName = String.Empty;
			try
			{
				do
				{
					try
					{
						try
						{
							userName = HttpContext.Current.User.Identity.Name;
						}
						catch
						{
							userName = Environment.UserName;
						}
						if (userName.IndexOf("ANALIT\\") == 0)
							userName = userName.Substring(7, userName.Length - 7);

						if (connection == null)
							connection = new MySqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["DB"]));

						if (connection.State == ConnectionState.Closed)
							connection.Open();

						dataAdapter = new MySqlDataAdapter();
						dataAdapter.SelectCommand = new MySqlCommand();
						transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

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
					catch (MySqlException MySQLErr)
					{
						if (transaction != null)
						{
							transaction.Rollback();
						}
						if ((MySQLErr.Number == 1205) || (MySQLErr.Number == 1213) || (MySQLErr.Number == 1422))
						{
							if (beforeSleep != null)
								beforeSleep(e, MySQLErr);
							Thread.Sleep(10000);
						}
						else
						{
							Quit = true;
							if (raiseException)
								throw;
							else
								log.Error(Utils.FormatException(MySQLErr, e, userName));
						}
					}
					catch (Exception ex)
					{
						Quit = true;

						if (transaction != null)
							transaction.Rollback();

						if (raiseException)
							throw;
						else
							log.Error(Utils.FormatException(ex, e, userName));
					}

				} while (!Quit);

			}
			finally
			{
				if ((connection != null) && closeConnection)
					connection.Close();
			}
			return result;
		}

		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue, 
			MySqlConnection connection) where T : ExecuteArgs
		{
			return ExecuteMethod(e, d, defaultValue, connection, false, GetClientCode, true, null);
		}

		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue,
			GetClientCodeDelegate getClientCode) where T : ExecuteArgs
		{
			return ExecuteMethod(e, d, defaultValue, null, false, getClientCode, true, null);
		}

		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue,
			MySqlConnection connection, GetClientCodeDelegate getClientCode) where T : ExecuteArgs
		{
			return ExecuteMethod(e, d, defaultValue, connection, false, getClientCode, true, null);
		}

		public static R ExecuteMethod<T, R>(T e, MethodDelegate<T, R> d, R defaultValue) where T : ExecuteArgs
		{
			return ExecuteMethod(e, d, defaultValue, null, false, GetClientCode, true, null);
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
