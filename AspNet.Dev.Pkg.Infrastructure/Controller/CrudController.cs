using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        protected readonly ICrudApplication<T, GetT, CreateT> App;
        public CrudController()
        {
            App = ServiceGet.GetProvider()?.GetService<ICrudApplication<T, GetT, CreateT>>();
        }

        protected async Task<int> SaveChangesAsync() => await App.SaveChangesAsync();

        /// <summary>
        /// 添加(单个)
        /// </summary>
        [HttpPost]
        public virtual async Task<T> AddAsync(CreateT entity) => await App.AddAsync(entity);

        /// <summary>
        /// 删除(单个 id)
        /// </summary>
        [HttpDelete, Route("{id}")]
        public virtual async Task DeleteAsync(Guid id) => await App.DeleteAsync(id);

        /// <summary>
        /// 删除(多个 id)
        /// </summary>
        [HttpDelete]
        public virtual async Task<int> DeleteRangeAsync(ICollection<Guid> id) => await App.DeleteRangeAsync(id);

        /// <summary>
        /// 更新
        /// </summary>
        [HttpPut, Route("{id}")]
        public virtual async Task UpdateAsync(Guid id, CreateT entity) => await App.UpdateAsync(id, entity);

        /// <summary>
        /// 添加(多个)
        /// </summary>
        [HttpPost, Route("AddRange")]
        public virtual async Task<int> AddRangeAsync(ICollection<CreateT> entitys) => await App.AddRangeAsync(entitys);

        public void Dispose() => App.SaveChanges();
        /// <summary>
        /// 查找(单个 id)
        /// </summary>
        [HttpGet, Route("{id}")]
        public virtual async Task<T> FindAsync(Guid id) => await App.FindAsync(id);

        /// <summary>
        /// 带条件分页
        /// </summary>
        [HttpGet]
        public virtual async Task<PageData<T>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page) => await App.FindPageAsync(input, page);
        /// <summary>
        /// 带条件查询(不分页)
        /// </summary>
        [HttpGet, Route("Query")]
        public virtual async Task<IReadOnlyCollection<T>> FindQueryAsync([FromQuery] GetT input) => await App.FindQueryAsync(input);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        [HttpGet, Route("ByIds")]
        public virtual async Task<IReadOnlyCollection<T>> FindByIdsAsync([FromQuery] ICollection<Guid> ids) => await App.FindByIdsAsync(ids);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        [HttpPost, Route("ByIds")]
        public virtual async Task<IReadOnlyCollection<T>> FindByIdsPostAsync(ICollection<Guid> ids) => await App.FindByIdsAsync(ids);
    }
}
