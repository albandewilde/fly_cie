using FlyCie.Model;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlyCie.App.Services
{
    public class ExternalTicketHandler : BackgroundService
    {
        private readonly QueueService _queueService;
        private List<Model.External.Ticket> _ticketList;
        private readonly List<Order> _orders;
        private readonly ExternalService _externalService;

        public ExternalTicketHandler( ExternalService externalService,
            QueueService queueService )
        {
            _externalService = externalService;
            _queueService = queueService;
            _ticketList = new List<Model.External.Ticket>();
            _orders = new List<Order>();
        }

        protected override async Task ExecuteAsync( CancellationToken stoppingToken )
        {
            while( true )
            {
                try
                {
                    Model.External.Ticket ticket;
                    _queueService.Queue.TryDequeue( out ticket );

                    if ( !( ticket is null ) )
                    {
                        var commissionAmount = Convert.ToInt32( ticket.payed_price * 0.1 );
                        ticket.payed_price += commissionAmount;

                        var createdTicket = await _externalService.SendBookTicket( ticket );
                        if( !(createdTicket is null) )
                        {
                            _ticketList.Add( createdTicket );
                            _orders.Add( new Order
                            {
                                BoughtTicket = ticket,
                                CommissionAmount = commissionAmount
                            } );
                        }
                    }
                }
                catch ( Exception e )
                {
                    return;
                }

                await Task.Delay( 5000 );
            }
        }
    }
}
