using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Column
{
    public interface ICreateColumnOptionForeignKeyCascadeBuilder : ICreateColumnOptionBuilder,
        IForeignKeyCascadeBuilder<ICreateColumnOptionBuilder, ICreateColumnOptionForeignKeyCascadeBuilder>
    { }
}
