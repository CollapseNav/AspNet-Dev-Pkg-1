using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Controller
{
    // [Route("[controller]")]
    // public class BaseController : ControllerBase
    // {
    //     protected readonly DbContext _context;
    //     protected IdentityUser<Guid> CurrentUser = null;
    //     protected HttpContext _httpContext = null;
    //     private readonly IMapper _mapper;
    //     public BaseController()
    //     {
    //         var provider = ServiceGet.GetProvider();
    //         _mapper = provider?.GetService<IMapper>();
    //         IHttpContextAccessor httpContextAccessor = provider?.GetService<IHttpContextAccessor>();
    //         _httpContext = httpContextAccessor?.HttpContext;
    //         var user = _httpContext?.User;
    //         var userInfo = user?.Claims.FirstOrDefault(item => item.Type.ToLower() == "userInfo".ToLower());
    //     }
    // }
    [Route("api/[controller]")]
    public class ModifyController<T, CreateT> : Controller, IModifyController<T, CreateT>
    where T : class, IBaseEntity
    where CreateT : IBaseCreate
    {
        protected IModifyApplication<T, CreateT> Write;
        public ModifyController()
        {
            Write = ServiceGet.GetProvider()?.GetService<IModifyApplication<T, CreateT>>();
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
    }
}

