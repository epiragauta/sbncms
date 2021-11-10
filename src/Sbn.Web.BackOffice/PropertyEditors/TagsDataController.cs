using System;
using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.PropertyEditors
{
    /// <summary>
    /// A controller used for type-ahead values for tags
    /// </summary>
    /// <remarks>
    /// DO NOT inherit from SbnAuthorizedJsonController since we don't want to use the angularized
    /// json formatter as it causes problems.
    /// </remarks>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    public class TagsDataController : SbnAuthorizedApiController
    {
        private readonly ITagQuery _tagQuery;

        public TagsDataController(ITagQuery tagQuery)
        {
            _tagQuery = tagQuery ?? throw new ArgumentNullException(nameof(tagQuery));
        }
        /// <summary>
        /// Returns all tags matching tagGroup, culture and an optional query
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <param name="culture"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TagModel> GetTags(string tagGroup, string culture, string query = null)
        {
            if (culture == string.Empty) culture = null;

            var result = _tagQuery.GetAllTags(tagGroup, culture);


            if (!query.IsNullOrWhiteSpace())
            {
                //TODO: add the query to TagQuery + the tag service, this is ugly but all we can do for now.
                //currently we are post filtering this :( but works for now
                result = result.Where(x => x.Text.InvariantContains(query));
            }

            return result.OrderBy(x => x.Text);
        }
    }
}
