using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface ICrudController<T, GetT, CreateT> : IBaseController<T, CreateT> where T : IBaseEntity where GetT : IBaseGet where CreateT : BaseCreate
    {
        /// <summary>
        /// 带条件分页
        /// </summary>
        Task<PageData<T>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page);
        /// <summary>
        /// 带条件查询(不分页)
        /// </summary>;
        Task<ICollection<T>> FindQueryAsync([FromQuery] GetT input);
    }
    public interface IQCrudController<T, GetT, CreateT> : ICrudController<T, GetT, CreateT> where T : IBaseEntity where GetT : IBaseGet<T> where CreateT : BaseCreate
    { }
}
