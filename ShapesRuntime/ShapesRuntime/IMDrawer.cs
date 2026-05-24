using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000018 RID: 24
	internal struct IMDrawer : IDisposable
	{
		// Token: 0x0600092B RID: 2347 RVA: 0x00021684 File Offset: 0x0001F884
		private static string[] GetMaterialKeywords(Material m)
		{
			string[] array;
			if (!IMDrawer.matKeywords.TryGetValue(m, out array))
			{
				array = (IMDrawer.matKeywords[m] = m.shaderKeywords);
			}
			return array;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000216B4 File Offset: 0x0001F8B4
		public IMDrawer(MetaMpb metaMpb, Material sourceMat, Mesh sourceMesh, int submesh = 0, bool cachedTMP = false)
		{
			this.mtx = Draw.Matrix;
			this.metaMpb = metaMpb;
			if (DrawCommand.IsAddingDrawCommandsToBuffer)
			{
				Draw.style.renderState.shader = sourceMat.shader;
				Draw.style.renderState.keywords = IMDrawer.GetMaterialKeywords(sourceMat);
				if (cachedTMP)
				{
					this.drawState.mesh = Object.Instantiate<Mesh>(sourceMesh);
					this.drawState.mat = Object.Instantiate<Material>(sourceMat);
					IMDrawer.ApplyGlobalPropertiesTMP(this.drawState.mat);
					List<Object> cachedAssets = DrawCommand.CurrentWritingCommandBuffer.cachedAssets;
					cachedAssets.Add(this.drawState.mesh);
					cachedAssets.Add(this.drawState.mat);
				}
				else
				{
					this.drawState.mat = IMMaterialPool.GetMaterial(ref Draw.style.renderState);
					this.drawState.mesh = sourceMesh;
				}
				this.drawState.submesh = submesh;
				if (IMDrawer.metaMpbPrevious != metaMpb && IMDrawer.metaMpbPrevious != null && IMDrawer.metaMpbPrevious.HasContent)
				{
					DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(IMDrawer.metaMpbPrevious.ExtractDrawCall());
				}
				if (!metaMpb.PreAppendCheck(this.drawState, this.mtx))
				{
					DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(metaMpb.ExtractDrawCall());
					if (!metaMpb.PreAppendCheck(this.drawState, this.mtx))
					{
						Debug.LogWarning("MetaMpb somehow not ready to be initialized");
					}
				}
				IMDrawer.metaMpbPrevious = metaMpb;
				return;
			}
			this.drawState.mesh = sourceMesh;
			this.drawState.mat = sourceMat;
			this.drawState.submesh = submesh;
			if (!metaMpb.PreAppendCheck(this.drawState, this.mtx))
			{
				Debug.LogError("Somehow PreAppendCheck failed for this draw");
			}
			this.ApplyGlobalProperties();
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00021868 File Offset: 0x0001FA68
		private void ApplyGlobalProperties()
		{
			if (!DrawCommand.IsAddingDrawCommandsToBuffer)
			{
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propZTest, (float)Draw.ZTest);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propZOffsetFactor, Draw.ZOffsetFactor);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propZOffsetUnits, (float)Draw.ZOffsetUnits);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propStencilComp, (float)Draw.StencilComp);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propStencilOpPass, (float)Draw.StencilOpPass);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propStencilID, (float)Draw.StencilRefID);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propStencilReadMask, (float)Draw.StencilReadMask);
				this.drawState.mat.SetFloat(ShapesMaterialUtils.propStencilWriteMask, (float)Draw.StencilWriteMask);
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00021958 File Offset: 0x0001FB58
		private static void ApplyGlobalPropertiesTMP(Material m)
		{
			m.SetInt(ShapesMaterialUtils.propZTestTMP, (int)Draw.ZTest);
			m.SetInt(ShapesMaterialUtils.propStencilComp, (int)Draw.StencilComp);
			m.SetInt(ShapesMaterialUtils.propStencilOpPass, (int)Draw.StencilOpPass);
			m.SetInt(ShapesMaterialUtils.propStencilIDTMP, (int)Draw.StencilRefID);
			m.SetInt(ShapesMaterialUtils.propStencilReadMask, (int)Draw.StencilReadMask);
			m.SetInt(ShapesMaterialUtils.propStencilWriteMask, (int)Draw.StencilWriteMask);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x000219C8 File Offset: 0x0001FBC8
		public void Dispose()
		{
			if (!DrawCommand.IsAddingDrawCommandsToBuffer)
			{
				this.metaMpb.ApplyDirectlyToMaterial();
				this.drawState.mat.SetPass(0);
				Graphics.DrawMeshNow(this.drawState.mesh, this.mtx, this.drawState.submesh);
				return;
			}
			if (!ShapesConfig.Instance.useImmediateModeInstancing)
			{
				DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(this.metaMpb.ExtractDrawCall());
			}
		}

		// Token: 0x040000C5 RID: 197
		internal static MetaMpb metaMpbPrevious;

		// Token: 0x040000C6 RID: 198
		private static Dictionary<Material, string[]> matKeywords = new Dictionary<Material, string[]>();

		// Token: 0x040000C7 RID: 199
		private MetaMpb metaMpb;

		// Token: 0x040000C8 RID: 200
		private ShapeDrawState drawState;

		// Token: 0x040000C9 RID: 201
		private Matrix4x4 mtx;
	}
}
