using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000406 RID: 1030
	public static class RenderPipelineManager
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x0003AEAE File Offset: 0x000390AE
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x0003AEB5 File Offset: 0x000390B5
		public static RenderPipeline currentPipeline
		{
			get
			{
				return RenderPipelineManager.s_currentPipeline;
			}
			private set
			{
				RenderPipelineManager.s_currentPipelineType = ((value != null) ? value.GetType().ToString() : RenderPipelineManager.s_builtinPipelineName);
				RenderPipelineManager.s_currentPipeline = value;
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002316 RID: 8982 RVA: 0x0003AED8 File Offset: 0x000390D8
		// (remove) Token: 0x06002317 RID: 8983 RVA: 0x0003AF0C File Offset: 0x0003910C
		[field: DebuggerBrowsable(0)]
		public static event Action<ScriptableRenderContext, List<Camera>> beginContextRendering;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06002318 RID: 8984 RVA: 0x0003AF40 File Offset: 0x00039140
		// (remove) Token: 0x06002319 RID: 8985 RVA: 0x0003AF74 File Offset: 0x00039174
		[field: DebuggerBrowsable(0)]
		public static event Action<ScriptableRenderContext, List<Camera>> endContextRendering;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600231A RID: 8986 RVA: 0x0003AFA8 File Offset: 0x000391A8
		// (remove) Token: 0x0600231B RID: 8987 RVA: 0x0003AFDC File Offset: 0x000391DC
		[field: DebuggerBrowsable(0)]
		public static event Action<ScriptableRenderContext, Camera[]> beginFrameRendering;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x0600231C RID: 8988 RVA: 0x0003B010 File Offset: 0x00039210
		// (remove) Token: 0x0600231D RID: 8989 RVA: 0x0003B044 File Offset: 0x00039244
		[field: DebuggerBrowsable(0)]
		public static event Action<ScriptableRenderContext, Camera> beginCameraRendering;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x0600231E RID: 8990 RVA: 0x0003B078 File Offset: 0x00039278
		// (remove) Token: 0x0600231F RID: 8991 RVA: 0x0003B0AC File Offset: 0x000392AC
		[field: DebuggerBrowsable(0)]
		public static event Action<ScriptableRenderContext, Camera[]> endFrameRendering;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06002320 RID: 8992 RVA: 0x0003B0E0 File Offset: 0x000392E0
		// (remove) Token: 0x06002321 RID: 8993 RVA: 0x0003B114 File Offset: 0x00039314
		[field: DebuggerBrowsable(0)]
		public static event Action<ScriptableRenderContext, Camera> endCameraRendering;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06002322 RID: 8994 RVA: 0x0003B148 File Offset: 0x00039348
		// (remove) Token: 0x06002323 RID: 8995 RVA: 0x0003B17C File Offset: 0x0003937C
		[field: DebuggerBrowsable(0)]
		public static event Action activeRenderPipelineTypeChanged;

		// Token: 0x06002324 RID: 8996 RVA: 0x0003B1AF File Offset: 0x000393AF
		internal static void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			Action<ScriptableRenderContext, Camera[]> action = RenderPipelineManager.beginFrameRendering;
			if (action != null)
			{
				action.Invoke(context, cameras.ToArray());
			}
			Action<ScriptableRenderContext, List<Camera>> action2 = RenderPipelineManager.beginContextRendering;
			if (action2 != null)
			{
				action2.Invoke(context, cameras);
			}
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0003B1DD File Offset: 0x000393DD
		internal static void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			Action<ScriptableRenderContext, Camera> action = RenderPipelineManager.beginCameraRendering;
			if (action != null)
			{
				action.Invoke(context, camera);
			}
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0003B1F3 File Offset: 0x000393F3
		internal static void EndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			Action<ScriptableRenderContext, Camera[]> action = RenderPipelineManager.endFrameRendering;
			if (action != null)
			{
				action.Invoke(context, cameras.ToArray());
			}
			Action<ScriptableRenderContext, List<Camera>> action2 = RenderPipelineManager.endContextRendering;
			if (action2 != null)
			{
				action2.Invoke(context, cameras);
			}
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0003B221 File Offset: 0x00039421
		internal static void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			Action<ScriptableRenderContext, Camera> action = RenderPipelineManager.endCameraRendering;
			if (action != null)
			{
				action.Invoke(context, camera);
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0003B237 File Offset: 0x00039437
		[RequiredByNativeCode]
		internal static void OnActiveRenderPipelineTypeChanged()
		{
			Action action = RenderPipelineManager.activeRenderPipelineTypeChanged;
			if (action != null)
			{
				action.Invoke();
			}
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0003B24C File Offset: 0x0003944C
		[RequiredByNativeCode]
		internal static void HandleRenderPipelineChange(RenderPipelineAsset pipelineAsset)
		{
			bool flag = RenderPipelineManager.s_CurrentPipelineAsset != pipelineAsset;
			bool flag2 = flag;
			if (flag2)
			{
				RenderPipelineManager.CleanupRenderPipeline();
				RenderPipelineManager.s_CurrentPipelineAsset = pipelineAsset;
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0003B27C File Offset: 0x0003947C
		[RequiredByNativeCode]
		internal static void CleanupRenderPipeline()
		{
			bool flag = RenderPipelineManager.currentPipeline != null && !RenderPipelineManager.currentPipeline.disposed;
			if (flag)
			{
				RenderPipelineManager.currentPipeline.Dispose();
				RenderPipelineManager.s_CurrentPipelineAsset = null;
				RenderPipelineManager.currentPipeline = null;
				SupportedRenderingFeatures.active = new SupportedRenderingFeatures();
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0003B2CC File Offset: 0x000394CC
		[RequiredByNativeCode]
		private static string GetCurrentPipelineAssetType()
		{
			return RenderPipelineManager.s_currentPipelineType;
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0003B2E4 File Offset: 0x000394E4
		[RequiredByNativeCode]
		private static void DoRenderLoop_Internal(RenderPipelineAsset pipe, IntPtr loopPtr, List<Camera.RenderRequest> renderRequests)
		{
			RenderPipelineManager.PrepareRenderPipeline(pipe);
			bool flag = RenderPipelineManager.currentPipeline == null;
			if (!flag)
			{
				ScriptableRenderContext scriptableRenderContext = new ScriptableRenderContext(loopPtr);
				RenderPipelineManager.s_Cameras.Clear();
				scriptableRenderContext.GetCameras(RenderPipelineManager.s_Cameras);
				bool flag2 = renderRequests == null;
				if (flag2)
				{
					RenderPipelineManager.currentPipeline.InternalRender(scriptableRenderContext, RenderPipelineManager.s_Cameras);
				}
				else
				{
					RenderPipelineManager.currentPipeline.InternalRenderWithRequests(scriptableRenderContext, RenderPipelineManager.s_Cameras, renderRequests);
				}
				RenderPipelineManager.s_Cameras.Clear();
			}
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0003B360 File Offset: 0x00039560
		internal static void PrepareRenderPipeline(RenderPipelineAsset pipelineAsset)
		{
			RenderPipelineManager.HandleRenderPipelineChange(pipelineAsset);
			bool flag = RenderPipelineManager.s_CurrentPipelineAsset != null && (RenderPipelineManager.currentPipeline == null || RenderPipelineManager.currentPipeline.disposed);
			if (flag)
			{
				RenderPipelineManager.currentPipeline = RenderPipelineManager.s_CurrentPipelineAsset.InternalCreatePipeline();
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600232E RID: 9006 RVA: 0x0003B3AF File Offset: 0x000395AF
		public static bool pipelineSwitchCompleted
		{
			get
			{
				return RenderPipelineManager.s_CurrentPipelineAsset == GraphicsSettings.currentRenderPipeline;
			}
		}

		// Token: 0x04000D08 RID: 3336
		internal static RenderPipelineAsset s_CurrentPipelineAsset;

		// Token: 0x04000D09 RID: 3337
		private static List<Camera> s_Cameras = new List<Camera>();

		// Token: 0x04000D0A RID: 3338
		private static string s_currentPipelineType;

		// Token: 0x04000D0B RID: 3339
		private static string s_builtinPipelineName = "Built-in Pipeline";

		// Token: 0x04000D0C RID: 3340
		private static RenderPipeline s_currentPipeline = null;
	}
}
