using System;

namespace BPMClient
{
    /// <summary>
    /// BpmClientConfig
    /// </summary>
    public class BpmClientConfig
    {
        public string BaseUrl { get; }

        public string Login { get; }

        public string Password { get; }

        public BpmClientConfig(string baseUrl, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) throw new Exception("baseUrl is empty");
            if (string.IsNullOrWhiteSpace(login)) throw new Exception("login is empty");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("password is empty");

            BaseUrl = baseUrl;
            Login = login;
            Password = password;
        }
    }
}
