using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComicLbrry
{
    class RequestHandlingCls
    {
        public string RequestLaunch(string url) => new WebClient().DownloadString(url);
    }
}
