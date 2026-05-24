using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200020E RID: 526
	internal static class PointerDeviceState
	{
		// Token: 0x06000FD7 RID: 4055 RVA: 0x0003E9C4 File Offset: 0x0003CBC4
		internal static void Reset()
		{
			for (int i = 0; i < PointerId.maxPointers; i++)
			{
				PointerDeviceState.s_PlayerPointerLocations[i].SetLocation(Vector2.zero, null);
				PointerDeviceState.s_PressedButtons[i] = 0;
				PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[i] = null;
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0003EA10 File Offset: 0x0003CC10
		internal static void RemovePanelData(IPanel panel)
		{
			for (int i = 0; i < PointerId.maxPointers; i++)
			{
				bool flag = PointerDeviceState.s_PlayerPointerLocations[i].Panel == panel;
				if (flag)
				{
					PointerDeviceState.s_PlayerPointerLocations[i].SetLocation(Vector2.zero, null);
				}
				bool flag2 = PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[i] == panel;
				if (flag2)
				{
					PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[i] = null;
				}
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0003EA78 File Offset: 0x0003CC78
		public static void SavePointerPosition(int pointerId, Vector2 position, IPanel panel, ContextType contextType)
		{
			if (contextType > ContextType.Editor)
			{
			}
			PointerDeviceState.s_PlayerPointerLocations[pointerId].SetLocation(position, panel);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0003EAA5 File Offset: 0x0003CCA5
		public static void PressButton(int pointerId, int buttonId)
		{
			Debug.Assert(buttonId >= 0);
			Debug.Assert(buttonId < 32);
			PointerDeviceState.s_PressedButtons[pointerId] |= 1 << buttonId;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0003EAD5 File Offset: 0x0003CCD5
		public static void ReleaseButton(int pointerId, int buttonId)
		{
			Debug.Assert(buttonId >= 0);
			Debug.Assert(buttonId < 32);
			PointerDeviceState.s_PressedButtons[pointerId] &= ~(1 << buttonId);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0003EB06 File Offset: 0x0003CD06
		public static void ReleaseAllButtons(int pointerId)
		{
			PointerDeviceState.s_PressedButtons[pointerId] = 0;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0003EB14 File Offset: 0x0003CD14
		public static Vector2 GetPointerPosition(int pointerId, ContextType contextType)
		{
			if (contextType > ContextType.Editor)
			{
			}
			return PointerDeviceState.s_PlayerPointerLocations[pointerId].Position;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0003EB40 File Offset: 0x0003CD40
		public static IPanel GetPanel(int pointerId, ContextType contextType)
		{
			if (contextType > ContextType.Editor)
			{
			}
			return PointerDeviceState.s_PlayerPointerLocations[pointerId].Panel;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0003EB6C File Offset: 0x0003CD6C
		private static bool HasFlagFast(PointerDeviceState.LocationFlag flagSet, PointerDeviceState.LocationFlag flag)
		{
			return (flagSet & flag) == flag;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0003EB84 File Offset: 0x0003CD84
		public static bool HasLocationFlag(int pointerId, ContextType contextType, PointerDeviceState.LocationFlag flag)
		{
			if (contextType > ContextType.Editor)
			{
			}
			return PointerDeviceState.HasFlagFast(PointerDeviceState.s_PlayerPointerLocations[pointerId].Flags, flag);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
		public static int GetPressedButtons(int pointerId)
		{
			return PointerDeviceState.s_PressedButtons[pointerId];
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0003EBD4 File Offset: 0x0003CDD4
		internal static bool HasAdditionalPressedButtons(int pointerId, int exceptButtonId)
		{
			return (PointerDeviceState.s_PressedButtons[pointerId] & ~(1 << exceptButtonId)) != 0;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0003EBF8 File Offset: 0x0003CDF8
		internal static void SetPlayerPanelWithSoftPointerCapture(int pointerId, IPanel panel)
		{
			PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[pointerId] = panel;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0003EC04 File Offset: 0x0003CE04
		internal static IPanel GetPlayerPanelWithSoftPointerCapture(int pointerId)
		{
			return PointerDeviceState.s_PlayerPanelWithSoftPointerCapture[pointerId];
		}

		// Token: 0x040006F8 RID: 1784
		private static PointerDeviceState.PointerLocation[] s_PlayerPointerLocations = new PointerDeviceState.PointerLocation[PointerId.maxPointers];

		// Token: 0x040006F9 RID: 1785
		private static int[] s_PressedButtons = new int[PointerId.maxPointers];

		// Token: 0x040006FA RID: 1786
		private static readonly IPanel[] s_PlayerPanelWithSoftPointerCapture = new IPanel[PointerId.maxPointers];

		// Token: 0x0200020F RID: 527
		[Flags]
		internal enum LocationFlag
		{
			// Token: 0x040006FC RID: 1788
			None = 0,
			// Token: 0x040006FD RID: 1789
			OutsidePanel = 1
		}

		// Token: 0x02000210 RID: 528
		private struct PointerLocation
		{
			// Token: 0x17000362 RID: 866
			// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0003EC4C File Offset: 0x0003CE4C
			// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0003EC54 File Offset: 0x0003CE54
			internal Vector2 Position { readonly get; private set; }

			// Token: 0x17000363 RID: 867
			// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0003EC5D File Offset: 0x0003CE5D
			// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0003EC65 File Offset: 0x0003CE65
			internal IPanel Panel { readonly get; private set; }

			// Token: 0x17000364 RID: 868
			// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0003EC6E File Offset: 0x0003CE6E
			// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0003EC76 File Offset: 0x0003CE76
			internal PointerDeviceState.LocationFlag Flags { readonly get; private set; }

			// Token: 0x06000FEC RID: 4076 RVA: 0x0003EC80 File Offset: 0x0003CE80
			internal void SetLocation(Vector2 position, IPanel panel)
			{
				this.Position = position;
				this.Panel = panel;
				this.Flags = PointerDeviceState.LocationFlag.None;
				bool flag = panel == null || !panel.visualTree.layout.Contains(position);
				if (flag)
				{
					this.Flags |= PointerDeviceState.LocationFlag.OutsidePanel;
				}
			}
		}
	}
}
