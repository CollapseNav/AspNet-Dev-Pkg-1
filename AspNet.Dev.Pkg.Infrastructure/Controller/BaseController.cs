using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspNet.Dev.Pkg.Infrastructure.Controller
{
    [Route("api/[controller]")]
    public class BaseController<T, CreateT> : ControllerBase, IBaseController<T, CreateT> where T : IBaseEntity where CreateT : BaseCreate
    {
        protected readonly DbContext _context;
        protected readonly ILogger _log;
        protected IBaseApplication<T, CreateT> _base;
        protected IdentityUser<Guid> CurrentUser = null;
        private readonly IMapper _mapper;
        public BaseController(ILogger<BaseController<T, CreateT>> logger, IBaseApplication<T, CreateT> app)
        {
            _log = logger;
            _base = app;
            var provider = ServiceGet.GetProvider();
            _mapper = provider?.GetService<IMapper>();
            IHttpContextAccessor httpContextAccessor = provider?.GetService<IHttpContextAccessor>();
            HttpContext httpContext = httpContextAccessor?.HttpContext;
            var user = httpContext?.User;
            var userInfo = user?.Claims.FirstOrDefault(item => item.Type.ToLower() == "userInfo".ToLower());
            if (userInfo != null)
            {
                app.SetCurrentUser(JsonConvert.DeserializeObject<IdentityUser<Guid>>(userInfo.Value));
            }
        }
        protected async Task<int> SaveChangesAsync()
        {
            return await _base.SaveChangesAsync();
        }
        /// <summary>
        /// 获取全部
        /// </summary>
        [HttpGet, Route("All")]
        public virtual async Task<ICollection<T>> FindAllAsync()
        {
            return await _base.FindAllAsync(null);
        }

        /// <summary>
        /// 添加(单个)
        /// </summary>
        [HttpPost]
        public virtual async Task<T> AddAsync(CreateT entity)
        {
            var result = await _base.AddAsync(entity);
            return result;
        }

        /// <summary>
        /// 查找(单个 id)
        /// </summary>
        [HttpGet, Route("{id}")]
        public virtual async Task<T> FindAsync(Guid id)
        {
            return await _base.FindAsync(id);
        }

        /// <summary>
        /// 删除(单个 id)
        /// </summary>
        [HttpDelete, Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _base.DeleteAsync(id);
        }

        /// <summary>
        /// 删除(多个 id)
        /// </summary>
        [HttpDelete]
        public virtual async Task<int> DeleteRangeAsync(ICollection<Guid> id)
        {
            return await _base.DeleteRangeAsync(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        [HttpPut, Route("{id}")]
        public virtual async Task UpdateAsync(Guid id, CreateT entity)
        {
            await _base.UpdateAsync(id, entity);
        }

        /// <summary>
        /// 添加(多个)
        /// </summary>
        [HttpPost, Route("AddRange")]
        public virtual async Task<int> AddRangeAsync(ICollection<CreateT> entitys)
        {
            return await _base.AddRangeAsync(entitys);
        }

        public void Dispose()
        {
            _base.SaveChanges();
        }
    }
}

