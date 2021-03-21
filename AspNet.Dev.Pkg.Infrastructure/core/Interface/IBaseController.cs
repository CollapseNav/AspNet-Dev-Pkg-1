using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseController
    {
    }
    public interface IBaseController<T, CreateT> : IBaseController, IDisposable where T : IBaseEntity where CreateT : BaseCreate
    {
        /// <summary>
        /// 获取全部
        /// </summary>
        Task<ICollection<T>> FindAllAsync();
        /// <summary>
        /// 添加(单个)
        /// </summary>
        Task<T> AddAsync(CreateT entity);

        /// <summary>
        /// 查找(单个 id)
        /// </summary>
        Task<T> FindAsync(Guid id);

        /// <summary>
        /// 删除(单个 id)
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 删除(多个 id)
        /// </summary>
        Task<int> DeleteRangeAsync(ICollection<Guid> id);

        /// <summary>
        /// 更新
        /// </summary>
        Task UpdateAsync(Guid id, CreateT entity);

        /// <summary>
        /// 添加(多个)
        /// </summary>
        Task<int> AddRangeAsync(ICollection<CreateT> entitys);
    }
}

