using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002EB RID: 747
	public class UxmlValueBounds : UxmlTypeRestriction
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x00061D38 File Offset: 0x0005FF38
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x00061D40 File Offset: 0x0005FF40
		public string min { get; set; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x00061D49 File Offset: 0x0005FF49
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x00061D51 File Offset: 0x0005FF51
		public string max { get; set; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x00061D5A File Offset: 0x0005FF5A
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x00061D62 File Offset: 0x0005FF62
		public bool excludeMin { get; set; }

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x00061D6B File Offset: 0x0005FF6B
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x00061D73 File Offset: 0x0005FF73
		public bool excludeMax { get; set; }

		// Token: 0x06001891 RID: 6289 RVA: 0x00061D7C File Offset: 0x0005FF7C
		public override bool Equals(UxmlTypeRestriction other)
		{
			UxmlValueBounds uxmlValueBounds = other as UxmlValueBounds;
			bool flag = uxmlValueBounds == null;
			return !flag && (this.min == uxmlValueBounds.min && this.max == uxmlValueBounds.max && this.excludeMin == uxmlValueBounds.excludeMin) && this.excludeMax == uxmlValueBounds.excludeMax;
		}
	}
}
