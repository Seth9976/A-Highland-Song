using System;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000139 RID: 313
	public struct RenderParams
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		public RenderParams(Material mat)
		{
			this.layer = 0;
			this.renderingLayerMask = GraphicsSettings.defaultRenderingLayerMask;
			this.rendererPriority = 0;
			this.worldBounds = new Bounds(Vector3.zero, Vector3.zero);
			this.camera = null;
			this.motionVectorMode = MotionVectorGenerationMode.Camera;
			this.reflectionProbeUsage = ReflectionProbeUsage.Off;
			this.material = mat;
			this.matProps = null;
			this.shadowCastingMode = ShadowCastingMode.Off;
			this.receiveShadows = false;
			this.lightProbeUsage = LightProbeUsage.Off;
			this.lightProbeProxyVolume = null;
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0000F074 File Offset: 0x0000D274
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0000F07C File Offset: 0x0000D27C
		public int layer { readonly get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0000F085 File Offset: 0x0000D285
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0000F08D File Offset: 0x0000D28D
		public uint renderingLayerMask { readonly get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0000F096 File Offset: 0x0000D296
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x0000F09E File Offset: 0x0000D29E
		public int rendererPriority { readonly get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0000F0A7 File Offset: 0x0000D2A7
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0000F0AF File Offset: 0x0000D2AF
		public Bounds worldBounds { readonly get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		public Camera camera { readonly get; set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0000F0C9 File Offset: 0x0000D2C9
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x0000F0D1 File Offset: 0x0000D2D1
		public MotionVectorGenerationMode motionVectorMode { readonly get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0000F0DA File Offset: 0x0000D2DA
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x0000F0E2 File Offset: 0x0000D2E2
		public ReflectionProbeUsage reflectionProbeUsage { readonly get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0000F0EB File Offset: 0x0000D2EB
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x0000F0F3 File Offset: 0x0000D2F3
		public Material material { readonly get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0000F0FC File Offset: 0x0000D2FC
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x0000F104 File Offset: 0x0000D304
		public MaterialPropertyBlock matProps { readonly get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0000F10D File Offset: 0x0000D30D
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0000F115 File Offset: 0x0000D315
		public ShadowCastingMode shadowCastingMode { readonly get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0000F11E File Offset: 0x0000D31E
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0000F126 File Offset: 0x0000D326
		public bool receiveShadows { readonly get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0000F12F File Offset: 0x0000D32F
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0000F137 File Offset: 0x0000D337
		public LightProbeUsage lightProbeUsage { readonly get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0000F140 File Offset: 0x0000D340
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x0000F148 File Offset: 0x0000D348
		public LightProbeProxyVolume lightProbeProxyVolume { readonly get; set; }
	}
}
