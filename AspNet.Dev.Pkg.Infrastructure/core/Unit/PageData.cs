using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Util;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Unit
{
    public class PageData<T>
    {
        public int Total { get; set; }
        public int Length { get => Data.Count; }
        public ICollection<T> Data { get; set; }
        public PageData() { }
        public PageData(int total, ICollection<T> data)
        {
            Total = total;
            Data = data;
        }
        public static PageData<T> GenPageData<Input>(PageData<Input> data)
        {
            var provider = ServiceGet.GetProvider();
            var mapper = provider?.GetService<IMapper>();
            return new PageData<T>(data.Total, mapper.Map<ICollection<T>>(data.Data));
        }
    }
}
