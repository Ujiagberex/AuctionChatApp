using RabbitMQ.Client;
using System.Text;

namespace ActionApp.Services
{
	public class RoomService : BackgroundService
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;

		public RoomService()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: "roomToBidding", durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				// Shows a user entering a bidding room and starting an auction
				var message = "Auction started";
				var body = Encoding.UTF8.GetBytes(message);

				_channel.BasicPublish(exchange: "", routingKey: "roomToBidding", basicProperties: null, body: body);
				Task.Delay(5000, stoppingToken).Wait(stoppingToken); // Send message every 5 seconds
			}

			return Task.CompletedTask;
		}

		public override void Dispose()
		{
			_channel.Close();
			_connection.Close();
			base.Dispose();
		}
	}
}
