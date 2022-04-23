using NHC.Messaging.Application.Crud;
using NHC.Messaging.Domain;

namespace NHC.Messaging.Application.Services
{
    public class CustomerService : CrudService<Customer, Customer>, ICustomerService
    {
        private readonly DataContext _dbContext;
        public CustomerService(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
