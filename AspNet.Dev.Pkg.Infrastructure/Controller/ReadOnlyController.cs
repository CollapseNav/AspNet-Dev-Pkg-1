using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using AspNet.Dev.Pkg.Infrastructure.Unit;

namespace AspNet.Dev.Pkg.Infrastructure.Controller
{
    [Route("api/[controller]")]
    public class ReadOnlyController<T, GetT> : Controller, IReadOnlyController<T, GetT>
    where T : class, IBaseEntity
    where GetT : IBaseGet<T>
    {
        protected IReadonlyApplication<T, GetT> Read { get; set; }
        public ReadOnlyController() : base()
        {
            Read = ServiceGet.GetProvider()?.GetService<IReadonlyApplication<T, GetT>>();
        }
        /// <summary>
        /// 查找(单个 id)
        /// </summary>
        [HttpGet, Route("{id}")]
        public virtual async Task<T> FindAsync(Guid id) => await Read.FindAsync(id);

        /// <summary>
        /// 带条件分页
        /// </summary>
        [HttpGet]
        public virtual async Task<PageData<T>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page) => await Read.FindPageAsync(input, page);
        /// <summary>
        /// 带条件查询(不分页)
        /// </summary>
        [HttpGet, Route("Query")]
        public virtual async Task<IReadOnlyCollection<T>> FindQueryAsync([FromQuery] GetT input) => await Read.FindQueryAsync(input);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        [HttpGet, Route("ByIds")]
        public virtual async Task<IReadOnlyCollection<T>> FindByIdsAsync([FromQuery] ICollection<Guid> ids) => await Read.FindByIdsAsync(ids);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        [HttpPost, Route("ByIds")]
        public virtual async Task<IReadOnlyCollection<T>> FindByIdsPostAsync(ICollection<Guid> ids) => await Read.FindByIdsAsync(ids);
    }
}

