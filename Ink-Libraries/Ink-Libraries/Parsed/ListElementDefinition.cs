using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000057 RID: 87
	public class ListElementDefinition : Object
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00017AEF File Offset: 0x00015CEF
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00017B04 File Offset: 0x00015D04
		public string fullName
		{
			get
			{
				ListDefinition listDefinition = base.parent as ListDefinition;
				if (listDefinition == null)
				{
					throw new Exception("Can't get full name without a parent list");
				}
				Identifier identifier = listDefinition.identifier;
				return ((identifier != null) ? identifier.ToString() : null) + "." + this.name;
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00017B51 File Offset: 0x00015D51
		public ListElementDefinition(Identifier identifier, bool inInitialList, int? explicitValue = null)
		{
			this.identifier = identifier;
			this.inInitialList = inInitialList;
			this.explicitValue = explicitValue;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00017B6E File Offset: 0x00015D6E
		public override Object GenerateRuntimeObject()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00017B75 File Offset: 0x00015D75
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			context.CheckForNamingCollisions(this, this.identifier, Story.SymbolType.ListItem, null);
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00017B8D File Offset: 0x00015D8D
		public override string typeName
		{
			get
			{
				return "List element";
			}
		}

		// Token: 0x0400016A RID: 362
		public Identifier identifier;

		// Token: 0x0400016B RID: 363
		public int? explicitValue;

		// Token: 0x0400016C RID: 364
		public int seriesValue;

		// Token: 0x0400016D RID: 365
		public bool inInitialList;
	}
}
