using System;

namespace Ink.Runtime
{
	// Token: 0x02000033 RID: 51
	public class BoolValue : Value<bool>
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00012EF1 File Offset: 0x000110F1
		public override ValueType valueType
		{
			get
			{
				return ValueType.Bool;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00012EF4 File Offset: 0x000110F4
		public override bool isTruthy
		{
			get
			{
				return base.value;
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00012EFC File Offset: 0x000110FC
		public BoolValue(bool boolVal)
			: base(boolVal)
		{
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00012F05 File Offset: 0x00011105
		public BoolValue()
			: this(false)
		{
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00012F10 File Offset: 0x00011110
		public override Value Cast(ValueType newType)
		{
			if (newType == this.valueType)
			{
				return this;
			}
			if (newType == ValueType.Int)
			{
				return new IntValue(base.value ? 1 : 0);
			}
			if (newType == ValueType.Float)
			{
				return new FloatValue(base.value ? 1f : 0f);
			}
			if (newType == ValueType.String)
			{
				return new StringValue(base.value ? "true" : "false");
			}
			throw base.BadCastException(newType);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00012F80 File Offset: 0x00011180
		public override string ToString()
		{
			if (!base.value)
			{
				return "false";
			}
			return "true";
		}
	}
}
