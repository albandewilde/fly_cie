using FlyCie.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public class ExternalTicketHandler : IHostedService
    {
        private ConcurrentQueue<Model.External.Ticket> _queue;
        private List<Model.External.Ticket> _ticketList;
        private readonly ExternalService _externalService;
        private readonly ILogger<ExternalTicketHandler> _logger;
        private readonly List<Order> _orders;

        public ExternalTicketHandler( 
            ExternalService externalService,
            ILogger<ExternalTicketHandler> logger )
        {
            _queue = new ConcurrentQueue<Model.External.Ticket>();
            _externalService = externalService;
            _ticketList = new List<Model.External.Ticket>();
            _logger = logger;
            _orders = new List<Order>();
        }

        public async Task StartAsync( CancellationToken cancellationToken )
        {
            _logger.LogInformation( "Starting External ticket handler" );

            while( true )
            {
                Model.External.Ticket ticket;
                _queue.TryDequeue( out ticket );

                var commissionAmount = Convert.ToInt32( ticket.payed_price * 0.1 );
                ticket.payed_price += commissionAmount;

                var createdTicket = await _externalService.SendBookTicket( ticket );
                _ticketList.Add( createdTicket );
                _orders.Add( new Order
                {
                    BoughtTicket = ticket,
                    CommissionAmount = commissionAmount
                } );
            }
        }

        public async Task StopAsync( CancellationToken cancellationToken )
        {
            _logger.LogWarning( "Stoping external ticket handler" );
        }

        public async Task EnqueueTicket( Model.External.Ticket ticket )
        {
            _logger.LogInformation( $"Enqueue ticket to send to external: {ticket}" );
            _queue.Enqueue( ticket );
        }
    }
}
