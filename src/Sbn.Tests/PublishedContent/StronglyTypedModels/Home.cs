using System;
using System.Web;
using Sbn.Core.Models;

namespace Sbn.Tests.PublishedContent.StronglyTypedModels
{
    /// <summary>
    /// Used for testing strongly-typed published content extensions that work against the <see cref="PublishedContentTests"/>
    /// </summary>
    public class Home : TypedModelBase
    {
        public Home(IPublishedContent publishedContent) : base(publishedContent)
        {
        }
    }
}
