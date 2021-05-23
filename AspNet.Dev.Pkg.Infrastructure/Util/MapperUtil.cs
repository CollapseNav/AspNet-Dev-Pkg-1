using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Util
{

    public static class MapperUtil
    {
        public static ICollection<T> Maps<T>(this object objs)
        {
            var mapper = ServiceGet.GetProvider()?.GetService<IMapper>();
            if (mapper == null)
                throw new Exception("无法获取IMapper实例");
            return mapper.Map<ICollection<T>>(objs);
        }

        public static IReadOnlyCollection<T> ReadOnlyMaps<T>(this object objs)
        {
            var mapper = ServiceGet.GetProvider()?.GetService<IMapper>();
            if (mapper == null)
                throw new Exception("无法获取IMapper实例");
            return mapper.Map<IReadOnlyCollection<T>>(objs);
        }

        public static T Map<T>(this object obj)
        {
            var mapper = ServiceGet.GetProvider()?.GetService<IMapper>();
            if (mapper == null)
                throw new Exception("无法获取IMapper实例");
            return mapper.Map<T>(obj);
        }
        public static T Map<T>(this object obj, T target)
        {
            var mapper = ServiceGet.GetProvider()?.GetService<IMapper>();
            if (mapper == null)
                throw new Exception("无法获取IMapper实例");
            return mapper.Map(obj, target);
        }
    }
}
