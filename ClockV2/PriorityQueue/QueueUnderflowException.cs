using System;

namespace PriorityQueue
{
    public class QueueUnderflowException : Exception
    {
        public QueueUnderflowException() : base("Queue is empty") { }

        public QueueUnderflowException(string message) : base(message) { }
    }
}
