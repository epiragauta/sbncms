namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.ForeignKey
{
    public interface ICreateForeignKeyFromTableBuilder : IFluentBuilder
    {
        ICreateForeignKeyForeignColumnBuilder FromTable(string table);
    }
}
