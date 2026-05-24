using System;

namespace UnityEngine.Pool
{
	// Token: 0x02000382 RID: 898
	public struct PooledObject<T> : IDisposable where T : class
	{
		// Token: 0x06001EB1 RID: 7857 RVA: 0x00031E56 File Offset: 0x00030056
		internal PooledObject(T value, IObjectPool<T> pool)
		{
			this.m_ToReturn = value;
			this.m_Pool = pool;
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x00031E67 File Offset: 0x00030067
		void IDisposable.Dispose()
		{
			this.m_Pool.Release(this.m_ToReturn);
		}

		// Token: 0x04000A15 RID: 2581
		private readonly T m_ToReturn;

		// Token: 0x04000A16 RID: 2582
		private readonly IObjectPool<T> m_Pool;
	}
}
