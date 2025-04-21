using System;

namespace PriorityQueue
{
    public class QueueOverflowException : Exception
    {
        public QueueOverflowException() : base("Queue is full") { }

        public QueueOverflowException(string message) : base(message) { }
    }
}
