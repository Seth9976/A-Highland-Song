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
	// Token: 0x020002A6 RID: 678
	[NativeHeader("PlatformDependent/Win/Webcam/VideoCaptureBindings.h")]
	[StaticAccessor("VideoCaptureBindings", StaticAccessorType.DoubleColon)]
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	[StructLayout(0)]
	public class VideoCapture : IDisposable
	{
		// Token: 0x06001CCC RID: 7372 RVA: 0x0002E248 File Offset: 0x0002C448
		private static VideoCapture.VideoCaptureResult MakeCaptureResult(VideoCapture.CaptureResultType resultType, long hResult)
		{
			return new VideoCapture.VideoCaptureResult
			{
				resultType = resultType,
				hResult = hResult
			};
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0002E274 File Offset: 0x0002C474
		private static VideoCapture.VideoCaptureResult MakeCaptureResult(long hResult)
		{
			VideoCapture.VideoCaptureResult videoCaptureResult = default(VideoCapture.VideoCaptureResult);
			bool flag = hResult == VideoCapture.HR_SUCCESS;
			VideoCapture.CaptureResultType captureResultType;
			if (flag)
			{
				captureResultType = VideoCapture.CaptureResultType.Success;
			}
			else
			{
				captureResultType = VideoCapture.CaptureResultType.UnknownError;
			}
			videoCaptureResult.resultType = captureResultType;
			videoCaptureResult.hResult = hResult;
			return videoCaptureResult;
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001CCE RID: 7374 RVA: 0x0002E2B8 File Offset: 0x0002C4B8
		public static IEnumerable<Resolution> SupportedResolutions
		{
			get
			{
				bool flag = VideoCapture.s_SupportedResolutions == null;
				if (flag)
				{
					VideoCapture.s_SupportedResolutions = VideoCapture.GetSupportedResolutions_Internal();
				}
				return VideoCapture.s_SupportedResolutions;
			}
		}

		// Token: 0x06001CCF RID: 7375
		[NativeName("GetSupportedResolutions")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		private static extern Resolution[] GetSupportedResolutions_Internal();

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0002E2E8 File Offset: 0x0002C4E8
		public static IEnumerable<float> GetSupportedFrameRatesForResolution(Resolution resolution)
		{
			return VideoCapture.GetSupportedFrameRatesForResolution_Internal(resolution.width, resolution.height);
		}

		// Token: 0x06001CD1 RID: 7377
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeName("GetSupportedFrameRatesForResolution")]
		[MethodImpl(4096)]
		private static extern float[] GetSupportedFrameRatesForResolution_Internal(int resolutionWidth, int resolutionHeight);

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001CD2 RID: 7378
		public extern bool IsRecording
		{
			[NativeMethod("VideoCaptureBindings::IsRecording", HasExplicitThis = true)]
			[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0002E314 File Offset: 0x0002C514
		public static void CreateAsync(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback)
		{
			bool flag = onCreatedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCreatedCallback");
			}
			VideoCapture.Instantiate_Internal(showHolograms, onCreatedCallback);
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x0002E340 File Offset: 0x0002C540
		public static void CreateAsync(VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback)
		{
			bool flag = onCreatedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onCreatedCallback");
			}
			VideoCapture.Instantiate_Internal(false, onCreatedCallback);
		}

		// Token: 0x06001CD5 RID: 7381
		[NativeName("Instantiate")]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		private static extern void Instantiate_Internal(bool showHolograms, VideoCapture.OnVideoCaptureResourceCreatedCallback onCreatedCallback);

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0002E36C File Offset: 0x0002C56C
		[RequiredByNativeCode]
		private static void InvokeOnCreatedVideoCaptureResourceDelegate(VideoCapture.OnVideoCaptureResourceCreatedCallback callback, IntPtr nativePtr)
		{
			bool flag = nativePtr == IntPtr.Zero;
			if (flag)
			{
				callback(null);
			}
			else
			{
				callback(new VideoCapture(nativePtr));
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0002E3A4 File Offset: 0x0002C5A4
		private VideoCapture(IntPtr nativeCaptureObject)
		{
			this.m_NativePtr = nativeCaptureObject;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0002E3B8 File Offset: 0x0002C5B8
		public void StartVideoModeAsync(CameraParameters setupParams, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback)
		{
			bool flag = onVideoModeStartedCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onVideoModeStartedCallback");
			}
			bool flag2 = setupParams.cameraResolutionWidth == 0 || setupParams.cameraResolutionHeight == 0;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("setupParams", "The camera resolution must be set to a supported resolution.");
			}
			bool flag3 = setupParams.frameRate == 0f;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("setupParams", "The camera frame rate must be set to a supported recording frame rate.");
			}
			this.StartVideoMode_Internal(setupParams, audioState, onVideoModeStartedCallback);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0002E432 File Offset: 0x0002C632
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeMethod("VideoCaptureBindings::StartVideoMode", HasExplicitThis = true)]
		private void StartVideoMode_Internal(CameraParameters cameraParameters, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback)
		{
			this.StartVideoMode_Internal_Injected(ref cameraParameters, audioState, onVideoModeStartedCallback);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x0002E43E File Offset: 0x0002C63E
		[RequiredByNativeCode]
		private static void InvokeOnVideoModeStartedDelegate(VideoCapture.OnVideoModeStartedCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001CDB RID: 7387
		[NativeMethod("VideoCaptureBindings::StopVideoMode", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		public extern void StopVideoModeAsync([NotNull("ArgumentNullException")] VideoCapture.OnVideoModeStoppedCallback onVideoModeStoppedCallback);

		// Token: 0x06001CDC RID: 7388 RVA: 0x0002E44E File Offset: 0x0002C64E
		[RequiredByNativeCode]
		private static void InvokeOnVideoModeStoppedDelegate(VideoCapture.OnVideoModeStoppedCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x0002E460 File Offset: 0x0002C660
		public void StartRecordingAsync(string filename, VideoCapture.OnStartedRecordingVideoCallback onStartedRecordingVideoCallback)
		{
			bool flag = onStartedRecordingVideoCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("onStartedRecordingVideoCallback");
			}
			bool flag2 = string.IsNullOrEmpty(filename);
			if (flag2)
			{
				throw new ArgumentNullException("filename");
			}
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
			this.StartRecordingVideoToDisk_Internal(fileInfo.FullName, onStartedRecordingVideoCallback);
		}

		// Token: 0x06001CDE RID: 7390
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeMethod("VideoCaptureBindings::StartRecordingVideoToDisk", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void StartRecordingVideoToDisk_Internal(string filename, VideoCapture.OnStartedRecordingVideoCallback onStartedRecordingVideoCallback);

		// Token: 0x06001CDF RID: 7391 RVA: 0x0002E507 File Offset: 0x0002C707
		[RequiredByNativeCode]
		private static void InvokeOnStartedRecordingVideoToDiskDelegate(VideoCapture.OnStartedRecordingVideoCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001CE0 RID: 7392
		[NativeMethod("VideoCaptureBindings::StopRecordingVideoToDisk", HasExplicitThis = true)]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[MethodImpl(4096)]
		public extern void StopRecordingAsync([NotNull("ArgumentNullException")] VideoCapture.OnStoppedRecordingVideoCallback onStoppedRecordingVideoCallback);

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0002E517 File Offset: 0x0002C717
		[RequiredByNativeCode]
		private static void InvokeOnStoppedRecordingVideoToDiskDelegate(VideoCapture.OnStoppedRecordingVideoCallback callback, long hResult)
		{
			callback(VideoCapture.MakeCaptureResult(hResult));
		}

		// Token: 0x06001CE2 RID: 7394
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeMethod("VideoCaptureBindings::GetUnsafePointerToVideoDeviceController", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern IntPtr GetUnsafePointerToVideoDeviceController();

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0002E528 File Offset: 0x0002C728
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

		// Token: 0x06001CE4 RID: 7396
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeMethod("VideoCaptureBindings::Dispose", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void Dispose_Internal();

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0002E568 File Offset: 0x0002C768
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

		// Token: 0x06001CE6 RID: 7398
		[ThreadAndSerializationSafe]
		[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
		[NativeMethod("VideoCaptureBindings::DisposeThreaded", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void DisposeThreaded_Internal();

		// Token: 0x06001CE7 RID: 7399
		[MethodImpl(4096)]
		private extern void StartVideoMode_Internal_Injected(ref CameraParameters cameraParameters, VideoCapture.AudioState audioState, VideoCapture.OnVideoModeStartedCallback onVideoModeStartedCallback);

		// Token: 0x04000967 RID: 2407
		internal IntPtr m_NativePtr;

		// Token: 0x04000968 RID: 2408
		private static Resolution[] s_SupportedResolutions;

		// Token: 0x04000969 RID: 2409
		private static readonly long HR_SUCCESS;

		// Token: 0x020002A7 RID: 679
		public enum CaptureResultType
		{
			// Token: 0x0400096B RID: 2411
			Success,
			// Token: 0x0400096C RID: 2412
			UnknownError
		}

		// Token: 0x020002A8 RID: 680
		public enum AudioState
		{
			// Token: 0x0400096E RID: 2414
			MicAudio,
			// Token: 0x0400096F RID: 2415
			ApplicationAudio,
			// Token: 0x04000970 RID: 2416
			ApplicationAndMicAudio,
			// Token: 0x04000971 RID: 2417
			None
		}

		// Token: 0x020002A9 RID: 681
		public struct VideoCaptureResult
		{
			// Token: 0x170005B3 RID: 1459
			// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x0002E5BC File Offset: 0x0002C7BC
			public bool success
			{
				get
				{
					return this.resultType == VideoCapture.CaptureResultType.Success;
				}
			}

			// Token: 0x04000972 RID: 2418
			public VideoCapture.CaptureResultType resultType;

			// Token: 0x04000973 RID: 2419
			public long hResult;
		}

		// Token: 0x020002AA RID: 682
		// (Invoke) Token: 0x06001CEA RID: 7402
		public delegate void OnVideoCaptureResourceCreatedCallback(VideoCapture captureObject);

		// Token: 0x020002AB RID: 683
		// (Invoke) Token: 0x06001CEE RID: 7406
		public delegate void OnVideoModeStartedCallback(VideoCapture.VideoCaptureResult result);

		// Token: 0x020002AC RID: 684
		// (Invoke) Token: 0x06001CF2 RID: 7410
		public delegate void OnVideoModeStoppedCallback(VideoCapture.VideoCaptureResult result);

		// Token: 0x020002AD RID: 685
		// (Invoke) Token: 0x06001CF6 RID: 7414
		public delegate void OnStartedRecordingVideoCallback(VideoCapture.VideoCaptureResult result);

		// Token: 0x020002AE RID: 686
		// (Invoke) Token: 0x06001CFA RID: 7418
		public delegate void OnStoppedRecordingVideoCallback(VideoCapture.VideoCaptureResult result);
	}
}
