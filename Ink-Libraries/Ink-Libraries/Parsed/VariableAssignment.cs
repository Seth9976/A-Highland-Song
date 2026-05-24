using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000063 RID: 99
	public class VariableAssignment : Object
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00019B92 File Offset: 0x00017D92
		public string variableName
		{
			get
			{
				return this.variableIdentifier.name;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00019B9F File Offset: 0x00017D9F
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00019BA7 File Offset: 0x00017DA7
		public Identifier variableIdentifier { get; protected set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00019BB0 File Offset: 0x00017DB0
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x00019BB8 File Offset: 0x00017DB8
		public Expression expression { get; protected set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00019BC1 File Offset: 0x00017DC1
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00019BC9 File Offset: 0x00017DC9
		public ListDefinition listDefinition { get; protected set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00019BD2 File Offset: 0x00017DD2
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x00019BDA File Offset: 0x00017DDA
		public bool isGlobalDeclaration { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00019BE3 File Offset: 0x00017DE3
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x00019BEB File Offset: 0x00017DEB
		public bool isNewTemporaryDeclaration { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00019BF4 File Offset: 0x00017DF4
		public bool isDeclaration
		{
			get
			{
				return this.isGlobalDeclaration || this.isNewTemporaryDeclaration;
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00019C06 File Offset: 0x00017E06
		public VariableAssignment(Identifier identifier, Expression assignedExpression)
		{
			this.variableIdentifier = identifier;
			if (assignedExpression)
			{
				this.expression = base.AddContent<Expression>(assignedExpression);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00019C2A File Offset: 0x00017E2A
		public VariableAssignment(Identifier identifier, ListDefinition listDef)
		{
			this.variableIdentifier = identifier;
			if (listDef)
			{
				this.listDefinition = base.AddContent<ListDefinition>(listDef);
				this.listDefinition.variableAssignment = this;
			}
			this.isGlobalDeclaration = true;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00019C64 File Offset: 0x00017E64
		public override Object GenerateRuntimeObject()
		{
			FlowBase flowBase = null;
			if (this.isGlobalDeclaration)
			{
				flowBase = base.story;
			}
			else if (this.isNewTemporaryDeclaration)
			{
				flowBase = base.ClosestFlowBase();
			}
			if (flowBase)
			{
				flowBase.TryAddNewVariableDeclaration(this);
			}
			if (this.isGlobalDeclaration)
			{
				return null;
			}
			Container container = new Container();
			if (this.expression != null)
			{
				container.AddContent(this.expression.runtimeObject);
			}
			else if (this.listDefinition != null)
			{
				container.AddContent(this.listDefinition.runtimeObject);
			}
			this._runtimeAssignment = new VariableAssignment(this.variableName, this.isNewTemporaryDeclaration);
			container.AddContent(this._runtimeAssignment);
			return container;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00019D18 File Offset: 0x00017F18
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.isDeclaration && this.listDefinition == null)
			{
				context.CheckForNamingCollisions(this, this.variableIdentifier, this.isGlobalDeclaration ? Story.SymbolType.Var : Story.SymbolType.Temp, null);
			}
			if (this.isGlobalDeclaration)
			{
				VariableReference variableReference = this.expression as VariableReference;
				if (variableReference && !variableReference.isConstantReference && !variableReference.isListItemReference)
				{
					this.Error("global variable assignments cannot refer to other variables, only literal values, constants and list items", null, false);
				}
			}
			if (!this.isNewTemporaryDeclaration)
			{
				FlowBase.VariableResolveResult variableResolveResult = context.ResolveVariableWithName(this.variableName, this);
				if (!variableResolveResult.found)
				{
					if (base.story.constants.ContainsKey(this.variableName))
					{
						this.Error("Can't re-assign to a constant (do you need to use VAR when declaring '" + this.variableName + "'?)", this, false);
					}
					else
					{
						this.Error("Variable could not be found to assign to: '" + this.variableName + "'", this, false);
					}
				}
				if (this._runtimeAssignment != null)
				{
					this._runtimeAssignment.isGlobal = variableResolveResult.isGlobal;
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00019E29 File Offset: 0x00018029
		public override string typeName
		{
			get
			{
				if (this.isNewTemporaryDeclaration)
				{
					return "temp";
				}
				if (this.isGlobalDeclaration)
				{
					return "VAR";
				}
				return "variable assignment";
			}
		}

		// Token: 0x04000191 RID: 401
		private VariableAssignment _runtimeAssignment;
	}
}
