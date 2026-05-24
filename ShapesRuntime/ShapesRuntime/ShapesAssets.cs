using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200005A RID: 90
	public class ShapesAssets : ScriptableObject
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00024404 File Offset: 0x00022604
		public static ShapesAssets Instance
		{
			get
			{
				return ShapesAssets.StaticLoader.instance;
			}
		}

		// Token: 0x040001CB RID: 459
		[Header("Config")]
		public TMP_FontAsset defaultFont;

		// Token: 0x040001CC RID: 460
		[Header("Meshes")]
		public Mesh[] meshQuad = new Mesh[5];

		// Token: 0x040001CD RID: 461
		public Mesh[] meshTriangle = new Mesh[5];

		// Token: 0x040001CE RID: 462
		public Mesh[] meshCube = new Mesh[5];

		// Token: 0x040001CF RID: 463
		public Mesh[] meshSphere = new Mesh[5];

		// Token: 0x040001D0 RID: 464
		public Mesh[] meshTorus = new Mesh[5];

		// Token: 0x040001D1 RID: 465
		public Mesh[] meshCapsule = new Mesh[5];

		// Token: 0x040001D2 RID: 466
		public Mesh[] meshCylinder = new Mesh[5];

		// Token: 0x040001D3 RID: 467
		public Mesh[] meshCone = new Mesh[5];

		// Token: 0x040001D4 RID: 468
		public Mesh[] meshConeUncapped = new Mesh[5];

		// Token: 0x040001D5 RID: 469
		[Header("Misc")]
		public TextAsset packageJson;

		// Token: 0x02000078 RID: 120
		private static class StaticLoader
		{
			// Token: 0x0400028A RID: 650
			public static readonly ShapesAssets instance = Resources.Load<ShapesAssets>("Shapes Assets");
		}
	}
}
