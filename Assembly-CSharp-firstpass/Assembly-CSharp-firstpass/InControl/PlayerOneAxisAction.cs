using System;

namespace InControl
{
	// Token: 0x02000027 RID: 39
	public class PlayerOneAxisAction : OneAxisInputControl
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600014B RID: 331 RVA: 0x0000660C File Offset: 0x0000480C
		// (remove) Token: 0x0600014C RID: 332 RVA: 0x00006644 File Offset: 0x00004844
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006679 File Offset: 0x00004879
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00006681 File Offset: 0x00004881
		public object UserData { get; set; }

		// Token: 0x0600014F RID: 335 RVA: 0x0000668A File Offset: 0x0000488A
		internal PlayerOneAxisAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			this.negativeAction = negativeAction;
			this.positiveAction = positiveAction;
			this.Raw = true;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000066A8 File Offset: 0x000048A8
		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeAction);
			this.ProcessActionUpdate(this.positiveAction);
			float num = Utility.ValueFromSides(this.negativeAction, this.positiveAction);
			base.CommitWithValue(num, updateTick, deltaTime);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000066F4 File Offset: 0x000048F4
		private void ProcessActionUpdate(PlayerAction action)
		{
			BindingSourceType bindingSourceType = this.LastInputType;
			if (action.UpdateTick > base.UpdateTick)
			{
				base.UpdateTick = action.UpdateTick;
				bindingSourceType = action.LastInputType;
			}
			if (this.LastInputType != bindingSourceType)
			{
				this.LastInputType = bindingSourceType;
				if (this.OnLastInputTypeChanged != null)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000674D File Offset: 0x0000494D
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00006754 File Offset: 0x00004954
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float LowerDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00006756 File Offset: 0x00004956
		// (set) Token: 0x06000155 RID: 341 RVA: 0x0000675D File Offset: 0x0000495D
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float UpperDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x0400017D RID: 381
		private PlayerAction negativeAction;

		// Token: 0x0400017E RID: 382
		private PlayerAction positiveAction;

		// Token: 0x0400017F RID: 383
		public BindingSourceType LastInputType;
	}
}
