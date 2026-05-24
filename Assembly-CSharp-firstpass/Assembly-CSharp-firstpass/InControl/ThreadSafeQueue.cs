using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x0200007A RID: 122
	internal class ThreadSafeQueue<T>
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x000140E4 File Offset: 0x000122E4
		public ThreadSafeQueue()
		{
			this.sync = new object();
			this.data = new Queue<T>();
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00014102 File Offset: 0x00012302
		public ThreadSafeQueue(int capacity)
		{
			this.sync = new object();
			this.data = new Queue<T>(capacity);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00014124 File Offset: 0x00012324
		public void Enqueue(T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				this.data.Enqueue(item);
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001416C File Offset: 0x0001236C
		public bool Dequeue(out T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.data.Count > 0)
				{
					item = this.data.Dequeue();
					return true;
				}
			}
			item = default(T);
			return false;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000141D4 File Offset: 0x000123D4
		public T Dequeue()
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.data.Count > 0)
				{
					return this.data.Dequeue();
				}
			}
			return default(T);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00014238 File Offset: 0x00012438
		public int Dequeue(ref IList<T> list)
		{
			object obj = this.sync;
			int num;
			lock (obj)
			{
				int count = this.data.Count;
				for (int i = 0; i < count; i++)
				{
					list.Add(this.data.Dequeue());
				}
				num = count;
			}
			return num;
		}

		// Token: 0x04000480 RID: 1152
		private object sync;

		// Token: 0x04000481 RID: 1153
		private Queue<T> data;
	}
}
