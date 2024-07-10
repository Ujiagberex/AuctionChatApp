
using ActionApp.Data;
using ActionApp.Interfaces;
using ActionApp.Repository;
using ActionApp.Services;
using Microsoft.EntityFrameworkCore;
using ActionApp.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace ActionApp
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSignalR();

			//Register DbContext that uses SQL Server as the RDBMS
			builder.Services.AddDbContext<AuctionAppDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			
			// Register RabbitMQ connection and services
			builder.Services.AddSingleton(sp =>
			{
				var factory = new ConnectionFactory()
				{
					HostName = "localhost",
					Port = 5672, // RabbitMQ port
					UserName = "guest", // RabbitMQ username
					Password = "guest" // RabbitMQ password
				};

				try
				{
					return factory.CreateConnection();
				}
				catch (BrokerUnreachableException ex)
				{
					// Log the error or handle it as needed
					throw new Exception("RabbitMQ broker is unreachable", ex);
				}
			});

			builder.Services.AddSingleton(sp =>
			{
				var connection = sp.GetRequiredService<IConnection>();
				return connection.CreateModel();
			});


			//Register the services to be hosted added
			builder.Services.AddHostedService<BiddingService>();
			builder.Services.AddHostedService<InvoiceService>();
			builder.Services.AddHostedService<NotificationService>();
			builder.Services.AddHostedService<PaymentService>();
			builder.Services.AddHostedService<RoomService>();

			//Register the Repositories
			builder.Services.AddScoped<IAuctionRepo, AuctionRepo>();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
				app.UseDeveloperExceptionPage();
			}
            else
            {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthorization();


			app.MapControllers();
			app.MapHub < ChatHub > ("/Chathub");

			app.Run();
		}
	}
}
