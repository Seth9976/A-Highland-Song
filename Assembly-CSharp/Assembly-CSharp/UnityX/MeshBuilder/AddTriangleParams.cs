using System;
using UnityEngine;

namespace UnityX.MeshBuilder
{
	// Token: 0x0200022A RID: 554
	public struct AddTriangleParams
	{
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0008B9B8 File Offset: 0x00089BB8
		public static AddTriangleParams standard
		{
			get
			{
				return new AddTriangleParams
				{
					front = true,
					uvTopLeft = new Vector2(0f, 1f),
					uvTopRight = new Vector2(1f, 1f),
					uvBottom = new Vector2(0.5f, 0f),
					colorTopLeft = Color.white,
					colorTopRight = Color.white,
					colorBottom = Color.white
				};
			}
		}

		// Token: 0x04001312 RID: 4882
		public bool front;

		// Token: 0x04001313 RID: 4883
		public bool back;

		// Token: 0x04001314 RID: 4884
		public Vector3 topLeft;

		// Token: 0x04001315 RID: 4885
		public Vector3 topRight;

		// Token: 0x04001316 RID: 4886
		public Vector3 bottom;

		// Token: 0x04001317 RID: 4887
		public Vector2 uvTopLeft;

		// Token: 0x04001318 RID: 4888
		public Vector2 uvTopRight;

		// Token: 0x04001319 RID: 4889
		public Vector2 uvBottom;

		// Token: 0x0400131A RID: 4890
		public Color colorTopLeft;

		// Token: 0x0400131B RID: 4891
		public Color colorTopRight;

		// Token: 0x0400131C RID: 4892
		public Color colorBottom;
	}
}
