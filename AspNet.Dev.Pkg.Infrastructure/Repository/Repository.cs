
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Z.EntityFramework.Plus;

namespace AspNet.Dev.Pkg.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T>, IRepository where T : class, IBaseEntity, new()
    {
        protected readonly DbContext _db;
        protected readonly DbSet<T> dbSet;
        public Repository(DbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public virtual IQueryable<T> FindQuery(IQueryable<T> query)
        {
            return query;
        }
        /// <summary>
        /// 查询所有符合条件的数据
        /// </summary>
        /// <param name="exp">筛选条件
        /// PS:若使用默认的NULL，则返回所有数据
        /// </param>
        public virtual IQueryable<T> FindQuery(Expression<Func<T, bool>> exp)
        {
            var hasIsDeleted = exp.Body.Print().IndexOf("IsDeleted") > -1;
            IQueryable<T> query = dbSet.AsQueryable();
            if (hasIsDeleted)
                query = query.IgnoreQueryFilters();
            if (exp == null)
                return query;
            return query.Where(exp);
        }
        /// <summary>
        /// 判断是否有符合条件的数据
        /// </summary>
        public virtual async Task<bool> IsExistAsync(Expression<Func<T, bool>> exp)
        {
            return await dbSet.AnyAsync(exp);
        }

        /// <summary>
        /// 计算符合条件的数据数量
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> exp = null)
        {
            return await FindQuery(exp).CountAsync();
        }
        /// <summary>
        /// 计算符合条件的数据数量
        /// </summary>
        public virtual async Task<int> CountAsync(IQueryable<T> exp = null)
        {
            return await FindQuery(exp).CountAsync();
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public virtual async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        public virtual int Save()
        {
            return _db.SaveChanges();
        }
        /// <summary>
        /// 运行sql语句
        /// </summary>
        public virtual async Task<int> ExeSqlAsync(string sql) => await _db.Database.ExecuteSqlRawAsync(sql);

        public virtual DbContext GetContext() => _db;

        public virtual DbSet<T> GetDbSet() => dbSet;
    }
}
