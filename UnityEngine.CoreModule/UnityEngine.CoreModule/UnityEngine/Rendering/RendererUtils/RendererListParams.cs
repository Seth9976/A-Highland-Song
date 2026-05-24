using System;

namespace UnityEngine.Rendering.RendererUtils
{
	// Token: 0x0200042B RID: 1067
	internal struct RendererListParams
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x0003EC02 File Offset: 0x0003CE02
		// (set) Token: 0x0600252C RID: 9516 RVA: 0x0003EC0A File Offset: 0x0003CE0A
		public bool isValid { readonly get; private set; }

		// Token: 0x0600252D RID: 9517 RVA: 0x0003EC14 File Offset: 0x0003CE14
		internal static RendererListParams Create(in RendererListDesc desc)
		{
			RendererListParams rendererListParams = default(RendererListParams);
			RendererListDesc rendererListDesc = desc;
			bool flag = !rendererListDesc.IsValid();
			RendererListParams rendererListParams2;
			if (flag)
			{
				rendererListParams2 = rendererListParams;
			}
			else
			{
				SortingSettings sortingSettings = new SortingSettings(desc.camera)
				{
					criteria = desc.sortingCriteria
				};
				DrawingSettings drawingSettings = new DrawingSettings(RendererListParams.s_EmptyName, sortingSettings)
				{
					perObjectData = desc.rendererConfiguration
				};
				bool flag2 = desc.passName != ShaderTagId.none;
				if (flag2)
				{
					Debug.Assert(desc.passNames == null);
					drawingSettings.SetShaderPassName(0, desc.passName);
				}
				else
				{
					for (int i = 0; i < desc.passNames.Length; i++)
					{
						drawingSettings.SetShaderPassName(i, desc.passNames[i]);
					}
				}
				bool flag3 = desc.overrideMaterial != null;
				if (flag3)
				{
					drawingSettings.overrideMaterial = desc.overrideMaterial;
					drawingSettings.overrideMaterialPassIndex = desc.overrideMaterialPassIndex;
				}
				FilteringSettings filteringSettings = new FilteringSettings(new RenderQueueRange?(desc.renderQueueRange), desc.layerMask, uint.MaxValue, 0)
				{
					excludeMotionVectorObjects = desc.excludeObjectMotionVectors
				};
				rendererListParams.isValid = true;
				rendererListParams.cullingResult = desc.cullingResult;
				rendererListParams.drawSettings = drawingSettings;
				rendererListParams.filteringSettings = filteringSettings;
				rendererListParams.stateBlock = desc.stateBlock;
				rendererListParams2 = rendererListParams;
			}
			return rendererListParams2;
		}

		// Token: 0x04000DDF RID: 3551
		private static readonly ShaderTagId s_EmptyName = new ShaderTagId("");

		// Token: 0x04000DE0 RID: 3552
		public static readonly RendererListParams nullRendererList = default(RendererListParams);

		// Token: 0x04000DE2 RID: 3554
		internal CullingResults cullingResult;

		// Token: 0x04000DE3 RID: 3555
		internal DrawingSettings drawSettings;

		// Token: 0x04000DE4 RID: 3556
		internal FilteringSettings filteringSettings;

		// Token: 0x04000DE5 RID: 3557
		internal RenderStateBlock? stateBlock;
	}
}
