using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using System.Threading.Tasks;
using Lib;
using Lib.Repos;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WorkerEventHub
{
    public class TeleEventProcessor : IEventProcessor
    {
        private readonly PointRepo _pointRepo;
        private readonly PointRepo _pointRepoShared;

        public TeleEventProcessor(PartitionContext context, string cs, string sharedCs)
        {
            _pointRepo = new PointRepo(cs);
            _pointRepoShared = new PointRepo(sharedCs);
        }


        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Processes the message on the eventHub
        /// </summary>
        /// <param name="context"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                
                var json = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                Console.WriteLine($"{json}");
                var data = JsonConvert.DeserializeObject<JObject>(json);
                var type = data.Value<string>("Type");
                var msg = data.Value<string>("Msg");
                switch (type)
                {
                    case "Point":

                        var point = JsonConvert.DeserializeObject<Point>(msg);
                        await _pointRepo.InsertAsync(point);
                        if (point.Type == 2)
                        {
                            await _pointRepoShared.InsertSharedPointAsync(point);
                        }
                        break;
                    case "Selfie":
                        var selfieTime = JsonConvert.DeserializeObject<UriTime>(msg);
                        await _pointRepo.InsertSelfieAsync(selfieTime);
                        break;
                }

               
            }

            await context.CheckpointAsync();
        }
    }
}
