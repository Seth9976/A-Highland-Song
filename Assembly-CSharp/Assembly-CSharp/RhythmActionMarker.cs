using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class RhythmActionMarker : MonoBehaviour
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000372 RID: 882 RVA: 0x0001AE04 File Offset: 0x00019004
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

	// Token: 0x06000373 RID: 883 RVA: 0x0001AE28 File Offset: 0x00019028
	public void Setup(Vector3 pos, BeatTrack.ObstacleRef obstacleRef)
	{
		this.obstacleRef = obstacleRef;
		Transform transform = base.transform;
		this.targetPos = pos;
		transform.position = pos;
		this.beamSpriteRenderer.transform.localPosition = Vector3.zero;
		this.isSpecialJump = TrackBuilder.specialJumpsAvailable && obstacleRef.special;
		this.baseSpriteRenderer.sprite = (this.isSpecialJump ? this.settings.specialBaseSprite : this.settings.baseSprite);
		this.baseSpriteRenderer.color = Color.white.WithAlpha(0f);
		this.beamSpriteRenderer.color = (this.isSpecialJump ? this.settings.specialBeamColor.WithAlpha(0f) : this.settings.beamColor.WithAlpha(0f));
		this.flashSpriteRenderer.color = Color.white.WithAlpha(0f);
		this._hiding = false;
		this._success = false;
		this._transition = 0f;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0001AF34 File Offset: 0x00019134
	private void OnEnable()
	{
		this.Setup(base.transform.position, default(BeatTrack.ObstacleRef));
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001AF5B File Offset: 0x0001915B
	private void OnDisable()
	{
		this.obstacleRef = default(BeatTrack.ObstacleRef);
		this._hiding = false;
		this._success = false;
		this._transition = 0f;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0001AF84 File Offset: 0x00019184
	public void Hide(RhythmActionMarker.HideReason hideReason, bool success = false, bool nailedIt = false, float timeToObs = 0f)
	{
		this._hiding = true;
		this._success = success;
		this.obstacleRef = default(BeatTrack.ObstacleRef);
		if (success)
		{
			base.transform.position -= this.settings.downwardClunkDist * Vector3.up;
		}
		if (this.buttonPrompt != null)
		{
			this.buttonPrompt.marker = null;
			this.buttonPrompt = null;
		}
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001AFFC File Offset: 0x000191FC
	private void Update()
	{
		if (!this._hiding)
		{
			if (Vector3.Distance(base.transform.position, this.targetPos) < 4f)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.targetPos, 0.05f);
			}
			else
			{
				base.transform.position = this.targetPos;
			}
		}
		int num = (this._hiding ? 0 : 1);
		float num2 = (this._success ? this.settings.successTransitionTime : this.settings.failTransitionTime);
		this._transition = Mathf.MoveTowards(this._transition, (float)num, Time.deltaTime / num2);
		float num3 = (this._success ? this.settings.successAlpha : this.settings.standardAlpha);
		Color color = (this.obstacleRef.special ? this.settings.specialBeamColor : this.settings.beamColor);
		this.baseSpriteRenderer.color = Color.white.WithAlpha(this._transition * num3);
		float num4 = GSR.Runner.direction * (base.transform.position.x - GSR.Runner.position.x);
		float num5 = this.settings.beamFadeInDist.InverseLerp(num4);
		this.beamSpriteRenderer.color = color.WithAlpha(this._transition * (1f - num5) * num3);
		if (this._hiding && this._success)
		{
			float num6 = this.settings.flashDuration / this.settings.successTransitionTime;
			float num7 = Mathf.InverseLerp(1f, 1f - num6, this._transition);
			Color flashColor = this.settings.flashColor;
			flashColor.a *= this.settings.flashAlpha.Evaluate(num7);
			this.flashSpriteRenderer.color = flashColor;
			this.flashSpriteRenderer.transform.localScale = this.settings.flashScale.Evaluate(num7) * Vector3.one;
		}
		if (this._hiding)
		{
			if (this._success)
			{
				this.baseSpriteRenderer.transform.localScale = Vector3.Scale(this.settings.overallScale, this.settings.baseSuccessScale);
				this.beamSpriteRenderer.transform.localScale = Vector3.Lerp(Vector3.Scale(this.settings.overallScale, this.settings.beamSuccessScale), this.settings.overallScale, this._transition);
				this.beamSpriteRenderer.transform.localPosition = (1f - this._transition) * this.settings.beamSuccessYPos * Vector3.up;
			}
			else
			{
				Vector3 vector = Vector3.Lerp(Vector3.Scale(this.settings.overallScale, this.settings.failScale), this.settings.overallScale, this._transition);
				this.baseSpriteRenderer.transform.localScale = vector;
				this.beamSpriteRenderer.transform.localScale = vector;
			}
		}
		else
		{
			this.baseSpriteRenderer.transform.localScale = this.settings.overallScale;
			this.beamSpriteRenderer.transform.localScale = this.settings.overallScale;
		}
		if (this._hiding && this._transition == 0f && this.prototype != null)
		{
			this.prototype.ReturnToPool();
		}
	}

	// Token: 0x040004B2 RID: 1202
	public BeatTrack.ObstacleRef obstacleRef;

	// Token: 0x040004B3 RID: 1203
	[NonSerialized]
	public Vector3 targetPos;

	// Token: 0x040004B4 RID: 1204
	[NonSerialized]
	public bool isSpecialJump;

	// Token: 0x040004B5 RID: 1205
	[NonSerialized]
	public RhythmActionMarkerPrompt buttonPrompt;

	// Token: 0x040004B6 RID: 1206
	public RhythmActionMarkerSettings settings;

	// Token: 0x040004B7 RID: 1207
	public SpriteRenderer baseSpriteRenderer;

	// Token: 0x040004B8 RID: 1208
	public SpriteRenderer beamSpriteRenderer;

	// Token: 0x040004B9 RID: 1209
	public SpriteRenderer flashSpriteRenderer;

	// Token: 0x040004BA RID: 1210
	private Prototype _prototype;

	// Token: 0x040004BB RID: 1211
	private bool _hiding;

	// Token: 0x040004BC RID: 1212
	private float _transition;

	// Token: 0x040004BD RID: 1213
	private bool _success;

	// Token: 0x02000298 RID: 664
	public enum HideReason
	{
		// Token: 0x04001535 RID: 5429
		NotMusicRunning,
		// Token: 0x04001536 RID: 5430
		Passed,
		// Token: 0x04001537 RID: 5431
		OutOfSync,
		// Token: 0x04001538 RID: 5432
		Jump,
		// Token: 0x04001539 RID: 5433
		FinalJump
	}
}
