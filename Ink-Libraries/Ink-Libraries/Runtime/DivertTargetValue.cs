using System;

namespace Ink.Runtime
{
	// Token: 0x02000037 RID: 55
	public class DivertTargetValue : Value<Path>
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000131D8 File Offset: 0x000113D8
		// (set) Token: 0x06000345 RID: 837 RVA: 0x000131E0 File Offset: 0x000113E0
		public Path targetPath
		{
			get
			{
				return base.value;
			}
			set
			{
				base.value = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000131E9 File Offset: 0x000113E9
		public override ValueType valueType
		{
			get
			{
				return ValueType.DivertTarget;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000347 RID: 839 RVA: 0x000131EC File Offset: 0x000113EC
		public override bool isTruthy
		{
			get
			{
				throw new Exception("Shouldn't be checking the truthiness of a divert target");
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000131F8 File Offset: 0x000113F8
		public DivertTargetValue(Path targetPath)
			: base(targetPath)
		{
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00013201 File Offset: 0x00011401
		public DivertTargetValue()
			: base(null)
		{
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0001320A File Offset: 0x0001140A
		public override Value Cast(ValueType newType)
		{
			if (newType == this.valueType)
			{
				return this;
			}
			throw base.BadCastException(newType);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0001321E File Offset: 0x0001141E
		public override string ToString()
		{
			string text = "DivertTargetValue(";
			Path targetPath = this.targetPath;
			return text + ((targetPath != null) ? targetPath.ToString() : null) + ")";
		}
	}
}
