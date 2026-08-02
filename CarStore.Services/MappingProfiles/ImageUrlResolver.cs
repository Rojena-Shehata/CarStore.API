using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Services.MappingProfiles
{
    public static class ImageUrlResolver
    {


        public static string ToImageUrl(this string? imageUrl, IConfiguration config)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;

            if (imageUrl.StartsWith("http"))
                return imageUrl;

            var baseUrl = config["URLs:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;

            return $"{baseUrl}/{imageUrl}";
        }
    }
}
