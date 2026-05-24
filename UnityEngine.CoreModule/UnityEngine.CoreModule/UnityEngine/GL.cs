using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x0200012E RID: 302
	[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Camera/CameraUtil.h")]
	[NativeHeader("Runtime/Camera/Camera.h")]
	[NativeHeader("Runtime/GfxDevice/GfxDevice.h")]
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public sealed class GL
	{
		// Token: 0x06000956 RID: 2390
		[NativeName("ImmediateVertex")]
		[MethodImpl(4096)]
		public static extern void Vertex3(float x, float y, float z);

		// Token: 0x06000957 RID: 2391 RVA: 0x0000E680 File Offset: 0x0000C880
		public static void Vertex(Vector3 v)
		{
			GL.Vertex3(v.x, v.y, v.z);
		}

		// Token: 0x06000958 RID: 2392
		[NativeName("ImmediateVertices")]
		[MethodImpl(4096)]
		internal unsafe static extern void Vertices(Vector3* v, Vector3* coords, Vector4* colors, int length);

		// Token: 0x06000959 RID: 2393
		[NativeName("ImmediateTexCoordAll")]
		[MethodImpl(4096)]
		public static extern void TexCoord3(float x, float y, float z);

		// Token: 0x0600095A RID: 2394 RVA: 0x0000E69B File Offset: 0x0000C89B
		public static void TexCoord(Vector3 v)
		{
			GL.TexCoord3(v.x, v.y, v.z);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0000E6B6 File Offset: 0x0000C8B6
		public static void TexCoord2(float x, float y)
		{
			GL.TexCoord3(x, y, 0f);
		}

		// Token: 0x0600095C RID: 2396
		[NativeName("ImmediateTexCoord")]
		[MethodImpl(4096)]
		public static extern void MultiTexCoord3(int unit, float x, float y, float z);

		// Token: 0x0600095D RID: 2397 RVA: 0x0000E6C6 File Offset: 0x0000C8C6
		public static void MultiTexCoord(int unit, Vector3 v)
		{
			GL.MultiTexCoord3(unit, v.x, v.y, v.z);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0000E6E2 File Offset: 0x0000C8E2
		public static void MultiTexCoord2(int unit, float x, float y)
		{
			GL.MultiTexCoord3(unit, x, y, 0f);
		}

		// Token: 0x0600095F RID: 2399
		[NativeName("ImmediateColor")]
		[MethodImpl(4096)]
		private static extern void ImmediateColor(float r, float g, float b, float a);

		// Token: 0x06000960 RID: 2400 RVA: 0x0000E6F3 File Offset: 0x0000C8F3
		public static void Color(Color c)
		{
			GL.ImmediateColor(c.r, c.g, c.b, c.a);
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000961 RID: 2401
		// (set) Token: 0x06000962 RID: 2402
		public static extern bool wireframe
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000963 RID: 2403
		// (set) Token: 0x06000964 RID: 2404
		public static extern bool sRGBWrite
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000965 RID: 2405
		// (set) Token: 0x06000966 RID: 2406
		[NativeProperty("UserBackfaceMode")]
		public static extern bool invertCulling
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000967 RID: 2407
		[MethodImpl(4096)]
		public static extern void Flush();

		// Token: 0x06000968 RID: 2408
		[MethodImpl(4096)]
		public static extern void RenderTargetBarrier();

		// Token: 0x06000969 RID: 2409 RVA: 0x0000E714 File Offset: 0x0000C914
		private static Matrix4x4 GetWorldViewMatrix()
		{
			Matrix4x4 matrix4x;
			GL.GetWorldViewMatrix_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000E729 File Offset: 0x0000C929
		private static void SetViewMatrix(Matrix4x4 m)
		{
			GL.SetViewMatrix_Injected(ref m);
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0000E734 File Offset: 0x0000C934
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0000E74B File Offset: 0x0000C94B
		public static Matrix4x4 modelview
		{
			get
			{
				return GL.GetWorldViewMatrix();
			}
			set
			{
				GL.SetViewMatrix(value);
			}
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0000E755 File Offset: 0x0000C955
		[NativeName("SetWorldMatrix")]
		public static void MultMatrix(Matrix4x4 m)
		{
			GL.MultMatrix_Injected(ref m);
		}

		// Token: 0x0600096E RID: 2414
		[Obsolete("IssuePluginEvent(eventID) is deprecated. Use IssuePluginEvent(callback, eventID) instead.", false)]
		[NativeName("InsertCustomMarker")]
		[MethodImpl(4096)]
		public static extern void IssuePluginEvent(int eventID);

		// Token: 0x0600096F RID: 2415
		[NativeName("SetUserBackfaceMode")]
		[Obsolete("SetRevertBackfacing(revertBackFaces) is deprecated. Use invertCulling property instead.", false)]
		[MethodImpl(4096)]
		public static extern void SetRevertBackfacing(bool revertBackFaces);

		// Token: 0x06000970 RID: 2416
		[FreeFunction("GLPushMatrixScript")]
		[MethodImpl(4096)]
		public static extern void PushMatrix();

		// Token: 0x06000971 RID: 2417
		[FreeFunction("GLPopMatrixScript")]
		[MethodImpl(4096)]
		public static extern void PopMatrix();

		// Token: 0x06000972 RID: 2418
		[FreeFunction("GLLoadIdentityScript")]
		[MethodImpl(4096)]
		public static extern void LoadIdentity();

		// Token: 0x06000973 RID: 2419
		[FreeFunction("GLLoadOrthoScript")]
		[MethodImpl(4096)]
		public static extern void LoadOrtho();

		// Token: 0x06000974 RID: 2420
		[FreeFunction("GLLoadPixelMatrixScript")]
		[MethodImpl(4096)]
		public static extern void LoadPixelMatrix();

		// Token: 0x06000975 RID: 2421 RVA: 0x0000E75E File Offset: 0x0000C95E
		[FreeFunction("GLLoadProjectionMatrixScript")]
		public static void LoadProjectionMatrix(Matrix4x4 mat)
		{
			GL.LoadProjectionMatrix_Injected(ref mat);
		}

		// Token: 0x06000976 RID: 2422
		[FreeFunction("GLInvalidateState")]
		[MethodImpl(4096)]
		public static extern void InvalidateState();

		// Token: 0x06000977 RID: 2423 RVA: 0x0000E768 File Offset: 0x0000C968
		[FreeFunction("GLGetGPUProjectionMatrix")]
		public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
		{
			Matrix4x4 matrix4x;
			GL.GetGPUProjectionMatrix_Injected(ref proj, renderIntoTexture, out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000978 RID: 2424
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern void GLLoadPixelMatrixScript(float left, float right, float bottom, float top);

		// Token: 0x06000979 RID: 2425 RVA: 0x0000E780 File Offset: 0x0000C980
		public static void LoadPixelMatrix(float left, float right, float bottom, float top)
		{
			GL.GLLoadPixelMatrixScript(left, right, bottom, top);
		}

		// Token: 0x0600097A RID: 2426
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern void GLIssuePluginEvent(IntPtr callback, int eventID);

		// Token: 0x0600097B RID: 2427 RVA: 0x0000E790 File Offset: 0x0000C990
		public static void IssuePluginEvent(IntPtr callback, int eventID)
		{
			bool flag = callback == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Null callback specified.", "callback");
			}
			GL.GLIssuePluginEvent(callback, eventID);
		}

		// Token: 0x0600097C RID: 2428
		[FreeFunction("GLBegin", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern void Begin(int mode);

		// Token: 0x0600097D RID: 2429
		[FreeFunction("GLEnd")]
		[MethodImpl(4096)]
		public static extern void End();

		// Token: 0x0600097E RID: 2430 RVA: 0x0000E7C5 File Offset: 0x0000C9C5
		[FreeFunction]
		private static void GLClear(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
		{
			GL.GLClear_Injected(clearDepth, clearColor, ref backgroundColor, depth);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000E7D1 File Offset: 0x0000C9D1
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			GL.GLClear(clearDepth, clearColor, backgroundColor, depth);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0000E7DE File Offset: 0x0000C9DE
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			GL.GLClear(clearDepth, clearColor, backgroundColor, 1f);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0000E7EF File Offset: 0x0000C9EF
		[FreeFunction("SetGLViewport")]
		public static void Viewport(Rect pixelRect)
		{
			GL.Viewport_Injected(ref pixelRect);
		}

		// Token: 0x06000982 RID: 2434
		[FreeFunction("ClearWithSkybox")]
		[MethodImpl(4096)]
		public static extern void ClearWithSkybox(bool clearDepth, Camera camera);

		// Token: 0x06000984 RID: 2436
		[MethodImpl(4096)]
		private static extern void GetWorldViewMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000985 RID: 2437
		[MethodImpl(4096)]
		private static extern void SetViewMatrix_Injected(ref Matrix4x4 m);

		// Token: 0x06000986 RID: 2438
		[MethodImpl(4096)]
		private static extern void MultMatrix_Injected(ref Matrix4x4 m);

		// Token: 0x06000987 RID: 2439
		[MethodImpl(4096)]
		private static extern void LoadProjectionMatrix_Injected(ref Matrix4x4 mat);

		// Token: 0x06000988 RID: 2440
		[MethodImpl(4096)]
		private static extern void GetGPUProjectionMatrix_Injected(ref Matrix4x4 proj, bool renderIntoTexture, out Matrix4x4 ret);

		// Token: 0x06000989 RID: 2441
		[MethodImpl(4096)]
		private static extern void GLClear_Injected(bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

		// Token: 0x0600098A RID: 2442
		[MethodImpl(4096)]
		private static extern void Viewport_Injected(ref Rect pixelRect);

		// Token: 0x040003CC RID: 972
		public const int TRIANGLES = 4;

		// Token: 0x040003CD RID: 973
		public const int TRIANGLE_STRIP = 5;

		// Token: 0x040003CE RID: 974
		public const int QUADS = 7;

		// Token: 0x040003CF RID: 975
		public const int LINES = 1;

		// Token: 0x040003D0 RID: 976
		public const int LINE_STRIP = 2;
	}
}
