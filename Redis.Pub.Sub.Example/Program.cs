using StackExchange.Redis;


ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1401");

ISubscriber subscriber = connection.GetSubscriber();

while (true)
{
    Console.Write("Mesaj : ");
    string message = Console.ReadLine();
    var clients = await subscriber.PublishAsync("mychannel",message);
}