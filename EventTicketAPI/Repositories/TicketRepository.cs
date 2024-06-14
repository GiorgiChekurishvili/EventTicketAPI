using EventTicketAPI.Dtos;
using EventTicketAPI.Dtos.Transactions;
using EventTicketAPI.Entities;
using EventTicketAPI.Services;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit.Text;
using MimeKit;
using System.Security.Cryptography;
using MailKit.Net.Smtp;

namespace EventTicketAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly EventTicketContext _context;
        private readonly ITransactionService _transactionService;
        private readonly IConfiguration _config;
        public TicketRepository(EventTicketContext context, ITransactionService transactionService, IConfiguration config)
        {

            _context = context;
            _transactionService = transactionService;
            _config = config;
        }

        public IEnumerable<TicketSale> GetTickets(int userid)
        {

            var finisheddata = _context.TicketSales
                               .Include(t => t.Event)
                               .Include(t => t.TicketType)
                               .Where(x => x.UserId == userid)
                               .ToList();
            return finisheddata;
            
        }
        public async Task<decimal> AddTicket(TicketSale ticketSale)
        {
            if (ticketSale != null)
            {
                var date = _context.TicketTypes.FirstOrDefault(x => x.Id == ticketSale.TicketTypeId);
                if (date.SalesEndDate > ticketSale.PurchaseDate && date.SalesStartDate < ticketSale.PurchaseDate)
                {


                    var _eventId = _context.TicketTypes.Where(x => x.EventId == ticketSale.EventId).ToList();
                    if (_eventId != null)
                    {
                        var totalticketprice = _eventId.FirstOrDefault(x => x.Id == ticketSale.TicketTypeId);
                        if (totalticketprice != null)
                        {
                            var _totalprice = ticketSale.TicketQuantity * totalticketprice.Price;
                            var token = CreateRandomToken();
                            TicketSale ticket = new TicketSale()
                            {
                                UserId = ticketSale.UserId,
                                EventId = ticketSale.EventId,
                                TicketTypeId = ticketSale.TicketTypeId,
                                TotalPrice = _totalprice,
                                TicketQuantity = ticketSale.TicketQuantity,
                                GeneratedTicketId = token
                               
                            };
                            var tickettype = _context.TicketTypes.FirstOrDefault(x => x.Id == ticketSale.TicketTypeId);
                            if (tickettype != null)
                            {
                                if (tickettype.TicketsAvailable != 0)
                                {
                                    var modified = tickettype.TicketsAvailable - ticketSale.TicketQuantity;
                                    tickettype.TicketsAvailable = modified;
                                    if (modified >= 0)
                                    {
                                        _context.TicketTypes.Update(tickettype);
                                        _context.SaveChanges();
                                    }
                                    
                                    else
                                    {
                                        return 0;
                                    }
                                    var user = _context.Users.Where(x=>x.Id == ticketSale.UserId).FirstOrDefault();
                                    if (user != null)
                                    {
                                        var eventname = _context.Events.FirstOrDefault(x => x.Id == ticketSale.EventId);
                                        FillTransactionsDto transactionsDto = new FillTransactionsDto()
                                        {
                                            UserId = ticketSale.UserId,
                                            Amount = _totalprice * -1,
                                            Reason = $"Bought Ticket Type: '{totalticketprice.TicketTypeName}', '{ticketSale.TicketQuantity}' Tickets, for '{eventname.EventName}'"
                                        };
                                        var buyTicketTransaction = await _transactionService.MakeTransaction(transactionsDto);
                                        await _transactionService.ResetTransactionsCache(ticketSale.UserId);
                                        await _transactionService.ResetBalanceCache(ticketSale.UserId);
                                        
                                        SendEmail(ticketSale, token);
                                        if (buyTicketTransaction == null)
                                        {
                                            return 0;
                                        }
                                    }
                                }
                                else
                                {
                                    return 0;
                                }
                                _context.TicketSales.Add(ticket);
                                _context.SaveChanges();


                                return _totalprice;

                            }
                        }

                    }
                }
                
            }
            return 0;

            
        }
        public void DeleteTicket(int ticketId, int userId)
        {
           var _eventid = _context.TicketSales.Where(x => x.UserId == userId).ToList();
            if (_eventid != null)
            {
                var _userid = _eventid.FirstOrDefault(x => x.Id == ticketId);
                
                if (_userid != null)
                {
                    var tickettype = _context.TicketTypes.FirstOrDefault(x => x.Id == _userid.TicketTypeId);
                    var modified = tickettype!.TicketsAvailable - _userid.TicketQuantity;
                    tickettype.TicketsAvailable = modified;

                    _context.TicketTypes.Update(tickettype);
                    _context.TicketSales.Remove(_userid);
                    _context.SaveChanges();
                }
                
            }
        }
        public int InsertTicketType(TicketType ticketType)
        {
            if (ticketType.TotalTickets != 0 || ticketType.Price != 0)
            {

                if (ticketType.SalesEndDate > DateTime.Now && ticketType.SalesStartDate >= DateTime.Now)
                {
                    if (ticketType.TotalTickets > 0)
                    {
                        if (ticketType.Price > 0)
                        {
                            var eventStartDate = _context.Events.FirstOrDefault(x => x.Id == ticketType.EventId);
                            if (ticketType.SalesEndDate < eventStartDate?.EventDate)
                            {

                                if (ticketType.SalesEndDate > ticketType.SalesStartDate)
                                {
                                    var ifExists = _context.TicketTypes.FirstOrDefault(x => x.EventId == ticketType.EventId && x.TicketTypeName.ToLower() == ticketType.TicketTypeName.ToLower());
                                    if (ifExists == null)
                                    {

                                        var capacity = _context.Events.FirstOrDefault(x => x.Id == ticketType.EventId);
                                        if (capacity != null)
                                        {

                                            var ticketsAvailable = _context.TicketTypes.Where(x => x.EventId == ticketType.EventId).ToList();
                                            var totalavailable = 0;
                                            for (int i = 0; i < ticketsAvailable.Count; i++)
                                            {
                                                totalavailable += ticketsAvailable[i].TotalTickets;
                                            }

                                            if (capacity.Capacity >= totalavailable + ticketType.TotalTickets)
                                            {
                                                ticketType.TicketsAvailable = ticketType.TotalTickets;
                                                _context.TicketTypes.Add(ticketType);
                                                _context.SaveChanges();

                                                return capacity.Capacity - (totalavailable + ticketType.TotalTickets);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return 0;
            
            
        }
        public void UpdateTicketTypeRepo(TicketType ticketType)
        {
            if (ticketType.TotalTickets <= 0 || ticketType.Price <= 0 || ticketType.SalesEndDate <  DateTime.Now)
            {
                
                return;
            }

            var relatedEvent = _context.Events.FirstOrDefault(e => e.Id == ticketType.EventId);
            if (relatedEvent == null)
            {
                
                return;
            }

            if (ticketType.SalesEndDate >= relatedEvent.EventDate)
            {
                
                return;
            }

            if (ticketType.SalesEndDate <= ticketType.SalesStartDate)
            {
                return;
            }

            var relatedTickets = _context.TicketTypes.Where(t => t.EventId == ticketType.EventId).ToList();
            var totalAvailableTickets = relatedTickets.Sum(t => t.TotalTickets);

            var oldTicket = _context.TicketTypes.FirstOrDefault(t => t.Id == ticketType.Id);
            if (oldTicket == null)
            {
                return;
            }
            _context.Entry(oldTicket).State = EntityState.Detached;

            var capacity= relatedEvent.Capacity;

            if (capacity >= (totalAvailableTickets - oldTicket?.TotalTickets) + ticketType.TotalTickets)
            {
                ticketType.TicketsAvailable = ticketType.TotalTickets;
                _context.TicketTypes.Update(ticketType);
                _context.SaveChanges();
            }

        }
    
        public void DeleteTicketType(int ticketTypeId, int eventId)
        {
           
            var ticketType = _context.TicketTypes.FirstOrDefault(x => x.Id == ticketTypeId);
            if (ticketType != null)
            {
                if (eventId == ticketType.EventId)
                {
                    var ticketsale = _context.TicketSales.Where(x=>x.TicketTypeId == ticketTypeId).ToList();
                    for (int i = 0; i < ticketsale.Count; i++)
                    {
                        _context.TicketSales.Remove(ticketsale[i]);
                    }
                    _context.TicketTypes.Remove(ticketType);
                    _context.SaveChanges();
                }
                
            }

        }
        public IEnumerable<TicketType> GetAllTicketTypes(int eventId)
        {
            var data = _context.TicketTypes.Where(x => x.EventId == eventId).ToList();
            return data;
        }
        public void SendEmail(TicketSale ticketSale, string token)
        {
            var email = _context.Users.FirstOrDefault(x => x.Id == ticketSale.UserId);
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSender").Value));
            mail.To.Add(MailboxAddress.Parse(email!.Email));
            mail.Subject = "Ticket";
            string eventName = "Concert";
            int quantity = 2;
            decimal pricePerTicket = 50.00m;
            decimal totalPrice = quantity * pricePerTicket;

            string bodyText = $@"
                Dear Customer,

                Thank you for your purchase!

                Here are your ticket details:

                Ticket ID: {token}
                Event: {eventName}
                Quantity: {quantity}
                Price per Ticket: ${pricePerTicket:F2}
                Total Price: ${totalPrice:F2}

                We hope you enjoy the event!
            ";
            mail.Body = new TextPart(TextFormat.Plain) { Text = bodyText, };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailSender").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(mail);
            smtp.Disconnect(true);

        }
        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
        }


    }
}
