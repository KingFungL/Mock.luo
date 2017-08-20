using Mock.Code;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.Entity.Infrastructure;
using EntityFramework.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Common;

namespace Mock.Data
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        //public DbContext dbcontext = DbContextFactory.GetCurrentDbContext();
        public System.Data.Entity.DbContext dbcontext = DbContextFactory.DbContext();


        /// <summary>
        /// 数据源
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbContext db
        {
            get
            {
                return dbcontext;
            }
        }

        public int Insert(TEntity entity)
        {
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return dbcontext.SaveChanges();
        }
        public int Insert(List<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return dbcontext.SaveChanges();
        }
        /// <summary>
        /// 根据实体属性全部字段修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            dbcontext.Entry(entity).State = EntityState.Modified;
            return dbcontext.SaveChanges();
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
                dbcontext.Set<TEntity>().Attach(entity);
                var SetEntry = ((IObjectContextAdapter)dbcontext).ObjectContext.
                    ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fileds)
                {
                    SetEntry.SetModifiedProperty(t);
                }
                iret = dbcontext.SaveChanges();
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
        public int Delete(TEntity entity)
        {
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return dbcontext.SaveChanges();
        }
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return dbcontext.SaveChanges();
        }
        public TEntity FindEntity(object keyValue)
        {
            return dbcontext.Set<TEntity>().Find(keyValue);
        }
        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> IQueryable()
        {
            return dbcontext.Set<TEntity>().AsNoTracking();
        }
        public IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Set<TEntity>().Where(predicate);
        }
        public List<TEntity> FindList(string strSql)
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList(string strSql, DbParameter[] dbParameter)
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
    }
}
