using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200026D RID: 621
	public struct CustomStyleProperty<T> : IEquatable<CustomStyleProperty<T>>
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001332 RID: 4914 RVA: 0x00052BC7 File Offset: 0x00050DC7
		// (set) Token: 0x06001333 RID: 4915 RVA: 0x00052BCF File Offset: 0x00050DCF
		public string name { readonly get; private set; }

		// Token: 0x06001334 RID: 4916 RVA: 0x00052BD8 File Offset: 0x00050DD8
		public CustomStyleProperty(string propertyName)
		{
			bool flag = !string.IsNullOrEmpty(propertyName) && !propertyName.StartsWith("--");
			if (flag)
			{
				throw new ArgumentException("Custom style property \"" + propertyName + "\" must start with \"--\" prefix.");
			}
			this.name = propertyName;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00052C24 File Offset: 0x00050E24
		public override bool Equals(object obj)
		{
			bool flag = !(obj is CustomStyleProperty<T>);
			return !flag && this.Equals((CustomStyleProperty<T>)obj);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00052C58 File Offset: 0x00050E58
		public bool Equals(CustomStyleProperty<T> other)
		{
			return this.name == other.name;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00052C7C File Offset: 0x00050E7C
		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00052C9C File Offset: 0x00050E9C
		public static bool operator ==(CustomStyleProperty<T> a, CustomStyleProperty<T> b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00052CB8 File Offset: 0x00050EB8
		public static bool operator !=(CustomStyleProperty<T> a, CustomStyleProperty<T> b)
		{
			return !(a == b);
		}
	}
}
