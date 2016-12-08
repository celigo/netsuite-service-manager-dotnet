using System;
using System.Collections.Generic;
using System.Threading;

namespace com.celigo.net.ServiceManager.Utility
{
	/// <summary>Fixed size blocking-queue.</summary>
	/// <remarks>
	/// Adopted from http://blogs.msdn.com/toub/archive/2006/04/12/575103.aspx
	/// </remarks>
	class BlockingQueue<T> : IEnumerable<T>
	{
		private readonly int _queueSize;
		private int _count = 0;

		private Queue<T> _queue;

		public T Dequeue()
		{
			lock (_queue)
			{
				while (_count <= 0)
				{
					Monitor.Wait(_queue);
				}
				_count--;
				return _queue.Dequeue();
			}
		}

		public void Enqueue(T data)
		{
			if (data == null) 
				throw new ArgumentNullException("data");
			lock (_queue)
			{
				_queue.Enqueue(data);
				_count++;
				Monitor.Pulse(_queue);
			}
		}

		public BlockingQueue(int size)
		{
			_queueSize = size;
			_queue = new Queue<T>(size);
		}

		public IEnumerator<T> GetEnumerator()
		{
			while (_queue.Count != _queueSize) 
                Monitor.Wait(_queue);

			int i = _count;
			while (i-- > 0) 
                yield return Dequeue();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}
}
