using backend.Data;
using backend.Models;
using backend.Repositories.Shared;

namespace backend.Repositories
{
    public class EventsRepository: RepositoryAsync<DbEvent>
    {
        
        public EventsRepository(AppDbContext context) : base(context)
        {
         
        }

        
    }
}
