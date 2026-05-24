using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025F RID: 607
	internal class UIRRepaintUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x0600122E RID: 4654 RVA: 0x00047604 File Offset: 0x00045804
		public UIRRepaintUpdater()
		{
			base.panelChanged += new Action<BaseVisualElementPanel>(this.OnPanelChanged);
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00047621 File Offset: 0x00045821
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return UIRRepaintUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00047628 File Offset: 0x00045828
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x00047630 File Offset: 0x00045830
		public bool drawStats { get; set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x00047639 File Offset: 0x00045839
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x00047641 File Offset: 0x00045841
		public bool breakBatches { get; set; }

		// Token: 0x06001234 RID: 4660 RVA: 0x0004764C File Offset: 0x0004584C
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				bool flag2 = (versionChangeType & VersionChangeType.Transform) > (VersionChangeType)0;
				bool flag3 = (versionChangeType & VersionChangeType.Size) > (VersionChangeType)0;
				bool flag4 = (versionChangeType & VersionChangeType.Overflow) > (VersionChangeType)0;
				bool flag5 = (versionChangeType & VersionChangeType.BorderRadius) > (VersionChangeType)0;
				bool flag6 = (versionChangeType & VersionChangeType.BorderWidth) > (VersionChangeType)0;
				bool flag7 = (versionChangeType & VersionChangeType.RenderHints) > (VersionChangeType)0;
				bool flag8 = flag7;
				if (flag8)
				{
					this.renderChain.UIEOnRenderHintsChanged(ve);
				}
				bool flag9 = flag2 || flag3 || flag6;
				if (flag9)
				{
					this.renderChain.UIEOnTransformOrSizeChanged(ve, flag2, flag3 || flag6);
				}
				bool flag10 = flag4 || flag5;
				if (flag10)
				{
					this.renderChain.UIEOnClippingChanged(ve, false);
				}
				bool flag11 = (versionChangeType & VersionChangeType.Opacity) > (VersionChangeType)0;
				if (flag11)
				{
					this.renderChain.UIEOnOpacityChanged(ve, false);
				}
				bool flag12 = (versionChangeType & VersionChangeType.Color) > (VersionChangeType)0;
				if (flag12)
				{
					this.renderChain.UIEOnColorChanged(ve);
				}
				bool flag13 = (versionChangeType & VersionChangeType.Repaint) > (VersionChangeType)0;
				if (flag13)
				{
					this.renderChain.UIEOnVisualsChanged(ve, false);
				}
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00047754 File Offset: 0x00045954
		public override void Update()
		{
			bool flag = this.renderChain == null;
			if (flag)
			{
				this.InitRenderChain();
			}
			bool flag2 = this.renderChain == null || this.renderChain.device == null;
			if (!flag2)
			{
				this.renderChain.ProcessChanges();
				PanelClearSettings clearSettings = base.panel.clearSettings;
				bool flag3 = clearSettings.clearColor || clearSettings.clearDepthStencil;
				if (flag3)
				{
					Color color = clearSettings.color;
					color = color.RGBMultiplied(color.a);
					GL.Clear(clearSettings.clearDepthStencil, clearSettings.clearColor, color, 0.99f);
				}
				this.renderChain.drawStats = this.drawStats;
				this.renderChain.device.breakBatches = this.breakBatches;
				this.renderChain.Render();
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00047830 File Offset: 0x00045A30
		protected virtual RenderChain CreateRenderChain()
		{
			return new RenderChain(base.panel);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0004784D File Offset: 0x00045A4D
		static UIRRepaintUpdater()
		{
			Utility.GraphicsResourcesRecreate += new Action<bool>(UIRRepaintUpdater.OnGraphicsResourcesRecreate);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0004787C File Offset: 0x00045A7C
		private static void OnGraphicsResourcesRecreate(bool recreate)
		{
			bool flag = !recreate;
			if (flag)
			{
				UIRenderDevice.PrepareForGfxDeviceRecreate();
			}
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				if (recreate)
				{
					KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
					AtlasBase atlas = keyValuePair.Value.atlas;
					if (atlas != null)
					{
						atlas.Reset();
					}
				}
				else
				{
					KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
					UIRRepaintUpdater uirrepaintUpdater = keyValuePair.Value.GetUpdater(VisualTreeUpdatePhase.Repaint) as UIRRepaintUpdater;
					if (uirrepaintUpdater != null)
					{
						uirrepaintUpdater.DestroyRenderChain();
					}
				}
			}
			bool flag2 = !recreate;
			if (flag2)
			{
				UIRenderDevice.FlushAllPendingDeviceDisposes();
			}
			else
			{
				UIRenderDevice.WrapUpGfxDeviceRecreate();
			}
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00047912 File Offset: 0x00045B12
		private void OnPanelChanged(BaseVisualElementPanel obj)
		{
			this.DetachFromPanel();
			this.AttachToPanel();
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00047924 File Offset: 0x00045B24
		private void AttachToPanel()
		{
			Debug.Assert(this.attachedPanel == null);
			bool flag = base.panel == null;
			if (!flag)
			{
				this.attachedPanel = base.panel;
				this.attachedPanel.atlasChanged += new Action(this.OnPanelAtlasChanged);
				this.attachedPanel.standardShaderChanged += new Action(this.OnPanelStandardShaderChanged);
				this.attachedPanel.standardWorldSpaceShaderChanged += new Action(this.OnPanelStandardWorldSpaceShaderChanged);
				this.attachedPanel.hierarchyChanged += this.OnPanelHierarchyChanged;
			}
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000479BC File Offset: 0x00045BBC
		private void DetachFromPanel()
		{
			bool flag = this.attachedPanel == null;
			if (!flag)
			{
				this.DestroyRenderChain();
				this.attachedPanel.atlasChanged -= new Action(this.OnPanelAtlasChanged);
				this.attachedPanel.standardShaderChanged -= new Action(this.OnPanelStandardShaderChanged);
				this.attachedPanel.standardWorldSpaceShaderChanged -= new Action(this.OnPanelStandardWorldSpaceShaderChanged);
				this.attachedPanel.hierarchyChanged -= this.OnPanelHierarchyChanged;
				this.attachedPanel = null;
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00047A48 File Offset: 0x00045C48
		private void InitRenderChain()
		{
			this.renderChain = this.CreateRenderChain();
			bool flag = this.attachedPanel.visualTree != null;
			if (flag)
			{
				this.renderChain.UIEOnChildAdded(this.attachedPanel.visualTree);
			}
			this.OnPanelStandardShaderChanged();
			bool flag2 = base.panel.contextType == ContextType.Player;
			if (flag2)
			{
				this.OnPanelStandardWorldSpaceShaderChanged();
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00047AAC File Offset: 0x00045CAC
		internal void DestroyRenderChain()
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				this.renderChain.Dispose();
				this.renderChain = null;
				this.ResetAllElementsDataRecursive(this.attachedPanel.visualTree);
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00047AEE File Offset: 0x00045CEE
		private void OnPanelAtlasChanged()
		{
			this.DestroyRenderChain();
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00047AF8 File Offset: 0x00045CF8
		private void OnPanelHierarchyChanged(VisualElement ve, HierarchyChangeType changeType)
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				switch (changeType)
				{
				case HierarchyChangeType.Add:
					this.renderChain.UIEOnChildAdded(ve);
					break;
				case HierarchyChangeType.Remove:
					this.renderChain.UIEOnChildRemoving(ve);
					break;
				case HierarchyChangeType.Move:
					this.renderChain.UIEOnChildrenReordered(ve);
					break;
				}
			}
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00047B5C File Offset: 0x00045D5C
		private void OnPanelStandardShaderChanged()
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				Shader shader = base.panel.standardShader;
				bool flag2 = shader == null;
				if (flag2)
				{
					shader = Shader.Find(UIRUtility.k_DefaultShaderName);
					Debug.Assert(shader != null, "Failed to load UIElements default shader");
					bool flag3 = shader != null;
					if (flag3)
					{
						shader.hideFlags |= HideFlags.DontSaveInEditor;
					}
				}
				this.renderChain.defaultShader = shader;
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00047BD8 File Offset: 0x00045DD8
		private void OnPanelStandardWorldSpaceShaderChanged()
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				Shader shader = base.panel.standardWorldSpaceShader;
				bool flag2 = shader == null;
				if (flag2)
				{
					shader = Shader.Find(UIRUtility.k_DefaultWorldSpaceShaderName);
					Debug.Assert(shader != null, "Failed to load UIElements default world-space shader");
					bool flag3 = shader != null;
					if (flag3)
					{
						shader.hideFlags |= HideFlags.DontSaveInEditor;
					}
				}
				this.renderChain.defaultWorldSpaceShader = shader;
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00047C54 File Offset: 0x00045E54
		private void ResetAllElementsDataRecursive(VisualElement ve)
		{
			ve.renderChainData = default(RenderChainVEData);
			int i = ve.hierarchy.childCount - 1;
			while (i >= 0)
			{
				this.ResetAllElementsDataRecursive(ve.hierarchy[i--]);
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x00047CA6 File Offset: 0x00045EA6
		// (set) Token: 0x06001244 RID: 4676 RVA: 0x00047CAE File Offset: 0x00045EAE
		private protected bool disposed { protected get; private set; }

		// Token: 0x06001245 RID: 4677 RVA: 0x00047CB8 File Offset: 0x00045EB8
		protected override void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.DetachFromPanel();
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000860 RID: 2144
		private BaseVisualElementPanel attachedPanel;

		// Token: 0x04000861 RID: 2145
		internal RenderChain renderChain;

		// Token: 0x04000862 RID: 2146
		private static readonly string s_Description = "Update Rendering";

		// Token: 0x04000863 RID: 2147
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(UIRRepaintUpdater.s_Description);
	}
}
