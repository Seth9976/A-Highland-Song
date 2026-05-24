using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class PeakStateAudioController : MonoBehaviour
{
	// Token: 0x0600004A RID: 74 RVA: 0x00005BFC File Offset: 0x00003DFC
	private void Update()
	{
		if (AudioController.instance == null)
		{
			return;
		}
		AudioController.instance.audioMixer.SetFloat("Telescope Volume", AudioUtils.LinearVolumeToDBVolume((float)(Game.instance.inPeakState ? 1 : 0)));
		if (!Game.instance.inPeakState)
		{
			return;
		}
		float num = 0f;
		float num2 = ((Time.deltaTime == 0f) ? 0f : (Mathf.Abs(this.lastZoomTransitionSmoothed - this.peakStateController.zoomTransitionSmoothed) / Time.deltaTime));
		this.lastZoomTransitionSmoothed = this.peakStateController.zoomTransitionSmoothed;
		num += this.settings.whooshOverZoomSpeed.Evaluate(num2);
		this.smoothedMoveSpeed = Mathf.SmoothDamp(this.smoothedMoveSpeed, GameCamera.instance.peakState.panningSpeed * GameCamera.instance.peakState.cameraProperties.viewportScale, ref this.smoothedMoveSpeedVelocity, this.settings.speedSmoothing);
		num += this.settings.whooshOverMoveSpeed.Evaluate(this.smoothedMoveSpeed);
		this._whoosh = Mathf.MoveTowards(this._whoosh, num, this.settings.whooshDamping * Time.deltaTime);
		this.zoomSource.volume = 0f;
		this.moveSource.pitch = this.settings.whooshPitchOverStrength.Evaluate(this._whoosh);
		this.moveSource.volume = this.settings.whooshVolumeOverStrength.Evaluate(this._whoosh);
	}

	// Token: 0x0400005F RID: 95
	public PeakStateAudioControllerSettings settings;

	// Token: 0x04000060 RID: 96
	public PeakStateController peakStateController;

	// Token: 0x04000061 RID: 97
	public AudioSource moveSource;

	// Token: 0x04000062 RID: 98
	public AudioSource zoomSource;

	// Token: 0x04000063 RID: 99
	private float lastZoomTransitionSmoothed;

	// Token: 0x04000064 RID: 100
	public float smoothedMoveSpeed;

	// Token: 0x04000065 RID: 101
	public float smoothedMoveSpeedVelocity;

	// Token: 0x04000066 RID: 102
	private float _whoosh;
}
