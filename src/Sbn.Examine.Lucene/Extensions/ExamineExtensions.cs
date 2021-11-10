// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Linq;
using System.Threading;
using Examine;
using Examine.Lucene.Providers;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Infrastructure.Examine;

namespace Sbn.Extensions
{
    /// <summary>
    /// Extension methods for the LuceneIndex
    /// </summary>
    public static class ExamineExtensions
    {
        internal static bool TryParseLuceneQuery(string query)
        {
            // TODO: I'd assume there would be a more strict way to parse the query but not that i can find yet, for now we'll
            // also do this rudimentary check
            if (!query.Contains(":"))
            {
                return false;
            }

            try
            {
                //This will pass with a plain old string without any fields, need to figure out a way to have it properly parse
                var parsed = new QueryParser(LuceneInfo.CurrentVersion, SbnExamineFieldNames.NodeNameFieldName, new KeywordAnalyzer()).Parse(query);
                return true;
            }
            catch (ParseException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the index can be read/opened
        /// </summary>
        /// <param name="indexer"></param>
        /// <param name="ex">The exception returned if there was an error</param>
        /// <returns></returns>
        public static bool IsHealthy(this LuceneIndex indexer, out Exception ex)
        {
            try
            {
                using (indexer.IndexWriter.IndexWriter.GetReader(false))
                {
                    ex = null;
                    return true;
                }
            }
            catch (Exception e)
            {
                ex = e;
                return false;
            }
        }

    }
}
