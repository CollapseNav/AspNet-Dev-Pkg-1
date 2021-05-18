using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace AspNet.Dev.Pkg.Infrastructure.Repository
{
    public class ModifyRepository<T> : Repository<T>, IModifyRepository<T>, IRepository<T>, IRepository
    where T : class, IBaseEntity, new()
    {
        public ModifyRepository(DbContext db) : base(db) { }

        /// <summary>
        /// 添加数据(单个)
        /// </summary>
        /// <param name="entity">新的数据</param>
        public virtual async Task<T> AddAsync(T entity)
        {
            entity.Init();
            await dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// 添加数据(集合)
        /// </summary>
        /// <param name="entityList">新的数据集合</param>
        public virtual async Task<int> AddRangeAsync(ICollection<T> entityList)
        {
            foreach (var entity in entityList)
                entity.Init();
            await dbSet.AddRangeAsync(entityList);
            return entityList.Count;
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">需要删除的数据</param>
        /// <param name="isTrue">是否真删</param>
        public virtual async Task<bool> DeleteAsync(T entity, bool isTrue = false)
        {
            entity.SoftDelete();
            await UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// 有条件地删除数据
        /// </summary>
        /// <param name="exp">筛选条件</param>
        /// <param name="isTrue">是否真删</param>
        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> exp, bool isTrue = false)
        {
            if (isTrue)
            {
                return await dbSet.Where(exp).DeleteAsync();
                // var entitys = await dbSet.Where(exp).ToListAsync();
                // dbSet.RemoveRange(entitys);
                // return entitys.Count;
            }
            return await UpdateAsync(exp, entity => new T { IsDeleted = true, LastModificationTime = DateTime.Now });
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="isTrue">是否真删</param>
        public virtual async Task<bool> DeleteAsync(Guid id, bool isTrue = false)
        {
            var entity = await dbSet.FindAsync(id);
            await DeleteAsync(entity);
            return true;
        }
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="isTrue">是否真删</param>
        public virtual async Task<int> DeleteAsync(ICollection<Guid> id, bool isTrue = false)
        {
            Expression<Func<T, bool>> exp = item => id.Contains(item.Id.Value);
            return await DeleteAsync(exp, isTrue);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public virtual async Task UpdateAsync(T entity)
        {
            entity.Update();
            var entry = _db.Entry(entity);
            entry.State = EntityState.Modified;
            await Task.FromResult("");
        }

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        public virtual async Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            return await dbSet.Where(where).UpdateAsync(entity);
        }
    }
}
