using Sbn.Cms.Infrastructure.Migrations.Expressions.Alter.Table;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Alter
{
    /// <summary>
    /// Builds an Alter expression.
    /// </summary>
    public interface IAlterBuilder : IFluentBuilder
    {
        /// <summary>
        /// Specifies the table to alter.
        /// </summary>
        IAlterTableBuilder Table(string tableName);
    }
}
