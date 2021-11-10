using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.TestData.Extensions;

// see https://github.com/Shazwazza/SbnScripts/tree/master/src/LoadTesting

namespace Sbn.TestData
{
    public class LoadTestComposer : IComposer
    {
        public void Compose(ISbnBuilder builder) => builder.AddSbnTestData();
    }
}
