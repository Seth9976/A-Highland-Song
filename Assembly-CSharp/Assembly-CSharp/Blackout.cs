using System;

// Token: 0x02000100 RID: 256
public class Blackout : MonoSingleton<Blackout>
{
	// Token: 0x17000214 RID: 532
	// (get) Token: 0x0600085E RID: 2142 RVA: 0x0004839D File Offset: 0x0004659D
	public static bool isAnimating
	{
		get
		{
			return MonoSingleton<Blackout>.instance.layout.isAnimating;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x0600085F RID: 2143 RVA: 0x000483AE File Offset: 0x000465AE
	public static bool isFullyHidden
	{
		get
		{
			return MonoSingleton<Blackout>.instance.layout.alpha == 0f;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000860 RID: 2144 RVA: 0x000483C6 File Offset: 0x000465C6
	public static bool isFullyVisible
	{
		get
		{
			return MonoSingleton<Blackout>.instance.layout.alpha == 1f;
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000861 RID: 2145 RVA: 0x000483DE File Offset: 0x000465DE
	public static bool isShownOrShowing
	{
		get
		{
			return MonoSingleton<Blackout>.instance.layout.alpha > 0f || MonoSingleton<Blackout>.instance.layout.targetAlpha > 0f;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000862 RID: 2146 RVA: 0x0004840E File Offset: 0x0004660E
	public static bool showing
	{
		get
		{
			return MonoSingleton<Blackout>.instance.layout.targetAlpha > 0f;
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00048426 File Offset: 0x00046626
	public static void Show()
	{
		MonoSingleton<Blackout>.instance.layout.alpha = 1f;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0004843C File Offset: 0x0004663C
	public static void Hide()
	{
		MonoSingleton<Blackout>.instance.layout.alpha = 0f;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00048454 File Offset: 0x00046654
	public static void FadeIn(float delay = 0f, Action then = null)
	{
		MonoSingleton<Blackout>.instance.layout.Animate(Blackout.fadeInTime, delay, delegate
		{
			MonoSingleton<Blackout>.instance.layout.alpha = 0f;
		}).Then(delegate
		{
			if (then != null)
			{
				then();
			}
		});
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x000484B4 File Offset: 0x000466B4
	public static void FadeOut(Action then = null)
	{
		MonoSingleton<Blackout>.instance.layout.Animate(Blackout.fadeOutTime, delegate
		{
			MonoSingleton<Blackout>.instance.layout.alpha = 1f;
		}).Then(delegate
		{
			if (then != null)
			{
				then();
			}
		});
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00048513 File Offset: 0x00046713
	private void Awake()
	{
		this.layout.alpha = 0f;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00048528 File Offset: 0x00046728
	private void Update()
	{
		this.layout.image.enabled = (this.layout.image.raycastTarget = this.layout.alpha > 0f);
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000869 RID: 2153 RVA: 0x0004856A File Offset: 0x0004676A
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

	// Token: 0x04000A20 RID: 2592
	public static float fadeOutTime = 4f;

	// Token: 0x04000A21 RID: 2593
	public static float fadeInTime = 2f;

	// Token: 0x04000A22 RID: 2594
	private SLayout _layout;
}
