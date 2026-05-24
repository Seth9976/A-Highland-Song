using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000029 RID: 41
	public class UnknownDeviceBindingSource : BindingSource
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000693F File Offset: 0x00004B3F
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00006947 File Offset: 0x00004B47
		public UnknownDeviceControl Control { get; protected set; }

		// Token: 0x06000167 RID: 359 RVA: 0x00006950 File Offset: 0x00004B50
		internal UnknownDeviceBindingSource()
		{
			this.Control = UnknownDeviceControl.None;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006963 File Offset: 0x00004B63
		public UnknownDeviceBindingSource(UnknownDeviceControl control)
		{
			this.Control = control;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006974 File Offset: 0x00004B74
		public override float GetValue(InputDevice device)
		{
			return this.Control.GetValue(device);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006990 File Offset: 0x00004B90
		public override bool GetState(InputDevice device)
		{
			return device != null && Utility.IsNotZero(this.GetValue(device));
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000069A4 File Offset: 0x00004BA4
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return "";
				}
				string text = "";
				if (this.Control.SourceRange == InputRangeType.ZeroToMinusOne)
				{
					text = "Negative ";
				}
				else if (this.Control.SourceRange == InputRangeType.ZeroToOne)
				{
					text = "Positive ";
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					string text2 = text;
					UnknownDeviceControl unknownDeviceControl = this.Control;
					return text2 + unknownDeviceControl.Control.ToString();
				}
				InputControl control = device.GetControl(this.Control.Control);
				if (control == InputControl.Null)
				{
					string text3 = text;
					UnknownDeviceControl unknownDeviceControl = this.Control;
					return text3 + unknownDeviceControl.Control.ToString();
				}
				return text + control.Handle;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00006A6C File Offset: 0x00004C6C
		public override string DeviceName
		{
			get
			{
				if (base.BoundTo == null)
				{
					return "";
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return "Unknown Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00006AA7 File Offset: 0x00004CA7
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Controller;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00006AAA File Offset: 0x00004CAA
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006AB0 File Offset: 0x00004CB0
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006AEC File Offset: 0x00004CEC
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006B24 File Offset: 0x00004D24
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006B45 File Offset: 0x00004D45
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006B48 File Offset: 0x00004D48
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Logger.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				InputDevice device = base.BoundTo.Device;
				return device == InputDevice.Null || device.HasControl(this.Control.Control);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006B90 File Offset: 0x00004D90
		public override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			UnknownDeviceControl unknownDeviceControl = default(UnknownDeviceControl);
			unknownDeviceControl.Load(reader);
			this.Control = unknownDeviceControl;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006BB4 File Offset: 0x00004DB4
		public override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
