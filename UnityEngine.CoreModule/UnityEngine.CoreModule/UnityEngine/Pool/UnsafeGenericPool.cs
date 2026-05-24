using System;

namespace UnityEngine.Pool
{
	// Token: 0x02000383 RID: 899
	public static class UnsafeGenericPool<T> where T : class, new()
	{
		// Token: 0x06001EB3 RID: 7859 RVA: 0x00031E7B File Offset: 0x0003007B
		public static T Get()
		{
			return UnsafeGenericPool<T>.s_Pool.Get();
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x00031E87 File Offset: 0x00030087
		public static PooledObject<T> Get(out T value)
		{
			return UnsafeGenericPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x00031E94 File Offset: 0x00030094
		public static void Release(T toRelease)
		{
			UnsafeGenericPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000A17 RID: 2583
		internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(() => new T(), null, null, null, false, 10, 10000);
	}
}
