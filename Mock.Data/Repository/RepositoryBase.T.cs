using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace Mock.Data.Repository
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        //public DbContext dbcontext = DbContextFactory.GetCurrentDbContext();
        public DbContext Dbcontext = DbContextFactory.DbContext();

        /// <summary>
        /// 数据源
        /// </summary>
        /// <returns></returns>
        public DbContext Db => Dbcontext;

        public int Insert(TEntity entity)
        {
            Dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return Dbcontext.SaveChanges();
        }
        public int Insert(List<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                Dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return Dbcontext.SaveChanges();
        }
        /// <summary>
        /// 根据实体属性全部字段修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            Dbcontext.Entry(entity).State = EntityState.Modified;
            return Dbcontext.SaveChanges();
        }

        /// <summary>  
        /// 更新指定字段  
        /// </summary>  
        /// <param name="entity">实体</param>  
        /// <param name="fileds">更新字段数组</param>  
        public int Update(TEntity entity, params string[] fileds)
        {
            int iret = 0;
            if (entity != null && fileds != null)
            {
                Dbcontext.Set<TEntity>().Attach(entity);
                var setEntry = ((IObjectContextAdapter)Dbcontext).ObjectContext.
                    ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fileds)
                {
                    setEntry.SetModifiedProperty(t);
                }
                iret = Dbcontext.SaveChanges();
            }
            return iret;
        }

        /// <summary>
        /// userQuery,x => new User { FaceUrl = faceUrl, AvatarUrl = avatarUrl}
        /// </summary>
        /// <param name="source">查询出的实体</param>
        /// <param name="updateExpression">更新字段的lambda表达式</param>
        /// <returns></returns>
        public int Update(IQueryable<TEntity> source, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int iret = source.Update<TEntity>(updateExpression);
            return iret;
        }
        public Task UpdateAsync(IQueryable<TEntity> source, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return source.UpdateAsync<TEntity>(updateExpression);
        }

        public int Delete(TEntity entity)
        {
            Dbcontext.Set<TEntity>().Attach(entity);
            Dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return Dbcontext.SaveChanges();
        }
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitys = Dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => Dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return Dbcontext.SaveChanges();
        }
        public TEntity FindEntity(object keyValue)
        {
            return Dbcontext.Set<TEntity>().Find(keyValue);
        }
        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return Dbcontext.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> Queryable()
        {
            return Dbcontext.Set<TEntity>();
        }
        public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate)
        {
            return Dbcontext.Set<TEntity>().AsNoTracking().Where(predicate);
        }
        public List<TEntity> FindList(string strSql)
        {
            return Dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList(string strSql, object[] dbParameter)
        {
            return Dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
    }
}
