using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000041 RID: 65
	public class ConditionalSingleBranch : Object
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00014674 File Offset: 0x00012874
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0001467C File Offset: 0x0001287C
		public bool isTrueBranch { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00014685 File Offset: 0x00012885
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0001468D File Offset: 0x0001288D
		public Expression ownExpression
		{
			get
			{
				return this._ownExpression;
			}
			set
			{
				this._ownExpression = value;
				if (this._ownExpression)
				{
					base.AddContent<Expression>(this._ownExpression);
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000146B0 File Offset: 0x000128B0
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x000146B8 File Offset: 0x000128B8
		public bool matchingEquality { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x000146C1 File Offset: 0x000128C1
		// (set) Token: 0x060003BA RID: 954 RVA: 0x000146C9 File Offset: 0x000128C9
		public bool isElse { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060003BB RID: 955 RVA: 0x000146D2 File Offset: 0x000128D2
		// (set) Token: 0x060003BC RID: 956 RVA: 0x000146DA File Offset: 0x000128DA
		public bool isInline { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003BD RID: 957 RVA: 0x000146E3 File Offset: 0x000128E3
		// (set) Token: 0x060003BE RID: 958 RVA: 0x000146EB File Offset: 0x000128EB
		public Divert returnDivert { get; protected set; }

		// Token: 0x060003BF RID: 959 RVA: 0x000146F4 File Offset: 0x000128F4
		public ConditionalSingleBranch(List<Object> content)
		{
			if (content != null)
			{
				this._innerWeave = new Weave(content, -1);
				base.AddContent<Weave>(this._innerWeave);
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001471C File Offset: 0x0001291C
		public override Object GenerateRuntimeObject()
		{
			if (this._innerWeave)
			{
				foreach (Object @object in this._innerWeave.content)
				{
					Text text = @object as Text;
					if (text && text.text.StartsWith("else:"))
					{
						base.Warning("Saw the text 'else:' which is being treated as content. Did you mean '- else:'?", text);
					}
				}
			}
			Container container = new Container();
			bool flag = this.matchingEquality && !this.isElse;
			if (flag)
			{
				container.AddContent(ControlCommand.Duplicate());
			}
			this._conditionalDivert = new Divert();
			this._conditionalDivert.isConditional = !this.isElse;
			if (!this.isTrueBranch && !this.isElse)
			{
				bool flag2 = this.ownExpression != null;
				if (flag2)
				{
					container.AddContent(ControlCommand.EvalStart());
				}
				if (this.ownExpression)
				{
					this.ownExpression.GenerateIntoContainer(container);
				}
				if (this.matchingEquality)
				{
					container.AddContent(NativeFunctionCall.CallWithName("=="));
				}
				if (flag2)
				{
					container.AddContent(ControlCommand.EvalEnd());
				}
			}
			container.AddContent(this._conditionalDivert);
			this._contentContainer = this.GenerateRuntimeForContent();
			this._contentContainer.name = "b";
			if (!this.isInline)
			{
				this._contentContainer.InsertContent(new StringValue("\n"), 0);
			}
			if (flag || (this.isElse && this.matchingEquality))
			{
				this._contentContainer.InsertContent(ControlCommand.PopEvaluatedValue(), 0);
			}
			container.AddToNamedContentOnly(this._contentContainer);
			this.returnDivert = new Divert();
			this._contentContainer.AddContent(this.returnDivert);
			return container;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000148E8 File Offset: 0x00012AE8
		private Container GenerateRuntimeForContent()
		{
			if (this._innerWeave == null)
			{
				return new Container();
			}
			return this._innerWeave.rootContainer;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00014909 File Offset: 0x00012B09
		public override void ResolveReferences(Story context)
		{
			this._conditionalDivert.targetPath = this._contentContainer.path;
			base.ResolveReferences(context);
		}

		// Token: 0x0400012E RID: 302
		private Container _contentContainer;

		// Token: 0x0400012F RID: 303
		private Divert _conditionalDivert;

		// Token: 0x04000130 RID: 304
		private Expression _ownExpression;

		// Token: 0x04000131 RID: 305
		private Weave _innerWeave;
	}
}
