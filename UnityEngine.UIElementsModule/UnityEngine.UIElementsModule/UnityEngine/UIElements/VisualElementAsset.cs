using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002ED RID: 749
	[Serializable]
	internal class VisualElementAsset : IUxmlAttributes, ISerializationCallbackReceiver
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x00061E84 File Offset: 0x00060084
		// (set) Token: 0x06001898 RID: 6296 RVA: 0x00061E9C File Offset: 0x0006009C
		public int id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x00061EA8 File Offset: 0x000600A8
		// (set) Token: 0x0600189A RID: 6298 RVA: 0x00061EC0 File Offset: 0x000600C0
		public int orderInDocument
		{
			get
			{
				return this.m_OrderInDocument;
			}
			set
			{
				this.m_OrderInDocument = value;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x00061ECC File Offset: 0x000600CC
		// (set) Token: 0x0600189C RID: 6300 RVA: 0x00061EE4 File Offset: 0x000600E4
		public int parentId
		{
			get
			{
				return this.m_ParentId;
			}
			set
			{
				this.m_ParentId = value;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x00061EF0 File Offset: 0x000600F0
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x00061F08 File Offset: 0x00060108
		public int ruleIndex
		{
			get
			{
				return this.m_RuleIndex;
			}
			set
			{
				this.m_RuleIndex = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x00061F14 File Offset: 0x00060114
		// (set) Token: 0x060018A0 RID: 6304 RVA: 0x00061F2C File Offset: 0x0006012C
		public string fullTypeName
		{
			get
			{
				return this.m_FullTypeName;
			}
			set
			{
				this.m_FullTypeName = value;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x00061F38 File Offset: 0x00060138
		// (set) Token: 0x060018A2 RID: 6306 RVA: 0x00061F50 File Offset: 0x00060150
		public string[] classes
		{
			get
			{
				return this.m_Classes;
			}
			set
			{
				this.m_Classes = value;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x00061F5C File Offset: 0x0006015C
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x00061F86 File Offset: 0x00060186
		public List<string> stylesheetPaths
		{
			get
			{
				List<string> list;
				if ((list = this.m_StylesheetPaths) == null)
				{
					list = (this.m_StylesheetPaths = new List<string>());
				}
				return list;
			}
			set
			{
				this.m_StylesheetPaths = value;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x00061F90 File Offset: 0x00060190
		public bool hasStylesheetPaths
		{
			get
			{
				return this.m_StylesheetPaths != null;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060018A6 RID: 6310 RVA: 0x00061F9C File Offset: 0x0006019C
		// (set) Token: 0x060018A7 RID: 6311 RVA: 0x00061FC6 File Offset: 0x000601C6
		public List<StyleSheet> stylesheets
		{
			get
			{
				List<StyleSheet> list;
				if ((list = this.m_Stylesheets) == null)
				{
					list = (this.m_Stylesheets = new List<StyleSheet>());
				}
				return list;
			}
			set
			{
				this.m_Stylesheets = value;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x00061FD0 File Offset: 0x000601D0
		public bool hasStylesheets
		{
			get
			{
				return this.m_Stylesheets != null;
			}
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00061FDB File Offset: 0x000601DB
		public VisualElementAsset(string fullTypeName)
		{
			this.m_FullTypeName = fullTypeName;
			this.m_Name = string.Empty;
			this.m_Text = string.Empty;
			this.m_PickingMode = PickingMode.Position;
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000020E6 File Offset: 0x000002E6
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0006200C File Offset: 0x0006020C
		public void OnAfterDeserialize()
		{
			bool flag = !string.IsNullOrEmpty(this.m_Name) && !this.m_Properties.Contains("name");
			if (flag)
			{
				this.AddProperty("name", this.m_Name);
			}
			bool flag2 = !string.IsNullOrEmpty(this.m_Text) && !this.m_Properties.Contains("text");
			if (flag2)
			{
				this.AddProperty("text", this.m_Text);
			}
			bool flag3 = this.m_PickingMode != PickingMode.Position && !this.m_Properties.Contains("picking-mode") && !this.m_Properties.Contains("pickingMode");
			if (flag3)
			{
				this.AddProperty("picking-mode", this.m_PickingMode.ToString());
			}
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x000620E3 File Offset: 0x000602E3
		public void AddProperty(string propertyName, string propertyValue)
		{
			this.SetOrAddProperty(propertyName, propertyValue);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x000620F0 File Offset: 0x000602F0
		private void SetOrAddProperty(string propertyName, string propertyValue)
		{
			bool flag = this.m_Properties == null;
			if (flag)
			{
				this.m_Properties = new List<string>();
			}
			for (int i = 0; i < this.m_Properties.Count - 1; i += 2)
			{
				bool flag2 = this.m_Properties[i] == propertyName;
				if (flag2)
				{
					this.m_Properties[i + 1] = propertyValue;
					return;
				}
			}
			this.m_Properties.Add(propertyName);
			this.m_Properties.Add(propertyValue);
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00062178 File Offset: 0x00060378
		public bool TryGetAttributeValue(string propertyName, out string value)
		{
			bool flag = this.m_Properties == null;
			bool flag2;
			if (flag)
			{
				value = null;
				flag2 = false;
			}
			else
			{
				for (int i = 0; i < this.m_Properties.Count - 1; i += 2)
				{
					bool flag3 = this.m_Properties[i] == propertyName;
					if (flag3)
					{
						value = this.m_Properties[i + 1];
						return true;
					}
				}
				value = null;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04000A73 RID: 2675
		[SerializeField]
		private string m_Name;

		// Token: 0x04000A74 RID: 2676
		[SerializeField]
		private int m_Id;

		// Token: 0x04000A75 RID: 2677
		[SerializeField]
		private int m_OrderInDocument;

		// Token: 0x04000A76 RID: 2678
		[SerializeField]
		private int m_ParentId;

		// Token: 0x04000A77 RID: 2679
		[SerializeField]
		private int m_RuleIndex;

		// Token: 0x04000A78 RID: 2680
		[SerializeField]
		private string m_Text;

		// Token: 0x04000A79 RID: 2681
		[SerializeField]
		private PickingMode m_PickingMode;

		// Token: 0x04000A7A RID: 2682
		[SerializeField]
		private string m_FullTypeName;

		// Token: 0x04000A7B RID: 2683
		[SerializeField]
		private string[] m_Classes;

		// Token: 0x04000A7C RID: 2684
		[SerializeField]
		private List<string> m_StylesheetPaths;

		// Token: 0x04000A7D RID: 2685
		[SerializeField]
		private List<StyleSheet> m_Stylesheets;

		// Token: 0x04000A7E RID: 2686
		[SerializeField]
		private List<string> m_Properties;
	}
}
