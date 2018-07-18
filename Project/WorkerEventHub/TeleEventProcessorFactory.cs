using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Lib.Repos;
using Microsoft.Azure.EventHubs.Processor;

namespace WorkerEventHub
{
    class TeleEventProcessorFactory : IEventProcessorFactory
    {

        private readonly string _cs;
        private readonly string _sharedCs;

        public TeleEventProcessorFactory(string cs, string sharedCs)
        {
            _cs = cs;
            _sharedCs = sharedCs;
        }

         
        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new TeleEventProcessor(context, _cs, _sharedCs);
        }
    }
}
