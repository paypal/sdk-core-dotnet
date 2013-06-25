/* NuGet Install
 * Visual Studio 2008
    * Install Newtonsoft.Json -OutputDirectory .\packages
    * Add reference from the folder "net35"
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
