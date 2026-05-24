using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000064 RID: 100
	public class ShapesTextDrawer : MonoBehaviour
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00026865 File Offset: 0x00024A65
		public static ShapesTextDrawer Instance
		{
			get
			{
				if (ShapesTextDrawer.instance == null)
				{
					ShapesTextDrawer.instance = Object.FindObjectOfType<ShapesTextDrawer>();
					if (ShapesTextDrawer.instance == null)
					{
						ShapesTextDrawer.instance = ShapesTextDrawer.Create();
					}
				}
				return ShapesTextDrawer.instance;
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002689C File Offset: 0x00024A9C
		private static ShapesTextDrawer Create()
		{
			GameObject gameObject = new GameObject("TEXT DRAWER");
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			ShapesTextDrawer shapesTextDrawer = gameObject.AddComponent<ShapesTextDrawer>();
			shapesTextDrawer.tmp = gameObject.AddComponent<TextMeshPro>();
			shapesTextDrawer.tmp.enableWordWrapping = false;
			shapesTextDrawer.tmp.overflowMode = TextOverflowModes.Overflow;
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			ShapesTextDrawer.Hide(new Object[] { gameObject });
			return shapesTextDrawer;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00026908 File Offset: 0x00024B08
		private static void Hide(params Object[] objs)
		{
			objs.ForEach(delegate(Object o)
			{
				o.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor;
			});
		}

		// Token: 0x04000247 RID: 583
		private static ShapesTextDrawer instance;

		// Token: 0x04000248 RID: 584
		public TextMeshPro tmp;
	}
}
