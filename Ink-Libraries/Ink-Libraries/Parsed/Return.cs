using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200005B RID: 91
	public class Return : Object
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001860C File Offset: 0x0001680C
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00018614 File Offset: 0x00016814
		public Expression returnedExpression { get; protected set; }

		// Token: 0x0600049D RID: 1181 RVA: 0x0001861D File Offset: 0x0001681D
		public Return(Expression returnedExpression = null)
		{
			if (returnedExpression)
			{
				this.returnedExpression = base.AddContent<Expression>(returnedExpression);
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001863C File Offset: 0x0001683C
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			if (this.returnedExpression)
			{
				container.AddContent(this.returnedExpression.runtimeObject);
			}
			else
			{
				container.AddContent(ControlCommand.EvalStart());
				container.AddContent(new Void());
				container.AddContent(ControlCommand.EvalEnd());
			}
			container.AddContent(ControlCommand.PopFunction());
			return container;
		}
	}
}
