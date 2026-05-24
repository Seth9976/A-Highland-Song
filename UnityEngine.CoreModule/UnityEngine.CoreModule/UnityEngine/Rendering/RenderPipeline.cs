using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000403 RID: 1027
	public abstract class RenderPipeline
	{
		// Token: 0x060022E9 RID: 8937
		protected abstract void Render(ScriptableRenderContext context, Camera[] cameras);

		// Token: 0x060022EA RID: 8938 RVA: 0x00004557 File Offset: 0x00002757
		protected virtual void ProcessRenderRequests(ScriptableRenderContext context, Camera camera, List<Camera.RenderRequest> renderRequests)
		{
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x0003ACCD File Offset: 0x00038ECD
		protected static void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			RenderPipelineManager.BeginContextRendering(context, new List<Camera>(cameras));
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0003ACDD File Offset: 0x00038EDD
		protected static void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			RenderPipelineManager.BeginContextRendering(context, cameras);
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0003ACE8 File Offset: 0x00038EE8
		protected static void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			RenderPipelineManager.BeginCameraRendering(context, camera);
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0003ACF3 File Offset: 0x00038EF3
		protected static void EndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			RenderPipelineManager.EndContextRendering(context, cameras);
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0003ACFE File Offset: 0x00038EFE
		protected static void EndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			RenderPipelineManager.EndContextRendering(context, new List<Camera>(cameras));
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x0003AD0E File Offset: 0x00038F0E
		protected static void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			RenderPipelineManager.EndCameraRendering(context, camera);
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x0003AD19 File Offset: 0x00038F19
		protected virtual void Render(ScriptableRenderContext context, List<Camera> cameras)
		{
			this.Render(context, cameras.ToArray());
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0003AD2C File Offset: 0x00038F2C
		internal void InternalRender(ScriptableRenderContext context, List<Camera> cameras)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(string.Format("{0} has been disposed. Do not call Render on disposed a RenderPipeline.", this));
			}
			this.Render(context, cameras);
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0003AD60 File Offset: 0x00038F60
		internal void InternalRenderWithRequests(ScriptableRenderContext context, List<Camera> cameras, List<Camera.RenderRequest> renderRequests)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(string.Format("{0} has been disposed. Do not call Render on disposed a RenderPipeline.", this));
			}
			this.ProcessRenderRequests(context, (cameras == null || cameras.Count == 0) ? null : cameras[0], renderRequests);
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x0003ADA7 File Offset: 0x00038FA7
		// (set) Token: 0x060022F5 RID: 8949 RVA: 0x0003ADAF File Offset: 0x00038FAF
		public bool disposed { get; private set; }

		// Token: 0x060022F6 RID: 8950 RVA: 0x0003ADB8 File Offset: 0x00038FB8
		internal void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
			this.disposed = true;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x00004557 File Offset: 0x00002757
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0003ADD4 File Offset: 0x00038FD4
		public virtual RenderPipelineGlobalSettings defaultSettings
		{
			get
			{
				return null;
			}
		}
	}
}
