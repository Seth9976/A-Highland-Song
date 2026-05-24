using System;
using System.Text;
using UnityEngine;

// Token: 0x020000A6 RID: 166
[RequireComponent(typeof(SLayout))]
public class DialogueBubbleView : MonoBehaviour
{
	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600055E RID: 1374 RVA: 0x0002AC84 File Offset: 0x00028E84
	public bool canAutoContinue
	{
		get
		{
			return !NarrativePresenter.instance.showingNarrativeChoiceWidget && !NarrativePresenter.instance.playingAudio && DialogueBubbleView.autoContinuePrefEnabled && !this.debugNeverEnd && this.animTextView.minimumReadTimeElapsed && !this.animTextView.returningToPool && !PhotoMode.visible;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x0600055F RID: 1375 RVA: 0x0002ACDC File Offset: 0x00028EDC
	// (set) Token: 0x06000560 RID: 1376 RVA: 0x0002ACE4 File Offset: 0x00028EE4
	public bool debugNeverEnd { get; set; }

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000561 RID: 1377 RVA: 0x0002ACED File Offset: 0x00028EED
	public static bool autoContinuePrefEnabled
	{
		get
		{
			return !DebugOptions.opts.dontAutoContinue;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000562 RID: 1378 RVA: 0x0002ACFC File Offset: 0x00028EFC
	private Prototype prototype
	{
		get
		{
			if (this._prototype == null)
			{
				this._prototype = base.GetComponent<Prototype>();
			}
			return this._prototype;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000563 RID: 1379 RVA: 0x0002AD1E File Offset: 0x00028F1E
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

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000564 RID: 1380 RVA: 0x0002AD40 File Offset: 0x00028F40
	public AnimatedTextView animTextView
	{
		get
		{
			if (this._animTextView == null)
			{
				this._animTextView = base.GetComponent<AnimatedTextView>();
			}
			return this._animTextView;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000565 RID: 1381 RVA: 0x0002AD62 File Offset: 0x00028F62
	private RectTransform canvasRT
	{
		get
		{
			if (this._canvasRT == null)
			{
				this._canvasRT = (RectTransform)base.GetComponentInParent<Canvas>().transform;
			}
			return this._canvasRT;
		}
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0002AD8E File Offset: 0x00028F8E
	private void Start()
	{
		if (this.prototype != null && !this.prototype.isOriginalPrototype)
		{
			this.prototype.OnReturnToPool += this.OnReturnToPool;
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0002ADC2 File Offset: 0x00028FC2
	private void OnDestroy()
	{
		if (this.prototype != null)
		{
			this.prototype.OnReturnToPool -= this.OnReturnToPool;
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0002ADE9 File Offset: 0x00028FE9
	private void OnEnable()
	{
		this.state = DialogueBubbleView.State.Hidden;
		this._sourceCharacterAnimator = null;
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0002AE0A File Offset: 0x0002900A
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
		this.state = DialogueBubbleView.State.Hidden;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0002AE24 File Offset: 0x00029024
	public void Setup(StoryCharacter storyCharacter, string textString)
	{
		this._sourceCharacter = storyCharacter;
		this._sourceCharacterAnimator = storyCharacter.animator;
		Transform transform = storyCharacter.fallbackMouthTransform;
		if (transform == null)
		{
			transform = storyCharacter.transform;
		}
		this._fallbackSourcePosition = transform.position;
		base.gameObject.name = "Dialogue Bubble View " + storyCharacter.name + ": " + textString;
		this.state = DialogueBubbleView.State.Showing;
		this._hasInitialPos = false;
		this._rotateSpeed = 0f;
		this._vel = default(Vector2);
		this.animTextView.Setup(textString, NarrativePresenter.instance.settings.textReadSettings, delegate
		{
			this.state = DialogueBubbleView.State.Shown;
		});
		this._tailLayout.alpha = 0f;
		this._background.alpha = 0f;
		this.layout.Animate(this._settings.backgroundFadeTime, delegate
		{
			this._background.alpha = this._settings.shadowMaxAlpha;
		});
		float num = 0.5f * ((float)this.animTextView.wordCount * this._settings.timeBetweenWords);
		this.layout.Animate(this._settings.fadeTimePerWord, num, delegate
		{
			this._tailLayout.alpha = 1f;
		});
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0002AF5A File Offset: 0x0002915A
	public void HideAndMarkForDestroy(float delay = 0f)
	{
		if (this.state == DialogueBubbleView.State.Shown || this.state == DialogueBubbleView.State.Showing)
		{
			this.state = DialogueBubbleView.State.Hiding;
			if (!this.animTextView.returningToPool)
			{
				this.animTextView.ReturnToPool(true);
			}
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0002AF8E File Offset: 0x0002918E
	private void Update()
	{
		if (this.state == DialogueBubbleView.State.Shown && this.canAutoContinue)
		{
			this.AutoContinue();
		}
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0002AFA8 File Offset: 0x000291A8
	private void OnReturnToPool()
	{
		this._sourceCharacter = null;
		this._sourceCharacterAnimator = null;
		this._hasInitialPos = false;
		this._vel = default(Vector2);
		this._rotateSpeed = 0f;
		this.state = DialogueBubbleView.State.Hidden;
		NarrativePresenter.instance.OnReturnBubbleToPool(this);
		this.layout.groupAlpha = 1f;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0002B004 File Offset: 0x00029204
	private void OnUIPositionUpdate(GameUI gameUI)
	{
		int num = -1;
		if (this._sourceCharacter != null && Time.time < this._sourceCharacter.lastDialogueBubbleTime + 20f)
		{
			num = this._sourceCharacter.lastDialogueBubbleAngleIdx;
		}
		Vector3 vector;
		if (this._sourceCharacterAnimator != null)
		{
			vector = this._sourceCharacterAnimator.mouthPosition;
		}
		else
		{
			vector = this._fallbackSourcePosition;
		}
		Vector2 vector2 = gameUI.WorldToCanvas(vector, this.canvasRT.sizeDelta);
		float num2 = Vector2.Distance(gameUI.WorldToCanvas(vector + this._settings.worldSpaceOffset * Vector3.up, this.canvasRT.sizeDelta), vector2) + this._settings.screenSpaceOffset;
		Vector2 size = this.layout.size;
		Vector2 canvasSize = gameUI.canvasSize;
		Rect rect = gameUI.narrativeContentInsets.ApplyToRect(new Rect(0f, 0f, canvasSize.x, canvasSize.y), false);
		Vector2 vector3 = vector2;
		vector3.x = Mathf.Clamp(vector2.x, rect.xMin, rect.xMax);
		vector3.y = Mathf.Clamp(vector2.y, rect.yMin, rect.yMax);
		Vector2 vector4 = vector2 - vector3;
		bool flag = vector4.magnitude > 0.1f;
		vector4 = (flag ? vector4.normalized : Vector2.zero);
		float num3 = float.MaxValue;
		int num4 = -1;
		float num5 = 0f;
		int num6 = 0;
		int num7 = 15;
		StringBuilder stringBuilder = null;
		if (this.debugAngleIdx != -1)
		{
			num7 = (num6 = this.debugAngleIdx);
			stringBuilder = new StringBuilder();
		}
		for (int i = num6; i <= num7; i++)
		{
			float num8 = (float)i / 16f * 2f * 3.1415927f;
			Vector2 vector5 = new Vector2(Mathf.Sin(num8), Mathf.Cos(num8));
			float num9 = 1f;
			Vector2 vector6 = Vector2.up;
			float num10 = 0.8f;
			if (flag)
			{
				vector6 = -vector4;
				num10 = 0.5f;
				if (stringBuilder != null)
				{
					stringBuilder.Append("Clamped ");
				}
			}
			else if (stringBuilder != null)
			{
				stringBuilder.Append("Upward ");
			}
			float num11 = Vector2.Dot(vector6, vector5);
			float num12 = 1f - Mathf.Max(0f, num11);
			float num13 = Mathf.Lerp(num10, 1f, num12);
			num9 *= num13;
			if (stringBuilder != null)
			{
				stringBuilder.AppendLine(num13.ToString("F3"));
			}
			if (num == i)
			{
				num9 *= 0.7f;
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine("Same as last 0.7");
				}
			}
			Vector2 vector7 = RectX.SplatVector(new Vector2(size.x + 2f * this._settings.marginToTailX, size.y + 2f * this._settings.marginToTailY), vector5);
			Vector2 vector8 = (num2 + this._settings.tailSize) * vector5;
			Rect rect2 = RectX.CreateFromCenter(vector3 + vector8 + vector7, size);
			if (rect2.xMin < rect.xMin || rect2.yMin < rect.yMin || rect2.xMax > rect.xMax || rect2.yMax > rect.yMax)
			{
				Rect rect3 = RectX.Intersect(rect2, rect);
				float num14 = Mathf.Max((rect2.width * rect2.height - rect3.width * rect3.height) / 5f, 100f);
				num9 *= num14;
				if (stringBuilder != null)
				{
					stringBuilder.Append("Overlap ");
					stringBuilder.AppendLine(num14.ToString("F1"));
				}
			}
			if (rect2.Contains(vector2) || rect2.Contains(vector3))
			{
				num9 *= 5f;
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine("Source 5.0");
				}
			}
			if (rect2.xMin < 80f || rect2.xMax > canvasSize.x - 80f || rect2.yMin < 80f || rect2.yMax > canvasSize.y - 80f)
			{
				num9 *= 1.4f;
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine("Too close to edge 1.4");
				}
			}
			foreach (GameUI.UnsafeRect unsafeRect in gameUI.unsafeRects)
			{
				Rect rect4 = RectX.Intersect(unsafeRect.rect, rect2);
				if (rect4 != Rect.zero)
				{
					float num15 = Mathf.Sqrt(rect4.width * rect4.height);
					float num16 = Mathf.InverseLerp(10f, 100f, num15);
					float num17 = Mathf.Lerp(1f, 6f, num16);
					num9 *= num17;
					if (stringBuilder != null)
					{
						stringBuilder.AppendLine("Unsafe rect " + num17.ToString("F3"));
					}
				}
			}
			if (stringBuilder != null)
			{
				stringBuilder.AppendLine("FINAL " + num9.ToString());
				this.debugScoreCalc = stringBuilder.ToString();
			}
			if (num9 < num3)
			{
				num5 = num8;
				num4 = i;
				num3 = num9;
			}
		}
		if (this._sourceCharacter != null)
		{
			this._sourceCharacter.lastDialogueBubbleAngleIdx = num4;
			this._sourceCharacter.lastDialogueBubbleTime = Time.time;
		}
		if (this._hasInitialPos)
		{
			this._targetAngle = 0.017453292f * Mathf.SmoothDampAngle(this._targetAngle * 57.29578f, num5 * 57.29578f, ref this._rotateSpeed, this._settings.rotateSmoothTime, float.MaxValue, Time.unscaledDeltaTime);
		}
		else
		{
			this._targetAngle = num5;
		}
		Vector2 vector9 = new Vector2(Mathf.Sin(this._targetAngle), Mathf.Cos(this._targetAngle));
		Vector2 vector10 = RectX.SplatVector(new Vector2(size.x + 2f * this._settings.marginToTailX, size.y + 2f * this._settings.marginToTailY), vector9);
		Vector2 vector11 = (num2 + this._settings.tailSize) * vector9;
		Vector2 vector12 = vector3 + vector11 + vector10;
		if (!this._hasInitialPos)
		{
			this._pos = vector12;
		}
		else
		{
			this._pos = Vector2.SmoothDamp(this._pos, vector12, ref this._vel, this._settings.smoothTime, float.MaxValue, Time.unscaledDeltaTime);
		}
		this.layout.center = this._pos;
		Vector2 normalized = (this._pos - vector3).normalized;
		Vector2 vector13 = RectX.SplatVector(new Vector2(size.x + 2f * this._settings.marginToTailX, size.y + 2f * this._settings.marginToTailY), normalized);
		this._tailLayout.origin = this.layout.middle - vector13;
		float num18 = -57.29578f * Mathf.Atan2(vector13.y, -vector13.x) + this._settings.tailAngleOffset;
		this._tailLayout.rotation = num18;
		this._tailLayout.transform.localScale = (this._settings.tailFlipAngleRange.Contains(num18) ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f));
		this._hasInitialPos = true;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0002B7A4 File Offset: 0x000299A4
	private void AutoContinue()
	{
		this.HideAndMarkForDestroy(0f);
	}

	// Token: 0x04000626 RID: 1574
	private const int numDirectionsToTry = 16;

	// Token: 0x04000627 RID: 1575
	public const string autoContinuePrefName = "AutoContinue";

	// Token: 0x04000629 RID: 1577
	private Prototype _prototype;

	// Token: 0x0400062A RID: 1578
	private SLayout _layout;

	// Token: 0x0400062B RID: 1579
	private AnimatedTextView _animTextView;

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	[Disable]
	private DialogueBubbleView.State state;

	// Token: 0x0400062D RID: 1581
	[Range(-1f, 15f)]
	public int debugAngleIdx = -1;

	// Token: 0x0400062E RID: 1582
	[TextArea(3, 8)]
	public string debugScoreCalc;

	// Token: 0x0400062F RID: 1583
	private RectTransform _canvasRT;

	// Token: 0x04000630 RID: 1584
	private Vector3 _fallbackSourcePosition;

	// Token: 0x04000631 RID: 1585
	private StoryCharacter _sourceCharacter;

	// Token: 0x04000632 RID: 1586
	private FrameAnimator _sourceCharacterAnimator;

	// Token: 0x04000633 RID: 1587
	private Vector2 _pos;

	// Token: 0x04000634 RID: 1588
	private Vector2 _vel;

	// Token: 0x04000635 RID: 1589
	private float _targetAngle;

	// Token: 0x04000636 RID: 1590
	private bool _hasInitialPos;

	// Token: 0x04000637 RID: 1591
	private float _rotateSpeed;

	// Token: 0x04000638 RID: 1592
	[SerializeField]
	private DialogueBubbleSettings _settings;

	// Token: 0x04000639 RID: 1593
	[SerializeField]
	private SLayout _background;

	// Token: 0x0400063A RID: 1594
	[SerializeField]
	private SLayout _tailLayout;

	// Token: 0x020002D0 RID: 720
	public enum State
	{
		// Token: 0x04001645 RID: 5701
		Hidden,
		// Token: 0x04001646 RID: 5702
		Showing,
		// Token: 0x04001647 RID: 5703
		Shown,
		// Token: 0x04001648 RID: 5704
		Hiding
	}
}
