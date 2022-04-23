using NHC.Messaging.Application.Crud;
using NHC.Messaging.Domain;

namespace NHC.Messaging.Application.Services
{
    public interface ICustomerService: ICrudService<Customer, Customer>
    {
    }
}
