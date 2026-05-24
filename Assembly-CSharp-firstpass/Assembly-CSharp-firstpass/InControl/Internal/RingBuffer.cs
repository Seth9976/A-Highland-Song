using System;

namespace InControl.Internal
{
	// Token: 0x02000080 RID: 128
	public class RingBuffer<T>
	{
		// Token: 0x060005F3 RID: 1523 RVA: 0x0001595C File Offset: 0x00013B5C
		public RingBuffer(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentException("RingBuffer size must be 1 or greater.");
			}
			this.size = size + 1;
			this.data = new T[this.size];
			this.head = 0;
			this.tail = 0;
			this.sync = new object();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000159B4 File Offset: 0x00013BB4
		public void Enqueue(T value)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.size > 1)
				{
					this.head = (this.head + 1) % this.size;
					if (this.head == this.tail)
					{
						this.tail = (this.tail + 1) % this.size;
					}
				}
				this.data[this.head] = value;
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00015A44 File Offset: 0x00013C44
		public T Dequeue()
		{
			object obj = this.sync;
			T t;
			lock (obj)
			{
				if (this.size > 1 && this.tail != this.head)
				{
					this.tail = (this.tail + 1) % this.size;
				}
				t = this.data[this.tail];
			}
			return t;
		}

		// Token: 0x04000496 RID: 1174
		private readonly int size;

		// Token: 0x04000497 RID: 1175
		private readonly T[] data;

		// Token: 0x04000498 RID: 1176
		private int head;

		// Token: 0x04000499 RID: 1177
		private int tail;

		// Token: 0x0400049A RID: 1178
		private readonly object sync;
	}
}
