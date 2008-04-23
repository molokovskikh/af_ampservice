using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using log4net;

namespace AmpService
{
	public class LockMonitor
	{
		private readonly Thread _monitorThread;
		private bool _stopped;
		private readonly Dictionary<HttpRequest, DateTime> _monitoringRequests = new Dictionary<HttpRequest, DateTime>();
		private readonly ILog _log = LogManager.GetLogger(typeof (LockMonitor));

		public LockMonitor()
		{
			try
			{
				_monitorThread = new Thread(Monitor);
				_monitorThread.Start();
			}
			catch(Exception ex)
			{
				_log.Error("Ошибка в конструкторе", ex);
			}
		}

		public void Add(HttpRequest request)
		{
			try
			{
				lock (_monitoringRequests)
					_monitoringRequests.Add(request, DateTime.Now);
			}
			catch(Exception ex)
			{
				_log.Error("Ошибка при добавлении запроса монитору", ex);
			}
		}

		public void Remove(HttpRequest request)
		{
			try
			{
				lock (_monitoringRequests)
					if (_monitoringRequests.ContainsKey(request))
						_monitoringRequests.Remove(request);
			}
			catch(Exception ex)
			{
				_log.Error("Ошибка при удалении запроса из монитора", ex);
			}
		}

		public void Stop()
		{
			try
			{
				_stopped = true;
				_monitorThread.Join(2000);
				if (_monitorThread.ThreadState != ThreadState.Stopped)
					_monitorThread.Abort();
			}
			catch(Exception ex)
			{
				_log.Error("Ошибка при остановке монитора", ex);
			}
		}

		private void Monitor()
		{
			try
			{
				while (!_stopped)
				{
					lock (_monitoringRequests)
					{
						var toNotify = new List<HttpRequest>();
						foreach (var pair in _monitoringRequests)
							if (DateTime.Now - pair.Value > TimeSpan.FromSeconds(10))
								toNotify.Add(pair.Key);

						if (toNotify.Count > 0)
						{
							var requests = "";

							foreach (var request in toNotify)
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
{0}
", requests);
						}

						foreach (var request in toNotify)
							_monitoringRequests.Remove(request);
					}
					Thread.Sleep(1000);
				}
			}
			catch(Exception ex)
			{
				_log.Error("Ошибка в работе монтора", ex);
			}
		}

	}
}
