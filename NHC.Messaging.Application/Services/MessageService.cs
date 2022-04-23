using Hangfire;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NHC.Messaging.Application.Crud;
using NHC.Messaging.Domain;
using NHC.Messaging.Domain.DTO;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NHC.Messaging.Application.Services
{
    public class MessageService :CrudService<Message, Message>, IMessageService
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;
        private const string accepted = "Accepted";

        public MessageService(DataContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task SendMessages(MessageDto messageDto)
        {
            var message = messageDto.Adapt<Message>();
            message.CustomerMessages = messageDto.CustomerMessages.Adapt<IList<CustomerMessage>>();
            var entity = await base.Add(message);
            messageDto.Id = entity.Id;
            BackgroundJob.Enqueue(() => SendMessage(messageDto));
          
        }
        public async Task SendMessage(MessageDto messageDto)
        {
            var messageBody = FormatMessage(messageDto.Header, messageDto.MessageContent);
            foreach (var item in messageDto.CustomerMessages)
            {
                var response = SendTwilloMessage(messageBody, item.CustomerDto.PhoneNumber);
                if (response != null && response.Status.ToString().ToUpper() == accepted.ToUpper())
                {
                    var customerMessage = await _dbContext.CustomerMessages.FirstOrDefaultAsync(x => x.CustomerId == item.CustomerId && x.MessageId == messageDto.Id);

                    customerMessage.Status = customerMessage != null? true: false; 
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
        public MessageResource SendTwilloMessage(string messageBody, string phone)
        {
            try
            {
                string accountSid = _configuration.GetValue<string>("Settings:TWILIO_ACCOUNT_SID");
                string authToken = _configuration.GetValue<string>("Settings:TWILIO_AUTH_TOKEN");

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: messageBody,
                    from: _configuration.GetValue<string>("Settings:Sender"),
                    to: new Twilio.Types.PhoneNumber(phone)
                );
                return message;
            } catch(Exception ex)
            {
                return null;
            }
        }

        public string FormatMessage(string header, string content)
        {
            return string.Concat(header, Environment.NewLine, content);
        }
       public List<MessageDto> GetAllMessages()
        {
            Dictionary<int,MessageDto> messageMap = new Dictionary<int,MessageDto>();
            var result =   _dbContext.Messages
                .Join(
                    _dbContext.CustomerMessages,
                    m => m.Id,
                    cm => cm.MessageId,
                    (m, cm) => new { m, cm }
                )
                .Join(
                    _dbContext.Customers,
                    combinedEntry => combinedEntry.cm.CustomerId,
                    c => c.Id,
                    (combinedEntry, c) =>
                        new MessageDto
                        {
                            Id = combinedEntry.m.Id,
                            Header = combinedEntry.m.Header,
                            MessageContent = combinedEntry.m.MessageContent,
                            Customers = _dbContext.Customers.Where(x => x.Id == combinedEntry.cm.CustomerId).Select(x => new CustomerDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                PhoneNumber = x.PhoneNumber,
                                MessageStatus = combinedEntry.cm.Status
                            } ).ToList()
                        }
        );
            foreach(var message in result.ToList())
            {
                if (!messageMap.ContainsKey(message.Id))
                {
                    messageMap.Add(message.Id, message);
                }
                else
                {
                    messageMap[message.Id].Customers.Add(message.Customers.First());
                }
            }
            return messageMap.Select(d => d.Value).ToList();
        }
    }
}
