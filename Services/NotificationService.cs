using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace ActionApp.Services
{
	public class NotificationService : BackgroundService
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;

		public NotificationService()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: "biddingToNotification", durable: false, exclusive: false, autoDelete: false, arguments: null);
			_channel.QueueDeclare(queue: "notificationToInvoice", durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				// Process the message

				// Simulate sending a message to Invoice Service
				var invoiceMessage = "Generate invoice";
				var invoiceBody = Encoding.UTF8.GetBytes(invoiceMessage);
				_channel.BasicPublish(exchange: "", routingKey: "notificationToInvoice", basicProperties: null, invoiceBody);
			};

			_channel.BasicConsume(queue: "biddingToNotification", autoAck: true, consumer: consumer);
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