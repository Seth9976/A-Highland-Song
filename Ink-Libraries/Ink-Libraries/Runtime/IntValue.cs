using System;

namespace Ink.Runtime
{
	// Token: 0x02000034 RID: 52
	public class IntValue : Value<int>
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00012F95 File Offset: 0x00011195
		public override ValueType valueType
		{
			get
			{
				return ValueType.Int;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00012F98 File Offset: 0x00011198
		public override bool isTruthy
		{
			get
			{
				return base.value != 0;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00012FA3 File Offset: 0x000111A3
		public IntValue(int intVal)
			: base(intVal)
		{
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00012FAC File Offset: 0x000111AC
		public IntValue()
			: this(0)
		{
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012FB8 File Offset: 0x000111B8
		public override Value Cast(ValueType newType)
		{
			if (newType == this.valueType)
			{
				return this;
			}
			if (newType == ValueType.Bool)
			{
				return new BoolValue(base.value != 0);
			}
			if (newType == ValueType.Float)
			{
				return new FloatValue((float)base.value);
			}
			if (newType == ValueType.String)
			{
				return new StringValue(base.value.ToString() ?? "");
			}
			throw base.BadCastException(newType);
		}
	}
}
