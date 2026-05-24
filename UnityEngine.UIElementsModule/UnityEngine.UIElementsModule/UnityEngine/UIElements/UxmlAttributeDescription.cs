using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x020002CA RID: 714
	public abstract class UxmlAttributeDescription
	{
		// Token: 0x060017D5 RID: 6101 RVA: 0x0006049D File Offset: 0x0005E69D
		protected UxmlAttributeDescription()
		{
			this.use = UxmlAttributeDescription.Use.Optional;
			this.restriction = null;
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x000604B7 File Offset: 0x0005E6B7
		// (set) Token: 0x060017D7 RID: 6103 RVA: 0x000604BF File Offset: 0x0005E6BF
		public string name { get; set; }

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x000604C8 File Offset: 0x0005E6C8
		// (set) Token: 0x060017D9 RID: 6105 RVA: 0x000604E0 File Offset: 0x0005E6E0
		public IEnumerable<string> obsoleteNames
		{
			get
			{
				return this.m_ObsoleteNames;
			}
			set
			{
				this.m_ObsoleteNames = Enumerable.ToArray<string>(value);
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x000604EF File Offset: 0x0005E6EF
		// (set) Token: 0x060017DB RID: 6107 RVA: 0x000604F7 File Offset: 0x0005E6F7
		public string type { get; protected set; }

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x00060500 File Offset: 0x0005E700
		// (set) Token: 0x060017DD RID: 6109 RVA: 0x00060508 File Offset: 0x0005E708
		public string typeNamespace { get; protected set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060017DE RID: 6110
		public abstract string defaultValueAsString { get; }

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x00060511 File Offset: 0x0005E711
		// (set) Token: 0x060017E0 RID: 6112 RVA: 0x00060519 File Offset: 0x0005E719
		public UxmlAttributeDescription.Use use { get; set; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00060522 File Offset: 0x0005E722
		// (set) Token: 0x060017E2 RID: 6114 RVA: 0x0006052A File Offset: 0x0005E72A
		public UxmlTypeRestriction restriction { get; set; }

		// Token: 0x060017E3 RID: 6115 RVA: 0x00060534 File Offset: 0x0005E734
		internal bool TryGetValueFromBagAsString(IUxmlAttributes bag, CreationContext cc, out string value)
		{
			bool flag = this.name == null && (this.m_ObsoleteNames == null || this.m_ObsoleteNames.Length == 0);
			bool flag2;
			if (flag)
			{
				Debug.LogError("Attribute description has no name.");
				value = null;
				flag2 = false;
			}
			else
			{
				string text;
				bag.TryGetAttributeValue("name", out text);
				bool flag3 = !string.IsNullOrEmpty(text) && cc.attributeOverrides != null;
				if (flag3)
				{
					for (int i = 0; i < cc.attributeOverrides.Count; i++)
					{
						bool flag4 = cc.attributeOverrides[i].m_ElementName != text;
						if (!flag4)
						{
							bool flag5 = cc.attributeOverrides[i].m_AttributeName != this.name;
							if (flag5)
							{
								bool flag6 = this.m_ObsoleteNames != null;
								if (!flag6)
								{
									goto IL_0147;
								}
								bool flag7 = false;
								for (int j = 0; j < this.m_ObsoleteNames.Length; j++)
								{
									bool flag8 = cc.attributeOverrides[i].m_AttributeName == this.m_ObsoleteNames[j];
									if (flag8)
									{
										flag7 = true;
										break;
									}
								}
								bool flag9 = !flag7;
								if (flag9)
								{
									goto IL_0147;
								}
							}
							value = cc.attributeOverrides[i].m_Value;
							return true;
						}
						IL_0147:;
					}
				}
				bool flag10 = this.name == null;
				if (flag10)
				{
					for (int k = 0; k < this.m_ObsoleteNames.Length; k++)
					{
						bool flag11 = bag.TryGetAttributeValue(this.m_ObsoleteNames[k], out value);
						if (flag11)
						{
							bool flag12 = cc.visualTreeAsset != null;
							if (flag12)
							{
							}
							return true;
						}
					}
					value = null;
					flag2 = false;
				}
				else
				{
					bool flag13 = !bag.TryGetAttributeValue(this.name, out value);
					if (flag13)
					{
						bool flag14 = this.m_ObsoleteNames != null;
						if (flag14)
						{
							for (int l = 0; l < this.m_ObsoleteNames.Length; l++)
							{
								bool flag15 = bag.TryGetAttributeValue(this.m_ObsoleteNames[l], out value);
								if (flag15)
								{
									bool flag16 = cc.visualTreeAsset != null;
									if (flag16)
									{
									}
									return true;
								}
							}
						}
						value = null;
						flag2 = false;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			return flag2;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00060798 File Offset: 0x0005E998
		protected bool TryGetValueFromBag<T>(IUxmlAttributes bag, CreationContext cc, Func<string, T, T> converterFunc, T defaultValue, ref T value)
		{
			string text;
			bool flag = this.TryGetValueFromBagAsString(bag, cc, out text);
			bool flag3;
			if (flag)
			{
				bool flag2 = converterFunc != null;
				if (flag2)
				{
					value = converterFunc.Invoke(text, defaultValue);
				}
				else
				{
					value = defaultValue;
				}
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000607E8 File Offset: 0x0005E9E8
		protected T GetValueFromBag<T>(IUxmlAttributes bag, CreationContext cc, Func<string, T, T> converterFunc, T defaultValue)
		{
			bool flag = converterFunc == null;
			if (flag)
			{
				throw new ArgumentNullException("converterFunc");
			}
			string text;
			bool flag2 = this.TryGetValueFromBagAsString(bag, cc, out text);
			T t;
			if (flag2)
			{
				t = converterFunc.Invoke(text, defaultValue);
			}
			else
			{
				t = defaultValue;
			}
			return t;
		}

		// Token: 0x04000A2A RID: 2602
		protected const string xmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x04000A2C RID: 2604
		private string[] m_ObsoleteNames;

		// Token: 0x020002CB RID: 715
		public enum Use
		{
			// Token: 0x04000A32 RID: 2610
			None,
			// Token: 0x04000A33 RID: 2611
			Optional,
			// Token: 0x04000A34 RID: 2612
			Prohibited,
			// Token: 0x04000A35 RID: 2613
			Required
		}
	}
}
