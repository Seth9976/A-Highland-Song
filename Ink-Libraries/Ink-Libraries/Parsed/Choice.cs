using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200003F RID: 63
	public class Choice : Object, IWeavePoint, INamedContent
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00013EB5 File Offset: 0x000120B5
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00013EBD File Offset: 0x000120BD
		public ContentList startContent { get; protected set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00013EC6 File Offset: 0x000120C6
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00013ECE File Offset: 0x000120CE
		public ContentList choiceOnlyContent { get; protected set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00013ED7 File Offset: 0x000120D7
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00013EDF File Offset: 0x000120DF
		public ContentList innerContent { get; protected set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00013EE8 File Offset: 0x000120E8
		public string name
		{
			get
			{
				Identifier identifier = this.identifier;
				if (identifier == null)
				{
					return null;
				}
				return identifier.name;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00013EFB File Offset: 0x000120FB
		// (set) Token: 0x06000399 RID: 921 RVA: 0x00013F03 File Offset: 0x00012103
		public Identifier identifier { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00013F0C File Offset: 0x0001210C
		// (set) Token: 0x0600039B RID: 923 RVA: 0x00013F14 File Offset: 0x00012114
		public Expression condition
		{
			get
			{
				return this._condition;
			}
			set
			{
				this._condition = value;
				if (this._condition)
				{
					base.AddContent<Expression>(this._condition);
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00013F37 File Offset: 0x00012137
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00013F3F File Offset: 0x0001213F
		public bool onceOnly { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00013F48 File Offset: 0x00012148
		// (set) Token: 0x0600039F RID: 927 RVA: 0x00013F50 File Offset: 0x00012150
		public bool isInvisibleDefault { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00013F59 File Offset: 0x00012159
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x00013F61 File Offset: 0x00012161
		public int indentationDepth { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00013F6A File Offset: 0x0001216A
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x00013F72 File Offset: 0x00012172
		public bool hasWeaveStyleInlineBrackets { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00013F7B File Offset: 0x0001217B
		public Container runtimeContainer
		{
			get
			{
				return this._innerContentContainer;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00013F83 File Offset: 0x00012183
		public Container innerContentContainer
		{
			get
			{
				return this._innerContentContainer;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00013F8B File Offset: 0x0001218B
		public override Container containerForCounting
		{
			get
			{
				return this._innerContentContainer;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00013F93 File Offset: 0x00012193
		public override Path runtimePath
		{
			get
			{
				return this._innerContentContainer.path;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00013FA0 File Offset: 0x000121A0
		public Choice(ContentList startContent, ContentList choiceOnlyContent, ContentList innerContent)
		{
			this.startContent = startContent;
			this.choiceOnlyContent = choiceOnlyContent;
			this.innerContent = innerContent;
			this.indentationDepth = 1;
			if (startContent)
			{
				base.AddContent<ContentList>(this.startContent);
			}
			if (choiceOnlyContent)
			{
				base.AddContent<ContentList>(this.choiceOnlyContent);
			}
			if (innerContent)
			{
				base.AddContent<ContentList>(this.innerContent);
			}
			this.onceOnly = true;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00014018 File Offset: 0x00012218
		public override Object GenerateRuntimeObject()
		{
			this._outerContainer = new Container();
			this._runtimeChoice = new ChoicePoint(this.onceOnly);
			this._runtimeChoice.isInvisibleDefault = this.isInvisibleDefault;
			if (this.startContent || this.choiceOnlyContent || this.condition)
			{
				this._outerContainer.AddContent(ControlCommand.EvalStart());
			}
			if (this.startContent)
			{
				this._returnToR1 = new DivertTargetValue();
				this._outerContainer.AddContent(this._returnToR1);
				VariableAssignment variableAssignment = new VariableAssignment("$r", true);
				this._outerContainer.AddContent(variableAssignment);
				this._outerContainer.AddContent(ControlCommand.BeginString());
				this._divertToStartContentOuter = new Divert();
				this._outerContainer.AddContent(this._divertToStartContentOuter);
				this._startContentRuntimeContainer = this.startContent.GenerateRuntimeObject() as Container;
				this._startContentRuntimeContainer.name = "s";
				Divert divert = new Divert();
				divert.variableDivertName = "$r";
				this._startContentRuntimeContainer.AddContent(divert);
				this._outerContainer.AddToNamedContentOnly(this._startContentRuntimeContainer);
				this._r1Label = new Container();
				this._r1Label.name = "$r1";
				this._outerContainer.AddContent(this._r1Label);
				this._outerContainer.AddContent(ControlCommand.EndString());
				this._runtimeChoice.hasStartContent = true;
			}
			if (this.choiceOnlyContent)
			{
				this._outerContainer.AddContent(ControlCommand.BeginString());
				Container container = this.choiceOnlyContent.GenerateRuntimeObject() as Container;
				this._outerContainer.AddContentsOfContainer(container);
				this._outerContainer.AddContent(ControlCommand.EndString());
				this._runtimeChoice.hasChoiceOnlyContent = true;
			}
			if (this.condition)
			{
				this.condition.GenerateIntoContainer(this._outerContainer);
				this._runtimeChoice.hasCondition = true;
			}
			if (this.startContent || this.choiceOnlyContent || this.condition)
			{
				this._outerContainer.AddContent(ControlCommand.EvalEnd());
			}
			this._outerContainer.AddContent(this._runtimeChoice);
			this._innerContentContainer = new Container();
			if (this.startContent)
			{
				this._returnToR2 = new DivertTargetValue();
				this._innerContentContainer.AddContent(ControlCommand.EvalStart());
				this._innerContentContainer.AddContent(this._returnToR2);
				this._innerContentContainer.AddContent(ControlCommand.EvalEnd());
				VariableAssignment variableAssignment2 = new VariableAssignment("$r", true);
				this._innerContentContainer.AddContent(variableAssignment2);
				this._divertToStartContentInner = new Divert();
				this._innerContentContainer.AddContent(this._divertToStartContentInner);
				this._r2Label = new Container();
				this._r2Label.name = "$r2";
				this._innerContentContainer.AddContent(this._r2Label);
			}
			if (this.innerContent)
			{
				Container container2 = this.innerContent.GenerateRuntimeObject() as Container;
				this._innerContentContainer.AddContentsOfContainer(container2);
			}
			if (base.story.countAllVisits)
			{
				this._innerContentContainer.visitsShouldBeCounted = true;
			}
			this._innerContentContainer.countingAtStartOnly = true;
			return this._outerContainer;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001436C File Offset: 0x0001256C
		public override void ResolveReferences(Story context)
		{
			if (this._innerContentContainer)
			{
				this._runtimeChoice.pathOnChoice = this._innerContentContainer.path;
				if (this.onceOnly)
				{
					this._innerContentContainer.visitsShouldBeCounted = true;
				}
			}
			if (this._returnToR1)
			{
				this._returnToR1.targetPath = this._r1Label.path;
			}
			if (this._returnToR2)
			{
				this._returnToR2.targetPath = this._r2Label.path;
			}
			if (this._divertToStartContentOuter)
			{
				this._divertToStartContentOuter.targetPath = this._startContentRuntimeContainer.path;
			}
			if (this._divertToStartContentInner)
			{
				this._divertToStartContentInner.targetPath = this._startContentRuntimeContainer.path;
			}
			base.ResolveReferences(context);
			if (this.identifier != null && this.identifier.name.Length > 0)
			{
				context.CheckForNamingCollisions(this, this.identifier, Story.SymbolType.SubFlowAndWeave, null);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001446D File Offset: 0x0001266D
		public override string ToString()
		{
			if (this.choiceOnlyContent != null)
			{
				return string.Format("* {0}[{1}]...", this.startContent, this.choiceOnlyContent);
			}
			return string.Format("* {0}...", this.startContent);
		}

		// Token: 0x0400011B RID: 283
		private ChoicePoint _runtimeChoice;

		// Token: 0x0400011C RID: 284
		private Container _innerContentContainer;

		// Token: 0x0400011D RID: 285
		private Container _outerContainer;

		// Token: 0x0400011E RID: 286
		private Container _startContentRuntimeContainer;

		// Token: 0x0400011F RID: 287
		private Divert _divertToStartContentOuter;

		// Token: 0x04000120 RID: 288
		private Divert _divertToStartContentInner;

		// Token: 0x04000121 RID: 289
		private Container _r1Label;

		// Token: 0x04000122 RID: 290
		private Container _r2Label;

		// Token: 0x04000123 RID: 291
		private DivertTargetValue _returnToR1;

		// Token: 0x04000124 RID: 292
		private DivertTargetValue _returnToR2;

		// Token: 0x04000125 RID: 293
		private Expression _condition;
	}
}
