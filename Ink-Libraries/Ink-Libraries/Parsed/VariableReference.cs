using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000064 RID: 100
	public class VariableReference : Expression
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00019E4C File Offset: 0x0001804C
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00019E54 File Offset: 0x00018054
		public string name { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00019E60 File Offset: 0x00018060
		public Identifier identifier
		{
			get
			{
				if (this.pathIdentifiers == null || this.pathIdentifiers.Count == 0)
				{
					return null;
				}
				if (this._singleIdentifier == null)
				{
					string text = string.Join(".", this.path.ToArray());
					DebugMetadata debugMetadata = this.pathIdentifiers.First<Identifier>().debugMetadata;
					DebugMetadata debugMetadata2 = this.pathIdentifiers.Aggregate(debugMetadata, (DebugMetadata acc, Identifier id) => acc.Merge(id.debugMetadata));
					this._singleIdentifier = new Identifier
					{
						name = text,
						debugMetadata = debugMetadata2
					};
				}
				return this._singleIdentifier;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00019EFE File Offset: 0x000180FE
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00019F06 File Offset: 0x00018106
		public List<string> path { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00019F0F File Offset: 0x0001810F
		public VariableReference runtimeVarRef
		{
			get
			{
				return this._runtimeVarRef;
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00019F18 File Offset: 0x00018118
		public VariableReference(List<Identifier> pathIdentifiers)
		{
			this.pathIdentifiers = pathIdentifiers;
			this.path = pathIdentifiers.Select(delegate(Identifier id)
			{
				if (id == null)
				{
					return null;
				}
				return id.name;
			}).ToList<string>();
			this.name = string.Join<Identifier>(".", pathIdentifiers);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00019F74 File Offset: 0x00018174
		public override void GenerateIntoContainer(Container container)
		{
			Expression expression = null;
			if (base.story.constants.TryGetValue(this.name, out expression))
			{
				expression.GenerateConstantIntoContainer(container);
				this.isConstantReference = true;
				return;
			}
			this._runtimeVarRef = new VariableReference(this.name);
			if (this.path.Count == 1 || this.path.Count == 2)
			{
				string text = null;
				string text2;
				if (this.path.Count == 1)
				{
					text2 = this.path[0];
				}
				else
				{
					text = this.path[0];
					text2 = this.path[1];
				}
				if (base.story.ResolveListItem(text, text2, this))
				{
					this.isListItemReference = true;
				}
			}
			container.AddContent(this._runtimeVarRef);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001A03C File Offset: 0x0001823C
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.isConstantReference || this.isListItemReference)
			{
				return;
			}
			Path path = new Path(this.pathIdentifiers);
			Object @object = path.ResolveFromContext(this);
			if (@object)
			{
				@object.containerForCounting.visitsShouldBeCounted = true;
				if (this._runtimeVarRef == null)
				{
					return;
				}
				this._runtimeVarRef.pathForCount = @object.runtimePath;
				this._runtimeVarRef.name = null;
				FlowBase flowBase = @object as FlowBase;
				if (flowBase && flowBase.isFunction && (base.parent is Weave || base.parent is ContentList || base.parent is FlowBase))
				{
					string[] array = new string[5];
					array[0] = "'";
					int num = 1;
					Identifier identifier = flowBase.identifier;
					array[num] = ((identifier != null) ? identifier.ToString() : null);
					array[2] = "' being used as read count rather than being called as function. Perhaps you intended to write ";
					array[3] = flowBase.name;
					array[4] = "()";
					base.Warning(string.Concat(array), null);
				}
				return;
			}
			else
			{
				if (this.path.Count > 1)
				{
					string text = "Could not find target for read count: ";
					Path path2 = path;
					string text2 = text + ((path2 != null) ? path2.ToString() : null);
					if (this.path.Count <= 2)
					{
						text2 = text2 + ", or couldn't find list item with the name " + string.Join(",", this.path.ToArray());
					}
					this.Error(text2, null, false);
					return;
				}
				if (!context.ResolveVariableWithName(this.name, this).found)
				{
					this.Error("Unresolved variable: " + this.ToString(), this, false);
				}
				return;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001A1CA File Offset: 0x000183CA
		public override string ToString()
		{
			return string.Join(".", this.path.ToArray());
		}

		// Token: 0x04000193 RID: 403
		private Identifier _singleIdentifier;

		// Token: 0x04000194 RID: 404
		public List<Identifier> pathIdentifiers;

		// Token: 0x04000196 RID: 406
		public bool isConstantReference;

		// Token: 0x04000197 RID: 407
		public bool isListItemReference;

		// Token: 0x04000198 RID: 408
		private VariableReference _runtimeVarRef;
	}
}
