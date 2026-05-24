using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002D9 RID: 729
	public class UxmlColorAttributeDescription : TypedUxmlAttributeDescription<Color>
	{
		// Token: 0x06001820 RID: 6176 RVA: 0x00060EDC File Offset: 0x0005F0DC
		public UxmlColorAttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = new Color(0f, 0f, 0f, 1f);
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x00060F2C File Offset: 0x0005F12C
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString();
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00060F54 File Offset: 0x0005F154
		public override Color GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<Color>(bag, cc, (string s, Color color) => UxmlColorAttributeDescription.ConvertValueToColor(s, color), base.defaultValue);
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00060F94 File Offset: 0x0005F194
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref Color value)
		{
			return base.TryGetValueFromBag<Color>(bag, cc, (string s, Color color) => UxmlColorAttributeDescription.ConvertValueToColor(s, color), base.defaultValue, ref value);
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00060FD4 File Offset: 0x0005F1D4
		private static Color ConvertValueToColor(string v, Color defaultValue)
		{
			Color color;
			bool flag = v == null || !ColorUtility.TryParseHtmlString(v, out color);
			Color color2;
			if (flag)
			{
				color2 = defaultValue;
			}
			else
			{
				color2 = color;
			}
			return color2;
		}
	}
}
