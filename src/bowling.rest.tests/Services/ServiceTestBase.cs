using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate;
using TemplateSrc.NHibernateProvider;
using NHibernate.Tool.hbm2ddl;
using Bowling.Entity.Domain;
using TemplateSrc.Init;
using Microsoft.Practices.ServiceLocation;
using SharpLite.NHibernateProvider;

namespace bowling.rest.tests.Services
{
	[TestClass]
	public class ServiceTestBase
	{
		protected Configuration _configuration = null;
		protected ISession _session = null;
		protected ISessionFactory _sessionFactory = null;
		[TestInitialize]
		public void TestInit()
		{
			DependencyResolverInitializer.Initialize();
			
			//this._configuration = NHibernateInitializer.Initialize();
			this._sessionFactory = LocalSessionInitializer.Initialize();
			this._session = this._sessionFactory.GetCurrentSession();
			
			//new SchemaExport(_configuration).Execute(false, true, false);


			// add some lanes and timeslots to the database
			// there are timeslots from 1000 hours => 2300 hours
			for (int i = 0; i < 10; i+=2)
			{
				_session.Save(new TimeSlot() { Start = TimeSpan.FromHours(10 + i), End = TimeSpan.FromHours(11 + i) });
				_session.Save(new Lane() { Number = i + 1 });
			}
		}

		[TestCleanup]
		public void TestCleanup()
		{
			if (this._session != null)
			{
				if (this._session.Transaction.IsActive)
				{
					this._session.Transaction.Rollback();
				}
				this._session.Disconnect();
				this._session.Clear();
				this._session.Close();
				this._session.Dispose();
				this._session = null;
			}
			this._sessionFactory.Close();
			this._sessionFactory.Dispose();
		}

		[ClassCleanup]
		public void ClassCleanup()
		{
			this.TestCleanup();
		}
	}
}
