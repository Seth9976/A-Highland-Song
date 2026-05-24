using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000018 RID: 24
	public abstract class BindingSource : IEquatable<BindingSource>
	{
		// Token: 0x0600007C RID: 124
		public abstract float GetValue(InputDevice inputDevice);

		// Token: 0x0600007D RID: 125
		public abstract bool GetState(InputDevice inputDevice);

		// Token: 0x0600007E RID: 126
		public abstract bool Equals(BindingSource other);

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007F RID: 127
		public abstract string Name { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000080 RID: 128
		public abstract string DeviceName { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000081 RID: 129
		public abstract InputDeviceClass DeviceClass { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000082 RID: 130
		public abstract InputDeviceStyle DeviceStyle { get; }

		// Token: 0x06000083 RID: 131 RVA: 0x00004151 File Offset: 0x00002351
		public static bool operator ==(BindingSource a, BindingSource b)
		{
			return a == b || (a != null && b != null && a.BindingSourceType == b.BindingSourceType && a.Equals(b));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004178 File Offset: 0x00002378
		public static bool operator !=(BindingSource a, BindingSource b)
		{
			return !(a == b);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004184 File Offset: 0x00002384
		public override bool Equals(object obj)
		{
			return this.Equals((BindingSource)obj);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004192 File Offset: 0x00002392
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000087 RID: 135
		public abstract BindingSourceType BindingSourceType { get; }

		// Token: 0x06000088 RID: 136
		public abstract void Save(BinaryWriter writer);

		// Token: 0x06000089 RID: 137
		public abstract void Load(BinaryReader reader, ushort dataFormatVersion);

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000419A File Offset: 0x0000239A
		// (set) Token: 0x0600008B RID: 139 RVA: 0x000041A2 File Offset: 0x000023A2
		internal PlayerAction BoundTo { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000041AB File Offset: 0x000023AB
		internal virtual bool IsValid
		{
			get
			{
				return true;
			}
		}
	}
}
