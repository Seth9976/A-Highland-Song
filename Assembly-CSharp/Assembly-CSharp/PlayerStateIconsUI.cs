using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class PlayerStateIconsUI : MonoBehaviour
{
	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000A94 RID: 2708 RVA: 0x000577CC File Offset: 0x000559CC
	private bool shouldShowTiredIcon
	{
		get
		{
			return GameClock.instance.isLate && !GameClock.instance.isWaitingForTimeToPass && !Runner.instance.hidden && !Runner.instance.dead && !Runner.instance.inFinalJump && Runner.instance.playerControlDisabled == PlayerControlDisableReason.None && !Runner.instance.isMusicRunning && Level.currentIndex + 1 < 9 && !PhotoMode.visible;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00057842 File Offset: 0x00055A42
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

	// Token: 0x06000A96 RID: 2710 RVA: 0x00057864 File Offset: 0x00055A64
	private void Start()
	{
		this._staminaIcon.groupAlpha = 0f;
		this._tiredIcon.groupAlpha = 0f;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00057886 File Offset: 0x00055A86
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00057899 File Offset: 0x00055A99
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x000578AC File Offset: 0x00055AAC
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		bool flag = Runner.instance.staminaIsLow && !PhotoMode.visible;
		if (this._showingStamina != flag || this._showingTired != this.shouldShowTiredIcon)
		{
			this._showingStamina = flag;
			this._showingTired = this.shouldShowTiredIcon;
			float y = 0f;
			this._tiredIcon.Animate(this._showingTired ? this.fadeInTime : this.fadeOutTime, delegate
			{
				if (this._showingTired)
				{
					this._tiredIcon.y = y;
					y += this._tiredIcon.height + this.iconSpacing;
					this._tiredIcon.groupAlpha = 1f;
					return;
				}
				this._tiredIcon.y = this.hiddenY;
				this._tiredIcon.groupAlpha = 0f;
			});
			this._staminaIcon.Animate(this._showingStamina ? this.fadeInTime : this.fadeOutTime, delegate
			{
				if (this._showingStamina)
				{
					this._staminaIcon.y = y;
					this._staminaIcon.groupAlpha = 1f;
					return;
				}
				this._staminaIcon.y = this.hiddenY;
				this._staminaIcon.groupAlpha = 0f;
			});
		}
		bool staminaIsVeryLow = Runner.instance.staminaIsVeryLow;
		if (staminaIsVeryLow != this._staminaRed)
		{
			this._staminaRed = staminaIsVeryLow;
			this._staminaIconBg.Animate((this._staminaIcon.groupAlpha == 0f) ? 0f : 0.2f, delegate
			{
				this._staminaIconBg.fillColor = (this._staminaRed ? this._staminaRedColor : Color.black);
			});
		}
		float num = this.baseScale * (1f + this.pulseAmount * Mathf.Sin(this.pulseSpeed * Time.unscaledTime));
		if (this._showingStamina)
		{
			this._staminaIcon.scale = num;
		}
		else if (this._staminaIcon.alpha > 0f)
		{
			this._staminaIcon.scale = Mathf.MoveTowards(this._staminaIcon.scale, 0.5f, Time.unscaledDeltaTime);
		}
		if (this._showingTired)
		{
			this._tiredIcon.scale = num;
			return;
		}
		if (this._tiredIcon.alpha > 0f)
		{
			this._tiredIcon.scale = Mathf.MoveTowards(this._tiredIcon.scale, 0.5f, Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00057A8C File Offset: 0x00055C8C
	private void OnUIPositionUpdate(GameUI ui)
	{
		Vector3 mouthPosition = Runner.instance.animator.mouthPosition;
		Vector2 vector = ui.WorldToCanvas(mouthPosition + this.worldOffset.x * Runner.instance.direction * Vector3.right + this.worldOffset.y * Vector3.up, default(Vector2));
		vector.x += Runner.instance.direction * this.screenOffset.x;
		vector.y += this.screenOffset.y;
		this.layout.center = vector;
	}

	// Token: 0x04000CC4 RID: 3268
	public Vector2 worldOffset = new Vector2(-0.2f, 1.5f);

	// Token: 0x04000CC5 RID: 3269
	public Vector2 screenOffset = Vector2.zero;

	// Token: 0x04000CC6 RID: 3270
	public float pulseAmount = 0.1f;

	// Token: 0x04000CC7 RID: 3271
	public float pulseSpeed = 1f;

	// Token: 0x04000CC8 RID: 3272
	public float baseScale = 1.5f;

	// Token: 0x04000CC9 RID: 3273
	public float iconSpacing = 10f;

	// Token: 0x04000CCA RID: 3274
	public float hiddenY = -30f;

	// Token: 0x04000CCB RID: 3275
	public float fadeInTime = 0.2f;

	// Token: 0x04000CCC RID: 3276
	public float fadeOutTime = 0.5f;

	// Token: 0x04000CCD RID: 3277
	private SLayout _layout;

	// Token: 0x04000CCE RID: 3278
	private bool _showingStamina;

	// Token: 0x04000CCF RID: 3279
	private bool _staminaRed;

	// Token: 0x04000CD0 RID: 3280
	private bool _showingTired;

	// Token: 0x04000CD1 RID: 3281
	[SerializeField]
	private SLayout _staminaIcon;

	// Token: 0x04000CD2 RID: 3282
	[SerializeField]
	private SLayout _staminaIconBg;

	// Token: 0x04000CD3 RID: 3283
	[SerializeField]
	private SLayout _tiredIcon;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	private Color _staminaRedColor;
}
