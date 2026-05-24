using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E5 RID: 485
	public class InputEvent : EventBase<InputEvent>
	{
		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0003C86D File Offset: 0x0003AA6D
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0003C875 File Offset: 0x0003AA75
		public string previousData { get; protected set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0003C87E File Offset: 0x0003AA7E
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0003C886 File Offset: 0x0003AA86
		public string newData { get; protected set; }

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003C88F File Offset: 0x0003AA8F
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0003C8A0 File Offset: 0x0003AAA0
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.previousData = null;
			this.newData = null;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003C8BC File Offset: 0x0003AABC
		public static InputEvent GetPooled(string previousData, string newData)
		{
			InputEvent pooled = EventBase<InputEvent>.GetPooled();
			pooled.previousData = previousData;
			pooled.newData = newData;
			return pooled;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0003C8E5 File Offset: 0x0003AAE5
		public InputEvent()
		{
			this.LocalInit();
		}
	}
}
