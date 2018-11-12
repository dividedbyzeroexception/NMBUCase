using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EventConsumer
{
    class EventReceiver
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "192.168.1.57", VirtualHost = "vhost", UserName = "user", Password = "pass" };

            Console.Title = "CONSUMER";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                        
                    };
                    channel.BasicConsume(queue: "Person",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }
            }
        }
    }
}
