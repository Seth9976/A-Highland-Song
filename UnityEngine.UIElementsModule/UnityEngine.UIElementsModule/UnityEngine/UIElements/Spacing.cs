using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200006B RID: 107
	internal struct Spacing
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000B224 File Offset: 0x00009424
		public float horizontal
		{
			get
			{
				return this.left + this.right;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000B244 File Offset: 0x00009444
		public float vertical
		{
			get
			{
				return this.top + this.bottom;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000B263 File Offset: 0x00009463
		public Spacing(float left, float top, float right, float bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B284 File Offset: 0x00009484
		public static Rect operator +(Rect r, Spacing a)
		{
			r.x -= a.left;
			r.y -= a.top;
			r.width += a.horizontal;
			r.height += a.vertical;
			return r;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000B2F0 File Offset: 0x000094F0
		public static Rect operator -(Rect r, Spacing a)
		{
			r.x += a.left;
			r.y += a.top;
			r.width = Mathf.Max(0f, r.width - a.horizontal);
			r.height = Mathf.Max(0f, r.height - a.vertical);
			return r;
		}

		// Token: 0x0400015D RID: 349
		public float left;

		// Token: 0x0400015E RID: 350
		public float top;

		// Token: 0x0400015F RID: 351
		public float right;

		// Token: 0x04000160 RID: 352
		public float bottom;
	}
}
