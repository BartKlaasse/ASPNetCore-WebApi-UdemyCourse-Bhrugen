using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI
{
    public class AppSettings
    {
        //FLOW: Deze class krijgt info uit het object AppSettings in appsettings.json
        public string Secret { get; set; }
    }
}