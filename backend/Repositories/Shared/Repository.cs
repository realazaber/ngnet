using backend.Data;
using backend.DTOs.Shared;
using backend.Models;

namespace backend.Repositories.Shared
{
    public abstract class Repository<TEntity>
        where TEntity : Entity
        
    {

        private readonly AppDbContext _context;
        protected Repository(AppDbContext context)
        {
            _context = context;
        }

        public virtual TEntity GetById(Guid id)
        {
            return _context.Set<TEntity>().FirstOrDefault(x => x.Id == id);
        }

        public virtual void Create(TEntity entity) { 
            _context.Set<TEntity>().Add(entity); 
        }

        public PagedResponse<List<TEntity>> GetMany(int pageNum, int pageSize)
        {
            var response = new PagedResponse<List<TEntity>>();
            response.totalCount = _context.Set<TEntity>().Count();
            response.pageNum = pageNum;
            response.pageSize = pageSize;
            response.data = _context.Set<TEntity>()
                                    .Skip((pageNum - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            return response;
        }


        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
