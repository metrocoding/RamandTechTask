// See https://aka.ms/new-console-template for more information

using RabbitConsumer.AsyncDataServices;

Console.Clear();

var subscriber = new MessageBusSubscriber();
subscriber.Listen();

Console.ReadLine();