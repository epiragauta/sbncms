using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Alter.Table
{
    public interface IAlterTableColumnOptionForeignKeyCascadeBuilder :
        IAlterTableColumnOptionBuilder,
        IForeignKeyCascadeBuilder<IAlterTableColumnOptionBuilder, IAlterTableColumnOptionForeignKeyCascadeBuilder>
    { }
}
