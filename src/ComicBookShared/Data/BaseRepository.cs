using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace ComicBookShared.Data
{
    public abstract class BaseRepository<TEntity>
        where TEntity : class // struct, new()
    {
        protected Context Context { get; private set; }

        //BaseRepository(context)
        public BaseRepository(Context context)
        {
            Context = context;
        }

        public abstract TEntity Get(int id, bool includRelatedEntites = true);
        public abstract IList<TEntity> GetList();


        //Add(entity)
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        //Update()
        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        //Delete()
        public void Delete(int id)
        {
            var set = Context.Set<TEntity>();
            var entity = set.Find(id);
            set.Remove(entity);
            Context.SaveChanges();
        }
    }
}
