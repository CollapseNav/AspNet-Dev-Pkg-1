using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public static class StringExt
    {
        public static bool IsNullOfEmpty(this string str) => string.IsNullOrEmpty(str);
        public static T ToObj<T>(this string str) => JsonConvert.DeserializeObject<T>(str);
        public static ICollection<T> ToObjCollection<T>(this string str) => JsonConvert.DeserializeObject<ICollection<T>>(str);
    }
}
