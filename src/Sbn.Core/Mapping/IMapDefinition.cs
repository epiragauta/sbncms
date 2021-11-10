namespace Sbn.Cms.Core.Mapping
{
    /// <summary>
    /// Defines maps for <see cref="SbnMapper"/>.
    /// </summary>
    public interface IMapDefinition
    {
        /// <summary>
        /// Defines maps.
        /// </summary>
        void DefineMaps(ISbnMapper mapper);
    }
}
