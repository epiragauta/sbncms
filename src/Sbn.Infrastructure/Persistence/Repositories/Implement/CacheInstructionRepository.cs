using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Cms.Infrastructure.Persistence.Factories;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Persistence.Repositories.Implement
{
    /// <summary>
    /// Represents the NPoco implementation of <see cref="ICacheInstructionRepository"/>.
    /// </summary>
    internal class CacheInstructionRepository : ICacheInstructionRepository
    {
        private readonly IScopeAccessor _scopeAccessor;

        public CacheInstructionRepository(IScopeAccessor scopeAccessor) => _scopeAccessor = scopeAccessor;

        /// <inheritdoc/>
        private IScope AmbientScope => _scopeAccessor.AmbientScope;

        /// <inheritdoc/>
        public int CountAll()
        {
            Sql<ISqlContext> sql = AmbientScope.SqlContext.Sql().Select("COUNT(*)")
                .From<CacheInstructionDto>();

            return AmbientScope.Database.ExecuteScalar<int>(sql);
        }

        /// <inheritdoc/>
        public int CountPendingInstructions(int lastId) =>
            AmbientScope.Database.ExecuteScalar<int>("SELECT SUM(instructionCount) FROM sbnCacheInstruction WHERE id > @lastId", new { lastId });

        /// <inheritdoc/>
        public int GetMaxId() =>
            AmbientScope.Database.ExecuteScalar<int>("SELECT MAX(id) FROM sbnCacheInstruction");

        /// <inheritdoc/>
        public bool Exists(int id) => AmbientScope.Database.Exists<CacheInstructionDto>(id);

        /// <inheritdoc/>
        public void Add(CacheInstruction cacheInstruction)
        {
            CacheInstructionDto dto = CacheInstructionFactory.BuildDto(cacheInstruction);
            AmbientScope.Database.Insert(dto);
        }

        /// <inheritdoc/>
        public IEnumerable<CacheInstruction> GetPendingInstructions(int lastId, int maxNumberToRetrieve)
        {
            Sql<ISqlContext> sql = AmbientScope.SqlContext.Sql().SelectAll()
                .From<CacheInstructionDto>()
                .Where<CacheInstructionDto>(dto => dto.Id > lastId)
                .OrderBy<CacheInstructionDto>(dto => dto.Id);
            Sql<ISqlContext> topSql = sql.SelectTop(maxNumberToRetrieve);
            return AmbientScope.Database.Fetch<CacheInstructionDto>(topSql).Select(CacheInstructionFactory.BuildEntity);
        }

        /// <inheritdoc/>
        public void DeleteInstructionsOlderThan(DateTime pruneDate)
        {
            // Using 2 queries is faster than convoluted joins.
            var maxId = AmbientScope.Database.ExecuteScalar<int>("SELECT MAX(id) FROM sbnCacheInstruction;");
            Sql deleteSql = new Sql().Append(@"DELETE FROM sbnCacheInstruction WHERE utcStamp < @pruneDate AND id < @maxId", new { pruneDate, maxId });
            AmbientScope.Database.Execute(deleteSql);
        }
    }
}
