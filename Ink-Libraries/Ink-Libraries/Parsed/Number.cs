using System;
using System.Globalization;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000058 RID: 88
	public class Number : Expression
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x00017B94 File Offset: 0x00015D94
		public Number(object value)
		{
			if (value is int || value is float || value is bool)
			{
				this.value = value;
				return;
			}
			throw new Exception("Unexpected object type in Number");
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00017BC8 File Offset: 0x00015DC8
		public override void GenerateIntoContainer(Container container)
		{
			if (this.value is int)
			{
				container.AddContent(new IntValue((int)this.value));
				return;
			}
			if (this.value is float)
			{
				container.AddContent(new FloatValue((float)this.value));
				return;
			}
			if (this.value is bool)
			{
				container.AddContent(new BoolValue((bool)this.value));
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00017C40 File Offset: 0x00015E40
		public override string ToString()
		{
			if (this.value is float)
			{
				return ((float)this.value).ToString(CultureInfo.InvariantCulture);
			}
			return this.value.ToString();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00017C80 File Offset: 0x00015E80
		public override bool Equals(object obj)
		{
			Number number = obj as Number;
			return !(number == null) && this.value.Equals(number.value);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00017CB0 File Offset: 0x00015EB0
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x0400016E RID: 366
		public object value;
	}
}
