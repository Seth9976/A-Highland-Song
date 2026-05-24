using System;
using UnityEngine;

namespace XInputDotNetPure
{
	// Token: 0x02000012 RID: 18
	public struct GamePadThumbSticks
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003DB5 File Offset: 0x00001FB5
		internal GamePadThumbSticks(GamePadThumbSticks.StickValue left, GamePadThumbSticks.StickValue right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003DC5 File Offset: 0x00001FC5
		public GamePadThumbSticks.StickValue Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003DCD File Offset: 0x00001FCD
		public GamePadThumbSticks.StickValue Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x0400007E RID: 126
		private GamePadThumbSticks.StickValue left;

		// Token: 0x0400007F RID: 127
		private GamePadThumbSticks.StickValue right;

		// Token: 0x02000216 RID: 534
		public struct StickValue
		{
			// Token: 0x0600094C RID: 2380 RVA: 0x00053235 File Offset: 0x00051435
			internal StickValue(float x, float y)
			{
				this.vector = new Vector2(x, y);
			}

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x0600094D RID: 2381 RVA: 0x00053244 File Offset: 0x00051444
			public float X
			{
				get
				{
					return this.vector.x;
				}
			}

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x0600094E RID: 2382 RVA: 0x00053251 File Offset: 0x00051451
			public float Y
			{
				get
				{
					return this.vector.y;
				}
			}

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x0600094F RID: 2383 RVA: 0x0005325E File Offset: 0x0005145E
			public Vector2 Vector
			{
				get
				{
					return this.vector;
				}
			}

			// Token: 0x040004A7 RID: 1191
			private Vector2 vector;
		}
	}
}
