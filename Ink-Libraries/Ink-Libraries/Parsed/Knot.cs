using System;
using System.Collections.Generic;

namespace Ink.Parsed
{
	// Token: 0x02000054 RID: 84
	public class Knot : FlowBase
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x000175F0 File Offset: 0x000157F0
		public override FlowLevel flowLevel
		{
			get
			{
				return FlowLevel.Knot;
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000175F3 File Offset: 0x000157F3
		public Knot(Identifier name, List<Object> topLevelObjects, List<FlowBase.Argument> arguments, bool isFunction)
			: base(name, topLevelObjects, arguments, isFunction, false)
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00017604 File Offset: 0x00015804
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			Story story = base.story;
			foreach (KeyValuePair<string, FlowBase> keyValuePair in base.subFlowsByName)
			{
				string key = keyValuePair.Key;
				Object @object = story.ContentWithNameAtLevel(key, new FlowLevel?(FlowLevel.Knot), false);
				if (@object)
				{
					FlowBase value = keyValuePair.Value;
					string text = string.Format("Stitch '{0}' has the same name as a knot (on {1})", value.identifier, @object.debugMetadata);
					this.Error(text, value, false);
				}
			}
		}
	}
}
