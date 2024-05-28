using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core;
using GrupoLTM.WebSmart.Domain.Models;

namespace GrupoLTM.WebSmart.Domain.Repository
{
    public class QuestionarioRepository 
    {

        DbContext Context;

        public QuestionarioRepository(DbContext context)
        {
            Context = context;

        }

        public void CommitChanges()
        {
            Context.SaveChanges();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> All<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> Filter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<Models.Questionario> Filter(Expression<Func<Questionario, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? Context.Set<Models.Questionario>().Where<Questionario>(filter).OrderByDescending(x => x.DataFim).AsQueryable() : Context.Set<Questionario>().AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.OrderByDescending(x => x.DataFim).Skip(skipCount).Take(size);
            var _resetSetCounter = filter != null ? Context.Set<Models.Questionario>().Where<Questionario>(filter).AsQueryable() : Context.Set<Questionario>().AsQueryable();
            total = _resetSetCounter.Count();
            return _resetSet.AsQueryable();
        }

        public virtual TEntity Create<TEntity>(TEntity TEntityObject) where TEntity : class
        {         
            var newEntry = Context.Set<TEntity>().Add(TEntityObject);
            Context.SaveChanges();
            return newEntry;
        }

        public virtual int Delete<TEntity>(TEntity TEntityObject) where TEntity : class
        {
            Context.Set<TEntity>().Remove(TEntityObject);
            return Context.SaveChanges();
        }

        public virtual int Update<TEntity>(TEntity TEntityObject) where TEntity : class
        {
            try
            {
                var entry = Context.Entry(TEntityObject);
                Context.Set<TEntity>().Attach(TEntityObject);
                entry.State = EntityState.Modified;
                return Context.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ex;
            }
        }

        public virtual int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var objects = Filter<TEntity>(predicate);
            foreach (var obj in objects)
                Context.Set<TEntity>().Remove(obj);
            return Context.SaveChanges();
        }

        public bool Contains<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Context.Set<TEntity>().Count<TEntity>(predicate) > 0;
        }

        public virtual TEntity Find<TEntity>(params object[] keys) where TEntity : class
        {
            return (TEntity)Context.Set<TEntity>().Find(keys);
        }

        public virtual TEntity Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Context.Set<TEntity>().FirstOrDefault<TEntity>(predicate);
        }

        public IQueryable<T> Filter<T>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50) where T : class
        {
            throw new NotImplementedException();
        }


        public virtual void ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams){
            Context.Database.ExecuteSqlCommand(procedureCommand, sqlParams);
           
        }


        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
}
}
