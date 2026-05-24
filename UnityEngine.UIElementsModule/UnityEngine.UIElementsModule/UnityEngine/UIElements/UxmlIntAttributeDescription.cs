using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020002D3 RID: 723
	public class UxmlIntAttributeDescription : TypedUxmlAttributeDescription<int>
	{
		// Token: 0x06001805 RID: 6149 RVA: 0x00060B93 File Offset: 0x0005ED93
		public UxmlIntAttributeDescription()
		{
			base.type = "int";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0;
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001806 RID: 6150 RVA: 0x00060BC0 File Offset: 0x0005EDC0
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00060BEC File Offset: 0x0005EDEC
		public override int GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<int>(bag, cc, (string s, int i) => UxmlIntAttributeDescription.ConvertValueToInt(s, i), base.defaultValue);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00060C2C File Offset: 0x0005EE2C
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref int value)
		{
			return base.TryGetValueFromBag<int>(bag, cc, (string s, int i) => UxmlIntAttributeDescription.ConvertValueToInt(s, i), base.defaultValue, ref value);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00060C6C File Offset: 0x0005EE6C
		private static int ConvertValueToInt(string v, int defaultValue)
		{
			int num;
			bool flag = v == null || !int.TryParse(v, ref num);
			int num2;
			if (flag)
			{
				num2 = defaultValue;
			}
			else
			{
				num2 = num;
			}
			return num2;
		}
	}
}
