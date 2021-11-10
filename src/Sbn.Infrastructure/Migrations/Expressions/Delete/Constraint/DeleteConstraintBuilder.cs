using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Delete.Expressions;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Delete.Constraint
{
    public class DeleteConstraintBuilder : ExpressionBuilderBase<DeleteConstraintExpression>,
        IDeleteConstraintBuilder
    {
        public DeleteConstraintBuilder(DeleteConstraintExpression expression)
            : base(expression)
        { }

        /// <inheritdoc />
        public IExecutableBuilder FromTable(string tableName)
        {
            Expression.Constraint.TableName = tableName;
            return new ExecutableBuilder(Expression);
        }
    }
}
