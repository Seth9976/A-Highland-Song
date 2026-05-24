using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class SLayoutAnimator : MonoBehaviour
{
	// Token: 0x06001037 RID: 4151 RVA: 0x00078FBC File Offset: 0x000771BC
	public bool IsAnimating(SLayout target)
	{
		using (List<SLayoutAnimation>.Enumerator enumerator = this._animations.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.owner == target)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0007901C File Offset: 0x0007721C
	public void StartAnimation(SLayoutAnimation anim)
	{
		anim.Start();
		if (!anim.isComplete)
		{
			this._animations.Add(anim);
		}
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x00079038 File Offset: 0x00077238
	public void AddDelay(float extraDelay)
	{
		SLayoutAnimation slayoutAnimation = SLayoutAnimation.AnimationUnderDefinition();
		if (slayoutAnimation != null)
		{
			slayoutAnimation.AddDelay(extraDelay);
		}
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x00079058 File Offset: 0x00077258
	public void AddDuration(float extraDuration)
	{
		SLayoutAnimation slayoutAnimation = SLayoutAnimation.AnimationUnderDefinition();
		if (slayoutAnimation != null)
		{
			slayoutAnimation.AddDuration(extraDuration);
		}
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x00079078 File Offset: 0x00077278
	public void Animatable(Action<float> customAnim)
	{
		SLayoutAnimation slayoutAnimation = SLayoutAnimation.AnimationUnderDefinition();
		if (slayoutAnimation != null)
		{
			slayoutAnimation.AddCustomAnim(customAnim);
			return;
		}
		customAnim(1f);
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x000790A4 File Offset: 0x000772A4
	public void CancelAnimations(SLayout target)
	{
		for (int i = 0; i < this._animations.Count; i++)
		{
			SLayoutAnimation slayoutAnimation = this._animations[i];
			if (slayoutAnimation.owner == target)
			{
				slayoutAnimation.Cancel();
				this._animationsToRemove.Add(slayoutAnimation);
			}
		}
		foreach (SLayoutAnimation slayoutAnimation2 in this._animationsToRemove)
		{
			this._animations.Remove(slayoutAnimation2);
		}
		this._animationsToRemove.Clear();
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0007914C File Offset: 0x0007734C
	public void CompleteAnimations(SLayout target)
	{
		for (int i = 0; i < this._animations.Count; i++)
		{
			SLayoutAnimation slayoutAnimation = this._animations[i];
			if (!(slayoutAnimation.owner != target))
			{
				slayoutAnimation.CompleteImmediate();
			}
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x0600103E RID: 4158 RVA: 0x00079190 File Offset: 0x00077390
	public static bool hasInstance
	{
		get
		{
			return SLayoutAnimator._instance != null;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600103F RID: 4159 RVA: 0x0007919D File Offset: 0x0007739D
	public static SLayoutAnimator instance
	{
		get
		{
			if (SLayoutAnimator._instance == null)
			{
				SLayoutAnimator._instance = Object.FindObjectOfType<SLayoutAnimator>();
				if (SLayoutAnimator._instance == null)
				{
					Debug.LogWarning("No SLayoutAnimator found. Please create one in your main scene.");
				}
			}
			return SLayoutAnimator._instance;
		}
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x000791D2 File Offset: 0x000773D2
	private void OnDisable()
	{
		this._animations.Clear();
		this._animationsToRemove.Clear();
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x000791EC File Offset: 0x000773EC
	private void Update()
	{
		if (this._animations.Count > 0)
		{
			for (int i = 0; i < this._animations.Count; i++)
			{
				SLayoutAnimation slayoutAnimation = this._animations[i];
				if (!slayoutAnimation.canAnimate)
				{
					this._animationsToRemove.Add(slayoutAnimation);
				}
				else
				{
					slayoutAnimation.Update();
					if (slayoutAnimation.isComplete)
					{
						this._animationsToRemove.Add(slayoutAnimation);
					}
				}
			}
			foreach (SLayoutAnimation slayoutAnimation2 in this._animationsToRemove)
			{
				this._animations.Remove(slayoutAnimation2);
			}
			this._animationsToRemove.Clear();
		}
	}

	// Token: 0x04001210 RID: 4624
	private List<SLayoutAnimation> _animations = new List<SLayoutAnimation>();

	// Token: 0x04001211 RID: 4625
	private List<SLayoutAnimation> _animationsToRemove = new List<SLayoutAnimation>();

	// Token: 0x04001212 RID: 4626
	private static SLayoutAnimator _instance;
}
