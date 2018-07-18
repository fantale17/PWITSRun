using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lib.Repos;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;

namespace WorkerEventHub
{
    class Program
    {

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }


        private static async Task MainAsync(string[] args)
        {
            //carica le impostazioni in appsettings
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json",
                    optional: true,
                    reloadOnChange: true);
            var configuration = builder.Build();

         


            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                configuration["EventHubPath"],
                PartitionReceiver.DefaultConsumerGroupName,
                configuration["ConnectionStrings:EventHubConnectionString"],
                configuration["ConnectionStrings:StorageAccountConnectionString"],
                configuration["StorageContainer"]);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorFactoryAsync(
                new TeleEventProcessorFactory(configuration["ConnectionStrings:DefaultConnection"], configuration["ConnectionStrings:SharedRunConnection"]));

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}
