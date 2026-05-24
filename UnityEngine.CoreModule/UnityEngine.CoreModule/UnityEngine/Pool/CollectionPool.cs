using System;
using System.Collections.Generic;

namespace UnityEngine.Pool
{
	// Token: 0x02000377 RID: 887
	public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
	{
		// Token: 0x06001E85 RID: 7813 RVA: 0x000318A9 File Offset: 0x0002FAA9
		public static TCollection Get()
		{
			return CollectionPool<TCollection, TItem>.s_Pool.Get();
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000318B5 File Offset: 0x0002FAB5
		public static PooledObject<TCollection> Get(out TCollection value)
		{
			return CollectionPool<TCollection, TItem>.s_Pool.Get(out value);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000318C2 File Offset: 0x0002FAC2
		public static void Release(TCollection toRelease)
		{
			CollectionPool<TCollection, TItem>.s_Pool.Release(toRelease);
		}

		// Token: 0x040009FE RID: 2558
		internal static readonly ObjectPool<TCollection> s_Pool = new ObjectPool<TCollection>(() => new TCollection(), null, delegate(TCollection l)
		{
			l.Clear();
		}, null, true, 10, 10000);
	}
}
