using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x020000B1 RID: 177
	public struct StylePropertyName : IEquatable<StylePropertyName>
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00016263 File Offset: 0x00014463
		internal readonly StylePropertyId id { get; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001626B File Offset: 0x0001446B
		private readonly string name { get; }

		// Token: 0x060005E6 RID: 1510 RVA: 0x00016274 File Offset: 0x00014474
		internal static StylePropertyId StylePropertyIdFromString(string name)
		{
			StylePropertyId stylePropertyId;
			bool flag = StylePropertyUtil.s_NameToId.TryGetValue(name, ref stylePropertyId);
			StylePropertyId stylePropertyId2;
			if (flag)
			{
				stylePropertyId2 = stylePropertyId;
			}
			else
			{
				stylePropertyId2 = StylePropertyId.Unknown;
			}
			return stylePropertyId2;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000162A0 File Offset: 0x000144A0
		internal StylePropertyName(StylePropertyId stylePropertyId)
		{
			this.id = stylePropertyId;
			this.name = null;
			string text;
			bool flag = StylePropertyUtil.s_IdToName.TryGetValue(stylePropertyId, ref text);
			if (flag)
			{
				this.name = text;
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000162D8 File Offset: 0x000144D8
		public StylePropertyName(string name)
		{
			this.id = StylePropertyName.StylePropertyIdFromString(name);
			this.name = null;
			bool flag = this.id > StylePropertyId.Unknown;
			if (flag)
			{
				this.name = name;
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00016310 File Offset: 0x00014510
		public static bool IsNullOrEmpty(StylePropertyName propertyName)
		{
			return propertyName.id == StylePropertyId.Unknown;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001632C File Offset: 0x0001452C
		public static bool operator ==(StylePropertyName lhs, StylePropertyName rhs)
		{
			return lhs.id == rhs.id;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00016350 File Offset: 0x00014550
		public static bool operator !=(StylePropertyName lhs, StylePropertyName rhs)
		{
			return lhs.id != rhs.id;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00016378 File Offset: 0x00014578
		public static implicit operator StylePropertyName(string name)
		{
			return new StylePropertyName(name);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00016390 File Offset: 0x00014590
		public override int GetHashCode()
		{
			return (int)this.id;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000163A8 File Offset: 0x000145A8
		public override bool Equals(object other)
		{
			return other is StylePropertyName && this.Equals((StylePropertyName)other);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000163D4 File Offset: 0x000145D4
		public bool Equals(StylePropertyName other)
		{
			return this == other;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000163F4 File Offset: 0x000145F4
		public override string ToString()
		{
			return this.name;
		}
	}
}
