using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000062 RID: 98
	public class TunnelOnwards : Object
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0001999E File Offset: 0x00017B9E
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x000199A6 File Offset: 0x00017BA6
		public Divert divertAfter
		{
			get
			{
				return this._divertAfter;
			}
			set
			{
				this._divertAfter = value;
				if (this._divertAfter)
				{
					base.AddContent<Divert>(this._divertAfter);
				}
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000199CC File Offset: 0x00017BCC
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			container.AddContent(ControlCommand.EvalStart());
			if (this.divertAfter)
			{
				Object @object = this.divertAfter.GenerateRuntimeObject();
				Container container2 = @object as Container;
				if (container2)
				{
					List<Expression> arguments = this.divertAfter.arguments;
					if (arguments != null && arguments.Count > 0)
					{
						int num = -1;
						int num2 = -1;
						for (int i = 0; i < container2.content.Count; i++)
						{
							ControlCommand controlCommand = container2.content[i] as ControlCommand;
							if (controlCommand)
							{
								if (num == -1 && controlCommand.commandType == ControlCommand.CommandType.EvalStart)
								{
									num = i;
								}
								else if (controlCommand.commandType == ControlCommand.CommandType.EvalEnd)
								{
									num2 = i;
								}
							}
						}
						for (int j = num + 1; j < num2; j++)
						{
							container2.content[j].parent = null;
							container.AddContent(container2.content[j]);
						}
					}
				}
				Divert divert = @object as Divert;
				if (divert != null && divert.hasVariableTarget)
				{
					VariableReference variableReference = new VariableReference(divert.variableDivertName);
					container.AddContent(variableReference);
				}
				else
				{
					this._overrideDivertTarget = new DivertTargetValue();
					container.AddContent(this._overrideDivertTarget);
				}
			}
			else
			{
				container.AddContent(new Void());
			}
			container.AddContent(ControlCommand.EvalEnd());
			container.AddContent(ControlCommand.PopTunnel());
			return container;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00019B3C File Offset: 0x00017D3C
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (this.divertAfter && this.divertAfter.targetContent)
			{
				this._overrideDivertTarget.targetPath = this.divertAfter.targetContent.runtimePath;
			}
		}

		// Token: 0x0400018A RID: 394
		private Divert _divertAfter;

		// Token: 0x0400018B RID: 395
		private DivertTargetValue _overrideDivertTarget;
	}
}
