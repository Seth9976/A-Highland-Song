using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000221 RID: 545
	internal class PropagationPaths
	{
		// Token: 0x0600106C RID: 4204 RVA: 0x0004000C File Offset: 0x0003E20C
		public PropagationPaths()
		{
			this.trickleDownPath = new List<VisualElement>(16);
			this.targetElements = new List<VisualElement>(4);
			this.bubbleUpPath = new List<VisualElement>(16);
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0004003C File Offset: 0x0003E23C
		public PropagationPaths(PropagationPaths paths)
		{
			this.trickleDownPath = new List<VisualElement>(paths.trickleDownPath);
			this.targetElements = new List<VisualElement>(paths.targetElements);
			this.bubbleUpPath = new List<VisualElement>(paths.bubbleUpPath);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0004007C File Offset: 0x0003E27C
		internal static PropagationPaths Copy(PropagationPaths paths)
		{
			PropagationPaths propagationPaths = PropagationPaths.s_Pool.Get();
			propagationPaths.trickleDownPath.AddRange(paths.trickleDownPath);
			propagationPaths.targetElements.AddRange(paths.targetElements);
			propagationPaths.bubbleUpPath.AddRange(paths.bubbleUpPath);
			return propagationPaths;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000400D0 File Offset: 0x0003E2D0
		public static PropagationPaths Build(VisualElement elem, EventBase evt, PropagationPaths.Type pathTypesRequested)
		{
			bool flag = elem == null || pathTypesRequested == PropagationPaths.Type.None;
			PropagationPaths propagationPaths;
			if (flag)
			{
				propagationPaths = null;
			}
			else
			{
				PropagationPaths propagationPaths2 = PropagationPaths.s_Pool.Get();
				propagationPaths2.targetElements.Add(elem);
				while (elem.hierarchy.parent != null)
				{
					elem = elem.hierarchy.parent;
					bool flag2 = elem.isCompositeRoot && !evt.ignoreCompositeRoots;
					if (flag2)
					{
						propagationPaths2.targetElements.Add(elem);
					}
					else
					{
						bool flag3 = (pathTypesRequested & PropagationPaths.Type.TrickleDown) == PropagationPaths.Type.TrickleDown && elem.HasTrickleDownHandlers();
						if (flag3)
						{
							propagationPaths2.trickleDownPath.Add(elem);
						}
						bool flag4 = (pathTypesRequested & PropagationPaths.Type.BubbleUp) == PropagationPaths.Type.BubbleUp && elem.HasBubbleUpHandlers();
						if (flag4)
						{
							propagationPaths2.bubbleUpPath.Add(elem);
						}
					}
				}
				propagationPaths = propagationPaths2;
			}
			return propagationPaths;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000401B6 File Offset: 0x0003E3B6
		public void Release()
		{
			this.bubbleUpPath.Clear();
			this.targetElements.Clear();
			this.trickleDownPath.Clear();
			PropagationPaths.s_Pool.Release(this);
		}

		// Token: 0x04000722 RID: 1826
		private static readonly ObjectPool<PropagationPaths> s_Pool = new ObjectPool<PropagationPaths>(100);

		// Token: 0x04000723 RID: 1827
		public readonly List<VisualElement> trickleDownPath;

		// Token: 0x04000724 RID: 1828
		public readonly List<VisualElement> targetElements;

		// Token: 0x04000725 RID: 1829
		public readonly List<VisualElement> bubbleUpPath;

		// Token: 0x04000726 RID: 1830
		private const int k_DefaultPropagationDepth = 16;

		// Token: 0x04000727 RID: 1831
		private const int k_DefaultTargetCount = 4;

		// Token: 0x02000222 RID: 546
		[Flags]
		public enum Type
		{
			// Token: 0x04000729 RID: 1833
			None = 0,
			// Token: 0x0400072A RID: 1834
			TrickleDown = 1,
			// Token: 0x0400072B RID: 1835
			BubbleUp = 2
		}
	}
}
