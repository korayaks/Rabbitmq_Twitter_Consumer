using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbitmq_Consumer
{
    public static class TopicExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("social-topic-exchange", ExchangeType.Topic);//Declare Exchange name , type, and arguments.
            channel.QueueDeclare("social-topic-queue-twitter",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);//Declare queue.
            channel.QueueBind("social-topic-queue-twitter", "social-topic-exchange", "twitter.*");//Bind queue with Exchange and Declare routeKey.
            channel.BasicQos(0, 10, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {//Message received event
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);//incoming message.
                Console.WriteLine(message);
            };
            channel.BasicConsume("social-topic-queue-twitter", true, consumer);
            Console.WriteLine("consumer started");
            Console.ReadLine();
        }
    }
}
