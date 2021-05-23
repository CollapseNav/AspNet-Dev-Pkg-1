using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Entity;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{
    public static class ObjToHash
    {
        public static HashEntry[] ToHashEntries<T>(this ICollection<T> objs) where T : BaseEntity
        {
            List<HashEntry> props = new();
            foreach (var obj in objs)
            {
                props.Add(new HashEntry(obj.Id.ToString(), JsonConvert.SerializeObject(obj)));
            }
            return props.ToArray();
        }

        public static T GetHashEntity<T>(this HashEntry hashEntry)
        {
            return JsonConvert.DeserializeObject<T>(hashEntry.Value);
        }

        public static ICollection<T> GetHashEntries<T>(this HashEntry[] hashEntries)
        {
            List<T> result = new();
            foreach (var entity in hashEntries)
            {
                result.Add(JsonConvert.DeserializeObject<T>(entity.Value));
            }
            return result;
        }
    }
}
