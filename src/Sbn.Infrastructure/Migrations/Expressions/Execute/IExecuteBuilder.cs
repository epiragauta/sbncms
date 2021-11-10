using NPoco;
using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;
using Sbn.Cms.Infrastructure.Persistence;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Execute
{
    /// <summary>
    /// Builds and executes an Sql statement.
    /// </summary>
    /// <remarks>Deals with multi-statements Sql.</remarks>
    public interface IExecuteBuilder : IFluentBuilder
    {
        /// <summary>
        /// Specifies the Sql statement to execute.
        /// </summary>
        IExecutableBuilder Sql(string sqlStatement);

        /// <summary>
        /// Specifies the Sql statement to execute.
        /// </summary>
        IExecutableBuilder Sql(Sql<ISqlContext> sql);
    }
}
