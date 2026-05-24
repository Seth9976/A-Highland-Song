using System;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A1 RID: 673
	[Serializable]
	internal class StyleSelector
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x0005D6D8 File Offset: 0x0005B8D8
		// (set) Token: 0x060016D2 RID: 5842 RVA: 0x0005D6F0 File Offset: 0x0005B8F0
		public StyleSelectorPart[] parts
		{
			get
			{
				return this.m_Parts;
			}
			internal set
			{
				this.m_Parts = value;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x0005D6FC File Offset: 0x0005B8FC
		// (set) Token: 0x060016D4 RID: 5844 RVA: 0x0005D714 File Offset: 0x0005B914
		public StyleSelectorRelationship previousRelationship
		{
			get
			{
				return this.m_PreviousRelationship;
			}
			internal set
			{
				this.m_PreviousRelationship = value;
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x0005D720 File Offset: 0x0005B920
		public override string ToString()
		{
			return string.Join(", ", Enumerable.ToArray<string>(Enumerable.Select<StyleSelectorPart, string>(this.parts, (StyleSelectorPart p) => p.ToString())));
		}

		// Token: 0x04000990 RID: 2448
		[SerializeField]
		private StyleSelectorPart[] m_Parts;

		// Token: 0x04000991 RID: 2449
		[SerializeField]
		private StyleSelectorRelationship m_PreviousRelationship;

		// Token: 0x04000992 RID: 2450
		internal int pseudoStateMask = -1;

		// Token: 0x04000993 RID: 2451
		internal int negatedPseudoStateMask = -1;
	}
}
