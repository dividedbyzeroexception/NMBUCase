using System;
using System.Text;
using RabbitMQ.Client;
using NMBUIDM;
using Newtonsoft.Json;

namespace EventSource
{
    class EventSender
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "vhost", UserName = "user", Password = "pass" };
            Console.Title = "SENDER";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    while (true)
                    {
                        var myevent = new PersonEvent {
                            EventAction = PersonEventAction.PersonUpdated,
                            EventID = 4096,
                            EventSource = "NO.NMBU.IT.IDM.USERWEB",
                            EventUniqueID = Guid.NewGuid(),
                            NewPerson = new Person {
                                FamilyName = "Diffie",
                                GivenName = "Whitfield",
                                Mobile = "909 00 808",
                                PersonID = 101
                            }
                        };
                    
                        string message = JsonConvert.SerializeObject(myevent, Formatting.Indented);
                        var body = Encoding.UTF8.GetBytes(message);
                    
                        channel.BasicPublish(exchange: "IDM",
                                             routingKey: "Person.All",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}", message);

                        System.Threading.Thread.Sleep(5000);
                    }                    
                }
            }
        }
    }
}
