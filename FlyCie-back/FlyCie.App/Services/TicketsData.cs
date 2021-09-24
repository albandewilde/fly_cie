using FlyCie.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyCie.App.Services
{
    public static class TicketsData
    {
        private static IEnumerable<Ticket> _tickets { get; set; }
        public static IEnumerable<Ticket> TicketList => _tickets;
        private static IEnumerable<Order> _orders { get; set; }
        public static IEnumerable<Order> Orders => _orders;
        public static void SetTickets( IEnumerable<Ticket> tickets )
        {
            _tickets = tickets;
        }
        public static void SetOrders( IEnumerable<Order> orders )
        {
            _orders = orders;
        }
    }
}
