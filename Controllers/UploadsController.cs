using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace TSApi.Controllers
{
    public class UploadsController : ApiController
    {
        private const string _LogoSubFolder = "Logos";


        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/Uploads/InvoiceLogo")]
        public HttpResponseMessage UploadLogo()
        {

            
           
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Unsupported media type");
            }

            try
            {
                
                var photos = Task.Run(() => Add(Request));
                return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Photos uploaded ok", Pictures = photos });
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


        public async Task<IEnumerable<PhotoViewModel>> Add(HttpRequestMessage request)
        {

            string workingFolder = ConfigurationManager.AppSettings["uploadsFolder"] + "/" + _LogoSubFolder;
            if (string.IsNullOrEmpty(workingFolder))
            {
                throw new Exception("Config parameter [uploadsFolder] not set.");
            }
            string uploadsUrl = ConfigurationManager.AppSettings["uploadsUrl"];
            if (string.IsNullOrEmpty(uploadsUrl))
            {
                throw new Exception("Config parameter [uploadsUrl] not set.");
            }
            var provider = new PhotoMultipartFormDataStreamProvider(workingFolder);

            await this.Request.Content.ReadAsMultipartAsync(provider);

            var photos = new List<PhotoViewModel>();

            foreach (var file in provider.FileData)
            {
                var fileInfo = new FileInfo(file.LocalFileName);



                photos.Add(new PhotoViewModel
                {
                    Name = fileInfo.Name,
                    Created = fileInfo.CreationTime,
                    Modified = fileInfo.LastWriteTime,
                    Size = fileInfo.Length / 1024,
                    Url = uploadsUrl + "/" + _LogoSubFolder + "/" + fileInfo.Name
                });
            }

            return photos;
        }


    }


    public class PhotoMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {

        public PhotoMultipartFormDataStreamProvider(string path) : base(path)
        {
        }


        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            // restrict what images can be selected
            var extensions = new[] { "png", "gif", "jpg" };
            var filename = headers.ContentDisposition.FileName.Replace("\"", string.Empty);

            if (filename.IndexOf('.') < 0)
                return Stream.Null;

            var extension = filename.Split('.').Last();

            return extensions.Any(i => i.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                       ? base.GetStream(parent, headers)
                       : Stream.Null;

        }


        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            //Make the file name URL safe and then use it & is the only disallowed url character allowed in a windows filename 
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            name = name.Trim(new char[] { '"' })
                        .Replace("&", "and");

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(name);

            return newFileName;
        }
    }

    public class PhotoViewModel
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public long Size { get; set; }

        public string Url { get; set; }

    }

}
