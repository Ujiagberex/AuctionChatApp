using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace ActionApp.Services
{
	public class InvoiceService : BackgroundService
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;

		public InvoiceService()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: "notificationToInvoice", durable: false, exclusive: false, autoDelete: false, arguments: null);
			_channel.QueueDeclare(queue: "invoiceToPayment", durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				// Process the message

				// Simulate sending a message to Payment Service
				var paymentMessage = "Process payment";
				var paymentBody = Encoding.UTF8.GetBytes(paymentMessage);
				_channel.BasicPublish(exchange: "", routingKey: "invoiceToPayment", basicProperties: null, paymentBody);
			};

			_channel.BasicConsume(queue: "notificationToInvoice", autoAck: true, consumer: consumer);
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
