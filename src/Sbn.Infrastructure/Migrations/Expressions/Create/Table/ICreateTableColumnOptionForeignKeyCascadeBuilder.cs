using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Table
{
    public interface ICreateTableColumnOptionForeignKeyCascadeBuilder :
        ICreateTableColumnOptionBuilder,
        IForeignKeyCascadeBuilder<ICreateTableColumnOptionBuilder, ICreateTableColumnOptionForeignKeyCascadeBuilder>
    { }
}
