using System.Collections.Generic;

namespace BPMClient
{
    /// <summary>
    /// IBpmClient
    /// </summary>
    public interface IBpmClient
    {
        /// <summary>
        /// Login.
        /// </summary>
        /// <returns></returns>
        bool Login();

        /// <summary>
        /// Call the sevice get.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string CallSeviceGet(string service, string method, Dictionary<string, object> parameters);

        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="parameters"></param>
        /// <param name="resultParameterName"></param>
        /// <returns></returns>
        string StartProcess(string processName, Dictionary<string, object> parameters, string resultParameterName = null);
    }
}
