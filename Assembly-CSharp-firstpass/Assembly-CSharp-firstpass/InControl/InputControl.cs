using System;

namespace InControl
{
	// Token: 0x02000031 RID: 49
	public class InputControl : OneAxisInputControl
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000072C2 File Offset: 0x000054C2
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000072CA File Offset: 0x000054CA
		public string Handle { get; protected set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000072D3 File Offset: 0x000054D3
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000072DB File Offset: 0x000054DB
		public InputControlType Target { get; protected set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000072E4 File Offset: 0x000054E4
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000072EC File Offset: 0x000054EC
		public bool IsButton { get; protected set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000072F5 File Offset: 0x000054F5
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000072FD File Offset: 0x000054FD
		public bool IsAnalog { get; protected set; }

		// Token: 0x060001A7 RID: 423 RVA: 0x00007306 File Offset: 0x00005506
		private InputControl()
		{
			this.Handle = "None";
			this.Target = InputControlType.None;
			this.Passive = false;
			this.IsButton = false;
			this.IsAnalog = false;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007335 File Offset: 0x00005535
		public InputControl(string handle, InputControlType target)
		{
			this.Handle = handle;
			this.Target = target;
			this.Passive = false;
			this.IsButton = Utility.TargetIsButton(target);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000736D File Offset: 0x0000556D
		public InputControl(string handle, InputControlType target, bool passive)
			: this(handle, target)
		{
			this.Passive = passive;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000737E File Offset: 0x0000557E
		internal void SetZeroTick()
		{
			this.zeroTick = base.UpdateTick;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000738C File Offset: 0x0000558C
		internal bool IsOnZeroTick
		{
			get
			{
				return base.UpdateTick == this.zeroTick;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000739C File Offset: 0x0000559C
		public bool IsStandard
		{
			get
			{
				return Utility.TargetIsStandard(this.Target);
			}
		}

		// Token: 0x040001AA RID: 426
		public static readonly InputControl Null = new InputControl
		{
			isNullControl = true
		};

		// Token: 0x040001AD RID: 429
		public bool Passive;

		// Token: 0x040001B0 RID: 432
		private ulong zeroTick;
	}
}
