using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000066 RID: 102
	public class Wrap<T> : Object where T : Object
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x0001AEB4 File Offset: 0x000190B4
		public Wrap(T objToWrap)
		{
			this._objToWrap = objToWrap;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001AEC3 File Offset: 0x000190C3
		public override Object GenerateRuntimeObject()
		{
			return this._objToWrap;
		}

		// Token: 0x040001A4 RID: 420
		private T _objToWrap;
	}
}
