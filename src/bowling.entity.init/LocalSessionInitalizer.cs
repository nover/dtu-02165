using Microsoft.Practices.ServiceLocation;
using NHibernate;
using SharpLite.NHibernateProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateSrc.NHibernateProvider
{
	/// <summary>
	/// Nhibernate session provider for non web projects
	/// </summary>
	public static class LocalSessionInitializer
	{
		public static ISessionFactory Initialize()
		{
			try
			{
				ISessionFactory factory = ServiceLocator.Current.GetInstance<ISessionFactory>();
				LazySessionContext.Bind(new Lazy<ISession>(() => BeginSession(factory)), factory);
				return factory;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static ISession BeginSession(ISessionFactory sessionFactory)
		{
			var session = sessionFactory.OpenSession();
			session.BeginTransaction();
			return session;
		}
	}
}
