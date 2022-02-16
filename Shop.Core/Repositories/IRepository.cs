using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public interface IRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp,params string[] includes);
        Task<bool> IsExisted(Expression<Func<TEntity, bool>> exp,params string[] includes);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> exp,params string[] includes);
        Task<int> CommitAsync();
        int Commit();
        void Remove(TEntity entity);
    }
}
