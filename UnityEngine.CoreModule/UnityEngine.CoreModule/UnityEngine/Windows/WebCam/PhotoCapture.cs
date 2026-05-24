using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Windows.WebCam
{
	// Token: 0x0200029D RID: 669
	[NativeHeader("PlatformDependent/Win/Webcam/PhotoCapture.h")]
	[StaticAccessor("PhotoCapture", StaticAccessorType.DoubleColon)]
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	[StructLayout(0)]
	public class PhotoCapture : IDisposable
	{
		// Token: 0x06001C83 RID: 7299 RVA: 0x0002DBB8 File Offset: 0x0002BDB8
		private static PhotoCapture.PhotoCaptureResult MakeCaptureResult(PhotoCapture.CaptureResultType resultType, long hResult)
		{
			return new PhotoCapture.PhotoCaptureResult
			{
				resultType = resultType,
				hResult = hResult
			};
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x0002DBE4 File Offset: 0x0002BDE4
		private static PhotoCapture.PhotoCaptureResult MakeCaptureResult(long hResult)
		{
			PhotoCapture.PhotoCaptureResult photoCaptureResult = default(PhotoCapture.PhotoCaptureResult);
			bool flag = hResult == PhotoCapture.HR_SUCCESS;
			PhotoCapture.CaptureResultType captureResultType;
			if (flag)
			{
				captureResultType = PhotoCapture.CaptureResultType.Success;
			}
			else
			{
				captureResultType = PhotoCapture.CaptureResultType.UnknownError;
			}
			photoCaptureResult.resultType = captureResultType;
			photoCaptureResult.hResult = hResult;
			return photoCaptureResult;
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0002DC28 File Offset: 0x0002BE28
		public static IEnumerable<Resolution> SupportedResolutions
		{
			get
			{
				bool flag = PhotoCapture.s_SupportedResolutions == null;
				if (flag)
				{
					PhotoCapture.s_SupportedResolutions = PhotoCapture.GetSupportedResolutions_Internal();
				}
				return PhotoCapture.s_SupportedResolutions;
			}
		}

		// Token: 0x06001C86 RID: 7302
		[NativeName("GetSupportedResolutions")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		private static extern Resolution[] GetSupportedResolutions_Internal();

		// Token: 0x06001C87 RID: 7303 RVA: 0x0002DC58 File Offset: 0x0002BE58
		public static void CreateAsync(bool showHolograms, PhotoCapture.OnCaptureResourceCreatedCallback onCreatedCallback)
		{
			bool flag = onCreatedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCreatedCallback");
			}
			PhotoCapture.Instantiate_Internal(showHolograms, onCreatedCallback);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x0002DC84 File Offset: 0x0002BE84
		public static void CreateAsync(PhotoCapture.OnCaptureResourceCreatedCallback onCreatedCallback)
		{
			bool flag = onCreatedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCreatedCallback");
			}
			PhotoCapture.Instantiate_Internal(false, onCreatedCallback);
		}

		// Token: 0x06001C89 RID: 7305
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("Instantiate")]
		[MethodImpl(4096)]
		private static extern IntPtr Instantiate_Internal(bool showHolograms, PhotoCapture.OnCaptureResourceCreatedCallback onCreatedCallback);

		// Token: 0x06001C8A RID: 7306 RVA: 0x0002DCB0 File Offset: 0x0002BEB0
		[RequiredByNativeCode]
		private static void InvokeOnCreatedResourceDelegate(PhotoCapture.OnCaptureResourceCreatedCallback callback, IntPtr nativePtr)
		{
			bool flag = nativePtr == IntPtr.Zero;
			if (flag)
			{
				callback(null);
			}
			else
			{
				callback(new PhotoCapture(nativePtr));
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x0002DCE8 File Offset: 0x0002BEE8
		private PhotoCapture(IntPtr nativeCaptureObject)
		{
			this.m_NativePtr = nativeCaptureObject;
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0002DCFC File Offset: 0x0002BEFC
		public void StartPhotoModeAsync(CameraParameters setupParams, PhotoCapture.OnPhotoModeStartedCallback onPhotoModeStartedCallback)
		{
			bool flag = onPhotoModeStartedCallback == null;
			if (flag)
			{
				throw new ArgumentException("onPhotoModeStartedCallback");
			}
			bool flag2 = setupParams.cameraResolutionWidth == 0 || setupParams.cameraResolutionHeight == 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("setupParams", "The camera resolution must be set to a supported resolution.");
			}
			this.StartPhotoMode_Internal(setupParams, onPhotoModeStartedCallback);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0002DD52 File Offset: 0x0002BF52
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("StartPhotoMode")]
		private void StartPhotoMode_Internal(CameraParameters setupParams, PhotoCapture.OnPhotoModeStartedCallback onPhotoModeStartedCallback)
		{
			this.StartPhotoMode_Internal_Injected(ref setupParams, onPhotoModeStartedCallback);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0002DD5D File Offset: 0x0002BF5D
		[RequiredByNativeCode]
		private static void InvokeOnPhotoModeStartedDelegate(PhotoCapture.OnPhotoModeStartedCallback callback, long hResult)
		{
			callback(PhotoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001C8F RID: 7311
		[NativeName("StopPhotoMode")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		public extern void StopPhotoModeAsync(PhotoCapture.OnPhotoModeStoppedCallback onPhotoModeStoppedCallback);

		// Token: 0x06001C90 RID: 7312 RVA: 0x0002DD6D File Offset: 0x0002BF6D
		[RequiredByNativeCode]
		private static void InvokeOnPhotoModeStoppedDelegate(PhotoCapture.OnPhotoModeStoppedCallback callback, long hResult)
		{
			callback(PhotoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0002DD80 File Offset: 0x0002BF80
		public void TakePhotoAsync(string filename, PhotoCaptureFileOutputFormat fileOutputFormat, PhotoCapture.OnCapturedToDiskCallback onCapturedPhotoToDiskCallback)
		{
			bool flag = onCapturedPhotoToDiskCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCapturedPhotoToDiskCallback");
			}
			bool flag2 = string.IsNullOrEmpty(filename);
			if (flag2)
			{
				throw new ArgumentNullException("filename");
			}
			filename = filename.Replace("/", "\\");
			string directoryName = Path.GetDirectoryName(filename);
			bool flag3 = !string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName);
			if (flag3)
			{
				throw new ArgumentException("The specified directory does not exist.", "filename");
			}
			FileInfo fileInfo = new FileInfo(filename);
			bool flag4 = fileInfo.Exists && fileInfo.IsReadOnly;
			if (flag4)
			{
				throw new ArgumentException("Cannot write to the file because it is read-only.", "filename");
			}
			this.CapturePhotoToDisk_Internal(filename, fileOutputFormat, onCapturedPhotoToDiskCallback);
		}

		// Token: 0x06001C92 RID: 7314
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("CapturePhotoToDisk")]
		[MethodImpl(4096)]
		private extern void CapturePhotoToDisk_Internal(string filename, PhotoCaptureFileOutputFormat fileOutputFormat, PhotoCapture.OnCapturedToDiskCallback onCapturedPhotoToDiskCallback);

		// Token: 0x06001C93 RID: 7315 RVA: 0x0002DE35 File Offset: 0x0002C035
		[RequiredByNativeCode]
		private static void InvokeOnCapturedPhotoToDiskDelegate(PhotoCapture.OnCapturedToDiskCallback callback, long hResult)
		{
			callback(PhotoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x0002DE48 File Offset: 0x0002C048
		public void TakePhotoAsync(PhotoCapture.OnCapturedToMemoryCallback onCapturedPhotoToMemoryCallback)
		{
			bool flag = onCapturedPhotoToMemoryCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCapturedPhotoToMemoryCallback");
			}
			this.CapturePhotoToMemory_Internal(onCapturedPhotoToMemoryCallback);
		}

		// Token: 0x06001C95 RID: 7317
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("CapturePhotoToMemory")]
		[MethodImpl(4096)]
		private extern void CapturePhotoToMemory_Internal(PhotoCapture.OnCapturedToMemoryCallback onCapturedPhotoToMemoryCallback);

		// Token: 0x06001C96 RID: 7318 RVA: 0x0002DE74 File Offset: 0x0002C074
		[RequiredByNativeCode]
		private static void InvokeOnCapturedPhotoToMemoryDelegate(PhotoCapture.OnCapturedToMemoryCallback callback, long hResult, IntPtr photoCaptureFramePtr)
		{
			PhotoCaptureFrame photoCaptureFrame = null;
			bool flag = photoCaptureFramePtr != IntPtr.Zero;
			if (flag)
			{
				photoCaptureFrame = new PhotoCaptureFrame(photoCaptureFramePtr);
			}
			callback(PhotoCapture.MakeCaptureResult(hResult), photoCaptureFrame);
		}

		// Token: 0x06001C97 RID: 7319
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("GetUnsafePointerToVideoDeviceController")]
		[ThreadAndSerializationSafe]
		[MethodImpl(4096)]
		public extern IntPtr GetUnsafePointerToVideoDeviceController();

		// Token: 0x06001C98 RID: 7320 RVA: 0x0002DEAC File Offset: 0x0002C0AC
		public void Dispose()
		{
			bool flag = this.m_NativePtr != IntPtr.Zero;
			if (flag)
			{
				this.Dispose_Internal();
				this.m_NativePtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001C99 RID: 7321
		[NativeName("Dispose")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		private extern void Dispose_Internal();

		// Token: 0x06001C9A RID: 7322 RVA: 0x0002DEEC File Offset: 0x0002C0EC
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_NativePtr != IntPtr.Zero;
				if (flag)
				{
					this.DisposeThreaded_Internal();
					this.m_NativePtr = IntPtr.Zero;
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001C9B RID: 7323
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("DisposeThreaded")]
		[MethodImpl(4096)]
		private extern void DisposeThreaded_Internal();

		// Token: 0x06001C9C RID: 7324
		[MethodImpl(4096)]
		private extern void StartPhotoMode_Internal_Injected(ref CameraParameters setupParams, PhotoCapture.OnPhotoModeStartedCallback onPhotoModeStartedCallback);

		// Token: 0x0400095B RID: 2395
		internal IntPtr m_NativePtr;

		// Token: 0x0400095C RID: 2396
		private static Resolution[] s_SupportedResolutions;

		// Token: 0x0400095D RID: 2397
		private static readonly long HR_SUCCESS;

		// Token: 0x0200029E RID: 670
		public enum CaptureResultType
		{
			// Token: 0x0400095F RID: 2399
			Success,
			// Token: 0x04000960 RID: 2400
			UnknownError
		}

		// Token: 0x0200029F RID: 671
		public struct PhotoCaptureResult
		{
			// Token: 0x170005AD RID: 1453
			// (get) Token: 0x06001C9D RID: 7325 RVA: 0x0002DF40 File Offset: 0x0002C140
			public bool success
			{
				get
				{
					return this.resultType == PhotoCapture.CaptureResultType.Success;
				}
			}

			// Token: 0x04000961 RID: 2401
			public PhotoCapture.CaptureResultType resultType;

			// Token: 0x04000962 RID: 2402
			public long hResult;
		}

		// Token: 0x020002A0 RID: 672
		// (Invoke) Token: 0x06001C9F RID: 7327
		public delegate void OnCaptureResourceCreatedCallback(PhotoCapture captureObject);

		// Token: 0x020002A1 RID: 673
		// (Invoke) Token: 0x06001CA3 RID: 7331
		public delegate void OnPhotoModeStartedCallback(PhotoCapture.PhotoCaptureResult result);

		// Token: 0x020002A2 RID: 674
		// (Invoke) Token: 0x06001CA7 RID: 7335
		public delegate void OnPhotoModeStoppedCallback(PhotoCapture.PhotoCaptureResult result);

		// Token: 0x020002A3 RID: 675
		// (Invoke) Token: 0x06001CAB RID: 7339
		public delegate void OnCapturedToDiskCallback(PhotoCapture.PhotoCaptureResult result);

		// Token: 0x020002A4 RID: 676
		// (Invoke) Token: 0x06001CAF RID: 7343
		public delegate void OnCapturedToMemoryCallback(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame);
	}
}
