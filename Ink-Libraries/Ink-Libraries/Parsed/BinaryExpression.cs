using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000047 RID: 71
	public class BinaryExpression : Expression
	{
		// Token: 0x060003FC RID: 1020 RVA: 0x00015A50 File Offset: 0x00013C50
		public BinaryExpression(Expression left, Expression right, string opName)
		{
			this.leftExpression = base.AddContent<Expression>(left);
			this.rightExpression = base.AddContent<Expression>(right);
			this.opName = opName;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00015A79 File Offset: 0x00013C79
		public override void GenerateIntoContainer(Container container)
		{
			this.leftExpression.GenerateIntoContainer(container);
			this.rightExpression.GenerateIntoContainer(container);
			this.opName = this.NativeNameForOp(this.opName);
			container.AddContent(NativeFunctionCall.CallWithName(this.opName));
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00015AB8 File Offset: 0x00013CB8
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.NativeNameForOp(this.opName) == "?")
			{
				UnaryExpression unaryExpression = this.leftExpression as UnaryExpression;
				if (unaryExpression != null && (unaryExpression.op == "not" || unaryExpression.op == "!"))
				{
					string text = "Using 'not' or '!' here negates '";
					Expression innerExpression = unaryExpression.innerExpression;
					this.Error(text + ((innerExpression != null) ? innerExpression.ToString() : null) + "' rather than the result of the '?' or 'has' operator. You need to add parentheses around the (A ? B) expression.", null, false);
				}
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00015B48 File Offset: 0x00013D48
		private string NativeNameForOp(string opName)
		{
			if (opName == "and")
			{
				return "&&";
			}
			if (opName == "or")
			{
				return "||";
			}
			if (opName == "mod")
			{
				return "%";
			}
			if (opName == "has")
			{
				return "?";
			}
			if (opName == "hasnt")
			{
				return "!?";
			}
			return opName;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00015BB5 File Offset: 0x00013DB5
		public override string ToString()
		{
			return string.Format("({0} {1} {2})", this.leftExpression, this.opName, this.rightExpression);
		}

		// Token: 0x04000142 RID: 322
		public Expression leftExpression;

		// Token: 0x04000143 RID: 323
		public Expression rightExpression;

		// Token: 0x04000144 RID: 324
		public string opName;
	}
}
