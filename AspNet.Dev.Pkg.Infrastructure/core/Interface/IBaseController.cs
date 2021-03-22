using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseController
    {
    }

    public interface IBaseController<T, CreateT> : IBaseController, IDisposable where T : IBaseEntity where CreateT : BaseCreate
    {
        /// <summary>
        /// 添加(单个)
        /// </summary>
        [HttpPost]
        Task<T> AddAsync(CreateT entity);

        /// <summary>
        /// 查找(单个 id)
        /// </summary>
        [HttpGet, Route("{id}")]
        Task<T> FindAsync(Guid id);

        /// <summary>
        /// 删除(单个 id)
        /// </summary>
        [HttpDelete, Route("{id}")]
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 删除(多个 id)
        /// </summary>
        [HttpDelete]
        Task<int> DeleteRangeAsync(ICollection<Guid> id);

        /// <summary>
        /// 更新
        /// </summary>
        [HttpPut, Route("{id}")]
        Task UpdateAsync(Guid id, CreateT entity);

        /// <summary>
        /// 添加(多个)
        /// </summary>
        [HttpPost, Route("AddRange")]
        Task<int> AddRangeAsync(ICollection<CreateT> entitys);
    }
}

