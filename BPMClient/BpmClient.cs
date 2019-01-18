using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BPMClient
{
    public class BpmClient : IBpmClient
    {
        private readonly BpmClientConfig _config;

        private CookieContainer AuthCookie = new CookieContainer();

        public CookieCollection Cookie { get; protected set; }

        private readonly bool _useCSRF = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BpmClient"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="useCSRF">if set to <c>true</c> [use CSRF].</param>
        /// <exception cref="System.ArgumentNullException">config</exception>
        public BpmClient(BpmClientConfig config, bool useCSRF = false)
        {
            _config = config ?? throw new ArgumentNullException("config");
            _useCSRF = useCSRF;
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            ResponseStatus status = null;
            var url = _config.BaseUrl + @"/ServiceModel/AuthService.svc/Login";
            var authRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            authRequest.Method = "POST";
            authRequest.ContentType = "application/json";
            authRequest.CookieContainer = AuthCookie;

            using (var requestStream = authRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(@"{ ""UserName"":""" + _config.Login + @""", ""UserPassword"":""" + _config.Password + @""" }");
                }
            }

            using (var response = (HttpWebResponse)authRequest.GetResponse())
            {
                Cookie = response.Cookies;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    status = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ResponseStatus>(responseText);
                }
            }

            if (status?.Code == 0) return true;
            return false;
        }

        /// <summary>
        /// Starts the sevice get.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// service is undefined
        /// or
        /// method is undefined
        /// </exception>
        public string StartSeviceGet(string service, string method, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(service)) throw new Exception("service is undefined");
            if (string.IsNullOrWhiteSpace(method)) throw new Exception("method is undefined");
            if (!Login()) return "Login error";

            var paramString = string.Empty;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    paramString += "&" + param.Key + "=" + param.Value;
                }
            }

            if (paramString.Length > 0) paramString = "?" + paramString.Substring(1);

            var url = _config.BaseUrl + $"/0/rest/{service}/{method}{paramString}";
            return MakeGetRequest(url);
        }

        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="resultParameterName">Name of the result parameter.</param>
        /// <returns></returns>
        /// <exception cref="Exception">processName is undefined</exception>
        public string StartProcess(string processName, Dictionary<string, object> parameters = null, string resultParameterName = null)
        {
            if (string.IsNullOrWhiteSpace(processName)) throw new Exception("processName is undefined");
            if (!Login()) return "Login error";

            var paramString = string.Empty;
            if (!string.IsNullOrWhiteSpace(resultParameterName))
                paramString += "?ResultParameterName=" + resultParameterName;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    paramString += "&" + param.Key + "=" + param.Value;
                }
            }

            if (string.IsNullOrWhiteSpace(resultParameterName) && paramString.Length > 0)
                paramString = "?" + paramString.Substring(1);

            var url = _config.BaseUrl + $"/0/ServiceModel/ProcessEngineService.svc/{processName}/Execute{paramString}";
            return MakeGetRequest(url);
        }

        /// <summary>
        /// Makes the get request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string MakeGetRequest(string url)
        {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.CookieContainer = AuthCookie;
            if (_useCSRF)
            {
                string csrfToken = Cookie["BPMCSRF"]?.Value;
                request.Headers.Add("BPMCSRF", csrfToken);
            }
            using (var response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

    }
}
