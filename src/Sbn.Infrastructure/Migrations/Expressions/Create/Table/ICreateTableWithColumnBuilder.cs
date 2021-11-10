using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Table
{
    public interface ICreateTableWithColumnBuilder : IFluentBuilder, IExecutableBuilder
    {
        ICreateTableColumnAsTypeBuilder WithColumn(string name);
    }
}
