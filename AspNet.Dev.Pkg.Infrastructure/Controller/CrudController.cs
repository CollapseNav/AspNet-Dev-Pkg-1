using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Dev.Pkg.Infrastructure.Controller
{
    [Route("[controller]")]
    public class CrudController<T, Return, GetT, CreateT> : BaseController<T, CreateT>,
    ICrudController<T, Return, GetT, CreateT>
    where T : IBaseEntity
    where Return : BaseReturn
    where GetT : IBaseGet
    where CreateT : BaseCreate
    {
        protected readonly ICrudApplication<T, Return, GetT, CreateT> _app;
        public CrudController(ICrudApplication<T, Return, GetT, CreateT> app) : base(app)
        {
            _app = app;
        }
        [HttpPost]
        public virtual new async Task<Return> AddAsync(CreateT entity) => await _app.AddAsync(entity);

        [HttpGet]
        public async Task<PageData<Return>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page) => await _app.FindPageAsync(input, page);

        [HttpGet, Route("Query")]
        public async Task<ICollection<Return>> FindQueryAsync([FromQuery] GetT input) => await _app.FindQueryAsync(input);
    }
    [Route("[controller]")]
    public class CrudController<T, GetT, CreateT> : BaseController<T, CreateT>,
    ICrudController<T, GetT, CreateT>
    where T : IBaseEntity
    where GetT : IBaseGet
    where CreateT : BaseCreate
    {
        protected readonly ICrudApplication<T, GetT, CreateT> _app;
        public CrudController(ICrudApplication<T, GetT, CreateT> app) : base(app) => _app = app;
        /// <summary>
        /// 带条件分页
        /// </summary>
        [HttpGet]
        public virtual async Task<PageData<T>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page) => await _app.FindPageAsync(input, page);
        /// <summary>
        /// 带条件查询(不分页)
        /// </summary>
        [HttpGet, Route("Query")]
        public virtual async Task<ICollection<T>> FindQueryAsync([FromQuery] GetT input) => await _app.FindQueryAsync(input);
    }
}
