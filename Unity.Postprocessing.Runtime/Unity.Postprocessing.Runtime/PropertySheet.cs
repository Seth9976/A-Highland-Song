using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000060 RID: 96
	public sealed class PropertySheet
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000E884 File Offset: 0x0000CA84
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000E88C File Offset: 0x0000CA8C
		public MaterialPropertyBlock properties { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000E895 File Offset: 0x0000CA95
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000E89D File Offset: 0x0000CA9D
		internal Material material { get; private set; }

		// Token: 0x060001CC RID: 460 RVA: 0x0000E8A6 File Offset: 0x0000CAA6
		internal PropertySheet(Material material)
		{
			this.material = material;
			this.properties = new MaterialPropertyBlock();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
		public void ClearKeywords()
		{
			this.material.shaderKeywords = null;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000E8CE File Offset: 0x0000CACE
		public void EnableKeyword(string keyword)
		{
			this.material.EnableKeyword(keyword);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000E8DC File Offset: 0x0000CADC
		public void DisableKeyword(string keyword)
		{
			this.material.DisableKeyword(keyword);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000E8EA File Offset: 0x0000CAEA
		internal void Release()
		{
			RuntimeUtilities.Destroy(this.material);
			this.material = null;
		}
	}
}
