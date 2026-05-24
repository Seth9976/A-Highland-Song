using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000069 RID: 105
	public class UnityInputDevice : InputDevice
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00011FB2 File Offset: 0x000101B2
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00011FBA File Offset: 0x000101BA
		public int JoystickId { get; private set; }

		// Token: 0x060004CF RID: 1231 RVA: 0x00011FC3 File Offset: 0x000101C3
		public UnityInputDevice(int joystickId, string joystickName)
			: this(null, joystickId, joystickName)
		{
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00011FD0 File Offset: 0x000101D0
		public UnityInputDevice(InputDeviceProfile deviceProfile, int joystickId, string joystickName)
		{
			this.profile = deviceProfile;
			this.JoystickId = joystickId;
			if (joystickId != 0)
			{
				base.SortOrder = 100 + joystickId;
			}
			UnityInputDevice.SetupAnalogQueries();
			UnityInputDevice.SetupButtonQueries();
			base.AnalogSnapshot = null;
			if (this.IsKnown)
			{
				base.Name = this.profile.DeviceName;
				base.Meta = this.profile.DeviceNotes;
				base.DeviceClass = this.profile.DeviceClass;
				base.DeviceStyle = this.profile.DeviceStyle;
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					if (Utility.TargetIsAlias(inputControlMapping.Target))
					{
						Logger.LogError(string.Concat(new string[]
						{
							"Cannot map control \"",
							inputControlMapping.Name,
							"\" as InputControlType.",
							inputControlMapping.Target.ToString(),
							" in profile \"",
							deviceProfile.DeviceName,
							"\" because this target is reserved as an alias. The mapping will be ignored."
						}));
					}
					else
					{
						InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Name);
						inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
						inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
						inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
						inputControl.Raw = inputControlMapping.Raw;
						inputControl.Passive = inputControlMapping.Passive;
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					if (Utility.TargetIsAlias(inputControlMapping2.Target))
					{
						Logger.LogError(string.Concat(new string[]
						{
							"Cannot map control \"",
							inputControlMapping2.Name,
							"\" as InputControlType.",
							inputControlMapping2.Target.ToString(),
							" in profile \"",
							deviceProfile.DeviceName,
							"\" because this target is reserved as an alias. The mapping will be ignored."
						}));
					}
					else
					{
						base.AddControl(inputControlMapping2.Target, inputControlMapping2.Name).Passive = inputControlMapping2.Passive;
					}
				}
				return;
			}
			base.Name = "Unknown Device";
			base.Meta = "\"" + joystickName + "\"";
			for (int k = 0; k < this.NumUnknownButtons; k++)
			{
				base.AddControl(InputControlType.Button0 + k, "Button " + k.ToString());
			}
			for (int l = 0; l < this.NumUnknownAnalogs; l++)
			{
				base.AddControl(InputControlType.Analog0 + l, "Analog " + l.ToString(), 0.2f, 0.9f);
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000122C4 File Offset: 0x000104C4
		public override void Update(ulong updateTick, float deltaTime)
		{
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

		// Token: 0x060004D2 RID: 1234 RVA: 0x00012400 File Offset: 0x00010600
		private static void SetupAnalogQueries()
		{
			if (UnityInputDevice.analogQueries == null)
			{
				UnityInputDevice.analogQueries = new string[10, 20];
				for (int i = 1; i <= 10; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityInputDevice.analogQueries[i - 1, j] = "joystick " + i.ToString() + " analog " + j.ToString();
					}
				}
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00012468 File Offset: 0x00010668
		private static void SetupButtonQueries()
		{
			if (UnityInputDevice.buttonQueries == null)
			{
				UnityInputDevice.buttonQueries = new string[10, 20];
				for (int i = 1; i <= 10; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityInputDevice.buttonQueries[i - 1, j] = "joystick " + i.ToString() + " button " + j.ToString();
					}
				}
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000124CF File Offset: 0x000106CF
		public override bool ReadRawButtonState(int index)
		{
			return index < 20 && Input.GetKey(UnityInputDevice.buttonQueries[this.JoystickId - 1, index]);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000124F0 File Offset: 0x000106F0
		public override float ReadRawAnalogValue(int index)
		{
			if (index < 20)
			{
				return Input.GetAxisRaw(UnityInputDevice.analogQueries[this.JoystickId - 1, index]);
			}
			return 0f;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00012515 File Offset: 0x00010715
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.profile == null || this.profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001252C File Offset: 0x0001072C
		public override bool IsKnown
		{
			get
			{
				return this.profile != null;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00012537 File Offset: 0x00010737
		public override int NumUnknownButtons
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001253B File Offset: 0x0001073B
		public override int NumUnknownAnalogs
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x04000459 RID: 1113
		private static string[,] analogQueries;

		// Token: 0x0400045A RID: 1114
		private static string[,] buttonQueries;

		// Token: 0x0400045B RID: 1115
		public const int MaxDevices = 10;

		// Token: 0x0400045C RID: 1116
		public const int MaxButtons = 20;

		// Token: 0x0400045D RID: 1117
		public const int MaxAnalogs = 20;

		// Token: 0x0400045F RID: 1119
		private readonly InputDeviceProfile profile;
	}
}
