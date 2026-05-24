using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Apple
{
	// Token: 0x0200048A RID: 1162
	[NativeConditional("PLATFORM_IOS || PLATFORM_TVOS || PLATFORM_OSX")]
	[NativeHeader("Runtime/Export/Apple/FrameCaptureMetalScriptBindings.h")]
	public class FrameCapture
	{
		// Token: 0x06002926 RID: 10534 RVA: 0x00008CAF File Offset: 0x00006EAF
		private FrameCapture()
		{
		}

		// Token: 0x06002927 RID: 10535
		[FreeFunction("FrameCaptureMetalScripting::IsDestinationSupported")]
		[MethodImpl(4096)]
		private static extern bool IsDestinationSupportedImpl(FrameCaptureDestination dest);

		// Token: 0x06002928 RID: 10536
		[FreeFunction("FrameCaptureMetalScripting::BeginCapture")]
		[MethodImpl(4096)]
		private static extern void BeginCaptureImpl(FrameCaptureDestination dest, string path);

		// Token: 0x06002929 RID: 10537
		[FreeFunction("FrameCaptureMetalScripting::EndCapture")]
		[MethodImpl(4096)]
		private static extern void EndCaptureImpl();

		// Token: 0x0600292A RID: 10538
		[FreeFunction("FrameCaptureMetalScripting::CaptureNextFrame")]
		[MethodImpl(4096)]
		private static extern void CaptureNextFrameImpl(FrameCaptureDestination dest, string path);

		// Token: 0x0600292B RID: 10539 RVA: 0x00043F50 File Offset: 0x00042150
		public static bool IsDestinationSupported(FrameCaptureDestination dest)
		{
			bool flag = dest != FrameCaptureDestination.DevTools && dest != FrameCaptureDestination.GPUTraceDocument;
			if (flag)
			{
				throw new ArgumentException("dest", "Argument dest has bad value (not one of FrameCaptureDestination enum values)");
			}
			return FrameCapture.IsDestinationSupportedImpl(dest);
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x00043F8C File Offset: 0x0004218C
		public static void BeginCaptureToXcode()
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.DevTools);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture with DevTools is not supported.");
			}
			FrameCapture.BeginCaptureImpl(FrameCaptureDestination.DevTools, null);
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x00043FBC File Offset: 0x000421BC
		public static void BeginCaptureToFile(string path)
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.GPUTraceDocument);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture to file is not supported.");
			}
			bool flag2 = string.IsNullOrEmpty(path);
			if (flag2)
			{
				throw new ArgumentException("path", "Path must be supplied when capture destination is GPUTraceDocument.");
			}
			bool flag3 = Path.GetExtension(path) != ".gputrace";
			if (flag3)
			{
				throw new ArgumentException("path", "Destination file should have .gputrace extension.");
			}
			FrameCapture.BeginCaptureImpl(FrameCaptureDestination.GPUTraceDocument, new Uri(path).AbsoluteUri);
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x00044032 File Offset: 0x00042232
		public static void EndCapture()
		{
			FrameCapture.EndCaptureImpl();
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x0004403C File Offset: 0x0004223C
		public static void CaptureNextFrameToXcode()
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.DevTools);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture with DevTools is not supported.");
			}
			FrameCapture.CaptureNextFrameImpl(FrameCaptureDestination.DevTools, null);
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0004406C File Offset: 0x0004226C
		public static void CaptureNextFrameToFile(string path)
		{
			bool flag = !FrameCapture.IsDestinationSupported(FrameCaptureDestination.GPUTraceDocument);
			if (flag)
			{
				throw new InvalidOperationException("Frame Capture to file is not supported.");
			}
			bool flag2 = string.IsNullOrEmpty(path);
			if (flag2)
			{
				throw new ArgumentException("path", "Path must be supplied when capture destination is GPUTraceDocument.");
			}
			bool flag3 = Path.GetExtension(path) != ".gputrace";
			if (flag3)
			{
				throw new ArgumentException("path", "Destination file should have .gputrace extension.");
			}
			FrameCapture.CaptureNextFrameImpl(FrameCaptureDestination.GPUTraceDocument, new Uri(path).AbsoluteUri);
		}
	}
}
