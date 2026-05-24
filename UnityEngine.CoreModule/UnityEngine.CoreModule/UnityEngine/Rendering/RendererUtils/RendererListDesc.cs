using System;

namespace UnityEngine.Rendering.RendererUtils
{
	// Token: 0x0200042A RID: 1066
	public struct RendererListDesc
	{
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x0003EAF3 File Offset: 0x0003CCF3
		// (set) Token: 0x06002521 RID: 9505 RVA: 0x0003EAFB File Offset: 0x0003CCFB
		internal CullingResults cullingResult { readonly get; private set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x0003EB04 File Offset: 0x0003CD04
		// (set) Token: 0x06002523 RID: 9507 RVA: 0x0003EB0C File Offset: 0x0003CD0C
		internal Camera camera { readonly get; set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x0003EB15 File Offset: 0x0003CD15
		// (set) Token: 0x06002525 RID: 9509 RVA: 0x0003EB1D File Offset: 0x0003CD1D
		internal ShaderTagId passName { readonly get; private set; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x0003EB26 File Offset: 0x0003CD26
		// (set) Token: 0x06002527 RID: 9511 RVA: 0x0003EB2E File Offset: 0x0003CD2E
		internal ShaderTagId[] passNames { readonly get; private set; }

		// Token: 0x06002528 RID: 9512 RVA: 0x0003EB37 File Offset: 0x0003CD37
		public RendererListDesc(ShaderTagId passName, CullingResults cullingResult, Camera camera)
		{
			this = default(RendererListDesc);
			this.passName = passName;
			this.passNames = null;
			this.cullingResult = cullingResult;
			this.camera = camera;
			this.layerMask = -1;
			this.overrideMaterialPassIndex = 0;
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x0003EB6F File Offset: 0x0003CD6F
		public RendererListDesc(ShaderTagId[] passNames, CullingResults cullingResult, Camera camera)
		{
			this = default(RendererListDesc);
			this.passNames = passNames;
			this.passName = ShaderTagId.none;
			this.cullingResult = cullingResult;
			this.camera = camera;
			this.layerMask = -1;
			this.overrideMaterialPassIndex = 0;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x0003EBAC File Offset: 0x0003CDAC
		public bool IsValid()
		{
			bool flag = this.camera == null || (this.passName == ShaderTagId.none && (this.passNames == null || this.passNames.Length == 0));
			return !flag;
		}

		// Token: 0x04000DD3 RID: 3539
		public SortingCriteria sortingCriteria;

		// Token: 0x04000DD4 RID: 3540
		public PerObjectData rendererConfiguration;

		// Token: 0x04000DD5 RID: 3541
		public RenderQueueRange renderQueueRange;

		// Token: 0x04000DD6 RID: 3542
		public RenderStateBlock? stateBlock;

		// Token: 0x04000DD7 RID: 3543
		public Material overrideMaterial;

		// Token: 0x04000DD8 RID: 3544
		public bool excludeObjectMotionVectors;

		// Token: 0x04000DD9 RID: 3545
		public int layerMask;

		// Token: 0x04000DDA RID: 3546
		public int overrideMaterialPassIndex;
	}
}
