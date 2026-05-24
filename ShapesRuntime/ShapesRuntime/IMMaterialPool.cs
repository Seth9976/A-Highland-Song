using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shapes
{
	// Token: 0x02000019 RID: 25
	internal static class IMMaterialPool
	{
		// Token: 0x06000931 RID: 2353 RVA: 0x00021A4D File Offset: 0x0001FC4D
		static IMMaterialPool()
		{
			SceneManager.sceneUnloaded += delegate(Scene scene)
			{
				IMMaterialPool.FlushAllMaterials();
			};
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00021A70 File Offset: 0x0001FC70
		internal static Material GetMaterial(ref RenderState state)
		{
			Material material;
			if (!IMMaterialPool.pool.TryGetValue(state, out material))
			{
				IMMaterialPool.pool.Add(state, material = state.CreateMaterial());
			}
			return material;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00021AAC File Offset: 0x0001FCAC
		private static void FlushAllMaterials()
		{
			foreach (Material material in IMMaterialPool.pool.Values)
			{
				if (material != null)
				{
					material.DestroyBranched();
				}
			}
			IMMaterialPool.pool.Clear();
		}

		// Token: 0x040000CA RID: 202
		public static Dictionary<RenderState, Material> pool = new Dictionary<RenderState, Material>();
	}
}
