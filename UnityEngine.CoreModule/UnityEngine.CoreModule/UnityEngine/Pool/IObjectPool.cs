using System;

namespace UnityEngine.Pool
{
	// Token: 0x0200037E RID: 894
	public interface IObjectPool<T> where T : class
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001E99 RID: 7833
		int CountInactive { get; }

		// Token: 0x06001E9A RID: 7834
		T Get();

		// Token: 0x06001E9B RID: 7835
		PooledObject<T> Get(out T v);

		// Token: 0x06001E9C RID: 7836
		void Release(T element);

		// Token: 0x06001E9D RID: 7837
		void Clear();
	}
}
