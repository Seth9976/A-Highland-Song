using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class GameUI : MonoSingleton<GameUI>
{
	// Token: 0x17000249 RID: 585
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x0004C49B File Offset: 0x0004A69B
	public HealthUI healthUI
	{
		get
		{
			return this._healthUI;
		}
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0004C4A3 File Offset: 0x0004A6A3
	public QuickButtonPrompt CreateQuickButtonPrompt()
	{
		return this._slipPromptPrototype.Instantiate<QuickButtonPrompt>(null);
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x0004C4B1 File Offset: 0x0004A6B1
	public Insets narrativeContentInsets
	{
		get
		{
			return this._narrativeContentInsets;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x0004C4B9 File Offset: 0x0004A6B9
	private RectTransform canvasRT
	{
		get
		{
			if (this._canvasRT == null)
			{
				this._canvasRT = base.GetComponent<Canvas>().transform as RectTransform;
			}
			return this._canvasRT;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x0004C4E5 File Offset: 0x0004A6E5
	public Vector2 canvasSize
	{
		get
		{
			return this.canvasRT.sizeDelta;
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0004C4F4 File Offset: 0x0004A6F4
	public Rect ContstrainWithinSafeZones(Rect layoutRect)
	{
		Rect rect = this.narrativeContentInsets.ApplyToRect(new Rect(Vector2.zero, this.canvasSize), false);
		if (layoutRect.xMin < rect.xMin)
		{
			layoutRect.x += rect.xMin - layoutRect.xMin;
		}
		else if (layoutRect.xMax > rect.xMax)
		{
			layoutRect.x -= layoutRect.xMax - rect.xMax;
		}
		if (layoutRect.yMin < rect.yMin)
		{
			layoutRect.y += rect.yMin - layoutRect.yMin;
		}
		else if (layoutRect.yMax > rect.yMax)
		{
			layoutRect.y -= layoutRect.yMax - rect.yMax;
		}
		foreach (GameUI.UnsafeRect unsafeRect in this.unsafeRects)
		{
			if (RectX.Intersects(unsafeRect.rect, layoutRect))
			{
				if (unsafeRect.pushDir.x >= 0f)
				{
					goto IL_0153;
				}
				float xMax = layoutRect.xMax;
				Rect rect2 = unsafeRect.rect;
				if (xMax <= rect2.xMin)
				{
					goto IL_0153;
				}
				float x = layoutRect.x;
				float xMax2 = layoutRect.xMax;
				rect2 = unsafeRect.rect;
				layoutRect.x = x - (xMax2 - rect2.xMin);
				IL_01A2:
				if (unsafeRect.pushDir.y > 0f)
				{
					float yMin = layoutRect.yMin;
					rect2 = unsafeRect.rect;
					if (yMin < rect2.yMax)
					{
						float y = layoutRect.y;
						rect2 = unsafeRect.rect;
						layoutRect.y = y + (rect2.yMax - layoutRect.yMin);
						continue;
					}
				}
				if (unsafeRect.pushDir.y >= 0f)
				{
					continue;
				}
				float yMax = layoutRect.yMax;
				rect2 = unsafeRect.rect;
				if (yMax > rect2.yMin)
				{
					float y2 = layoutRect.y;
					float yMax2 = layoutRect.yMax;
					rect2 = unsafeRect.rect;
					layoutRect.y = y2 - (yMax2 - rect2.yMin);
					continue;
				}
				continue;
				IL_0153:
				if (unsafeRect.pushDir.x <= 0f)
				{
					goto IL_01A2;
				}
				float xMin = layoutRect.xMin;
				rect2 = unsafeRect.rect;
				if (xMin < rect2.xMax)
				{
					float x2 = layoutRect.x;
					rect2 = unsafeRect.rect;
					layoutRect.x = x2 + (rect2.xMax - layoutRect.xMin);
					goto IL_01A2;
				}
				goto IL_01A2;
			}
		}
		return layoutRect;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0004C77C File Offset: 0x0004A97C
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		Insets insets = default(Insets);
		this.unsafeRects.Clear();
		if (!PhotoMode.visible)
		{
			SLayout top = MonoSingleton<BlackBars>.instance.top;
			SLayout bottom = MonoSingleton<BlackBars>.instance.bottom;
			insets.top = top.parentRect.height - top.bottomY + this.settings.minimumContentMargin;
			insets.bottom = bottom.topY + this.settings.minimumContentMargin;
			if (this._healthUI.layout.groupAlpha > 0.1f)
			{
				insets.left = this.settings.healthUIMargin;
			}
			if (MonoSingleton<Tutorial>.instance.visible)
			{
				this.unsafeRects.Add(new GameUI.UnsafeRect
				{
					rect = MonoSingleton<Tutorial>.instance.mainPanelLayout.rect.Expand(50f),
					pushDir = Vector2.up
				});
			}
			Vector2 size = this.canvasRT.rect.size;
			Vector2 vector = new Vector2(275f, 100f);
			this.unsafeRects.Add(new GameUI.UnsafeRect
			{
				rect = new Rect(size.x - vector.x, size.y - vector.y, vector.x, vector.y),
				pushDir = Vector2.down
			});
			Vector2 vector2 = new Vector2(315f, 175f);
			this.unsafeRects.Add(new GameUI.UnsafeRect
			{
				rect = new Rect(size.x - vector2.x, 0f, vector2.x, vector2.y),
				pushDir = Vector2.up
			});
		}
		Runner instance = Runner.instance;
		float num = ((instance.direction > 0f) ? 1f : 0.3f);
		float num2 = ((instance.direction < 0f) ? 1f : 0.3f);
		Vector3 mouthPosition = instance.animator.mouthPosition;
		Vector2 vector3 = this.WorldToCanvas(mouthPosition - num * Vector3.right - 1.5f * Vector3.up, default(Vector2));
		Vector2 vector4 = this.WorldToCanvas(mouthPosition + num2 * Vector3.right + 0.7f * Vector3.up, default(Vector2));
		Rect rect = new Rect(Mathf.Min(vector3.x, vector4.x), Mathf.Min(vector3.y, vector4.y), Mathf.Abs(vector4.x - vector3.x), Mathf.Abs(vector4.y - vector3.y));
		Vector2 center = rect.center;
		Vector2 vector5 = Vector2.down;
		if (center.x > 0.7f * this.canvasSize.x && center.y < 0.5f * this.canvasSize.y)
		{
			vector5 = Vector2.up;
		}
		else if (rect.yMin > 250f)
		{
			vector5 = Vector2.down;
		}
		else if (rect.yMax < this.canvasSize.y - 300f)
		{
			vector5 = Vector2.up;
		}
		else if (rect.center.x > 0.5f * this.canvasSize.x)
		{
			vector5 = -Vector2.right;
		}
		else
		{
			vector5 = Vector2.right;
		}
		this.unsafeRects.Add(new GameUI.UnsafeRect
		{
			rect = rect,
			pushDir = vector5
		});
		insets.right += this.settings.mapUIInsetMargin * MonoSingleton<MapsViewController>.instance.maximisedNorm;
		this._narrativeContentInsets.left = GameUI.<Update>g__UpdateInset|16_0(this._narrativeContentInsets.left, insets.left);
		this._narrativeContentInsets.right = GameUI.<Update>g__UpdateInset|16_0(this._narrativeContentInsets.right, insets.right);
		this._narrativeContentInsets.top = GameUI.<Update>g__UpdateInset|16_0(this._narrativeContentInsets.top, insets.top);
		this._narrativeContentInsets.bottom = GameUI.<Update>g__UpdateInset|16_0(this._narrativeContentInsets.bottom, insets.bottom);
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0004CBF0 File Offset: 0x0004ADF0
	private Vector2 PushScreenPosOffscreen(Vector2 screenPosNorm, float margin)
	{
		if (screenPosNorm.x >= -margin && screenPosNorm.x <= 1f + margin && screenPosNorm.y >= -margin && screenPosNorm.y <= 1f + margin)
		{
			Vector2 vector = new Vector2(Mathf.Abs(screenPosNorm.x - 0.5f), Mathf.Abs(screenPosNorm.y - 0.5f));
			if (vector.x > vector.y)
			{
				screenPosNorm.x = ((screenPosNorm.x < 0.5f) ? (-margin) : (1f + margin));
			}
			else
			{
				screenPosNorm.y = ((screenPosNorm.y < 0.5f) ? (-margin) : (1f + margin));
			}
		}
		return screenPosNorm;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0004CCB0 File Offset: 0x0004AEB0
	public Vector2 WorldToCanvas(Vector3 worldPos, Vector2 canvasSizeOverride = default(Vector2))
	{
		if (this._projectionCam == null)
		{
			this._projectionCam = GameCamera.instance.camera;
		}
		Vector3 vector = this._projectionCam.WorldToScreenPoint(worldPos);
		Vector2 vector2 = new Vector2(vector.x / (float)this._projectionCam.pixelWidth, vector.y / (float)this._projectionCam.pixelHeight);
		if (vector.z <= 0f)
		{
			vector2.x = 1f - vector2.x;
			vector2.y = 1f - vector2.y;
			vector2 = this.PushScreenPosOffscreen(vector2, 1f);
		}
		Vector2 vector3;
		if (canvasSizeOverride.x == 0f)
		{
			vector3 = this.canvasSize;
		}
		else
		{
			vector3 = canvasSizeOverride;
		}
		return Vector2.Scale(vector2, vector3);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0004CD88 File Offset: 0x0004AF88
	[CompilerGenerated]
	internal static float <Update>g__UpdateInset|16_0(float current, float target)
	{
		float num = (float)((target >= current) ? 800 : 300);
		return Mathf.MoveTowards(current, target, num * Time.deltaTime);
	}

	// Token: 0x04000AEB RID: 2795
	public GameUISettings settings;

	// Token: 0x04000AEC RID: 2796
	public ClimbPrompt climbPrompt;

	// Token: 0x04000AED RID: 2797
	public Prototype rhythmActionMarkerPromptPrototype;

	// Token: 0x04000AEE RID: 2798
	public List<GameUI.UnsafeRect> unsafeRects = new List<GameUI.UnsafeRect>();

	// Token: 0x04000AEF RID: 2799
	private RectTransform _canvasRT;

	// Token: 0x04000AF0 RID: 2800
	private Camera _projectionCam;

	// Token: 0x04000AF1 RID: 2801
	[SerializeField]
	private Prototype _slipPromptPrototype;

	// Token: 0x04000AF2 RID: 2802
	[SerializeField]
	private HealthUI _healthUI;

	// Token: 0x04000AF3 RID: 2803
	private Insets _narrativeContentInsets;

	// Token: 0x02000339 RID: 825
	public struct UnsafeRect
	{
		// Token: 0x04001833 RID: 6195
		public Rect rect;

		// Token: 0x04001834 RID: 6196
		public Vector2 pushDir;
	}
}
