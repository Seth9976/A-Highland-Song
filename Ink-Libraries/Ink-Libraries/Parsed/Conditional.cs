using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000040 RID: 64
	public class Conditional : Object
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060003AC RID: 940 RVA: 0x000144A4 File Offset: 0x000126A4
		// (set) Token: 0x060003AD RID: 941 RVA: 0x000144AC File Offset: 0x000126AC
		public Expression initialCondition { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003AE RID: 942 RVA: 0x000144B5 File Offset: 0x000126B5
		// (set) Token: 0x060003AF RID: 943 RVA: 0x000144BD File Offset: 0x000126BD
		public List<ConditionalSingleBranch> branches { get; private set; }

		// Token: 0x060003B0 RID: 944 RVA: 0x000144C8 File Offset: 0x000126C8
		public Conditional(Expression condition, List<ConditionalSingleBranch> branches)
		{
			this.initialCondition = condition;
			if (this.initialCondition)
			{
				base.AddContent<Expression>(condition);
			}
			this.branches = branches;
			if (this.branches != null)
			{
				base.AddContent<Object>(this.branches.Cast<Object>().ToList<Object>());
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001451C File Offset: 0x0001271C
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			if (this.initialCondition)
			{
				container.AddContent(this.initialCondition.runtimeObject);
			}
			foreach (ConditionalSingleBranch conditionalSingleBranch in this.branches)
			{
				Container container2 = (Container)conditionalSingleBranch.runtimeObject;
				container.AddContent(container2);
			}
			if (this.initialCondition != null && this.branches[0].ownExpression != null && !this.branches[this.branches.Count - 1].isElse)
			{
				container.AddContent(ControlCommand.PopEvaluatedValue());
			}
			this._reJoinTarget = ControlCommand.NoOp();
			container.AddContent(this._reJoinTarget);
			return container;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00014608 File Offset: 0x00012808
		public override void ResolveReferences(Story context)
		{
			Path path = this._reJoinTarget.path;
			foreach (ConditionalSingleBranch conditionalSingleBranch in this.branches)
			{
				conditionalSingleBranch.returnDivert.targetPath = path;
			}
			base.ResolveReferences(context);
		}

		// Token: 0x04000128 RID: 296
		private ControlCommand _reJoinTarget;
	}
}
