using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Mock.Data.Repository
{
    /// <summary>
    /// 仓储实现
    /// </summary>
    public class RepositoryBase : IRepositoryBase, IDisposable
    {
        // private DbContext dbcontext = DbContextFactory.GetCurrentDbContext();
        private readonly DbContext _dbcontext = DbContextFactory.DbContext();
        private DbTransaction DbTransaction { get; set; }
        public IRepositoryBase BeginTrans()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)_dbcontext).ObjectContext.Connection;
            if (dbConnection.State == System.Data.ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            DbTransaction = dbConnection.BeginTransaction();
            return this;
        }
        public int Commit()
        {
            try
            {
                var returnValue = _dbcontext.SaveChanges();
                DbTransaction?.Commit();
                return returnValue;
            }
            catch (Exception)
            {
                DbTransaction?.Rollback();
                throw;
            }
            finally
            {
                this.Dispose();
            }
        }
        public void Dispose()
        {
            DbTransaction?.Dispose();
            //this.dbcontext.Dispose();
        }
        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            _dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return DbTransaction == null ? this.Commit() : 0;
        }
        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            foreach (var entity in entitys)
            {
                _dbcontext.Entry<TEntity>(entity).State =EntityState.Added;
            }
            return DbTransaction == null ? this.Commit() : 0;
        }
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            //dbcontext.Set<TEntity>().Attach(entity);
            //PropertyInfo[] props = entity.GetType().GetProperties();
            //foreach (PropertyInfo prop in props)
            //{
            //    if (prop.GetValue(entity, null) != null)
            //    {
            //        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
            //            dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
            //        dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
            //    }
            //}
            _dbcontext.Entry(entity).State = EntityState.Modified;
            return DbTransaction == null ? this.Commit() : 0;
        }

       public int Update<TEntity>(TEntity entity, params string[] modifystr) where TEntity : class
        {
            _dbcontext.Set<TEntity>().Attach(entity);
            foreach (string item in modifystr)
            {
                    _dbcontext.Entry(entity).Property(item).IsModified = true;
            }
            return DbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _dbcontext.Set<TEntity>().Attach(entity);
            _dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return DbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entitys = _dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => _dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return DbTransaction == null ? this.Commit() : 0;
        }
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class
        {
            return _dbcontext.Set<TEntity>().Find(keyValue);
        }
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _dbcontext.Set<TEntity>().FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> Queryable<TEntity>() where TEntity : class
        {
            return _dbcontext.Set<TEntity>().AsNoTracking();
        }
        public IQueryable<TEntity> Queryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _dbcontext.Set<TEntity>().Where(predicate);
        }
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            return _dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter) where TEntity : class
        {
            return _dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
    }
}
