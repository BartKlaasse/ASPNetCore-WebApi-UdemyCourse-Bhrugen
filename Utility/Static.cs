using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyMVC.Utility
{
    public static class Static
    {
        public static string APIBaseUrl = "https://localhost:5001";
        public static string NationalParkAPIPath = APIBaseUrl + "/api/v1/nationalparks";
        public static string TrailAPIPath = APIBaseUrl + "/api/v1/trails";
    }
}