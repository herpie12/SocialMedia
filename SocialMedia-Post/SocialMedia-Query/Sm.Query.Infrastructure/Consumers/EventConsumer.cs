using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Sm.Query.Infrastructure.Converter;
using Sm.Query.Infrastructure.Handlers;
using System.Text.Json;

namespace Sm.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IEventHandler _eventHandler;

        public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
        {
            _consumerConfig = config.Value;
            _eventHandler = eventHandler;
        }
        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
                 .SetKeyDeserializer(Deserializers.Utf8)
                 .SetValueDeserializer(Deserializers.Utf8)
                 .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult?.Message == null) continue;

                var options = new JsonSerializerOptions
                {
                    Converters = { new EventJsonConverter() }
                };

                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

                //Retrive method from eventhandler which name is: On, based on the event coming in.
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[]
                {
                    @event.GetType()
                });

                if (handlerMethod != null)
                {
                    throw new ArgumentException(nameof(handlerMethod), "could not find event handler method!");
                }

                //Invoke the matching method in the eventhandler
                handlerMethod.Invoke(_eventHandler, new object[] { @event });

                consumer.Commit(consumeResult);
            }
        }
    }
}
