using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class HurtFlash : MonoSingleton<HurtFlash>
{
	// Token: 0x17000252 RID: 594
	// (get) Token: 0x0600093E RID: 2366 RVA: 0x0004DAA8 File Offset: 0x0004BCA8
	private SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0004DACA File Offset: 0x0004BCCA
	public void Flash(HurtFlash.Intensity intensity)
	{
		if (intensity == HurtFlash.Intensity.Strong)
		{
			this._opacity = 1f;
			return;
		}
		if (intensity == HurtFlash.Intensity.Weak)
		{
			this._opacity = Mathf.Clamp01(this._opacity + this.weakHurtOpacity);
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0004DAF7 File Offset: 0x0004BCF7
	private void OnEnable()
	{
		this._opacity = 0f;
		this.RefreshVisual();
		Health.onHealthChanged = (Action<Health, float>)Delegate.Combine(Health.onHealthChanged, new Action<Health, float>(this.OnHealthChanged));
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0004DB2A File Offset: 0x0004BD2A
	private void OnDisable()
	{
		Health.onHealthChanged = (Action<Health, float>)Delegate.Remove(Health.onHealthChanged, new Action<Health, float>(this.OnHealthChanged));
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0004DB4C File Offset: 0x0004BD4C
	private void Update()
	{
		this._opacity *= TimeX.Damping(this.opacityDamping);
		if (this._opacity < 0.05f)
		{
			this._opacity = 0f;
		}
		this.RefreshVisual();
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0004DB84 File Offset: 0x0004BD84
	private void RefreshVisual()
	{
		this.layout.alpha = this._opacity * this.maxAlpha;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0004DB9E File Offset: 0x0004BD9E
	private void OnHealthChanged(Health health, float deltaHealth)
	{
		if (deltaHealth >= 0f)
		{
			return;
		}
		if (deltaHealth < -2f)
		{
			this.Flash(HurtFlash.Intensity.Strong);
			return;
		}
		if (deltaHealth < -1f)
		{
			this.Flash(HurtFlash.Intensity.Weak);
		}
	}

	// Token: 0x04000B22 RID: 2850
	public float opacityDamping = 0.95f;

	// Token: 0x04000B23 RID: 2851
	public float maxAlpha = 0.5f;

	// Token: 0x04000B24 RID: 2852
	public float weakHurtOpacity = 0.5f;

	// Token: 0x04000B25 RID: 2853
	private SLayout _layout;

	// Token: 0x04000B26 RID: 2854
	private float _opacity;

	// Token: 0x0200033E RID: 830
	public enum Intensity
	{
		// Token: 0x0400183C RID: 6204
		Weak,
		// Token: 0x0400183D RID: 6205
		Strong
	}
}
