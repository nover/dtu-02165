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
using System.Collections.Generic;
using System.Linq;

namespace bowling.rest.tests.Services
{
	[TestClass]
	public class ServiceTestBase
	{
		protected Configuration _configuration = null;
		protected ISession _session = null;
		protected ISessionFactory _sessionFactory = null;

		protected List<Lane> lanes;
		protected List<TimeSlot> timeSlots;

		[TestInitialize]
		public void TestInit()
		{
			this.lanes = new List<Lane>();
			this.timeSlots = new List<TimeSlot>();
			DependencyResolverInitializer.Initialize();

			this._configuration = ServiceLocator.Current.GetInstance<Configuration>();
			this._sessionFactory = LocalSessionInitializer.Initialize();
			this._session = this._sessionFactory.GetCurrentSession();
			
			new SchemaExport(_configuration).Execute(false, true, false);


			// add some lanes and timeslots to the database
			// there are timeslots from 1000 hours => 2300 hours
			for (int i = 0; i < 10; i+=2)
			{
				var timeSlot = new TimeSlot() { Start = TimeSpan.FromHours(10 + i), End = TimeSpan.FromHours(11 + i) };
				this.timeSlots.Add(timeSlot);
				_session.Save(timeSlot);

				var lane = new Lane() { Number = i + 1 };
				this.lanes.Add(lane);
				_session.Save(lane);
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
