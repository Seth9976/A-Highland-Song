using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C1 RID: 449
	public class ChangeEvent<T> : EventBase<ChangeEvent<T>>, IChangeEvent
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00039FA5 File Offset: 0x000381A5
		// (set) Token: 0x06000E22 RID: 3618 RVA: 0x00039FAD File Offset: 0x000381AD
		public T previousValue { get; protected set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00039FB6 File Offset: 0x000381B6
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x00039FBE File Offset: 0x000381BE
		public T newValue { get; protected set; }

		// Token: 0x06000E25 RID: 3621 RVA: 0x00039FC7 File Offset: 0x000381C7
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00039FD8 File Offset: 0x000381D8
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.previousValue = default(T);
			this.newValue = default(T);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0003A010 File Offset: 0x00038210
		public static ChangeEvent<T> GetPooled(T previousValue, T newValue)
		{
			ChangeEvent<T> pooled = EventBase<ChangeEvent<T>>.GetPooled();
			pooled.previousValue = previousValue;
			pooled.newValue = newValue;
			return pooled;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0003A039 File Offset: 0x00038239
		public ChangeEvent()
		{
			this.LocalInit();
		}
	}
}
