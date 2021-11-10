using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Column
{
    public interface ICreateColumnOptionBuilder : IColumnOptionBuilder<ICreateColumnOptionBuilder, ICreateColumnOptionForeignKeyCascadeBuilder>
        , IExecutableBuilder
    { }
}
