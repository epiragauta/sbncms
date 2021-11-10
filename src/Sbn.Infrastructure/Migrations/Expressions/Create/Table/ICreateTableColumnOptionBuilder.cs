using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Table
{
    public interface ICreateTableColumnOptionBuilder :
        IColumnOptionBuilder<ICreateTableColumnOptionBuilder, ICreateTableColumnOptionForeignKeyCascadeBuilder>,
        ICreateTableWithColumnBuilder
    { }
}
