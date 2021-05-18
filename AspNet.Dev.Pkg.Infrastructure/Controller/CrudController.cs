using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Controller
{
    [Route("[controller]")]
    public class CrudController<T, GetT, CreateT> : ICrudController<T, GetT, CreateT>
    where T : IBaseEntity
    where GetT : IBaseGet<T>
    where CreateT : BaseCreate
    {
        protected readonly IModifyApplication<T, CreateT> Write;
        protected readonly IReadonlyApplication<T, GetT> Read;
        public CrudController()
        {
            ServiceGet.GetProvider().Resolve<IModifyApplication<T, CreateT>>();
            Write = ServiceGet.GetProvider()?.GetService<IModifyApplication<T, CreateT>>();
            Read = ServiceGet.GetProvider()?.GetService<IReadonlyApplication<T, GetT>>();
        }

        protected async Task<int> SaveChangesAsync() => await Write.SaveChangesAsync();

        /// <summary>
        /// 添加(单个)
        /// </summary>
        [HttpPost]
        public virtual async Task<T> AddAsync(CreateT entity) => await Write.AddAsync(entity);

        /// <summary>
        /// 删除(单个 id)
        /// </summary>
        [HttpDelete, Route("{id}")]
        public virtual async Task DeleteAsync(Guid id) => await Write.DeleteAsync(id);

        /// <summary>
        /// 删除(多个 id)
        /// </summary>
        [HttpDelete]
        public virtual async Task<int> DeleteRangeAsync(ICollection<Guid> id) => await Write.DeleteRangeAsync(id);

        /// <summary>
        /// 更新
        /// </summary>
        [HttpPut, Route("{id}")]
        public virtual async Task UpdateAsync(Guid id, CreateT entity) => await Write.UpdateAsync(id, entity);

        /// <summary>
        /// 添加(多个)
        /// </summary>
        [HttpPost, Route("AddRange")]
        public virtual async Task<int> AddRangeAsync(ICollection<CreateT> entitys) => await Write.AddRangeAsync(entitys);

        public void Dispose() => Write.SaveChanges();
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
