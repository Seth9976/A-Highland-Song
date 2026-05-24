using System;

namespace Ink.Runtime
{
	// Token: 0x02000028 RID: 40
	public struct SearchResult
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000D7E9 File Offset: 0x0000B9E9
		public Object correctObj
		{
			get
			{
				if (!this.approximate)
				{
					return this.obj;
				}
				return null;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000D7FB File Offset: 0x0000B9FB
		public Container container
		{
			get
			{
				return this.obj as Container;
			}
		}

		// Token: 0x040000C2 RID: 194
		public Object obj;

		// Token: 0x040000C3 RID: 195
		public bool approximate;
	}
}
