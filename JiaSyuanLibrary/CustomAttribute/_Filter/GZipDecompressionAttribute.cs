using System.IO;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ionic.Zlib;

namespace JiaSyuanLibrary.CustomAttribute
{
    public class GZipDecompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var content = actionContext.Request.Content;
            var zipContentBytes = content?.ReadAsByteArrayAsync().Result;
            var unzipContentBytes = zipContentBytes == null ? new byte[0] : Decompress(zipContentBytes);
            actionContext.Request.Content = new ByteArrayContent(unzipContentBytes);
            base.OnActionExecuting(actionContext);
        }

        private static byte[] Decompress(byte[] sourceBytes)
        {
            using (var compressedStream = new MemoryStream(sourceBytes))
            using (var zipStream = new GZipStream(compressedStream,
                CompressionMode.Decompress,
                CompressionLevel.BestSpeed,
                true))

            using (var outputStream = new MemoryStream())
            {
                zipStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
