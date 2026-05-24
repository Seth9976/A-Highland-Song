using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000049 RID: 73
	public class IncDecExpression : Expression
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x00015D7F File Offset: 0x00013F7F
		public IncDecExpression(Identifier varIdentifier, bool isInc)
		{
			this.varIdentifier = varIdentifier;
			this.isInc = isInc;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00015D95 File Offset: 0x00013F95
		public IncDecExpression(Identifier varIdentifier, Expression expression, bool isInc)
			: this(varIdentifier, isInc)
		{
			this.expression = expression;
			base.AddContent<Expression>(expression);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00015DB0 File Offset: 0x00013FB0
		public override void GenerateIntoContainer(Container container)
		{
			Identifier identifier = this.varIdentifier;
			container.AddContent(new VariableReference((identifier != null) ? identifier.name : null));
			if (this.expression)
			{
				this.expression.GenerateIntoContainer(container);
			}
			else
			{
				container.AddContent(new IntValue(1));
			}
			container.AddContent(NativeFunctionCall.CallWithName(this.isInc ? "+" : "-"));
			Identifier identifier2 = this.varIdentifier;
			this._runtimeAssignment = new VariableAssignment((identifier2 != null) ? identifier2.name : null, false);
			container.AddContent(this._runtimeAssignment);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00015E4C File Offset: 0x0001404C
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			Identifier identifier = this.varIdentifier;
			FlowBase.VariableResolveResult variableResolveResult = context.ResolveVariableWithName((identifier != null) ? identifier.name : null, this);
			if (!variableResolveResult.found)
			{
				string[] array = new string[6];
				array[0] = "variable for ";
				array[1] = this.incrementDecrementWord;
				array[2] = " could not be found: '";
				int num = 3;
				Identifier identifier2 = this.varIdentifier;
				array[num] = ((identifier2 != null) ? identifier2.ToString() : null);
				array[4] = "' after searching: ";
				array[5] = base.descriptionOfScope;
				this.Error(string.Concat(array), null, false);
			}
			this._runtimeAssignment.isGlobal = variableResolveResult.isGlobal;
			if (!(base.parent is Weave) && !(base.parent is FlowBase) && !(base.parent is ContentList))
			{
				this.Error("Can't use " + this.incrementDecrementWord + " as sub-expression", null, false);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00015F29 File Offset: 0x00014129
		private string incrementDecrementWord
		{
			get
			{
				if (this.isInc)
				{
					return "increment";
				}
				return "decrement";
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015F40 File Offset: 0x00014140
		public override string ToString()
		{
			if (this.expression)
			{
				Identifier identifier = this.varIdentifier;
				return ((identifier != null) ? identifier.ToString() : null) + (this.isInc ? " += " : " -= ") + this.expression.ToString();
			}
			Identifier identifier2 = this.varIdentifier;
			return ((identifier2 != null) ? identifier2.ToString() : null) + (this.isInc ? "++" : "--");
		}

		// Token: 0x04000147 RID: 327
		public Identifier varIdentifier;

		// Token: 0x04000148 RID: 328
		public bool isInc;

		// Token: 0x04000149 RID: 329
		public Expression expression;

		// Token: 0x0400014A RID: 330
		private VariableAssignment _runtimeAssignment;
	}
}
