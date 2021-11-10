using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Delete.ForeignKey
{
    /// <summary>
    /// Builds a Delete expression.
    /// </summary>
    public interface IDeleteForeignKeyOnTableBuilder : IFluentBuilder
    {
        /// <summary>
        /// Specifies the table of the foreign key.
        /// </summary>
        IExecutableBuilder OnTable(string foreignTableName);
    }
}
