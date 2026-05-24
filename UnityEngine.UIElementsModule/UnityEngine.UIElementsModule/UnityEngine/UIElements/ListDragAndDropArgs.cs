using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001AE RID: 430
	internal struct ListDragAndDropArgs : IListDragAndDropArgs
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00038923 File Offset: 0x00036B23
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x0003892B File Offset: 0x00036B2B
		public object target { readonly get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00038934 File Offset: 0x00036B34
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x0003893C File Offset: 0x00036B3C
		public int insertAtIndex { readonly get; set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00038945 File Offset: 0x00036B45
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0003894D File Offset: 0x00036B4D
		public DragAndDropPosition dragAndDropPosition { readonly get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00038956 File Offset: 0x00036B56
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0003895E File Offset: 0x00036B5E
		public IDragAndDropData dragAndDropData { readonly get; set; }
	}
}
