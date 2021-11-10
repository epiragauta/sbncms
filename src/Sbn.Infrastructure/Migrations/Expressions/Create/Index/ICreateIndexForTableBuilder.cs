namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Index
{
    public interface ICreateIndexForTableBuilder : IFluentBuilder
    {
        ICreateIndexOnColumnBuilder OnTable(string tableName);
    }
}
