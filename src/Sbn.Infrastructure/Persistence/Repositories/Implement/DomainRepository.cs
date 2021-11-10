using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using NPoco;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Querying;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Persistence.Repositories.Implement
{
    // TODO: We need to get a readonly ISO code for the domain assigned

    internal class DomainRepository : EntityRepositoryBase<int, IDomain>, IDomainRepository
    {
        public DomainRepository(IScopeAccessor scopeAccessor, AppCaches cache, ILogger<DomainRepository> logger)
            : base(scopeAccessor, cache, logger)
        { }

        protected override IRepositoryCachePolicy<IDomain, int> CreateCachePolicy()
        {
            return new FullDataSetRepositoryCachePolicy<IDomain, int>(GlobalIsolatedCache, ScopeAccessor, GetEntityId, /*expires:*/ false);
        }

        protected override IDomain PerformGet(int id)
        {
            //use the underlying GetAll which will force cache all domains
            return GetMany().FirstOrDefault(x => x.Id == id);
        }

        protected override IEnumerable<IDomain> PerformGetAll(params int[] ids)
        {
            var sql = GetBaseQuery(false).Where<DomainDto>(x => x.Id > 0);
            if (ids.Any())
            {
                sql.WhereIn<DomainDto>(x => x.Id, ids);
            }

            return Database.Fetch<DomainDto>(sql).Select(ConvertFromDto);
        }

        protected override IEnumerable<IDomain> PerformGetByQuery(IQuery<IDomain> query)
        {
            throw new NotSupportedException("This repository does not support this method");
        }

        protected override Sql<ISqlContext> GetBaseQuery(bool isCount)
        {
            var sql = Sql();
            if (isCount)
            {
                sql.SelectCount().From<DomainDto>();
            }
            else
            {
                sql.Select("sbnDomain.*, sbnLanguage.languageISOCode")
                    .From<DomainDto>()
                    .LeftJoin<LanguageDto>()
                    .On<DomainDto, LanguageDto>(dto => dto.DefaultLanguage, dto => dto.Id);
            }

            return sql;
        }

        protected override string GetBaseWhereClause()
        {
            return $"{Constants.DatabaseSchema.Tables.Domain}.id = @id";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            var list = new List<string>
                {
                    "DELETE FROM sbnDomain WHERE id = @id"
                };
            return list;
        }

        protected override void PersistNewItem(IDomain entity)
        {
            var exists = Database.ExecuteScalar<int>("SELECT COUNT(*) FROM sbnDomain WHERE domainName = @domainName", new { domainName = entity.DomainName });
            if (exists > 0) throw new DuplicateNameException(string.Format("The domain name {0} is already assigned", entity.DomainName));

            if (entity.RootContentId.HasValue)
            {
                var contentExists = Database.ExecuteScalar<int>($"SELECT COUNT(*) FROM {Cms.Core.Constants.DatabaseSchema.Tables.Content} WHERE nodeId = @id", new { id = entity.RootContentId.Value });
                if (contentExists == 0) throw new NullReferenceException("No content exists with id " + entity.RootContentId.Value);
            }

            if (entity.LanguageId.HasValue)
            {
                var languageExists = Database.ExecuteScalar<int>("SELECT COUNT(*) FROM sbnLanguage WHERE id = @id", new { id = entity.LanguageId.Value });
                if (languageExists == 0) throw new NullReferenceException("No language exists with id " + entity.LanguageId.Value);
            }

            entity.AddingEntity();

            var factory = new DomainModelFactory();
            var dto = factory.BuildDto(entity);

            var id = Convert.ToInt32(Database.Insert(dto));
            entity.Id = id;

            //if the language changed, we need to resolve the ISO code!
            if (entity.LanguageId.HasValue)
            {
                ((SbnDomain)entity).LanguageIsoCode = Database.ExecuteScalar<string>("SELECT languageISOCode FROM sbnLanguage WHERE id=@langId", new { langId = entity.LanguageId });
            }

            entity.ResetDirtyProperties();
        }

        protected override void PersistUpdatedItem(IDomain entity)
        {
            entity.UpdatingEntity();

            var exists = Database.ExecuteScalar<int>("SELECT COUNT(*) FROM sbnDomain WHERE domainName = @domainName AND sbnDomain.id <> @id",
                new { domainName = entity.DomainName, id = entity.Id });
            //ensure there is no other domain with the same name on another entity
            if (exists > 0) throw new DuplicateNameException(string.Format("The domain name {0} is already assigned", entity.DomainName));

            if (entity.RootContentId.HasValue)
            {
                var contentExists = Database.ExecuteScalar<int>($"SELECT COUNT(*) FROM {Cms.Core.Constants.DatabaseSchema.Tables.Content} WHERE nodeId = @id", new { id = entity.RootContentId.Value });
                if (contentExists == 0) throw new NullReferenceException("No content exists with id " + entity.RootContentId.Value);
            }

            if (entity.LanguageId.HasValue)
            {
                var languageExists = Database.ExecuteScalar<int>("SELECT COUNT(*) FROM sbnLanguage WHERE id = @id", new { id = entity.LanguageId.Value });
                if (languageExists == 0) throw new NullReferenceException("No language exists with id " + entity.LanguageId.Value);
            }

            var factory = new DomainModelFactory();
            var dto = factory.BuildDto(entity);

            Database.Update(dto);

            //if the language changed, we need to resolve the ISO code!
            if (entity.WasPropertyDirty("LanguageId"))
            {
                ((SbnDomain)entity).LanguageIsoCode = Database.ExecuteScalar<string>("SELECT languageISOCode FROM sbnLanguage WHERE id=@langId", new {langId = entity.LanguageId});
            }

            entity.ResetDirtyProperties();
        }

        public IDomain GetByName(string domainName)
        {
            return GetMany().FirstOrDefault(x => x.DomainName.InvariantEquals(domainName));
        }

        public bool Exists(string domainName)
        {
            return GetMany().Any(x => x.DomainName.InvariantEquals(domainName));
        }

        public IEnumerable<IDomain> GetAll(bool includeWildcards)
        {
            return GetMany().Where(x => includeWildcards || x.IsWildcard == false);
        }

        public IEnumerable<IDomain> GetAssignedDomains(int contentId, bool includeWildcards)
        {
            return GetMany()
                .Where(x => x.RootContentId == contentId)
                .Where(x => includeWildcards || x.IsWildcard == false);
        }

        private IDomain ConvertFromDto(DomainDto dto)
        {
            var factory = new DomainModelFactory();
            var entity = factory.BuildEntity(dto);
            return entity;
        }

        internal class DomainModelFactory
        {

            public IDomain BuildEntity(DomainDto dto)
            {
                var domain = new SbnDomain(dto.DomainName, dto.IsoCode)
                {
                    Id = dto.Id,
                    LanguageId = dto.DefaultLanguage,
                    RootContentId = dto.RootStructureId
                };
                // reset dirty initial properties (U4-1946)
                domain.ResetDirtyProperties(false);
                return domain;
            }

            public DomainDto BuildDto(IDomain entity)
            {
                var dto = new DomainDto { DefaultLanguage = entity.LanguageId, DomainName = entity.DomainName, Id = entity.Id, RootStructureId = entity.RootContentId };
                return dto;
            }
        }
    }
}
