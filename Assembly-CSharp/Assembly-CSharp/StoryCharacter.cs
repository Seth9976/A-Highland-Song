using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class StoryCharacter : MonoBehaviour
{
	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000518 RID: 1304 RVA: 0x00028942 File Offset: 0x00026B42
	private AudioSource audioSource
	{
		get
		{
			if (this._audioSource == null)
			{
				this._audioSource = base.GetComponent<AudioSource>();
			}
			return this._audioSource;
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00028964 File Offset: 0x00026B64
	public static StoryCharacter WithName(string name)
	{
		StoryCharacter storyCharacter = null;
		StoryCharacter._allCharacters.TryGetValue(name, out storyCharacter);
		return storyCharacter;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00028984 File Offset: 0x00026B84
	public void SetCustomPose(string transitionInAnimName, string animName)
	{
		if (string.IsNullOrWhiteSpace(animName))
		{
			this.activeInkPoseAnim = null;
			this.animator.SetAnimation(this.idle, 0, FrameAnimator.PosMatch.None);
			return;
		}
		this.activeInkPoseAnim = animName;
		if (transitionInAnimName != null)
		{
			this.animator.SetAnimationWithTransition(transitionInAnimName, this.activeInkPoseAnim, 0, false, false, FrameAnimator.PosMatch.None);
			return;
		}
		this.animator.SetAnimation(this.activeInkPoseAnim, FrameAnimator.PosMatch.None);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x000289E8 File Offset: 0x00026BE8
	private void Awake()
	{
		Prop component = base.GetComponent<Prop>();
		StoryCharacter._allCharacters[(component != null) ? component.inkListItemName : base.name] = this;
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00028A1E File Offset: 0x00026C1E
	private void Update()
	{
		this.UpdateAmbientAnims();
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00028A28 File Offset: 0x00026C28
	private void UpdateAmbientAnims()
	{
		if (this.ambientAnims == null || this.ambientAnims.Length == 0)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(this.activeInkPoseAnim))
		{
			return;
		}
		if (this._ambientAnimIdx == -1)
		{
			this._ambientAnimIdx = this.ambientAnims.RandomIndex<StoryCharacter.AmbientAnim>();
			StoryCharacter.AmbientAnim ambientAnim = this.ambientAnims[this._ambientAnimIdx];
			this._ambientAnimStartTime = Time.time + ambientAnim.waitDuration.RandomBell(3);
			this._ambientAnimDuration = ambientAnim.duration.RandomBell(3);
			return;
		}
		StoryCharacter.AmbientAnim ambientAnim2 = this.ambientAnims[this._ambientAnimIdx];
		if (Time.time > this._ambientAnimStartTime + this._ambientAnimDuration)
		{
			if (this.animator.IsAnimation(ambientAnim2.mainAnim.name, null, null, null))
			{
				if (ambientAnim2.transitionOut != null)
				{
					this.animator.SetAnimationWithTransition(ambientAnim2.transitionOut.name, this.idle.name, 0, false, false, FrameAnimator.PosMatch.None);
				}
				else
				{
					this.animator.SetAnimation(this.idle.name, FrameAnimator.PosMatch.None);
					this._ambientAnimIdx = -1;
				}
			}
			else if (this.animator.IsAnimation(this.idle.name, null, null, null))
			{
				this._ambientAnimIdx = -1;
			}
		}
		else if (Time.time > this._ambientAnimStartTime && this.animator.IsAnimation(this.idle.name, null, null, null))
		{
			if (ambientAnim2.transitionIn != null)
			{
				this.animator.SetAnimationWithTransition(ambientAnim2.transitionIn.name, ambientAnim2.mainAnim.name, 0, false, false, FrameAnimator.PosMatch.None);
			}
			else
			{
				this.animator.SetAnimation(ambientAnim2.mainAnim.name, FrameAnimator.PosMatch.None);
			}
		}
		if (ambientAnim2.audioClips != null && ambientAnim2.audioClips.Length != 0 && this.animator.IsAnimation(ambientAnim2.mainAnim.name, null, null, null) && this.animator.JustPassedTime(ambientAnim2.audioTimeNorm))
		{
			AudioClip audioClip = ambientAnim2.audioClips.Random<AudioClip>();
			this.audioSource.clip = audioClip;
			this.audioSource.Play();
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00028C47 File Offset: 0x00026E47
	private void OnDestroy()
	{
		StoryCharacter._allCharacters.Remove(base.name);
	}

	// Token: 0x040005E5 RID: 1509
	public FrameAnimator animator;

	// Token: 0x040005E6 RID: 1510
	public Transform fallbackMouthTransform;

	// Token: 0x040005E7 RID: 1511
	public FrameAnimation idle;

	// Token: 0x040005E8 RID: 1512
	[NonSerialized]
	public string activeInkPoseAnim;

	// Token: 0x040005E9 RID: 1513
	public StoryCharacter.AmbientAnim[] ambientAnims;

	// Token: 0x040005EA RID: 1514
	[NonSerialized]
	public int lastDialogueBubbleAngleIdx = -1;

	// Token: 0x040005EB RID: 1515
	[NonSerialized]
	public float lastDialogueBubbleTime = -1f;

	// Token: 0x040005EC RID: 1516
	private AudioSource _audioSource;

	// Token: 0x040005ED RID: 1517
	private int _ambientAnimIdx = -1;

	// Token: 0x040005EE RID: 1518
	private float _ambientAnimStartTime;

	// Token: 0x040005EF RID: 1519
	private float _ambientAnimDuration;

	// Token: 0x040005F0 RID: 1520
	private static Dictionary<string, StoryCharacter> _allCharacters = new Dictionary<string, StoryCharacter>(StringComparer.OrdinalIgnoreCase);

	// Token: 0x020002CC RID: 716
	[Serializable]
	public struct AmbientAnim
	{
		// Token: 0x04001638 RID: 5688
		public FrameAnimation transitionIn;

		// Token: 0x04001639 RID: 5689
		public FrameAnimation mainAnim;

		// Token: 0x0400163A RID: 5690
		public FrameAnimation transitionOut;

		// Token: 0x0400163B RID: 5691
		public Range duration;

		// Token: 0x0400163C RID: 5692
		public Range waitDuration;

		// Token: 0x0400163D RID: 5693
		public AudioClip[] audioClips;

		// Token: 0x0400163E RID: 5694
		public float audioTimeNorm;
	}
}
