using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KJ_API_Projekt.ApiKey
{
    public class ApiKeyConstants
    {
        public static readonly string HttpHeaderField = "X-Api-Key";
        public static readonly string HttpQueryParamKey = "ApiKey";
        public static readonly string AuthenticationSchemeName = "api key";
    }
}
