using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000063 RID: 99
	public class TouchPool
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x00011538 File Offset: 0x0000F738
		public TouchPool(int capacity)
		{
			this.freeTouches = new List<Touch>(capacity);
			for (int i = 0; i < capacity; i++)
			{
				this.freeTouches.Add(new Touch());
			}
			this.usedTouches = new List<Touch>(capacity);
			this.Touches = new ReadOnlyCollection<Touch>(this.usedTouches);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00011590 File Offset: 0x0000F790
		public TouchPool()
			: this(16)
		{
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001159C File Offset: 0x0000F79C
		public Touch FindOrCreateTouch(int fingerId)
		{
			int count = this.usedTouches.Count;
			Touch touch;
			for (int i = 0; i < count; i++)
			{
				touch = this.usedTouches[i];
				if (touch.fingerId == fingerId)
				{
					return touch;
				}
			}
			touch = this.NewTouch();
			touch.fingerId = fingerId;
			this.usedTouches.Add(touch);
			return touch;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000115F4 File Offset: 0x0000F7F4
		public Touch FindTouch(int fingerId)
		{
			int count = this.usedTouches.Count;
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.usedTouches[i];
				if (touch.fingerId == fingerId)
				{
					return touch;
				}
			}
			return null;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00011634 File Offset: 0x0000F834
		private Touch NewTouch()
		{
			int count = this.freeTouches.Count;
			if (count > 0)
			{
				Touch touch = this.freeTouches[count - 1];
				this.freeTouches.RemoveAt(count - 1);
				return touch;
			}
			return new Touch();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00011673 File Offset: 0x0000F873
		public void FreeTouch(Touch touch)
		{
			touch.Reset();
			this.freeTouches.Add(touch);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00011688 File Offset: 0x0000F888
		public void FreeEndedTouches()
		{
			for (int i = this.usedTouches.Count - 1; i >= 0; i--)
			{
				if (this.usedTouches[i].phase == TouchPhase.Ended)
				{
					this.usedTouches.RemoveAt(i);
				}
			}
		}

		// Token: 0x0400043A RID: 1082
		public readonly ReadOnlyCollection<Touch> Touches;

		// Token: 0x0400043B RID: 1083
		private List<Touch> usedTouches;

		// Token: 0x0400043C RID: 1084
		private List<Touch> freeTouches;
	}
}
