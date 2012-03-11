using System.Web;
using System.Web.Mvc;

namespace _1CW_Site.ActionResults
{
    public class ImageResult : ActionResult
    {
        public byte[] ImageBytes { get; set; }
        public string ContentType { get; set; }

        public ImageResult(byte[] image, string contentType)
        {
            ImageBytes = image;
            ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = ContentType;

            if (ImageBytes == null) return;
            response.BinaryWrite(ImageBytes);
        }
    }
}