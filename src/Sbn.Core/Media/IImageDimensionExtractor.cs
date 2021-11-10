using System.Drawing;
using System.IO;

namespace Sbn.Cms.Core.Media
{
    public interface IImageDimensionExtractor
    {
        public Size? GetDimensions(Stream stream);
    }
}
