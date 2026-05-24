using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200029F RID: 671
	[Serializable]
	internal class StyleProperty
	{
		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060016C7 RID: 5831 RVA: 0x0005D648 File Offset: 0x0005B848
		// (set) Token: 0x060016C8 RID: 5832 RVA: 0x0005D660 File Offset: 0x0005B860
		public string name
		{
			get
			{
				return this.m_Name;
			}
			internal set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x0005D66C File Offset: 0x0005B86C
		// (set) Token: 0x060016CA RID: 5834 RVA: 0x0005D684 File Offset: 0x0005B884
		public int line
		{
			get
			{
				return this.m_Line;
			}
			internal set
			{
				this.m_Line = value;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x0005D690 File Offset: 0x0005B890
		// (set) Token: 0x060016CC RID: 5836 RVA: 0x0005D6A8 File Offset: 0x0005B8A8
		public StyleValueHandle[] values
		{
			get
			{
				return this.m_Values;
			}
			internal set
			{
				this.m_Values = value;
			}
		}

		// Token: 0x04000988 RID: 2440
		[SerializeField]
		private string m_Name;

		// Token: 0x04000989 RID: 2441
		[SerializeField]
		private int m_Line;

		// Token: 0x0400098A RID: 2442
		[SerializeField]
		private StyleValueHandle[] m_Values;

		// Token: 0x0400098B RID: 2443
		[NonSerialized]
		internal bool isCustomProperty;

		// Token: 0x0400098C RID: 2444
		[NonSerialized]
		internal bool requireVariableResolve;
	}
}
