using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Domain
{
    public abstract class Entity:TransactionEntity
    {
        public int Id { get; set; }
    }
}
