using System;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x02000018 RID: 24
	public class Divert : Object
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000089A0 File Offset: 0x00006BA0
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000089EB File Offset: 0x00006BEB
		public Path targetPath
		{
			get
			{
				if (this._targetPath != null && this._targetPath.isRelative)
				{
					Object @object = this.targetPointer.Resolve();
					if (@object)
					{
						this._targetPath = @object.path;
					}
				}
				return this._targetPath;
			}
			set
			{
				this._targetPath = value;
				this._targetPointer = Pointer.Null;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00008A00 File Offset: 0x00006C00
		public Pointer targetPointer
		{
			get
			{
				if (this._targetPointer.isNull)
				{
					Object obj = base.ResolvePath(this._targetPath).obj;
					if (this._targetPath.lastComponent.isIndex)
					{
						this._targetPointer.container = obj.parent as Container;
						this._targetPointer.index = this._targetPath.lastComponent.index;
					}
					else
					{
						this._targetPointer = Pointer.StartOf(obj as Container);
					}
				}
				return this._targetPointer;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00008A88 File Offset: 0x00006C88
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00008AA0 File Offset: 0x00006CA0
		public string targetPathString
		{
			get
			{
				if (this.targetPath == null)
				{
					return null;
				}
				return base.CompactPathString(this.targetPath);
			}
			set
			{
				if (value == null)
				{
					this.targetPath = null;
					return;
				}
				this.targetPath = new Path(value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00008AB9 File Offset: 0x00006CB9
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00008AC1 File Offset: 0x00006CC1
		public string variableDivertName { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00008ACA File Offset: 0x00006CCA
		public bool hasVariableTarget
		{
			get
			{
				return this.variableDivertName != null;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00008AD5 File Offset: 0x00006CD5
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00008ADD File Offset: 0x00006CDD
		public bool pushesToStack { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00008AE6 File Offset: 0x00006CE6
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00008AEE File Offset: 0x00006CEE
		public bool isExternal { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00008AF7 File Offset: 0x00006CF7
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00008AFF File Offset: 0x00006CFF
		public int externalArgs { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00008B08 File Offset: 0x00006D08
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00008B10 File Offset: 0x00006D10
		public bool isConditional { get; set; }

		// Token: 0x06000184 RID: 388 RVA: 0x00008B19 File Offset: 0x00006D19
		public Divert()
		{
			this.pushesToStack = false;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008B28 File Offset: 0x00006D28
		public Divert(PushPopType stackPushType)
		{
			this.pushesToStack = true;
			this.stackPushType = stackPushType;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008B40 File Offset: 0x00006D40
		public override bool Equals(object obj)
		{
			Divert divert = obj as Divert;
			if (!divert || this.hasVariableTarget != divert.hasVariableTarget)
			{
				return false;
			}
			if (this.hasVariableTarget)
			{
				return this.variableDivertName == divert.variableDivertName;
			}
			return this.targetPath.Equals(divert.targetPath);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008B97 File Offset: 0x00006D97
		public override int GetHashCode()
		{
			if (this.hasVariableTarget)
			{
				return this.variableDivertName.GetHashCode() + 12345;
			}
			return this.targetPath.GetHashCode() + 54321;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008BC4 File Offset: 0x00006DC4
		public override string ToString()
		{
			if (this.hasVariableTarget)
			{
				return "Divert(variable: " + this.variableDivertName + ")";
			}
			if (this.targetPath == null)
			{
				return "Divert(null)";
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text = this.targetPath.ToString();
			int? num = base.DebugLineNumberOfPath(this.targetPath);
			if (num != null)
			{
				string text2 = "line ";
				int? num2 = num;
				text = text2 + num2.ToString();
			}
			stringBuilder.Append("Divert");
			if (this.isConditional)
			{
				stringBuilder.Append("?");
			}
			if (this.pushesToStack)
			{
				if (this.stackPushType == PushPopType.Function)
				{
					stringBuilder.Append(" function");
				}
				else
				{
					stringBuilder.Append(" tunnel");
				}
			}
			stringBuilder.Append(" -> ");
			stringBuilder.Append(this.targetPathString);
			stringBuilder.Append(" (");
			stringBuilder.Append(text);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04000066 RID: 102
		private Path _targetPath;

		// Token: 0x04000067 RID: 103
		private Pointer _targetPointer;

		// Token: 0x0400006A RID: 106
		public PushPopType stackPushType;
	}
}
