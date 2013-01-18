using System.Web.Mvc;
using TemplateSrc.NHibernateProvider;
using NHibernate;
using SharpLite.Domain.DataInterfaces;
using SharpLite.NHibernateProvider;
using StructureMap;
using System;
using ServiceStack.ServiceClient.Web;
using System.Web.Http;
using Microsoft.Practices.ServiceLocation;
using StructureMap.ServiceLocatorAdapter;

namespace TemplateSrc.Init
{
	public class DependencyResolverInitializer
	{
		public static void Initialize()
		{
			_container = new Container(x =>
			{
				x.For<ISessionFactory>()
					.Singleton()
					.Use(() => NHibernateInitializer.Initialize().BuildSessionFactory());
				x.For<IEntityDuplicateChecker>().Use<EntityDuplicateChecker>();
				x.For(typeof(IRepository<>)).Use(typeof(Repository<>));
				x.For(typeof(IRepositoryWithTypedId<,>)).Use(typeof(RepositoryWithTypedId<,>));
			});

			//IDependencyResolver drStructureMap = new StructureMapDependencyResolver(_container);
			//DependencyResolver.SetResolver(drStructureMap);
			var smServiceLocator = new StructureMapServiceLocator(_container);
			ServiceLocator.SetLocatorProvider(() => smServiceLocator);

		}

		public static void AddDependency(Type pluginType, Type concreteType)
		{
			_container.Configure(x => x.For(pluginType).Use(concreteType));
		}

		private static Container _container;
	}
}