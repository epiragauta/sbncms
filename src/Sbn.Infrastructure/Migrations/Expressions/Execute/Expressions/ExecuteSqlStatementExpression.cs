using NPoco;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Execute.Expressions
{
    public class ExecuteSqlStatementExpression : MigrationExpressionBase
    {
        public ExecuteSqlStatementExpression(IMigrationContext context)
            : base(context)
        { }

        public virtual string SqlStatement { get; set; }

        public virtual Sql<ISqlContext> SqlObject { get; set; }

        public void ExecuteSqlObject()
        {
            Execute(SqlObject);
        }

        protected override string GetSql()
        {
            return SqlStatement;
        }
    }
}
