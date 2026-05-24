using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200004E RID: 78
	public class FunctionCall : Expression
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00016C94 File Offset: 0x00014E94
		public string name
		{
			get
			{
				return this._proxyDivert.target.firstComponent;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00016CA6 File Offset: 0x00014EA6
		public Divert proxyDivert
		{
			get
			{
				return this._proxyDivert;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00016CAE File Offset: 0x00014EAE
		public List<Expression> arguments
		{
			get
			{
				return this._proxyDivert.arguments;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00016CBB File Offset: 0x00014EBB
		public Divert runtimeDivert
		{
			get
			{
				return this._proxyDivert.runtimeDivert;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00016CC8 File Offset: 0x00014EC8
		public bool isChoiceCount
		{
			get
			{
				return this.name == "CHOICE_COUNT";
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00016CDA File Offset: 0x00014EDA
		public bool isTurns
		{
			get
			{
				return this.name == "TURNS";
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00016CEC File Offset: 0x00014EEC
		public bool isTurnsSince
		{
			get
			{
				return this.name == "TURNS_SINCE";
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00016CFE File Offset: 0x00014EFE
		public bool isRandom
		{
			get
			{
				return this.name == "RANDOM";
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00016D10 File Offset: 0x00014F10
		public bool isSeedRandom
		{
			get
			{
				return this.name == "SEED_RANDOM";
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00016D22 File Offset: 0x00014F22
		public bool isListRange
		{
			get
			{
				return this.name == "LIST_RANGE";
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00016D34 File Offset: 0x00014F34
		public bool isListRandom
		{
			get
			{
				return this.name == "LIST_RANDOM";
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00016D46 File Offset: 0x00014F46
		public bool isReadCount
		{
			get
			{
				return this.name == "READ_COUNT";
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00016D58 File Offset: 0x00014F58
		public FunctionCall(Identifier functionName, List<Expression> arguments)
		{
			this._proxyDivert = new Divert(new Path(functionName), arguments);
			this._proxyDivert.isFunctionCall = true;
			base.AddContent<Divert>(this._proxyDivert);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00016D8C File Offset: 0x00014F8C
		public override void GenerateIntoContainer(Container container)
		{
			ListDefinition listDefinition = base.story.ResolveList(this.name);
			bool flag = false;
			if (this.isChoiceCount)
			{
				if (this.arguments.Count > 0)
				{
					this.Error("The CHOICE_COUNT() function shouldn't take any arguments", null, false);
				}
				container.AddContent(ControlCommand.ChoiceCount());
			}
			else if (this.isTurns)
			{
				if (this.arguments.Count > 0)
				{
					this.Error("The TURNS() function shouldn't take any arguments", null, false);
				}
				container.AddContent(ControlCommand.Turns());
			}
			else if (this.isTurnsSince || this.isReadCount)
			{
				DivertTarget divertTarget = this.arguments[0] as DivertTarget;
				VariableReference variableReference = this.arguments[0] as VariableReference;
				if (this.arguments.Count != 1 || (divertTarget == null && variableReference == null))
				{
					this.Error("The " + this.name + "() function should take one argument: a divert target to the target knot, stitch, gather or choice you want to check. e.g. TURNS_SINCE(-> myKnot)", null, false);
					return;
				}
				if (divertTarget)
				{
					this._divertTargetToCount = divertTarget;
					base.AddContent<DivertTarget>(this._divertTargetToCount);
					this._divertTargetToCount.GenerateIntoContainer(container);
				}
				else
				{
					this._variableReferenceToCount = variableReference;
					base.AddContent<VariableReference>(this._variableReferenceToCount);
					this._variableReferenceToCount.GenerateIntoContainer(container);
				}
				if (this.isTurnsSince)
				{
					container.AddContent(ControlCommand.TurnsSince());
				}
				else
				{
					container.AddContent(ControlCommand.ReadCount());
				}
			}
			else if (this.isRandom)
			{
				if (this.arguments.Count != 2)
				{
					this.Error("RANDOM should take 2 parameters: a minimum and a maximum integer", null, false);
				}
				for (int i = 0; i < this.arguments.Count; i++)
				{
					if (this.arguments[i] is Number && !((this.arguments[i] as Number).value is int))
					{
						string text = ((i == 0) ? "minimum" : "maximum");
						this.Error("RANDOM's " + text + " parameter should be an integer", null, false);
					}
					this.arguments[i].GenerateIntoContainer(container);
				}
				container.AddContent(ControlCommand.Random());
			}
			else if (this.isSeedRandom)
			{
				if (this.arguments.Count != 1)
				{
					this.Error("SEED_RANDOM should take 1 parameter - an integer seed", null, false);
				}
				Number number = this.arguments[0] as Number;
				if (number && !(number.value is int))
				{
					this.Error("SEED_RANDOM's parameter should be an integer seed", null, false);
				}
				this.arguments[0].GenerateIntoContainer(container);
				container.AddContent(ControlCommand.SeedRandom());
			}
			else if (this.isListRange)
			{
				if (this.arguments.Count != 3)
				{
					this.Error("LIST_RANGE should take 3 parameters - a list, a min and a max", null, false);
				}
				for (int j = 0; j < this.arguments.Count; j++)
				{
					this.arguments[j].GenerateIntoContainer(container);
				}
				container.AddContent(ControlCommand.ListRange());
			}
			else if (this.isListRandom)
			{
				if (this.arguments.Count != 1)
				{
					this.Error("LIST_RANDOM should take 1 parameter - a list", null, false);
				}
				this.arguments[0].GenerateIntoContainer(container);
				container.AddContent(ControlCommand.ListRandom());
			}
			else if (NativeFunctionCall.CallExistsWithName(this.name))
			{
				NativeFunctionCall nativeFunctionCall = NativeFunctionCall.CallWithName(this.name);
				if (nativeFunctionCall.numberOfParameters != this.arguments.Count)
				{
					string text2 = this.name + " should take " + nativeFunctionCall.numberOfParameters.ToString() + " parameter";
					if (nativeFunctionCall.numberOfParameters > 1)
					{
						text2 += "s";
					}
					this.Error(text2, null, false);
				}
				for (int k = 0; k < this.arguments.Count; k++)
				{
					this.arguments[k].GenerateIntoContainer(container);
				}
				container.AddContent(NativeFunctionCall.CallWithName(this.name));
			}
			else if (listDefinition != null)
			{
				if (this.arguments.Count > 1)
				{
					this.Error("Can currently only construct a list from one integer (or an empty list from a given list definition)", null, false);
				}
				if (this.arguments.Count == 1)
				{
					container.AddContent(new StringValue(this.name));
					this.arguments[0].GenerateIntoContainer(container);
					container.AddContent(ControlCommand.ListFromInt());
				}
				else
				{
					InkList inkList = new InkList();
					inkList.SetInitialOriginName(this.name);
					container.AddContent(new ListValue(inkList));
				}
			}
			else
			{
				container.AddContent(this._proxyDivert.runtimeObject);
				flag = true;
			}
			if (!flag)
			{
				base.content.Remove(this._proxyDivert);
			}
			if (this.shouldPopReturnedValue)
			{
				container.AddContent(ControlCommand.PopEvaluatedValue());
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001725C File Offset: 0x0001545C
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			if (!base.content.Contains(this._proxyDivert) && this.arguments != null)
			{
				foreach (Expression expression in this.arguments)
				{
					expression.ResolveReferences(context);
				}
			}
			if (this._divertTargetToCount)
			{
				Divert divert = this._divertTargetToCount.divert;
				bool flag = divert.runtimeDivert.variableDivertName != null;
				if (flag)
				{
					this.Error("When getting the TURNS_SINCE() of a variable target, remove the '->' - i.e. it should just be TURNS_SINCE(" + divert.runtimeDivert.variableDivertName + ")", null, false);
					return;
				}
				Object targetContent = divert.targetContent;
				if (!(targetContent == null))
				{
					targetContent.containerForCounting.turnIndexShouldBeCounted = true;
					return;
				}
				if (!flag)
				{
					string text = "Failed to find target for TURNS_SINCE: '";
					Path target = divert.target;
					this.Error(text + ((target != null) ? target.ToString() : null) + "'", null, false);
					return;
				}
			}
			else if (this._variableReferenceToCount && this._variableReferenceToCount.runtimeVarRef.pathForCount != null)
			{
				this.Error(string.Concat(new string[]
				{
					"Should be ",
					this.name,
					"(-> ",
					this._variableReferenceToCount.name,
					"). Usage without the '->' only makes sense for variable targets."
				}), null, false);
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000173D0 File Offset: 0x000155D0
		public static bool IsBuiltIn(string name)
		{
			return NativeFunctionCall.CallExistsWithName(name) || name == "CHOICE_COUNT" || name == "TURNS_SINCE" || name == "TURNS" || name == "RANDOM" || name == "SEED_RANDOM" || name == "LIST_VALUE" || name == "LIST_RANDOM" || name == "READ_COUNT";
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00017450 File Offset: 0x00015650
		public override string ToString()
		{
			string text = string.Join(", ", this.arguments.ToStringsArray<Expression>());
			return string.Format("{0}({1})", this.name, text);
		}

		// Token: 0x0400015B RID: 347
		public bool shouldPopReturnedValue;

		// Token: 0x0400015C RID: 348
		private Divert _proxyDivert;

		// Token: 0x0400015D RID: 349
		private DivertTarget _divertTargetToCount;

		// Token: 0x0400015E RID: 350
		private VariableReference _variableReferenceToCount;
	}
}
