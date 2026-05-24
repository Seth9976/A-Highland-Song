using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200008B RID: 139
[RequireComponent(typeof(Animation))]
[ExecuteInEditMode]
public class InkAnimation : MonoBehaviour
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060003EE RID: 1006 RVA: 0x00020153 File Offset: 0x0001E353
	public bool isPlaying
	{
		get
		{
			return this.animation.isPlaying;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060003EF RID: 1007 RVA: 0x00020160 File Offset: 0x0001E360
	public bool isLooping
	{
		get
		{
			AnimationState animationState = null;
			foreach (object obj in this.animation)
			{
				AnimationState animationState2 = (AnimationState)obj;
				if (animationState2.enabled && (animationState == null || animationState2.weight >= animationState.weight))
				{
					animationState = animationState2;
				}
			}
			return animationState != null && animationState.wrapMode == WrapMode.Loop;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000201EC File Offset: 0x0001E3EC
	public Animation animation
	{
		get
		{
			if (this._animation == null)
			{
				this._animation = base.GetComponent<Animation>();
			}
			return this._animation;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0002020E File Offset: 0x0001E40E
	public static Dictionary<string, InkAnimation> all
	{
		get
		{
			return InkAnimation._animators;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00020218 File Offset: 0x0001E418
	public static bool anyNonLoopingActive
	{
		get
		{
			foreach (InkAnimation inkAnimation in InkAnimation._animators.Values)
			{
				if (inkAnimation.isPlaying && !inkAnimation.isLooping)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00020280 File Offset: 0x0001E480
	public static List<InkAnimation> activeNonLoopingAnims
	{
		get
		{
			InkAnimation._activeNonLoopingAnims.Clear();
			foreach (InkAnimation inkAnimation in InkAnimation._animators.Values)
			{
				if (inkAnimation.isPlaying && !inkAnimation.isLooping)
				{
					InkAnimation._activeNonLoopingAnims.Add(inkAnimation);
				}
			}
			return InkAnimation._activeNonLoopingAnims;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000202FC File Offset: 0x0001E4FC
	public string debugInfo
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("animPlayOrder: ");
			stringBuilder.Append(string.Join(", ", this.animPlayOrder));
			stringBuilder.Append(". ");
			if (this.animation.clip != null)
			{
				stringBuilder.Append(string.Format("animation.clip = {0}, wrapMode = {1}. ", this.animation.clip.name, this.animation.clip.wrapMode));
			}
			else
			{
				stringBuilder.Append("animation.clip is null. ");
			}
			foreach (object obj in this.animation)
			{
				AnimationState animationState = (AnimationState)obj;
				stringBuilder.Append(string.Format("AnimationState {0} enabled={1}, clip={2}, speed={3}, normalizedTime={4}, weight={5}, wrapMode={6}, layer={7}, blendMode={8}.", new object[] { animationState.name, animationState.enabled, animationState.clip, animationState.speed, animationState.normalizedTime, animationState.weight, animationState.wrapMode, animationState.layer, animationState.blendMode }));
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x00020474 File Offset: 0x0001E674
	public void Begin(string specificAnimName = null, bool queue = false)
	{
		if (specificAnimName != null)
		{
			if (this.animation.GetClip(specificAnimName) == null)
			{
				Debug.LogError("Specific animation not found: " + specificAnimName);
				return;
			}
			if (queue)
			{
				this.animation.CrossFadeQueued(specificAnimName, 0.5f, QueueMode.CompleteOthers);
			}
			else
			{
				this.animation.Play(specificAnimName, PlayMode.StopAll);
			}
			if (!this.animPlayOrder.Contains(specificAnimName))
			{
				this.animPlayOrder.Add(specificAnimName);
			}
		}
		else
		{
			if (this.animation.clip == null)
			{
				Debug.LogError("Can't add anim clip to play order for serialization since there isn't one?");
			}
			else
			{
				this.animPlayOrder.Clear();
				this.animPlayOrder.Add(this.animation.clip.name);
			}
			this.animation.Play();
		}
		if (this.isCameraTarget)
		{
			GameCamera.instance.SetAnimationActive(this);
		}
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0002054F File Offset: 0x0001E74F
	public static IEnumerator AwaitCompletion(string animatorName)
	{
		InkAnimation anim;
		if (InkAnimation._animators.TryGetValue(animatorName, out anim))
		{
			while (anim.animation.isPlaying)
			{
				yield return null;
			}
		}
		else
		{
			for (;;)
			{
				using (Dictionary<string, InkAnimation>.ValueCollection.Enumerator enumerator = InkAnimation._animators.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.animation.isPlaying)
						{
							yield return null;
							goto IL_00D8;
						}
					}
				}
				break;
				IL_00D8:;
			}
			Dictionary<string, InkAnimation>.ValueCollection.Enumerator enumerator = default(Dictionary<string, InkAnimation>.ValueCollection.Enumerator);
		}
		yield break;
		yield break;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00020560 File Offset: 0x0001E760
	public static void StopAndResetAll()
	{
		foreach (InkAnimation inkAnimation in InkAnimation._animators.Values)
		{
			inkAnimation.StopAndReset(null);
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x000205B8 File Offset: 0x0001E7B8
	public void StopAndReset(string[] reapplyPlayOrder = null)
	{
		this.animation.Stop();
		for (int i = this.animPlayOrder.Count - 1; i >= 0; i--)
		{
			string text = this.animPlayOrder[i];
			AnimationState animationState = this.animation[text];
			animationState.speed = 0f;
			animationState.normalizedTime = 0f;
			animationState.weight = 1f;
			animationState.enabled = true;
			this.animation.Sample();
			animationState.speed = 1f;
			animationState.weight = 0f;
			animationState.enabled = false;
		}
		this.animPlayOrder.Clear();
		if (reapplyPlayOrder != null && reapplyPlayOrder.Length != 0)
		{
			foreach (string text2 in reapplyPlayOrder)
			{
				AnimationState animationState2 = this.animation[text2];
				animationState2.speed = 0f;
				animationState2.normalizedTime = 1f;
				animationState2.weight = 1f;
				animationState2.enabled = true;
				this.animation.Sample();
				animationState2.speed = 1f;
				animationState2.weight = 0f;
				animationState2.enabled = false;
			}
			string text3 = reapplyPlayOrder[reapplyPlayOrder.Length - 1];
			if (this.animation[text3].clip.wrapMode == WrapMode.Loop)
			{
				this.animation.Play(text3);
			}
			this.animPlayOrder.AddRange(reapplyPlayOrder);
			return;
		}
		if (this.defaultLoopingAnim != null)
		{
			this.animation.clip = this.defaultLoopingAnim;
			this.animation.Play();
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x00020744 File Offset: 0x0001E944
	private void OnEnable()
	{
		InkAnimation._animators[base.name] = this;
		if (!Application.isPlaying)
		{
			Animation component = base.GetComponent<Animation>();
			component.hideFlags = HideFlags.HideInInspector;
			component.playAutomatically = false;
		}
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00020771 File Offset: 0x0001E971
	private void OnDisable()
	{
		InkAnimation._animators.Remove(base.name);
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00020784 File Offset: 0x0001E984
	private void Update()
	{
		if (this.defaultLoopingAnim != null && !this.animation.isPlaying && this.animPlayOrder.Count == 0 && this.defaultLoopingAnim.wrapMode == WrapMode.Loop)
		{
			this.animation.clip = this.defaultLoopingAnim;
			this.animation.Play();
		}
	}

	// Token: 0x04000542 RID: 1346
	public bool isCameraTarget;

	// Token: 0x04000543 RID: 1347
	public float cameraBlendInTime = 1f;

	// Token: 0x04000544 RID: 1348
	public float cameraBlendOutTime = 1f;

	// Token: 0x04000545 RID: 1349
	public AnimationClip defaultLoopingAnim;

	// Token: 0x04000546 RID: 1350
	[NonSerialized]
	public List<string> animPlayOrder = new List<string>(8);

	// Token: 0x04000547 RID: 1351
	private Animation _animation;

	// Token: 0x04000548 RID: 1352
	private static List<InkAnimation> _activeNonLoopingAnims = new List<InkAnimation>();

	// Token: 0x04000549 RID: 1353
	private static Dictionary<string, InkAnimation> _animators = new Dictionary<string, InkAnimation>(StringComparer.OrdinalIgnoreCase);
}
