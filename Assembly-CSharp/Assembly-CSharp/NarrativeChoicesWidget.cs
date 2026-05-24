using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class NarrativeChoicesWidget : MonoBehaviour
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00055006 File Offset: 0x00053206
	public SLayout layout
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

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00055028 File Offset: 0x00053228
	// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00055030 File Offset: 0x00053230
	public bool visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			if (value != this.visible)
			{
				this._visible = value;
				this.layout.Animate(value ? 0.2f : 0.5f, delegate
				{
					this.layout.groupAlpha = (float)(this._visible ? 1 : 0);
				}).Then(delegate
				{
					if (!this.visible)
					{
						base.GetComponent<Prototype>().ReturnToPool();
					}
				});
			}
		}
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00055088 File Offset: 0x00053288
	private void Start()
	{
		Prototype component = base.GetComponent<Prototype>();
		if (!component.isOriginalPrototype)
		{
			component.OnReturnToPool += this.OnReturnToPool;
		}
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x000550B6 File Offset: 0x000532B6
	private void OnDestroy()
	{
		base.GetComponent<Prototype>().OnReturnToPool -= this.OnReturnToPool;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x000550CF File Offset: 0x000532CF
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x000550E2 File Offset: 0x000532E2
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x000550F8 File Offset: 0x000532F8
	public void Setup(Transform attachTo, List<GameChoice> choices, int maxToDisplay = 2147483647, bool canRunOff = false)
	{
		if (attachTo == null)
		{
			Debug.LogWarning("Choices widget attachTo is null!");
		}
		this.attachTo = attachTo;
		this.choicesWidget.SetChoices(choices, maxToDisplay, false, canRunOff);
		this.layout.size = this.choicesWidget.layout.size;
		SLayout.WithoutAnimating(delegate
		{
			this.visible = false;
			this.layout.groupAlpha = 0f;
		});
		this.visible = true;
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00055162 File Offset: 0x00053362
	private void OnReturnToPool()
	{
		this.attachTo = null;
		this.layout.CancelAnimations();
		this.choicesWidget.Clear();
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00055184 File Offset: 0x00053384
	private void OnUIPositionUpdate(GameUI ui)
	{
		this.layout.origin = ui.WorldToCanvas(this.attachTo.position, default(Vector2));
		Rect rect = this.layout.rect;
		rect.width += 70f;
		rect = ui.ContstrainWithinSafeZones(rect);
		rect.width -= 70f;
		this.layout.rect = rect;
	}

	// Token: 0x04000C3C RID: 3132
	public Transform attachTo;

	// Token: 0x04000C3D RID: 3133
	private SLayout _layout;

	// Token: 0x04000C3E RID: 3134
	public ChoicesWidget choicesWidget;

	// Token: 0x04000C3F RID: 3135
	private bool _visible;
}
