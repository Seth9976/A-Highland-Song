using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000B RID: 11
	[ExecuteAlways]
	public class ShapeGroup : MonoBehaviour
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004959 File Offset: 0x00002B59
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00004961 File Offset: 0x00002B61
		internal bool IsEnabled { get; private set; }

		// Token: 0x0600012D RID: 301 RVA: 0x0000496A File Offset: 0x00002B6A
		private void OnEnable()
		{
			ShapeGroup.shapeGroupsInScene++;
			this.IsEnabled = true;
			this.UpdateChildShapes();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004985 File Offset: 0x00002B85
		private void OnDisable()
		{
			ShapeGroup.shapeGroupsInScene--;
			this.IsEnabled = false;
			this.UpdateChildShapes();
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000049A0 File Offset: 0x00002BA0
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000049A8 File Offset: 0x00002BA8
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.UpdateChildShapes();
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000049B7 File Offset: 0x00002BB7
		private void OnValidate()
		{
			this.UpdateChildShapes();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000049C0 File Offset: 0x00002BC0
		private void UpdateChildShapes()
		{
			ShapeRenderer[] componentsInChildren = base.GetComponentsInChildren<ShapeRenderer>();
			if (componentsInChildren != null)
			{
				ShapeRenderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].UpdateAllMaterialProperties();
				}
			}
		}

		// Token: 0x04000046 RID: 70
		public static int shapeGroupsInScene;

		// Token: 0x04000047 RID: 71
		[ShapesColorField(true)]
		[SerializeField]
		private Color color = Color.white;
	}
}
