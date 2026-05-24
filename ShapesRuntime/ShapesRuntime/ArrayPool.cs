using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000046 RID: 70
	internal static class ArrayPool<T>
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x00023BEE File Offset: 0x00021DEE
		public static T[] Alloc(int maxCount)
		{
			if (ArrayPool<T>.pool.Count != 0)
			{
				return ArrayPool<T>.pool.Pop();
			}
			return new T[maxCount];
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00023C0D File Offset: 0x00021E0D
		public static void Free(T[] obj)
		{
			ArrayPool<T>.pool.Push(obj);
		}

		// Token: 0x04000180 RID: 384
		private static readonly Stack<T[]> pool = new Stack<T[]>();
	}
}
