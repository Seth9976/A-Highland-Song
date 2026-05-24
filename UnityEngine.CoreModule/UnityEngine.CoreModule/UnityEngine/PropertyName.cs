using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001E3 RID: 483
	[UsedByNativeCode]
	public struct PropertyName : IEquatable<PropertyName>
	{
		// Token: 0x060015E2 RID: 5602 RVA: 0x00023212 File Offset: 0x00021412
		public PropertyName(string name)
		{
			this = new PropertyName(PropertyNameUtils.PropertyNameFromString(name));
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00023222 File Offset: 0x00021422
		public PropertyName(PropertyName other)
		{
			this.id = other.id;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00023231 File Offset: 0x00021431
		public PropertyName(int id)
		{
			this.id = id;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0002323C File Offset: 0x0002143C
		public static bool IsNullOrEmpty(PropertyName prop)
		{
			return prop.id == 0;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00023258 File Offset: 0x00021458
		public static bool operator ==(PropertyName lhs, PropertyName rhs)
		{
			return lhs.id == rhs.id;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00023278 File Offset: 0x00021478
		public static bool operator !=(PropertyName lhs, PropertyName rhs)
		{
			return lhs.id != rhs.id;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0002329C File Offset: 0x0002149C
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x000232B4 File Offset: 0x000214B4
		public override bool Equals(object other)
		{
			return other is PropertyName && this.Equals((PropertyName)other);
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x000232E0 File Offset: 0x000214E0
		public bool Equals(PropertyName other)
		{
			return this == other;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00023300 File Offset: 0x00021500
		public static implicit operator PropertyName(string name)
		{
			return new PropertyName(name);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00023318 File Offset: 0x00021518
		public static implicit operator PropertyName(int id)
		{
			return new PropertyName(id);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00023330 File Offset: 0x00021530
		public override string ToString()
		{
			return string.Format("Unknown:{0}", this.id);
		}

		// Token: 0x040007BF RID: 1983
		internal int id;
	}
}
