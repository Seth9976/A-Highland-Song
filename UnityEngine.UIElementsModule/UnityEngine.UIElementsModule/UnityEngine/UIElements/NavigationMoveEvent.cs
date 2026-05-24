using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000203 RID: 515
	public class NavigationMoveEvent : NavigationEventBase<NavigationMoveEvent>
	{
		// Token: 0x06000FBA RID: 4026 RVA: 0x0003E5E0 File Offset: 0x0003C7E0
		internal static NavigationMoveEvent.Direction DetermineMoveDirection(float x, float y, float deadZone = 0.6f)
		{
			bool flag = new Vector2(x, y).sqrMagnitude < deadZone * deadZone;
			NavigationMoveEvent.Direction direction;
			if (flag)
			{
				direction = NavigationMoveEvent.Direction.None;
			}
			else
			{
				bool flag2 = Mathf.Abs(x) > Mathf.Abs(y);
				if (flag2)
				{
					bool flag3 = x > 0f;
					if (flag3)
					{
						direction = NavigationMoveEvent.Direction.Right;
					}
					else
					{
						direction = NavigationMoveEvent.Direction.Left;
					}
				}
				else
				{
					bool flag4 = y > 0f;
					if (flag4)
					{
						direction = NavigationMoveEvent.Direction.Up;
					}
					else
					{
						direction = NavigationMoveEvent.Direction.Down;
					}
				}
			}
			return direction;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0003E64B File Offset: 0x0003C84B
		// (set) Token: 0x06000FBC RID: 4028 RVA: 0x0003E653 File Offset: 0x0003C853
		public NavigationMoveEvent.Direction direction { get; private set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0003E65C File Offset: 0x0003C85C
		// (set) Token: 0x06000FBE RID: 4030 RVA: 0x0003E664 File Offset: 0x0003C864
		public Vector2 move { get; private set; }

		// Token: 0x06000FBF RID: 4031 RVA: 0x0003E670 File Offset: 0x0003C870
		public static NavigationMoveEvent GetPooled(Vector2 moveVector)
		{
			NavigationMoveEvent pooled = EventBase<NavigationMoveEvent>.GetPooled();
			pooled.direction = NavigationMoveEvent.DetermineMoveDirection(moveVector.x, moveVector.y, 0.6f);
			pooled.move = moveVector;
			return pooled;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0003E6AE File Offset: 0x0003C8AE
		protected override void Init()
		{
			base.Init();
			this.direction = NavigationMoveEvent.Direction.None;
			this.move = Vector2.zero;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0003E6CC File Offset: 0x0003C8CC
		public NavigationMoveEvent()
		{
			this.Init();
		}

		// Token: 0x02000204 RID: 516
		public enum Direction
		{
			// Token: 0x040006EC RID: 1772
			None,
			// Token: 0x040006ED RID: 1773
			Left,
			// Token: 0x040006EE RID: 1774
			Up,
			// Token: 0x040006EF RID: 1775
			Right,
			// Token: 0x040006F0 RID: 1776
			Down
		}
	}
}
