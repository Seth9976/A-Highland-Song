using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Shapes
{
	// Token: 0x02000016 RID: 22
	public class DrawCommand : IDisposable
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00020F99 File Offset: 0x0001F199
		internal static bool IsAddingDrawCommandsToBuffer
		{
			get
			{
				return DrawCommand.drawCommandWriteNestLevel > 0;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00020FA3 File Offset: 0x0001F1A3
		internal static DrawCommand CurrentWritingCommandBuffer
		{
			get
			{
				return DrawCommand.cBuffersWriting.Peek();
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00020FB0 File Offset: 0x0001F1B0
		static DrawCommand()
		{
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(DrawCommand.OnPostRenderBuiltInRP));
			SceneManager.sceneUnloaded += delegate(Scene scene)
			{
				DrawCommand.FlushNullCameras();
			};
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00021008 File Offset: 0x0001F208
		public static void ClearAllCommands()
		{
			DrawCommand.FlushNullCameras();
			foreach (List<DrawCommand> list in DrawCommand.cBuffersRendering.Values)
			{
				list.ForEach(delegate(DrawCommand cmd)
				{
					cmd.Clear();
				});
				list.Clear();
			}
			DrawCommand.cBuffersRendering.Clear();
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00021090 File Offset: 0x0001F290
		public static void FlushNullCameras()
		{
			foreach (KeyValuePair<Camera, List<DrawCommand>> keyValuePair in DrawCommand.cBuffersRendering.Where((KeyValuePair<Camera, List<DrawCommand>> kvp) => kvp.Key == null).ToList<KeyValuePair<Camera, List<DrawCommand>>>())
			{
				keyValuePair.Value.ForEach(delegate(DrawCommand cmd)
				{
					cmd.Clear();
				});
				DrawCommand.cBuffersRendering.Remove(keyValuePair.Key);
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00021144 File Offset: 0x0001F344
		private static void RegisterCommand(DrawCommand cmd)
		{
			cmd.AddToCamera();
			List<DrawCommand> list;
			if (!DrawCommand.cBuffersRendering.TryGetValue(cmd.cam, out list))
			{
				DrawCommand.cBuffersRendering.Add(cmd.cam, list = new List<DrawCommand>());
			}
			list.Add(cmd);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002118C File Offset: 0x0001F38C
		private static void OnPostRenderBuiltInRP(Camera cam)
		{
			List<DrawCommand> list;
			if (DrawCommand.cBuffersRendering.TryGetValue(cam, out list))
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i].CheckIfRenderIsDone())
					{
						list[i].Clear();
						list.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000211DC File Offset: 0x0001F3DC
		internal DrawCommand Initialize(Camera cam, CameraEvent cameraEvent = CameraEvent.BeforeImageEffects)
		{
			this.cam = cam;
			this.hasValidCamera = cam != null;
			if (!this.hasValidCamera)
			{
				Debug.LogWarning("null camera passed into DrawCommand, nothing will be drawn");
			}
			this.camEvt = cameraEvent;
			this.name = "Shapes Draw Command";
			DrawCommand.cBuffersWriting.Push(this);
			DrawCommand.drawCommandWriteNestLevel++;
			this.pushPopState = ShapesConfig.Instance.pushPopStateInDrawCommands;
			if (this.pushPopState)
			{
				Draw.Push();
			}
			return this;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00021258 File Offset: 0x0001F458
		internal void AppendToBuffer(CommandBuffer cmd)
		{
			foreach (ShapeDrawCall shapeDrawCall in this.drawCalls)
			{
				shapeDrawCall.AddToCommandBuffer(cmd);
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x000212AC File Offset: 0x0001F4AC
		private void Clear()
		{
			this.CleanupCachedAssets();
			this.RemoveFromCamera();
			this.hasRendered = false;
			for (int i = 0; i < this.drawCalls.Count; i++)
			{
				this.drawCalls[i].Cleanup();
			}
			ListPool<ShapeDrawCall>.Free(this.drawCalls);
			ListPool<Object>.Free(this.cachedAssets);
			ObjectPool<DrawCommand>.Free(this);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00021314 File Offset: 0x0001F514
		private void CleanupCachedAssets()
		{
			foreach (Object @object in this.cachedAssets)
			{
				@object.DestroyBranched();
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00021364 File Offset: 0x0001F564
		public void Dispose()
		{
			if (IMDrawer.metaMpbPrevious != null && IMDrawer.metaMpbPrevious.HasContent)
			{
				this.drawCalls.Add(IMDrawer.metaMpbPrevious.ExtractDrawCall());
			}
			if (this.hasValidCamera)
			{
				DrawCommand.RegisterCommand(this);
			}
			DrawCommand.drawCommandWriteNestLevel--;
			DrawCommand.cBuffersWriting.Pop();
			if (this.pushPopState)
			{
				Draw.Pop();
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000213CB File Offset: 0x0001F5CB
		private bool CheckIfRenderIsDone()
		{
			if (this.hasRendered)
			{
				return true;
			}
			this.hasRendered = true;
			return false;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000213E0 File Offset: 0x0001F5E0
		private void AddToCamera()
		{
			this.cmdBuf = ObjectPool<CommandBuffer>.Alloc();
			this.cmdBuf.name = this.name;
			this.AppendToBuffer(this.cmdBuf);
			this.cam.AddCommandBuffer(this.camEvt, this.cmdBuf);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002142C File Offset: 0x0001F62C
		private void RemoveFromCamera()
		{
			if (this.cam != null)
			{
				this.cam.RemoveCommandBuffer(this.camEvt, this.cmdBuf);
			}
			this.cmdBuf.Clear();
			ObjectPool<CommandBuffer>.Free(this.cmdBuf);
		}

		// Token: 0x0400008C RID: 140
		private static int bufferID;

		// Token: 0x0400008D RID: 141
		private static int drawCommandWriteNestLevel;

		// Token: 0x0400008E RID: 142
		private static Stack<DrawCommand> cBuffersWriting = new Stack<DrawCommand>();

		// Token: 0x0400008F RID: 143
		internal static Dictionary<Camera, List<DrawCommand>> cBuffersRendering = new Dictionary<Camera, List<DrawCommand>>();

		// Token: 0x04000090 RID: 144
		private bool hasValidCamera;

		// Token: 0x04000091 RID: 145
		internal bool hasRendered;

		// Token: 0x04000092 RID: 146
		internal string name;

		// Token: 0x04000093 RID: 147
		private bool pushPopState;

		// Token: 0x04000094 RID: 148
		private Camera cam;

		// Token: 0x04000095 RID: 149
		internal readonly List<Object> cachedAssets = ListPool<Object>.Alloc();

		// Token: 0x04000096 RID: 150
		internal readonly List<ShapeDrawCall> drawCalls = ListPool<ShapeDrawCall>.Alloc();

		// Token: 0x04000097 RID: 151
		private CameraEvent camEvt;

		// Token: 0x04000098 RID: 152
		private CommandBuffer cmdBuf;
	}
}
