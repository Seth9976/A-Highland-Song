using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F8 RID: 760
	public struct CreationContext : IEquatable<CreationContext>
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x000639CD File Offset: 0x00061BCD
		// (set) Token: 0x060018F4 RID: 6388 RVA: 0x000639D5 File Offset: 0x00061BD5
		public VisualElement target { readonly get; private set; }

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x000639DE File Offset: 0x00061BDE
		// (set) Token: 0x060018F6 RID: 6390 RVA: 0x000639E6 File Offset: 0x00061BE6
		public VisualTreeAsset visualTreeAsset { readonly get; private set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x000639EF File Offset: 0x00061BEF
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x000639F7 File Offset: 0x00061BF7
		public Dictionary<string, VisualElement> slotInsertionPoints { readonly get; private set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x00063A00 File Offset: 0x00061C00
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x00063A08 File Offset: 0x00061C08
		internal List<TemplateAsset.AttributeOverride> attributeOverrides { readonly get; private set; }

		// Token: 0x060018FB RID: 6395 RVA: 0x00063A11 File Offset: 0x00061C11
		internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, VisualTreeAsset vta, VisualElement target)
		{
			this = new CreationContext(slotInsertionPoints, null, vta, target);
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x00063A1F File Offset: 0x00061C1F
		internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, List<TemplateAsset.AttributeOverride> attributeOverrides, VisualTreeAsset vta, VisualElement target)
		{
			this.target = target;
			this.slotInsertionPoints = slotInsertionPoints;
			this.attributeOverrides = attributeOverrides;
			this.visualTreeAsset = vta;
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00063A44 File Offset: 0x00061C44
		public override bool Equals(object obj)
		{
			return obj is CreationContext && this.Equals((CreationContext)obj);
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00063A70 File Offset: 0x00061C70
		public bool Equals(CreationContext other)
		{
			return EqualityComparer<VisualElement>.Default.Equals(this.target, other.target) && EqualityComparer<VisualTreeAsset>.Default.Equals(this.visualTreeAsset, other.visualTreeAsset) && EqualityComparer<Dictionary<string, VisualElement>>.Default.Equals(this.slotInsertionPoints, other.slotInsertionPoints);
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00063AD0 File Offset: 0x00061CD0
		public override int GetHashCode()
		{
			int num = -2123482148;
			num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(this.target);
			num = num * -1521134295 + EqualityComparer<VisualTreeAsset>.Default.GetHashCode(this.visualTreeAsset);
			return num * -1521134295 + EqualityComparer<Dictionary<string, VisualElement>>.Default.GetHashCode(this.slotInsertionPoints);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x00063B34 File Offset: 0x00061D34
		public static bool operator ==(CreationContext context1, CreationContext context2)
		{
			return context1.Equals(context2);
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00063B50 File Offset: 0x00061D50
		public static bool operator !=(CreationContext context1, CreationContext context2)
		{
			return !(context1 == context2);
		}

		// Token: 0x04000AA9 RID: 2729
		public static readonly CreationContext Default = default(CreationContext);
	}
}
