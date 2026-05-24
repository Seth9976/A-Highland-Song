using System;
using System.Collections.Generic;
using System.Threading;
using InControl.Internal;
using InControl.UnityDeviceProfiles;
using UnityEngine;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x0200007E RID: 126
	public class XInputDeviceManager : InputDeviceManager
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x00015530 File Offset: 0x00013730
		public XInputDeviceManager()
		{
			if (InputManager.XInputUpdateRate == 0U)
			{
				this.timeStep = Mathf.FloorToInt(Time.fixedDeltaTime * 1000f);
			}
			else
			{
				this.timeStep = Mathf.FloorToInt(1f / InputManager.XInputUpdateRate * 1000f);
			}
			this.bufferSize = (int)Math.Max(InputManager.XInputBufferSize, 1U);
			for (int i = 0; i < 4; i++)
			{
				this.gamePadState[i] = new RingBuffer<GamePadState>(this.bufferSize);
			}
			this.StartWorker();
			for (int j = 0; j < 4; j++)
			{
				this.devices.Add(new XInputDevice(j, this));
			}
			this.Update(0UL, 0f);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000155F9 File Offset: 0x000137F9
		private void StartWorker()
		{
			if (this.thread == null)
			{
				this.thread = new Thread(new ThreadStart(this.Worker));
				this.thread.IsBackground = true;
				this.thread.Start();
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00015631 File Offset: 0x00013831
		private void StopWorker()
		{
			if (this.thread != null)
			{
				this.thread.Abort();
				this.thread.Join();
				this.thread = null;
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00015658 File Offset: 0x00013858
		private void Worker()
		{
			for (;;)
			{
				for (int i = 0; i < 4; i++)
				{
					this.gamePadState[i].Enqueue(GamePad.GetState((PlayerIndex)i));
				}
				Thread.Sleep(this.timeStep);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00015690 File Offset: 0x00013890
		internal GamePadState GetState(int deviceIndex)
		{
			return this.gamePadState[deviceIndex].Dequeue();
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000156A0 File Offset: 0x000138A0
		public override void Update(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 4; i++)
			{
				XInputDevice xinputDevice = this.devices[i] as XInputDevice;
				if (!xinputDevice.IsConnected)
				{
					xinputDevice.GetState();
				}
				if (xinputDevice.IsConnected != this.deviceConnected[i])
				{
					if (xinputDevice.IsConnected)
					{
						InputManager.AttachDevice(xinputDevice);
					}
					else
					{
						InputManager.DetachDevice(xinputDevice);
					}
					this.deviceConnected[i] = xinputDevice.IsConnected;
				}
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001570D File Offset: 0x0001390D
		public override void Destroy()
		{
			this.StopWorker();
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00015718 File Offset: 0x00013918
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			try
			{
				GamePad.GetState(PlayerIndex.One);
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + ".dll could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00015774 File Offset: 0x00013974
		internal static void Enable()
		{
			List<string> list = new List<string>();
			if (XInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.HideDevicesWithProfile(typeof(Xbox360WindowsUnityProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWindowsUnityProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWindows10UnityProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWindows10AEUnityProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF310ModeXWindowsUnityProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF510ModeXWindowsUnityProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF710ModeXWindowsUnityProfile));
				InputManager.AddDeviceManager<XInputDeviceManager>();
				return;
			}
			foreach (string text in list)
			{
				Logger.LogError(text);
			}
		}

		// Token: 0x0400048D RID: 1165
		private readonly bool[] deviceConnected = new bool[4];

		// Token: 0x0400048E RID: 1166
		private const int maxDevices = 4;

		// Token: 0x0400048F RID: 1167
		private readonly RingBuffer<GamePadState>[] gamePadState = new RingBuffer<GamePadState>[4];

		// Token: 0x04000490 RID: 1168
		private Thread thread;

		// Token: 0x04000491 RID: 1169
		private readonly int timeStep;

		// Token: 0x04000492 RID: 1170
		private int bufferSize;
	}
}
