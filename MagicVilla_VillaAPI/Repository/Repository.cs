using MagicVilla_VillaAPI.DB;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T>:IRepository<T> where T: class
    {
        private readonly ApplicationDBContext _db;
        public DbSet<T> dbSet;
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet=_db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,int pageSize=0,int pageNumber=1)
        {
            IQueryable<T> entity = dbSet;
            if (filter != null)
            {
                entity = entity.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageNumber > 100) { pageNumber = 100; }
                entity = entity.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            }
            if (includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    entity=entity.Include(includeProp);
                }
            }
            
            return await entity.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> entity = dbSet;
            if (!tracked)
            {
                entity = entity.AsNoTracking();
            }
            if (filter != null)
            {
                entity = entity.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    entity = entity.Include(includeProp);
                }
            }
            return await entity.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        

    }
}
