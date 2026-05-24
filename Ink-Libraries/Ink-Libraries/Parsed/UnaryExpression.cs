using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000048 RID: 72
	public class UnaryExpression : Expression
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x00015BD4 File Offset: 0x00013DD4
		public static Expression WithInner(Expression inner, string op)
		{
			Number number = inner as Number;
			if (number)
			{
				if (op == "-")
				{
					if (number.value is int)
					{
						return new Number(-(int)number.value);
					}
					if (number.value is float)
					{
						return new Number(-(float)number.value);
					}
				}
				else if (op == "!" || op == "not")
				{
					if (number.value is int)
					{
						return new Number((int)number.value == 0);
					}
					if (number.value is float)
					{
						return new Number((float)number.value == 0f);
					}
					if (number.value is bool)
					{
						return new Number(!(bool)number.value);
					}
				}
				throw new Exception("Unexpected operation or number type");
			}
			return new UnaryExpression(inner, op);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00015CED File Offset: 0x00013EED
		public UnaryExpression(Expression inner, string op)
		{
			this.innerExpression = base.AddContent<Expression>(inner);
			this.op = op;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00015D09 File Offset: 0x00013F09
		public override void GenerateIntoContainer(Container container)
		{
			this.innerExpression.GenerateIntoContainer(container);
			container.AddContent(NativeFunctionCall.CallWithName(this.nativeNameForOp));
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00015D28 File Offset: 0x00013F28
		public override string ToString()
		{
			string nativeNameForOp = this.nativeNameForOp;
			Expression expression = this.innerExpression;
			return nativeNameForOp + ((expression != null) ? expression.ToString() : null);
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00015D47 File Offset: 0x00013F47
		private string nativeNameForOp
		{
			get
			{
				if (this.op == "-")
				{
					return "_";
				}
				if (this.op == "not")
				{
					return "!";
				}
				return this.op;
			}
		}

		// Token: 0x04000145 RID: 325
		public Expression innerExpression;

		// Token: 0x04000146 RID: 326
		public string op;
	}
}
