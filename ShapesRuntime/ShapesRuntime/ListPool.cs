using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000048 RID: 72
	internal static class ListPool<T> where T : new()
	{
		// Token: 0x06000A01 RID: 2561 RVA: 0x00023C5D File Offset: 0x00021E5D
		public static List<T> Alloc()
		{
			if (ListPool<T>.pool.Count != 0)
			{
				return ListPool<T>.pool.Pop();
			}
			return new List<T>();
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00023C7B File Offset: 0x00021E7B
		public static void Free(List<T> list)
		{
			list.Clear();
			ListPool<T>.pool.Push(list);
		}

		// Token: 0x04000182 RID: 386
		private static readonly Stack<List<T>> pool = new Stack<List<T>>();
	}
}
