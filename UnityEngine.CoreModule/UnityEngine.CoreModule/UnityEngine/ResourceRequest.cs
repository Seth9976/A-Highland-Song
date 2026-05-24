using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001E7 RID: 487
	[RequiredByNativeCode]
	[StructLayout(0)]
	public class ResourceRequest : AsyncOperation
	{
		// Token: 0x0600160D RID: 5645 RVA: 0x00023594 File Offset: 0x00021794
		protected virtual Object GetResult()
		{
			return Resources.Load(this.m_Path, this.m_Type);
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x000235B8 File Offset: 0x000217B8
		public Object asset
		{
			get
			{
				return this.GetResult();
			}
		}

		// Token: 0x040007C5 RID: 1989
		internal string m_Path;

		// Token: 0x040007C6 RID: 1990
		internal Type m_Type;
	}
}
