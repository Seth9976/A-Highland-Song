using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000120 RID: 288
public class LookFurtherVignette : MonoBehaviour
{
	// Token: 0x17000264 RID: 612
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x000514FA File Offset: 0x0004F6FA
	private Image image
	{
		get
		{
			if (this._image == null)
			{
				this._image = base.GetComponent<Image>();
			}
			return this._image;
		}
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0005151C File Offset: 0x0004F71C
	private void Update()
	{
		float num = (Game.instance.lookingFurther ? this.maxAlpha : 0f);
		Color color = this.image.color;
		float num2 = ((num > color.a) ? this.fadeInTime : this.fadeOutTime);
		color.a = Mathf.MoveTowards(color.a, num, Time.deltaTime / num2);
		this.image.enabled = color.a > 0f;
		this.image.color = color;
	}

	// Token: 0x04000BB8 RID: 3000
	public float maxAlpha = 0.5f;

	// Token: 0x04000BB9 RID: 3001
	public float fadeInTime = 0.3f;

	// Token: 0x04000BBA RID: 3002
	public float fadeOutTime = 0.3f;

	// Token: 0x04000BBB RID: 3003
	private Image _image;
}
