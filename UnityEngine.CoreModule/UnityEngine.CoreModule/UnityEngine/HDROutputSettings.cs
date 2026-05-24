using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000136 RID: 310
	[UsedByNativeCode]
	[NativeHeader("Runtime/GfxDevice/HDROutputSettings.h")]
	public class HDROutputSettings
	{
		// Token: 0x060009C0 RID: 2496 RVA: 0x0000EB6C File Offset: 0x0000CD6C
		internal HDROutputSettings()
		{
			this.m_DisplayIndex = 0;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0000EB7D File Offset: 0x0000CD7D
		internal HDROutputSettings(int displayIndex)
		{
			this.m_DisplayIndex = displayIndex;
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0000EB90 File Offset: 0x0000CD90
		public static HDROutputSettings main
		{
			get
			{
				return HDROutputSettings._mainDisplay;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0000EBA8 File Offset: 0x0000CDA8
		public bool active
		{
			get
			{
				return HDROutputSettings.GetActive(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0000EBC8 File Offset: 0x0000CDC8
		public bool available
		{
			get
			{
				return HDROutputSettings.GetAvailable(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0000EC05 File Offset: 0x0000CE05
		public bool automaticHDRTonemapping
		{
			get
			{
				return HDROutputSettings.GetAutomaticHDRTonemapping(this.m_DisplayIndex);
			}
			set
			{
				HDROutputSettings.SetAutomaticHDRTonemapping(this.m_DisplayIndex, value);
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0000EC18 File Offset: 0x0000CE18
		public ColorGamut displayColorGamut
		{
			get
			{
				return HDROutputSettings.GetDisplayColorGamut(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0000EC38 File Offset: 0x0000CE38
		public RenderTextureFormat format
		{
			get
			{
				return GraphicsFormatUtility.GetRenderTextureFormat(HDROutputSettings.GetGraphicsFormat(this.m_DisplayIndex));
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0000EC5C File Offset: 0x0000CE5C
		public GraphicsFormat graphicsFormat
		{
			get
			{
				return HDROutputSettings.GetGraphicsFormat(this.m_DisplayIndex);
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0000EC99 File Offset: 0x0000CE99
		public float paperWhiteNits
		{
			get
			{
				return HDROutputSettings.GetPaperWhiteNits(this.m_DisplayIndex);
			}
			set
			{
				HDROutputSettings.SetPaperWhiteNits(this.m_DisplayIndex, value);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		public int maxFullFrameToneMapLuminance
		{
			get
			{
				return HDROutputSettings.GetMaxFullFrameToneMapLuminance(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x0000ECCC File Offset: 0x0000CECC
		public int maxToneMapLuminance
		{
			get
			{
				return HDROutputSettings.GetMaxToneMapLuminance(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0000ECEC File Offset: 0x0000CEEC
		public int minToneMapLuminance
		{
			get
			{
				return HDROutputSettings.GetMinToneMapLuminance(this.m_DisplayIndex);
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0000ED0C File Offset: 0x0000CF0C
		public bool HDRModeChangeRequested
		{
			get
			{
				return HDROutputSettings.GetHDRModeChangeRequested(this.m_DisplayIndex);
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0000ED29 File Offset: 0x0000CF29
		public void RequestHDRModeChange(bool enabled)
		{
			HDROutputSettings.RequestHDRModeChangeInternal(this.m_DisplayIndex, enabled);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0000ED3C File Offset: 0x0000CF3C
		[Obsolete("SetPaperWhiteInNits is deprecated, please use paperWhiteNits instead.")]
		public static void SetPaperWhiteInNits(float paperWhite)
		{
			int num = 0;
			bool available = HDROutputSettings.GetAvailable(num);
			if (available)
			{
				HDROutputSettings.SetPaperWhiteNits(num, paperWhite);
			}
		}

		// Token: 0x060009D2 RID: 2514
		[FreeFunction("HDROutputSettingsBindings::GetActive", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern bool GetActive(int displayIndex);

		// Token: 0x060009D3 RID: 2515
		[FreeFunction("HDROutputSettingsBindings::GetAvailable", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern bool GetAvailable(int displayIndex);

		// Token: 0x060009D4 RID: 2516
		[FreeFunction("HDROutputSettingsBindings::GetAutomaticHDRTonemapping", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern bool GetAutomaticHDRTonemapping(int displayIndex);

		// Token: 0x060009D5 RID: 2517
		[FreeFunction("HDROutputSettingsBindings::SetAutomaticHDRTonemapping", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetAutomaticHDRTonemapping(int displayIndex, bool scripted);

		// Token: 0x060009D6 RID: 2518
		[FreeFunction("HDROutputSettingsBindings::GetDisplayColorGamut", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern ColorGamut GetDisplayColorGamut(int displayIndex);

		// Token: 0x060009D7 RID: 2519
		[FreeFunction("HDROutputSettingsBindings::GetGraphicsFormat", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern GraphicsFormat GetGraphicsFormat(int displayIndex);

		// Token: 0x060009D8 RID: 2520
		[FreeFunction("HDROutputSettingsBindings::GetPaperWhiteNits", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern float GetPaperWhiteNits(int displayIndex);

		// Token: 0x060009D9 RID: 2521
		[FreeFunction("HDROutputSettingsBindings::SetPaperWhiteNits", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetPaperWhiteNits(int displayIndex, float paperWhite);

		// Token: 0x060009DA RID: 2522
		[FreeFunction("HDROutputSettingsBindings::GetMaxFullFrameToneMapLuminance", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern int GetMaxFullFrameToneMapLuminance(int displayIndex);

		// Token: 0x060009DB RID: 2523
		[FreeFunction("HDROutputSettingsBindings::GetMaxToneMapLuminance", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern int GetMaxToneMapLuminance(int displayIndex);

		// Token: 0x060009DC RID: 2524
		[FreeFunction("HDROutputSettingsBindings::GetMinToneMapLuminance", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern int GetMinToneMapLuminance(int displayIndex);

		// Token: 0x060009DD RID: 2525
		[FreeFunction("HDROutputSettingsBindings::GetHDRModeChangeRequested", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern bool GetHDRModeChangeRequested(int displayIndex);

		// Token: 0x060009DE RID: 2526
		[FreeFunction("HDROutputSettingsBindings::RequestHDRModeChange", HasExplicitThis = false, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void RequestHDRModeChangeInternal(int displayIndex, bool enabled);

		// Token: 0x040003E0 RID: 992
		private int m_DisplayIndex;

		// Token: 0x040003E1 RID: 993
		public static HDROutputSettings[] displays = new HDROutputSettings[]
		{
			new HDROutputSettings()
		};

		// Token: 0x040003E2 RID: 994
		private static HDROutputSettings _mainDisplay = HDROutputSettings.displays[0];
	}
}
