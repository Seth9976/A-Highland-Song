using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000052 RID: 82
	public class NativeInputDevice : InputDevice
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000D490 File Offset: 0x0000B690
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000D498 File Offset: 0x0000B698
		public uint Handle { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000D4A1 File Offset: 0x0000B6A1
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0000D4A9 File Offset: 0x0000B6A9
		public InputDeviceInfo Info { get; private set; }

		// Token: 0x060003D4 RID: 980 RVA: 0x0000D4B2 File Offset: 0x0000B6B2
		internal NativeInputDevice()
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000D4CC File Offset: 0x0000B6CC
		internal void Initialize(uint deviceHandle, InputDeviceInfo deviceInfo, InputDeviceProfile deviceProfile)
		{
			this.Handle = deviceHandle;
			this.Info = deviceInfo;
			this.profile = deviceProfile;
			base.SortOrder = (int)(1000U + this.Handle);
			this.numUnknownButtons = Math.Min((int)this.Info.numButtons, 20);
			this.numUnknownAnalogs = Math.Min((int)this.Info.numAnalogs, 20);
			this.buttons = new short[this.Info.numButtons];
			this.analogs = new short[this.Info.numAnalogs];
			base.AnalogSnapshot = null;
			this.controlSourceByTarget = new InputControlSource[531];
			base.ClearInputState();
			base.ClearControls();
			if (this.IsKnown)
			{
				base.Name = this.profile.DeviceName ?? this.Info.name;
				base.Name = base.Name.Replace("{NAME}", this.Info.name).Trim();
				base.Meta = this.profile.DeviceNotes ?? this.Info.name;
				base.DeviceClass = this.profile.DeviceClass;
				base.DeviceStyle = this.profile.DeviceStyle;
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Name);
					inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
					inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
					inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
					inputControl.Raw = inputControlMapping.Raw;
					inputControl.Passive = inputControlMapping.Passive;
					this.controlSourceByTarget[(int)inputControlMapping.Target] = inputControlMapping.Source;
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					base.AddControl(inputControlMapping2.Target, inputControlMapping2.Name).Passive = inputControlMapping2.Passive;
					this.controlSourceByTarget[(int)inputControlMapping2.Target] = inputControlMapping2.Source;
				}
			}
			else
			{
				base.Name = "Unknown Device";
				base.Meta = this.Info.name;
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.AddControl(InputControlType.Button0 + k, "Button " + k.ToString());
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.AddControl(InputControlType.Analog0 + l, "Analog " + l.ToString(), 0.2f, 0.9f);
				}
			}
			this.skipUpdateFrames = 1;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000D7D3 File Offset: 0x0000B9D3
		internal void Initialize(uint deviceHandle, InputDeviceInfo deviceInfo)
		{
			this.Initialize(deviceHandle, deviceInfo, this.profile);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.SendStatusUpdates();
			if (this.skipUpdateFrames > 0)
			{
				this.skipUpdateFrames--;
				return;
			}
			IntPtr intPtr;
			if (Native.GetDeviceState(this.Handle, out intPtr))
			{
				Marshal.Copy(intPtr, this.buttons, 0, this.buttons.Length);
				intPtr = new IntPtr(intPtr.ToInt64() + (long)(this.buttons.Length * 2));
				Marshal.Copy(intPtr, this.analogs, 0, this.analogs.Length);
			}
			if (this.IsKnown)
			{
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					float value = inputControlMapping.Source.GetValue(this);
					InputControl control = base.GetControl(inputControlMapping.Target);
					if (!inputControlMapping.IgnoreInitialZeroValue || !control.IsOnZeroTick || !Utility.IsZero(value))
					{
						float num = inputControlMapping.ApplyToValue(value);
						control.UpdateWithValue(num, updateTick, deltaTime);
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					bool state = inputControlMapping2.Source.GetState(this);
					base.UpdateWithState(inputControlMapping2.Target, state, updateTick, deltaTime);
				}
				return;
			}
			for (int k = 0; k < this.NumUnknownButtons; k++)
			{
				base.UpdateWithState(InputControlType.Button0 + k, this.ReadRawButtonState(k), updateTick, deltaTime);
			}
			for (int l = 0; l < this.NumUnknownAnalogs; l++)
			{
				base.UpdateWithValue(InputControlType.Analog0 + l, this.ReadRawAnalogValue(l), updateTick, deltaTime);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000D993 File Offset: 0x0000BB93
		public override bool ReadRawButtonState(int index)
		{
			return index < this.buttons.Length && this.buttons[index] > -32767;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000D9B1 File Offset: 0x0000BBB1
		public override float ReadRawAnalogValue(int index)
		{
			if (index < this.analogs.Length)
			{
				return (float)this.analogs[index] / 32767f;
			}
			return 0f;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000D9D3 File Offset: 0x0000BBD3
		private static byte FloatToByte(float value)
		{
			return (byte)(Mathf.Clamp01(value) * 255f);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000D9E2 File Offset: 0x0000BBE2
		public override void Vibrate(float leftSpeed, float rightSpeed)
		{
			this.sendVibrate = true;
			this.vibrateToSend = new Vector2(leftSpeed, rightSpeed);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000D9F8 File Offset: 0x0000BBF8
		public override void VibrateTriggers(float leftTriggerSpeed, float rightTriggerSpeed)
		{
			this.sendVibrateTriggers = true;
			this.vibrateTriggersToSend = new Vector2(leftTriggerSpeed, rightTriggerSpeed);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000DA0E File Offset: 0x0000BC0E
		public override void SetLightColor(float red, float green, float blue)
		{
			this.sendLightColor = true;
			this.lightColorToSend = new Vector3(red, green, blue);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000DA25 File Offset: 0x0000BC25
		public override void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
			this.sendLightFlash = true;
			this.lightFlashToSend = new Vector2(flashOnDuration, flashOffDuration);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		private void SendStatusUpdates()
		{
			if (this.sendVibrate && InputManager.CurrentTime - this.lastTimeVibrateWasSent > 0.02f)
			{
				Native.SetHapticState(this.Handle, NativeInputDevice.FloatToByte(this.vibrateToSend.x), NativeInputDevice.FloatToByte(this.vibrateToSend.y));
				this.sendVibrate = false;
				this.lastTimeVibrateWasSent = InputManager.CurrentTime;
				this.vibrateToSend = Vector2.zero;
			}
			if (this.sendVibrateTriggers && InputManager.CurrentTime - this.lastTimeVibrateTriggersWasSent > 0.02f)
			{
				Native.SetTriggersHapticState(this.Handle, NativeInputDevice.FloatToByte(this.vibrateTriggersToSend.x), NativeInputDevice.FloatToByte(this.vibrateTriggersToSend.y));
				this.sendVibrateTriggers = false;
				this.lastTimeVibrateTriggersWasSent = InputManager.CurrentTime;
				this.vibrateTriggersToSend = Vector2.zero;
			}
			if (this.sendLightColor && InputManager.CurrentTime - this.lastTimeLightColorWasSent > 0.02f)
			{
				Native.SetLightColor(this.Handle, NativeInputDevice.FloatToByte(this.lightColorToSend.x), NativeInputDevice.FloatToByte(this.lightColorToSend.y), NativeInputDevice.FloatToByte(this.lightColorToSend.z));
				this.sendLightColor = false;
				this.lastTimeLightColorWasSent = InputManager.CurrentTime;
				this.lightColorToSend = Vector3.zero;
			}
			if (this.sendLightFlash && InputManager.CurrentTime - this.lastTimeLightFlashWasSent > 0.02f)
			{
				Native.SetLightFlash(this.Handle, NativeInputDevice.FloatToByte(this.lightFlashToSend.x), NativeInputDevice.FloatToByte(this.lightFlashToSend.y));
				this.sendLightFlash = false;
				this.lastTimeLightFlashWasSent = InputManager.CurrentTime;
				this.lightFlashToSend = Vector2.zero;
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		public string GetAppleGlyphNameForControl(InputControlType controlType)
		{
			InputControlSource inputControlSource = this.controlSourceByTarget[(int)controlType];
			if (inputControlSource.SourceType != InputControlSourceType.None)
			{
				InputControlSourceType sourceType = inputControlSource.SourceType;
				IntPtr zero;
				uint num;
				if (sourceType != InputControlSourceType.Button)
				{
					if (sourceType != InputControlSourceType.Analog)
					{
						zero = IntPtr.Zero;
						num = 0U;
					}
					else
					{
						num = Native.GetAnalogGlyphName(this.Handle, (uint)inputControlSource.Index, out zero);
					}
				}
				else
				{
					num = Native.GetButtonGlyphName(this.Handle, (uint)inputControlSource.Index, out zero);
				}
				if (num > 0U)
				{
					this.glyphName.Clear();
					int num2 = 0;
					while ((long)num2 < (long)((ulong)num))
					{
						this.glyphName.Append((char)Marshal.ReadByte(zero, num2));
						num2++;
					}
					return this.glyphName.ToString();
				}
			}
			return "";
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		public bool HasSameVendorID(InputDeviceInfo deviceInfo)
		{
			return this.Info.HasSameVendorID(deviceInfo);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		public bool HasSameProductID(InputDeviceInfo deviceInfo)
		{
			return this.Info.HasSameProductID(deviceInfo);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		public bool HasSameVersionNumber(InputDeviceInfo deviceInfo)
		{
			return this.Info.HasSameVersionNumber(deviceInfo);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
		public bool HasSameLocation(InputDeviceInfo deviceInfo)
		{
			return this.Info.HasSameLocation(deviceInfo);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000DD10 File Offset: 0x0000BF10
		public bool HasSameSerialNumber(InputDeviceInfo deviceInfo)
		{
			return this.Info.HasSameSerialNumber(deviceInfo);
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000DD2C File Offset: 0x0000BF2C
		public string ProfileName
		{
			get
			{
				if (this.profile != null)
				{
					return this.profile.GetType().Name;
				}
				return "N/A";
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000DD4C File Offset: 0x0000BF4C
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.profile == null || this.profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000DD63 File Offset: 0x0000BF63
		public override bool IsKnown
		{
			get
			{
				return this.profile != null;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000DD6E File Offset: 0x0000BF6E
		public override int NumUnknownButtons
		{
			get
			{
				return this.numUnknownButtons;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000DD76 File Offset: 0x0000BF76
		public override int NumUnknownAnalogs
		{
			get
			{
				return this.numUnknownAnalogs;
			}
		}

		// Token: 0x0400038C RID: 908
		private const int maxUnknownButtons = 20;

		// Token: 0x0400038D RID: 909
		private const int maxUnknownAnalogs = 20;

		// Token: 0x04000390 RID: 912
		private short[] buttons;

		// Token: 0x04000391 RID: 913
		private short[] analogs;

		// Token: 0x04000392 RID: 914
		private InputDeviceProfile profile;

		// Token: 0x04000393 RID: 915
		private int skipUpdateFrames;

		// Token: 0x04000394 RID: 916
		private int numUnknownButtons;

		// Token: 0x04000395 RID: 917
		private int numUnknownAnalogs;

		// Token: 0x04000396 RID: 918
		private InputControlSource[] controlSourceByTarget;

		// Token: 0x04000397 RID: 919
		private bool sendVibrate;

		// Token: 0x04000398 RID: 920
		private float lastTimeVibrateWasSent;

		// Token: 0x04000399 RID: 921
		private Vector2 vibrateToSend;

		// Token: 0x0400039A RID: 922
		private bool sendVibrateTriggers;

		// Token: 0x0400039B RID: 923
		private float lastTimeVibrateTriggersWasSent;

		// Token: 0x0400039C RID: 924
		private Vector2 vibrateTriggersToSend;

		// Token: 0x0400039D RID: 925
		private bool sendLightColor;

		// Token: 0x0400039E RID: 926
		private float lastTimeLightColorWasSent;

		// Token: 0x0400039F RID: 927
		private Vector3 lightColorToSend;

		// Token: 0x040003A0 RID: 928
		private bool sendLightFlash;

		// Token: 0x040003A1 RID: 929
		private float lastTimeLightFlashWasSent;

		// Token: 0x040003A2 RID: 930
		private Vector2 lightFlashToSend;

		// Token: 0x040003A3 RID: 931
		private readonly StringBuilder glyphName = new StringBuilder(256);

		// Token: 0x040003A4 RID: 932
		private const string defaultGlyphName = "";
	}
}
