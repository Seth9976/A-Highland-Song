using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200004F RID: 79
	public class Gather : Object, IWeavePoint, INamedContent
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00017484 File Offset: 0x00015684
		public string name
		{
			get
			{
				Identifier identifier = this.identifier;
				if (identifier == null)
				{
					return null;
				}
				return identifier.name;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00017497 File Offset: 0x00015697
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0001749F File Offset: 0x0001569F
		public Identifier identifier { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000174A8 File Offset: 0x000156A8
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x000174B0 File Offset: 0x000156B0
		public int indentationDepth { get; protected set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000174B9 File Offset: 0x000156B9
		public Container runtimeContainer
		{
			get
			{
				return (Container)base.runtimeObject;
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000174C6 File Offset: 0x000156C6
		public Gather(Identifier identifier, int indentationDepth)
		{
			this.identifier = identifier;
			this.indentationDepth = indentationDepth;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000174DC File Offset: 0x000156DC
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			container.name = this.name;
			if (base.story.countAllVisits)
			{
				container.visitsShouldBeCounted = true;
			}
			container.countingAtStartOnly = true;
			if (base.content != null)
			{
				foreach (Object @object in base.content)
				{
					container.AddContent(@object.runtimeObject);
				}
			}
			return container;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001756C File Offset: 0x0001576C
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.identifier != null && this.identifier.name.Length > 0)
			{
				context.CheckForNamingCollisions(this, this.identifier, Story.SymbolType.SubFlowAndWeave, null);
			}
		}
	}
}
