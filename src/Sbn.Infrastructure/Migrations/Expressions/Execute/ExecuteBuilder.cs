using NPoco;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Execute.Expressions;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Execute
{
    public class ExecuteBuilder : ExpressionBuilderBase<ExecuteSqlStatementExpression>,
        IExecuteBuilder, IExecutableBuilder
    {
        public ExecuteBuilder(IMigrationContext context)
            : base(new ExecuteSqlStatementExpression(context))
        { }

        /// <inheritdoc />
        public void Do()
        {
            // slightly awkward, but doing it right would mean a *lot*
            // of changes for MigrationExpressionBase

            if (Expression.SqlObject == null)
                Expression.Execute();
            else
                Expression.ExecuteSqlObject();
        }

        /// <inheritdoc />
        public IExecutableBuilder Sql(string sqlStatement)
        {
            Expression.SqlStatement = sqlStatement;
            return this;
        }

        /// <inheritdoc />
        public IExecutableBuilder Sql(Sql<ISqlContext> sql)
        {
            Expression.SqlObject = sql;
            return this;
        }
    }
}
