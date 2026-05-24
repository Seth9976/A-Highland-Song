using System;
using System.Collections.Generic;

namespace Ink
{
	// Token: 0x0200000A RID: 10
	public static class InkStringConversionExtensions
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00006530 File Offset: 0x00004730
		public static string[] ToStringsArray<T>(this List<T> list)
		{
			int count = list.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				string[] array2 = array;
				int num = i;
				T t = list[i];
				array2[num] = t.ToString();
			}
			return array;
		}
	}
}
