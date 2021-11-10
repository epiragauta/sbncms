using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Alter.Table
{
    public interface IAlterTableColumnOptionBuilder : IColumnOptionBuilder<IAlterTableColumnOptionBuilder, IAlterTableColumnOptionForeignKeyCascadeBuilder>,
        IAlterTableBuilder, IExecutableBuilder
    { }
}
