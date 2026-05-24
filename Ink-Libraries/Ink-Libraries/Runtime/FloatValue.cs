using System;
using System.Globalization;

namespace Ink.Runtime
{
	// Token: 0x02000035 RID: 53
	public class FloatValue : Value<float>
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0001301F File Offset: 0x0001121F
		public override ValueType valueType
		{
			get
			{
				return ValueType.Float;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00013022 File Offset: 0x00011222
		public override bool isTruthy
		{
			get
			{
				return base.value != 0f;
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00013034 File Offset: 0x00011234
		public FloatValue(float val)
			: base(val)
		{
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0001303D File Offset: 0x0001123D
		public FloatValue()
			: this(0f)
		{
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001304C File Offset: 0x0001124C
		public override Value Cast(ValueType newType)
		{
			if (newType == this.valueType)
			{
				return this;
			}
			if (newType == ValueType.Bool)
			{
				return new BoolValue(base.value != 0f);
			}
			if (newType == ValueType.Int)
			{
				return new IntValue((int)base.value);
			}
			if (newType == ValueType.String)
			{
				return new StringValue(base.value.ToString(CultureInfo.InvariantCulture) ?? "");
			}
			throw base.BadCastException(newType);
		}
	}
}
