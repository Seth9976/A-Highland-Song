using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class LightningEffectsController : MonoSingleton<LightningEffectsController>
{
	// Token: 0x06000BC1 RID: 3009 RVA: 0x0005E93B File Offset: 0x0005CB3B
	private void OnEnable()
	{
		this.nextTime = Time.unscaledTime + Random.Range(this.settings.randomTimeBetweenFlashes.x, this.settings.randomTimeBetweenFlashes.y);
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0005E970 File Offset: 0x0005CB70
	private void Update()
	{
		float num = Mathf.MoveTowards(this.strength, this.targetStrength, Time.unscaledDeltaTime * 0.1f);
		if (this.strength != num)
		{
			if (this.strength == 0f && num > 0f)
			{
				num = 1f;
				this.nextTime = Time.unscaledTime;
			}
			this.strength = num;
		}
		if (Time.unscaledTime >= this.nextTime)
		{
			this.falloffCurveIndex = this.settings.falloffCurves.RandomIndex<AnimationCurve>();
			this.lastTime = Time.unscaledTime;
			this.nextTime = Time.unscaledTime + Random.Range(this.settings.randomTimeBetweenFlashes.x, this.settings.randomTimeBetweenFlashes.y);
			if (this.strength > 0f)
			{
				this.audioPlayer.PlayOneShot(this.thunderClips.GetRandomClip(), this.strength);
				GameCamera.instance.cameraShakeState.StartShake(CameraShakeName.Lightning);
				if (this.onLightningFlash != null)
				{
					this.onLightningFlash(this.strength);
				}
			}
		}
		this.currentFlashStrength = Mathf.Clamp01(this.strength * this.settings.falloffCurves[this.falloffCurveIndex].Evaluate(Time.unscaledTime - this.lastTime));
		Shader.SetGlobalFloat("_LightningStrength", this.currentFlashStrength);
		float num2 = Mathf.Pow(this.currentFlashStrength * this.settings.backlightMultiplier, this.settings.backlightPower);
		this.flash.color = (this.lightingFlash.color = (this.backlightFlash.color = Color.white.WithAlpha(num2)));
		this.flash.enabled = (this.lightingFlash.enabled = (this.backlightFlash.enabled = num2 > 0f));
	}

	// Token: 0x04000DE0 RID: 3552
	public LightningEffectsControllerSettings settings;

	// Token: 0x04000DE1 RID: 3553
	public AudioSource audioPlayer;

	// Token: 0x04000DE2 RID: 3554
	public AudioClipSet thunderClips;

	// Token: 0x04000DE3 RID: 3555
	public float targetStrength;

	// Token: 0x04000DE4 RID: 3556
	public float strength;

	// Token: 0x04000DE5 RID: 3557
	public SpriteRenderer backlightFlash;

	// Token: 0x04000DE6 RID: 3558
	public SpriteRenderer lightingFlash;

	// Token: 0x04000DE7 RID: 3559
	public SpriteRenderer flash;

	// Token: 0x04000DE8 RID: 3560
	public int falloffCurveIndex;

	// Token: 0x04000DE9 RID: 3561
	public float currentFlashStrength;

	// Token: 0x04000DEA RID: 3562
	public float lastTime;

	// Token: 0x04000DEB RID: 3563
	public float nextTime;

	// Token: 0x04000DEC RID: 3564
	public Action<float> onLightningFlash;
}
