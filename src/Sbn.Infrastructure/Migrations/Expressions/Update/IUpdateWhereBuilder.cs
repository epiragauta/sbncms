using Sbn.Cms.Infrastructure.Migrations.Expressions.Common;

namespace Sbn.Cms.Infrastructure.Migrations.Expressions.Update
{
    /// <summary>
    /// Builds an Update expression.
    /// </summary>
    public interface IUpdateWhereBuilder
    {
        /// <summary>
        /// Specifies rows to update.
        /// </summary>
        IExecutableBuilder Where(object dataAsAnonymousType);

        /// <summary>
        /// Specifies that all rows must be updated.
        /// </summary>
        IExecutableBuilder AllRows();
    }
}
