using System;
using System.IO;

namespace InControl
{
	// Token: 0x0200001C RID: 28
	public class DeviceBindingSource : BindingSource
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000041B6 File Offset: 0x000023B6
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000041BE File Offset: 0x000023BE
		public InputControlType Control { get; protected set; }

		// Token: 0x06000092 RID: 146 RVA: 0x000041C7 File Offset: 0x000023C7
		internal DeviceBindingSource()
		{
			this.Control = InputControlType.None;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000041D6 File Offset: 0x000023D6
		public DeviceBindingSource(InputControlType control)
		{
			this.Control = control;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000041E5 File Offset: 0x000023E5
		public override float GetValue(InputDevice inputDevice)
		{
			if (inputDevice == null)
			{
				return 0f;
			}
			return inputDevice.GetControl(this.Control).Value;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004201 File Offset: 0x00002401
		public override bool GetState(InputDevice inputDevice)
		{
			return inputDevice != null && inputDevice.GetControl(this.Control).State;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000421C File Offset: 0x0000241C
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return "";
				}
				InputDevice device = base.BoundTo.Device;
				if (device.GetControl(this.Control) == InputControl.Null)
				{
					return this.Control.ToString();
				}
				return device.GetControl(this.Control).Handle;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000427C File Offset: 0x0000247C
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
					return "Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000042B7 File Offset: 0x000024B7
		public override InputDeviceClass DeviceClass
		{
			get
			{
				if (base.BoundTo != null)
				{
					return base.BoundTo.Device.DeviceClass;
				}
				return InputDeviceClass.Unknown;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000042D3 File Offset: 0x000024D3
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				if (base.BoundTo != null)
				{
					return base.BoundTo.Device.DeviceStyle;
				}
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000042F0 File Offset: 0x000024F0
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004328 File Offset: 0x00002528
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000435C File Offset: 0x0000255C
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000437D File Offset: 0x0000257D
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.DeviceBindingSource;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004380 File Offset: 0x00002580
		public override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000438E File Offset: 0x0000258E
		public override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			this.Control = (InputControlType)reader.ReadInt32();
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000439C File Offset: 0x0000259C
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Logger.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				return base.BoundTo.Device.HasControl(this.Control) || Utility.TargetIsStandard(this.Control);
			}
		}
	}
}
