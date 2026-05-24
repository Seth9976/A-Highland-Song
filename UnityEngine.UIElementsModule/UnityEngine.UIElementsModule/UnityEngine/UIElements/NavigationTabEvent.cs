using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000205 RID: 517
	internal class NavigationTabEvent : NavigationEventBase<NavigationTabEvent>
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0003E6DD File Offset: 0x0003C8DD
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x0003E6E5 File Offset: 0x0003C8E5
		public NavigationTabEvent.Direction direction { get; private set; }

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0003E6F0 File Offset: 0x0003C8F0
		internal static NavigationTabEvent.Direction DetermineMoveDirection(int moveValue)
		{
			return (moveValue > 0) ? NavigationTabEvent.Direction.Next : ((moveValue < 0) ? NavigationTabEvent.Direction.Previous : NavigationTabEvent.Direction.None);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0003E714 File Offset: 0x0003C914
		public static NavigationTabEvent GetPooled(int moveValue)
		{
			NavigationTabEvent pooled = EventBase<NavigationTabEvent>.GetPooled();
			pooled.direction = NavigationTabEvent.DetermineMoveDirection(moveValue);
			return pooled;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0003E73A File Offset: 0x0003C93A
		protected override void Init()
		{
			base.Init();
			this.direction = NavigationTabEvent.Direction.None;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003E74C File Offset: 0x0003C94C
		public NavigationTabEvent()
		{
			this.Init();
		}

		// Token: 0x02000206 RID: 518
		public enum Direction
		{
			// Token: 0x040006F3 RID: 1779
			None,
			// Token: 0x040006F4 RID: 1780
			Next,
			// Token: 0x040006F5 RID: 1781
			Previous
		}
	}
}
