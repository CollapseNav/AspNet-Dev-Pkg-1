using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNet.Dev.Pkg.Infrastructure.Controller
{
    [Route("api/[controller]")]
    public class CrudController<T, GetT, CreateT> : BaseController<T, CreateT>,
    ICrudController<T, GetT, CreateT> where T : IBaseEntity where GetT : IBaseGet where CreateT : BaseCreate
    {
        private readonly ICrudApplication<T, GetT, CreateT> _app;
        public CrudController(ILogger<CrudController<T, GetT, CreateT>> logger, ICrudApplication<T, GetT, CreateT> app) : base(logger, app)
        {
            _app = app;
        }
        /// <summary>
        /// 带条件分页
        /// </summary>
        [HttpGet]
        public virtual async Task<PageData<T>> FindPageAsync([FromQuery] GetT input, [FromQuery] PageRequest page)
        {
            return await _app.FindPageAsync(input, page);
        }
        /// <summary>
        /// 带条件查询(不分页)
        /// </summary>
        [HttpGet, Route("Query")]
        public virtual async Task<ICollection<T>> FindQueryAsync([FromQuery] GetT input)
        {
            return await _app.FindAllAsync(input);
        }
    }

    [Route("api/[controller]")]
    public class QCrudController<T, GetT, CreateT> : CrudController<T, GetT, CreateT>,
    IQCrudController<T, GetT, CreateT> where T : IBaseEntity where GetT : IBaseGet<T> where CreateT : BaseCreate
    {
        public QCrudController(ILogger<QCrudController<T, GetT, CreateT>> logger, IQCrudApplication<T, GetT, CreateT> app) : base(logger, app) { }
    }
}
