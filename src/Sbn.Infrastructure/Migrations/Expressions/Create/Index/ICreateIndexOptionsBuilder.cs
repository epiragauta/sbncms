namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Create.Index
{
    public interface ICreateIndexOptionsBuilder : IFluentBuilder
    {
        ICreateIndexOnColumnBuilder Unique();
        ICreateIndexOnColumnBuilder NonClustered();
        ICreateIndexOnColumnBuilder Clustered();
    }
}
