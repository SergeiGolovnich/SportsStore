using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public static class UrlExtentions
    {
        public static string PathAndQuery(this HttpRequest httpRequest) =>
            httpRequest.QueryString.HasValue ?
            $"{httpRequest.Path}{httpRequest.QueryString}" :
            httpRequest.Path.ToString();
    }
}
