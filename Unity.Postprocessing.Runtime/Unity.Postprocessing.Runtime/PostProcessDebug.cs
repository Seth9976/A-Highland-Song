using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200004B RID: 75
	[ExecuteAlways]
	[AddComponentMenu("Rendering/Post-process Debug", 1002)]
	public sealed class PostProcessDebug : MonoBehaviour
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00009B4B File Offset: 0x00007D4B
		private void OnEnable()
		{
			this.m_CmdAfterEverything = new CommandBuffer
			{
				name = "Post-processing Debug Overlay"
			};
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00009B63 File Offset: 0x00007D63
		private void OnDisable()
		{
			if (this.m_CurrentCamera != null)
			{
				this.m_CurrentCamera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, this.m_CmdAfterEverything);
			}
			this.m_CurrentCamera = null;
			this.m_PreviousPostProcessLayer = null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009B94 File Offset: 0x00007D94
		private void Update()
		{
			this.UpdateStates();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00009B9C File Offset: 0x00007D9C
		private void Reset()
		{
			this.postProcessLayer = base.GetComponent<PostProcessLayer>();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00009BAC File Offset: 0x00007DAC
		private void UpdateStates()
		{
			if (this.m_PreviousPostProcessLayer != this.postProcessLayer)
			{
				if (this.m_CurrentCamera != null)
				{
					this.m_CurrentCamera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, this.m_CmdAfterEverything);
					this.m_CurrentCamera = null;
				}
				this.m_PreviousPostProcessLayer = this.postProcessLayer;
				if (this.postProcessLayer != null)
				{
					this.m_CurrentCamera = this.postProcessLayer.GetComponent<Camera>();
					this.m_CurrentCamera.AddCommandBuffer(CameraEvent.AfterImageEffects, this.m_CmdAfterEverything);
				}
			}
			if (this.postProcessLayer == null || !this.postProcessLayer.enabled)
			{
				return;
			}
			if (this.lightMeter)
			{
				this.postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.LightMeter);
			}
			if (this.histogram)
			{
				this.postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.Histogram);
			}
			if (this.waveform)
			{
				this.postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.Waveform);
			}
			if (this.vectorscope)
			{
				this.postProcessLayer.debugLayer.RequestMonitorPass(MonitorType.Vectorscope);
			}
			this.postProcessLayer.debugLayer.RequestDebugOverlay(this.debugOverlay);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00009CC8 File Offset: 0x00007EC8
		private void OnPostRender()
		{
			this.m_CmdAfterEverything.Clear();
			if (this.postProcessLayer == null || !this.postProcessLayer.enabled || !this.postProcessLayer.debugLayer.debugOverlayActive)
			{
				return;
			}
			this.m_CmdAfterEverything.Blit(this.postProcessLayer.debugLayer.debugOverlayTarget, BuiltinRenderTextureType.CameraTarget);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00009D30 File Offset: 0x00007F30
		private void OnGUI()
		{
			if (this.postProcessLayer == null || !this.postProcessLayer.enabled)
			{
				return;
			}
			RenderTexture.active = null;
			Rect rect = new Rect(5f, 5f, 0f, 0f);
			PostProcessDebugLayer debugLayer = this.postProcessLayer.debugLayer;
			this.DrawMonitor(ref rect, debugLayer.lightMeter, this.lightMeter);
			this.DrawMonitor(ref rect, debugLayer.histogram, this.histogram);
			this.DrawMonitor(ref rect, debugLayer.waveform, this.waveform);
			this.DrawMonitor(ref rect, debugLayer.vectorscope, this.vectorscope);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00009DD8 File Offset: 0x00007FD8
		private void DrawMonitor(ref Rect rect, Monitor monitor, bool enabled)
		{
			if (!enabled || monitor.output == null)
			{
				return;
			}
			rect.width = (float)monitor.output.width;
			rect.height = (float)monitor.output.height;
			GUI.DrawTexture(rect, monitor.output);
			rect.x += (float)monitor.output.width + 5f;
		}

		// Token: 0x04000106 RID: 262
		public PostProcessLayer postProcessLayer;

		// Token: 0x04000107 RID: 263
		private PostProcessLayer m_PreviousPostProcessLayer;

		// Token: 0x04000108 RID: 264
		public bool lightMeter;

		// Token: 0x04000109 RID: 265
		public bool histogram;

		// Token: 0x0400010A RID: 266
		public bool waveform;

		// Token: 0x0400010B RID: 267
		public bool vectorscope;

		// Token: 0x0400010C RID: 268
		public DebugOverlay debugOverlay;

		// Token: 0x0400010D RID: 269
		private Camera m_CurrentCamera;

		// Token: 0x0400010E RID: 270
		private CommandBuffer m_CmdAfterEverything;
	}
}
