using backend.Data;
using backend.DTOs.Shared;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Shared
{
    public abstract class RepositoryAsync<TEntity>
        where TEntity : Entity
        
    {

        protected readonly AppDbContext _context;
        protected RepositoryAsync(AppDbContext context) 
        {
            _context = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<PagedResponse<List<TEntity>>> GetManyAsync(int pageNum, int pageSize)
        {
            PagedResponse<List<TEntity>> response = new PagedResponse<List<TEntity>>();
            response.totalCount = await _context.Set<TEntity>().CountAsync();
            response.pageNum = pageNum;
            response.pageSize = pageSize;
            response.data = await _context.Set<TEntity>()
                                    .Skip((pageNum - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            return response;
        }

        public virtual IQueryable<TEntity> GetQueryableAsync()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
