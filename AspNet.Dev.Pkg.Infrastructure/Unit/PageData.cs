using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Interface;

namespace AspNet.Dev.Pkg.Infrastructure.Unit
{
    public class PageData<T> where T : IBaseEntity
    {
        public int Total { get; set; }
        public int Length { get => Data.Count; }
        public ICollection<T> Data { get; set; }
    }
}
