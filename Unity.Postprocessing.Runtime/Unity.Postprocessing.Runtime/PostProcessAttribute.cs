using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class PostProcessAttribute : Attribute
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002093 File Offset: 0x00000293
		public PostProcessAttribute(Type renderer, PostProcessEvent eventType, string menuItem, bool allowInSceneView = true)
		{
			this.renderer = renderer;
			this.eventType = eventType;
			this.menuItem = menuItem;
			this.allowInSceneView = allowInSceneView;
			this.builtinEffect = false;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020BF File Offset: 0x000002BF
		internal PostProcessAttribute(Type renderer, string menuItem, bool allowInSceneView = true)
		{
			this.renderer = renderer;
			this.menuItem = menuItem;
			this.allowInSceneView = allowInSceneView;
			this.builtinEffect = true;
		}

		// Token: 0x04000006 RID: 6
		public readonly Type renderer;

		// Token: 0x04000007 RID: 7
		public readonly PostProcessEvent eventType;

		// Token: 0x04000008 RID: 8
		public readonly string menuItem;

		// Token: 0x04000009 RID: 9
		public readonly bool allowInSceneView;

		// Token: 0x0400000A RID: 10
		internal readonly bool builtinEffect;
	}
}
