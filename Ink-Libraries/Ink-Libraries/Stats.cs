using System;
using System.Collections.Generic;
using Ink.Parsed;

namespace Ink
{
	// Token: 0x0200000D RID: 13
	public struct Stats
	{
		// Token: 0x060000AC RID: 172 RVA: 0x0000667C File Offset: 0x0000487C
		public static Stats Generate(Story story)
		{
			Stats stats = default(Stats);
			List<Text> list = story.FindAll<Text>(null);
			stats.words = 0;
			foreach (Text text in list)
			{
				int num = 0;
				bool flag = true;
				foreach (char c in text.text)
				{
					if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
					{
						flag = true;
					}
					else if (flag)
					{
						num++;
						flag = false;
					}
				}
				stats.words += num;
			}
			List<Knot> list2 = story.FindAll<Knot>(null);
			stats.knots = list2.Count;
			stats.functions = 0;
			using (List<Knot>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.isFunction)
					{
						stats.functions++;
					}
				}
			}
			List<Stitch> list3 = story.FindAll<Stitch>(null);
			stats.stitches = list3.Count;
			List<Choice> list4 = story.FindAll<Choice>(null);
			stats.choices = list4.Count;
			List<Gather> list5 = story.FindAll<Gather>((Gather g) => g.debugMetadata != null);
			stats.gathers = list5.Count;
			List<Divert> list6 = story.FindAll<Divert>(null);
			stats.diverts = list6.Count - 1;
			return stats;
		}

		// Token: 0x04000034 RID: 52
		public int words;

		// Token: 0x04000035 RID: 53
		public int knots;

		// Token: 0x04000036 RID: 54
		public int stitches;

		// Token: 0x04000037 RID: 55
		public int functions;

		// Token: 0x04000038 RID: 56
		public int choices;

		// Token: 0x04000039 RID: 57
		public int gathers;

		// Token: 0x0400003A RID: 58
		public int diverts;
	}
}
