using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020002DC RID: 732
	public class UxmlEnumAttributeDescription<T> : TypedUxmlAttributeDescription<T> where T : struct, IConvertible
	{
		// Token: 0x06001830 RID: 6192 RVA: 0x0006117C File Offset: 0x0005F37C
		public UxmlEnumAttributeDescription()
		{
			bool flag = !typeof(T).IsEnum;
			if (flag)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = new T();
			UxmlEnumeration uxmlEnumeration = new UxmlEnumeration();
			List<string> list = new List<string>();
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				T t = (T)((object)obj);
				list.Add(t.ToString(CultureInfo.InvariantCulture));
			}
			uxmlEnumeration.values = list;
			base.restriction = uxmlEnumeration;
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x00061264 File Offset: 0x0005F464
		public override string defaultValueAsString
		{
			get
			{
				T defaultValue = base.defaultValue;
				return defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00061294 File Offset: 0x0005F494
		public override T GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<T>(bag, cc, (string s, T convertible) => UxmlEnumAttributeDescription<T>.ConvertValueToEnum<T>(s, convertible), base.defaultValue);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000612D4 File Offset: 0x0005F4D4
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref T value)
		{
			return base.TryGetValueFromBag<T>(bag, cc, (string s, T convertible) => UxmlEnumAttributeDescription<T>.ConvertValueToEnum<T>(s, convertible), base.defaultValue, ref value);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00061314 File Offset: 0x0005F514
		private static U ConvertValueToEnum<U>(string v, U defaultValue)
		{
			bool flag = v == null || !Enum.IsDefined(typeof(U), v);
			U u;
			if (flag)
			{
				u = defaultValue;
			}
			else
			{
				U u2 = (U)((object)Enum.Parse(typeof(U), v));
				u = u2;
			}
			return u;
		}
	}
}
