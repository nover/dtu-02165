using System.IO;
using TemplateSrc.NHibernateProvider;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TemplateSrc.Tests.NHibernateProvider
{
    /// <summary>
    /// Provides a means to verify that the target database is in compliance with all mappings.
    /// Taken from http://ayende.com/Blog/archive/2006/08/09/NHibernateMappingCreatingSanityChecks.aspx.
    /// 
    /// If this is failing, the error will likely inform you that there is a missing table or column
    /// which needs to be added to your database.
    /// </summary>
    [TestClass]
    public class MappingIntegrationTests
    {
        [TestInitialize]
        public virtual void SetUp() {
            _configuration = NHibernateInitializer.Initialize();
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                var session = this._sessionFactory.GetCurrentSession();

                if (session != null)
                {
                    if (session.Transaction.IsActive)
                    {
                        session.Transaction.Rollback();
                    }
                    session.Disconnect();
                    session.Clear();
                    session.Close();
                    session.Dispose();
                    session = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            this._sessionFactory.Close();
            this._sessionFactory.Dispose();
        }

        [TestMethod]
        public void CanConfirmDatabaseMatchesMappings() {
            var allClassMetadata = _sessionFactory.GetAllClassMetadata();

            foreach (var entry in allClassMetadata) {
                _sessionFactory.OpenSession().CreateCriteria(entry.Value.GetMappedClass(EntityMode.Poco))
                    .SetMaxResults(0).List();
            }
        }

        /// <summary>
        /// Generates and outputs the database schema SQL to the console
        /// </summary>
        [TestMethod]
        public void CanGenerateDatabaseSchema() {
            using (ISession session = _sessionFactory.OpenSession()) {
                using (TextWriter stringWriter = new StreamWriter("./UnitTestGeneratedSchema.sql")) {
                    new SchemaExport(_configuration).Execute(true, false, false, session.Connection, stringWriter);
                }
            }
        }

        private Configuration _configuration;
        private ISessionFactory _sessionFactory;
    }
}
