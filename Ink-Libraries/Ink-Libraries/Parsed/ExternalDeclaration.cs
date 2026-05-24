using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200004B RID: 75
	public class ExternalDeclaration : Object, INamedContent
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00016048 File Offset: 0x00014248
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0001605B File Offset: 0x0001425B
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00016063 File Offset: 0x00014263
		public Identifier identifier { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0001606C File Offset: 0x0001426C
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00016074 File Offset: 0x00014274
		public List<string> argumentNames { get; set; }

		// Token: 0x06000414 RID: 1044 RVA: 0x0001607D File Offset: 0x0001427D
		public ExternalDeclaration(Identifier identifier, List<string> argumentNames)
		{
			this.identifier = identifier;
			this.argumentNames = argumentNames;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00016093 File Offset: 0x00014293
		public override Object GenerateRuntimeObject()
		{
			base.story.AddExternal(this);
			return null;
		}
	}
}
