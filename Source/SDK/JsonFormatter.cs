/* NuGet Install
 * Visual Studio 2005 or 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from "net20" for Visual Studio 2005 or "net35" for Visual Studio 2008
 * Visual Studio 2010 or higher
    * Install-Package Newtonsoft.Json
    * Reference is auto-added 
*/
using Newtonsoft.Json;

namespace PayPal
{
    public static class JsonFormatter
    {  
        public static string ConvertToJson<T>(T t) 
        {
            return JsonConvert.SerializeObject(t);
        }

        public static T ConvertFromJson<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
