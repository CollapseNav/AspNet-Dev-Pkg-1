using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IReadOnlyController<T, GetT> : IController
    where T : IBaseEntity
    where GetT : IBaseGet<T>
    {
        /// <summary>
        /// 查找(单个 id)
        /// </summary>
        [HttpGet, Route("{id}")]
        Task<T> FindAsync(Guid id);
        /// <summary>
        /// 带条件分页
        /// </summary>
        [HttpGet]
        Task<PageData<T>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page);
        /// <summary>
        /// 带条件查询(不分页)
        /// </summary>
        [HttpGet, Route("Query")]
        Task<IReadOnlyCollection<T>> FindQueryAsync([FromQuery] GetT input);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        [HttpGet, Route("ByIds")]
        Task<IReadOnlyCollection<T>> FindByIdsAsync(ICollection<Guid> ids);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        [HttpPost, Route("ByIds")]
        Task<IReadOnlyCollection<T>> FindByIdsPostAsync(ICollection<Guid> ids);
    }
}
