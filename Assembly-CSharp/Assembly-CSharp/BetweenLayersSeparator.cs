using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200016B RID: 363
[NullableContext(1)]
[Nullable(new byte[] { 0, 1 })]
public class BetweenLayersSeparator : MonoSingleton<BetweenLayersSeparator>
{
	// Token: 0x06000C18 RID: 3096 RVA: 0x00060962 File Offset: 0x0005EB62
	public void RefreshImmediate()
	{
		this.Refresh(true);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x0006096B File Offset: 0x0005EB6B
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		this.Refresh(false);
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0006097C File Offset: 0x0005EB7C
	private void Refresh(bool immediate)
	{
		Runner instance = Runner.instance;
		float num = ((Runner.instance.playerControlDisabled > PlayerControlDisableReason.None) ? 0.2f : 1f);
		float num2 = Mathf.Lerp(1f, 0.4f, this.darknessFadeOutScalar);
		float num3 = 1f;
		foreach (FadeSeparator fadeSeparator in Level.current.fadeSeparators.Nearby(instance.position, new Range(-100000f, 100000f), 0f, null))
		{
			if (Range.Centered(fadeSeparator.transform.position.z, fadeSeparator.depth).Contains((float)instance.physicalDepthLayerIdx) && (instance.position - fadeSeparator.transform.position).magnitude < fadeSeparator.radius && fadeSeparator.opacityScalar < num3)
			{
				num3 = fadeSeparator.opacityScalar;
			}
		}
		float num4 = Mathf.Min(num, num2);
		num4 = Mathf.Min(num4, num3);
		float num5 = ((num4 < 1f) ? 5f : 2f);
		this._fadedOutScalar = Mathf.MoveTowards(this._fadedOutScalar, num4, Time.deltaTime / num5);
		if (immediate)
		{
			this._fadedOutScalar = num4;
		}
		Color separatorColor = this.settings.separatorColor;
		separatorColor.a *= Mathf.InverseLerp(this.settings.maxCameraDistance, this.settings.minCameraDistance, GameCamera.instance.cameraProperties.distance);
		separatorColor.a *= this._fadedOutScalar;
		Vector3 vector = new Vector3(instance.position.x, instance.position.y, (float)instance.physicalDepthLayerIdx + this.settings.separatorZOffset);
		int num6 = Mathf.RoundToInt((float)instance.physicalDepthLayerIdx);
		if (num6 != this._activeLayerIdx || this._activeSeparator == null)
		{
			if (this._activeSeparator != null)
			{
				this._oldSeparators.Add(this._activeSeparator);
				this._activeSeparator = null;
			}
			this._activeSeparator = this._layerSeparatorProto.Instantiate<SpriteRenderer>(null);
			this._activeSeparator.color = separatorColor.WithAlpha(0f);
			this._activeSeparator.transform.position = vector;
			this._activeLayerIdx = num6;
		}
		if (this._activeSeparator != null)
		{
			this._activeSeparator.color = separatorColor.WithAlpha(this._activeSeparator.color.a);
			this.MoveAlphaTowards(this._activeSeparator, separatorColor.a, immediate);
			if (Vector2.Distance(vector, this._activeSeparator.transform.position) > 10f)
			{
				this._oldSeparators.Add(this._activeSeparator);
				this._activeSeparator = null;
			}
			else
			{
				Vector2 vector2 = Vector2.Lerp(this._activeSeparator.transform.position, vector, TimeX.Lerping(this.settings.lerpSpeed2d));
				if (immediate)
				{
					vector2 = vector;
				}
				this._activeSeparator.transform.position = new Vector3(vector2.x, vector2.y, vector.z);
			}
		}
		this._oldSeparators.UpdateAndRemoveIf(delegate(SpriteRenderer old)
		{
			this.MoveAlphaTowards(old, 0f, immediate);
			if (old.color.a == 0f)
			{
				old.GetComponent<Prototype>().ReturnToPool();
				return true;
			}
			return false;
		});
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00060D30 File Offset: 0x0005EF30
	private void MoveAlphaTowards(SpriteRenderer spriteRenderer, float targetAlpha, bool immediate)
	{
		Color color = spriteRenderer.color;
		if (immediate)
		{
			color.a = targetAlpha;
		}
		else
		{
			color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime / this.settings.separatorCrossFadeDuration);
		}
		spriteRenderer.color = color;
	}

	// Token: 0x04000E63 RID: 3683
	[Disable]
	public float darknessFadeOutScalar = 1f;

	// Token: 0x04000E64 RID: 3684
	public BetweenLayersSeparatorSettings settings = Presume<BetweenLayersSeparatorSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\World\\BetweenLayersSeparator\\BetweenLayersSeparator.cs", 17);

	// Token: 0x04000E65 RID: 3685
	private int _activeLayerIdx = int.MinValue;

	// Token: 0x04000E66 RID: 3686
	[Nullable(2)]
	private SpriteRenderer _activeSeparator;

	// Token: 0x04000E67 RID: 3687
	private List<SpriteRenderer> _oldSeparators = new List<SpriteRenderer>();

	// Token: 0x04000E68 RID: 3688
	private float _fadedOutScalar = 1f;

	// Token: 0x04000E69 RID: 3689
	[SerializeField]
	private Prototype _layerSeparatorProto = Presume<Prototype>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\World\\BetweenLayersSeparator\\BetweenLayersSeparator.cs", 136);
}
