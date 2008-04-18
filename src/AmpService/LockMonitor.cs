using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using log4net;
using MySql.Data.MySqlClient;

namespace AmpService
{
	public class LockMonitor
	{
		private readonly Thread _monitorThread;
		private bool _stopped = false;
		private readonly Dictionary<HttpRequest, DateTime> _monitoringRequests = new Dictionary<HttpRequest, DateTime>();
		private readonly ILog _log = LogManager.GetLogger(typeof (LockMonitor));

		public LockMonitor()
		{
			_monitorThread = new Thread(Monitor);
			_monitorThread.Start();
		}

		public void Add(HttpRequest request)
		{
			lock (_monitoringRequests)
				_monitoringRequests.Add(request, DateTime.Now);
		}

		public void Remove(HttpRequest request)
		{
			lock (_monitoringRequests)
				if (_monitoringRequests.ContainsKey(request))
					_monitoringRequests.Remove(request);
		}

		public void Stop()
		{
			_stopped = true;
			_monitorThread.Join(2000);
			if (_monitorThread.ThreadState != ThreadState.Stopped)
				_monitorThread.Abort();
		}

		private void Monitor()
		{
			while (!_stopped)
			{
				lock (_monitoringRequests)
				{
					List<HttpRequest> toNotify = new List<HttpRequest>();
					foreach (KeyValuePair<HttpRequest, DateTime> pair in _monitoringRequests)
						if (DateTime.Now - pair.Value > TimeSpan.FromSeconds(10))
							toNotify.Add(pair.Key);

					if (toNotify.Count > 0)
					{
						string innodbStatus;
						using (MySqlConnection connection = new MySqlConnection(Literals.ConnectionString))
						{
							connection.Open();
							MySqlCommand command = connection.CreateCommand();
							command.CommandText = "show engine innodb status;";
							using (MySqlDataReader reader = command.ExecuteReader())
							{
								reader.Read();
								innodbStatus = (string) reader[2];
							}
						}

						string requests = "";

						foreach (HttpRequest request in toNotify)
						{
							requests += String.Format("Запрос: {0}\r\n", request.Path);
							NameValueCollection parameters;
							if (request.RequestType == "GET")
								parameters = request.QueryString;
							else
								parameters = request.Form;
							foreach (string key in parameters)
								requests += String.Format("{0}: {1}\r\n", key, request.Params[key]);

							requests += "\r\n";
						}


						_log.ErrorFormat(@"Следующие запросы ожидают более 10 секунд: 
{0}{1}
", requests, innodbStatus);
					}

					foreach (HttpRequest request in toNotify)
						_monitoringRequests.Remove(request);
				}
				Thread.Sleep(1000);
			}
		}

	}
}
