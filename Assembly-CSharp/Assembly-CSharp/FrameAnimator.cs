using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B3 RID: 179
[NullableContext(2)]
[Nullable(0)]
public class FrameAnimator : MonoBehaviour
{
	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600059B RID: 1435 RVA: 0x0002C382 File Offset: 0x0002A582
	// (set) Token: 0x0600059C RID: 1436 RVA: 0x0002C38A File Offset: 0x0002A58A
	public FrameAnimation currentAnimation
	{
		get
		{
			return this._currentAnimation;
		}
		set
		{
			if (this._currentAnimation != value)
			{
				this._currentAnimation = value;
				this._prevFrameIdx = -1;
			}
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x0002C3A8 File Offset: 0x0002A5A8
	public FrameAnimation currentAnimationVariant
	{
		get
		{
			if (this.currentAnimation == null)
			{
				return null;
			}
			if (this._animationVariantNumber == 0)
			{
				return this.currentAnimation;
			}
			if (this.currentAnimation.variants == null || this.currentAnimation.variants.Length == 0)
			{
				return this.currentAnimation;
			}
			int num = this._animationVariantNumber - 1;
			if (num < 0 || num >= this.currentAnimation.variants.Length)
			{
				return this.currentAnimation;
			}
			return this.currentAnimation.variants[num];
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x0600059E RID: 1438 RVA: 0x0002C428 File Offset: 0x0002A628
	public Sprite activeFrame
	{
		get
		{
			if (this._spriteRenderer == null && this._image == null && this.spriteRenderer == null && this.image == null)
			{
				return null;
			}
			if (this._spriteRenderer != null)
			{
				return this._spriteRenderer.sprite;
			}
			return this._image.sprite;
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x0600059F RID: 1439 RVA: 0x0002C494 File Offset: 0x0002A694
	public bool inSecondHalfOfReversal
	{
		get
		{
			return this._currentAnimation != null && this._currentAnimation.reversesDirection && this.frameIdx >= this._currentAnimation.frames.Length / 2;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0002C4D0 File Offset: 0x0002A6D0
	public Vector3 mouthPosition
	{
		get
		{
			return this.AnnotationPos<Vector2>(this.currentAnimationVariant, (this.currentAnimationVariant != null) ? this.currentAnimationVariant.mouthPositions : null, (Vector2 p) => p, base.transform.TransformPoint(this.defaultMouthPos));
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0002C53C File Offset: 0x0002A73C
	public Vector3 headTorchPos
	{
		get
		{
			return this.AnnotationPos<FrameAnimation.HeadTorchPos>(this.currentAnimationVariant, (this.currentAnimationVariant != null) ? this.currentAnimationVariant.headTorchPositions : null, (FrameAnimation.HeadTorchPos p) => p.pos, this.mouthPosition + base.transform.TransformVector(this.defaultHeadTorchOffsetFromMouth));
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0002C5B4 File Offset: 0x0002A7B4
	public float headTorchAngle
	{
		get
		{
			float num = 0f;
			FrameAnimation currentAnimationVariant = this.currentAnimationVariant;
			if (currentAnimationVariant != null && currentAnimationVariant.headTorchPositions != null && currentAnimationVariant.headTorchPositions.Count > 0)
			{
				int num2 = Mathf.Min(this.frameIdx % currentAnimationVariant.headTorchPositions.Count, currentAnimationVariant.headTorchPositions.Count - 1);
				num = currentAnimationVariant.headTorchPositions[num2].angle;
			}
			return num;
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0002C628 File Offset: 0x0002A828
	private Vector3 AnnotationPos<T>(FrameAnimation anim, [Nullable(new byte[] { 2, 1 })] List<T> list, [Nullable(1)] Func<T, Vector2> getPos, Vector3 defaultPos)
	{
		if (anim != null && list != null && list.Count > 0)
		{
			int num = this.frameIdx % anim.frames.Length;
			int num2 = Mathf.Min(num, list.Count - 1);
			Vector2 vector = getPos(list[num2]);
			Sprite sprite = anim.frames[num];
			Vector2 vector2 = new Vector2(Mathf.Lerp(sprite.bounds.min.x, sprite.bounds.max.x, vector.x), Mathf.Lerp(sprite.bounds.min.y, sprite.bounds.max.y, vector.y));
			return base.transform.TransformPoint(vector2);
		}
		return defaultPos;
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0002C70C File Offset: 0x0002A90C
	public Vector3 frameBottomLeft
	{
		get
		{
			Vector2 vector = Vector2.zero;
			FrameAnimation currentAnimationVariant = this.currentAnimationVariant;
			if (currentAnimationVariant != null)
			{
				int num = this.frameIdx % currentAnimationVariant.frames.Length;
				vector = currentAnimationVariant.frames[num].bounds.min;
			}
			return base.transform.TransformPoint(vector);
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0002C76B File Offset: 0x0002A96B
	// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0002C773 File Offset: 0x0002A973
	public int frameIdx { get; private set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0002C77C File Offset: 0x0002A97C
	// (set) Token: 0x060005A8 RID: 1448 RVA: 0x0002C7EC File Offset: 0x0002A9EC
	public float normalizedTime
	{
		get
		{
			if (this.currentAnimation == null || this.currentAnimation.frames.Length == 0)
			{
				return 0f;
			}
			float num = (float)this.currentAnimation.frames.Length;
			if (this.currentAnimation.mode == FrameAnimation.Mode.Clamp && this.currentAnimation.frames.Length > 1)
			{
				num -= 1f;
			}
			return (float)this.frameIdx / num;
		}
		set
		{
			if (this.currentAnimation == null || this.currentAnimation.frames.Length == 0)
			{
				return;
			}
			int num = Mathf.RoundToInt(value * (float)this.currentAnimation.frames.Length);
			if (num == this.currentAnimation.frames.Length)
			{
				num = this.currentAnimation.frames.Length - 1;
			}
			this.SetFrameIdx(num, (this.speed >= 0f) ? 1 : (-1));
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0002C868 File Offset: 0x0002AA68
	public Vector2 remainingRootMotion
	{
		get
		{
			FrameAnimation currentAnimationVariant = this.currentAnimationVariant;
			if (currentAnimationVariant == null)
			{
				return Vector2.zero;
			}
			return currentAnimationVariant.RootMotionBetweenFrames(this.frameIdx, currentAnimationVariant.frames.Length - 1, (this.speed >= 0f) ? 1 : (-1));
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060005AA RID: 1450 RVA: 0x0002C8B2 File Offset: 0x0002AAB2
	[Nullable(1)]
	public SpriteRenderer spriteRenderer
	{
		[NullableContext(1)]
		get
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			return this._spriteRenderer;
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x0002C8D4 File Offset: 0x0002AAD4
	[Nullable(1)]
	public Image image
	{
		[NullableContext(1)]
		get
		{
			if (this._image == null)
			{
				this._image = base.GetComponent<Image>();
			}
			return this._image;
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
	[NullableContext(1)]
	[return: Nullable(2)]
	public FrameAnimation TryGetAnim(string name)
	{
		if (name == null)
		{
			return null;
		}
		FrameAnimation frameAnimation;
		if (!this._animationsByName.TryGetValue(name, out frameAnimation))
		{
			Debug.LogError("FrameAnimator couldn't set animation " + name + " since it couldn't be found", this);
			return null;
		}
		return frameAnimation;
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0002C934 File Offset: 0x0002AB34
	[NullableContext(1)]
	public void SetAnimation(string name, FrameAnimator.PosMatch posMatch = FrameAnimator.PosMatch.None)
	{
		FrameAnimation frameAnimation = this.TryGetAnim(name);
		if (frameAnimation == null)
		{
			return;
		}
		this.SetAnimation(frameAnimation, 0, posMatch);
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0002C95C File Offset: 0x0002AB5C
	public void SetAnimationVariantNumber(int variantNumber)
	{
		this._animationVariantNumber = 0;
		if (this._currentAnimation == null)
		{
			return;
		}
		if (this._currentAnimation.variants == null || this._currentAnimation.variants.Length == 0)
		{
			return;
		}
		this._animationVariantNumber = variantNumber;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0002C998 File Offset: 0x0002AB98
	[NullableContext(1)]
	public void SetAnimationWithTransition(FrameAnimation transitionAnim, FrameAnimation targetAnim, int targetAnimFirstFrameIdx = 0, bool forceTransition = false, bool reversed = false, FrameAnimator.PosMatch transitionPosMatch = FrameAnimator.PosMatch.None)
	{
		if (transitionAnim == null)
		{
			return;
		}
		if (targetAnim == null)
		{
			return;
		}
		if ((this.currentAnimation == transitionAnim && this._queuedAnim == targetAnim) || (targetAnim == this.currentAnimation && !forceTransition))
		{
			return;
		}
		int num = (reversed ? (transitionAnim.frames.Length - targetAnimFirstFrameIdx - 1) : 0);
		this.SetAnimation(transitionAnim, num, transitionPosMatch);
		this._queuedAnim = targetAnim;
		if (reversed && targetAnimFirstFrameIdx == 0)
		{
			targetAnimFirstFrameIdx = this._queuedAnim.frames.Length - targetAnimFirstFrameIdx - 1;
		}
		this._queuedFirstFrameIdx = targetAnimFirstFrameIdx;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0002CA30 File Offset: 0x0002AC30
	[NullableContext(1)]
	public void SetAnimationWithTransition(string transitionAnimName, string targetAnimName, int targetAnimFirstFrameIdx = 0, bool forceTransition = false, bool reversed = false, FrameAnimator.PosMatch transitionPosMatch = FrameAnimator.PosMatch.None)
	{
		FrameAnimation frameAnimation = this.TryGetAnim(transitionAnimName);
		if (frameAnimation == null)
		{
			return;
		}
		FrameAnimation frameAnimation2 = this.TryGetAnim(targetAnimName);
		if (frameAnimation2 == null)
		{
			return;
		}
		this.SetAnimationWithTransition(frameAnimation, frameAnimation2, targetAnimFirstFrameIdx, forceTransition, reversed, transitionPosMatch);
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0002CA70 File Offset: 0x0002AC70
	public bool IsAnimation([Nullable(1)] string name1, string name2 = null, string name3 = null, string name4 = null)
	{
		if (this.currentAnimation == null)
		{
			return name1 == null;
		}
		string name5 = this.currentAnimation.name;
		return name5 == name1 || name5 == name2 || name5 == name3 || name5 == name4;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0002CACC File Offset: 0x0002ACCC
	public bool IsAnimation([Nullable(1)] FrameAnimation anim1, FrameAnimation anim2 = null, FrameAnimation anim3 = null, FrameAnimation anim4 = null)
	{
		if (this._currentAnimation == null)
		{
			return anim1 == null;
		}
		return this._currentAnimation == anim1 || this._currentAnimation == anim2 || this._currentAnimation == anim3 || this._currentAnimation == anim4;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0002CB34 File Offset: 0x0002AD34
	public bool IsOrWillBeAnimation([Nullable(1)] string name1, string name2 = null, string name3 = null, string name4 = null, string name5 = null, string name6 = null)
	{
		if (this.currentAnimation == null)
		{
			return name1 == null;
		}
		string name7 = this.currentAnimation.name;
		if (name7 == name1)
		{
			return true;
		}
		if (name7 == name2)
		{
			return true;
		}
		if (name7 == name3)
		{
			return true;
		}
		if (name7 == name4)
		{
			return true;
		}
		if (name7 == name5)
		{
			return true;
		}
		if (name7 == name6)
		{
			return true;
		}
		if (this._queuedAnim != null)
		{
			string name8 = this._queuedAnim.name;
			if (name8 == name1)
			{
				return true;
			}
			if (name8 == name2)
			{
				return true;
			}
			if (name8 == name3)
			{
				return true;
			}
			if (name8 == name4)
			{
				return true;
			}
			if (name8 == name5)
			{
				return true;
			}
			if (name8 == name6)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0002CC05 File Offset: 0x0002AE05
	[NullableContext(1)]
	public bool HasAnimation(string name)
	{
		return this._animationsByName.ContainsKey(name);
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0002CC14 File Offset: 0x0002AE14
	[NullableContext(1)]
	public void SetAnimation(FrameAnimation anim, int firstFrameIdx = 0, FrameAnimator.PosMatch posMatch = FrameAnimator.PosMatch.None)
	{
		if (this.currentAnimation == anim || (anim != null && anim == this._queuedAnim))
		{
			return;
		}
		Vector3 vector = default(Vector3);
		if (posMatch == FrameAnimator.PosMatch.Mouth)
		{
			vector = this.mouthPosition;
		}
		else if (posMatch == FrameAnimator.PosMatch.Frame)
		{
			vector = this.frameBottomLeft;
		}
		this.currentAnimation = anim;
		this._animationVariantNumber = 0;
		this.SetFrameIdx(firstFrameIdx, 0);
		this._prevFrameIdx = -1;
		this._timeToSpend = 0f;
		if (this.onRootMotion != null && posMatch != FrameAnimator.PosMatch.None)
		{
			Vector3 vector2 = default(Vector3);
			if (posMatch == FrameAnimator.PosMatch.Mouth)
			{
				vector2 = this.mouthPosition;
			}
			else if (posMatch == FrameAnimator.PosMatch.Frame)
			{
				vector2 = this.frameBottomLeft;
			}
			Vector3 vector3 = vector2 - vector;
			this.onRootMotion(-vector3, true);
		}
		this._queuedAnim = null;
		this._queuedFirstFrameIdx = 0;
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0002CCE8 File Offset: 0x0002AEE8
	public bool SetFrameIdx(int idx, int rootMotionDir)
	{
		Sprite sprite = null;
		bool flag = false;
		int num = idx;
		FrameAnimation currentAnimationVariant = this.currentAnimationVariant;
		if (currentAnimationVariant != null)
		{
			if (currentAnimationVariant.mode == FrameAnimation.Mode.Clamp)
			{
				int num2 = currentAnimationVariant.frames.Length - 1;
				if (idx > num2)
				{
					idx = num2;
					flag = true;
				}
				else if (idx < 0)
				{
					idx = 0;
					flag = true;
				}
				num = idx;
			}
			else
			{
				flag = idx >= currentAnimationVariant.frames.Length || idx < 0;
				num = idx % currentAnimationVariant.frames.Length;
				if (num < 0)
				{
					num += currentAnimationVariant.frames.Length;
				}
			}
			sprite = currentAnimationVariant.frames[num];
		}
		if (rootMotionDir != 0 && currentAnimationVariant != null && this.onRootMotion != null && num != this.frameIdx)
		{
			Vector2 vector = currentAnimationVariant.RootMotionBetweenFrames(this.frameIdx, num, rootMotionDir);
			if (vector != Vector2.zero)
			{
				this.onRootMotion(vector, false);
			}
		}
		this.SetSprite(sprite);
		this._prevFrameIdx = this.frameIdx;
		this.frameIdx = num;
		if (currentAnimationVariant != null && this._spriteRenderer != null)
		{
			this._spriteRenderer.flipX = currentAnimationVariant.flipX;
		}
		return flag;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0002CDFC File Offset: 0x0002AFFC
	public bool JustPassedTime(float t)
	{
		if (this._prevFrameIdx == -1)
		{
			return false;
		}
		FrameAnimation currentAnimationVariant = this.currentAnimationVariant;
		if (currentAnimationVariant == null)
		{
			return false;
		}
		float num = (float)this._prevFrameIdx / currentAnimationVariant.duration;
		float num2 = (float)this.frameIdx / currentAnimationVariant.duration;
		if (num2 >= t && num < t)
		{
			return true;
		}
		if (num2 < num)
		{
			if (num2 >= t)
			{
				return true;
			}
			if (t > num)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0002CE60 File Offset: 0x0002B060
	private void SetSprite(Sprite sprite)
	{
		if (this._spriteRenderer == null && this._image == null && this.spriteRenderer == null && this.image == null)
		{
			return;
		}
		if (this._spriteRenderer != null)
		{
			this._spriteRenderer.sprite = sprite;
			return;
		}
		if (this._image != null)
		{
			this._image.sprite = sprite;
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0002CEDC File Offset: 0x0002B0DC
	private void Awake()
	{
		foreach (FrameAnimation frameAnimation in this.animations)
		{
			this._animationsByName[frameAnimation.name] = frameAnimation;
		}
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0002CF14 File Offset: 0x0002B114
	private void Update()
	{
		this.ManualUpdate(this.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002CF30 File Offset: 0x0002B130
	public void ManualUpdate(float dt)
	{
		FrameAnimation currentAnimationVariant = this.currentAnimationVariant;
		if (currentAnimationVariant == null)
		{
			this.SetSprite(null);
			return;
		}
		if (this.speed == 0f)
		{
			return;
		}
		if (currentAnimationVariant.fps == 0)
		{
			return;
		}
		this._timeToSpend += dt;
		int num = 0;
		while (this._timeToSpend > 0f && num < 10)
		{
			float num2 = 1f / (float)currentAnimationVariant.fps / Mathf.Abs(this.speed);
			if (currentAnimationVariant.timingAdjustments != null && this.frameIdx < currentAnimationVariant.timingAdjustments.Length)
			{
				float num3 = currentAnimationVariant.timingAdjustments[this.frameIdx];
				num2 += num3;
			}
			if (this._timeToSpend < num2)
			{
				break;
			}
			this._timeToSpend -= num2;
			int num4 = this.frameIdx + ((this.speed < 0f) ? (-1) : 1);
			bool inSecondHalfOfReversal = this.inSecondHalfOfReversal;
			bool flag = this.SetFrameIdx(num4, (this.speed >= 0f) ? 1 : (-1));
			num++;
			if (this.inSecondHalfOfReversal && !inSecondHalfOfReversal && this.onReverseDirection != null)
			{
				this.onReverseDirection();
			}
			if (flag && currentAnimationVariant.mode == FrameAnimation.Mode.Clamp && this._queuedAnim != null)
			{
				this.PlayQueuedAnimation();
				break;
			}
		}
		if (num >= 10)
		{
			this._timeToSpend = 0f;
		}
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0002D083 File Offset: 0x0002B283
	public void ForceCompleteTransition()
	{
		this.PlayQueuedAnimation();
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002D08C File Offset: 0x0002B28C
	private void PlayQueuedAnimation()
	{
		if (this._queuedAnim == null)
		{
			return;
		}
		FrameAnimation queuedAnim = this._queuedAnim;
		int queuedFirstFrameIdx = this._queuedFirstFrameIdx;
		this._queuedAnim = null;
		this._queuedFirstFrameIdx = 0;
		this.SetAnimation(queuedAnim, queuedFirstFrameIdx, FrameAnimator.PosMatch.None);
	}

	// Token: 0x04000672 RID: 1650
	public Action onReverseDirection;

	// Token: 0x04000673 RID: 1651
	public FrameAnimator.OnRootMotionDelegate onRootMotion;

	// Token: 0x04000674 RID: 1652
	public bool useUnscaledTime;

	// Token: 0x04000675 RID: 1653
	[SerializeField]
	private FrameAnimation _currentAnimation;

	// Token: 0x04000676 RID: 1654
	public Vector2 defaultMouthPos;

	// Token: 0x04000677 RID: 1655
	public Vector2 defaultHeadTorchOffsetFromMouth;

	// Token: 0x04000679 RID: 1657
	[Nullable(1)]
	public FrameAnimation[] animations = new FrameAnimation[0];

	// Token: 0x0400067A RID: 1658
	public float speed = 1f;

	// Token: 0x0400067B RID: 1659
	private SpriteRenderer _spriteRenderer;

	// Token: 0x0400067C RID: 1660
	private Image _image;

	// Token: 0x0400067D RID: 1661
	private int _animationVariantNumber;

	// Token: 0x0400067E RID: 1662
	private float _currFrameStartTime;

	// Token: 0x0400067F RID: 1663
	private FrameAnimation _queuedAnim;

	// Token: 0x04000680 RID: 1664
	private int _queuedFirstFrameIdx;

	// Token: 0x04000681 RID: 1665
	private bool _reverseDirectionPending;

	// Token: 0x04000682 RID: 1666
	private int _prevFrameIdx = -1;

	// Token: 0x04000683 RID: 1667
	[Nullable(1)]
	private Dictionary<string, FrameAnimation> _animationsByName = new Dictionary<string, FrameAnimation>();

	// Token: 0x04000684 RID: 1668
	private float _timeToSpend;

	// Token: 0x020002D5 RID: 725
	// (Invoke) Token: 0x0600164F RID: 5711
	[NullableContext(0)]
	public delegate void OnRootMotionDelegate(Vector2 motion, bool worldSpace);

	// Token: 0x020002D6 RID: 726
	[NullableContext(0)]
	public enum PosMatch
	{
		// Token: 0x0400165F RID: 5727
		None,
		// Token: 0x04001660 RID: 5728
		Mouth,
		// Token: 0x04001661 RID: 5729
		Frame
	}
}
