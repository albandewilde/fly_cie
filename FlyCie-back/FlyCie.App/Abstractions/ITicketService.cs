using FlyCie.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlyCie.App.Abstractions
{
    public interface ITicketService
    {
        Task<string> GetCurrencies();
        Task<Currency> GetCurrency( string currencyName );
        Task<List<Ticket>> HandleBook( TicketForm ticketForm );
    }
}
