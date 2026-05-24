using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class Credits : MonoSingleton<Credits>
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060008DD RID: 2269 RVA: 0x0004A6BF File Offset: 0x000488BF
	public bool visible
	{
		get
		{
			return this._layout.gameObject.activeSelf;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x0004A6D1 File Offset: 0x000488D1
	public bool nearlyBlackedOutOrHiding
	{
		get
		{
			return this._background.alpha > 0.95f || this._hiding;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060008DF RID: 2271 RVA: 0x0004A6ED File Offset: 0x000488ED
	public bool canExitEarly
	{
		get
		{
			return this.visible && this._time > this._settings.initialPause;
		}
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0004A70C File Offset: 0x0004890C
	public void Show(CreditsContext context, Action onComplete = null)
	{
		this._layout.gameObject.SetActive(true);
		this._layout.Animate((context == CreditsContext.GameEnd) ? this._settings.fadeInTime : this._settings.fastFadeInTime, delegate
		{
			this._layout.groupAlpha = 1f;
		});
		this._context = context;
		this._scrollingContainer.y = 0f;
		this._background.alpha = this._settings.backgroundInitialAlpha;
		this._time = 0f;
		this._charactersRunStartTime = 0f;
		this._hiding = false;
		this._charactersRunning = false;
		for (int i = 0; i < this._runningCharacters.Length; i++)
		{
			this._runningCharacters[i].transform.localPosition = this._runningCharactersStartPos[i];
			this._runningCharacters[i].alpha = 0f;
		}
		GameInput.PushControlStack(this);
		Credits.wantsGameplayPaused = true;
		this._onComplete = onComplete;
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0004A80C File Offset: 0x00048A0C
	public void Hide(bool fast)
	{
		if (this._hiding)
		{
			return;
		}
		this._hiding = true;
		this._layout.Animate(fast ? this._settings.fastFadeOutTime : this._settings.fadeOutTime, delegate
		{
			this._layout.groupAlpha = 0f;
		}).Then(delegate
		{
			this._layout.gameObject.SetActive(false);
			this._hiding = false;
			GameInput.PopControlStack(this, true);
			Credits.wantsGameplayPaused = false;
			if (this._onComplete != null)
			{
				Action onComplete = this._onComplete;
				this._onComplete = null;
				onComplete();
			}
		});
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0004A870 File Offset: 0x00048A70
	private void Awake()
	{
		this._layout.gameObject.SetActive(false);
		this._layout.groupAlpha = 0f;
		this._runningCharactersStartPos = new Vector2[this._runningCharacters.Length];
		for (int i = 0; i < this._runningCharacters.Length; i++)
		{
			this._runningCharactersStartPos[i] = this._runningCharacters[i].transform.localPosition;
		}
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0004A8E8 File Offset: 0x00048AE8
	private void Update()
	{
		if (!this.visible)
		{
			return;
		}
		if (!this._layout.isAnimating || this._time > 0f)
		{
			this._time += Time.unscaledDeltaTime;
		}
		if (this._time > this._settings.initialPause)
		{
			float y = this._scrollingContainer.y;
			float num = -Mathf.InverseLerp(0f, 1f, this._time) * this._settings.scrollSpeed * Time.unscaledDeltaTime;
			if (Mathf.Abs(GameInput.moveUpDown) > 0.1f)
			{
				num *= 10f * -GameInput.moveUpDown;
			}
			float num2 = y + num;
			float num3 = -(this._scrollingContainer.height - this._layout.canvasHeight);
			if (num2 < num3)
			{
				num2 = num3;
			}
			if (num2 > 0f)
			{
				num2 = 0f;
			}
			if (num2 != y)
			{
				this._scrollingContainer.y = num2;
			}
			if (!this._hiding && GameInput.Back(this) && this.canExitEarly)
			{
				this.Hide(true);
			}
			float num4 = this._runningCharacterTrigger.y + y;
			if (!this._charactersRunning && num4 < this._runningCharacters[0].centerY)
			{
				this._charactersRunning = true;
				this._charactersRunStartTime = this._time;
			}
		}
		float num5 = this._settings.backgroundFadeTimeRange.InverseLerp(this._time);
		this._background.alpha = Mathf.Lerp(this._settings.backgroundInitialAlpha, 1f, num5);
		if (this._charactersRunning)
		{
			foreach (SLayout slayout in this._runningCharacters)
			{
				slayout.x += Time.unscaledDeltaTime * this._settings.charactersRunSpeed;
				slayout.alpha += Time.unscaledDeltaTime / 0.5f;
			}
		}
		if (!this._hiding && this._charactersRunStartTime > 0f && this._time > this._charactersRunStartTime + this._settings.charactersRunTime)
		{
			this.Hide(false);
		}
	}

	// Token: 0x04000A9C RID: 2716
	public static bool wantsGameplayPaused;

	// Token: 0x04000A9D RID: 2717
	private float _time;

	// Token: 0x04000A9E RID: 2718
	private float _charactersRunStartTime;

	// Token: 0x04000A9F RID: 2719
	private bool _hiding;

	// Token: 0x04000AA0 RID: 2720
	private bool _charactersRunning;

	// Token: 0x04000AA1 RID: 2721
	private CreditsContext _context;

	// Token: 0x04000AA2 RID: 2722
	private Action _onComplete;

	// Token: 0x04000AA3 RID: 2723
	private Vector2[] _runningCharactersStartPos;

	// Token: 0x04000AA4 RID: 2724
	[SerializeField]
	private SLayout _layout;

	// Token: 0x04000AA5 RID: 2725
	[SerializeField]
	private SLayout _scrollingContainer;

	// Token: 0x04000AA6 RID: 2726
	[SerializeField]
	private SLayout[] _runningCharacters;

	// Token: 0x04000AA7 RID: 2727
	[SerializeField]
	private SLayout _runningCharacterTrigger;

	// Token: 0x04000AA8 RID: 2728
	[SerializeField]
	private SLayout _background;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	private CreditsSettings _settings;
}
