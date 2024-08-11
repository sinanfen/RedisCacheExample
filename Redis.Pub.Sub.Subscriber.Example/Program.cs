using StackExchange.Redis;


ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1401");

ISubscriber subscriber = connection.GetSubscriber();

await subscriber.SubscribeAsync("mychannel.*", (channel, message) =>
{
    Console.Write(message);
});

Console.Read();