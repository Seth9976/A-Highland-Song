using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using UnityEngine.XR;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000054 RID: 84
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[ImageEffectAllowedInSceneView]
	[AddComponentMenu("Rendering/Post-process Layer", 1000)]
	[RequireComponent(typeof(Camera))]
	public sealed class PostProcessLayer : MonoBehaviour
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000A70B File Offset: 0x0000890B
		// (set) Token: 0x0600011D RID: 285 RVA: 0x0000A713 File Offset: 0x00008913
		public Dictionary<PostProcessEvent, List<PostProcessLayer.SerializedBundleRef>> sortedBundles { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000A71C File Offset: 0x0000891C
		// (set) Token: 0x0600011F RID: 287 RVA: 0x0000A724 File Offset: 0x00008924
		public DepthTextureMode cameraDepthFlags { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000A72D File Offset: 0x0000892D
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000A735 File Offset: 0x00008935
		public bool haveBundlesBeenInited { get; private set; }

		// Token: 0x06000122 RID: 290 RVA: 0x0000A740 File Offset: 0x00008940
		private void OnEnable()
		{
			this.Init(null);
			if (!this.haveBundlesBeenInited)
			{
				this.InitBundles();
			}
			this.m_LogHistogram = new LogHistogram();
			this.m_PropertySheetFactory = new PropertySheetFactory();
			this.m_TargetPool = new TargetPool();
			this.debugLayer.OnEnable();
			if (RuntimeUtilities.scriptableRenderPipelineActive)
			{
				return;
			}
			this.InitLegacy();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000A79C File Offset: 0x0000899C
		private void InitLegacy()
		{
			this.m_LegacyCmdBufferBeforeReflections = new CommandBuffer
			{
				name = "Deferred Ambient Occlusion"
			};
			this.m_LegacyCmdBufferBeforeLighting = new CommandBuffer
			{
				name = "Deferred Ambient Occlusion"
			};
			this.m_LegacyCmdBufferOpaque = new CommandBuffer
			{
				name = "Opaque Only Post-processing"
			};
			this.m_LegacyCmdBuffer = new CommandBuffer
			{
				name = "Post-processing"
			};
			this.m_Camera = base.GetComponent<Camera>();
			this.m_Camera.AddCommandBuffer(CameraEvent.BeforeReflections, this.m_LegacyCmdBufferBeforeReflections);
			this.m_Camera.AddCommandBuffer(CameraEvent.BeforeLighting, this.m_LegacyCmdBufferBeforeLighting);
			this.m_Camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m_LegacyCmdBufferOpaque);
			this.m_Camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, this.m_LegacyCmdBuffer);
			this.m_CurrentContext = new PostProcessRenderContext();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000A863 File Offset: 0x00008A63
		private bool DynamicResolutionAllowsFinalBlitToCameraTarget()
		{
			return !this.m_Camera.allowDynamicResolution || ((double)ScalableBufferManager.heightScaleFactor == 1.0 && (double)ScalableBufferManager.widthScaleFactor == 1.0);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000A898 File Offset: 0x00008A98
		[ImageEffectUsesCommandBuffer]
		private void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			if (this.finalBlitToCameraTarget && !this.m_CurrentContext.stereoActive && this.DynamicResolutionAllowsFinalBlitToCameraTarget())
			{
				RenderTexture.active = dst;
				return;
			}
			Graphics.Blit(src, dst);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		public void Init(PostProcessResources resources)
		{
			if (resources != null)
			{
				this.m_Resources = resources;
			}
			RuntimeUtilities.CreateIfNull<TemporalAntialiasing>(ref this.temporalAntialiasing);
			RuntimeUtilities.CreateIfNull<SubpixelMorphologicalAntialiasing>(ref this.subpixelMorphologicalAntialiasing);
			RuntimeUtilities.CreateIfNull<FastApproximateAntialiasing>(ref this.fastApproximateAntialiasing);
			RuntimeUtilities.CreateIfNull<Dithering>(ref this.dithering);
			RuntimeUtilities.CreateIfNull<Fog>(ref this.fog);
			RuntimeUtilities.CreateIfNull<PostProcessDebugLayer>(ref this.debugLayer);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000A928 File Offset: 0x00008B28
		public void InitBundles()
		{
			if (this.haveBundlesBeenInited)
			{
				return;
			}
			RuntimeUtilities.CreateIfNull<List<PostProcessLayer.SerializedBundleRef>>(ref this.m_BeforeTransparentBundles);
			RuntimeUtilities.CreateIfNull<List<PostProcessLayer.SerializedBundleRef>>(ref this.m_BeforeStackBundles);
			RuntimeUtilities.CreateIfNull<List<PostProcessLayer.SerializedBundleRef>>(ref this.m_AfterStackBundles);
			this.m_Bundles = new Dictionary<Type, PostProcessBundle>();
			foreach (Type type in PostProcessManager.instance.settingsTypes.Keys)
			{
				PostProcessBundle postProcessBundle = new PostProcessBundle((PostProcessEffectSettings)ScriptableObject.CreateInstance(type));
				this.m_Bundles.Add(type, postProcessBundle);
			}
			this.UpdateBundleSortList(this.m_BeforeTransparentBundles, PostProcessEvent.BeforeTransparent);
			this.UpdateBundleSortList(this.m_BeforeStackBundles, PostProcessEvent.BeforeStack);
			this.UpdateBundleSortList(this.m_AfterStackBundles, PostProcessEvent.AfterStack);
			this.sortedBundles = new Dictionary<PostProcessEvent, List<PostProcessLayer.SerializedBundleRef>>(default(PostProcessEventComparer))
			{
				{
					PostProcessEvent.BeforeTransparent,
					this.m_BeforeTransparentBundles
				},
				{
					PostProcessEvent.BeforeStack,
					this.m_BeforeStackBundles
				},
				{
					PostProcessEvent.AfterStack,
					this.m_AfterStackBundles
				}
			};
			this.haveBundlesBeenInited = true;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000AA40 File Offset: 0x00008C40
		private void UpdateBundleSortList(List<PostProcessLayer.SerializedBundleRef> sortedList, PostProcessEvent evt)
		{
			List<PostProcessBundle> effects = (from kvp in this.m_Bundles
				where kvp.Value.attribute.eventType == evt && !kvp.Value.attribute.builtinEffect
				select kvp.Value).ToList<PostProcessBundle>();
			sortedList.RemoveAll(delegate(PostProcessLayer.SerializedBundleRef x)
			{
				string searchStr = x.assemblyQualifiedName;
				return !effects.Exists((PostProcessBundle b) => b.settings.GetType().AssemblyQualifiedName == searchStr);
			});
			foreach (PostProcessBundle postProcessBundle in effects)
			{
				string typeName2 = postProcessBundle.settings.GetType().AssemblyQualifiedName;
				if (!sortedList.Exists((PostProcessLayer.SerializedBundleRef b) => b.assemblyQualifiedName == typeName2))
				{
					PostProcessLayer.SerializedBundleRef serializedBundleRef = new PostProcessLayer.SerializedBundleRef
					{
						assemblyQualifiedName = typeName2
					};
					sortedList.Add(serializedBundleRef);
				}
			}
			foreach (PostProcessLayer.SerializedBundleRef serializedBundleRef2 in sortedList)
			{
				string typeName = serializedBundleRef2.assemblyQualifiedName;
				PostProcessBundle postProcessBundle2 = effects.Find((PostProcessBundle b) => b.settings.GetType().AssemblyQualifiedName == typeName);
				serializedBundleRef2.bundle = postProcessBundle2;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000ABB0 File Offset: 0x00008DB0
		private void OnDisable()
		{
			if (this.m_Camera != null)
			{
				if (this.m_LegacyCmdBufferBeforeReflections != null)
				{
					this.m_Camera.RemoveCommandBuffer(CameraEvent.BeforeReflections, this.m_LegacyCmdBufferBeforeReflections);
				}
				if (this.m_LegacyCmdBufferBeforeLighting != null)
				{
					this.m_Camera.RemoveCommandBuffer(CameraEvent.BeforeLighting, this.m_LegacyCmdBufferBeforeLighting);
				}
				if (this.m_LegacyCmdBufferOpaque != null)
				{
					this.m_Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m_LegacyCmdBufferOpaque);
				}
				if (this.m_LegacyCmdBuffer != null)
				{
					this.m_Camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, this.m_LegacyCmdBuffer);
				}
			}
			this.temporalAntialiasing.Release();
			this.m_LogHistogram.Release();
			foreach (PostProcessBundle postProcessBundle in this.m_Bundles.Values)
			{
				postProcessBundle.Release();
			}
			this.m_Bundles.Clear();
			this.m_PropertySheetFactory.Release();
			if (this.debugLayer != null)
			{
				this.debugLayer.OnDisable();
			}
			TextureLerper.instance.Clear();
			this.haveBundlesBeenInited = false;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		private void Reset()
		{
			this.volumeTrigger = base.transform;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		private void OnPreCull()
		{
			if (RuntimeUtilities.scriptableRenderPipelineActive)
			{
				return;
			}
			if (this.m_Camera == null || this.m_CurrentContext == null)
			{
				this.InitLegacy();
			}
			if (SystemInfo.usesLoadStoreActions)
			{
				Rect rect = this.m_Camera.rect;
				if (Mathf.Abs(rect.x) > 1E-06f || Mathf.Abs(rect.y) > 1E-06f || Mathf.Abs(1f - rect.width) > 1E-06f || Mathf.Abs(1f - rect.height) > 1E-06f)
				{
					Debug.LogWarning("When used with builtin render pipeline, Postprocessing package expects to be used on a fullscreen Camera.\nPlease note that using Camera viewport may result in visual artefacts or some things not working.", this.m_Camera);
				}
			}
			if (this.m_CurrentContext.IsTemporalAntialiasingActive() && !this.m_Camera.usePhysicalProperties)
			{
				this.m_Camera.ResetProjectionMatrix();
				this.m_Camera.nonJitteredProjectionMatrix = this.m_Camera.projectionMatrix;
				if (this.m_Camera.stereoEnabled)
				{
					this.m_Camera.ResetStereoProjectionMatrices();
					if (this.m_Camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
					{
						this.m_Camera.CopyStereoDeviceProjectionMatrixToNonJittered(Camera.StereoscopicEye.Right);
						this.m_Camera.projectionMatrix = this.m_Camera.GetStereoNonJitteredProjectionMatrix(Camera.StereoscopicEye.Right);
						this.m_Camera.nonJitteredProjectionMatrix = this.m_Camera.projectionMatrix;
						this.m_Camera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, this.m_Camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right));
					}
					else if (this.m_Camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Left || this.m_Camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono)
					{
						this.m_Camera.CopyStereoDeviceProjectionMatrixToNonJittered(Camera.StereoscopicEye.Left);
						this.m_Camera.projectionMatrix = this.m_Camera.GetStereoNonJitteredProjectionMatrix(Camera.StereoscopicEye.Left);
						this.m_Camera.nonJitteredProjectionMatrix = this.m_Camera.projectionMatrix;
						this.m_Camera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, this.m_Camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left));
					}
				}
			}
			if (this.m_Camera.stereoEnabled)
			{
				Shader.SetGlobalFloat(ShaderIDs.RenderViewportScaleFactor, XRSettings.renderViewportScale);
			}
			else
			{
				Shader.SetGlobalFloat(ShaderIDs.RenderViewportScaleFactor, 1f);
			}
			this.BuildCommandBuffers();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000AEE8 File Offset: 0x000090E8
		private void OnPreRender()
		{
			if (RuntimeUtilities.scriptableRenderPipelineActive || this.m_Camera.stereoActiveEye != Camera.MonoOrStereoscopicEye.Right)
			{
				return;
			}
			this.BuildCommandBuffers();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000AF06 File Offset: 0x00009106
		private static bool RequiresInitialBlit(Camera camera, PostProcessRenderContext context)
		{
			return true;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000AF0C File Offset: 0x0000910C
		private void UpdateSrcDstForOpaqueOnly(ref int src, ref int dst, PostProcessRenderContext context, RenderTargetIdentifier cameraTarget, int opaqueOnlyEffectsRemaining)
		{
			if (src > -1)
			{
				context.command.ReleaseTemporaryRT(src);
			}
			context.source = context.destination;
			src = dst;
			if (opaqueOnlyEffectsRemaining == 1)
			{
				context.destination = cameraTarget;
				return;
			}
			dst = this.m_TargetPool.Get();
			context.destination = dst;
			context.GetScreenSpaceTemporaryRT(context.command, dst, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000AF7C File Offset: 0x0000917C
		private void BuildCommandBuffers()
		{
			PostProcessRenderContext currentContext = this.m_CurrentContext;
			RenderTextureFormat renderTextureFormat = (this.m_Camera.targetTexture ? this.m_Camera.targetTexture.format : (this.m_Camera.allowHDR ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.Default));
			if (!RuntimeUtilities.isFloatingPointFormat(renderTextureFormat))
			{
				this.m_NaNKilled = true;
			}
			currentContext.Reset();
			currentContext.camera = this.m_Camera;
			currentContext.sourceFormat = renderTextureFormat;
			this.m_LegacyCmdBufferBeforeReflections.Clear();
			this.m_LegacyCmdBufferBeforeLighting.Clear();
			this.m_LegacyCmdBufferOpaque.Clear();
			this.m_LegacyCmdBuffer.Clear();
			this.SetupContext(currentContext);
			currentContext.command = this.m_LegacyCmdBufferOpaque;
			TextureLerper.instance.BeginFrame(currentContext);
			this.UpdateVolumeSystem(currentContext.camera, currentContext.command);
			PostProcessBundle bundle = this.GetBundle<AmbientOcclusion>();
			AmbientOcclusion ambientOcclusion = bundle.CastSettings<AmbientOcclusion>();
			AmbientOcclusionRenderer ambientOcclusionRenderer = bundle.CastRenderer<AmbientOcclusionRenderer>();
			bool flag = ambientOcclusion.IsEnabledAndSupported(currentContext);
			bool flag2 = ambientOcclusionRenderer.IsAmbientOnly(currentContext);
			bool flag3 = flag && flag2;
			bool flag4 = flag && !flag2;
			PostProcessBundle bundle2 = this.GetBundle<ScreenSpaceReflections>();
			PostProcessEffectSettings settings = bundle2.settings;
			PostProcessEffectRenderer renderer = bundle2.renderer;
			bool flag5 = settings.IsEnabledAndSupported(currentContext);
			if (currentContext.stereoActive)
			{
				currentContext.UpdateSinglePassStereoState(currentContext.IsTemporalAntialiasingActive(), flag, flag5);
			}
			if (flag3)
			{
				IAmbientOcclusionMethod ambientOcclusionMethod = ambientOcclusionRenderer.Get();
				currentContext.command = this.m_LegacyCmdBufferBeforeReflections;
				ambientOcclusionMethod.RenderAmbientOnly(currentContext);
				currentContext.command = this.m_LegacyCmdBufferBeforeLighting;
				ambientOcclusionMethod.CompositeAmbientOnly(currentContext);
			}
			else if (flag4)
			{
				currentContext.command = this.m_LegacyCmdBufferOpaque;
				ambientOcclusionRenderer.Get().RenderAfterOpaque(currentContext);
			}
			bool flag6 = this.fog.IsEnabledAndSupported(currentContext);
			bool flag7 = this.HasOpaqueOnlyEffects(currentContext);
			int num = 0;
			num += (flag5 ? 1 : 0);
			num += (flag6 ? 1 : 0);
			num += (flag7 ? 1 : 0);
			RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);
			if (num > 0)
			{
				CommandBuffer legacyCmdBufferOpaque = this.m_LegacyCmdBufferOpaque;
				currentContext.command = legacyCmdBufferOpaque;
				currentContext.source = renderTargetIdentifier;
				currentContext.destination = renderTargetIdentifier;
				int num2 = -1;
				int num3 = -1;
				this.UpdateSrcDstForOpaqueOnly(ref num2, ref num3, currentContext, renderTargetIdentifier, num + 1);
				if (PostProcessLayer.RequiresInitialBlit(this.m_Camera, currentContext) || num == 1)
				{
					legacyCmdBufferOpaque.BuiltinBlit(currentContext.source, currentContext.destination, RuntimeUtilities.copyStdMaterial, this.stopNaNPropagation ? 1 : 0);
					this.UpdateSrcDstForOpaqueOnly(ref num2, ref num3, currentContext, renderTargetIdentifier, num);
				}
				if (flag5)
				{
					renderer.RenderOrLog(currentContext);
					num--;
					this.UpdateSrcDstForOpaqueOnly(ref num2, ref num3, currentContext, renderTargetIdentifier, num);
				}
				if (flag6)
				{
					this.fog.Render(currentContext);
					num--;
					this.UpdateSrcDstForOpaqueOnly(ref num2, ref num3, currentContext, renderTargetIdentifier, num);
				}
				if (flag7)
				{
					this.RenderOpaqueOnly(currentContext);
				}
				legacyCmdBufferOpaque.ReleaseTemporaryRT(num2);
			}
			int num4 = -1;
			bool flag8 = !this.m_NaNKilled && this.stopNaNPropagation && RuntimeUtilities.isFloatingPointFormat(renderTextureFormat);
			if ((!currentContext.stereoActive || currentContext.numberOfEyes <= 1 || currentContext.stereoRenderingMode != PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced) && (PostProcessLayer.RequiresInitialBlit(this.m_Camera, currentContext) || flag8))
			{
				int num5 = currentContext.width;
				RenderTextureDescriptor eyeTextureDesc = XRSettings.eyeTextureDesc;
				if (currentContext.stereoActive && currentContext.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
				{
					num5 = eyeTextureDesc.width;
				}
				num4 = this.m_TargetPool.Get();
				currentContext.GetScreenSpaceTemporaryRT(this.m_LegacyCmdBuffer, num4, 0, renderTextureFormat, RenderTextureReadWrite.sRGB, FilterMode.Bilinear, num5, 0);
				this.m_LegacyCmdBuffer.BuiltinBlit(renderTargetIdentifier, num4, RuntimeUtilities.copyStdMaterial, this.stopNaNPropagation ? 1 : 0);
				if (!this.m_NaNKilled)
				{
					this.m_NaNKilled = this.stopNaNPropagation;
				}
				currentContext.source = num4;
			}
			else
			{
				currentContext.source = renderTargetIdentifier;
			}
			currentContext.destination = renderTargetIdentifier;
			if (this.finalBlitToCameraTarget && !this.m_CurrentContext.stereoActive && !RuntimeUtilities.scriptableRenderPipelineActive && this.DynamicResolutionAllowsFinalBlitToCameraTarget())
			{
				if (this.m_Camera.targetTexture)
				{
					currentContext.destination = this.m_Camera.targetTexture.colorBuffer;
				}
				else
				{
					currentContext.flip = true;
					currentContext.destination = Display.main.colorBuffer;
				}
			}
			currentContext.command = this.m_LegacyCmdBuffer;
			this.Render(currentContext);
			if (num4 > -1)
			{
				this.m_LegacyCmdBuffer.ReleaseTemporaryRT(num4);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000B3BC File Offset: 0x000095BC
		private void OnPostRender()
		{
			if (RuntimeUtilities.scriptableRenderPipelineActive)
			{
				return;
			}
			if (this.m_CurrentContext.IsTemporalAntialiasingActive())
			{
				if (this.m_CurrentContext.physicalCamera)
				{
					this.m_Camera.usePhysicalProperties = true;
					return;
				}
				this.m_Camera.ResetProjectionMatrix();
				if (this.m_CurrentContext.stereoActive && (RuntimeUtilities.isSinglePassStereoEnabled || this.m_Camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right))
				{
					this.m_Camera.ResetStereoProjectionMatrices();
					if (XRSettings.stereoRenderingMode == XRSettings.StereoRenderingMode.MultiPass)
					{
						this.m_Camera.projectionMatrix = this.m_Camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
					}
				}
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000B44E File Offset: 0x0000964E
		public PostProcessBundle GetBundle<T>() where T : PostProcessEffectSettings
		{
			return this.GetBundle(typeof(T));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000B460 File Offset: 0x00009660
		public PostProcessBundle GetBundle(Type settingsType)
		{
			return this.m_Bundles[settingsType];
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000B46E File Offset: 0x0000966E
		public T GetSettings<T>() where T : PostProcessEffectSettings
		{
			return this.GetBundle<T>().CastSettings<T>();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000B47B File Offset: 0x0000967B
		public void BakeMSVOMap(CommandBuffer cmd, Camera camera, RenderTargetIdentifier destination, RenderTargetIdentifier? depthMap, bool invert, bool isMSAA = false)
		{
			MultiScaleVO multiScaleVO = this.GetBundle<AmbientOcclusion>().CastRenderer<AmbientOcclusionRenderer>().GetMultiScaleVO();
			multiScaleVO.SetResources(this.m_Resources);
			multiScaleVO.GenerateAOMap(cmd, camera, destination, depthMap, invert, isMSAA);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000B4A8 File Offset: 0x000096A8
		internal void OverrideSettings(List<PostProcessEffectSettings> baseSettings, float interpFactor)
		{
			foreach (PostProcessEffectSettings postProcessEffectSettings in baseSettings)
			{
				if (postProcessEffectSettings.active)
				{
					PostProcessEffectSettings settings = this.GetBundle(postProcessEffectSettings.GetType()).settings;
					int count = postProcessEffectSettings.parameters.Count;
					for (int i = 0; i < count; i++)
					{
						ParameterOverride parameterOverride = postProcessEffectSettings.parameters[i];
						if (parameterOverride.overrideState)
						{
							ParameterOverride parameterOverride2 = settings.parameters[i];
							parameterOverride2.Interp(parameterOverride2, parameterOverride, interpFactor);
						}
					}
				}
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000B554 File Offset: 0x00009754
		private void SetLegacyCameraFlags(PostProcessRenderContext context)
		{
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			foreach (KeyValuePair<Type, PostProcessBundle> keyValuePair in this.m_Bundles)
			{
				if (keyValuePair.Value.settings.IsEnabledAndSupported(context))
				{
					depthTextureMode |= keyValuePair.Value.renderer.GetCameraFlags();
				}
			}
			if (context.IsTemporalAntialiasingActive())
			{
				depthTextureMode |= this.temporalAntialiasing.GetCameraFlags();
			}
			if (this.fog.IsEnabledAndSupported(context))
			{
				depthTextureMode |= this.fog.GetCameraFlags();
			}
			if (this.debugLayer.debugOverlay != DebugOverlay.None)
			{
				depthTextureMode |= this.debugLayer.GetCameraFlags();
			}
			context.camera.depthTextureMode |= depthTextureMode;
			this.cameraDepthFlags = depthTextureMode;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000B634 File Offset: 0x00009834
		public void ResetHistory()
		{
			foreach (KeyValuePair<Type, PostProcessBundle> keyValuePair in this.m_Bundles)
			{
				keyValuePair.Value.ResetHistory();
			}
			this.temporalAntialiasing.ResetHistory();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000B698 File Offset: 0x00009898
		public bool HasOpaqueOnlyEffects(PostProcessRenderContext context)
		{
			return this.HasActiveEffects(PostProcessEvent.BeforeTransparent, context);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000B6A4 File Offset: 0x000098A4
		public bool HasActiveEffects(PostProcessEvent evt, PostProcessRenderContext context)
		{
			foreach (PostProcessLayer.SerializedBundleRef serializedBundleRef in this.sortedBundles[evt])
			{
				bool flag = serializedBundleRef.bundle.settings.IsEnabledAndSupported(context);
				if (context.isSceneView)
				{
					if (serializedBundleRef.bundle.attribute.allowInSceneView && flag)
					{
						return true;
					}
				}
				else if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000B734 File Offset: 0x00009934
		private void SetupContext(PostProcessRenderContext context)
		{
			if (this.m_OldResources != this.m_Resources || !RuntimeUtilities.isValidResources())
			{
				RuntimeUtilities.UpdateResources(this.m_Resources);
				this.m_OldResources = this.m_Resources;
			}
			this.m_IsRenderingInSceneView = context.camera.cameraType == CameraType.SceneView;
			context.isSceneView = this.m_IsRenderingInSceneView;
			context.resources = this.m_Resources;
			context.propertySheets = this.m_PropertySheetFactory;
			context.debugLayer = this.debugLayer;
			context.antialiasing = this.antialiasingMode;
			context.temporalAntialiasing = this.temporalAntialiasing;
			context.logHistogram = this.m_LogHistogram;
			context.physicalCamera = context.camera.usePhysicalProperties;
			this.SetLegacyCameraFlags(context);
			this.debugLayer.SetFrameSize(context.width, context.height);
			this.m_CurrentContext = context;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000B810 File Offset: 0x00009A10
		public void UpdateVolumeSystem(Camera cam, CommandBuffer cmd)
		{
			if (this.m_SettingsUpdateNeeded)
			{
				cmd.BeginSample("VolumeBlending");
				PostProcessManager.instance.UpdateSettings(this, cam);
				cmd.EndSample("VolumeBlending");
				this.m_TargetPool.Reset();
				if (RuntimeUtilities.scriptableRenderPipelineActive)
				{
					Shader.SetGlobalFloat(ShaderIDs.RenderViewportScaleFactor, 1f);
				}
			}
			this.m_SettingsUpdateNeeded = false;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000B870 File Offset: 0x00009A70
		public void RenderOpaqueOnly(PostProcessRenderContext context)
		{
			if (RuntimeUtilities.scriptableRenderPipelineActive)
			{
				this.SetupContext(context);
			}
			TextureLerper.instance.BeginFrame(context);
			this.UpdateVolumeSystem(context.camera, context.command);
			this.RenderList(this.sortedBundles[PostProcessEvent.BeforeTransparent], context, "OpaqueOnly");
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000B8C0 File Offset: 0x00009AC0
		public void Render(PostProcessRenderContext context)
		{
			if (RuntimeUtilities.scriptableRenderPipelineActive)
			{
				this.SetupContext(context);
			}
			TextureLerper.instance.BeginFrame(context);
			CommandBuffer command = context.command;
			this.UpdateVolumeSystem(context.camera, context.command);
			int num = -1;
			RenderTargetIdentifier source = context.source;
			if (context.stereoActive && context.numberOfEyes > 1 && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
			{
				command.SetSinglePassStereo(SinglePassStereoMode.None);
				command.DisableShaderKeyword("UNITY_SINGLE_PASS_STEREO");
			}
			for (int i = 0; i < context.numberOfEyes; i++)
			{
				bool flag = false;
				if (this.stopNaNPropagation && !this.m_NaNKilled)
				{
					num = this.m_TargetPool.Get();
					context.GetScreenSpaceTemporaryRT(command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
					if (context.stereoActive && context.numberOfEyes > 1)
					{
						if (context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
						{
							command.BlitFullscreenTriangleFromTexArray(context.source, num, RuntimeUtilities.copyFromTexArraySheet, 1, false, i);
							flag = true;
						}
						else if (context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
						{
							command.BlitFullscreenTriangleFromDoubleWide(context.source, num, RuntimeUtilities.copyStdFromDoubleWideMaterial, 1, i);
							flag = true;
						}
					}
					else
					{
						command.BlitFullscreenTriangle(context.source, num, RuntimeUtilities.copySheet, 1, false, null, false);
					}
					context.source = num;
					this.m_NaNKilled = true;
				}
				if (!flag && context.numberOfEyes > 1)
				{
					num = this.m_TargetPool.Get();
					context.GetScreenSpaceTemporaryRT(command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
					if (context.stereoActive)
					{
						if (context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
						{
							command.BlitFullscreenTriangleFromTexArray(context.source, num, RuntimeUtilities.copyFromTexArraySheet, 1, false, i);
						}
						else if (context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
						{
							command.BlitFullscreenTriangleFromDoubleWide(context.source, num, RuntimeUtilities.copyStdFromDoubleWideMaterial, this.stopNaNPropagation ? 1 : 0, i);
						}
					}
					context.source = num;
				}
				if (context.IsTemporalAntialiasingActive())
				{
					if (!RuntimeUtilities.scriptableRenderPipelineActive)
					{
						if (context.stereoActive)
						{
							if (context.camera.stereoActiveEye != Camera.MonoOrStereoscopicEye.Right)
							{
								this.temporalAntialiasing.ConfigureStereoJitteredProjectionMatrices(context);
							}
						}
						else
						{
							this.temporalAntialiasing.ConfigureJitteredProjectionMatrix(context);
						}
					}
					int num2 = this.m_TargetPool.Get();
					RenderTargetIdentifier destination = context.destination;
					context.GetScreenSpaceTemporaryRT(command, num2, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
					context.destination = num2;
					this.temporalAntialiasing.Render(context);
					context.source = num2;
					context.destination = destination;
					if (num > -1)
					{
						command.ReleaseTemporaryRT(num);
					}
					num = num2;
				}
				bool flag2 = this.HasActiveEffects(PostProcessEvent.BeforeStack, context);
				bool flag3 = this.HasActiveEffects(PostProcessEvent.AfterStack, context) && !this.breakBeforeColorGrading;
				bool flag4 = (flag3 || this.antialiasingMode == PostProcessLayer.Antialiasing.FastApproximateAntialiasing || (this.antialiasingMode == PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing && this.subpixelMorphologicalAntialiasing.IsSupported())) && !this.breakBeforeColorGrading;
				if (flag2)
				{
					num = this.RenderInjectionPoint(PostProcessEvent.BeforeStack, context, "BeforeStack", num);
				}
				num = this.RenderBuiltins(context, !flag4, num, i);
				if (flag3)
				{
					num = this.RenderInjectionPoint(PostProcessEvent.AfterStack, context, "AfterStack", num);
				}
				if (flag4)
				{
					this.RenderFinalPass(context, num, i);
				}
				if (context.stereoActive)
				{
					context.source = source;
				}
			}
			if (context.stereoActive && context.numberOfEyes > 1 && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
			{
				command.SetSinglePassStereo(SinglePassStereoMode.SideBySide);
				command.EnableShaderKeyword("UNITY_SINGLE_PASS_STEREO");
			}
			this.debugLayer.RenderSpecialOverlays(context);
			this.debugLayer.RenderMonitors(context);
			TextureLerper.instance.EndFrame();
			this.debugLayer.EndFrame();
			this.m_SettingsUpdateNeeded = true;
			this.m_NaNKilled = false;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000BC68 File Offset: 0x00009E68
		private int RenderInjectionPoint(PostProcessEvent evt, PostProcessRenderContext context, string marker, int releaseTargetAfterUse = -1)
		{
			int num = this.m_TargetPool.Get();
			RenderTargetIdentifier destination = context.destination;
			CommandBuffer command = context.command;
			context.GetScreenSpaceTemporaryRT(command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
			context.destination = num;
			this.RenderList(this.sortedBundles[evt], context, marker);
			context.source = num;
			context.destination = destination;
			if (releaseTargetAfterUse > -1)
			{
				command.ReleaseTemporaryRT(releaseTargetAfterUse);
			}
			return num;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000BCE4 File Offset: 0x00009EE4
		private void RenderList(List<PostProcessLayer.SerializedBundleRef> list, PostProcessRenderContext context, string marker)
		{
			CommandBuffer command = context.command;
			command.BeginSample(marker);
			this.m_ActiveEffects.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				PostProcessBundle bundle = list[i].bundle;
				if (bundle.settings.IsEnabledAndSupported(context) && (!context.isSceneView || (context.isSceneView && bundle.attribute.allowInSceneView)))
				{
					this.m_ActiveEffects.Add(bundle.renderer);
				}
			}
			int count = this.m_ActiveEffects.Count;
			if (count == 1)
			{
				this.m_ActiveEffects[0].RenderOrLog(context);
			}
			else
			{
				this.m_Targets.Clear();
				this.m_Targets.Add(context.source);
				int num = this.m_TargetPool.Get();
				int num2 = this.m_TargetPool.Get();
				for (int j = 0; j < count - 1; j++)
				{
					this.m_Targets.Add((j % 2 == 0) ? num : num2);
				}
				this.m_Targets.Add(context.destination);
				context.GetScreenSpaceTemporaryRT(command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
				if (count > 2)
				{
					context.GetScreenSpaceTemporaryRT(command, num2, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
				}
				for (int k = 0; k < count; k++)
				{
					context.source = this.m_Targets[k];
					context.destination = this.m_Targets[k + 1];
					this.m_ActiveEffects[k].RenderOrLog(context);
				}
				command.ReleaseTemporaryRT(num);
				if (count > 2)
				{
					command.ReleaseTemporaryRT(num2);
				}
			}
			command.EndSample(marker);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000BE94 File Offset: 0x0000A094
		private void ApplyFlip(PostProcessRenderContext context, MaterialPropertyBlock properties)
		{
			if (context.flip && !context.isSceneView)
			{
				properties.SetVector(ShaderIDs.UVTransform, new Vector4(1f, 1f, 0f, 0f));
				return;
			}
			this.ApplyDefaultFlip(properties);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		private void ApplyDefaultFlip(MaterialPropertyBlock properties)
		{
			properties.SetVector(ShaderIDs.UVTransform, SystemInfo.graphicsUVStartsAtTop ? new Vector4(1f, -1f, 0f, 1f) : new Vector4(1f, 1f, 0f, 0f));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000BF28 File Offset: 0x0000A128
		private int RenderBuiltins(PostProcessRenderContext context, bool isFinalPass, int releaseTargetAfterUse = -1, int eye = -1)
		{
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.uber);
			propertySheet.ClearKeywords();
			propertySheet.properties.Clear();
			context.uberSheet = propertySheet;
			context.autoExposureTexture = RuntimeUtilities.whiteTexture;
			context.bloomBufferNameID = -1;
			if (isFinalPass && context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
			{
				propertySheet.EnableKeyword("STEREO_INSTANCING_ENABLED");
			}
			CommandBuffer command = context.command;
			command.BeginSample("BuiltinStack");
			int num = -1;
			RenderTargetIdentifier destination = context.destination;
			if (!isFinalPass)
			{
				num = this.m_TargetPool.Get();
				context.GetScreenSpaceTemporaryRT(command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
				context.destination = num;
				if (this.antialiasingMode == PostProcessLayer.Antialiasing.FastApproximateAntialiasing && !this.fastApproximateAntialiasing.keepAlpha && RuntimeUtilities.hasAlpha(context.sourceFormat))
				{
					propertySheet.properties.SetFloat(ShaderIDs.LumaInAlpha, 1f);
				}
			}
			int num2 = this.RenderEffect<DepthOfField>(context, true);
			int num3 = this.RenderEffect<MotionBlur>(context, true);
			if (this.ShouldGenerateLogHistogram(context))
			{
				this.m_LogHistogram.Generate(context);
			}
			int xrActiveEye = context.xrActiveEye;
			if (context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.MultiPass)
			{
				context.xrActiveEye = eye;
			}
			this.RenderEffect<AutoExposure>(context, false);
			context.xrActiveEye = xrActiveEye;
			propertySheet.properties.SetTexture(ShaderIDs.AutoExposureTex, context.autoExposureTexture);
			this.RenderEffect<LensDistortion>(context, false);
			this.RenderEffect<ChromaticAberration>(context, false);
			this.RenderEffect<Bloom>(context, false);
			this.RenderEffect<Vignette>(context, false);
			this.RenderEffect<Grain>(context, false);
			if (!this.breakBeforeColorGrading)
			{
				this.RenderEffect<ColorGrading>(context, false);
			}
			if (isFinalPass)
			{
				propertySheet.EnableKeyword("FINALPASS");
				this.dithering.Render(context);
				this.ApplyFlip(context, propertySheet.properties);
			}
			else
			{
				this.ApplyDefaultFlip(propertySheet.properties);
			}
			if (context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
			{
				propertySheet.properties.SetFloat(ShaderIDs.DepthSlice, (float)eye);
				command.BlitFullscreenTriangleToTexArray(context.source, context.destination, propertySheet, 0, false, eye);
			}
			else if (isFinalPass && context.stereoActive && context.numberOfEyes > 1 && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
			{
				command.BlitFullscreenTriangleToDoubleWide(context.source, context.destination, propertySheet, 0, eye);
			}
			else
			{
				command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false, null, false);
			}
			context.source = context.destination;
			context.destination = destination;
			if (releaseTargetAfterUse > -1)
			{
				command.ReleaseTemporaryRT(releaseTargetAfterUse);
			}
			if (num3 > -1)
			{
				command.ReleaseTemporaryRT(num3);
			}
			if (num2 > -1)
			{
				command.ReleaseTemporaryRT(num2);
			}
			if (context.bloomBufferNameID > -1)
			{
				command.ReleaseTemporaryRT(context.bloomBufferNameID);
			}
			command.EndSample("BuiltinStack");
			return num;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
		private void RenderFinalPass(PostProcessRenderContext context, int releaseTargetAfterUse = -1, int eye = -1)
		{
			CommandBuffer command = context.command;
			command.BeginSample("FinalPass");
			if (this.breakBeforeColorGrading)
			{
				PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.discardAlpha);
				if (context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
				{
					propertySheet.EnableKeyword("STEREO_INSTANCING_ENABLED");
				}
				if (context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
				{
					propertySheet.properties.SetFloat(ShaderIDs.DepthSlice, (float)eye);
					command.BlitFullscreenTriangleToTexArray(context.source, context.destination, propertySheet, 0, false, eye);
				}
				else if (context.stereoActive && context.numberOfEyes > 1 && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
				{
					command.BlitFullscreenTriangleToDoubleWide(context.source, context.destination, propertySheet, 0, eye);
				}
				else
				{
					command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false, null, false);
				}
			}
			else
			{
				PropertySheet propertySheet2 = context.propertySheets.Get(context.resources.shaders.finalPass);
				propertySheet2.ClearKeywords();
				propertySheet2.properties.Clear();
				context.uberSheet = propertySheet2;
				int num = -1;
				if (context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
				{
					propertySheet2.EnableKeyword("STEREO_INSTANCING_ENABLED");
				}
				if (this.antialiasingMode == PostProcessLayer.Antialiasing.FastApproximateAntialiasing)
				{
					propertySheet2.EnableKeyword(this.fastApproximateAntialiasing.fastMode ? "FXAA_LOW" : "FXAA");
					if (RuntimeUtilities.hasAlpha(context.sourceFormat))
					{
						if (this.fastApproximateAntialiasing.keepAlpha)
						{
							propertySheet2.EnableKeyword("FXAA_KEEP_ALPHA");
						}
					}
					else
					{
						propertySheet2.EnableKeyword("FXAA_NO_ALPHA");
					}
				}
				else if (this.antialiasingMode == PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing && this.subpixelMorphologicalAntialiasing.IsSupported())
				{
					num = this.m_TargetPool.Get();
					RenderTargetIdentifier destination = context.destination;
					context.GetScreenSpaceTemporaryRT(context.command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
					context.destination = num;
					this.subpixelMorphologicalAntialiasing.Render(context);
					context.source = num;
					context.destination = destination;
				}
				this.dithering.Render(context);
				this.ApplyFlip(context, propertySheet2.properties);
				if (context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePassInstanced)
				{
					propertySheet2.properties.SetFloat(ShaderIDs.DepthSlice, (float)eye);
					command.BlitFullscreenTriangleToTexArray(context.source, context.destination, propertySheet2, 0, false, eye);
				}
				else if (context.stereoActive && context.numberOfEyes > 1 && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass)
				{
					command.BlitFullscreenTriangleToDoubleWide(context.source, context.destination, propertySheet2, 0, eye);
				}
				else
				{
					command.BlitFullscreenTriangle(context.source, context.destination, propertySheet2, 0, false, null, false);
				}
				if (num > -1)
				{
					command.ReleaseTemporaryRT(num);
				}
			}
			if (releaseTargetAfterUse > -1)
			{
				command.ReleaseTemporaryRT(releaseTargetAfterUse);
			}
			command.EndSample("FinalPass");
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
		private int RenderEffect<T>(PostProcessRenderContext context, bool useTempTarget = false) where T : PostProcessEffectSettings
		{
			PostProcessBundle bundle = this.GetBundle<T>();
			if (!bundle.settings.IsEnabledAndSupported(context))
			{
				return -1;
			}
			if (this.m_IsRenderingInSceneView && !bundle.attribute.allowInSceneView)
			{
				return -1;
			}
			if (!useTempTarget)
			{
				bundle.renderer.RenderOrLog(context);
				return -1;
			}
			RenderTargetIdentifier destination = context.destination;
			int num = this.m_TargetPool.Get();
			context.GetScreenSpaceTemporaryRT(context.command, num, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, 0, 0);
			context.destination = num;
			bundle.renderer.RenderOrLog(context);
			context.source = num;
			context.destination = destination;
			return num;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000C570 File Offset: 0x0000A770
		private bool ShouldGenerateLogHistogram(PostProcessRenderContext context)
		{
			bool flag = this.GetBundle<AutoExposure>().settings.IsEnabledAndSupported(context);
			bool flag2 = this.debugLayer.lightMeter.IsRequestedAndSupported(context);
			return flag || flag2;
		}

		// Token: 0x04000133 RID: 307
		public Transform volumeTrigger;

		// Token: 0x04000134 RID: 308
		public LayerMask volumeLayer;

		// Token: 0x04000135 RID: 309
		public bool stopNaNPropagation = true;

		// Token: 0x04000136 RID: 310
		public bool finalBlitToCameraTarget;

		// Token: 0x04000137 RID: 311
		public PostProcessLayer.Antialiasing antialiasingMode;

		// Token: 0x04000138 RID: 312
		public TemporalAntialiasing temporalAntialiasing;

		// Token: 0x04000139 RID: 313
		public SubpixelMorphologicalAntialiasing subpixelMorphologicalAntialiasing;

		// Token: 0x0400013A RID: 314
		public FastApproximateAntialiasing fastApproximateAntialiasing;

		// Token: 0x0400013B RID: 315
		public Fog fog;

		// Token: 0x0400013C RID: 316
		private Dithering dithering;

		// Token: 0x0400013D RID: 317
		public PostProcessDebugLayer debugLayer;

		// Token: 0x0400013E RID: 318
		[SerializeField]
		private PostProcessResources m_Resources;

		// Token: 0x0400013F RID: 319
		[NonSerialized]
		private PostProcessResources m_OldResources;

		// Token: 0x04000140 RID: 320
		[Preserve]
		[SerializeField]
		private bool m_ShowToolkit;

		// Token: 0x04000141 RID: 321
		[Preserve]
		[SerializeField]
		private bool m_ShowCustomSorter;

		// Token: 0x04000142 RID: 322
		public bool breakBeforeColorGrading;

		// Token: 0x04000143 RID: 323
		[SerializeField]
		private List<PostProcessLayer.SerializedBundleRef> m_BeforeTransparentBundles;

		// Token: 0x04000144 RID: 324
		[SerializeField]
		private List<PostProcessLayer.SerializedBundleRef> m_BeforeStackBundles;

		// Token: 0x04000145 RID: 325
		[SerializeField]
		private List<PostProcessLayer.SerializedBundleRef> m_AfterStackBundles;

		// Token: 0x04000149 RID: 329
		private Dictionary<Type, PostProcessBundle> m_Bundles;

		// Token: 0x0400014A RID: 330
		private PropertySheetFactory m_PropertySheetFactory;

		// Token: 0x0400014B RID: 331
		private CommandBuffer m_LegacyCmdBufferBeforeReflections;

		// Token: 0x0400014C RID: 332
		private CommandBuffer m_LegacyCmdBufferBeforeLighting;

		// Token: 0x0400014D RID: 333
		private CommandBuffer m_LegacyCmdBufferOpaque;

		// Token: 0x0400014E RID: 334
		private CommandBuffer m_LegacyCmdBuffer;

		// Token: 0x0400014F RID: 335
		private Camera m_Camera;

		// Token: 0x04000150 RID: 336
		private PostProcessRenderContext m_CurrentContext;

		// Token: 0x04000151 RID: 337
		private LogHistogram m_LogHistogram;

		// Token: 0x04000152 RID: 338
		private bool m_SettingsUpdateNeeded = true;

		// Token: 0x04000153 RID: 339
		private bool m_IsRenderingInSceneView;

		// Token: 0x04000154 RID: 340
		private TargetPool m_TargetPool;

		// Token: 0x04000155 RID: 341
		private bool m_NaNKilled;

		// Token: 0x04000156 RID: 342
		private readonly List<PostProcessEffectRenderer> m_ActiveEffects = new List<PostProcessEffectRenderer>();

		// Token: 0x04000157 RID: 343
		private readonly List<RenderTargetIdentifier> m_Targets = new List<RenderTargetIdentifier>();

		// Token: 0x0200007A RID: 122
		public enum Antialiasing
		{
			// Token: 0x040002A7 RID: 679
			None,
			// Token: 0x040002A8 RID: 680
			FastApproximateAntialiasing,
			// Token: 0x040002A9 RID: 681
			SubpixelMorphologicalAntialiasing,
			// Token: 0x040002AA RID: 682
			TemporalAntialiasing
		}

		// Token: 0x0200007B RID: 123
		[Serializable]
		public sealed class SerializedBundleRef
		{
			// Token: 0x040002AB RID: 683
			public string assemblyQualifiedName;

			// Token: 0x040002AC RID: 684
			public PostProcessBundle bundle;
		}
	}
}
