using FlyCie.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlyCie.App.Abstractions
{
    public interface ITicketService
    {
        Task<List<Ticket>> BookTickets( TicketForm ticketForm );
        Task<string> GetCurrencies();
        Task<Currency> GetCurrency( string currencyName );
    }
}
