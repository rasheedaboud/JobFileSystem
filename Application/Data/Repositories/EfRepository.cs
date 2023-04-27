using Ardalis.Specification.EntityFrameworkCore;
using JobFileSystem.Shared.Interfaces;

namespace JobFileSystem.Application.Data.Repositories
{

    public class EfRepository<T> :  RepositoryBase<T>,IAggregateRoot, IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;

        public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


        // Not required to implement anything. Add additional functionalities if required.
    }
}
