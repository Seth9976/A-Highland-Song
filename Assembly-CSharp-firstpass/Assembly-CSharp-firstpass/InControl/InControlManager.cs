using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x0200002D RID: 45
	public class InControlManager : SingletonMonoBehavior<InControlManager>
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00006E9C File Offset: 0x0000509C
		private void OnEnable()
		{
			if (base.EnforceSingleton)
			{
				return;
			}
			InputManager.InvertYAxis = this.invertYAxis;
			InputManager.SuspendInBackground = this.suspendInBackground;
			InputManager.EnableICade = this.enableICade;
			InputManager.EnableXInput = this.enableXInput;
			InputManager.XInputUpdateRate = (uint)Mathf.Max(this.xInputUpdateRate, 0);
			InputManager.XInputBufferSize = (uint)Mathf.Max(this.xInputBufferSize, 0);
			InputManager.EnableNativeInput = this.enableNativeInput;
			InputManager.NativeInputEnableXInput = this.nativeInputEnableXInput;
			InputManager.NativeInputEnableMFi = this.nativeInputEnableMFi;
			InputManager.NativeInputUpdateRate = (uint)Mathf.Max(this.nativeInputUpdateRate, 0);
			InputManager.NativeInputPreventSleep = this.nativeInputPreventSleep;
			if (this.logDebugInfo)
			{
				Logger.OnLogMessage -= InControlManager.LogMessage;
				Logger.OnLogMessage += InControlManager.LogMessage;
			}
			InputManager.SetupInternal();
			SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
			SceneManager.sceneLoaded += this.OnSceneWasLoaded;
			if (this.dontDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(this);
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006F9D File Offset: 0x0000519D
		private void OnDisable()
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
			InputManager.ResetInternal();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006FBE File Offset: 0x000051BE
		private void Update()
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			if (this.applicationHasQuit)
			{
				return;
			}
			if (this.updateMode == InControlUpdateMode.Default || (this.updateMode == InControlUpdateMode.FixedUpdate && Utility.IsZero(Time.timeScale)))
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006FF4 File Offset: 0x000051F4
		private void FixedUpdate()
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			if (this.applicationHasQuit)
			{
				return;
			}
			if (this.updateMode == InControlUpdateMode.FixedUpdate)
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007016 File Offset: 0x00005216
		private void OnApplicationFocus(bool focusState)
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			if (this.applicationHasQuit)
			{
				return;
			}
			InputManager.OnApplicationFocus(focusState);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007030 File Offset: 0x00005230
		private void OnApplicationPause(bool pauseState)
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			if (this.applicationHasQuit)
			{
				return;
			}
			InputManager.OnApplicationPause(pauseState);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000704A File Offset: 0x0000524A
		private void OnApplicationQuit()
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			if (this.applicationHasQuit)
			{
				return;
			}
			InputManager.OnApplicationQuit();
			this.applicationHasQuit = true;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000706A File Offset: 0x0000526A
		private void OnSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (base.IsNotTheSingleton)
			{
				return;
			}
			if (this.applicationHasQuit)
			{
				return;
			}
			if (loadSceneMode == LoadSceneMode.Single)
			{
				InputManager.OnLevelWasLoaded();
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007088 File Offset: 0x00005288
		private static void LogMessage(LogMessage logMessage)
		{
			switch (logMessage.Type)
			{
			case LogMessageType.Info:
				Debug.Log(logMessage.Text);
				return;
			case LogMessageType.Warning:
				Debug.LogWarning(logMessage.Text);
				return;
			case LogMessageType.Error:
				Debug.LogError(logMessage.Text);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000197 RID: 407
		public bool logDebugInfo = true;

		// Token: 0x04000198 RID: 408
		public bool invertYAxis;

		// Token: 0x04000199 RID: 409
		[SerializeField]
		private bool useFixedUpdate;

		// Token: 0x0400019A RID: 410
		public bool dontDestroyOnLoad = true;

		// Token: 0x0400019B RID: 411
		public bool suspendInBackground;

		// Token: 0x0400019C RID: 412
		public InControlUpdateMode updateMode;

		// Token: 0x0400019D RID: 413
		public bool enableICade;

		// Token: 0x0400019E RID: 414
		public bool enableXInput;

		// Token: 0x0400019F RID: 415
		public bool xInputOverrideUpdateRate;

		// Token: 0x040001A0 RID: 416
		public int xInputUpdateRate;

		// Token: 0x040001A1 RID: 417
		public bool xInputOverrideBufferSize;

		// Token: 0x040001A2 RID: 418
		public int xInputBufferSize;

		// Token: 0x040001A3 RID: 419
		public bool enableNativeInput = true;

		// Token: 0x040001A4 RID: 420
		public bool nativeInputEnableXInput = true;

		// Token: 0x040001A5 RID: 421
		public bool nativeInputEnableMFi = true;

		// Token: 0x040001A6 RID: 422
		public bool nativeInputPreventSleep;

		// Token: 0x040001A7 RID: 423
		public bool nativeInputOverrideUpdateRate;

		// Token: 0x040001A8 RID: 424
		public int nativeInputUpdateRate;

		// Token: 0x040001A9 RID: 425
		private bool applicationHasQuit;
	}
}
