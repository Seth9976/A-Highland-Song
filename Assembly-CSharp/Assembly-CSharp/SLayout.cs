using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001D0 RID: 464
[NullableContext(2)]
[Nullable(0)]
[ExecuteInEditMode]
public class SLayout : UIBehaviour
{
	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0007714A File Offset: 0x0007534A
	[Nullable(0)]
	public RoundRect roundRect
	{
		[NullableContext(0)]
		get
		{
			return this.graphic as RoundRect;
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06000F83 RID: 3971 RVA: 0x00077157 File Offset: 0x00075357
	// (set) Token: 0x06000F84 RID: 3972 RVA: 0x0007716A File Offset: 0x0007536A
	public Color fillColor
	{
		get
		{
			this.InitFillColor();
			return this._fillColor.value;
		}
		set
		{
			this.InitFillColor();
			this._fillColor.value = value;
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0007717E File Offset: 0x0007537E
	public Color targetFillColor
	{
		get
		{
			this.InitFillColor();
			if (this._fillColor.animatedProperty == null)
			{
				return this._fillColor.value;
			}
			return this._fillColor.animatedProperty.end;
		}
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x000771AF File Offset: 0x000753AF
	private void InitFillColor()
	{
		if (this._fillColor == null)
		{
			this._fillColor = new SLayoutColorProperty
			{
				getter = delegate
				{
					if (!(this.roundRect != null))
					{
						return Color.white;
					}
					return this.roundRect.fillColor;
				},
				setter = delegate(Color c)
				{
					if (this.roundRect != null)
					{
						this.roundRect.fillColor = c;
					}
				}
			};
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06000F87 RID: 3975 RVA: 0x000771E8 File Offset: 0x000753E8
	// (set) Token: 0x06000F88 RID: 3976 RVA: 0x000771FB File Offset: 0x000753FB
	public Color outlineColor
	{
		get
		{
			this.InitOutlineColor();
			return this._outlineColor.value;
		}
		set
		{
			this.InitOutlineColor();
			this._outlineColor.value = value;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0007720F File Offset: 0x0007540F
	public Color targetOutlineColor
	{
		get
		{
			this.InitOutlineColor();
			if (this._outlineColor.animatedProperty == null)
			{
				return this._outlineColor.value;
			}
			return this._outlineColor.animatedProperty.end;
		}
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x00077240 File Offset: 0x00075440
	private void InitOutlineColor()
	{
		if (this._outlineColor == null)
		{
			this._outlineColor = new SLayoutColorProperty
			{
				getter = delegate
				{
					if (!(this.roundRect != null))
					{
						return Color.white;
					}
					return this.roundRect.outlineColor;
				},
				setter = delegate(Color c)
				{
					if (this.roundRect != null)
					{
						this.roundRect.outlineColor = c;
					}
				}
			};
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00077279 File Offset: 0x00075479
	[Nullable(0)]
	public TextMeshProUGUI textMeshPro
	{
		[NullableContext(0)]
		get
		{
			return this.graphic as TextMeshProUGUI;
		}
	}

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000F8C RID: 3980 RVA: 0x00077288 File Offset: 0x00075488
	// (remove) Token: 0x06000F8D RID: 3981 RVA: 0x000772C0 File Offset: 0x000754C0
	[Nullable(new byte[] { 2, 1 })]
	[field: Nullable(new byte[] { 2, 1 })]
	public event Action<SLayout> onRectChange;

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06000F8E RID: 3982 RVA: 0x000772F8 File Offset: 0x000754F8
	public bool isAnimating
	{
		get
		{
			SLayoutAnimator instance = SLayoutAnimator.instance;
			return !(instance == null) && instance.IsAnimating(this);
		}
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00077320 File Offset: 0x00075520
	protected override void OnDisable()
	{
		base.OnDisable();
		this._x = null;
		this._y = null;
		this._width = null;
		this._height = null;
		this._rotation = null;
		this._scale = null;
		this._groupAlpha = null;
		this._color = null;
		this._canvas = null;
		this._canvasGroup = null;
		this._graphic = null;
		this._searchedForTimeScalar = false;
		this._timeScalar = null;
		this.CancelAnimations();
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00077394 File Offset: 0x00075594
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (this.onRectChange != null)
		{
			this.onRectChange(this);
		}
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x000773B0 File Offset: 0x000755B0
	[NullableContext(1)]
	public SLayoutAnimation Animate(float duration, Action animAction)
	{
		return this.Animate(duration, 0f, animAction);
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x000773BF File Offset: 0x000755BF
	[NullableContext(1)]
	public SLayoutAnimation Animate(float duration, float delay, Action animAction)
	{
		return this.Animate(duration, delay, null, animAction);
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x000773CC File Offset: 0x000755CC
	[NullableContext(1)]
	public SLayoutAnimation After(float delay, Action nonAnimatedAction)
	{
		SLayoutAnimation slayoutAnimation = new SLayoutAnimation(0f, delay, null, null, nonAnimatedAction, this);
		SLayoutAnimator.instance.StartAnimation(slayoutAnimation);
		return slayoutAnimation;
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x000773F8 File Offset: 0x000755F8
	[NullableContext(1)]
	public SLayoutAnimation Animate(float duration, float delay, [Nullable(2)] AnimationCurve customCurve, Action animAction)
	{
		SLayoutAnimation slayoutAnimation = new SLayoutAnimation(duration, delay, customCurve, animAction, null, this);
		SLayoutAnimator.instance.StartAnimation(slayoutAnimation);
		return slayoutAnimation;
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00077420 File Offset: 0x00075620
	[NullableContext(1)]
	public SLayoutAnimation AnimateCustom(float duration, Action<float> customAnimAction)
	{
		SLayoutAnimation slayoutAnimation = new SLayoutAnimation(duration, 0f, null, delegate
		{
			SLayout.Animatable(customAnimAction);
		}, null, this);
		SLayoutAnimator.instance.StartAnimation(slayoutAnimation);
		return slayoutAnimation;
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00077464 File Offset: 0x00075664
	[NullableContext(1)]
	public SLayoutAnimation AnimateCustom(float duration, float delay, Action<float> customAnimAction)
	{
		SLayoutAnimation slayoutAnimation = new SLayoutAnimation(duration, delay, null, delegate
		{
			SLayout.Animatable(customAnimAction);
		}, null, this);
		SLayoutAnimator.instance.StartAnimation(slayoutAnimation);
		return slayoutAnimation;
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x000774A1 File Offset: 0x000756A1
	public void AddDelay(float extraDelay)
	{
		SLayoutAnimator.instance.AddDelay(extraDelay);
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x000774AE File Offset: 0x000756AE
	public void AddDuration(float extraDuration)
	{
		SLayoutAnimator.instance.AddDuration(extraDuration);
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x000774BB File Offset: 0x000756BB
	[NullableContext(1)]
	public static void Animatable(Action<float> customAnim)
	{
		SLayoutAnimator.instance.Animatable(customAnim);
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x000774C8 File Offset: 0x000756C8
	[NullableContext(1)]
	public static void Animatable(float initial, float target, Action<float> setter)
	{
		SLayoutAnimator.instance.Animatable(delegate(float t)
		{
			setter(Mathf.LerpUnclamped(initial, target, t));
		});
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x00077508 File Offset: 0x00075708
	[NullableContext(1)]
	public static void Animatable(Color initial, Color target, Action<Color> setter)
	{
		SLayoutAnimator.instance.Animatable(delegate(float t)
		{
			setter(Color.Lerp(initial, target, t));
		});
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00077546 File Offset: 0x00075746
	public void CancelAnimations()
	{
		if (SLayoutAnimator.hasInstance)
		{
			SLayoutAnimator.instance.CancelAnimations(this);
		}
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x0007755A File Offset: 0x0007575A
	public void CompleteAnimations()
	{
		SLayoutAnimator.instance.CompleteAnimations(this);
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x00077567 File Offset: 0x00075767
	[NullableContext(1)]
	public static void WithoutAnimating(Action action)
	{
		SLayoutAnimation.StartPreventAnimation();
		action();
		SLayoutAnimation.EndPreventAnimation();
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00077579 File Offset: 0x00075779
	[Nullable(1)]
	public Canvas canvas
	{
		[NullableContext(1)]
		get
		{
			if (this._canvas == null)
			{
				this._canvas = base.transform.GetComponentInParent<Canvas>();
			}
			return this._canvas;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x000775A0 File Offset: 0x000757A0
	public float canvasWidth
	{
		get
		{
			return ((RectTransform)this.canvas.transform).rect.width;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x000775CC File Offset: 0x000757CC
	public float canvasHeight
	{
		get
		{
			return ((RectTransform)this.canvas.transform).rect.height;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x000775F8 File Offset: 0x000757F8
	public Vector2 canvasSize
	{
		get
		{
			return ((RectTransform)this.canvas.transform).rect.size;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x00077622 File Offset: 0x00075822
	public CanvasGroup canvasGroup
	{
		get
		{
			if (this._canvasGroup == null)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
			}
			return this._canvasGroup;
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x00077644 File Offset: 0x00075844
	public Image image
	{
		get
		{
			return this.graphic as Image;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00077651 File Offset: 0x00075851
	public Text text
	{
		get
		{
			return this.graphic as Text;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0007765E File Offset: 0x0007585E
	public Graphic graphic
	{
		get
		{
			if (this._graphic == null)
			{
				this._graphic = base.GetComponent<Graphic>();
			}
			return this._graphic;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00077680 File Offset: 0x00075880
	public SLayout parent
	{
		get
		{
			return base.transform.parent.GetComponent<SLayout>();
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00077694 File Offset: 0x00075894
	public Rect parentRect
	{
		get
		{
			RectTransform parentRectTransform = this.parentRectTransform;
			if (parentRectTransform == null)
			{
				return Rect.zero;
			}
			Rect rect = parentRectTransform.rect;
			return new Rect(this.GetRectTransformX(parentRectTransform), this.GetRectTransformY(parentRectTransform), rect.width, rect.height);
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x000776DF File Offset: 0x000758DF
	public RectTransform parentRectTransform
	{
		get
		{
			return base.transform.parent as RectTransform;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06000FAA RID: 4010 RVA: 0x000776F1 File Offset: 0x000758F1
	// (set) Token: 0x06000FAB RID: 4011 RVA: 0x00077712 File Offset: 0x00075912
	public float timeScale
	{
		get
		{
			if (!(this.timeScalar == null))
			{
				return this.timeScalar.timeScaleMultiplier;
			}
			return 1f;
		}
		set
		{
			if (this.timeScalar != null)
			{
				this.timeScalar.timeScaleMultiplier = value;
			}
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00077730 File Offset: 0x00075930
	private SLayoutCanvasTimeScalar timeScalar
	{
		get
		{
			if (!this._searchedForTimeScalar)
			{
				foreach (Canvas canvas in base.transform.GetComponentsInParent<Canvas>())
				{
					this._timeScalar = canvas.GetComponent<SLayoutCanvasTimeScalar>();
					if (this._timeScalar != null)
					{
						break;
					}
				}
				this._searchedForTimeScalar = true;
			}
			return this._timeScalar;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0007778A File Offset: 0x0007598A
	// (set) Token: 0x06000FAE RID: 4014 RVA: 0x0007779D File Offset: 0x0007599D
	public float x
	{
		get
		{
			this.InitX();
			return this._x.value;
		}
		set
		{
			this.InitX();
			this._x.value = value;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06000FAF RID: 4015 RVA: 0x000777B1 File Offset: 0x000759B1
	public float targetX
	{
		get
		{
			this.InitX();
			if (this._x.animatedProperty == null)
			{
				return this._x.value;
			}
			return this._x.animatedProperty.end;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x000777E2 File Offset: 0x000759E2
	// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x000777F5 File Offset: 0x000759F5
	public float y
	{
		get
		{
			this.InitY();
			return this._y.value;
		}
		set
		{
			this.InitY();
			this._y.value = value;
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00077809 File Offset: 0x00075A09
	public float targetY
	{
		get
		{
			this.InitY();
			if (this._y.animatedProperty == null)
			{
				return this._y.value;
			}
			return this._y.animatedProperty.end;
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0007783A File Offset: 0x00075A3A
	// (set) Token: 0x06000FB4 RID: 4020 RVA: 0x0007784D File Offset: 0x00075A4D
	public float width
	{
		get
		{
			this.InitWidth();
			return this._width.value;
		}
		set
		{
			this.InitWidth();
			this._width.value = value;
		}
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00077861 File Offset: 0x00075A61
	public float targetWidth
	{
		get
		{
			this.InitWidth();
			if (this._width.animatedProperty == null)
			{
				return this._width.value;
			}
			return this._width.animatedProperty.end;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x00077892 File Offset: 0x00075A92
	// (set) Token: 0x06000FB7 RID: 4023 RVA: 0x000778A5 File Offset: 0x00075AA5
	public float height
	{
		get
		{
			this.InitHeight();
			return this._height.value;
		}
		set
		{
			this.InitHeight();
			this._height.value = value;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x000778B9 File Offset: 0x00075AB9
	public float targetHeight
	{
		get
		{
			this.InitHeight();
			if (this._height.animatedProperty == null)
			{
				return this._height.value;
			}
			return this._height.animatedProperty.end;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x000778EA File Offset: 0x00075AEA
	// (set) Token: 0x06000FBA RID: 4026 RVA: 0x000778FD File Offset: 0x00075AFD
	public Vector2 position
	{
		get
		{
			return new Vector2(this.x, this.y);
		}
		set
		{
			this.x = value.x;
			this.y = value.y;
		}
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00077917 File Offset: 0x00075B17
	public Vector2 targetPosition
	{
		get
		{
			return new Vector2(this.targetX, this.targetY);
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0007792A File Offset: 0x00075B2A
	// (set) Token: 0x06000FBD RID: 4029 RVA: 0x0007793D File Offset: 0x00075B3D
	public Vector2 size
	{
		get
		{
			return new Vector2(this.width, this.height);
		}
		set
		{
			this.width = value.x;
			this.height = value.y;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00077957 File Offset: 0x00075B57
	public Vector2 targetSize
	{
		get
		{
			return new Vector2(this.targetWidth, this.targetHeight);
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0007796A File Offset: 0x00075B6A
	// (set) Token: 0x06000FC0 RID: 4032 RVA: 0x0007797D File Offset: 0x00075B7D
	public float rotation
	{
		get
		{
			this.InitRotation();
			return this._rotation.value;
		}
		set
		{
			this.InitRotation();
			this._rotation.value = value;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x00077991 File Offset: 0x00075B91
	public float targetRotation
	{
		get
		{
			this.InitRotation();
			if (this._rotation.animatedProperty == null)
			{
				return this._rotation.value;
			}
			return this._rotation.animatedProperty.end;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x000779C2 File Offset: 0x00075BC2
	// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x000779D5 File Offset: 0x00075BD5
	public float scale
	{
		get
		{
			this.InitScale();
			return this._scale.value;
		}
		set
		{
			this.InitScale();
			this._scale.value = value;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000779E9 File Offset: 0x00075BE9
	public float targetScale
	{
		get
		{
			this.InitScale();
			if (this._scale.animatedProperty == null)
			{
				return this._scale.value;
			}
			return this._scale.animatedProperty.end;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x00077A1A File Offset: 0x00075C1A
	// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x00077A2D File Offset: 0x00075C2D
	public float groupAlpha
	{
		get
		{
			this.InitGroupAlpha();
			return this._groupAlpha.value;
		}
		set
		{
			this.InitGroupAlpha();
			this._groupAlpha.value = value;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x00077A41 File Offset: 0x00075C41
	public float targetGroupAlpha
	{
		get
		{
			this.InitGroupAlpha();
			if (this._groupAlpha.animatedProperty == null)
			{
				return this._groupAlpha.value;
			}
			return this._groupAlpha.animatedProperty.end;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x00077A72 File Offset: 0x00075C72
	// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x00077A85 File Offset: 0x00075C85
	public Color color
	{
		get
		{
			this.InitColor();
			return this._color.value;
		}
		set
		{
			this.InitColor();
			this._color.value = value;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00077A99 File Offset: 0x00075C99
	public Color targetColor
	{
		get
		{
			this.InitColor();
			if (this._color.animatedProperty == null)
			{
				return this._color.value;
			}
			return this._color.animatedProperty.end;
		}
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00077ACA File Offset: 0x00075CCA
	// (set) Token: 0x06000FCC RID: 4044 RVA: 0x00077AE4 File Offset: 0x00075CE4
	public float alpha
	{
		get
		{
			this.InitColor();
			return this._color.value.a;
		}
		set
		{
			this.InitColor();
			Color value2 = this._color.value;
			value2.a = value;
			this._color.value = value2;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00077B17 File Offset: 0x00075D17
	public float targetAlpha
	{
		get
		{
			return this.targetColor.a;
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00077B24 File Offset: 0x00075D24
	// (set) Token: 0x06000FCF RID: 4047 RVA: 0x00077B43 File Offset: 0x00075D43
	public Rect rect
	{
		get
		{
			return new Rect(this.x, this.y, this.width, this.height);
		}
		set
		{
			this.x = value.x;
			this.y = value.y;
			this.width = value.width;
			this.height = value.height;
		}
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x00077B79 File Offset: 0x00075D79
	public Rect targetRect
	{
		get
		{
			return new Rect(this.targetX, this.targetY, this.targetWidth, this.targetHeight);
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x00077B98 File Offset: 0x00075D98
	// (set) Token: 0x06000FD2 RID: 4050 RVA: 0x00077BB8 File Offset: 0x00075DB8
	public Rect localRect
	{
		get
		{
			return new Rect(0f, 0f, this.width, this.height);
		}
		set
		{
			this.x += value.x;
			this.y += value.y;
			this.width = value.width;
			this.height = value.height;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00077C07 File Offset: 0x00075E07
	public Rect targetLocalRect
	{
		get
		{
			return new Rect(0f, 0f, this.targetWidth, this.targetHeight);
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x00077C24 File Offset: 0x00075E24
	// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x00077C37 File Offset: 0x00075E37
	public Vector2 center
	{
		get
		{
			return new Vector2(this.centerX, this.centerY);
		}
		set
		{
			this.centerX = value.x;
			this.centerY = value.y;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00077C51 File Offset: 0x00075E51
	public Vector2 targetCenter
	{
		get
		{
			return new Vector2(this.targetCenterX, this.targetCenterY);
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00077C64 File Offset: 0x00075E64
	// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x00077C79 File Offset: 0x00075E79
	public float centerX
	{
		get
		{
			return this.x + 0.5f * this.width;
		}
		set
		{
			this.x = value - 0.5f * this.width;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00077C8F File Offset: 0x00075E8F
	public float targetCenterX
	{
		get
		{
			return this.targetX + 0.5f * this.targetWidth;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06000FDA RID: 4058 RVA: 0x00077CA4 File Offset: 0x00075EA4
	// (set) Token: 0x06000FDB RID: 4059 RVA: 0x00077CB9 File Offset: 0x00075EB9
	public float centerY
	{
		get
		{
			return this.y + 0.5f * this.height;
		}
		set
		{
			this.y = value - 0.5f * this.height;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00077CCF File Offset: 0x00075ECF
	public float targetCenterY
	{
		get
		{
			return this.targetY + 0.5f * this.targetHeight;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06000FDD RID: 4061 RVA: 0x00077CE4 File Offset: 0x00075EE4
	public Vector2 middle
	{
		get
		{
			return new Vector2(this.middleX, this.middleY);
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06000FDE RID: 4062 RVA: 0x00077CF7 File Offset: 0x00075EF7
	public float middleX
	{
		get
		{
			return 0.5f * this.width;
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06000FDF RID: 4063 RVA: 0x00077D05 File Offset: 0x00075F05
	public float middleY
	{
		get
		{
			return 0.5f * this.height;
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00077D14 File Offset: 0x00075F14
	// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x00077D44 File Offset: 0x00075F44
	public float originX
	{
		get
		{
			float num = this.rectTransform.pivot.x * this.width;
			return this.x + num;
		}
		set
		{
			float num = this.rectTransform.pivot.x * this.width;
			this.x = value - num;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x00077D74 File Offset: 0x00075F74
	// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x00077DA4 File Offset: 0x00075FA4
	public float originY
	{
		get
		{
			float num = this.rectTransform.pivot.y * this.height;
			return this.y + num;
		}
		set
		{
			float num = this.rectTransform.pivot.y * this.height;
			this.y = value - num;
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00077DD2 File Offset: 0x00075FD2
	// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x00077DE5 File Offset: 0x00075FE5
	public Vector2 pivot
	{
		get
		{
			return new Vector2(this.pivotX, this.pivotY);
		}
		set
		{
			this.pivotX = value.x;
			this.pivotY = value.y;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00077DFF File Offset: 0x00075FFF
	// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x00077E18 File Offset: 0x00076018
	public float pivotX
	{
		get
		{
			return this.rectTransform.pivot.x * this.width;
		}
		set
		{
			Vector2 pivot = this.rectTransform.pivot;
			pivot.x = value / this.width;
			this.rectTransform.pivot = pivot;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x00077E4C File Offset: 0x0007604C
	// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x00077E68 File Offset: 0x00076068
	public float pivotY
	{
		get
		{
			return this.rectTransform.pivot.y * this.height;
		}
		set
		{
			Vector2 pivot = this.rectTransform.pivot;
			pivot.y = value / this.height;
			this.rectTransform.pivot = pivot;
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00077E9C File Offset: 0x0007609C
	// (set) Token: 0x06000FEB RID: 4075 RVA: 0x00077EAF File Offset: 0x000760AF
	public Vector2 origin
	{
		get
		{
			return new Vector2(this.originX, this.originY);
		}
		set
		{
			this.originX = value.x;
			this.originY = value.y;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00077EC9 File Offset: 0x000760C9
	// (set) Token: 0x06000FED RID: 4077 RVA: 0x00077ED8 File Offset: 0x000760D8
	public float rightX
	{
		get
		{
			return this.x + this.width;
		}
		set
		{
			this.x = value - this.width;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00077EE8 File Offset: 0x000760E8
	public float targetRightX
	{
		get
		{
			return this.targetX + this.targetWidth;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00077EF7 File Offset: 0x000760F7
	// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x00077F15 File Offset: 0x00076115
	public float bottomY
	{
		get
		{
			if (this.originTopLeft)
			{
				return this.y + this.height;
			}
			return this.y;
		}
		set
		{
			if (this.originTopLeft)
			{
				this.y = value - this.height;
				return;
			}
			this.y = value;
		}
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00077F35 File Offset: 0x00076135
	public float targetBottomY
	{
		get
		{
			if (this.originTopLeft)
			{
				return this.targetY + this.targetHeight;
			}
			return this.targetY;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00077F53 File Offset: 0x00076153
	// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x00077F71 File Offset: 0x00076171
	public float topY
	{
		get
		{
			if (this.originTopLeft)
			{
				return this.y;
			}
			return this.y + this.height;
		}
		set
		{
			if (this.originTopLeft)
			{
				this.y = value;
				return;
			}
			this.y = value - this.height;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00077F91 File Offset: 0x00076191
	public float targetTopY
	{
		get
		{
			if (this.originTopLeft)
			{
				return this.targetY;
			}
			return this.targetY + this.targetHeight;
		}
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x00077FB0 File Offset: 0x000761B0
	[NullableContext(1)]
	private Vector2 GetPivotPos(RectTransform rt)
	{
		Vector2 size = rt.rect.size;
		Vector2 pivot = rt.pivot;
		return new Vector2(size.x * pivot.x, size.y * pivot.y);
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x00077FF4 File Offset: 0x000761F4
	public Vector2 CanvasToSLayoutSpace(Vector2 canvasSpacePos)
	{
		Vector2 zero = Vector2.zero;
		RectTransform rectTransform = this.rectTransform.parent as RectTransform;
		if (rectTransform == null)
		{
			return zero;
		}
		float num = rectTransform.pivot.x * rectTransform.rect.width;
		zero.x = num;
		if (this.originTopLeft)
		{
			canvasSpacePos.y = -canvasSpacePos.y;
			float num2 = (1f - rectTransform.pivot.y) * rectTransform.rect.height;
			zero.y = num2;
		}
		else
		{
			float num3 = rectTransform.pivot.y * rectTransform.rect.height;
			zero.y = num3;
		}
		return canvasSpacePos + zero;
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x000780B8 File Offset: 0x000762B8
	[NullableContext(1)]
	public Vector2 ConvertPositionToTarget(Vector2 localLayoutPos, SLayout targetLayout)
	{
		if (this.originTopLeft)
		{
			localLayoutPos.y = this.height - localLayoutPos.y;
		}
		Vector2 vector = localLayoutPos - this.GetPivotPos(this.rectTransform);
		Vector3 vector2 = this.rectTransform.TransformPoint(vector);
		RectTransform rectTransform = (targetLayout ? targetLayout.rectTransform : null);
		if (rectTransform == null)
		{
			rectTransform = this.canvas.transform as RectTransform;
		}
		Vector2 vector3 = rectTransform.InverseTransformPoint(vector2) + this.GetPivotPos(rectTransform);
		if (targetLayout != null && targetLayout.originTopLeft)
		{
			vector3.y = targetLayout.height - vector3.y;
		}
		return vector3;
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x00078174 File Offset: 0x00076374
	[NullableContext(1)]
	public Rect ConvertRectToTarget(Rect localRect, SLayout targetLayout)
	{
		Vector2 vector = this.ConvertPositionToTarget(localRect.min, targetLayout);
		Vector2 vector2 = this.ConvertPositionToTarget(localRect.max, targetLayout);
		return new Rect(vector.x, Mathf.Min(vector.y, vector2.y), vector2.x - vector.x, Mathf.Abs(vector.y - vector2.y));
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x000781DA File Offset: 0x000763DA
	[Nullable(1)]
	public RectTransform rectTransform
	{
		[NullableContext(1)]
		get
		{
			return (RectTransform)base.transform;
		}
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x000781E8 File Offset: 0x000763E8
	[NullableContext(1)]
	private float GetRectTransformX(RectTransform rt)
	{
		float num = rt.pivot.x * rt.rect.width;
		RectTransform rectTransform = rt.parent as RectTransform;
		if (rectTransform == null)
		{
			return 0f;
		}
		return rectTransform.pivot.x * rectTransform.rect.width + base.transform.localPosition.x - num;
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x00078258 File Offset: 0x00076458
	[NullableContext(1)]
	private float GetRectTransformY(RectTransform rt)
	{
		RectTransform rectTransform = rt.parent as RectTransform;
		if (rectTransform == null)
		{
			return 0f;
		}
		if (this.originTopLeft)
		{
			float num = (1f - rt.pivot.y) * rt.rect.height;
			return (1f - rectTransform.pivot.y) * rectTransform.rect.height - base.transform.localPosition.y - num;
		}
		float num2 = rt.pivot.y * rt.rect.height;
		return rectTransform.pivot.y * rectTransform.rect.height + base.transform.localPosition.y - num2;
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x00078328 File Offset: 0x00076528
	private void SetRectTransformX(float x)
	{
		RectTransform parentRectTransform = this.parentRectTransform;
		if (parentRectTransform == null)
		{
			return;
		}
		RectTransform rectTransform = this.rectTransform;
		float num = parentRectTransform.pivot.x * parentRectTransform.rect.width;
		float num2 = rectTransform.pivot.x * rectTransform.rect.width;
		float num3 = -num + x + num2;
		Vector3 localPosition = rectTransform.localPosition;
		if (localPosition.x == num3)
		{
			return;
		}
		localPosition.x = num3;
		rectTransform.localPosition = localPosition;
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x000783B0 File Offset: 0x000765B0
	private void SetRectTransformY(float y)
	{
		RectTransform parentRectTransform = this.parentRectTransform;
		if (parentRectTransform == null)
		{
			return;
		}
		RectTransform rectTransform = this.rectTransform;
		float num3;
		if (this.originTopLeft)
		{
			float num = (1f - parentRectTransform.pivot.y) * parentRectTransform.rect.height;
			float num2 = (1f - rectTransform.pivot.y) * rectTransform.rect.height;
			num3 = num - y - num2;
		}
		else
		{
			float num4 = parentRectTransform.pivot.y * parentRectTransform.rect.height;
			float num5 = rectTransform.pivot.y * rectTransform.rect.height;
			num3 = -num4 + y + num5;
		}
		Vector3 localPosition = rectTransform.localPosition;
		if (localPosition.y == num3)
		{
			return;
		}
		localPosition.y = num3;
		rectTransform.localPosition = localPosition;
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x00078488 File Offset: 0x00076688
	private void SetRectTransformWidth(float width)
	{
		RectTransform rectTransform = this.rectTransform;
		float num = (rectTransform.anchorMax.x - rectTransform.anchorMin.x) * this.parentRect.width;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		sizeDelta.x = width - num;
		if (rectTransform.sizeDelta.x != sizeDelta.x)
		{
			float rectTransformX = this.GetRectTransformX(rectTransform);
			rectTransform.sizeDelta = sizeDelta;
			this.SetRectTransformX(rectTransformX);
		}
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x00078500 File Offset: 0x00076700
	private void SetRectTransformHeight(float height)
	{
		RectTransform rectTransform = this.rectTransform;
		float num = (rectTransform.anchorMax.y - rectTransform.anchorMin.y) * this.parentRect.height;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		sizeDelta.y = height - num;
		if (rectTransform.sizeDelta.y != sizeDelta.y)
		{
			float rectTransformY = this.GetRectTransformY(rectTransform);
			rectTransform.sizeDelta = sizeDelta;
			this.SetRectTransformY(rectTransformY);
		}
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x00078577 File Offset: 0x00076777
	private void InitX()
	{
		if (this._x == null)
		{
			this._x = new SLayoutFloatProperty
			{
				getter = () => this.GetRectTransformX(this.rectTransform),
				setter = new Action<float>(this.SetRectTransformX)
			};
		}
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x000785B0 File Offset: 0x000767B0
	private void InitY()
	{
		if (this._y == null)
		{
			this._y = new SLayoutFloatProperty
			{
				getter = () => this.GetRectTransformY(this.rectTransform),
				setter = new Action<float>(this.SetRectTransformY)
			};
		}
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x000785E9 File Offset: 0x000767E9
	private void InitWidth()
	{
		if (this._width == null)
		{
			this._width = new SLayoutFloatProperty
			{
				getter = () => this.rectTransform.rect.width,
				setter = new Action<float>(this.SetRectTransformWidth)
			};
		}
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x00078622 File Offset: 0x00076822
	private void InitHeight()
	{
		if (this._height == null)
		{
			this._height = new SLayoutFloatProperty
			{
				getter = () => this.rectTransform.rect.height,
				setter = new Action<float>(this.SetRectTransformHeight)
			};
		}
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x0007865B File Offset: 0x0007685B
	private void InitRotation()
	{
		if (this._rotation == null)
		{
			this._rotation = new SLayoutAngleProperty
			{
				getter = () => base.transform.rotation.eulerAngles.z,
				setter = delegate(float r)
				{
					base.transform.rotation = Quaternion.Euler(0f, 0f, r);
				}
			};
		}
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x00078694 File Offset: 0x00076894
	private void InitScale()
	{
		if (this._scale == null)
		{
			this._scale = new SLayoutFloatProperty
			{
				getter = () => base.transform.localScale.x,
				setter = delegate(float s)
				{
					base.transform.localScale = new Vector3(s, s, s);
				}
			};
		}
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x000786CD File Offset: 0x000768CD
	private void InitGroupAlpha()
	{
		if (this._groupAlpha == null)
		{
			this._groupAlpha = new SLayoutFloatProperty
			{
				getter = delegate
				{
					if (!(this.canvasGroup != null))
					{
						return 1f;
					}
					return this.canvasGroup.alpha;
				},
				setter = delegate(float a)
				{
					if (this.canvasGroup != null)
					{
						this.canvasGroup.alpha = a;
					}
				}
			};
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x00078706 File Offset: 0x00076906
	private void InitColor()
	{
		if (this._color == null)
		{
			this._color = new SLayoutColorProperty
			{
				getter = delegate
				{
					if (!(this.graphic != null))
					{
						return Color.white;
					}
					return this.graphic.color;
				},
				setter = delegate(Color c)
				{
					if (this.graphic != null)
					{
						this.graphic.color = c;
					}
				}
			};
		}
	}

	// Token: 0x040011EE RID: 4590
	[Nullable(0)]
	private SLayoutColorProperty _fillColor;

	// Token: 0x040011EF RID: 4591
	[Nullable(0)]
	private SLayoutColorProperty _outlineColor;

	// Token: 0x040011F0 RID: 4592
	public bool originTopLeft;

	// Token: 0x040011F2 RID: 4594
	private Canvas _canvas;

	// Token: 0x040011F3 RID: 4595
	private CanvasGroup _canvasGroup;

	// Token: 0x040011F4 RID: 4596
	private Graphic _graphic;

	// Token: 0x040011F5 RID: 4597
	private bool _searchedForTimeScalar;

	// Token: 0x040011F6 RID: 4598
	private SLayoutCanvasTimeScalar _timeScalar;

	// Token: 0x040011F7 RID: 4599
	[Nullable(1)]
	public static AnimationCurve popCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.8f, 1.1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x040011F8 RID: 4600
	[Nullable(1)]
	public static AnimationCurve reversePopCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.2f, -0.1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x040011F9 RID: 4601
	[Nullable(1)]
	public static AnimationCurve thereAndBack = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x040011FA RID: 4602
	private SLayoutFloatProperty _x;

	// Token: 0x040011FB RID: 4603
	private SLayoutFloatProperty _y;

	// Token: 0x040011FC RID: 4604
	private SLayoutFloatProperty _width;

	// Token: 0x040011FD RID: 4605
	private SLayoutFloatProperty _height;

	// Token: 0x040011FE RID: 4606
	private SLayoutAngleProperty _rotation;

	// Token: 0x040011FF RID: 4607
	private SLayoutFloatProperty _scale;

	// Token: 0x04001200 RID: 4608
	private SLayoutFloatProperty _groupAlpha;

	// Token: 0x04001201 RID: 4609
	private SLayoutColorProperty _color;
}
