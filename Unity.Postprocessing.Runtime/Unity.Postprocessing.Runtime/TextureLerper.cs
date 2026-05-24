using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000067 RID: 103
	internal class TextureLerper
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600021F RID: 543 RVA: 0x000106C8 File Offset: 0x0000E8C8
		internal static TextureLerper instance
		{
			get
			{
				if (TextureLerper.m_Instance == null)
				{
					TextureLerper.m_Instance = new TextureLerper();
				}
				return TextureLerper.m_Instance;
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000106E0 File Offset: 0x0000E8E0
		private TextureLerper()
		{
			this.m_Recycled = new List<RenderTexture>();
			this.m_Actives = new List<RenderTexture>();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000106FE File Offset: 0x0000E8FE
		internal void BeginFrame(PostProcessRenderContext context)
		{
			this.m_Command = context.command;
			this.m_PropertySheets = context.propertySheets;
			this.m_Resources = context.resources;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00010724 File Offset: 0x0000E924
		internal void EndFrame()
		{
			if (this.m_Recycled.Count > 0)
			{
				foreach (RenderTexture renderTexture in this.m_Recycled)
				{
					RuntimeUtilities.Destroy(renderTexture);
				}
				this.m_Recycled.Clear();
			}
			if (this.m_Actives.Count > 0)
			{
				foreach (RenderTexture renderTexture2 in this.m_Actives)
				{
					this.m_Recycled.Add(renderTexture2);
				}
				this.m_Actives.Clear();
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000107F0 File Offset: 0x0000E9F0
		private RenderTexture Get(RenderTextureFormat format, int w, int h, int d = 1, bool enableRandomWrite = false, bool force3D = false)
		{
			RenderTexture renderTexture = null;
			int count = this.m_Recycled.Count;
			int i;
			for (i = 0; i < count; i++)
			{
				RenderTexture renderTexture2 = this.m_Recycled[i];
				if (renderTexture2.width == w && renderTexture2.height == h && renderTexture2.volumeDepth == d && renderTexture2.format == format && renderTexture2.enableRandomWrite == enableRandomWrite && (!force3D || renderTexture2.dimension == TextureDimension.Tex3D))
				{
					renderTexture = renderTexture2;
					break;
				}
			}
			if (renderTexture == null)
			{
				TextureDimension textureDimension = ((d > 1 || force3D) ? TextureDimension.Tex3D : TextureDimension.Tex2D);
				renderTexture = new RenderTexture(w, h, 0, format)
				{
					dimension = textureDimension,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					anisoLevel = 0,
					volumeDepth = d,
					enableRandomWrite = enableRandomWrite
				};
				renderTexture.Create();
			}
			else
			{
				this.m_Recycled.RemoveAt(i);
			}
			this.m_Actives.Add(renderTexture);
			return renderTexture;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000108D8 File Offset: 0x0000EAD8
		internal Texture Lerp(Texture from, Texture to, float t)
		{
			if (from == to)
			{
				return from;
			}
			if (t <= 0f)
			{
				return from;
			}
			if (t >= 1f)
			{
				return to;
			}
			RenderTexture renderTexture;
			if (from is Texture3D || (from is RenderTexture && ((RenderTexture)from).volumeDepth > 1))
			{
				int num = ((from is Texture3D) ? ((Texture3D)from).depth : ((RenderTexture)from).volumeDepth);
				int num2 = Mathf.Max(Mathf.Max(from.width, from.height), num);
				renderTexture = this.Get(RenderTextureFormat.ARGBHalf, from.width, from.height, num, true, true);
				ComputeShader texture3dLerp = this.m_Resources.computeShaders.texture3dLerp;
				int num3 = texture3dLerp.FindKernel("KTexture3DLerp");
				this.m_Command.SetComputeVectorParam(texture3dLerp, "_DimensionsAndLerp", new Vector4((float)from.width, (float)from.height, (float)num, t));
				this.m_Command.SetComputeTextureParam(texture3dLerp, num3, "_Output", renderTexture);
				this.m_Command.SetComputeTextureParam(texture3dLerp, num3, "_From", from);
				this.m_Command.SetComputeTextureParam(texture3dLerp, num3, "_To", to);
				uint num4;
				uint num5;
				uint num6;
				texture3dLerp.GetKernelThreadGroupSizes(num3, out num4, out num5, out num6);
				int num7 = Mathf.CeilToInt((float)num2 / num4);
				int num8 = Mathf.CeilToInt((float)num2 / num6);
				this.m_Command.DispatchCompute(texture3dLerp, num3, num7, num7, num8);
				return renderTexture;
			}
			RenderTextureFormat uncompressedRenderTextureFormat = TextureFormatUtilities.GetUncompressedRenderTextureFormat(to);
			renderTexture = this.Get(uncompressedRenderTextureFormat, to.width, to.height, 1, false, false);
			PropertySheet propertySheet = this.m_PropertySheets.Get(this.m_Resources.shaders.texture2dLerp);
			propertySheet.properties.SetTexture(ShaderIDs.To, to);
			propertySheet.properties.SetFloat(ShaderIDs.Interp, t);
			this.m_Command.BlitFullscreenTriangle(from, renderTexture, propertySheet, 0, false, null, false);
			return renderTexture;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00010AD8 File Offset: 0x0000ECD8
		internal Texture Lerp(Texture from, Color to, float t)
		{
			if ((double)t < 1E-05)
			{
				return from;
			}
			RenderTexture renderTexture;
			if (from is Texture3D || (from is RenderTexture && ((RenderTexture)from).volumeDepth > 1))
			{
				int num = ((from is Texture3D) ? ((Texture3D)from).depth : ((RenderTexture)from).volumeDepth);
				float num2 = (float)Mathf.Max(Mathf.Max(from.width, from.height), num);
				renderTexture = this.Get(RenderTextureFormat.ARGBHalf, from.width, from.height, num, true, true);
				ComputeShader texture3dLerp = this.m_Resources.computeShaders.texture3dLerp;
				int num3 = texture3dLerp.FindKernel("KTexture3DLerpToColor");
				this.m_Command.SetComputeVectorParam(texture3dLerp, "_DimensionsAndLerp", new Vector4((float)from.width, (float)from.height, (float)num, t));
				this.m_Command.SetComputeVectorParam(texture3dLerp, "_TargetColor", new Vector4(to.r, to.g, to.b, to.a));
				this.m_Command.SetComputeTextureParam(texture3dLerp, num3, "_Output", renderTexture);
				this.m_Command.SetComputeTextureParam(texture3dLerp, num3, "_From", from);
				int num4 = Mathf.CeilToInt(num2 / 4f);
				this.m_Command.DispatchCompute(texture3dLerp, num3, num4, num4, num4);
				return renderTexture;
			}
			RenderTextureFormat uncompressedRenderTextureFormat = TextureFormatUtilities.GetUncompressedRenderTextureFormat(from);
			renderTexture = this.Get(uncompressedRenderTextureFormat, from.width, from.height, 1, false, false);
			PropertySheet propertySheet = this.m_PropertySheets.Get(this.m_Resources.shaders.texture2dLerp);
			propertySheet.properties.SetVector(ShaderIDs.TargetColor, new Vector4(to.r, to.g, to.b, to.a));
			propertySheet.properties.SetFloat(ShaderIDs.Interp, t);
			this.m_Command.BlitFullscreenTriangle(from, renderTexture, propertySheet, 1, false, null, false);
			return renderTexture;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00010CDC File Offset: 0x0000EEDC
		internal void Clear()
		{
			foreach (RenderTexture renderTexture in this.m_Actives)
			{
				RuntimeUtilities.Destroy(renderTexture);
			}
			foreach (RenderTexture renderTexture2 in this.m_Recycled)
			{
				RuntimeUtilities.Destroy(renderTexture2);
			}
			this.m_Actives.Clear();
			this.m_Recycled.Clear();
		}

		// Token: 0x04000242 RID: 578
		private static TextureLerper m_Instance;

		// Token: 0x04000243 RID: 579
		private CommandBuffer m_Command;

		// Token: 0x04000244 RID: 580
		private PropertySheetFactory m_PropertySheets;

		// Token: 0x04000245 RID: 581
		private PostProcessResources m_Resources;

		// Token: 0x04000246 RID: 582
		private List<RenderTexture> m_Recycled;

		// Token: 0x04000247 RID: 583
		private List<RenderTexture> m_Actives;
	}
}
