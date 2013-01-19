using NHibernate;
using System.Diagnostics;


namespace TemplateSrc.NHibernateProvider
{
	public class BowlingAppStatementInterceptor : EmptyInterceptor
	{
		public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
		{
			Debug.WriteLine(sql.ToString());
			return sql;
		}
	}
}
