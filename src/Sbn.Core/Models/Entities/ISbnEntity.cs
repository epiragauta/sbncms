namespace Sbn.Cms.Core.Models.Entities
{

    /// <summary>
    /// Represents an entity that can be managed by the entity service.
    /// </summary>
    /// <remarks>
    /// <para>An ISbnEntity can be related to another via the IRelationService.</para>
    /// <para>ISbnEntities can be retrieved with the IEntityService.</para>
    /// <para>An ISbnEntity can participate in notifications.</para>
    /// </remarks>
    public interface ISbnEntity : ITreeEntity
    { }
}
