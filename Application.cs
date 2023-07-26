using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener
{
    public class Application
    {
        private readonly Core logService;
        public Application()
        {
            logService = new Core();
        }
        public void GetMessageFromRabbitMq()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // RabbitMQ bağlantı bilgileri
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            // Queue adı
            string queueName = "urls";

            // Queue declare (oluştur)
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Consumer oluşturma
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);

                // Gelen urle head fonksiyonu çağırıyoruz
                var statusCode = await logService.SendHeadRequestToUrl(message);
                logService.Logger(message, statusCode);
            };

            // Consumer'ı Queue'ya bağlama
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Consumer started. Press any key to exit.");
            Console.ReadLine();
        }
    }
}
