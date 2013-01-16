using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateSrc.NHibernateProvider
{
    internal class NhibernateSchemaValidator
    {
        public static bool ValidateSchema(Configuration config)
        {
            NHibernate.Tool.hbm2ddl.SchemaValidator myvalidator = new NHibernate.Tool.hbm2ddl.SchemaValidator(config);
            try
            {
                myvalidator.Validate();
                myvalidator = null;
                return true;
            }
            catch (Exception)
            {
            }
            finally
            {
                myvalidator = null;
            }

            return false;
        }
    }
}
