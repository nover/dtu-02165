using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemplateSrc.NHibernateProvider;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace bowling.entity.mstest
{
    [TestClass]
    public abstract class EntityTestBase
    {
        protected ISession _session = null;
        [TestInitialize]
        public void TestInit()
        {

            var _configuration = NHibernateInitializer.Initialize();
            var _sessionFactory = _configuration.BuildSessionFactory();
            this._session = _sessionFactory.OpenSession();
            new SchemaExport(_configuration).Execute(false, true, false);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (this._session != null)
            {
                this._session.Clear();
                this._session.Close();
                this._session = null;
            }
        }
    }
}
