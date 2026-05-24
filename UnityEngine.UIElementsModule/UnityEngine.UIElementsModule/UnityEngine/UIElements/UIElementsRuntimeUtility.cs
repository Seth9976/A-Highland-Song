using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x020000BD RID: 189
	internal static class UIElementsRuntimeUtility
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000638 RID: 1592 RVA: 0x000172B0 File Offset: 0x000154B0
		// (remove) Token: 0x06000639 RID: 1593 RVA: 0x000172E4 File Offset: 0x000154E4
		[field: DebuggerBrowsable(0)]
		private static event Action s_onRepaintOverlayPanels;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600063A RID: 1594 RVA: 0x00017318 File Offset: 0x00015518
		// (remove) Token: 0x0600063B RID: 1595 RVA: 0x00017344 File Offset: 0x00015544
		internal static event Action onRepaintOverlayPanels
		{
			add
			{
				bool flag = UIElementsRuntimeUtility.s_onRepaintOverlayPanels == null;
				if (flag)
				{
					UIElementsRuntimeUtility.RegisterPlayerloopCallback();
				}
				UIElementsRuntimeUtility.s_onRepaintOverlayPanels += value;
			}
			remove
			{
				UIElementsRuntimeUtility.s_onRepaintOverlayPanels -= value;
				bool flag = UIElementsRuntimeUtility.s_onRepaintOverlayPanels == null;
				if (flag)
				{
					UIElementsRuntimeUtility.UnregisterPlayerloopCallback();
				}
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600063C RID: 1596 RVA: 0x00017370 File Offset: 0x00015570
		// (remove) Token: 0x0600063D RID: 1597 RVA: 0x000173A4 File Offset: 0x000155A4
		[field: DebuggerBrowsable(0)]
		public static event Action<BaseRuntimePanel> onCreatePanel;

		// Token: 0x0600063E RID: 1598 RVA: 0x000173D8 File Offset: 0x000155D8
		static UIElementsRuntimeUtility()
		{
			UIElementsRuntimeUtilityNative.RepaintOverlayPanelsCallback = delegate
			{
			};
			Canvas.externBeginRenderOverlays = new Action<int>(UIElementsRuntimeUtility.BeginRenderOverlays);
			Canvas.externRenderOverlaysBefore = delegate(int displayIndex, int sortOrder)
			{
				UIElementsRuntimeUtility.RenderOverlaysBeforePriority(displayIndex, (float)sortOrder);
			};
			Canvas.externEndRenderOverlays = new Action<int>(UIElementsRuntimeUtility.EndRenderOverlays);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00017474 File Offset: 0x00015674
		public static EventBase CreateEvent(Event systemEvent)
		{
			return UIElementsUtility.CreateEvent(systemEvent, systemEvent.rawType);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00017494 File Offset: 0x00015694
		public static BaseRuntimePanel FindOrCreateRuntimePanel(ScriptableObject ownerObject, UIElementsRuntimeUtility.CreateRuntimePanelDelegate createDelegate)
		{
			Panel panel;
			bool flag = UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out panel);
			if (flag)
			{
				BaseRuntimePanel baseRuntimePanel = panel as BaseRuntimePanel;
				bool flag2 = baseRuntimePanel != null;
				if (flag2)
				{
					return baseRuntimePanel;
				}
				UIElementsRuntimeUtility.RemoveCachedPanelInternal(ownerObject.GetInstanceID());
			}
			BaseRuntimePanel baseRuntimePanel2 = createDelegate(ownerObject);
			baseRuntimePanel2.IMGUIEventInterests = new EventInterests
			{
				wantsMouseMove = true,
				wantsMouseEnterLeaveWindow = true
			};
			UIElementsRuntimeUtility.RegisterCachedPanelInternal(ownerObject.GetInstanceID(), baseRuntimePanel2);
			Action<BaseRuntimePanel> action = UIElementsRuntimeUtility.onCreatePanel;
			if (action != null)
			{
				action.Invoke(baseRuntimePanel2);
			}
			return baseRuntimePanel2;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00017528 File Offset: 0x00015728
		public static void DisposeRuntimePanel(ScriptableObject ownerObject)
		{
			Panel panel;
			bool flag = UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out panel);
			if (flag)
			{
				panel.Dispose();
				UIElementsRuntimeUtility.RemoveCachedPanelInternal(ownerObject.GetInstanceID());
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001755C File Offset: 0x0001575C
		private static void RegisterCachedPanelInternal(int instanceID, IPanel panel)
		{
			UIElementsUtility.RegisterCachedPanel(instanceID, panel as Panel);
			UIElementsRuntimeUtility.s_PanelOrderingDirty = true;
			bool flag = !UIElementsRuntimeUtility.s_RegisteredPlayerloopCallback;
			if (flag)
			{
				UIElementsRuntimeUtility.s_RegisteredPlayerloopCallback = true;
				UIElementsRuntimeUtility.RegisterPlayerloopCallback();
				Canvas.SetExternalCanvasEnabled(true);
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000175A0 File Offset: 0x000157A0
		private static void RemoveCachedPanelInternal(int instanceID)
		{
			UIElementsUtility.RemoveCachedPanel(instanceID);
			UIElementsRuntimeUtility.s_PanelOrderingDirty = true;
			UIElementsRuntimeUtility.s_SortedRuntimePanels.Clear();
			UIElementsUtility.GetAllPanels(UIElementsRuntimeUtility.s_SortedRuntimePanels, ContextType.Player);
			bool flag = UIElementsRuntimeUtility.s_SortedRuntimePanels.Count == 0;
			if (flag)
			{
				UIElementsRuntimeUtility.s_RegisteredPlayerloopCallback = false;
				UIElementsRuntimeUtility.UnregisterPlayerloopCallback();
				Canvas.SetExternalCanvasEnabled(false);
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000175F8 File Offset: 0x000157F8
		public static void RepaintOverlayPanels()
		{
			foreach (Panel panel in UIElementsRuntimeUtility.GetSortedPlayerPanels())
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)panel;
				bool flag = !baseRuntimePanel.drawToCameras;
				if (flag)
				{
					UIElementsRuntimeUtility.RepaintOverlayPanel(baseRuntimePanel);
				}
			}
			bool flag2 = UIElementsRuntimeUtility.s_onRepaintOverlayPanels != null;
			if (flag2)
			{
				UIElementsRuntimeUtility.s_onRepaintOverlayPanels.Invoke();
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001767C File Offset: 0x0001587C
		public static void RepaintOverlayPanel(BaseRuntimePanel panel)
		{
			Camera current = Camera.current;
			RenderTexture active = RenderTexture.active;
			Camera.SetupCurrent(null);
			RenderTexture.active = null;
			using (UIElementsRuntimeUtility.s_RepaintProfilerMarker.Auto())
			{
				panel.Repaint(Event.current);
			}
			Camera.SetupCurrent(current);
			RenderTexture.active = active;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000176F0 File Offset: 0x000158F0
		internal static void BeginRenderOverlays(int displayIndex)
		{
			UIElementsRuntimeUtility.currentOverlayIndex = 0;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000176FC File Offset: 0x000158FC
		internal static void RenderOverlaysBeforePriority(int displayIndex, float maxPriority)
		{
			bool flag = UIElementsRuntimeUtility.currentOverlayIndex < 0;
			if (!flag)
			{
				List<Panel> sortedPlayerPanels = UIElementsRuntimeUtility.GetSortedPlayerPanels();
				while (UIElementsRuntimeUtility.currentOverlayIndex < sortedPlayerPanels.Count)
				{
					BaseRuntimePanel baseRuntimePanel = sortedPlayerPanels[UIElementsRuntimeUtility.currentOverlayIndex] as BaseRuntimePanel;
					bool flag2 = baseRuntimePanel != null;
					if (flag2)
					{
						bool flag3 = baseRuntimePanel.sortingPriority >= maxPriority;
						if (flag3)
						{
							break;
						}
						bool flag4 = baseRuntimePanel.targetDisplay == displayIndex;
						if (flag4)
						{
							UIElementsRuntimeUtility.RepaintOverlayPanel(baseRuntimePanel);
						}
					}
					UIElementsRuntimeUtility.currentOverlayIndex++;
				}
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00017788 File Offset: 0x00015988
		internal static void EndRenderOverlays(int displayIndex)
		{
			UIElementsRuntimeUtility.RenderOverlaysBeforePriority(displayIndex, float.MaxValue);
			UIElementsRuntimeUtility.currentOverlayIndex = -1;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0001779D File Offset: 0x0001599D
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x000177A4 File Offset: 0x000159A4
		internal static Object activeEventSystem { get; private set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x000177AC File Offset: 0x000159AC
		internal static bool useDefaultEventSystem
		{
			get
			{
				return UIElementsRuntimeUtility.activeEventSystem == null;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000177BC File Offset: 0x000159BC
		public static void RegisterEventSystem(Object eventSystem)
		{
			bool flag = UIElementsRuntimeUtility.activeEventSystem != null && UIElementsRuntimeUtility.activeEventSystem != eventSystem && eventSystem.GetType().Name == "EventSystem";
			if (flag)
			{
				Debug.LogWarning("There can be only one active Event System.");
			}
			UIElementsRuntimeUtility.activeEventSystem = eventSystem;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00017814 File Offset: 0x00015A14
		public static void UnregisterEventSystem(Object eventSystem)
		{
			bool flag = UIElementsRuntimeUtility.activeEventSystem == eventSystem;
			if (flag)
			{
				UIElementsRuntimeUtility.activeEventSystem = null;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00017838 File Offset: 0x00015A38
		internal static DefaultEventSystem defaultEventSystem
		{
			get
			{
				DefaultEventSystem defaultEventSystem;
				if ((defaultEventSystem = UIElementsRuntimeUtility.s_DefaultEventSystem) == null)
				{
					defaultEventSystem = (UIElementsRuntimeUtility.s_DefaultEventSystem = new DefaultEventSystem());
				}
				return defaultEventSystem;
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00017850 File Offset: 0x00015A50
		public static void UpdateRuntimePanels()
		{
			UIElementsRuntimeUtility.RemoveUnusedPanels();
			foreach (Panel panel in UIElementsRuntimeUtility.GetSortedPlayerPanels())
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)panel;
				baseRuntimePanel.Update();
			}
			bool flag = Application.isPlaying && UIElementsRuntimeUtility.useDefaultEventSystem;
			if (flag)
			{
				UIElementsRuntimeUtility.defaultEventSystem.Update(DefaultEventSystem.UpdateMode.IgnoreIfAppNotFocused);
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000178D4 File Offset: 0x00015AD4
		internal static void MarkPotentiallyEmpty(PanelSettings settings)
		{
			bool flag = !UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings.Contains(settings);
			if (flag)
			{
				UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings.Add(settings);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00017900 File Offset: 0x00015B00
		internal static void RemoveUnusedPanels()
		{
			foreach (PanelSettings panelSettings in UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings)
			{
				UIDocumentList attachedUIDocumentsList = panelSettings.m_AttachedUIDocumentsList;
				bool flag = attachedUIDocumentsList == null || attachedUIDocumentsList.m_AttachedUIDocuments.Count == 0;
				if (flag)
				{
					panelSettings.DisposePanel();
				}
			}
			UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings.Clear();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00017984 File Offset: 0x00015B84
		public static void RegisterPlayerloopCallback()
		{
			UIElementsRuntimeUtilityNative.RegisterPlayerloopCallback();
			UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback = new Action(UIElementsRuntimeUtility.UpdateRuntimePanels);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001799E File Offset: 0x00015B9E
		public static void UnregisterPlayerloopCallback()
		{
			UIElementsRuntimeUtilityNative.UnregisterPlayerloopCallback();
			UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback = null;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000179AD File Offset: 0x00015BAD
		internal static void SetPanelOrderingDirty()
		{
			UIElementsRuntimeUtility.s_PanelOrderingDirty = true;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000179B8 File Offset: 0x00015BB8
		internal static List<Panel> GetSortedPlayerPanels()
		{
			bool flag = UIElementsRuntimeUtility.s_PanelOrderingDirty;
			if (flag)
			{
				UIElementsRuntimeUtility.SortPanels();
			}
			return UIElementsRuntimeUtility.s_SortedRuntimePanels;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000179E0 File Offset: 0x00015BE0
		private static void SortPanels()
		{
			UIElementsRuntimeUtility.s_SortedRuntimePanels.Clear();
			UIElementsUtility.GetAllPanels(UIElementsRuntimeUtility.s_SortedRuntimePanels, ContextType.Player);
			UIElementsRuntimeUtility.s_SortedRuntimePanels.Sort(delegate(Panel a, Panel b)
			{
				BaseRuntimePanel baseRuntimePanel = a as BaseRuntimePanel;
				BaseRuntimePanel baseRuntimePanel2 = b as BaseRuntimePanel;
				bool flag = baseRuntimePanel == null || baseRuntimePanel2 == null;
				int num;
				if (flag)
				{
					num = 0;
				}
				else
				{
					float num2 = baseRuntimePanel.sortingPriority - baseRuntimePanel2.sortingPriority;
					bool flag2 = Mathf.Approximately(0f, num2);
					if (flag2)
					{
						num = baseRuntimePanel.m_RuntimePanelCreationIndex.CompareTo(baseRuntimePanel2.m_RuntimePanelCreationIndex);
					}
					else
					{
						num = ((num2 < 0f) ? (-1) : 1);
					}
				}
				return num;
			});
			UIElementsRuntimeUtility.s_PanelOrderingDirty = false;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00017A38 File Offset: 0x00015C38
		internal static Vector2 MultiDisplayBottomLeftToPanelPosition(Vector2 position, out int? targetDisplay)
		{
			Vector2 vector = UIElementsRuntimeUtility.MultiDisplayToLocalScreenPosition(position, out targetDisplay);
			return UIElementsRuntimeUtility.ScreenBottomLeftToPanelPosition(vector, targetDisplay.GetValueOrDefault());
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00017A60 File Offset: 0x00015C60
		internal static Vector2 MultiDisplayToLocalScreenPosition(Vector2 position, out int? targetDisplay)
		{
			Vector3 vector = Display.RelativeMouseAt(position);
			bool flag = vector != Vector3.zero;
			Vector2 vector2;
			if (flag)
			{
				targetDisplay = new int?((int)vector.z);
				vector2 = vector;
			}
			else
			{
				targetDisplay = default(int?);
				vector2 = position;
			}
			return vector2;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00017AB4 File Offset: 0x00015CB4
		internal static Vector2 ScreenBottomLeftToPanelPosition(Vector2 position, int targetDisplay)
		{
			int num = Screen.height;
			bool flag = targetDisplay > 0 && targetDisplay < Display.displays.Length;
			if (flag)
			{
				num = Display.displays[targetDisplay].systemHeight;
			}
			position.y = (float)num - position.y;
			return position;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00017B00 File Offset: 0x00015D00
		internal static Vector2 ScreenBottomLeftToPanelDelta(Vector2 delta)
		{
			delta.y = -delta.y;
			return delta;
		}

		// Token: 0x0400027F RID: 639
		private static bool s_RegisteredPlayerloopCallback = false;

		// Token: 0x04000280 RID: 640
		private static List<Panel> s_SortedRuntimePanels = new List<Panel>();

		// Token: 0x04000281 RID: 641
		private static bool s_PanelOrderingDirty = true;

		// Token: 0x04000282 RID: 642
		internal static readonly string s_RepaintProfilerMarkerName = "UIElementsRuntimeUtility.DoDispatch(Repaint Event)";

		// Token: 0x04000283 RID: 643
		private static readonly ProfilerMarker s_RepaintProfilerMarker = new ProfilerMarker(UIElementsRuntimeUtility.s_RepaintProfilerMarkerName);

		// Token: 0x04000284 RID: 644
		private static int currentOverlayIndex = -1;

		// Token: 0x04000286 RID: 646
		private static DefaultEventSystem s_DefaultEventSystem;

		// Token: 0x04000287 RID: 647
		private static List<PanelSettings> s_PotentiallyEmptyPanelSettings = new List<PanelSettings>();

		// Token: 0x020000BE RID: 190
		// (Invoke) Token: 0x0600065C RID: 1628
		public delegate BaseRuntimePanel CreateRuntimePanelDelegate(ScriptableObject ownerObject);
	}
}
