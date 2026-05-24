using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000047 RID: 71
	internal static class ObjectPool<T> where T : new()
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x00023C26 File Offset: 0x00021E26
		public static T Alloc()
		{
			if (ObjectPool<T>.pool.Count != 0)
			{
				return ObjectPool<T>.pool.Pop();
			}
			return new T();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00023C44 File Offset: 0x00021E44
		public static void Free(T obj)
		{
			ObjectPool<T>.pool.Push(obj);
		}

		// Token: 0x04000181 RID: 385
		private static readonly Stack<T> pool = new Stack<T>();
	}
}
