using System.Collections.Generic;
using System.Web;

namespace PayPal.Util
{
    public class NVPUtil
    {
        /// <summary>
        /// Split a Name Value formatted string into a dictionary
        /// </summary>
        /// <param name="nvpString"></param>
        /// <returns></returns>
        public Dictionary<string, string> ParseNVPString(string nvpString)
        {
            Dictionary<string, string> nvpMap = new Dictionary<string, string>();
            string[] keyValuePairs = nvpString.Split('&');
            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    nvpMap.Add(keyValue[0], HttpUtility.UrlDecode(keyValue[1], BaseConstants.ENCODING_FORMAT));
                }
            }
            return nvpMap;
        }
    }
}
