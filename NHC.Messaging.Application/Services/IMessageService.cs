using NHC.Messaging.Application.Crud;
using NHC.Messaging.Domain;
using NHC.Messaging.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Application.Services
{
    public interface IMessageService: ICrudService<Message, Message>
    {
         Task<Message> GetById(int id);
         List<MessageDto> GetAllMessages();
         Task SendMessages(MessageDto message);
    }
}
