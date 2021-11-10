namespace Sbn.Cms.Core.Web
{
    /// <summary>
    /// Provides access to a TryGetSbnContext bool method that will return true if the "current" <see cref="ISbnContext"/> is not null.
    /// Provides a Clear() method that will clear the current <see cref="SbnContext"/> object.
    /// Provides a Set() method that til set the current <see cref="SbnContext"/> object.
    /// </summary>
    public interface ISbnContextAccessor
    {
        bool TryGetSbnContext(out ISbnContext sbnContext);
        void Clear();
        void Set(ISbnContext sbnContext);
    }
}
