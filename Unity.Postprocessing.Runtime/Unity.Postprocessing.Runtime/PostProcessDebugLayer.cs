using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public sealed class PostProcessDebugLayer
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00009E53 File Offset: 0x00008053
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00009E5B File Offset: 0x0000805B
		public RenderTexture debugOverlayTarget { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00009E64 File Offset: 0x00008064
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00009E6C File Offset: 0x0000806C
		public bool debugOverlayActive { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00009E75 File Offset: 0x00008075
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00009E7D File Offset: 0x0000807D
		public DebugOverlay debugOverlay { get; private set; }

		// Token: 0x060000FC RID: 252 RVA: 0x00009E88 File Offset: 0x00008088
		internal void OnEnable()
		{
			RuntimeUtilities.CreateIfNull<LightMeterMonitor>(ref this.lightMeter);
			RuntimeUtilities.CreateIfNull<HistogramMonitor>(ref this.histogram);
			RuntimeUtilities.CreateIfNull<WaveformMonitor>(ref this.waveform);
			RuntimeUtilities.CreateIfNull<VectorscopeMonitor>(ref this.vectorscope);
			RuntimeUtilities.CreateIfNull<PostProcessDebugLayer.OverlaySettings>(ref this.overlaySettings);
			this.m_Monitors = new Dictionary<MonitorType, Monitor>
			{
				{
					MonitorType.LightMeter,
					this.lightMeter
				},
				{
					MonitorType.Histogram,
					this.histogram
				},
				{
					MonitorType.Waveform,
					this.waveform
				},
				{
					MonitorType.Vectorscope,
					this.vectorscope
				}
			};
			foreach (KeyValuePair<MonitorType, Monitor> keyValuePair in this.m_Monitors)
			{
				keyValuePair.Value.OnEnable();
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00009F58 File Offset: 0x00008158
		internal void OnDisable()
		{
			foreach (KeyValuePair<MonitorType, Monitor> keyValuePair in this.m_Monitors)
			{
				keyValuePair.Value.OnDisable();
			}
			this.DestroyDebugOverlayTarget();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00009FB8 File Offset: 0x000081B8
		private void DestroyDebugOverlayTarget()
		{
			RuntimeUtilities.Destroy(this.debugOverlayTarget);
			this.debugOverlayTarget = null;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00009FCC File Offset: 0x000081CC
		public void RequestMonitorPass(MonitorType monitor)
		{
			this.m_Monitors[monitor].requested = true;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009FE0 File Offset: 0x000081E0
		public void RequestDebugOverlay(DebugOverlay mode)
		{
			this.debugOverlay = mode;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009FE9 File Offset: 0x000081E9
		internal void SetFrameSize(int width, int height)
		{
			this.frameWidth = width;
			this.frameHeight = height;
			this.debugOverlayActive = false;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000A000 File Offset: 0x00008200
		public void PushDebugOverlay(CommandBuffer cmd, RenderTargetIdentifier source, PropertySheet sheet, int pass)
		{
			if (this.debugOverlayTarget == null || !this.debugOverlayTarget.IsCreated() || this.debugOverlayTarget.width != this.frameWidth || this.debugOverlayTarget.height != this.frameHeight)
			{
				RuntimeUtilities.Destroy(this.debugOverlayTarget);
				this.debugOverlayTarget = new RenderTexture(this.frameWidth, this.frameHeight, 0, RenderTextureFormat.ARGB32)
				{
					name = "Debug Overlay Target",
					anisoLevel = 1,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					hideFlags = HideFlags.HideAndDontSave
				};
				this.debugOverlayTarget.Create();
			}
			cmd.BlitFullscreenTriangle(source, this.debugOverlayTarget, sheet, pass, false, null, false);
			this.debugOverlayActive = true;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000A0CD File Offset: 0x000082CD
		internal DepthTextureMode GetCameraFlags()
		{
			if (this.debugOverlay == DebugOverlay.Depth)
			{
				return DepthTextureMode.Depth;
			}
			if (this.debugOverlay == DebugOverlay.Normals)
			{
				return DepthTextureMode.DepthNormals;
			}
			if (this.debugOverlay == DebugOverlay.MotionVectors)
			{
				return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
			}
			return DepthTextureMode.None;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000A0F4 File Offset: 0x000082F4
		internal void RenderMonitors(PostProcessRenderContext context)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (KeyValuePair<MonitorType, Monitor> keyValuePair in this.m_Monitors)
			{
				bool flag3 = keyValuePair.Value.IsRequestedAndSupported(context);
				flag = flag || flag3;
				flag2 |= flag3 && keyValuePair.Value.NeedsHalfRes();
			}
			if (!flag)
			{
				return;
			}
			CommandBuffer command = context.command;
			command.BeginSample("Monitors");
			if (flag2)
			{
				command.GetTemporaryRT(ShaderIDs.HalfResFinalCopy, context.width / 2, context.height / 2, 0, FilterMode.Bilinear, context.sourceFormat);
				command.Blit(context.destination, ShaderIDs.HalfResFinalCopy);
			}
			foreach (KeyValuePair<MonitorType, Monitor> keyValuePair2 in this.m_Monitors)
			{
				Monitor value = keyValuePair2.Value;
				if (value.requested)
				{
					value.Render(context);
				}
			}
			if (flag2)
			{
				command.ReleaseTemporaryRT(ShaderIDs.HalfResFinalCopy);
			}
			command.EndSample("Monitors");
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000A230 File Offset: 0x00008430
		internal void RenderSpecialOverlays(PostProcessRenderContext context)
		{
			if (this.debugOverlay == DebugOverlay.Depth)
			{
				PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.debugOverlays);
				propertySheet.properties.SetVector(ShaderIDs.Params, new Vector4(this.overlaySettings.linearDepth ? 1f : 0f, 0f, 0f, 0f));
				this.PushDebugOverlay(context.command, BuiltinRenderTextureType.None, propertySheet, 0);
				return;
			}
			if (this.debugOverlay == DebugOverlay.Normals)
			{
				PropertySheet propertySheet2 = context.propertySheets.Get(context.resources.shaders.debugOverlays);
				propertySheet2.ClearKeywords();
				if (context.camera.actualRenderingPath == RenderingPath.DeferredLighting)
				{
					propertySheet2.EnableKeyword("SOURCE_GBUFFER");
				}
				this.PushDebugOverlay(context.command, BuiltinRenderTextureType.None, propertySheet2, 1);
				return;
			}
			if (this.debugOverlay == DebugOverlay.MotionVectors)
			{
				PropertySheet propertySheet3 = context.propertySheets.Get(context.resources.shaders.debugOverlays);
				propertySheet3.properties.SetVector(ShaderIDs.Params, new Vector4(this.overlaySettings.motionColorIntensity, (float)this.overlaySettings.motionGridSize, 0f, 0f));
				this.PushDebugOverlay(context.command, context.source, propertySheet3, 2);
				return;
			}
			if (this.debugOverlay == DebugOverlay.NANTracker)
			{
				PropertySheet propertySheet4 = context.propertySheets.Get(context.resources.shaders.debugOverlays);
				this.PushDebugOverlay(context.command, context.source, propertySheet4, 3);
				return;
			}
			if (this.debugOverlay == DebugOverlay.ColorBlindnessSimulation)
			{
				PropertySheet propertySheet5 = context.propertySheets.Get(context.resources.shaders.debugOverlays);
				propertySheet5.properties.SetVector(ShaderIDs.Params, new Vector4(this.overlaySettings.colorBlindnessStrength, 0f, 0f, 0f));
				this.PushDebugOverlay(context.command, context.source, propertySheet5, (int)(4 + this.overlaySettings.colorBlindnessType));
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000A430 File Offset: 0x00008630
		internal void EndFrame()
		{
			foreach (KeyValuePair<MonitorType, Monitor> keyValuePair in this.m_Monitors)
			{
				keyValuePair.Value.requested = false;
			}
			if (!this.debugOverlayActive)
			{
				this.DestroyDebugOverlayTarget();
			}
			this.debugOverlay = DebugOverlay.None;
		}

		// Token: 0x0400011F RID: 287
		public LightMeterMonitor lightMeter;

		// Token: 0x04000120 RID: 288
		public HistogramMonitor histogram;

		// Token: 0x04000121 RID: 289
		public WaveformMonitor waveform;

		// Token: 0x04000122 RID: 290
		public VectorscopeMonitor vectorscope;

		// Token: 0x04000123 RID: 291
		private Dictionary<MonitorType, Monitor> m_Monitors;

		// Token: 0x04000124 RID: 292
		private int frameWidth;

		// Token: 0x04000125 RID: 293
		private int frameHeight;

		// Token: 0x04000129 RID: 297
		public PostProcessDebugLayer.OverlaySettings overlaySettings;

		// Token: 0x02000078 RID: 120
		[Serializable]
		public class OverlaySettings
		{
			// Token: 0x0400029E RID: 670
			public bool linearDepth;

			// Token: 0x0400029F RID: 671
			[Range(0f, 16f)]
			public float motionColorIntensity = 4f;

			// Token: 0x040002A0 RID: 672
			[Range(4f, 128f)]
			public int motionGridSize = 64;

			// Token: 0x040002A1 RID: 673
			public ColorBlindnessType colorBlindnessType;

			// Token: 0x040002A2 RID: 674
			[Range(0f, 1f)]
			public float colorBlindnessStrength = 1f;
		}
	}
}
