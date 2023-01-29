using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ComicLbrry
{
    class ApiConfigCls
    {
        public static long TmStmp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public static string PublicK => "ba449717eb5bbd27d6939205fdbe604e";
        public static string PrivateK => "a3506d58754753c6d9bdfae5fd8658d6328c86f4";
        public static string Hash => BitConverter.ToString(MD5
                                                            .Create()
                                                            .ComputeHash(Encoding.Default
                                                                            .GetBytes($"{TmStmp}{PrivateK}{PublicK}"))
                                                )
                                                .ToLower()
                                                .Replace("-", string.Empty);
    }
}
