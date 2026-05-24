using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class PeakWidgetController : MonoSingleton<PeakWidgetController>
{
	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00055F41 File Offset: 0x00054141
	// (set) Token: 0x06000A50 RID: 2640 RVA: 0x00055F49 File Offset: 0x00054149
	public bool visible { get; private set; }

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00055F52 File Offset: 0x00054152
	public PeakWidget focussedWidget
	{
		get
		{
			return this._focussedWidget;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00055F5C File Offset: 0x0005415C
	public void ShowWidgets()
	{
		foreach (PeakWidget peakWidget in this._activeWidgets)
		{
			peakWidget.prototype.ReturnToPool();
		}
		this._activeWidgets.Clear();
		foreach (Narrative.NamedPeakWidget namedPeakWidget in Narrative.instance.ProducePeakWidgetContent())
		{
			if (!MonoSingleton<PeakStateController>.instance.active || !(MonoSingleton<PeakStateController>.instance.currentPeakProp == namedPeakWidget.prop))
			{
				Map map = null;
				if (namedPeakWidget.hasMap)
				{
					Map.allByPropName.TryGetValue(namedPeakWidget.prop.inkListItemName, out map);
				}
				PeakWidget peakWidget2 = this.peakWidgetProto.Instantiate<PeakWidget>(null);
				peakWidget2.Setup(namedPeakWidget.prop, namedPeakWidget.name, map);
				peakWidget2.occlusionIndex = this._activeWidgets.Count;
				peakWidget2.isOccluded = true;
				this._activeWidgets.Add(peakWidget2);
			}
		}
		this.visible = true;
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00056098 File Offset: 0x00054298
	public void HideWidgets()
	{
		this.visible = false;
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x000560A1 File Offset: 0x000542A1
	public void ViewWidgetDetail()
	{
		if (!MonoSingleton<JournalController>.instance.canShowAndNotAlreadyVisible)
		{
			MonoSingleton<JournalController>.instance.Open(false);
		}
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x000560BA File Offset: 0x000542BA
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000560D0 File Offset: 0x000542D0
	private void Update()
	{
		if (this.visible && this.focussedWidget != null && !this.focussedWidget.isDisabled && MonoSingleton<PeakStateController>.instance.allowInput && !MonoSingleton<MapsViewController>.instance.maximised && GameInput.selectMenuItem)
		{
			GameInput.ClearInputState();
			this.ViewWidgetDetail();
		}
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x0005612C File Offset: 0x0005432C
	private void OnUIPositionUpdate(GameUI ui)
	{
		PeakWidget peakWidget = null;
		float num = float.MaxValue;
		int num2 = 0;
		Vector2 canvasSize = ui.canvasSize;
		Vector2 vector = new Vector2(0.5f, 0.5f);
		if (MonoSingleton<PeakStateController>.instance.active)
		{
			vector = Vector2.Lerp(vector, MonoSingleton<MapsViewController>.instance.reticulePosNorm, MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm);
		}
		Vector2 vector2 = Vector2.Scale(ui.canvasSize, vector);
		int num3 = Level.currentIndex;
		if (MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel)
		{
			num3++;
		}
		this._sortedVisibleWidgets.Clear();
		foreach (PeakWidget peakWidget2 in this._activeWidgets)
		{
			Vector2 vector3 = ui.WorldToCanvas(peakWidget2.prop.widgetAttachPos, default(Vector2));
			peakWidget2.layout.origin = vector3;
			peakWidget2.isOnScreen = new Rect(Vector2.zero, canvasSize).Contains(vector3);
			if (peakWidget2.isOnScreen)
			{
				num2++;
			}
			float num4 = Vector2.Distance(vector3, vector2);
			if (this.visible && MonoSingleton<PeakStateController>.instance.zoomTransitionSmoothed > 0.3f && num4 < 100f && num4 < num)
			{
				peakWidget = peakWidget2;
				num = num4;
			}
			float num5 = 1f - Mathf.InverseLerp(100f, 400f, num4);
			peakWidget2.maxAlpha = Mathf.Lerp(0f, 1f, num5);
			peakWidget2.wantsVisible = this.visible && peakWidget2.isOnScreen && !peakWidget2.isOccluded && peakWidget2.levelIdx >= num3;
			peakWidget2.distanceForSorting = num4;
			if (peakWidget2.wantsVisible)
			{
				this._sortedVisibleWidgets.Add(peakWidget2);
			}
		}
		this._sortedVisibleWidgets.Sort((PeakWidget w1, PeakWidget w2) => w1.distanceForSorting.CompareTo(w2.distanceForSorting));
		if (this._sortedVisibleWidgets.Count > this.maxVisibleCount)
		{
			this._sortedVisibleWidgets.RemoveRange(this.maxVisibleCount, this._sortedVisibleWidgets.Count - this.maxVisibleCount);
		}
		this._activeWidgets.UpdateAndRemoveIf(delegate(PeakWidget widget)
		{
			float groupAlpha = widget.layout.groupAlpha;
			float num7 = groupAlpha;
			if (this.visible && widget.wantsVisible && this._sortedVisibleWidgets.Contains(widget))
			{
				num7 += Time.deltaTime / 1f;
			}
			else
			{
				num7 -= Time.deltaTime / 0.2f;
			}
			num7 = Mathf.Clamp(num7, 0f, widget.maxAlpha);
			if (num7 != groupAlpha)
			{
				widget.layout.groupAlpha = num7;
			}
			if (!this.visible && groupAlpha <= 0f)
			{
				widget.prototype.ReturnToPool();
				return true;
			}
			return false;
		});
		int frameCount = Time.frameCount;
		int num6 = Level.currentIndex;
		if (MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel)
		{
			num6 = Level.currentIndex + 1;
		}
		foreach (PeakWidget peakWidget3 in this._activeWidgets)
		{
			if (peakWidget3.isOnScreen && (peakWidget3.occlusionIndex + frameCount) % num2 == 0)
			{
				Vector2 origin = peakWidget3.layout.origin;
				Poly poly = Raycast.RayHitAnyPolyAnyLevel(Raycast.ViewportPointToRay(peakWidget3.layout.origin / canvasSize), peakWidget3.prop.transform.position.z - 5f, num6, null);
				peakWidget3.isOccluded = poly != null;
			}
		}
		if (this._focussedWidget != peakWidget)
		{
			if (this._focussedWidget != null)
			{
				this._focussedWidget.UnFocus();
			}
			this._focussedWidget = peakWidget;
			if (this._focussedWidget != null)
			{
				this._focussedWidget.Focus();
			}
		}
	}

	// Token: 0x04000C84 RID: 3204
	public int maxVisibleCount = 5;

	// Token: 0x04000C86 RID: 3206
	private PeakWidget _focussedWidget;

	// Token: 0x04000C87 RID: 3207
	private List<PeakWidget> _activeWidgets = new List<PeakWidget>(128);

	// Token: 0x04000C88 RID: 3208
	private List<PeakWidget> _sortedVisibleWidgets = new List<PeakWidget>(128);

	// Token: 0x04000C89 RID: 3209
	[SerializeField]
	private Prototype peakWidgetProto;
}
