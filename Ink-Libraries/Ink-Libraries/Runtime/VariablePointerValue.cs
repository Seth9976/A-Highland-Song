using System;

namespace Ink.Runtime
{
	// Token: 0x02000038 RID: 56
	public class VariablePointerValue : Value<string>
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00013241 File Offset: 0x00011441
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00013249 File Offset: 0x00011449
		public string variableName
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00013252 File Offset: 0x00011452
		public override ValueType valueType
		{
			get
			{
				return ValueType.VariablePointer;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00013255 File Offset: 0x00011455
		public override bool isTruthy
		{
			get
			{
				throw new Exception("Shouldn't be checking the truthiness of a variable pointer");
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00013261 File Offset: 0x00011461
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00013269 File Offset: 0x00011469
		public int contextIndex { get; set; }

		// Token: 0x06000352 RID: 850 RVA: 0x00013272 File Offset: 0x00011472
		public VariablePointerValue(string variableName, int contextIndex = -1)
			: base(variableName)
		{
			this.contextIndex = contextIndex;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00013282 File Offset: 0x00011482
		public VariablePointerValue()
			: this(null, -1)
		{
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0001328C File Offset: 0x0001148C
		public override Value Cast(ValueType newType)
		{
			if (newType == this.valueType)
			{
				return this;
			}
			throw base.BadCastException(newType);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000132A0 File Offset: 0x000114A0
		public override string ToString()
		{
			return "VariablePointerValue(" + this.variableName + ")";
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000132B7 File Offset: 0x000114B7
		public override Object Copy()
		{
			return new VariablePointerValue(this.variableName, this.contextIndex);
		}
	}
}
