using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200004A RID: 74
	public class MultipleConditionExpression : Expression
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00015FBC File Offset: 0x000141BC
		public List<Expression> subExpressions
		{
			get
			{
				return base.content.Cast<Expression>().ToList<Expression>();
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00015FCE File Offset: 0x000141CE
		public MultipleConditionExpression(List<Expression> conditionExpressions)
		{
			base.AddContent<Expression>(conditionExpressions);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00015FE0 File Offset: 0x000141E0
		public override void GenerateIntoContainer(Container container)
		{
			bool flag = true;
			foreach (Expression expression in this.subExpressions)
			{
				expression.GenerateIntoContainer(container);
				if (!flag)
				{
					container.AddContent(NativeFunctionCall.CallWithName("&&"));
				}
				flag = false;
			}
		}
	}
}
