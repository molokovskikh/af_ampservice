using System.Collections.Generic;
using System.Reflection;
using Castle.Windsor;
using Common.Models.Repositories;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Common.Models.Tests.Repositories
{
	public class SessionFactoryHolder : ISessionFactoryHolder
	{
		private readonly ISessionFactory _sessionFactory;

		public SessionFactoryHolder()
		{
			var configuration = new Configuration();
			configuration.Configure();
			configuration.AddInputStream(HbmSerializer.Default.Serialize(Assembly.Load("Common.Models")));
			_sessionFactory = configuration.BuildSessionFactory();
		}

		public ISessionFactory SessionFactory
		{
			get { return _sessionFactory; }
		}
	}

	[TestFixture]
	public class OfferRepositiryFixture
	{
		private IOfferRepository repository;
		private Client client;

		[SetUp]
		public void SetUp()
		{
			var windsorContainer = new WindsorContainer();
			windsorContainer.AddComponent("RepositoryInterceptor", typeof(RepositoryInterceptor));
			windsorContainer.AddComponent("SessionFactoryHolder", typeof(ISessionFactoryHolder), typeof(SessionFactoryHolder));
			windsorContainer.AddComponent("OfferRepository", typeof(IOfferRepository), typeof(OfferRepository));
			IoC.Initialize(windsorContainer);

			repository = IoC.Resolve<IOfferRepository>();
			client = new Client {FirmCode = 2575};
		}

		[Test]
		public void GetOffersWithoutCategoryTest()
		{
			var offers = repository.FindAllForSmartOrder(client);
			float lastCost = 0;
			uint lastFullCode = 0;

			foreach (var offer in offers)
			{
				Assert.That(!offer.Junk);
				if (offer.ProductId == lastFullCode)
				{
					Assert.That(offer.Cost, Is.GreaterThanOrEqualTo(lastCost));
				}
				else
				{
					Assert.That(offer.ProductId, Is.GreaterThan(lastFullCode));
					lastFullCode = offer.ProductId;
					lastCost = offer.Cost;
				}
			}
		}

		[Test]
		public void GetOffersWithCategoryTest()
		{
			var offers = repository.FindAllForSmartOrderWithCategory(client);
			float lastCost = 0;
			uint lastFullCode = 0;
			uint lastCategory = 0;

			foreach (var offer in offers)
			{
				Assert.That(!offer.Junk);
				if (offer.ProductId == lastFullCode)
				{
					Assert.That(offer.Cost, Is.GreaterThanOrEqualTo(lastCost));
					Assert.That(offer.PriceList.FirmCategory, Is.LessThanOrEqualTo(lastCategory));
				}
				else
				{
					Assert.That(offer.ProductId, Is.GreaterThan(lastFullCode));
					lastCategory = offer.PriceList.FirmCategory;
					lastFullCode = offer.ProductId;
					lastCost = offer.Cost;
				}
			}
		}

		[Test]
		public void GetOffersByIds()
		{
			var offers = repository.FindAllForSmartOrder(client);
			var list = repository.FindByIds(client, new[] {offers[0].Id});
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list[0].Id, Is.EqualTo(offers[0].Id));
		}
	}
}
