using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class FootstepAudioManager : MonoBehaviour
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000615 RID: 1557 RVA: 0x00030B65 File Offset: 0x0002ED65
	private Runner runner
	{
		get
		{
			return Runner.instance;
		}
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00030B6C File Offset: 0x0002ED6C
	private void OnEnable()
	{
		Runner.onFootstep = (Action<int>)Delegate.Combine(Runner.onFootstep, new Action<int>(this.OnFootstep));
		Runner.onJumpEnd = (Action)Delegate.Combine(Runner.onJumpEnd, new Action(this.OnJumpEnd));
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00030BBC File Offset: 0x0002EDBC
	private void OnDisable()
	{
		Runner.onFootstep = (Action<int>)Delegate.Remove(Runner.onFootstep, new Action<int>(this.OnFootstep));
		Runner.onJumpEnd = (Action)Delegate.Remove(Runner.onJumpEnd, new Action(this.OnJumpEnd));
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00030C09 File Offset: 0x0002EE09
	private void OnFootstep(int footstep)
	{
		if (this.runner.balancing)
		{
			Debug.Log("LAND ON BALANCE POINT");
			return;
		}
		this.PlayFootstepSound(false);
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00030C2A File Offset: 0x0002EE2A
	private void OnJumpEnd()
	{
		if (this.runner.balancing)
		{
			this.PlaySteppingStoneFootstepSound();
			return;
		}
		this.PlayFootstepSound(true);
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00030C48 File Offset: 0x0002EE48
	private void PlaySteppingStoneFootstepSound()
	{
		float num = this.footstepSFXSettings.steppingStoneVolume;
		if (this.runner.isMusicRunning)
		{
			num *= this.footstepSFXSettings.runningGameplaySteppingStoneVolumeScale;
		}
		AudioClip audioClip = this.footstepSFXSettings.steppingStoneClip.Random<AudioClip>();
		this.footstepSource.PlayOneShot(audioClip, num);
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x00030C9C File Offset: 0x0002EE9C
	private void PlayFootstepSound(bool jumpLand)
	{
		FootstepAudioClipSet footstepAudioClipSet = this.footstepSFXSettings.footstepAudioClipDatabase.TryGetClipSetFromSurfaceType(Runner.instance.surfaceTypeSampler.surfaceType);
		if (footstepAudioClipSet == null)
		{
			return;
		}
		float num = this.footstepSFXSettings.footstepVolume;
		num *= this.footstepSFXSettings.footstepVolumeOverSlopeAngle.Evaluate(Mathf.Abs(this.runner.relativeSlopeAngle));
		if (this.runner.isMusicRunning)
		{
			num *= this.footstepSFXSettings.runningGameplayFootstepVolumeScale;
		}
		float num2 = this.footstepSFXSettings.footstepVolume;
		if (this.runner.isMusicRunning)
		{
			num2 *= this.footstepSFXSettings.runningGameplayFootstepVolumeScale;
		}
		AudioClip audioClip = null;
		bool flag = this.runner.speed > this.footstepSFXSettings.walkRunThreshold;
		AudioClip audioClip2;
		if (jumpLand)
		{
			audioClip2 = footstepAudioClipSet.landing.GetRandomClip();
		}
		else if (flag)
		{
			audioClip2 = footstepAudioClipSet.running.GetRandomClip();
		}
		else
		{
			audioClip2 = footstepAudioClipSet.walking.GetRandomClip();
		}
		if (footstepAudioClipSet.additional != null)
		{
			audioClip = footstepAudioClipSet.additional.GetRandomClip();
		}
		if (audioClip2 != null && num > 0f)
		{
			this.footstepSource.PlayOneShot(audioClip2, num);
		}
		if (audioClip != null && num2 > 0f)
		{
			this.footstepSource.PlayOneShot(audioClip, num2);
		}
	}

	// Token: 0x04000710 RID: 1808
	public FootstepSFXSettings footstepSFXSettings;

	// Token: 0x04000711 RID: 1809
	[Space]
	public AudioSource footstepSource;
}
