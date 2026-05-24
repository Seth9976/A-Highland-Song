using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013E RID: 318
public class QuickButtonPrompt : MonoBehaviour
{
	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00058598 File Offset: 0x00056798
	private SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x000585BA File Offset: 0x000567BA
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
		this.layout.groupAlpha = 1f;
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x000585DD File Offset: 0x000567DD
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x000585F0 File Offset: 0x000567F0
	public void Hide()
	{
		this.layout.Animate(0.5f, delegate
		{
			this.layout.groupAlpha = 0f;
		}).Then(delegate
		{
			base.GetComponent<Prototype>().ReturnToPool();
		});
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00058620 File Offset: 0x00056820
	private void OnUIPositionUpdate(GameUI gameUI)
	{
		this.layout.origin = gameUI.WorldToCanvas(Runner.instance.transform.position, default(Vector2)) + this.screenOffset;
		float num = Mathf.InverseLerp(-1f, 1f, Mathf.Sin(this.flashSpeed * Time.time));
		this._promptTextMesh.color = Color.Lerp(this.color1, this.color2, num);
		this._promptImage.color = Color.Lerp(this.color1, this.color2, num);
		this._promptBackgroundImage.color = Color.Lerp(this.color1, this.color2, num);
	}

	// Token: 0x04000CF9 RID: 3321
	public float flashSpeed = 10f;

	// Token: 0x04000CFA RID: 3322
	public Vector2 screenOffset;

	// Token: 0x04000CFB RID: 3323
	public Color color1 = Color.white;

	// Token: 0x04000CFC RID: 3324
	public Color color2 = Color.white.WithAlpha(0.5f);

	// Token: 0x04000CFD RID: 3325
	private SLayout _layout;

	// Token: 0x04000CFE RID: 3326
	[SerializeField]
	private TextMeshProUGUI _promptTextMesh;

	// Token: 0x04000CFF RID: 3327
	[SerializeField]
	private Image _promptImage;

	// Token: 0x04000D00 RID: 3328
	[SerializeField]
	private Image _promptBackgroundImage;
}
