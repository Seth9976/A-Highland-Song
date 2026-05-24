using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000053 RID: 83
	public interface IWeavePoint
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000451 RID: 1105
		int indentationDepth { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000452 RID: 1106
		Container runtimeContainer { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000453 RID: 1107
		List<Object> content { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000454 RID: 1108
		string name { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000455 RID: 1109
		Identifier identifier { get; }
	}
}
