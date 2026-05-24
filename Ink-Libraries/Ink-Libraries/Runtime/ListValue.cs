using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x02000039 RID: 57
	public class ListValue : Value<InkList>
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000357 RID: 855 RVA: 0x000132CA File Offset: 0x000114CA
		public override ValueType valueType
		{
			get
			{
				return ValueType.List;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000358 RID: 856 RVA: 0x000132CD File Offset: 0x000114CD
		public override bool isTruthy
		{
			get
			{
				return base.value.Count > 0;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000132E0 File Offset: 0x000114E0
		public override Value Cast(ValueType newType)
		{
			if (newType == ValueType.Int)
			{
				KeyValuePair<InkListItem, int> maxItem = base.value.maxItem;
				if (maxItem.Key.isNull)
				{
					return new IntValue(0);
				}
				return new IntValue(maxItem.Value);
			}
			else if (newType == ValueType.Float)
			{
				KeyValuePair<InkListItem, int> maxItem2 = base.value.maxItem;
				if (maxItem2.Key.isNull)
				{
					return new FloatValue(0f);
				}
				return new FloatValue((float)maxItem2.Value);
			}
			else if (newType == ValueType.String)
			{
				KeyValuePair<InkListItem, int> maxItem3 = base.value.maxItem;
				if (maxItem3.Key.isNull)
				{
					return new StringValue("");
				}
				return new StringValue(maxItem3.Key.ToString());
			}
			else
			{
				if (newType == this.valueType)
				{
					return this;
				}
				throw base.BadCastException(newType);
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000133B4 File Offset: 0x000115B4
		public ListValue()
			: base(null)
		{
			base.value = new InkList();
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000133C8 File Offset: 0x000115C8
		public ListValue(InkList list)
			: base(null)
		{
			base.value = new InkList(list);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000133DD File Offset: 0x000115DD
		public ListValue(InkListItem singleItem, int singleValue)
			: base(null)
		{
			base.value = new InkList { { singleItem, singleValue } };
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000133FC File Offset: 0x000115FC
		public static void RetainListOriginsForAssignment(Object oldValue, Object newValue)
		{
			ListValue listValue = oldValue as ListValue;
			ListValue listValue2 = newValue as ListValue;
			if (listValue && listValue2 && listValue2.value.Count == 0)
			{
				listValue2.value.SetInitialOriginNames(listValue.value.originNames);
			}
		}
	}
}
