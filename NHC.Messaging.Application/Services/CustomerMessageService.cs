using NHC.Messaging.Application.Crud;
using NHC.Messaging.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Application.Services
{
    public class CustomerMessageService :ICustomerMessageService
    {
        private readonly DataContext _dbContext;

        public CustomerMessageService(DataContext dbContext) 
        {
            _dbContext = dbContext;
        }
      }
}
