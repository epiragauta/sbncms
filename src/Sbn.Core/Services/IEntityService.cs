using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Entities;
using Sbn.Cms.Core.Persistence.Querying;

namespace Sbn.Cms.Core.Services
{
    public interface IEntityService
    {
        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        IEntitySlim Get(int id);

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="key">The unique key of the entity.</param>
        IEntitySlim Get(Guid key);

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="objectType">The object type of the entity.</param>
        IEntitySlim Get(int id, SbnObjectTypes objectType);

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <param name="key">The unique key of the entity.</param>
        /// <param name="objectType">The object type of the entity.</param>
        IEntitySlim Get(Guid key, SbnObjectTypes objectType);

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <typeparam name="T">The type used to determine the object type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity.</param>
        IEntitySlim Get<T>(int id) where T : ISbnEntity;

        /// <summary>
        /// Gets an entity.
        /// </summary>
        /// <typeparam name="T">The type used to determine the object type of the entity.</typeparam>
        /// <param name="key">The unique key of the entity.</param>
        IEntitySlim Get<T>(Guid key) where T : ISbnEntity;

        /// <summary>
        /// Determines whether an entity exists.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        bool Exists(int id);

        /// <summary>
        /// Determines whether an entity exists.
        /// </summary>
        /// <param name="key">The unique key of the entity.</param>
        bool Exists(Guid key);

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <typeparam name="T">The type used to determine the object type of the entities.</typeparam>
        IEnumerable<IEntitySlim> GetAll<T>() where T : ISbnEntity;

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <typeparam name="T">The type used to determine the object type of the entities.</typeparam>
        /// <param name="ids">The identifiers of the entities.</param>
        /// <remarks>If <paramref name="ids"/> is empty, returns all entities.</remarks>
        IEnumerable<IEntitySlim> GetAll<T>(params int[] ids) where T : ISbnEntity;

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        IEnumerable<IEntitySlim> GetAll(SbnObjectTypes objectType);

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        /// <param name="ids">The identifiers of the entities.</param>
        /// <remarks>If <paramref name="ids"/> is empty, returns all entities.</remarks>
        IEnumerable<IEntitySlim> GetAll(SbnObjectTypes objectType, params int[] ids);

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        IEnumerable<IEntitySlim> GetAll(Guid objectType);

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        /// <param name="ids">The identifiers of the entities.</param>
        /// <remarks>If <paramref name="ids"/> is empty, returns all entities.</remarks>
        IEnumerable<IEntitySlim> GetAll(Guid objectType, params int[] ids);

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <typeparam name="T">The type used to determine the object type of the entities.</typeparam>
        /// <param name="keys">The unique identifiers of the entities.</param>
        /// <remarks>If <paramref name="keys"/> is empty, returns all entities.</remarks>
        IEnumerable<IEntitySlim> GetAll<T>(params Guid[] keys) where T : ISbnEntity;

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        /// <param name="keys">The unique identifiers of the entities.</param>
        /// <remarks>If <paramref name="keys"/> is empty, returns all entities.</remarks>
        IEnumerable<IEntitySlim> GetAll(SbnObjectTypes objectType, Guid[] keys);

        /// <summary>
        /// Gets entities of a given object type.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        /// <param name="keys">The unique identifiers of the entities.</param>
        /// <remarks>If <paramref name="keys"/> is empty, returns all entities.</remarks>
        IEnumerable<IEntitySlim> GetAll(Guid objectType, params Guid[] keys);

        /// <summary>
        /// Gets entities at root.
        /// </summary>
        /// <param name="objectType">The object type of the entities.</param>
        IEnumerable<IEntitySlim> GetRootEntities(SbnObjectTypes objectType);

        /// <summary>
        /// Gets the parent of an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        IEntitySlim GetParent(int id);

        /// <summary>
        /// Gets the parent of an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="objectType">The object type of the parent.</param>
        IEntitySlim GetParent(int id, SbnObjectTypes objectType);

        /// <summary>
        /// Gets the children of an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        IEnumerable<IEntitySlim> GetChildren(int id);

        /// <summary>
        /// Gets the children of an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="objectType">The object type of the children.</param>
        IEnumerable<IEntitySlim> GetChildren(int id, SbnObjectTypes objectType);

        /// <summary>
        /// Gets the descendants of an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        IEnumerable<IEntitySlim> GetDescendants(int id);

        /// <summary>
        /// Gets the descendants of an entity.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="objectType">The object type of the descendants.</param>
        IEnumerable<IEntitySlim> GetDescendants(int id, SbnObjectTypes objectType);

        /// <summary>
        /// Gets children of an entity.
        /// </summary>
        IEnumerable<IEntitySlim> GetPagedChildren(int id, SbnObjectTypes objectType, long pageIndex, int pageSize, out long totalRecords,
            IQuery<ISbnEntity> filter = null, Ordering ordering = null);

        /// <summary>
        /// Gets descendants of an entity.
        /// </summary>
        IEnumerable<IEntitySlim> GetPagedDescendants(int id, SbnObjectTypes objectType, long pageIndex, int pageSize, out long totalRecords,
            IQuery<ISbnEntity> filter = null, Ordering ordering = null);

        /// <summary>
        /// Gets descendants of entities.
        /// </summary>
        IEnumerable<IEntitySlim> GetPagedDescendants(IEnumerable<int> ids, SbnObjectTypes objectType, long pageIndex, int pageSize, out long totalRecords,
            IQuery<ISbnEntity> filter = null, Ordering ordering = null);

        // TODO: Do we really need this? why not just pass in -1
        /// <summary>
        /// Gets descendants of root.
        /// </summary>
        IEnumerable<IEntitySlim> GetPagedDescendants(SbnObjectTypes objectType, long pageIndex, int pageSize, out long totalRecords,
            IQuery<ISbnEntity> filter = null, Ordering ordering = null, bool includeTrashed = true);

        /// <summary>
        /// Gets the object type of an entity.
        /// </summary>
        SbnObjectTypes GetObjectType(int id);

        /// <summary>
        /// Gets the object type of an entity.
        /// </summary>
        SbnObjectTypes GetObjectType(Guid key);

        /// <summary>
        /// Gets the object type of an entity.
        /// </summary>
        SbnObjectTypes GetObjectType(ISbnEntity entity);

        /// <summary>
        /// Gets the CLR type of an entity.
        /// </summary>
        Type GetEntityType(int id);

        /// <summary>
        /// Gets the integer identifier corresponding to a unique Guid identifier.
        /// </summary>
        Attempt<int> GetId(Guid key, SbnObjectTypes objectType);

        /// <summary>
        /// Gets the integer identifier corresponding to a Udi.
        /// </summary>
        Attempt<int> GetId(Udi udi);

        /// <summary>
        /// Gets the unique Guid identifier corresponding to an integer identifier.
        /// </summary>
        Attempt<Guid> GetKey(int id, SbnObjectTypes sbnObjectType);

        /// <summary>
        /// Gets paths for entities.
        /// </summary>
        IEnumerable<TreeEntityPath> GetAllPaths(SbnObjectTypes objectType, params int[] ids);

        /// <summary>
        /// Gets paths for entities.
        /// </summary>
        IEnumerable<TreeEntityPath> GetAllPaths(SbnObjectTypes objectType, params Guid[] keys);

        /// <summary>
        /// Reserves an identifier for a key.
        /// </summary>
        /// <param name="key">They key.</param>
        /// <returns>The identifier.</returns>
        /// <remarks>When a new content or a media is saved with the key, it will have the reserved identifier.</remarks>
        int ReserveId(Guid key);
    }
}
