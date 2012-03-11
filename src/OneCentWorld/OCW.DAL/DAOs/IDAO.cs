using System;
using System.Collections.Generic;

namespace OCW.DAL.DAOs
{
    public interface IDAO<TEntity, in TKey, out TEntityList>
        where TEntityList : IEnumerable<TEntity>
    {
        TEntity Add(TEntity entity);
        TEntity Read(TKey key);
        TEntityList ReadAll(Func<TEntity, bool> predicate = null);
        TEntityList ReadAll(int pageSize, int page, Func<TEntity, bool> predicate = null);
        int Count(Func<TEntity, bool> predicate = null);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(TKey entity);
    }
}
