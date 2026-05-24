using System;
using System.Globalization;

namespace Ink.Runtime
{
	// Token: 0x02000036 RID: 54
	public class StringValue : Value<string>
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600033A RID: 826 RVA: 0x000130BC File Offset: 0x000112BC
		public override ValueType valueType
		{
			get
			{
				return ValueType.String;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600033B RID: 827 RVA: 0x000130BF File Offset: 0x000112BF
		public override bool isTruthy
		{
			get
			{
				return base.value.Length > 0;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000130CF File Offset: 0x000112CF
		// (set) Token: 0x0600033D RID: 829 RVA: 0x000130D7 File Offset: 0x000112D7
		public bool isNewline { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600033E RID: 830 RVA: 0x000130E0 File Offset: 0x000112E0
		// (set) Token: 0x0600033F RID: 831 RVA: 0x000130E8 File Offset: 0x000112E8
		public bool isInlineWhitespace { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000340 RID: 832 RVA: 0x000130F1 File Offset: 0x000112F1
		public bool isNonWhitespace
		{
			get
			{
				return !this.isNewline && !this.isInlineWhitespace;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00013108 File Offset: 0x00011308
		public StringValue(string str)
			: base(str)
		{
			this.isNewline = base.value == "\n";
			this.isInlineWhitespace = true;
			foreach (char c in base.value)
			{
				if (c != ' ' && c != '\t')
				{
					this.isInlineWhitespace = false;
					return;
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0001316B File Offset: 0x0001136B
		public StringValue()
			: this("")
		{
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00013178 File Offset: 0x00011378
		public override Value Cast(ValueType newType)
		{
			if (newType == this.valueType)
			{
				return this;
			}
			if (newType == ValueType.Int)
			{
				int num;
				if (int.TryParse(base.value, out num))
				{
					return new IntValue(num);
				}
				return null;
			}
			else
			{
				if (newType != ValueType.Float)
				{
					throw base.BadCastException(newType);
				}
				float num2;
				if (float.TryParse(base.value, NumberStyles.Float, CultureInfo.InvariantCulture, out num2))
				{
					return new FloatValue(num2);
				}
				return null;
			}
		}
	}
}
