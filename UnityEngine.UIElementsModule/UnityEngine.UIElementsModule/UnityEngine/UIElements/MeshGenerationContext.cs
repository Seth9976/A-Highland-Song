using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025D RID: 605
	public class MeshGenerationContext
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x0004758C File Offset: 0x0004578C
		public VisualElement visualElement
		{
			get
			{
				return this.painter.visualElement;
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000475A9 File Offset: 0x000457A9
		internal MeshGenerationContext(IStylePainter painter)
		{
			this.painter = painter;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000475BC File Offset: 0x000457BC
		public MeshWriteData Allocate(int vertexCount, int indexCount, Texture texture = null)
		{
			return this.painter.DrawMesh(vertexCount, indexCount, texture, null, MeshGenerationContext.MeshFlags.None);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000475E0 File Offset: 0x000457E0
		internal MeshWriteData Allocate(int vertexCount, int indexCount, Texture texture, Material material, MeshGenerationContext.MeshFlags flags)
		{
			return this.painter.DrawMesh(vertexCount, indexCount, texture, material, flags);
		}

		// Token: 0x0400085B RID: 2139
		internal IStylePainter painter;

		// Token: 0x0200025E RID: 606
		[Flags]
		internal enum MeshFlags
		{
			// Token: 0x0400085D RID: 2141
			None = 0,
			// Token: 0x0400085E RID: 2142
			UVisDisplacement = 1,
			// Token: 0x0400085F RID: 2143
			SkipDynamicAtlas = 2
		}
	}
}
