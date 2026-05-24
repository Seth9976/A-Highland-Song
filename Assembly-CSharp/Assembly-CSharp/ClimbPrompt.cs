using System;
using EasyButtons;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class ClimbPrompt : MonoBehaviour
{
	// Token: 0x1700023F RID: 575
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x0004A3BE File Offset: 0x000485BE
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

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x0004A3E0 File Offset: 0x000485E0
	// (set) Token: 0x060008CF RID: 2255 RVA: 0x0004A3ED File Offset: 0x000485ED
	public int upwardClimbsCompleted
	{
		get
		{
			return PlayerPrefsX.GetInt("UpwardClimbsCompleted", 0);
		}
		set
		{
			PlayerPrefsX.SetInt("UpwardClimbsCompleted", value);
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0004A3FA File Offset: 0x000485FA
	[Button("Reset tutorial climb-up count")]
	public static void ResetUpwardClimbCounter()
	{
		PlayerPrefsX.DeleteKey("UpwardClimbsCompleted");
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0004A408 File Offset: 0x00048608
	public void ShowAt(Vector3 promptPos3d, bool isUpwardClimb, float fill, bool lowStamina)
	{
		this.worldPos = promptPos3d;
		if (isUpwardClimb)
		{
			this._background.origin = this.upwardLocalBgPos;
			this._background.rotation = 180f;
			this._icon.origin = this.upwardIconPos;
		}
		else
		{
			this._background.origin = Vector2.zero;
			this._background.rotation = 0f;
			this._icon.origin = this._downwardIconPos;
		}
		this._fill.image.fillAmount = fill;
		this.SetLowStaminaColouring(lowStamina);
		if (this.layout.targetGroupAlpha != 1f)
		{
			this.layout.CancelAnimations();
			this.layout.Animate(0.2f, delegate
			{
				this.layout.groupAlpha = 1f;
			});
		}
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0004A4D7 File Offset: 0x000486D7
	public void Hide(float delay = 0f)
	{
		if (this.layout.targetGroupAlpha != 0f)
		{
			this.layout.CancelAnimations();
			this.layout.Animate(0.5f, delay, delegate
			{
				this.layout.groupAlpha = 0f;
			});
		}
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0004A514 File Offset: 0x00048714
	private void SetLowStaminaColouring(bool newLowStaminaColouring)
	{
		if (newLowStaminaColouring != this._colouredForLowStamina)
		{
			this._colouredForLowStamina = newLowStaminaColouring;
			this._background.Animate((this.layout.groupAlpha == 0f) ? 0f : 0.2f, delegate
			{
				this._background.color = (this._colouredForLowStamina ? this._veryLowStaminaBGColor : this._normalBGColor);
				this._fill.color = (this._colouredForLowStamina ? this._lowStaminaFillColor : this._fillColor);
			});
		}
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0004A568 File Offset: 0x00048768
	public void Confirm()
	{
		if (this.layout.groupAlpha == 0f && this.layout.targetAlpha == 0f)
		{
			return;
		}
		this._fill.image.fillAmount = 1f;
		this.Hide(0.3f);
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0004A5BA File Offset: 0x000487BA
	private void Start()
	{
		this.layout.groupAlpha = 0f;
		this._downwardIconPos = this._icon.origin;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0004A5DD File Offset: 0x000487DD
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0004A5F0 File Offset: 0x000487F0
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0004A604 File Offset: 0x00048804
	private void OnUIPositionUpdate(GameUI ui)
	{
		if (this.layout.groupAlpha > 0f)
		{
			this.layout.center = ui.WorldToCanvas(this.worldPos, default(Vector2));
		}
	}

	// Token: 0x04000A8A RID: 2698
	public Vector3 worldPos;

	// Token: 0x04000A8B RID: 2699
	public Vector2 upwardLocalBgPos;

	// Token: 0x04000A8C RID: 2700
	public Vector2 upwardIconPos;

	// Token: 0x04000A8D RID: 2701
	private SLayout _layout;

	// Token: 0x04000A8E RID: 2702
	[SerializeField]
	private SLayout _background;

	// Token: 0x04000A8F RID: 2703
	[SerializeField]
	private SLayout _icon;

	// Token: 0x04000A90 RID: 2704
	[SerializeField]
	private SLayout _fill;

	// Token: 0x04000A91 RID: 2705
	[SerializeField]
	private Color _normalBGColor;

	// Token: 0x04000A92 RID: 2706
	[SerializeField]
	private Color _veryLowStaminaBGColor;

	// Token: 0x04000A93 RID: 2707
	[SerializeField]
	private Color _fillColor;

	// Token: 0x04000A94 RID: 2708
	[SerializeField]
	private Color _lowStaminaFillColor;

	// Token: 0x04000A95 RID: 2709
	private Vector2 _downwardIconPos;

	// Token: 0x04000A96 RID: 2710
	private bool _completedUpwardTutorial;

	// Token: 0x04000A97 RID: 2711
	private bool _colouredForLowStamina;
}
