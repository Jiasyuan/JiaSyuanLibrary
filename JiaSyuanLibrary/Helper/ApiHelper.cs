using JiaSyuanLibrary.Enums;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace JiaSyuanLibrary.Helper
{
    public static class ApiHelper
    {
        private static string GetApiContentType(EnumContentType contentType)
        {
            string result;
            switch (contentType)
            {
                case EnumContentType.json:
                    result = ConstSetting.ContentTypeJson;
                    break;
                case EnumContentType.formurlencoded:
                    result = ConstSetting.ContentTypeform;
                    break;
                default:
                    throw new Exception("Unknown ContentType.");
            }

            return result;
        }

        public static string SerializeToJson<T>(this T objectToSerialize)
        {
            return JsonSerializer.Serialize(objectToSerialize);
        }

        public static T DeserializeJson<T>(string fromJsonString)
        {
            return (T)JsonSerializer.Deserialize(fromJsonString, typeof(T));
        }

        private static string CombinePath(string serverBasePath, string callPath)
        {
            if (serverBasePath.Length > 1 && serverBasePath.EndsWith("/")) { serverBasePath = serverBasePath.Substring(0, serverBasePath.Length - 1); }
            if (callPath.Length > 1 && callPath.StartsWith("/")) { callPath = callPath.Substring(1); }
            return serverBasePath + "/" + callPath;
        }

        public static T PostApi<T>(string apiServer, string controllerName, string actionName, object parameter)
        {
            return PostApi<T>(apiServer, EnumContentType.json, controllerName, actionName, null, parameter);
        }

        public static T PostApi<T>(string apiServer, EnumContentType contentType, string controllerName, string actionName, string getParam, object parameter, bool isJson = true)
        {
            return PostApi<T>(apiServer, contentType, EnumApiMethodType.Post, controllerName, actionName, getParam, parameter, isJson);
        }

        public static T PostApi<T>(string apiServer, EnumContentType contentType, EnumApiMethodType apiMethodType, string controllerName, string actionName, string getParam, object parameter, bool isJson = true)
        {
            return Api<T>(apiServer, contentType, EnumApiMethodType.Post, controllerName + "/" + actionName, getParam, parameter, isJson);
        }

        public static T GetApi<T>(string apiServer, string controllerName, string actionName, bool isJson = true)
        {
            return Api<T>(apiServer, EnumContentType.json, EnumApiMethodType.Get, controllerName + "/" + actionName, null, null, isJson);
        }

        public static T GetApi<T>(string apiServer, string controllerName, string actionName, string getParam, bool isJson = true)
        {
            return Api<T>(apiServer, EnumContentType.json, EnumApiMethodType.Get, controllerName + "/" + actionName, getParam, null, isJson);
        }

        public static T Api<T>(string apiServer, EnumContentType contentType, EnumApiMethodType apiMethodType, string methodName, string getParam, object parameter, bool isJson = true)
        {
            // 如果有getParam，確保有加"?"
            if (!string.IsNullOrWhiteSpace(getParam) && !getParam.StartsWith("?"))
            {
                getParam = "?" + getParam.Trim();
            }
            else if (string.IsNullOrWhiteSpace(getParam))
            {
                getParam = default;
            }

            // 整理呼叫的url
            string apiUrl = CombinePath(apiServer, methodName) + getParam;

            HttpWebRequest request = HttpWebRequest.Create(apiUrl) as HttpWebRequest;
            string postTypeStr =string.Empty;
            switch (apiMethodType)
            {
                case EnumApiMethodType.Post:
                    postTypeStr = WebRequestMethods.Http.Post;
                    break;
                case EnumApiMethodType.Get:
                    postTypeStr = WebRequestMethods.Http.Get;
                    break;
                case EnumApiMethodType.Put:
                    postTypeStr = WebRequestMethods.Http.Put;
                    break;
                default:
                    break;
            }
            request.Method = postTypeStr; // 方法
            request.KeepAlive = true; //是否保持連線
            request.ContentType = GetApiContentType(contentType);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };//for https
            // 讓這個request最多等10分鐘
            request.Timeout = 600000;
            request.MaximumResponseHeadersLength = int.MaxValue;
            request.MaximumAutomaticRedirections = int.MaxValue;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            try
            {
                // 整理成呼叫的body paramter
                if (apiMethodType != EnumApiMethodType.Get)
                {
                    string jsonParameterString = SerializeToJson<object>(parameter);
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes(jsonParameterString);
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                string jsonResult = default;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var temp = reader.ReadToEnd();
                            jsonResult = temp;
                            //TODO:反序列化
                            if (string.IsNullOrEmpty(temp))
                            {
                                return default(T);
                            }
                            else
                            {
                                T result = default(T);
                                if (isJson)
                                {
                                    result = DeserializeJson<T>(jsonResult);
                                }
                                else
                                {
                                    result = (T)Convert.ChangeType(temp, typeof(T));
                                }
                                return result;
                            }
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw new Exception("服務無回應", webException);
                }
                using (StreamReader reader = new StreamReader(webException.Response.GetResponseStream()))
                {
                    HttpWebResponse res = (HttpWebResponse)webException.Response;
                    var pageContent = reader.ReadToEnd();
                    T result = JsonSerializer.Deserialize<T>(pageContent);
                    return result;
                }
            }
            catch (Exception)
            {
                throw ;
            }
        }
    }

    /// <summary>
    /// Const Setting
    /// </summary>
    internal static class ConstSetting
    {
        public static string ContentTypeJson => "application/json";
        public static string ContentTypeform => "application/x-www-form-urlencoded";
    }
}
