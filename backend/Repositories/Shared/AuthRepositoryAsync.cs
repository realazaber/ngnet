using backend.Data;
using backend.DTOs.Shared;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Shared
{
    public abstract class AuthRepositoryAsync<TAuthEntity>
        where TAuthEntity : AuthEntity        
    {

        protected readonly AppDbContext _context;
        protected AuthRepositoryAsync(AppDbContext context) 
        {
            _context = context;
        }

        public virtual async Task<TAuthEntity> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Set<TAuthEntity>().Where(x => x.CreatorId == userId).FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task CreateAsync(TAuthEntity entity)
        {
            await _context.Set<TAuthEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<PagedResponse<List<TAuthEntity>>> GetManyAsync(int pageNum, int pageSize, Guid userId)
        {
            PagedResponse<List<TAuthEntity>> response = new PagedResponse<List<TAuthEntity>>();
            response.totalCount = await _context.Set<TAuthEntity>().CountAsync();
            response.pageNum = pageNum;
            response.pageSize = pageSize;
            response.data = await _context.Set<TAuthEntity>()
                                    .Where(x => x.CreatorId == userId)
                                    .Skip((pageNum - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            return response;
        }

        public virtual IQueryable<TAuthEntity> GetQueryableAsync()
        {
            return _context.Set<TAuthEntity>().AsQueryable();
        }

        public virtual async Task UpdateAsync(TAuthEntity entity, Guid userId)
        {
            if (entity.CreatorId != userId) {
                return;
            }
            _context.Set<TAuthEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TAuthEntity entity, Guid userId)
        {
            if (entity.CreatorId != userId)
            {
                return;
            }
            _context.Set<TAuthEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
