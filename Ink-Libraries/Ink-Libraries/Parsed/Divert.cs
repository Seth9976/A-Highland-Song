using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000044 RID: 68
	public class Divert : Object
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00014B40 File Offset: 0x00012D40
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x00014B48 File Offset: 0x00012D48
		public Path target { get; protected set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00014B51 File Offset: 0x00012D51
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x00014B59 File Offset: 0x00012D59
		public Object targetContent { get; protected set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00014B62 File Offset: 0x00012D62
		// (set) Token: 0x060003DA RID: 986 RVA: 0x00014B6A File Offset: 0x00012D6A
		public List<Expression> arguments { get; protected set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00014B73 File Offset: 0x00012D73
		// (set) Token: 0x060003DC RID: 988 RVA: 0x00014B7B File Offset: 0x00012D7B
		public Divert runtimeDivert { get; protected set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00014B84 File Offset: 0x00012D84
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00014B8C File Offset: 0x00012D8C
		public bool isFunctionCall { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00014B95 File Offset: 0x00012D95
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00014B9D File Offset: 0x00012D9D
		public bool isEmpty { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00014BA6 File Offset: 0x00012DA6
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x00014BAE File Offset: 0x00012DAE
		public bool isTunnel { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00014BB7 File Offset: 0x00012DB7
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x00014BBF File Offset: 0x00012DBF
		public bool isThread { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public bool isEnd
		{
			get
			{
				return this.target != null && this.target.dotSeparatedComponents == "END";
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00014BE9 File Offset: 0x00012DE9
		public bool isDone
		{
			get
			{
				return this.target != null && this.target.dotSeparatedComponents == "DONE";
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014C0A File Offset: 0x00012E0A
		public Divert(Path target, List<Expression> arguments = null)
		{
			this.target = target;
			this.arguments = arguments;
			if (arguments != null)
			{
				base.AddContent<Object>(arguments.Cast<Object>().ToList<Object>());
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00014C34 File Offset: 0x00012E34
		public Divert(Object targetContent)
		{
			this.targetContent = targetContent;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00014C44 File Offset: 0x00012E44
		public override Object GenerateRuntimeObject()
		{
			if (this.isEnd)
			{
				return ControlCommand.End();
			}
			if (this.isDone)
			{
				return ControlCommand.Done();
			}
			this.runtimeDivert = new Divert();
			this.ResolveTargetContent();
			this.CheckArgumentValidity();
			bool flag = this.arguments != null && this.arguments.Count > 0;
			if (flag || this.isFunctionCall || this.isTunnel || this.isThread)
			{
				Container container = new Container();
				if (flag)
				{
					if (!this.isFunctionCall)
					{
						container.AddContent(ControlCommand.EvalStart());
					}
					List<FlowBase.Argument> list = null;
					if (this.targetContent)
					{
						list = (this.targetContent as FlowBase).arguments;
					}
					for (int i = 0; i < this.arguments.Count; i++)
					{
						Expression expression = this.arguments[i];
						FlowBase.Argument argument = null;
						if (list != null && i < list.Count)
						{
							argument = list[i];
						}
						if (argument != null && argument.isByReference)
						{
							VariableReference variableReference = expression as VariableReference;
							if (variableReference == null)
							{
								string text = "Expected variable name to pass by reference to 'ref ";
								Identifier identifier = argument.identifier;
								this.Error(text + ((identifier != null) ? identifier.ToString() : null) + "' but saw " + expression.ToString(), null, false);
								break;
							}
							Path path = new Path(variableReference.pathIdentifiers);
							if (path.ResolveFromContext(this) != null)
							{
								this.Error(string.Concat(new string[]
								{
									"can't pass a read count by reference. '",
									path.dotSeparatedComponents,
									"' is a knot/stitch/label, but '",
									this.target.dotSeparatedComponents,
									"' requires the name of a VAR to be passed."
								}), null, false);
								break;
							}
							VariablePointerValue variablePointerValue = new VariablePointerValue(variableReference.name, -1);
							container.AddContent(variablePointerValue);
						}
						else
						{
							expression.GenerateIntoContainer(container);
						}
					}
					if (!this.isFunctionCall)
					{
						container.AddContent(ControlCommand.EvalEnd());
					}
				}
				if (this.isThread)
				{
					container.AddContent(ControlCommand.StartThread());
				}
				else if (this.isFunctionCall || this.isTunnel)
				{
					this.runtimeDivert.pushesToStack = true;
					this.runtimeDivert.stackPushType = (this.isFunctionCall ? PushPopType.Function : PushPopType.Tunnel);
				}
				container.AddContent(this.runtimeDivert);
				return container;
			}
			return this.runtimeDivert;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00014E8C File Offset: 0x0001308C
		public string PathAsVariableName()
		{
			return this.target.firstComponent;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00014E9C File Offset: 0x0001309C
		private void ResolveTargetContent()
		{
			if (this.isEmpty || this.isEnd)
			{
				return;
			}
			if (this.targetContent == null)
			{
				string variableTargetName = this.PathAsVariableName();
				if (variableTargetName != null)
				{
					FlowBase.VariableResolveResult variableResolveResult = base.ClosestFlowBase().ResolveVariableWithName(variableTargetName, this);
					if (variableResolveResult.found)
					{
						if (variableResolveResult.isArgument)
						{
							FlowBase.Argument argument = variableResolveResult.ownerFlow.arguments.Where((FlowBase.Argument a) => a.identifier.name == variableTargetName).First<FlowBase.Argument>();
							if (!argument.isDivertTarget)
							{
								string[] array = new string[6];
								array[0] = "Since '";
								int num = 1;
								Identifier identifier = argument.identifier;
								array[num] = ((identifier != null) ? identifier.ToString() : null);
								array[2] = "' is used as a variable divert target (on ";
								int num2 = 3;
								DebugMetadata debugMetadata = base.debugMetadata;
								array[num2] = ((debugMetadata != null) ? debugMetadata.ToString() : null);
								array[4] = "), it should be marked as: -> ";
								int num3 = 5;
								Identifier identifier2 = argument.identifier;
								array[num3] = ((identifier2 != null) ? identifier2.ToString() : null);
								this.Error(string.Concat(array), variableResolveResult.ownerFlow, false);
							}
						}
						this.runtimeDivert.variableDivertName = variableTargetName;
						return;
					}
				}
				this.targetContent = this.target.ResolveFromContext(this);
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00014FD0 File Offset: 0x000131D0
		public override void ResolveReferences(Story context)
		{
			if (this.isEmpty || this.isEnd || this.isDone)
			{
				return;
			}
			if (this.targetContent)
			{
				this.runtimeDivert.targetPath = this.targetContent.runtimePath;
			}
			base.ResolveReferences(context);
			FlowBase flowBase = this.targetContent as FlowBase;
			if (flowBase)
			{
				if (!flowBase.isFunction && this.isFunctionCall)
				{
					Identifier identifier = flowBase.identifier;
					string text = ((identifier != null) ? identifier.ToString() : null);
					string text2 = " hasn't been marked as a function, but it's being called as one. Do you need to delcare the knot as '== function ";
					Identifier identifier2 = flowBase.identifier;
					base.Error(text + text2 + ((identifier2 != null) ? identifier2.ToString() : null) + " =='?", null, false);
				}
				else if (flowBase.isFunction && !this.isFunctionCall && !(base.parent is DivertTarget))
				{
					Identifier identifier3 = flowBase.identifier;
					string text3 = ((identifier3 != null) ? identifier3.ToString() : null);
					string text4 = " can't be diverted to. It can only be called as a function since it's been marked as such: '";
					Identifier identifier4 = flowBase.identifier;
					base.Error(text3 + text4 + ((identifier4 != null) ? identifier4.ToString() : null) + "(...)'", null, false);
				}
			}
			bool flag = this.targetContent != null;
			bool flag2 = false;
			bool flag3 = false;
			if (this.target.numberOfComponents == 1)
			{
				flag2 = FunctionCall.IsBuiltIn(this.target.firstComponent);
				flag3 = context.IsExternal(this.target.firstComponent);
				if (flag2 || flag3)
				{
					if (!this.isFunctionCall)
					{
						base.Error(this.target.firstComponent + " must be called as a function: ~ " + this.target.firstComponent + "()", null, false);
					}
					if (flag3)
					{
						this.runtimeDivert.isExternal = true;
						if (this.arguments != null)
						{
							this.runtimeDivert.externalArgs = this.arguments.Count;
						}
						this.runtimeDivert.pushesToStack = false;
						this.runtimeDivert.targetPath = new Path(this.target.firstComponent);
						this.CheckExternalArgumentValidity(context);
					}
					return;
				}
			}
			if (this.runtimeDivert.variableDivertName != null)
			{
				return;
			}
			if (!flag && !flag2 && !flag3)
			{
				string text5 = "target not found: '";
				Path target = this.target;
				this.Error(text5 + ((target != null) ? target.ToString() : null) + "'", null, false);
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000151FC File Offset: 0x000133FC
		private void CheckArgumentValidity()
		{
			if (this.isEmpty)
			{
				return;
			}
			int num = 0;
			if (this.arguments != null && this.arguments.Count > 0)
			{
				num = this.arguments.Count;
			}
			if (this.targetContent == null)
			{
				return;
			}
			FlowBase flowBase = this.targetContent as FlowBase;
			if (num == 0 && (flowBase == null || !flowBase.hasParameters))
			{
				return;
			}
			if (flowBase == null && num > 0)
			{
				this.Error("target needs to be a knot or stitch in order to pass arguments", null, false);
				return;
			}
			if (flowBase.arguments == null && num > 0)
			{
				this.Error("target (" + flowBase.name + ") doesn't take parameters", null, false);
				return;
			}
			if (base.parent is DivertTarget)
			{
				if (num > 0)
				{
					this.Error("can't store arguments in a divert target variable", null, false);
				}
				return;
			}
			int count = flowBase.arguments.Count;
			if (count != num)
			{
				string text;
				if (num == 0)
				{
					text = "but there weren't any passed to it";
				}
				else if (num < count)
				{
					text = "but only got " + num.ToString();
				}
				else
				{
					text = "but got " + num.ToString();
				}
				string[] array = new string[6];
				array[0] = "to '";
				int num2 = 1;
				Identifier identifier = flowBase.identifier;
				array[num2] = ((identifier != null) ? identifier.ToString() : null);
				array[2] = "' requires ";
				array[3] = count.ToString();
				array[4] = " arguments, ";
				array[5] = text;
				this.Error(string.Concat(array), null, false);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				FlowBase.Argument argument = flowBase.arguments[i];
				Expression expression = this.arguments[i];
				if (argument.isDivertTarget)
				{
					VariableReference variableReference = expression as VariableReference;
					if (!(expression is DivertTarget) && variableReference == null)
					{
						string[] array2 = new string[6];
						array2[0] = "Target '";
						int num3 = 1;
						Identifier identifier2 = flowBase.identifier;
						array2[num3] = ((identifier2 != null) ? identifier2.ToString() : null);
						array2[2] = "' expects a divert target for the parameter named -> ";
						int num4 = 3;
						Identifier identifier3 = argument.identifier;
						array2[num4] = ((identifier3 != null) ? identifier3.ToString() : null);
						array2[4] = " but saw ";
						int num5 = 5;
						Expression expression2 = expression;
						array2[num5] = ((expression2 != null) ? expression2.ToString() : null);
						this.Error(string.Concat(array2), expression, false);
					}
					else if (variableReference != null)
					{
						Path path = new Path(variableReference.pathIdentifiers);
						if (path.ResolveFromContext(variableReference) != null)
						{
							string[] array3 = new string[5];
							array3[0] = "Passing read count of '";
							array3[1] = path.dotSeparatedComponents;
							array3[2] = "' instead of a divert target. You probably meant '";
							int num6 = 3;
							Path path2 = path;
							array3[num6] = ((path2 != null) ? path2.ToString() : null);
							array3[4] = "'";
							this.Error(string.Concat(array3), null, false);
						}
					}
				}
			}
			if (flowBase == null)
			{
				this.Error("Can't call as a function or with arguments unless it's a knot or stitch", null, false);
				return;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000154B0 File Offset: 0x000136B0
		private void CheckExternalArgumentValidity(Story context)
		{
			string firstComponent = this.target.firstComponent;
			ExternalDeclaration externalDeclaration = null;
			context.externals.TryGetValue(firstComponent, out externalDeclaration);
			int count = externalDeclaration.argumentNames.Count;
			int num = 0;
			if (this.arguments != null)
			{
				num = this.arguments.Count;
			}
			if (num != count)
			{
				this.Error(string.Concat(new string[]
				{
					"incorrect number of arguments sent to external function '",
					firstComponent,
					"'. Expected ",
					count.ToString(),
					" but got ",
					num.ToString()
				}), null, false);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015544 File Offset: 0x00013744
		public override void Error(string message, Object source = null, bool isWarning = false)
		{
			if (source != this && source)
			{
				base.Error(message, source, false);
				return;
			}
			if (this.isFunctionCall)
			{
				base.Error("Function call " + message, source, isWarning);
				return;
			}
			base.Error("Divert " + message, source, isWarning);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001559B File Offset: 0x0001379B
		public override string ToString()
		{
			if (this.target != null)
			{
				return this.target.ToString();
			}
			return "-> <empty divert>";
		}
	}
}
