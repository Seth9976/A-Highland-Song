using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002DB RID: 731
	public class UxmlTypeAttributeDescription<TBase> : TypedUxmlAttributeDescription<Type>
	{
		// Token: 0x06001829 RID: 6185 RVA: 0x00061015 File Offset: 0x0005F215
		public UxmlTypeAttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = null;
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x00061040 File Offset: 0x0005F240
		public override string defaultValueAsString
		{
			get
			{
				return (base.defaultValue == null) ? "null" : base.defaultValue.FullName;
			}
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0006106C File Offset: 0x0005F26C
		public override Type GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<Type>(bag, cc, (string s, Type type1) => this.ConvertValueToType(s, type1), base.defaultValue);
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00061098 File Offset: 0x0005F298
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref Type value)
		{
			return base.TryGetValueFromBag<Type>(bag, cc, (string s, Type type1) => this.ConvertValueToType(s, type1), base.defaultValue, ref value);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x000610C8 File Offset: 0x0005F2C8
		private Type ConvertValueToType(string v, Type defaultValue)
		{
			bool flag = string.IsNullOrEmpty(v);
			Type type;
			if (flag)
			{
				type = defaultValue;
			}
			else
			{
				try
				{
					Type type2 = Type.GetType(v, true);
					bool flag2 = !typeof(TBase).IsAssignableFrom(type2);
					if (!flag2)
					{
						return type2;
					}
					Debug.LogError(string.Concat(new string[]
					{
						"Type: Invalid type \"",
						v,
						"\". Type must derive from ",
						typeof(TBase).FullName,
						"."
					}));
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				type = defaultValue;
			}
			return type;
		}
	}
}
