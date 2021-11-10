using System.Data;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.ForeignKey
{
    public interface ICreateForeignKeyCascadeBuilder : IFluentBuilder, IExecutableBuilder
    {
        ICreateForeignKeyCascadeBuilder OnDelete(Rule rule);
        ICreateForeignKeyCascadeBuilder OnUpdate(Rule rule);
        IExecutableBuilder OnDeleteOrUpdate(Rule rule);
    }
}
