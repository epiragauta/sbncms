// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Actions;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Core.PropertyEditors;

namespace Sbn.Extensions
{
    public static class TypeLoaderExtensions
    {
        /// <summary>
        /// Gets all types implementing <see cref="IDataEditor"/>.
        /// </summary>
        public static IEnumerable<Type> GetDataEditors(this TypeLoader mgr) => mgr.GetTypes<IDataEditor>();

        /// <summary>
        /// Gets all types implementing ICacheRefresher.
        /// </summary>
        public static IEnumerable<Type> GetCacheRefreshers(this TypeLoader mgr) => mgr.GetTypes<ICacheRefresher>();

        /// <summary>
        /// Gets all types implementing <see cref="IAction"/>
        /// </summary>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetActions(this TypeLoader mgr) => mgr.GetTypes<IAction>();
    }
}
