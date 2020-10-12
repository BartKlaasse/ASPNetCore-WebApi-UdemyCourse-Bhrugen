using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyMVC.Utility
{
    public static class Static
    {
        public static string APIBaseUrl = "http://127.0.0.1:5300";
        public static string NationalParkAPIPath = APIBaseUrl + "/api/v1/nationalparks/";
        public static string TrailAPIPath = APIBaseUrl + "/api/v1/trails/";
        public static string AccountAPIPath = APIBaseUrl + "/api/v1/Users/";
    }
}