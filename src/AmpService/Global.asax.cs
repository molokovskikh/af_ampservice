using System;
using System.Reflection;
using System.Web;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Common.Models;
using Common.Models.Repositories;
using Common.MySql;
using Common.Service;
using Common.Service.Interceptors;
using Common.Service.Models;
using log4net;
using log4net.Config;
using NHibernate.Mapping.Attributes;
using With = Common.MySql.With;

namespace AmpService
{
	public class Global : HttpApplication
	{
		private readonly ILog _log = LogManager.GetLogger(typeof (Global));

		protected void Application_Start(object sender, EventArgs e)
		{
			try
			{
				Initialize();
			}
			catch (Exception ex)
			{
				_log.Error("Не удалось запустить приложение", ex);
			}
		}

		public static void Initialize(string connectionStringName = "Main")
		{
			XmlConfigurator.Configure();
			GlobalContext.Properties["Version"] = Assembly.GetExecutingAssembly().GetName().Version;
			ConnectionHelper.DefaultConnectionStringName = "Main";
			With.DefaultConnectionStringName = ConnectionHelper.GetConnectionName();
			var sessionFactoryHolder = new SessionFactoryHolder(ConnectionHelper.GetConnectionName());
			sessionFactoryHolder
				.Configuration
				.AddInputStream(HbmSerializer.Default.Serialize(typeof(ServiceLogEntity).Assembly))
				.AddInputStream(HbmSerializer.Default.Serialize(typeof(FutureClient).Assembly));

			IoC.Container.Register(
				Component.For<Service>().Interceptors(
					InterceptorReference.ForType<ErrorLoggingInterceptor>(),
					InterceptorReference.ForType<LoggingInterceptor>(),
					InterceptorReference.ForType<ContextLoaderInterceptor>(),
					InterceptorReference.ForType<PermissionCheckInterceptor>(),
					InterceptorReference.ForType<UpdateLastAccessTimeInterceptor>(),
					InterceptorReference.ForType<MonitorExecutingTimeInterceptor>()
				).Anywhere,

				Component.For<ErrorLoggingInterceptor>(),
				Component.For<ContextLoaderInterceptor>(),
				Component.For<LoggingInterceptor>()
					.Parameters(Parameter.ForKey("ServiceName").Eq("Amp")),
				Component.For<PermissionCheckInterceptor>()
					.Parameters(Parameter.ForKey("Permission").Eq("IOL")),
				Component.For<UpdateLastAccessTimeInterceptor>()
					.Parameters(Parameter.ForKey("Field").Eq("IOLTime")),
				Component.For<MonitorExecutingTimeInterceptor>(),

				Component.For<LockMonitor>()
					.Parameters(Parameter.ForKey("TimeOut").Eq("10000")),
				Component.For<IClientLoader>().ImplementedBy<ClientLoader>(),

				Component.For<ISessionFactoryHolder>().Instance(sessionFactoryHolder),
				Component.For<RepositoryInterceptor>(),
				Component.For(typeof (IRepository<>)).ImplementedBy(typeof (Repository<>)),
				Component.For<ISecurityRepository>().ImplementedBy<SecurityRepository>(),
				Component.For<IOfferRepository>().ImplementedBy<OfferRepository>(),
				Component.For<ILogRepository>().ImplementedBy<LogRepository>()
				);

			ServiceContext.GetUserName = () => ServiceContext.NormalizeUsername(HttpContext.Current.User.Identity.Name);
			ServiceContext.GetHost = () => HttpContext.Current.Request.UserHostAddress;
			IoC.Resolve<LockMonitor>().Start();
		}

		public static void Deinitialize()
		{
			IoC.Resolve<LockMonitor>().Stop();
			IoC.Container.Dispose();
		}

		protected void Application_End(object sender, EventArgs e)
		{
			Deinitialize();
		}
	}
}