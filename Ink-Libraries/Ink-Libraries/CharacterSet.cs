using System;
using System.Collections.Generic;

namespace Ink
{
	// Token: 0x02000003 RID: 3
	public class CharacterSet : HashSet<char>
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020F5 File Offset: 0x000002F5
		public static CharacterSet FromRange(char start, char end)
		{
			return new CharacterSet().AddRange(start, end);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002103 File Offset: 0x00000303
		public CharacterSet()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000210B File Offset: 0x0000030B
		public CharacterSet(string str)
		{
			this.AddCharacters(str);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000211B File Offset: 0x0000031B
		public CharacterSet(CharacterSet charSetToCopy)
		{
			this.AddCharacters(charSetToCopy);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000212C File Offset: 0x0000032C
		public CharacterSet AddRange(char start, char end)
		{
			for (char c = start; c <= end; c += '\u0001')
			{
				base.Add(c);
			}
			return this;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002150 File Offset: 0x00000350
		public CharacterSet AddCharacters(IEnumerable<char> chars)
		{
			foreach (char c in chars)
			{
				base.Add(c);
			}
			return this;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000219C File Offset: 0x0000039C
		public CharacterSet AddCharacters(string chars)
		{
			foreach (char c in chars)
			{
				base.Add(c);
			}
			return this;
		}
	}
}
