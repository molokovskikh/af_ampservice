using System;
using System.Web;

namespace AmpService
{
	public class Global : HttpApplication
	{
		private static readonly LockMonitor _monitor = new LockMonitor();

		protected void Application_Start(object sender, EventArgs e)
		{
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			_monitor.Remove(Request);
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			_monitor.Add(Request);
		}

		protected void Application_End(object sender, EventArgs e)
		{
			_monitor.Stop();
		}
	}
}