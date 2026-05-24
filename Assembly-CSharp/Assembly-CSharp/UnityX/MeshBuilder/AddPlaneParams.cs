using System;
using UnityEngine;

namespace UnityX.MeshBuilder
{
	// Token: 0x02000229 RID: 553
	public struct AddPlaneParams
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x0008B8A4 File Offset: 0x00089AA4
		public static AddPlaneParams standard
		{
			get
			{
				return new AddPlaneParams
				{
					front = true,
					topLeft = new Vector3(-0.5f, 0.5f, 0f),
					topRight = new Vector3(0.5f, 0.5f, 0f),
					bottomRight = new Vector3(0.5f, -0.5f, 0f),
					bottomLeft = new Vector3(-0.5f, -0.5f, 0f),
					uvTopLeft = new Vector2(0f, 1f),
					uvTopRight = new Vector2(1f, 1f),
					uvBottomRight = new Vector2(1f, 0f),
					uvBottomLeft = new Vector2(0f, 0f),
					colorTopLeft = Color.white,
					colorTopRight = Color.white,
					colorBottomRight = Color.white,
					colorBottomLeft = Color.white
				};
			}
		}

		// Token: 0x04001304 RID: 4868
		public bool front;

		// Token: 0x04001305 RID: 4869
		public bool back;

		// Token: 0x04001306 RID: 4870
		public Vector3 topLeft;

		// Token: 0x04001307 RID: 4871
		public Vector3 topRight;

		// Token: 0x04001308 RID: 4872
		public Vector3 bottomRight;

		// Token: 0x04001309 RID: 4873
		public Vector3 bottomLeft;

		// Token: 0x0400130A RID: 4874
		public Vector2 uvTopLeft;

		// Token: 0x0400130B RID: 4875
		public Vector2 uvTopRight;

		// Token: 0x0400130C RID: 4876
		public Vector2 uvBottomRight;

		// Token: 0x0400130D RID: 4877
		public Vector2 uvBottomLeft;

		// Token: 0x0400130E RID: 4878
		public Color colorTopLeft;

		// Token: 0x0400130F RID: 4879
		public Color colorTopRight;

		// Token: 0x04001310 RID: 4880
		public Color colorBottomRight;

		// Token: 0x04001311 RID: 4881
		public Color colorBottomLeft;
	}
}
