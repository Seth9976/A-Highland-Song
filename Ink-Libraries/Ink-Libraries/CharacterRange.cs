using System;
using System.Collections.Generic;

namespace Ink
{
	// Token: 0x02000002 RID: 2
	public sealed class CharacterRange
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static CharacterRange Define(char start, char end, IEnumerable<char> excludes = null)
		{
			return new CharacterRange(start, end, excludes);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
		public CharacterSet ToCharacterSet()
		{
			if (this._correspondingCharSet.Count == 0)
			{
				for (char c = this._start; c <= this._end; c += '\u0001')
				{
					if (!this._excludes.Contains(c))
					{
						this._correspondingCharSet.Add(c);
					}
				}
			}
			return this._correspondingCharSet;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020AE File Offset: 0x000002AE
		public char start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020B6 File Offset: 0x000002B6
		public char end
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020BE File Offset: 0x000002BE
		private CharacterRange(char start, char end, IEnumerable<char> excludes)
		{
			this._start = start;
			this._end = end;
			this._excludes = ((excludes == null) ? new HashSet<char>() : new HashSet<char>(excludes));
		}

		// Token: 0x04000001 RID: 1
		private char _start;

		// Token: 0x04000002 RID: 2
		private char _end;

		// Token: 0x04000003 RID: 3
		private ICollection<char> _excludes;

		// Token: 0x04000004 RID: 4
		private CharacterSet _correspondingCharSet = new CharacterSet();
	}
}
