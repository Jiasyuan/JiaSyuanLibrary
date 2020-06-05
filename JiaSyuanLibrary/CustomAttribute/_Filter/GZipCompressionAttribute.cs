using System.Net.Http;
using System.Web.Http.Filters;
using System.IO;
using Ionic.Zlib;

namespace JiaSyuanLibrary.CustomAttribute
{

    public class GZipCompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            var content = actContext.Response.Content;
            var bytes = content?.ReadAsByteArrayAsync().Result;
            var zipContent = bytes == null ? new byte[0] : Compress(bytes);
            actContext.Response.Content = new ByteArrayContent(zipContent);
            actContext.Response.Content.Headers.Remove("Content-Type");
            actContext.Response.Content.Headers.Add("Content-encoding", "gzip");

            //actContext.Response.Content.Headers.Add("Content-Type",     "application/json");
            actContext.Response.Content.Headers.Add("Content-Type", "application/json;charset=utf-8");
            base.OnActionExecuted(actContext);
        }

        private static byte[] Compress(byte[] sourceBytes)
        {
            if (sourceBytes == null)
            {
                return null;
            }

            using (var outputStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(outputStream,
                    CompressionMode.Compress,
                    CompressionLevel.BestSpeed,
                    true))
                {
                    zipStream.Write(sourceBytes, 0, sourceBytes.Length);
                }

                return outputStream.ToArray();
            }
        }
    }
}
