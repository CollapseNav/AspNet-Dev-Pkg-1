using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public static class StringExt
    {
        public static bool IsNullOfEmpty(this string str) => string.IsNullOrEmpty(str);
        public static T ToObj<T>(this string str) => JsonConvert.DeserializeObject<T>(str);
        public static ICollection<T> ToObjCollection<T>(this string str) => JsonConvert.DeserializeObject<ICollection<T>>(str);
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);


        public static string PadLeft(this int num, int total, char fill) => num.ToString().PadLeft(total, fill);
        public static string PadRight(this int num, int total, char fill) => num.ToString().PadRight(total, fill);
        public static string PadLeft(this double num, int total, char fill) => num.ToString().PadLeft(total, fill);
        public static string PadRight(this double num, int total, char fill) => num.ToString().PadRight(total, fill);
        public static string PadLeft(this long num, int total, char fill) => num.ToString().PadLeft(total, fill);
        public static string PadRight(this long num, int total, char fill) => num.ToString().PadRight(total, fill);
    }
}
