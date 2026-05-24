using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200003A RID: 58
	public class TwoAxisInputControl : IInputControl
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00007F91 File Offset: 0x00006191
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00007F99 File Offset: 0x00006199
		public float X { get; protected set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00007FA2 File Offset: 0x000061A2
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00007FAA File Offset: 0x000061AA
		public float Y { get; protected set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00007FB3 File Offset: 0x000061B3
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00007FBB File Offset: 0x000061BB
		public OneAxisInputControl Left { get; protected set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00007FC4 File Offset: 0x000061C4
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00007FCC File Offset: 0x000061CC
		public OneAxisInputControl Right { get; protected set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00007FD5 File Offset: 0x000061D5
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00007FDD File Offset: 0x000061DD
		public OneAxisInputControl Up { get; protected set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00007FE6 File Offset: 0x000061E6
		// (set) Token: 0x06000215 RID: 533 RVA: 0x00007FEE File Offset: 0x000061EE
		public OneAxisInputControl Down { get; protected set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00007FF7 File Offset: 0x000061F7
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00007FFF File Offset: 0x000061FF
		public ulong UpdateTick { get; protected set; }

		// Token: 0x06000218 RID: 536 RVA: 0x00008008 File Offset: 0x00006208
		public TwoAxisInputControl()
		{
			this.Left = new OneAxisInputControl();
			this.Right = new OneAxisInputControl();
			this.Up = new OneAxisInputControl();
			this.Down = new OneAxisInputControl();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008070 File Offset: 0x00006270
		public void ClearInputState()
		{
			this.Left.ClearInputState();
			this.Right.ClearInputState();
			this.Up.ClearInputState();
			this.Down.ClearInputState();
			this.lastState = false;
			this.lastValue = Vector2.zero;
			this.thisState = false;
			this.thisValue = Vector2.zero;
			this.X = 0f;
			this.Y = 0f;
			this.clearInputState = true;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000080EA File Offset: 0x000062EA
		public void Filter(TwoAxisInputControl twoAxisInputControl, float deltaTime)
		{
			this.UpdateWithAxes(twoAxisInputControl.X, twoAxisInputControl.Y, InputManager.CurrentTick, deltaTime);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008104 File Offset: 0x00006304
		internal void UpdateWithAxes(float x, float y, ulong updateTick, float deltaTime)
		{
			this.lastState = this.thisState;
			this.lastValue = this.thisValue;
			this.thisValue = (this.Raw ? new Vector2(x, y) : this.DeadZoneFunc(x, y, this.LowerDeadZone, this.UpperDeadZone));
			this.X = this.thisValue.x;
			this.Y = this.thisValue.y;
			this.Left.CommitWithValue(Mathf.Max(0f, -this.X), updateTick, deltaTime);
			this.Right.CommitWithValue(Mathf.Max(0f, this.X), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.Up.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
			}
			else
			{
				this.Up.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
			}
			this.thisState = this.Up.State || this.Down.State || this.Left.State || this.Right.State;
			if (this.clearInputState)
			{
				this.lastState = this.thisState;
				this.lastValue = this.thisValue;
				this.clearInputState = false;
				this.HasChanged = false;
				return;
			}
			if (this.thisValue != this.lastValue)
			{
				this.UpdateTick = updateTick;
				this.HasChanged = true;
				return;
			}
			this.HasChanged = false;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600021C RID: 540 RVA: 0x000082CE File Offset: 0x000064CE
		// (set) Token: 0x0600021D RID: 541 RVA: 0x000082D8 File Offset: 0x000064D8
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
				this.Left.Sensitivity = this.sensitivity;
				this.Right.Sensitivity = this.sensitivity;
				this.Up.Sensitivity = this.sensitivity;
				this.Down.Sensitivity = this.sensitivity;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00008335 File Offset: 0x00006535
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00008340 File Offset: 0x00006540
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
				this.Left.StateThreshold = this.stateThreshold;
				this.Right.StateThreshold = this.stateThreshold;
				this.Up.StateThreshold = this.stateThreshold;
				this.Down.StateThreshold = this.stateThreshold;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000839D File Offset: 0x0000659D
		// (set) Token: 0x06000221 RID: 545 RVA: 0x000083A8 File Offset: 0x000065A8
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
				this.Left.LowerDeadZone = this.lowerDeadZone;
				this.Right.LowerDeadZone = this.lowerDeadZone;
				this.Up.LowerDeadZone = this.lowerDeadZone;
				this.Down.LowerDeadZone = this.lowerDeadZone;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00008405 File Offset: 0x00006605
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00008410 File Offset: 0x00006610
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
				this.Left.UpperDeadZone = this.upperDeadZone;
				this.Right.UpperDeadZone = this.upperDeadZone;
				this.Up.UpperDeadZone = this.upperDeadZone;
				this.Down.UpperDeadZone = this.upperDeadZone;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000846D File Offset: 0x0000666D
		public bool State
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00008475 File Offset: 0x00006675
		public bool LastState
		{
			get
			{
				return this.lastState;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000847D File Offset: 0x0000667D
		public Vector2 Value
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00008485 File Offset: 0x00006685
		public Vector2 LastValue
		{
			get
			{
				return this.lastValue;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000848D File Offset: 0x0000668D
		public Vector2 Vector
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00008495 File Offset: 0x00006695
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000849D File Offset: 0x0000669D
		public bool HasChanged { get; protected set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000084A6 File Offset: 0x000066A6
		public bool IsPressed
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000084AE File Offset: 0x000066AE
		public bool WasPressed
		{
			get
			{
				return this.thisState && !this.lastState;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000084C3 File Offset: 0x000066C3
		public bool WasReleased
		{
			get
			{
				return !this.thisState && this.lastState;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000084D5 File Offset: 0x000066D5
		public float Angle
		{
			get
			{
				return Utility.VectorToAngle(this.thisValue);
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000084E2 File Offset: 0x000066E2
		public static implicit operator bool(TwoAxisInputControl instance)
		{
			return instance.thisState;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000084EA File Offset: 0x000066EA
		public static implicit operator Vector2(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000084F2 File Offset: 0x000066F2
		public static implicit operator Vector3(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x04000288 RID: 648
		public static readonly TwoAxisInputControl Null = new TwoAxisInputControl();

		// Token: 0x04000290 RID: 656
		public DeadZoneFunc DeadZoneFunc = new DeadZoneFunc(DeadZone.Circular);

		// Token: 0x04000291 RID: 657
		private float sensitivity = 1f;

		// Token: 0x04000292 RID: 658
		private float lowerDeadZone;

		// Token: 0x04000293 RID: 659
		private float upperDeadZone = 1f;

		// Token: 0x04000294 RID: 660
		private float stateThreshold;

		// Token: 0x04000295 RID: 661
		public bool Raw;

		// Token: 0x04000296 RID: 662
		private bool thisState;

		// Token: 0x04000297 RID: 663
		private bool lastState;

		// Token: 0x04000298 RID: 664
		private Vector2 thisValue;

		// Token: 0x04000299 RID: 665
		private Vector2 lastValue;

		// Token: 0x0400029A RID: 666
		private bool clearInputState;
	}
}
