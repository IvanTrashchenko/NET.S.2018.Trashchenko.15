using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionsLibrary
{
    /// <summary>
    /// Custom queue collection class.
    /// </summary>
    /// <typeparam name="T">IEquatable type of each stack element.</typeparam>
    public class Queue<T> : IEnumerable<T>
        where T : IEquatable<T>
    {
        #region Constants

        /// <summary>
        /// Default capacity of queue.
        /// </summary>
        private const int DefaultCapacity = 4;

        #endregion

        #region Fields

        /// <summary>
        /// Capapcity of queue.
        /// </summary>
        private int capacity;

        /// <summary>
        /// First valid element in the queue.
        /// </summary>
        private int head;

        /// <summary>
        /// Inner array of queue's elements.
        /// </summary>
        private T[] innerArray;

        /// <summary>
        /// Last valid element in the queue.
        /// </summary>
        private int tail;

        /// <summary>
        /// Field for tracking changes of the queue.
        /// </summary>
        private int version;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the queue class with default capacity.
        /// </summary>
        public Queue()
            : this(DefaultCapacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the queue class with specific capacity.
        /// </summary>
        /// <param name="capacity">Capacity of the queue</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when capacity is out of range.</exception>
        public Queue(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(capacity)} is out of range.");
            }

            this.capacity = capacity;
            this.innerArray = new T[this.capacity];
        }

        /// <summary>
        /// Initializes a new instance of the queue class with the help of IEnumerable element.
        /// </summary>
        /// <param name="collection">IEnumerable element.</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null.</exception>
        public Queue(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException($"{nameof(collection)} can not be null.");
            }

            this.innerArray = new T[DefaultCapacity];

            foreach (var item in collection)
            {
                this.Enqueue(item);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets amount of valid elements in queue.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Returns true if queue is empty; otherwise, false.
        /// </summary>
        public bool IsEmpty => this.Count == 0;

        /// <summary>
        /// Queue's indexator.
        /// </summary>
        /// <param name="index">Index value.</param>
        /// <returns>Queue's element of specific index.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when index is out of range.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index > this.capacity - 1)
                {
                    throw new IndexOutOfRangeException($"{nameof(index)} is out of range.");
                }

                int resultIndex = (this.head + index) >= this.capacity
                                      ? (this.head + index - this.capacity)
                                      : (this.head + index);

                return this.innerArray[resultIndex];
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method which clears queue's elements.
        /// </summary>
        public void Clear()
        {
            Array.Clear(this.innerArray, 0, this.capacity);

            this.head = 0;
            this.tail = 0;
            this.Count = 0;
            this.version++;
        }

        /// <summary>
        /// Method which determines whether a specific element is in the queue.
        /// </summary>
        /// <param name="item">Specific element.</param>
        /// <returns>True, if the queue contains the element; otherwise, false.</returns>
        public bool Contains(T item)
        {
            foreach (var element in this.innerArray)
            {
                /*if (ReferenceEquals(element, item))
                {
                    return true;
                }
                 
                if (element == null)
                {
                    continue;
                }*/

                // if (EqualityComparer<T>.Default.Equals(item, element))
                if (element.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <returns>Head object of the queue.</returns>
        public T Dequeue()
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            T removed = this.innerArray[this.head];
            this.innerArray[this.head] = default(T);
            this.head++;
            this.Count--;
            this.version++;
            return removed;
        }

        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">Object to add.</param>
        public void Enqueue(T item)
        {
            if (this.Count == this.capacity)
            {
                this.capacity += DefaultCapacity;
                T[] arr = new T[this.capacity];

                if (this.head < this.tail)
                {
                    Array.Copy(this.innerArray, this.head, arr, 0, this.Count);
                }
                else
                {
                    Array.Copy(this.innerArray, this.head, arr, 0, this.innerArray.Length - this.head);
                    Array.Copy(this.innerArray, 0, arr, this.innerArray.Length - this.head, this.tail);
                }

                this.innerArray = arr;
                this.head = 0;
                this.tail = this.Count;
            }

            if (this.tail == this.capacity)
            {
                this.tail = 0;
            }

            this.innerArray[this.tail] = item;
            this.tail++;
            this.Count++;
            this.version++;
        }

        /// <summary>
        /// Returns the object at the beginning of the queue without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            return this.innerArray[this.head];
        }

        /// <summary>
        /// Copies the queue elements to a new array.
        /// </summary>
        /// <returns>Array of queue's elements.</returns>
        public T[] ToArray()
        {
            T[] arr = new T[this.Count];
            if (this.IsEmpty)
            {
                return arr;
            }

            if (this.head < this.tail)
            {
                Array.Copy(this.innerArray, this.head, arr, 0, this.Count);
            }
            else
            {
                Array.Copy(this.innerArray, this.head, arr, 0, this.innerArray.Length - this.head);
                Array.Copy(this.innerArray, 0, arr, this.innerArray.Length - this.head, this.tail);
            }

            return arr;
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the queue.
        /// </summary>
        public void TrimExcess()
        {
            T[] arr = new T[this.Count];
            int i = 0;
            foreach (var item in this.innerArray)
            {
                if (!EqualityComparer<T>.Default.Equals(item, default(T)))
                {
                    arr[i] = item;
                    i++;
                }
            }

            this.innerArray = arr;
            this.head = 0;
            this.tail = this.Count;
            this.capacity = this.Count;
            this.version++;
        }

        #endregion

        #region IEnumerable methods

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Enumerator

        public struct Enumerator : IEnumerator<T>
        {
            private readonly Queue<T> queue;

            private readonly int version;

            private int currentIndex;
           
            internal Enumerator(Queue<T> queue)
            {
                this.queue = queue;
                this.version = queue.version;
                this.currentIndex = -1;
                this.Current = default(T);
            }

            public T Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this.version != this.queue.version)
                {
                    throw new InvalidOperationException("Queue was changed.");
                }

                if (this.currentIndex == -2)
                {
                    return false;
                }

                this.currentIndex++;

                if (this.currentIndex == this.queue.capacity)
                {
                    this.currentIndex = -2;
                    this.Current = default(T);
                    return false;
                }

                this.Current = this.queue[this.currentIndex];
                return true;
            }

            public void Reset()
            {
                if (this.version != this.queue.version)
                {
                    throw new InvalidOperationException("Queue was changed.");
                }

                this.currentIndex = -1;
                this.Current = default(T);
            }
        }

        #endregion
    }
}