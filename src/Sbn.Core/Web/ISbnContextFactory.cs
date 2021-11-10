
namespace Sbn.Cms.Core.Web
{
    /// <summary>
    /// Creates and manages <see cref="ISbnContext"/> instances.
    /// </summary>
    public interface ISbnContextFactory
    {
        /// <summary>
        /// Ensures that a current <see cref="ISbnContext"/> exists.
        /// </summary>
        /// <remarks>
        /// <para>If an <see cref="ISbnContext"/> is already registered in the
        /// <see cref="ISbnContextAccessor"/>, returns a non-root reference to it.
        /// Otherwise, create a new instance, registers it, and return a root reference
        /// to it.</para>
        /// <para>If <paramref name="httpContext"/> is null, the factory tries to use
        /// <see cref="HttpContext.Current"/> if it exists. Otherwise, it uses a dummy
        /// <see cref="HttpContextBase"/>.</para>
        /// </remarks>
        /// <example>
        /// using (var contextReference = contextFactory.EnsureSbnContext())
        /// {
        ///   var sbnContext = contextReference.SbnContext;
        ///   // use sbnContext...
        /// }
        /// </example>
        SbnContextReference EnsureSbnContext();
    }
}
