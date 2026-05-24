using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001C9 RID: 457
[RequireComponent(typeof(CanvasGroup))]
[ExecuteInEditMode]
public class CanvasGroupOpacityInteractionEnabler : UIBehaviour
{
	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06000F49 RID: 3913 RVA: 0x000759E9 File Offset: 0x00073BE9
	private CanvasGroup canvasGroup
	{
		get
		{
			return base.GetComponent<CanvasGroup>();
		}
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x000759F4 File Offset: 0x00073BF4
	protected override void OnCanvasGroupChanged()
	{
		base.OnCanvasGroupChanged();
		bool flag = ((this.alphaThreshold == 1f) ? (this.canvasGroup.alpha >= this.alphaThreshold) : (this.canvasGroup.alpha > this.alphaThreshold));
		bool flag2 = this.blocksRaycasts && flag;
		if (this.canvasGroup.blocksRaycasts != flag2)
		{
			this.canvasGroup.blocksRaycasts = flag2;
		}
		bool flag3 = this.interactable && flag;
		if (this.canvasGroup.interactable != flag3)
		{
			this.canvasGroup.interactable = flag3;
		}
	}

	// Token: 0x040011C9 RID: 4553
	[Range(0f, 1f)]
	public float alphaThreshold = 1f;

	// Token: 0x040011CA RID: 4554
	public bool interactable = true;

	// Token: 0x040011CB RID: 4555
	public bool blocksRaycasts = true;
}
