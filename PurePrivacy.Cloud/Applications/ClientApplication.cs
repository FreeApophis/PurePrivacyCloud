using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PurePrivacy.Cloud.Applications
{
    class ClientApplication : IApplication
    {
        private readonly ILogger<ClientApplication> _logger;
        readonly Queue<Task> _tasks = new Queue<Task>();
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        public ClientApplication(ILogger<ClientApplication> logger)
        {
            _logger = logger;
        }

        public void Run(string[] args)
        {
            foreach (int index in Enumerable.Range(0, 1000))
            {
                _tasks.Enqueue(Task.Run(() => DoWork(index)));
            }

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        void DoWork(int index)
        {
            var wait = _random.Next(20000);

            _logger.Log(LogLevel.Information, $"DoWork[{index}] for {wait}ms in Thread {Thread.CurrentThread.ManagedThreadId }");

            Thread.Sleep(wait);

            _logger.Log(LogLevel.Information, $"Halfway done [{index}] another {wait}ms in Thread {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(wait);

            _logger.Log(LogLevel.Information, $"Done[{index}] in Thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}