using System;

namespace InControl
{
	// Token: 0x02000028 RID: 40
	public class PlayerTwoAxisAction : TwoAxisInputControl
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000675F File Offset: 0x0000495F
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00006767 File Offset: 0x00004967
		public bool InvertXAxis { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006770 File Offset: 0x00004970
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00006778 File Offset: 0x00004978
		public bool InvertYAxis { get; set; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600015A RID: 346 RVA: 0x00006784 File Offset: 0x00004984
		// (remove) Token: 0x0600015B RID: 347 RVA: 0x000067BC File Offset: 0x000049BC
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000067F1 File Offset: 0x000049F1
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000067F9 File Offset: 0x000049F9
		public object UserData { get; set; }

		// Token: 0x0600015E RID: 350 RVA: 0x00006802 File Offset: 0x00004A02
		internal PlayerTwoAxisAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			this.negativeXAction = negativeXAction;
			this.positiveXAction = positiveXAction;
			this.negativeYAction = negativeYAction;
			this.positiveYAction = positiveYAction;
			this.InvertXAxis = false;
			this.InvertYAxis = false;
			this.Raw = true;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000683C File Offset: 0x00004A3C
		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeXAction);
			this.ProcessActionUpdate(this.positiveXAction);
			this.ProcessActionUpdate(this.negativeYAction);
			this.ProcessActionUpdate(this.positiveYAction);
			float num = Utility.ValueFromSides(this.negativeXAction, this.positiveXAction, this.InvertXAxis);
			float num2 = Utility.ValueFromSides(this.negativeYAction, this.positiveYAction, InputManager.InvertYAxis || this.InvertYAxis);
			base.UpdateWithAxes(num, num2, updateTick, deltaTime);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000068D4 File Offset: 0x00004AD4
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000692D File Offset: 0x00004B2D
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00006934 File Offset: 0x00004B34
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006936 File Offset: 0x00004B36
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000693D File Offset: 0x00004B3D
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

		// Token: 0x04000182 RID: 386
		private PlayerAction negativeXAction;

		// Token: 0x04000183 RID: 387
		private PlayerAction positiveXAction;

		// Token: 0x04000184 RID: 388
		private PlayerAction negativeYAction;

		// Token: 0x04000185 RID: 389
		private PlayerAction positiveYAction;

		// Token: 0x04000188 RID: 392
		public BindingSourceType LastInputType;
	}
}
