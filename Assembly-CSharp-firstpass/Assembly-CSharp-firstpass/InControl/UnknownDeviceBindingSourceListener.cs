using System;

namespace InControl
{
	// Token: 0x0200002A RID: 42
	public class UnknownDeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000176 RID: 374 RVA: 0x00006BD0 File Offset: 0x00004DD0
		public void Reset()
		{
			this.detectFound = UnknownDeviceControl.None;
			this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease;
			this.TakeSnapshotOnUnknownDevices();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006BEC File Offset: 0x00004DEC
		private void TakeSnapshotOnUnknownDevices()
		{
			int count = InputManager.Devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.Devices[i];
				if (inputDevice.IsUnknown)
				{
					inputDevice.TakeSnapshot();
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006C2C File Offset: 0x00004E2C
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeUnknownControllers || device.IsKnown)
			{
				return null;
			}
			if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease && this.detectFound && !this.IsPressed(this.detectFound, device))
			{
				BindingSource bindingSource = new UnknownDeviceBindingSource(this.detectFound);
				this.Reset();
				return bindingSource;
			}
			UnknownDeviceControl unknownDeviceControl = this.ListenForControl(listenOptions, device);
			if (unknownDeviceControl)
			{
				if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress)
				{
					this.detectFound = unknownDeviceControl;
					this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease;
				}
			}
			else if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease)
			{
				this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress;
			}
			return null;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006CBC File Offset: 0x00004EBC
		private bool IsPressed(UnknownDeviceControl control, InputDevice device)
		{
			return Utility.AbsoluteIsOverThreshold(control.GetValue(device), 0.5f);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006CD0 File Offset: 0x00004ED0
		private UnknownDeviceControl ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsUnknown)
			{
				UnknownDeviceControl firstPressedButton = device.GetFirstPressedButton();
				if (firstPressedButton)
				{
					return firstPressedButton;
				}
				UnknownDeviceControl firstPressedAnalog = device.GetFirstPressedAnalog();
				if (firstPressedAnalog)
				{
					return firstPressedAnalog;
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x0400018C RID: 396
		private UnknownDeviceControl detectFound;

		// Token: 0x0400018D RID: 397
		private UnknownDeviceBindingSourceListener.DetectPhase detectPhase;

		// Token: 0x02000219 RID: 537
		private enum DetectPhase
		{
			// Token: 0x040004BA RID: 1210
			WaitForInitialRelease,
			// Token: 0x040004BB RID: 1211
			WaitForControlPress,
			// Token: 0x040004BC RID: 1212
			WaitForControlRelease
		}
	}
}
