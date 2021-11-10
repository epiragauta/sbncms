using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Media;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Web.Common.ActionsResults;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Cms.Web.Common.Authorization;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    [Authorize(Policy = AuthorizationPolicies.SectionAccessForTinyMce)]
    public class TinyMceController : SbnAuthorizedApiController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly ContentSettings _contentSettings;
        private readonly IIOHelper _ioHelper;
        private readonly IImageUrlGenerator _imageUrlGenerator;

        public TinyMceController(
            IHostingEnvironment hostingEnvironment,
            IShortStringHelper shortStringHelper,
            IOptions<ContentSettings> contentSettings,
            IIOHelper ioHelper,
            IImageUrlGenerator imageUrlGenerator)
        {
            _hostingEnvironment = hostingEnvironment;
            _shortStringHelper = shortStringHelper;
            _contentSettings = contentSettings.Value;
            _ioHelper = ioHelper;
            _imageUrlGenerator = imageUrlGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> file)
        {
            // Create an unique folder path to help with concurrent users to avoid filename clash
            var imageTempPath = _hostingEnvironment.MapPathWebRoot(Constants.SystemDirectories.TempImageUploads + "/" + Guid.NewGuid().ToString());

            // Ensure image temp path exists
            if(Directory.Exists(imageTempPath) == false)
            {
                Directory.CreateDirectory(imageTempPath);
            }

            // Must have a file
            if (file.Count == 0)
            {
                return NotFound();
            }

            // Should only have one file
            if (file.Count > 1)
            {
                return new SbnProblemResult("Only one file can be uploaded at a time", HttpStatusCode.BadRequest);
            }

            var formFile = file.First();

            // Really we should only have one file per request to this endpoint
            //  var file = result.FileData[0];
            var fileName = formFile.FileName.Trim(new[] { '\"' }).TrimEnd();
            var safeFileName = fileName.ToSafeFileName(_shortStringHelper);
            var ext = safeFileName.Substring(safeFileName.LastIndexOf('.') + 1).ToLower();

            if (_contentSettings.IsFileAllowedForUpload(ext) == false || _imageUrlGenerator.SupportedImageFileTypes.Contains(ext) == false)
            {
                // Throw some error - to say can't upload this IMG type
                return new SbnProblemResult("This is not an image filetype extension that is approved", HttpStatusCode.BadRequest);
            }

            var newFilePath = imageTempPath +  Path.DirectorySeparatorChar + safeFileName;
            var relativeNewFilePath = _ioHelper.GetRelativePath(newFilePath);

            await using (var stream = System.IO.File.Create(newFilePath))
            {
                await formFile.CopyToAsync(stream);
            }

            return Ok(new { tmpLocation = relativeNewFilePath });

        }
    }
}
