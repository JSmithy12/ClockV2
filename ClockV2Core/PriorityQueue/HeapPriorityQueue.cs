using System;

namespace PriorityQueue
{
    public class HeapPriorityQueue<T>
    {
        private (T item, int priority)[] heap;
        private int size;

        public HeapPriorityQueue(int capacity)
        {
            heap = new (T, int)[capacity];
            size = 0;
        }

        public void Add(T item, int priority)
        {
            if (size >= heap.Length)
                throw new InvalidOperationException("Queue overflow");
            heap[size] = (item, priority);
            size++;
            HeapifyUp(size - 1);
        }

        public T Head()
        {
            if (size == 0)
                throw new InvalidOperationException("Queue underflow");
            return heap[0].item;
        }

        public void Remove()
        {
            if (size == 0)
                throw new InvalidOperationException("Queue underflow");

            heap[0] = heap[size - 1];
            size--;
            HeapifyDown(0);
        }

        public bool IsEmpty() => size == 0;

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (heap[index].priority >= heap[parentIndex].priority)
                    break;

                var temp = heap[index];
                heap[index] = heap[parentIndex];
                heap[parentIndex] = temp;

                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = size - 1;
            while (index <= lastIndex)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallestChild = index;

                if (leftChild <= lastIndex && heap[leftChild].priority < heap[smallestChild].priority)
                    smallestChild = leftChild;

                if (rightChild <= lastIndex && heap[rightChild].priority < heap[smallestChild].priority)
                    smallestChild = rightChild;

                if (smallestChild == index)
                    break;

                var temp = heap[index];
                heap[index] = heap[smallestChild];
                heap[smallestChild] = temp;

                index = smallestChild;
            }
        }
    }
}
