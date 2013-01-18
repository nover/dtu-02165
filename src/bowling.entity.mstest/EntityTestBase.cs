using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemplateSrc.NHibernateProvider;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;

namespace bowling.entity.mstest
{
    [TestClass]
    public abstract class EntityTestBase
    {
        protected Configuration _configuration = null;
        protected ISession _session = null;
        protected ISessionFactory _sessionFactory = null;
        [TestInitialize]
        public void TestInit()
        {

            this._configuration = NHibernateInitializer.Initialize();
            this._sessionFactory = _configuration.BuildSessionFactory();
            this._session = _sessionFactory.OpenSession();
            new SchemaExport(_configuration).Execute(false, true, false);
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
    }
}
