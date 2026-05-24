using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.UIElements.UIR.Implementation;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200030A RID: 778
	internal class RenderChain : IDisposable
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x00067FB4 File Offset: 0x000661B4
		internal RenderChainCommand firstCommand
		{
			get
			{
				return this.m_FirstCommand;
			}
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00067FCC File Offset: 0x000661CC
		static RenderChain()
		{
			Utility.RegisterIntermediateRenderers += new Action<Camera>(RenderChain.OnRegisterIntermediateRenderers);
			Utility.RenderNodeExecute += new Action<IntPtr>(RenderChain.OnRenderNodeExecute);
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00068070 File Offset: 0x00066270
		public RenderChain(BaseVisualElementPanel panel)
		{
			this.Constructor(panel, new UIRenderDevice(0U, 0U), panel.atlas, new VectorImageManager(panel.atlas));
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00068134 File Offset: 0x00066334
		protected RenderChain(BaseVisualElementPanel panel, UIRenderDevice device, AtlasBase atlas, VectorImageManager vectorImageManager)
		{
			this.Constructor(panel, device, atlas, vectorImageManager);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x000681E4 File Offset: 0x000663E4
		private void Constructor(BaseVisualElementPanel panelObj, UIRenderDevice deviceObj, AtlasBase atlas, VectorImageManager vectorImageMan)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			this.m_DirtyTracker.heads = new List<VisualElement>(8);
			this.m_DirtyTracker.tails = new List<VisualElement>(8);
			this.m_DirtyTracker.minDepths = new int[5];
			this.m_DirtyTracker.maxDepths = new int[5];
			this.m_DirtyTracker.Reset();
			bool flag = this.m_RenderNodesData.Count < 1;
			if (flag)
			{
				this.m_RenderNodesData.Add(new RenderChain.RenderNodeData
				{
					matPropBlock = new MaterialPropertyBlock()
				});
			}
			this.panel = panelObj;
			this.device = deviceObj;
			this.atlas = atlas;
			this.vectorImageManager = vectorImageMan;
			this.shaderInfoAllocator.Construct();
			this.painter = new UIRStylePainter(this);
			Font.textureRebuilt += new Action<Font>(this.OnFontReset);
			BaseRuntimePanel baseRuntimePanel = this.panel as BaseRuntimePanel;
			bool flag2 = baseRuntimePanel != null && baseRuntimePanel.drawToCameras;
			if (flag2)
			{
				this.drawInCameras = true;
				this.m_StaticIndex = RenderChain.RenderChainStaticIndexAllocator.AllocateIndex(this);
			}
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x00068308 File Offset: 0x00066508
		private void Destructor()
		{
			bool flag = this.m_StaticIndex >= 0;
			if (flag)
			{
				RenderChain.RenderChainStaticIndexAllocator.FreeIndex(this.m_StaticIndex);
			}
			this.m_StaticIndex = -1;
			RenderChainCommand firstCommand = this.m_FirstCommand;
			for (VisualElement visualElement = RenderChain.GetFirstElementInPanel((firstCommand != null) ? firstCommand.owner : null); visualElement != null; visualElement = visualElement.renderChainData.next)
			{
				this.ResetTextures(visualElement);
			}
			UIRUtility.Destroy(this.m_DefaultMat);
			UIRUtility.Destroy(this.m_DefaultWorldSpaceMat);
			this.m_DefaultMat = (this.m_DefaultWorldSpaceMat = null);
			Font.textureRebuilt -= new Action<Font>(this.OnFontReset);
			UIRStylePainter painter = this.painter;
			if (painter != null)
			{
				painter.Dispose();
			}
			UIRTextUpdatePainter textUpdatePainter = this.m_TextUpdatePainter;
			if (textUpdatePainter != null)
			{
				textUpdatePainter.Dispose();
			}
			VectorImageManager vectorImageManager = this.vectorImageManager;
			if (vectorImageManager != null)
			{
				vectorImageManager.Dispose();
			}
			this.shaderInfoAllocator.Dispose();
			UIRenderDevice device = this.device;
			if (device != null)
			{
				device.Dispose();
			}
			this.painter = null;
			this.m_TextUpdatePainter = null;
			this.atlas = null;
			this.shaderInfoAllocator = default(UIRVEShaderInfoAllocator);
			this.device = null;
			this.m_ActiveRenderNodes = 0;
			this.m_RenderNodesData.Clear();
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x0006843D File Offset: 0x0006663D
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00068445 File Offset: 0x00066645
		private protected bool disposed { protected get; private set; }

		// Token: 0x06001956 RID: 6486 RVA: 0x0006844E File Offset: 0x0006664E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00068460 File Offset: 0x00066660
		protected void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.Destructor();
				}
				this.disposed = true;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x00068490 File Offset: 0x00066690
		internal ChainBuilderStats stats
		{
			get
			{
				return this.m_Stats;
			}
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000684A8 File Offset: 0x000666A8
		public void ProcessChanges()
		{
			RenderChain.s_MarkerProcess.Begin();
			this.m_Stats = default(ChainBuilderStats);
			this.m_Stats.elementsAdded = this.m_Stats.elementsAdded + this.m_StatsElementsAdded;
			this.m_Stats.elementsRemoved = this.m_Stats.elementsRemoved + this.m_StatsElementsRemoved;
			this.m_StatsElementsAdded = (this.m_StatsElementsRemoved = 0U);
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			int num = 0;
			RenderDataDirtyTypes renderDataDirtyTypes = RenderDataDirtyTypes.Clipping | RenderDataDirtyTypes.ClippingHierarchy;
			RenderDataDirtyTypes renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			RenderChain.s_MarkerClipProcessing.Begin();
			for (int i = this.m_DirtyTracker.minDepths[num]; i <= this.m_DirtyTracker.maxDepths[num]; i++)
			{
				VisualElement visualElement = this.m_DirtyTracker.heads[i];
				while (visualElement != null)
				{
					VisualElement nextDirty = visualElement.renderChainData.nextDirty;
					bool flag = (visualElement.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag)
					{
						bool flag2 = visualElement.renderChainData.isInChain && visualElement.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag2)
						{
							RenderEvents.ProcessOnClippingChanged(this, visualElement, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement, renderDataDirtyTypes2);
					}
					visualElement = nextDirty;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			RenderChain.s_MarkerClipProcessing.End();
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 1;
			renderDataDirtyTypes = RenderDataDirtyTypes.Opacity | RenderDataDirtyTypes.OpacityHierarchy;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			RenderChain.s_MarkerOpacityProcessing.Begin();
			for (int j = this.m_DirtyTracker.minDepths[num]; j <= this.m_DirtyTracker.maxDepths[num]; j++)
			{
				VisualElement visualElement2 = this.m_DirtyTracker.heads[j];
				while (visualElement2 != null)
				{
					VisualElement nextDirty2 = visualElement2.renderChainData.nextDirty;
					bool flag3 = (visualElement2.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag3)
					{
						bool flag4 = visualElement2.renderChainData.isInChain && visualElement2.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag4)
						{
							RenderEvents.ProcessOnOpacityChanged(this, visualElement2, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement2, renderDataDirtyTypes2);
					}
					visualElement2 = nextDirty2;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			RenderChain.s_MarkerOpacityProcessing.End();
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 2;
			renderDataDirtyTypes = RenderDataDirtyTypes.Color;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			RenderChain.s_MarkerColorsProcessing.Begin();
			for (int k = this.m_DirtyTracker.minDepths[num]; k <= this.m_DirtyTracker.maxDepths[num]; k++)
			{
				VisualElement visualElement3 = this.m_DirtyTracker.heads[k];
				while (visualElement3 != null)
				{
					VisualElement nextDirty3 = visualElement3.renderChainData.nextDirty;
					bool flag5 = (visualElement3.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag5)
					{
						bool flag6 = visualElement3.renderChainData.isInChain && visualElement3.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag6)
						{
							RenderEvents.ProcessOnColorChanged(this, visualElement3, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement3, renderDataDirtyTypes2);
					}
					visualElement3 = nextDirty3;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			RenderChain.s_MarkerColorsProcessing.End();
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 3;
			renderDataDirtyTypes = RenderDataDirtyTypes.Transform | RenderDataDirtyTypes.ClipRectSize;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			RenderChain.s_MarkerTransformProcessing.Begin();
			for (int l = this.m_DirtyTracker.minDepths[num]; l <= this.m_DirtyTracker.maxDepths[num]; l++)
			{
				VisualElement visualElement4 = this.m_DirtyTracker.heads[l];
				while (visualElement4 != null)
				{
					VisualElement nextDirty4 = visualElement4.renderChainData.nextDirty;
					bool flag7 = (visualElement4.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag7)
					{
						bool flag8 = visualElement4.renderChainData.isInChain && visualElement4.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag8)
						{
							RenderEvents.ProcessOnTransformOrSizeChanged(this, visualElement4, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement4, renderDataDirtyTypes2);
					}
					visualElement4 = nextDirty4;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			RenderChain.s_MarkerTransformProcessing.End();
			this.m_BlockDirtyRegistration = true;
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 4;
			renderDataDirtyTypes = RenderDataDirtyTypes.Visuals | RenderDataDirtyTypes.VisualsHierarchy;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			RenderChain.s_MarkerVisualsProcessing.Begin();
			for (int m = this.m_DirtyTracker.minDepths[num]; m <= this.m_DirtyTracker.maxDepths[num]; m++)
			{
				VisualElement visualElement5 = this.m_DirtyTracker.heads[m];
				while (visualElement5 != null)
				{
					VisualElement nextDirty5 = visualElement5.renderChainData.nextDirty;
					bool flag9 = (visualElement5.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag9)
					{
						bool flag10 = visualElement5.renderChainData.isInChain && visualElement5.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag10)
						{
							RenderEvents.ProcessOnVisualsChanged(this, visualElement5, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement5, renderDataDirtyTypes2);
					}
					visualElement5 = nextDirty5;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			RenderChain.s_MarkerVisualsProcessing.End();
			this.m_BlockDirtyRegistration = false;
			this.m_DirtyTracker.Reset();
			this.ProcessTextRegen(true);
			bool fontWasReset = this.m_FontWasReset;
			if (fontWasReset)
			{
				for (int n = 0; n < 2; n++)
				{
					bool flag11 = !this.m_FontWasReset;
					if (flag11)
					{
						break;
					}
					this.m_FontWasReset = false;
					this.ProcessTextRegen(false);
				}
			}
			AtlasBase atlas = this.atlas;
			if (atlas != null)
			{
				atlas.InvokeUpdateDynamicTextures(this.panel);
			}
			VectorImageManager vectorImageManager = this.vectorImageManager;
			if (vectorImageManager != null)
			{
				vectorImageManager.Commit();
			}
			this.shaderInfoAllocator.IssuePendingStorageChanges();
			UIRenderDevice device = this.device;
			if (device != null)
			{
				device.OnFrameRenderingBegin();
			}
			RenderChain.s_MarkerProcess.End();
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00068B8C File Offset: 0x00066D8C
		public void Render()
		{
			Material standardMaterial = this.GetStandardMaterial();
			this.panel.InvokeUpdateMaterial(standardMaterial);
			Exception ex = null;
			bool flag = this.m_FirstCommand != null;
			if (flag)
			{
				bool flag2 = !this.drawInCameras;
				if (flag2)
				{
					Rect layout = this.panel.visualTree.layout;
					if (standardMaterial != null)
					{
						standardMaterial.SetPass(0);
					}
					Matrix4x4 matrix4x = ProjectionUtils.Ortho(layout.xMin, layout.xMax, layout.yMax, layout.yMin, -0.001f, 1.001f);
					GL.LoadProjectionMatrix(matrix4x);
					GL.modelview = Matrix4x4.identity;
					UIRenderDevice device = this.device;
					RenderChainCommand firstCommand = this.m_FirstCommand;
					Material material = standardMaterial;
					Material material2 = standardMaterial;
					VectorImageManager vectorImageManager = this.vectorImageManager;
					device.EvaluateChain(firstCommand, material, material2, (vectorImageManager != null) ? vectorImageManager.atlas : null, this.shaderInfoAllocator.atlas, this.panel.scaledPixelsPerPoint, this.shaderInfoAllocator.transformConstants, this.shaderInfoAllocator.clipRectConstants, this.m_RenderNodesData[0].matPropBlock, true, ref ex);
				}
			}
			bool flag3 = ex != null;
			if (!flag3)
			{
				bool drawStats = this.drawStats;
				if (drawStats)
				{
					this.DrawStats();
				}
				return;
			}
			bool flag4 = GUIUtility.IsExitGUIException(ex);
			if (flag4)
			{
				throw ex;
			}
			throw new ImmediateModeException(ex);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00068CCC File Offset: 0x00066ECC
		private void ProcessTextRegen(bool timeSliced)
		{
			bool flag = (timeSliced && this.m_DirtyTextRemaining == 0) || this.m_TextElementCount == 0;
			if (!flag)
			{
				RenderChain.s_MarkerTextRegen.Begin();
				bool flag2 = this.m_TextUpdatePainter == null;
				if (flag2)
				{
					this.m_TextUpdatePainter = new UIRTextUpdatePainter();
				}
				VisualElement visualElement = this.m_FirstTextElement;
				this.m_DirtyTextStartIndex = (timeSliced ? (this.m_DirtyTextStartIndex % this.m_TextElementCount) : 0);
				for (int i = 0; i < this.m_DirtyTextStartIndex; i++)
				{
					visualElement = visualElement.renderChainData.nextText;
				}
				bool flag3 = visualElement == null;
				if (flag3)
				{
					visualElement = this.m_FirstTextElement;
				}
				int num = (timeSliced ? Math.Min(50, this.m_DirtyTextRemaining) : this.m_TextElementCount);
				for (int j = 0; j < num; j++)
				{
					RenderEvents.ProcessRegenText(this, visualElement, this.m_TextUpdatePainter, this.device, ref this.m_Stats);
					visualElement = visualElement.renderChainData.nextText;
					this.m_DirtyTextStartIndex++;
					bool flag4 = visualElement == null;
					if (flag4)
					{
						visualElement = this.m_FirstTextElement;
						this.m_DirtyTextStartIndex = 0;
					}
				}
				this.m_DirtyTextRemaining = Math.Max(0, this.m_DirtyTextRemaining - num);
				bool flag5 = this.m_DirtyTextRemaining > 0;
				if (flag5)
				{
					BaseVisualElementPanel panel = this.panel;
					if (panel != null)
					{
						panel.OnVersionChanged(this.m_FirstTextElement, VersionChangeType.Transform);
					}
				}
				RenderChain.s_MarkerTextRegen.End();
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00068E40 File Offset: 0x00067040
		public void UIEOnChildAdded(VisualElement ve)
		{
			VisualElement parent = ve.hierarchy.parent;
			int num = ((parent != null) ? parent.IndexOf(ve) : 0);
			bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
			if (blockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be added to an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
			}
			bool flag = parent != null && !parent.renderChainData.isInChain;
			if (!flag)
			{
				uint num2 = RenderEvents.DepthFirstOnChildAdded(this, parent, ve, num, true);
				Debug.Assert(ve.renderChainData.isInChain);
				Debug.Assert(ve.panel == this.panel);
				this.UIEOnClippingChanged(ve, true);
				this.UIEOnOpacityChanged(ve, false);
				this.UIEOnVisualsChanged(ve, true);
				ve.MarkRenderHintsClean();
				this.m_StatsElementsAdded += num2;
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00068F00 File Offset: 0x00067100
		public void UIEOnChildrenReordered(VisualElement ve)
		{
			bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
			if (blockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be moved under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
			}
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				RenderEvents.DepthFirstOnChildRemoving(this, ve.hierarchy[i]);
			}
			for (int j = 0; j < childCount; j++)
			{
				RenderEvents.DepthFirstOnChildAdded(this, ve, ve.hierarchy[j], j, false);
			}
			this.UIEOnClippingChanged(ve, true);
			this.UIEOnOpacityChanged(ve, true);
			this.UIEOnVisualsChanged(ve, true);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00068FA8 File Offset: 0x000671A8
		public void UIEOnChildRemoving(VisualElement ve)
		{
			bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
			if (blockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be removed from an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
			}
			this.m_StatsElementsRemoved += RenderEvents.DepthFirstOnChildRemoving(this, ve);
			Debug.Assert(!ve.renderChainData.isInChain);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00068FF3 File Offset: 0x000671F3
		public void StopTrackingGroupTransformElement(VisualElement ve)
		{
			this.m_LastGroupTransformElementScale.Remove(ve);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x00069004 File Offset: 0x00067204
		public void UIEOnRenderHintsChanged(VisualElement ve)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("Render Hints cannot change under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.UIEOnChildRemoving(ve);
				this.UIEOnChildAdded(ve);
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00069048 File Offset: 0x00067248
		public void UIEOnClippingChanged(VisualElement ve, bool hierarchical)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change clipping state under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Clipping | (hierarchical ? RenderDataDirtyTypes.ClippingHierarchy : RenderDataDirtyTypes.None), RenderDataDirtyTypeClasses.Clipping);
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00069094 File Offset: 0x00067294
		public void UIEOnOpacityChanged(VisualElement ve, bool hierarchical = false)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change opacity under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Opacity | (hierarchical ? RenderDataDirtyTypes.OpacityHierarchy : RenderDataDirtyTypes.None), RenderDataDirtyTypeClasses.Opacity);
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x000690E4 File Offset: 0x000672E4
		public void UIEOnColorChanged(VisualElement ve)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change background color under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Color, RenderDataDirtyTypeClasses.Color);
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0006912C File Offset: 0x0006732C
		public void UIEOnTransformOrSizeChanged(VisualElement ve, bool transformChanged, bool clipRectSizeChanged)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change size or transform under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				RenderDataDirtyTypes renderDataDirtyTypes = (transformChanged ? RenderDataDirtyTypes.Transform : RenderDataDirtyTypes.None) | (clipRectSizeChanged ? RenderDataDirtyTypes.ClipRectSize : RenderDataDirtyTypes.None);
				this.m_DirtyTracker.RegisterDirty(ve, renderDataDirtyTypes, RenderDataDirtyTypeClasses.TransformSize);
			}
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00069180 File Offset: 0x00067380
		public void UIEOnVisualsChanged(VisualElement ve, bool hierarchical)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot be marked for dirty repaint under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Visuals | (hierarchical ? RenderDataDirtyTypes.VisualsHierarchy : RenderDataDirtyTypes.None), RenderDataDirtyTypeClasses.Visuals);
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x000691CD File Offset: 0x000673CD
		// (set) Token: 0x06001967 RID: 6503 RVA: 0x000691D5 File Offset: 0x000673D5
		internal BaseVisualElementPanel panel { get; private set; }

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x000691DE File Offset: 0x000673DE
		// (set) Token: 0x06001969 RID: 6505 RVA: 0x000691E6 File Offset: 0x000673E6
		internal UIRenderDevice device { get; private set; }

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x000691EF File Offset: 0x000673EF
		// (set) Token: 0x0600196B RID: 6507 RVA: 0x000691F7 File Offset: 0x000673F7
		internal AtlasBase atlas { get; private set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x00069200 File Offset: 0x00067400
		// (set) Token: 0x0600196D RID: 6509 RVA: 0x00069208 File Offset: 0x00067408
		internal VectorImageManager vectorImageManager { get; private set; }

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x00069211 File Offset: 0x00067411
		// (set) Token: 0x0600196F RID: 6511 RVA: 0x00069219 File Offset: 0x00067419
		internal UIRStylePainter painter { get; private set; }

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x00069222 File Offset: 0x00067422
		// (set) Token: 0x06001971 RID: 6513 RVA: 0x0006922A File Offset: 0x0006742A
		internal bool drawStats { get; set; }

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00069233 File Offset: 0x00067433
		// (set) Token: 0x06001973 RID: 6515 RVA: 0x0006923B File Offset: 0x0006743B
		internal bool drawInCameras { get; private set; }

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x00069244 File Offset: 0x00067444
		// (set) Token: 0x06001975 RID: 6517 RVA: 0x0006925C File Offset: 0x0006745C
		internal Shader defaultShader
		{
			get
			{
				return this.m_DefaultShader;
			}
			set
			{
				bool flag = this.m_DefaultShader == value;
				if (!flag)
				{
					this.m_DefaultShader = value;
					UIRUtility.Destroy(this.m_DefaultMat);
					this.m_DefaultMat = null;
				}
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x00069298 File Offset: 0x00067498
		// (set) Token: 0x06001977 RID: 6519 RVA: 0x000692B0 File Offset: 0x000674B0
		internal Shader defaultWorldSpaceShader
		{
			get
			{
				return this.m_DefaultWorldSpaceShader;
			}
			set
			{
				bool flag = this.m_DefaultWorldSpaceShader == value;
				if (!flag)
				{
					this.m_DefaultWorldSpaceShader = value;
					UIRUtility.Destroy(this.m_DefaultWorldSpaceMat);
					this.m_DefaultWorldSpaceMat = null;
				}
			}
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x000692EC File Offset: 0x000674EC
		internal Material GetStandardMaterial()
		{
			bool flag = this.m_DefaultMat == null && this.m_DefaultShader != null;
			if (flag)
			{
				this.m_DefaultMat = new Material(this.m_DefaultShader);
				this.m_DefaultMat.hideFlags |= HideFlags.DontSaveInEditor;
			}
			return this.m_DefaultMat;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0006934C File Offset: 0x0006754C
		internal Material GetStandardWorldSpaceMaterial()
		{
			bool flag = this.m_DefaultWorldSpaceMat == null && this.m_DefaultWorldSpaceShader != null;
			if (flag)
			{
				this.m_DefaultWorldSpaceMat = new Material(this.m_DefaultWorldSpaceShader);
				this.m_DefaultWorldSpaceMat.hideFlags |= HideFlags.DontSaveInEditor;
			}
			return this.m_DefaultWorldSpaceMat;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x000693AC File Offset: 0x000675AC
		internal void EnsureFitsDepth(int depth)
		{
			this.m_DirtyTracker.EnsureFits(depth);
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x000693BC File Offset: 0x000675BC
		internal void ChildWillBeRemoved(VisualElement ve)
		{
			bool flag = ve.renderChainData.dirtiedValues > RenderDataDirtyTypes.None;
			if (flag)
			{
				this.m_DirtyTracker.ClearDirty(ve, ~ve.renderChainData.dirtiedValues);
			}
			Debug.Assert(ve.renderChainData.dirtiedValues == RenderDataDirtyTypes.None);
			Debug.Assert(ve.renderChainData.prevDirty == null);
			Debug.Assert(ve.renderChainData.nextDirty == null);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00069434 File Offset: 0x00067634
		internal RenderChainCommand AllocCommand()
		{
			RenderChainCommand renderChainCommand = this.m_CommandPool.Get();
			renderChainCommand.Reset();
			return renderChainCommand;
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0006945C File Offset: 0x0006765C
		internal void FreeCommand(RenderChainCommand cmd)
		{
			bool flag = cmd.state.material != null;
			if (flag)
			{
				this.m_CustomMaterialCommands--;
			}
			cmd.Reset();
			this.m_CommandPool.Return(cmd);
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x000694A4 File Offset: 0x000676A4
		internal void OnRenderCommandAdded(RenderChainCommand command)
		{
			bool flag = command.prev == null;
			if (flag)
			{
				this.m_FirstCommand = command;
			}
			bool flag2 = command.state.material != null;
			if (flag2)
			{
				this.m_CustomMaterialCommands++;
			}
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x000694EC File Offset: 0x000676EC
		internal void OnRenderCommandsRemoved(RenderChainCommand firstCommand, RenderChainCommand lastCommand)
		{
			bool flag = firstCommand.prev == null;
			if (flag)
			{
				this.m_FirstCommand = lastCommand.next;
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00069514 File Offset: 0x00067714
		internal void AddTextElement(VisualElement ve)
		{
			bool flag = this.m_FirstTextElement != null;
			if (flag)
			{
				this.m_FirstTextElement.renderChainData.prevText = ve;
				ve.renderChainData.nextText = this.m_FirstTextElement;
			}
			this.m_FirstTextElement = ve;
			this.m_TextElementCount++;
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00069568 File Offset: 0x00067768
		internal void RemoveTextElement(VisualElement ve)
		{
			bool flag = ve.renderChainData.prevText != null;
			if (flag)
			{
				ve.renderChainData.prevText.renderChainData.nextText = ve.renderChainData.nextText;
			}
			bool flag2 = ve.renderChainData.nextText != null;
			if (flag2)
			{
				ve.renderChainData.nextText.renderChainData.prevText = ve.renderChainData.prevText;
			}
			bool flag3 = this.m_FirstTextElement == ve;
			if (flag3)
			{
				this.m_FirstTextElement = ve.renderChainData.nextText;
			}
			ve.renderChainData.prevText = (ve.renderChainData.nextText = null);
			this.m_TextElementCount--;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00069620 File Offset: 0x00067820
		internal void OnGroupTransformElementChangedTransform(VisualElement ve)
		{
			Vector2 vector;
			bool flag = !this.m_LastGroupTransformElementScale.TryGetValue(ve, ref vector) || ve.worldTransform.m00 != vector.x || ve.worldTransform.m11 != vector.y;
			if (flag)
			{
				this.m_DirtyTextRemaining = this.m_TextElementCount;
				this.m_LastGroupTransformElementScale[ve] = new Vector2(ve.worldTransform.m00, ve.worldTransform.m11);
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x000696A4 File Offset: 0x000678A4
		private unsafe static RenderChain.RenderNodeData AccessRenderNodeData(IntPtr obj)
		{
			int* ptr = (int*)obj.ToPointer();
			RenderChain renderChain = RenderChain.RenderChainStaticIndexAllocator.AccessIndex(*ptr);
			return renderChain.m_RenderNodesData[ptr[1]];
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x000696D8 File Offset: 0x000678D8
		private static void OnRenderNodeExecute(IntPtr obj)
		{
			RenderChain.RenderNodeData renderNodeData = RenderChain.AccessRenderNodeData(obj);
			Exception ex = null;
			renderNodeData.device.EvaluateChain(renderNodeData.firstCommand, renderNodeData.initialMaterial, renderNodeData.standardMaterial, renderNodeData.vectorAtlas, renderNodeData.shaderInfoAtlas, renderNodeData.dpiScale, renderNodeData.transformConstants, renderNodeData.clipRectConstants, renderNodeData.matPropBlock, false, ref ex);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00069734 File Offset: 0x00067934
		private static void OnRegisterIntermediateRenderers(Camera camera)
		{
			int num = 0;
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
				Panel value = keyValuePair.Value;
				UIRRepaintUpdater uirrepaintUpdater = value.GetUpdater(VisualTreeUpdatePhase.Repaint) as UIRRepaintUpdater;
				RenderChain renderChain = ((uirrepaintUpdater != null) ? uirrepaintUpdater.renderChain : null);
				bool flag = renderChain == null || renderChain.m_StaticIndex < 0 || renderChain.m_FirstCommand == null;
				if (!flag)
				{
					BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)value;
					Material standardWorldSpaceMaterial = renderChain.GetStandardWorldSpaceMaterial();
					RenderChain.RenderNodeData renderNodeData = default(RenderChain.RenderNodeData);
					renderNodeData.device = renderChain.device;
					renderNodeData.standardMaterial = standardWorldSpaceMaterial;
					VectorImageManager vectorImageManager = renderChain.vectorImageManager;
					renderNodeData.vectorAtlas = ((vectorImageManager != null) ? vectorImageManager.atlas : null);
					renderNodeData.shaderInfoAtlas = renderChain.shaderInfoAllocator.atlas;
					renderNodeData.dpiScale = baseRuntimePanel.scaledPixelsPerPoint;
					renderNodeData.transformConstants = renderChain.shaderInfoAllocator.transformConstants;
					renderNodeData.clipRectConstants = renderChain.shaderInfoAllocator.clipRectConstants;
					bool flag2 = renderChain.m_CustomMaterialCommands == 0;
					if (flag2)
					{
						renderNodeData.initialMaterial = standardWorldSpaceMaterial;
						renderNodeData.firstCommand = renderChain.m_FirstCommand;
						RenderChain.OnRegisterIntermediateRendererMat(baseRuntimePanel, renderChain, ref renderNodeData, camera, num++);
					}
					else
					{
						Material material = null;
						RenderChainCommand renderChainCommand = renderChain.m_FirstCommand;
						RenderChainCommand renderChainCommand2 = renderChainCommand;
						while (renderChainCommand != null)
						{
							bool flag3 = renderChainCommand.type > CommandType.Draw;
							if (flag3)
							{
								renderChainCommand = renderChainCommand.next;
							}
							else
							{
								Material material2 = ((renderChainCommand.state.material == null) ? standardWorldSpaceMaterial : renderChainCommand.state.material);
								bool flag4 = material2 != material;
								if (flag4)
								{
									bool flag5 = material != null;
									if (flag5)
									{
										renderNodeData.initialMaterial = material;
										renderNodeData.firstCommand = renderChainCommand2;
										RenderChain.OnRegisterIntermediateRendererMat(baseRuntimePanel, renderChain, ref renderNodeData, camera, num++);
										renderChainCommand2 = renderChainCommand;
									}
									material = material2;
								}
								renderChainCommand = renderChainCommand.next;
							}
						}
						bool flag6 = renderChainCommand2 != null;
						if (flag6)
						{
							renderNodeData.initialMaterial = material;
							renderNodeData.firstCommand = renderChainCommand2;
							RenderChain.OnRegisterIntermediateRendererMat(baseRuntimePanel, renderChain, ref renderNodeData, camera, num++);
						}
					}
				}
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00069960 File Offset: 0x00067B60
		private unsafe static void OnRegisterIntermediateRendererMat(BaseRuntimePanel rtp, RenderChain renderChain, ref RenderChain.RenderNodeData rnd, Camera camera, int sameDistanceSortPriority)
		{
			int activeRenderNodes = renderChain.m_ActiveRenderNodes;
			renderChain.m_ActiveRenderNodes = activeRenderNodes + 1;
			int num = activeRenderNodes;
			bool flag = num < renderChain.m_RenderNodesData.Count;
			if (flag)
			{
				RenderChain.RenderNodeData renderNodeData = renderChain.m_RenderNodesData[num];
				rnd.matPropBlock = renderNodeData.matPropBlock;
				renderChain.m_RenderNodesData[num] = rnd;
			}
			else
			{
				rnd.matPropBlock = new MaterialPropertyBlock();
				num = renderChain.m_RenderNodesData.Count;
				renderChain.m_RenderNodesData.Add(rnd);
			}
			int* ptr = stackalloc int[(UIntPtr)8];
			*ptr = renderChain.m_StaticIndex;
			ptr[1] = num;
			Utility.RegisterIntermediateRenderer(camera, rnd.initialMaterial, rtp.panelToWorld, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue)), 3, 0, false, sameDistanceSortPriority, (ulong)((long)camera.cullingMask), 2, new IntPtr((void*)ptr), 8);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00069A48 File Offset: 0x00067C48
		internal void RepaintTexturedElements()
		{
			RenderChainCommand firstCommand = this.m_FirstCommand;
			for (VisualElement visualElement = RenderChain.GetFirstElementInPanel((firstCommand != null) ? firstCommand.owner : null); visualElement != null; visualElement = visualElement.renderChainData.next)
			{
				bool flag = visualElement.renderChainData.textures != null;
				if (flag)
				{
					this.UIEOnVisualsChanged(visualElement, false);
				}
			}
			this.UIEOnOpacityChanged(this.panel.visualTree, false);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00069AB4 File Offset: 0x00067CB4
		private void OnFontReset(Font font)
		{
			this.m_FontWasReset = true;
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00069AC0 File Offset: 0x00067CC0
		public void AppendTexture(VisualElement ve, Texture src, TextureId id, bool isAtlas)
		{
			BasicNode<TextureEntry> basicNode = this.m_TexturePool.Get();
			basicNode.data.source = src;
			basicNode.data.actual = id;
			basicNode.data.replaced = isAtlas;
			basicNode.AppendTo(ref ve.renderChainData.textures);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00069B14 File Offset: 0x00067D14
		public void ResetTextures(VisualElement ve)
		{
			AtlasBase atlas = this.atlas;
			TextureRegistry textureRegistry = this.m_TextureRegistry;
			BasicNodePool<TextureEntry> texturePool = this.m_TexturePool;
			BasicNode<TextureEntry> basicNode = ve.renderChainData.textures;
			ve.renderChainData.textures = null;
			while (basicNode != null)
			{
				BasicNode<TextureEntry> next = basicNode.next;
				bool replaced = basicNode.data.replaced;
				if (replaced)
				{
					atlas.ReturnAtlas(ve, basicNode.data.source as Texture2D, basicNode.data.actual);
				}
				else
				{
					textureRegistry.Release(basicNode.data.actual);
				}
				texturePool.Return(basicNode);
				basicNode = next;
			}
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00069BB8 File Offset: 0x00067DB8
		private void DrawStats()
		{
			bool flag = this.device != null;
			float num = 12f;
			Rect rect = new Rect(30f, 60f, 1000f, 100f);
			GUI.Box(new Rect(20f, 40f, 200f, (float)(flag ? 380 : 256)), "UI Toolkit Draw Stats");
			GUI.Label(rect, "Elements added\t: " + this.m_Stats.elementsAdded.ToString());
			rect.y += num;
			GUI.Label(rect, "Elements removed\t: " + this.m_Stats.elementsRemoved.ToString());
			rect.y += num;
			GUI.Label(rect, "Mesh allocs allocated\t: " + this.m_Stats.newMeshAllocations.ToString());
			rect.y += num;
			GUI.Label(rect, "Mesh allocs updated\t: " + this.m_Stats.updatedMeshAllocations.ToString());
			rect.y += num;
			GUI.Label(rect, "Clip update roots\t: " + this.m_Stats.recursiveClipUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Clip update total\t: " + this.m_Stats.recursiveClipUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Opacity update roots\t: " + this.m_Stats.recursiveOpacityUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Opacity update total\t: " + this.m_Stats.recursiveOpacityUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Xform update roots\t: " + this.m_Stats.recursiveTransformUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Xform update total\t: " + this.m_Stats.recursiveTransformUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by bone\t: " + this.m_Stats.boneTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by skipping\t: " + this.m_Stats.skipTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by nudging\t: " + this.m_Stats.nudgeTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by repaint\t: " + this.m_Stats.visualUpdateTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Visual update roots\t: " + this.m_Stats.recursiveVisualUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Visual update total\t: " + this.m_Stats.recursiveVisualUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Visual update flats\t: " + this.m_Stats.nonRecursiveVisualUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Dirty processed\t: " + this.m_Stats.dirtyProcessed.ToString());
			rect.y += num;
			GUI.Label(rect, "Group-xform updates\t: " + this.m_Stats.groupTransformElementsChanged.ToString());
			rect.y += num;
			GUI.Label(rect, "Text regens\t: " + this.m_Stats.textUpdates.ToString());
			rect.y += num;
			bool flag2 = !flag;
			if (!flag2)
			{
				rect.y += num;
				UIRenderDevice.DrawStatistics drawStatistics = this.device.GatherDrawStatistics();
				GUI.Label(rect, "Frame index\t: " + drawStatistics.currentFrameIndex.ToString());
				rect.y += num;
				GUI.Label(rect, "Command count\t: " + drawStatistics.commandCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Draw commands\t: " + drawStatistics.drawCommandCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Draw ranges\t: " + drawStatistics.drawRangeCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Draw range calls\t: " + drawStatistics.drawRangeCallCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Material sets\t: " + drawStatistics.materialSetCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Stencil changes\t: " + drawStatistics.stencilRefChanges.ToString());
				rect.y += num;
				GUI.Label(rect, "Immediate draws\t: " + drawStatistics.immediateDraws.ToString());
				rect.y += num;
				GUI.Label(rect, "Total triangles\t: " + (drawStatistics.totalIndices / 3U).ToString());
				rect.y += num;
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0006A1BC File Offset: 0x000683BC
		private static VisualElement GetFirstElementInPanel(VisualElement ve)
		{
			for (;;)
			{
				bool flag;
				if (ve != null)
				{
					VisualElement prev = ve.renderChainData.prev;
					flag = prev != null && prev.renderChainData.isInChain;
				}
				else
				{
					flag = false;
				}
				if (!flag)
				{
					break;
				}
				ve = ve.renderChainData.prev;
			}
			return ve;
		}

		// Token: 0x04000B0E RID: 2830
		private RenderChainCommand m_FirstCommand;

		// Token: 0x04000B0F RID: 2831
		private RenderChain.DepthOrderedDirtyTracking m_DirtyTracker;

		// Token: 0x04000B10 RID: 2832
		private LinkedPool<RenderChainCommand> m_CommandPool = new LinkedPool<RenderChainCommand>(() => new RenderChainCommand(), delegate(RenderChainCommand cmd)
		{
		}, 10000);

		// Token: 0x04000B11 RID: 2833
		private BasicNodePool<TextureEntry> m_TexturePool = new BasicNodePool<TextureEntry>();

		// Token: 0x04000B12 RID: 2834
		private List<RenderChain.RenderNodeData> m_RenderNodesData = new List<RenderChain.RenderNodeData>();

		// Token: 0x04000B13 RID: 2835
		private Shader m_DefaultShader;

		// Token: 0x04000B14 RID: 2836
		private Shader m_DefaultWorldSpaceShader;

		// Token: 0x04000B15 RID: 2837
		private Material m_DefaultMat;

		// Token: 0x04000B16 RID: 2838
		private Material m_DefaultWorldSpaceMat;

		// Token: 0x04000B17 RID: 2839
		private bool m_BlockDirtyRegistration;

		// Token: 0x04000B18 RID: 2840
		private int m_StaticIndex = -1;

		// Token: 0x04000B19 RID: 2841
		private int m_ActiveRenderNodes = 0;

		// Token: 0x04000B1A RID: 2842
		private int m_CustomMaterialCommands = 0;

		// Token: 0x04000B1B RID: 2843
		private ChainBuilderStats m_Stats;

		// Token: 0x04000B1C RID: 2844
		private uint m_StatsElementsAdded;

		// Token: 0x04000B1D RID: 2845
		private uint m_StatsElementsRemoved;

		// Token: 0x04000B1E RID: 2846
		private VisualElement m_FirstTextElement;

		// Token: 0x04000B1F RID: 2847
		private UIRTextUpdatePainter m_TextUpdatePainter;

		// Token: 0x04000B20 RID: 2848
		private int m_TextElementCount;

		// Token: 0x04000B21 RID: 2849
		private int m_DirtyTextStartIndex;

		// Token: 0x04000B22 RID: 2850
		private int m_DirtyTextRemaining;

		// Token: 0x04000B23 RID: 2851
		private bool m_FontWasReset;

		// Token: 0x04000B24 RID: 2852
		private Dictionary<VisualElement, Vector2> m_LastGroupTransformElementScale = new Dictionary<VisualElement, Vector2>();

		// Token: 0x04000B25 RID: 2853
		private TextureRegistry m_TextureRegistry = TextureRegistry.instance;

		// Token: 0x04000B26 RID: 2854
		private static ProfilerMarker s_MarkerProcess = new ProfilerMarker("RenderChain.Process");

		// Token: 0x04000B27 RID: 2855
		private static ProfilerMarker s_MarkerClipProcessing = new ProfilerMarker("RenderChain.UpdateClips");

		// Token: 0x04000B28 RID: 2856
		private static ProfilerMarker s_MarkerOpacityProcessing = new ProfilerMarker("RenderChain.UpdateOpacity");

		// Token: 0x04000B29 RID: 2857
		private static ProfilerMarker s_MarkerColorsProcessing = new ProfilerMarker("RenderChain.UpdateColors");

		// Token: 0x04000B2A RID: 2858
		private static ProfilerMarker s_MarkerTransformProcessing = new ProfilerMarker("RenderChain.UpdateTransforms");

		// Token: 0x04000B2B RID: 2859
		private static ProfilerMarker s_MarkerVisualsProcessing = new ProfilerMarker("RenderChain.UpdateVisuals");

		// Token: 0x04000B2C RID: 2860
		private static ProfilerMarker s_MarkerTextRegen = new ProfilerMarker("RenderChain.RegenText");

		// Token: 0x04000B2E RID: 2862
		internal static Action OnPreRender = null;

		// Token: 0x04000B33 RID: 2867
		internal UIRVEShaderInfoAllocator shaderInfoAllocator;

		// Token: 0x0200030B RID: 779
		private struct DepthOrderedDirtyTracking
		{
			// Token: 0x0600198D RID: 6541 RVA: 0x0006A204 File Offset: 0x00068404
			public void EnsureFits(int maxDepth)
			{
				while (this.heads.Count <= maxDepth)
				{
					this.heads.Add(null);
					this.tails.Add(null);
				}
			}

			// Token: 0x0600198E RID: 6542 RVA: 0x0006A248 File Offset: 0x00068448
			public void RegisterDirty(VisualElement ve, RenderDataDirtyTypes dirtyTypes, RenderDataDirtyTypeClasses dirtyTypeClass)
			{
				Debug.Assert(dirtyTypes > RenderDataDirtyTypes.None);
				int hierarchyDepth = ve.renderChainData.hierarchyDepth;
				this.minDepths[(int)dirtyTypeClass] = ((hierarchyDepth < this.minDepths[(int)dirtyTypeClass]) ? hierarchyDepth : this.minDepths[(int)dirtyTypeClass]);
				this.maxDepths[(int)dirtyTypeClass] = ((hierarchyDepth > this.maxDepths[(int)dirtyTypeClass]) ? hierarchyDepth : this.maxDepths[(int)dirtyTypeClass]);
				bool flag = ve.renderChainData.dirtiedValues > RenderDataDirtyTypes.None;
				if (flag)
				{
					ve.renderChainData.dirtiedValues = ve.renderChainData.dirtiedValues | dirtyTypes;
				}
				else
				{
					ve.renderChainData.dirtiedValues = dirtyTypes;
					bool flag2 = this.tails[hierarchyDepth] != null;
					if (flag2)
					{
						this.tails[hierarchyDepth].renderChainData.nextDirty = ve;
						ve.renderChainData.prevDirty = this.tails[hierarchyDepth];
						this.tails[hierarchyDepth] = ve;
					}
					else
					{
						List<VisualElement> list = this.heads;
						int num = hierarchyDepth;
						this.tails[hierarchyDepth] = ve;
						list[num] = ve;
					}
				}
			}

			// Token: 0x0600198F RID: 6543 RVA: 0x0006A350 File Offset: 0x00068550
			public void ClearDirty(VisualElement ve, RenderDataDirtyTypes dirtyTypesInverse)
			{
				Debug.Assert(ve.renderChainData.dirtiedValues > RenderDataDirtyTypes.None);
				ve.renderChainData.dirtiedValues = ve.renderChainData.dirtiedValues & dirtyTypesInverse;
				bool flag = ve.renderChainData.dirtiedValues == RenderDataDirtyTypes.None;
				if (flag)
				{
					bool flag2 = ve.renderChainData.prevDirty != null;
					if (flag2)
					{
						ve.renderChainData.prevDirty.renderChainData.nextDirty = ve.renderChainData.nextDirty;
					}
					bool flag3 = ve.renderChainData.nextDirty != null;
					if (flag3)
					{
						ve.renderChainData.nextDirty.renderChainData.prevDirty = ve.renderChainData.prevDirty;
					}
					bool flag4 = this.tails[ve.renderChainData.hierarchyDepth] == ve;
					if (flag4)
					{
						Debug.Assert(ve.renderChainData.nextDirty == null);
						this.tails[ve.renderChainData.hierarchyDepth] = ve.renderChainData.prevDirty;
					}
					bool flag5 = this.heads[ve.renderChainData.hierarchyDepth] == ve;
					if (flag5)
					{
						Debug.Assert(ve.renderChainData.prevDirty == null);
						this.heads[ve.renderChainData.hierarchyDepth] = ve.renderChainData.nextDirty;
					}
					ve.renderChainData.prevDirty = (ve.renderChainData.nextDirty = null);
				}
			}

			// Token: 0x06001990 RID: 6544 RVA: 0x0006A4C8 File Offset: 0x000686C8
			public void Reset()
			{
				for (int i = 0; i < this.minDepths.Length; i++)
				{
					this.minDepths[i] = int.MaxValue;
					this.maxDepths[i] = int.MinValue;
				}
			}

			// Token: 0x04000B37 RID: 2871
			public List<VisualElement> heads;

			// Token: 0x04000B38 RID: 2872
			public List<VisualElement> tails;

			// Token: 0x04000B39 RID: 2873
			public int[] minDepths;

			// Token: 0x04000B3A RID: 2874
			public int[] maxDepths;

			// Token: 0x04000B3B RID: 2875
			public uint dirtyID;
		}

		// Token: 0x0200030C RID: 780
		private struct RenderChainStaticIndexAllocator
		{
			// Token: 0x06001991 RID: 6545 RVA: 0x0006A50C File Offset: 0x0006870C
			public static int AllocateIndex(RenderChain renderChain)
			{
				int num = RenderChain.RenderChainStaticIndexAllocator.renderChains.IndexOf(null);
				bool flag = num >= 0;
				if (flag)
				{
					RenderChain.RenderChainStaticIndexAllocator.renderChains[num] = renderChain;
				}
				else
				{
					num = RenderChain.RenderChainStaticIndexAllocator.renderChains.Count;
					RenderChain.RenderChainStaticIndexAllocator.renderChains.Add(renderChain);
				}
				return num;
			}

			// Token: 0x06001992 RID: 6546 RVA: 0x0006A55E File Offset: 0x0006875E
			public static void FreeIndex(int index)
			{
				RenderChain.RenderChainStaticIndexAllocator.renderChains[index] = null;
			}

			// Token: 0x06001993 RID: 6547 RVA: 0x0006A570 File Offset: 0x00068770
			public static RenderChain AccessIndex(int index)
			{
				return RenderChain.RenderChainStaticIndexAllocator.renderChains[index];
			}

			// Token: 0x04000B3C RID: 2876
			private static List<RenderChain> renderChains = new List<RenderChain>(4);
		}

		// Token: 0x0200030D RID: 781
		private struct RenderNodeData
		{
			// Token: 0x04000B3D RID: 2877
			public Material standardMaterial;

			// Token: 0x04000B3E RID: 2878
			public Material initialMaterial;

			// Token: 0x04000B3F RID: 2879
			public MaterialPropertyBlock matPropBlock;

			// Token: 0x04000B40 RID: 2880
			public RenderChainCommand firstCommand;

			// Token: 0x04000B41 RID: 2881
			public UIRenderDevice device;

			// Token: 0x04000B42 RID: 2882
			public Texture vectorAtlas;

			// Token: 0x04000B43 RID: 2883
			public Texture shaderInfoAtlas;

			// Token: 0x04000B44 RID: 2884
			public float dpiScale;

			// Token: 0x04000B45 RID: 2885
			public NativeSlice<Transform3x4> transformConstants;

			// Token: 0x04000B46 RID: 2886
			public NativeSlice<Vector4> clipRectConstants;
		}
	}
}
