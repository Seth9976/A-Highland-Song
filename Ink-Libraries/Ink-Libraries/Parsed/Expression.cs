using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000046 RID: 70
	public abstract class Expression : Object
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001596D File Offset: 0x00013B6D
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00015975 File Offset: 0x00013B75
		public bool outputWhenComplete { get; set; }

		// Token: 0x060003F8 RID: 1016 RVA: 0x00015980 File Offset: 0x00013B80
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			container.AddContent(ControlCommand.EvalStart());
			this.GenerateIntoContainer(container);
			if (this.outputWhenComplete)
			{
				container.AddContent(ControlCommand.EvalOutput());
			}
			container.AddContent(ControlCommand.EvalEnd());
			return container;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000159C4 File Offset: 0x00013BC4
		public void GenerateConstantIntoContainer(Container container)
		{
			if (this._prototypeRuntimeConstantExpression == null)
			{
				this._prototypeRuntimeConstantExpression = new Container();
				this.GenerateIntoContainer(this._prototypeRuntimeConstantExpression);
			}
			foreach (Object @object in this._prototypeRuntimeConstantExpression.content)
			{
				container.AddContent(@object.Copy());
			}
		}

		// Token: 0x060003FA RID: 1018
		public abstract void GenerateIntoContainer(Container container);

		// Token: 0x04000141 RID: 321
		private Container _prototypeRuntimeConstantExpression;
	}
}
