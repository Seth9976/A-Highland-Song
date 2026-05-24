using System;
using System.Collections.Generic;
using UnityEngine.XR;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000057 RID: 87
	public sealed class PostProcessRenderContext
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000D266 File Offset: 0x0000B466
		// (set) Token: 0x06000166 RID: 358 RVA: 0x0000D270 File Offset: 0x0000B470
		public Camera camera
		{
			get
			{
				return this.m_Camera;
			}
			set
			{
				this.m_Camera = value;
				if (this.m_Camera.stereoEnabled)
				{
					RenderTextureDescriptor eyeTextureDesc = XRSettings.eyeTextureDesc;
					this.stereoRenderingMode = PostProcessRenderContext.StereoRenderingMode.SinglePass;
					this.numberOfEyes = 1;
					if (XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass)
					{
						this.stereoRenderingMode = PostProcessRenderContext.StereoRenderingMode.MultiPass;
					}
					if (eyeTextureDesc.dimension == TextureDimension.Tex2DArray)
					{
						this.stereoRenderingMode = PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced;
					}
					if (this.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
					{
						this.numberOfEyes = 2;
					}
					this.width = eyeTextureDesc.width;
					this.height = eyeTextureDesc.height;
					this.m_sourceDescriptor = eyeTextureDesc;
					if (this.m_Camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
					{
						this.xrActiveEye = 1;
					}
					this.screenWidth = XRSettings.eyeTextureWidth;
					this.screenHeight = XRSettings.eyeTextureHeight;
					this.stereoActive = true;
					return;
				}
				this.width = this.m_Camera.pixelWidth;
				this.height = this.m_Camera.pixelHeight;
				this.m_sourceDescriptor.width = this.width;
				this.m_sourceDescriptor.height = this.height;
				this.screenWidth = this.width;
				this.screenHeight = this.height;
				this.stereoActive = false;
				this.numberOfEyes = 1;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000D395 File Offset: 0x0000B595
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000D39D File Offset: 0x0000B59D
		public CommandBuffer command { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000D3A6 File Offset: 0x0000B5A6
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000D3AE File Offset: 0x0000B5AE
		public RenderTargetIdentifier source { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000D3B7 File Offset: 0x0000B5B7
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000D3BF File Offset: 0x0000B5BF
		public RenderTargetIdentifier destination { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		public RenderTextureFormat sourceFormat { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000D3D9 File Offset: 0x0000B5D9
		// (set) Token: 0x06000170 RID: 368 RVA: 0x0000D3E1 File Offset: 0x0000B5E1
		public bool flip { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000D3EA File Offset: 0x0000B5EA
		// (set) Token: 0x06000172 RID: 370 RVA: 0x0000D3F2 File Offset: 0x0000B5F2
		public PostProcessResources resources { get; internal set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000D3FB File Offset: 0x0000B5FB
		// (set) Token: 0x06000174 RID: 372 RVA: 0x0000D403 File Offset: 0x0000B603
		public PropertySheetFactory propertySheets { get; internal set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000D40C File Offset: 0x0000B60C
		// (set) Token: 0x06000176 RID: 374 RVA: 0x0000D414 File Offset: 0x0000B614
		public Dictionary<string, object> userData { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000D41D File Offset: 0x0000B61D
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000D425 File Offset: 0x0000B625
		public PostProcessDebugLayer debugLayer { get; internal set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000D42E File Offset: 0x0000B62E
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000D436 File Offset: 0x0000B636
		public int width { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000D43F File Offset: 0x0000B63F
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000D447 File Offset: 0x0000B647
		public int height { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000D450 File Offset: 0x0000B650
		// (set) Token: 0x0600017E RID: 382 RVA: 0x0000D458 File Offset: 0x0000B658
		public bool stereoActive { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000D461 File Offset: 0x0000B661
		// (set) Token: 0x06000180 RID: 384 RVA: 0x0000D469 File Offset: 0x0000B669
		public int xrActiveEye { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000D472 File Offset: 0x0000B672
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000D47A File Offset: 0x0000B67A
		public int numberOfEyes { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000D483 File Offset: 0x0000B683
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000D48B File Offset: 0x0000B68B
		public PostProcessRenderContext.StereoRenderingMode stereoRenderingMode { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000D494 File Offset: 0x0000B694
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000D49C File Offset: 0x0000B69C
		public int screenWidth { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000D4A5 File Offset: 0x0000B6A5
		// (set) Token: 0x06000188 RID: 392 RVA: 0x0000D4AD File Offset: 0x0000B6AD
		public int screenHeight { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000D4B6 File Offset: 0x0000B6B6
		// (set) Token: 0x0600018A RID: 394 RVA: 0x0000D4BE File Offset: 0x0000B6BE
		public bool isSceneView { get; internal set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000D4C7 File Offset: 0x0000B6C7
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000D4CF File Offset: 0x0000B6CF
		public PostProcessLayer.Antialiasing antialiasing { get; internal set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
		// (set) Token: 0x0600018E RID: 398 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		public TemporalAntialiasing temporalAntialiasing { get; internal set; }

		// Token: 0x0600018F RID: 399 RVA: 0x0000D4EC File Offset: 0x0000B6EC
		public void Reset()
		{
			this.m_Camera = null;
			this.width = 0;
			this.height = 0;
			this.m_sourceDescriptor = new RenderTextureDescriptor(0, 0);
			this.physicalCamera = false;
			this.stereoActive = false;
			this.xrActiveEye = 0;
			this.screenWidth = 0;
			this.screenHeight = 0;
			this.command = null;
			this.source = 0;
			this.destination = 0;
			this.sourceFormat = RenderTextureFormat.ARGB32;
			this.flip = false;
			this.resources = null;
			this.propertySheets = null;
			this.debugLayer = null;
			this.isSceneView = false;
			this.antialiasing = PostProcessLayer.Antialiasing.None;
			this.temporalAntialiasing = null;
			this.uberSheet = null;
			this.autoExposureTexture = null;
			this.logLut = null;
			this.autoExposure = null;
			this.bloomBufferNameID = -1;
			if (this.userData == null)
			{
				this.userData = new Dictionary<string, object>();
			}
			this.userData.Clear();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000D5D6 File Offset: 0x0000B7D6
		public bool IsTemporalAntialiasingActive()
		{
			return this.antialiasing == PostProcessLayer.Antialiasing.TemporalAntialiasing && !this.isSceneView && this.temporalAntialiasing.IsSupported();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000D5F6 File Offset: 0x0000B7F6
		public bool IsDebugOverlayEnabled(DebugOverlay overlay)
		{
			return this.debugLayer.debugOverlay == overlay;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000D606 File Offset: 0x0000B806
		public void PushDebugOverlay(CommandBuffer cmd, RenderTargetIdentifier source, PropertySheet sheet, int pass)
		{
			this.debugLayer.PushDebugOverlay(cmd, source, sheet, pass);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000D618 File Offset: 0x0000B818
		internal RenderTextureDescriptor GetDescriptor(int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default)
		{
			RenderTextureDescriptor renderTextureDescriptor = new RenderTextureDescriptor(this.m_sourceDescriptor.width, this.m_sourceDescriptor.height, this.m_sourceDescriptor.colorFormat, depthBufferBits);
			renderTextureDescriptor.dimension = this.m_sourceDescriptor.dimension;
			renderTextureDescriptor.volumeDepth = this.m_sourceDescriptor.volumeDepth;
			renderTextureDescriptor.vrUsage = this.m_sourceDescriptor.vrUsage;
			renderTextureDescriptor.msaaSamples = this.m_sourceDescriptor.msaaSamples;
			renderTextureDescriptor.memoryless = this.m_sourceDescriptor.memoryless;
			renderTextureDescriptor.useMipMap = this.m_sourceDescriptor.useMipMap;
			renderTextureDescriptor.autoGenerateMips = this.m_sourceDescriptor.autoGenerateMips;
			renderTextureDescriptor.enableRandomWrite = this.m_sourceDescriptor.enableRandomWrite;
			renderTextureDescriptor.shadowSamplingMode = this.m_sourceDescriptor.shadowSamplingMode;
			if (this.m_Camera.allowDynamicResolution)
			{
				renderTextureDescriptor.useDynamicScale = true;
			}
			if (colorFormat != RenderTextureFormat.Default)
			{
				renderTextureDescriptor.colorFormat = colorFormat;
			}
			if (readWrite == RenderTextureReadWrite.sRGB)
			{
				renderTextureDescriptor.sRGB = true;
			}
			else if (readWrite == RenderTextureReadWrite.Linear)
			{
				renderTextureDescriptor.sRGB = false;
			}
			else if (readWrite == RenderTextureReadWrite.Default)
			{
				renderTextureDescriptor.sRGB = QualitySettings.activeColorSpace > ColorSpace.Gamma;
			}
			return renderTextureDescriptor;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000D740 File Offset: 0x0000B940
		public void GetScreenSpaceTemporaryRT(CommandBuffer cmd, int nameID, int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, FilterMode filter = FilterMode.Bilinear, int widthOverride = 0, int heightOverride = 0)
		{
			RenderTextureDescriptor descriptor = this.GetDescriptor(depthBufferBits, colorFormat, readWrite);
			if (widthOverride > 0)
			{
				descriptor.width = widthOverride;
			}
			if (heightOverride > 0)
			{
				descriptor.height = heightOverride;
			}
			if (this.stereoActive && descriptor.dimension == TextureDimension.Tex2DArray)
			{
				descriptor.dimension = TextureDimension.Tex2D;
			}
			cmd.GetTemporaryRT(nameID, descriptor, filter);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000D79C File Offset: 0x0000B99C
		public RenderTexture GetScreenSpaceTemporaryRT(int depthBufferBits = 0, RenderTextureFormat colorFormat = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int widthOverride = 0, int heightOverride = 0)
		{
			RenderTextureDescriptor descriptor = this.GetDescriptor(depthBufferBits, colorFormat, readWrite);
			if (widthOverride > 0)
			{
				descriptor.width = widthOverride;
			}
			if (heightOverride > 0)
			{
				descriptor.height = heightOverride;
			}
			return RenderTexture.GetTemporary(descriptor);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
		public void UpdateSinglePassStereoState(bool isTAAEnabled, bool isAOEnabled, bool isSSREnabled)
		{
			RenderTextureDescriptor eyeTextureDesc = XRSettings.eyeTextureDesc;
			this.screenWidth = XRSettings.eyeTextureWidth;
			if (this.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
			{
				if (isTAAEnabled || isAOEnabled || isSSREnabled)
				{
					this.numberOfEyes = 1;
				}
				else
				{
					this.numberOfEyes = 2;
					eyeTextureDesc.width /= 2;
					eyeTextureDesc.vrUsage = VRTextureUsage.None;
					this.screenWidth /= 2;
				}
				this.width = eyeTextureDesc.width;
				this.height = eyeTextureDesc.height;
				this.m_sourceDescriptor = eyeTextureDesc;
			}
		}

		// Token: 0x04000162 RID: 354
		private Camera m_Camera;

		// Token: 0x04000177 RID: 375
		internal PropertySheet uberSheet;

		// Token: 0x04000178 RID: 376
		internal Texture autoExposureTexture;

		// Token: 0x04000179 RID: 377
		internal LogHistogram logHistogram;

		// Token: 0x0400017A RID: 378
		internal Texture logLut;

		// Token: 0x0400017B RID: 379
		internal AutoExposure autoExposure;

		// Token: 0x0400017C RID: 380
		internal int bloomBufferNameID;

		// Token: 0x0400017D RID: 381
		internal bool physicalCamera;

		// Token: 0x0400017E RID: 382
		private RenderTextureDescriptor m_sourceDescriptor;

		// Token: 0x02000083 RID: 131
		public enum StereoRenderingMode
		{
			// Token: 0x040002B9 RID: 697
			MultiPass,
			// Token: 0x040002BA RID: 698
			SinglePass,
			// Token: 0x040002BB RID: 699
			SinglePassInstanced,
			// Token: 0x040002BC RID: 700
			SinglePassMultiview
		}
	}
}
