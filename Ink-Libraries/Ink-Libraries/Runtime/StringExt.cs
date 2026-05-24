using System;
using System.Collections.Generic;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x0200002E RID: 46
	public static class StringExt
	{
		// Token: 0x06000318 RID: 792 RVA: 0x00012CA8 File Offset: 0x00010EA8
		public static string Join<T>(string separator, List<T> objects)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (T t in objects)
			{
				if (!flag)
				{
					stringBuilder.Append(separator);
				}
				stringBuilder.Append(t.ToString());
				flag = false;
			}
			return stringBuilder.ToString();
		}
	}
}
