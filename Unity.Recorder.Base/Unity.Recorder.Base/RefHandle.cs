using System;
using System.Runtime.InteropServices;

namespace Unity.Media
{
	// Token: 0x02000002 RID: 2
	public class RefHandle<T> : IDisposable where T : class
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public bool IsCreated
		{
			get
			{
				return this.m_Handle.IsAllocated;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002094 File Offset: 0x00000294
		public T Target
		{
			get
			{
				if (!this.IsCreated)
				{
					return default(T);
				}
				return this.m_Handle.Target as T;
			}
			set
			{
				if (this.IsCreated)
				{
					this.m_Handle.Free();
				}
				if (value != null)
				{
					this.m_Handle = GCHandle.Alloc(value, GCHandleType.Normal);
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C3 File Offset: 0x000002C3
		public RefHandle()
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020CB File Offset: 0x000002CB
		public RefHandle(T target)
		{
			this.m_Handle = default(GCHandle);
			this.Target = target;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020E6 File Offset: 0x000002E6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020F5 File Offset: 0x000002F5
		public void Dispose(bool disposing)
		{
			if (this.Disposed)
			{
				return;
			}
			if (this.IsCreated)
			{
				this.m_Handle.Free();
			}
			this.Disposed = true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000211C File Offset: 0x0000031C
		~RefHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x04000001 RID: 1
		private GCHandle m_Handle;

		// Token: 0x04000002 RID: 2
		private bool Disposed;
	}
}
