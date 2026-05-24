using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000039 RID: 57
	public class OneAxisInputControl : IInputControl
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000079DF File Offset: 0x00005BDF
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x000079E7 File Offset: 0x00005BE7
		public ulong UpdateTick { get; protected set; }

		// Token: 0x060001E5 RID: 485 RVA: 0x000079F0 File Offset: 0x00005BF0
		private void PrepareForUpdate(ulong updateTick)
		{
			if (this.isNullControl)
			{
				return;
			}
			if (updateTick < this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated with an earlier tick.");
			}
			if (this.pendingCommit && updateTick != this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated for a new tick until pending tick is committed.");
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007A64 File Offset: 0x00005C64
		public bool UpdateWithState(bool state, ulong updateTick, float deltaTime)
		{
			if (this.isNullControl)
			{
				return false;
			}
			this.PrepareForUpdate(updateTick);
			this.nextState.Set(state || this.nextState.State);
			return state;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007A94 File Offset: 0x00005C94
		public bool UpdateWithValue(float value, ulong updateTick, float deltaTime)
		{
			if (this.isNullControl)
			{
				return false;
			}
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				if (!this.Raw)
				{
					value = Utility.ApplyDeadZone(value, this.lowerDeadZone, this.upperDeadZone);
				}
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007B08 File Offset: 0x00005D08
		internal bool UpdateWithRawValue(float value, ulong updateTick, float deltaTime)
		{
			if (this.isNullControl)
			{
				return false;
			}
			this.Raw = true;
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007B68 File Offset: 0x00005D68
		internal void SetValue(float value, ulong updateTick)
		{
			if (this.isNullControl)
			{
				return;
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
			this.nextState.RawValue = value;
			this.nextState.Set(value, this.StateThreshold);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007BCA File Offset: 0x00005DCA
		public void ClearInputState()
		{
			this.lastState.Reset();
			this.thisState.Reset();
			this.nextState.Reset();
			this.wasRepeated = false;
			this.clearInputState = true;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007BFC File Offset: 0x00005DFC
		public void Commit()
		{
			if (this.isNullControl)
			{
				return;
			}
			this.pendingCommit = false;
			this.thisState = this.nextState;
			if (this.clearInputState)
			{
				this.lastState = this.nextState;
				this.UpdateTick = this.pendingTick;
				this.clearInputState = false;
				return;
			}
			bool state = this.lastState.State;
			bool state2 = this.thisState.State;
			this.wasRepeated = false;
			if (state && !state2)
			{
				this.nextRepeatTime = 0f;
			}
			else if (state2)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (!state)
				{
					this.nextRepeatTime = realtimeSinceStartup + this.FirstRepeatDelay;
				}
				else if (realtimeSinceStartup >= this.nextRepeatTime)
				{
					this.wasRepeated = true;
					this.nextRepeatTime = realtimeSinceStartup + this.RepeatDelay;
				}
			}
			if (this.thisState != this.lastState)
			{
				this.UpdateTick = this.pendingTick;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007CD8 File Offset: 0x00005ED8
		public void CommitWithState(bool state, ulong updateTick, float deltaTime)
		{
			this.UpdateWithState(state, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007CEA File Offset: 0x00005EEA
		public void CommitWithValue(float value, ulong updateTick, float deltaTime)
		{
			this.UpdateWithValue(value, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007CFC File Offset: 0x00005EFC
		internal void CommitWithSides(InputControl negativeSide, InputControl positiveSide, ulong updateTick, float deltaTime)
		{
			this.LowerDeadZone = Mathf.Max(negativeSide.LowerDeadZone, positiveSide.LowerDeadZone);
			this.UpperDeadZone = Mathf.Min(negativeSide.UpperDeadZone, positiveSide.UpperDeadZone);
			this.Raw = negativeSide.Raw || positiveSide.Raw;
			float num = Utility.ValueFromSides(negativeSide.RawValue, positiveSide.RawValue);
			this.CommitWithValue(num, updateTick, deltaTime);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00007D6A File Offset: 0x00005F6A
		public bool State
		{
			get
			{
				return this.EnabledInHierarchy && this.thisState.State;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00007D81 File Offset: 0x00005F81
		public bool LastState
		{
			get
			{
				return this.EnabledInHierarchy && this.lastState.State;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00007D98 File Offset: 0x00005F98
		public float Value
		{
			get
			{
				if (!this.EnabledInHierarchy)
				{
					return 0f;
				}
				return this.thisState.Value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00007DB3 File Offset: 0x00005FB3
		public float LastValue
		{
			get
			{
				if (!this.EnabledInHierarchy)
				{
					return 0f;
				}
				return this.lastState.Value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00007DCE File Offset: 0x00005FCE
		public float RawValue
		{
			get
			{
				if (!this.EnabledInHierarchy)
				{
					return 0f;
				}
				return this.thisState.RawValue;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00007DE9 File Offset: 0x00005FE9
		internal float NextRawValue
		{
			get
			{
				if (!this.EnabledInHierarchy)
				{
					return 0f;
				}
				return this.nextState.RawValue;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00007E04 File Offset: 0x00006004
		internal bool HasInput
		{
			get
			{
				return this.EnabledInHierarchy && Utility.IsNotZero(this.thisState.Value);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00007E20 File Offset: 0x00006020
		public bool HasChanged
		{
			get
			{
				return this.EnabledInHierarchy && this.thisState != this.lastState;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00007E3D File Offset: 0x0000603D
		public bool IsPressed
		{
			get
			{
				return this.EnabledInHierarchy && this.thisState.State;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00007E54 File Offset: 0x00006054
		public bool WasPressed
		{
			get
			{
				return this.EnabledInHierarchy && this.thisState && !this.lastState;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00007E7B File Offset: 0x0000607B
		public bool WasReleased
		{
			get
			{
				return this.EnabledInHierarchy && !this.thisState && this.lastState;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007E9F File Offset: 0x0000609F
		public bool WasRepeated
		{
			get
			{
				return this.EnabledInHierarchy && this.wasRepeated;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00007EB1 File Offset: 0x000060B1
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00007EB9 File Offset: 0x000060B9
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00007EC7 File Offset: 0x000060C7
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00007ECF File Offset: 0x000060CF
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00007EDD File Offset: 0x000060DD
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00007EE5 File Offset: 0x000060E5
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007EF3 File Offset: 0x000060F3
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00007EFB File Offset: 0x000060FB
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00007F09 File Offset: 0x00006109
		public bool IsNullControl
		{
			get
			{
				return this.isNullControl;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00007F11 File Offset: 0x00006111
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00007F19 File Offset: 0x00006119
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00007F22 File Offset: 0x00006122
		public bool EnabledInHierarchy
		{
			get
			{
				return this.enabled && this.ownerEnabled;
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00007F34 File Offset: 0x00006134
		public static implicit operator bool(OneAxisInputControl instance)
		{
			return instance.State;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007F3C File Offset: 0x0000613C
		public static implicit operator float(OneAxisInputControl instance)
		{
			return instance.Value;
		}

		// Token: 0x04000276 RID: 630
		private float sensitivity = 1f;

		// Token: 0x04000277 RID: 631
		private float lowerDeadZone;

		// Token: 0x04000278 RID: 632
		private float upperDeadZone = 1f;

		// Token: 0x04000279 RID: 633
		private float stateThreshold;

		// Token: 0x0400027A RID: 634
		protected bool isNullControl;

		// Token: 0x0400027B RID: 635
		public float FirstRepeatDelay = 0.8f;

		// Token: 0x0400027C RID: 636
		public float RepeatDelay = 0.1f;

		// Token: 0x0400027D RID: 637
		public bool Raw;

		// Token: 0x0400027E RID: 638
		private bool enabled = true;

		// Token: 0x0400027F RID: 639
		protected bool ownerEnabled = true;

		// Token: 0x04000280 RID: 640
		private ulong pendingTick;

		// Token: 0x04000281 RID: 641
		private bool pendingCommit;

		// Token: 0x04000282 RID: 642
		private float nextRepeatTime;

		// Token: 0x04000283 RID: 643
		private bool wasRepeated;

		// Token: 0x04000284 RID: 644
		private bool clearInputState;

		// Token: 0x04000285 RID: 645
		private InputControlState lastState;

		// Token: 0x04000286 RID: 646
		private InputControlState nextState;

		// Token: 0x04000287 RID: 647
		private InputControlState thisState;
	}
}
