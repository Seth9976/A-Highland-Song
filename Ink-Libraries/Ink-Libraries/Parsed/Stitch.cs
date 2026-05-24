using System;
using System.Collections.Generic;

namespace Ink.Parsed
{
	// Token: 0x0200005E RID: 94
	public class Stitch : FlowBase
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00018A50 File Offset: 0x00016C50
		public override FlowLevel flowLevel
		{
			get
			{
				return FlowLevel.Stitch;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00018A53 File Offset: 0x00016C53
		public Stitch(Identifier name, List<Object> topLevelObjects, List<FlowBase.Argument> arguments, bool isFunction)
			: base(name, topLevelObjects, arguments, isFunction, false)
		{
		}
	}
}
