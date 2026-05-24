using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x02000289 RID: 649
	public static class PhraseRecognitionSystem
	{
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001C20 RID: 7200
		public static extern bool isSupported
		{
			[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
			[ThreadSafe]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001C21 RID: 7201
		public static extern SpeechSystemStatus Status
		{
			[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06001C22 RID: 7202
		[NativeThrows]
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		public static extern void Restart();

		// Token: 0x06001C23 RID: 7203
		[NativeHeader("PlatformDependent/Win/Bindings/SpeechBindings.h")]
		[MethodImpl(4096)]
		public static extern void Shutdown();

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001C24 RID: 7204 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
		// (remove) Token: 0x06001C25 RID: 7205 RVA: 0x0002D22C File Offset: 0x0002B42C
		[field: DebuggerBrowsable(0)]
		public static event PhraseRecognitionSystem.ErrorDelegate OnError;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06001C26 RID: 7206 RVA: 0x0002D260 File Offset: 0x0002B460
		// (remove) Token: 0x06001C27 RID: 7207 RVA: 0x0002D294 File Offset: 0x0002B494
		[field: DebuggerBrowsable(0)]
		public static event PhraseRecognitionSystem.StatusDelegate OnStatusChanged;

		// Token: 0x06001C28 RID: 7208 RVA: 0x0002D2C8 File Offset: 0x0002B4C8
		[RequiredByNativeCode]
		private static void PhraseRecognitionSystem_InvokeErrorEvent(SpeechError errorCode)
		{
			PhraseRecognitionSystem.ErrorDelegate onError = PhraseRecognitionSystem.OnError;
			bool flag = onError != null;
			if (flag)
			{
				onError(errorCode);
			}
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0002D2EC File Offset: 0x0002B4EC
		[RequiredByNativeCode]
		private static void PhraseRecognitionSystem_InvokeStatusChangedEvent(SpeechSystemStatus status)
		{
			PhraseRecognitionSystem.StatusDelegate onStatusChanged = PhraseRecognitionSystem.OnStatusChanged;
			bool flag = onStatusChanged != null;
			if (flag)
			{
				onStatusChanged(status);
			}
		}

		// Token: 0x0200028A RID: 650
		// (Invoke) Token: 0x06001C2B RID: 7211
		public delegate void ErrorDelegate(SpeechError errorCode);

		// Token: 0x0200028B RID: 651
		// (Invoke) Token: 0x06001C2F RID: 7215
		public delegate void StatusDelegate(SpeechSystemStatus status);
	}
}
