using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Util;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Unit
{
    public class PageData<T>
    {
        public int? Total { get; set; }
        public int? Length { get => Data?.Count; }
        public IReadOnlyCollection<T> Data { get; set; }
        public PageData() { }
        public PageData(int? total, IReadOnlyCollection<T> data)
        {
            Total = total;
            Data = data;
        }
        public static PageData<T> GenPageData<Input>(PageData<Input> data)
        {
            return new PageData<T>(data.Total, data.Data.Maps<T>() as IReadOnlyCollection<T>);
        }
    }
}
