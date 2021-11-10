using System.Collections.Generic;

namespace Sbn.Cms.Core.Configuration.SbnSettings
{
    public interface ITypeFinderConfig
    {
        IEnumerable<string> AssembliesAcceptingLoadExceptions { get; }
    }
}
