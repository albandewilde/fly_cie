using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace FlyCie.App.Services
{
    public class QueueService
    {
        private ConcurrentQueue<Model.External.Ticket> _queue;
        private ILogger<QueueService> _logger;

        public QueueService()
        {
            _queue = new ConcurrentQueue<Model.External.Ticket>();
        }

        public ConcurrentQueue<Model.External.Ticket> Queue => _queue;

        public void EnqueueTicket( Model.External.Ticket ticket )
        {
            _logger.LogInformation( $"Enqueue ticket to send to external: {ticket}" );
            _queue.Enqueue( ticket );
        }
    }
}
