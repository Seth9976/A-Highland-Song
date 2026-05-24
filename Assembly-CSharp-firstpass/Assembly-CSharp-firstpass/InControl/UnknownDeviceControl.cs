using System;
using System.IO;

namespace InControl
{
	// Token: 0x0200002B RID: 43
	public struct UnknownDeviceControl : IEquatable<UnknownDeviceControl>
	{
		// Token: 0x0600017C RID: 380 RVA: 0x00006D14 File Offset: 0x00004F14
		public UnknownDeviceControl(InputControlType control, InputRangeType sourceRange)
		{
			this.Control = control;
			this.SourceRange = sourceRange;
			this.IsButton = Utility.TargetIsButton(control);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006D3F File Offset: 0x00004F3F
		internal float GetValue(InputDevice device)
		{
			if (device == null)
			{
				return 0f;
			}
			return InputRange.Remap(device.GetControl(this.Control).Value, this.SourceRange, InputRangeType.ZeroToOne);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00006D67 File Offset: 0x00004F67
		public int Index
		{
			get
			{
				return this.Control - (this.IsButton ? InputControlType.Button0 : InputControlType.Analog0);
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006D84 File Offset: 0x00004F84
		public static bool operator ==(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006DA0 File Offset: 0x00004FA0
		public static bool operator !=(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			return !(a == b);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006DAC File Offset: 0x00004FAC
		public bool Equals(UnknownDeviceControl other)
		{
			return this.Control == other.Control && this.SourceRange == other.SourceRange;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006DCC File Offset: 0x00004FCC
		public override bool Equals(object other)
		{
			return this.Equals((UnknownDeviceControl)other);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006DDA File Offset: 0x00004FDA
		public override int GetHashCode()
		{
			return this.Control.GetHashCode() ^ this.SourceRange.GetHashCode();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006DFF File Offset: 0x00004FFF
		public static implicit operator bool(UnknownDeviceControl control)
		{
			return control.Control > InputControlType.None;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006E0A File Offset: 0x0000500A
		public override string ToString()
		{
			return string.Format("UnknownDeviceControl( {0}, {1} )", this.Control.ToString(), this.SourceRange.ToString());
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006E38 File Offset: 0x00005038
		internal void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
			writer.Write((int)this.SourceRange);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006E52 File Offset: 0x00005052
		internal void Load(BinaryReader reader)
		{
			this.Control = (InputControlType)reader.ReadInt32();
			this.SourceRange = (InputRangeType)reader.ReadInt32();
			this.IsButton = Utility.TargetIsButton(this.Control);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x0400018E RID: 398
		public static readonly UnknownDeviceControl None = new UnknownDeviceControl(InputControlType.None, InputRangeType.None);

		// Token: 0x0400018F RID: 399
		public InputControlType Control;

		// Token: 0x04000190 RID: 400
		public InputRangeType SourceRange;

		// Token: 0x04000191 RID: 401
		public bool IsButton;

		// Token: 0x04000192 RID: 402
		public bool IsAnalog;
	}
}
