using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000045 RID: 69
	public class DivertTarget : Expression
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x000155B6 File Offset: 0x000137B6
		public DivertTarget(Divert divert)
		{
			this.divert = base.AddContent<Divert>(divert);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000155CB File Offset: 0x000137CB
		public override void GenerateIntoContainer(Container container)
		{
			this.divert.GenerateRuntimeObject();
			this._runtimeDivert = this.divert.runtimeDivert;
			this._runtimeDivertTargetValue = new DivertTargetValue();
			container.AddContent(this._runtimeDivertTargetValue);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015604 File Offset: 0x00013804
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.divert.isDone || this.divert.isEnd)
			{
				this.Error("Can't Can't use -> DONE or -> END as variable divert targets", this, false);
				return;
			}
			Object @object = this;
			while (@object && @object is Expression)
			{
				bool flag = false;
				bool flag2 = false;
				Object parent = @object.parent;
				if (parent is BinaryExpression)
				{
					BinaryExpression binaryExpression = parent as BinaryExpression;
					if (binaryExpression.opName != "==" && binaryExpression.opName != "!=")
					{
						flag = true;
					}
					else
					{
						if (!(binaryExpression.leftExpression is DivertTarget) && !(binaryExpression.leftExpression is VariableReference))
						{
							flag = true;
						}
						if (!(binaryExpression.rightExpression is DivertTarget) && !(binaryExpression.rightExpression is VariableReference))
						{
							flag = true;
						}
					}
					flag2 = true;
				}
				else if (parent is FunctionCall)
				{
					FunctionCall functionCall = parent as FunctionCall;
					if (!functionCall.isTurnsSince && !functionCall.isReadCount)
					{
						flag = true;
					}
					flag2 = true;
				}
				else if (parent is Expression)
				{
					flag = true;
					flag2 = true;
				}
				else if (parent is MultipleConditionExpression)
				{
					flag = true;
					flag2 = true;
				}
				else if (parent is Choice && ((Choice)parent).condition == @object)
				{
					flag = true;
					flag2 = true;
				}
				else if (parent is Conditional || parent is ConditionalSingleBranch)
				{
					flag = true;
					flag2 = true;
				}
				if (flag)
				{
					string text = "Can't use a divert target like that. Did you intend to call '";
					Path target = this.divert.target;
					this.Error(text + ((target != null) ? target.ToString() : null) + "' as a function: likeThis(), or check the read count: likeThis, with no arrows?", this, false);
				}
				if (flag2)
				{
					break;
				}
				@object = parent;
			}
			if (this._runtimeDivert.hasVariableTarget)
			{
				this.Error("Since '" + this.divert.target.dotSeparatedComponents + "' is a variable, it shouldn't be preceded by '->' here.", null, false);
			}
			this._runtimeDivertTargetValue.targetPath = this._runtimeDivert.targetPath;
			Object targetContent = this.divert.targetContent;
			if (targetContent != null)
			{
				Container containerForCounting = targetContent.containerForCounting;
				if (containerForCounting != null)
				{
					FunctionCall functionCall2 = base.parent as FunctionCall;
					if (functionCall2 && functionCall2.isTurnsSince)
					{
						containerForCounting.turnIndexShouldBeCounted = true;
					}
					else
					{
						containerForCounting.visitsShouldBeCounted = true;
						containerForCounting.turnIndexShouldBeCounted = true;
					}
				}
				FlowBase flowBase = targetContent as FlowBase;
				if (flowBase != null && flowBase.arguments != null)
				{
					foreach (FlowBase.Argument argument in flowBase.arguments)
					{
						if (argument.isByReference)
						{
							string[] array = new string[5];
							array[0] = "Can't store a divert target to a knot or function that has by-reference arguments ('";
							int num = 1;
							Identifier identifier = flowBase.identifier;
							array[num] = ((identifier != null) ? identifier.ToString() : null);
							array[2] = "' has 'ref ";
							int num2 = 3;
							Identifier identifier2 = argument.identifier;
							array[num2] = ((identifier2 != null) ? identifier2.ToString() : null);
							array[4] = "').";
							this.Error(string.Concat(array), null, false);
						}
					}
				}
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00015910 File Offset: 0x00013B10
		public override bool Equals(object obj)
		{
			DivertTarget divertTarget = obj as DivertTarget;
			if (divertTarget == null)
			{
				return false;
			}
			string dotSeparatedComponents = this.divert.target.dotSeparatedComponents;
			string dotSeparatedComponents2 = divertTarget.divert.target.dotSeparatedComponents;
			return dotSeparatedComponents.Equals(dotSeparatedComponents2);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015956 File Offset: 0x00013B56
		public override int GetHashCode()
		{
			return this.divert.target.dotSeparatedComponents.GetHashCode();
		}

		// Token: 0x0400013D RID: 317
		public Divert divert;

		// Token: 0x0400013E RID: 318
		private DivertTargetValue _runtimeDivertTargetValue;

		// Token: 0x0400013F RID: 319
		private Divert _runtimeDivert;
	}
}
