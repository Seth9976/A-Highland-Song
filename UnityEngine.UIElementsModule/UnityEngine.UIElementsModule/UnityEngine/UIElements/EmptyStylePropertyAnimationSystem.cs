using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x020000B0 RID: 176
	internal class EmptyStylePropertyAnimationSystem : IStylePropertyAnimationSystem
	{
		// Token: 0x060005CF RID: 1487 RVA: 0x0001614C File Offset: 0x0001434C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, float startValue, float endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00016160 File Offset: 0x00014360
		public bool StartTransition(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00016174 File Offset: 0x00014374
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Length startValue, Length endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00016188 File Offset: 0x00014388
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Color startValue, Color endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001619C File Offset: 0x0001439C
		public bool StartAnimationEnum(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000161B0 File Offset: 0x000143B0
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Background startValue, Background endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000161C4 File Offset: 0x000143C4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, FontDefinition startValue, FontDefinition endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000161D8 File Offset: 0x000143D8
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Font startValue, Font endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000161EC File Offset: 0x000143EC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TextShadow startValue, TextShadow endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00016200 File Offset: 0x00014400
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Scale startValue, Scale endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00016214 File Offset: 0x00014414
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TransformOrigin startValue, TransformOrigin endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00016228 File Offset: 0x00014428
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Translate startValue, Translate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001623C File Offset: 0x0001443C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Rotate startValue, Rotate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000020E6 File Offset: 0x000002E6
		public void CancelAllAnimations()
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000020E6 File Offset: 0x000002E6
		public void CancelAllAnimations(VisualElement owner)
		{
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000020E6 File Offset: 0x000002E6
		public void CancelAnimation(VisualElement owner, StylePropertyId id)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00016250 File Offset: 0x00014450
		public bool HasRunningAnimation(VisualElement owner, StylePropertyId id)
		{
			return false;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000020E6 File Offset: 0x000002E6
		public void UpdateAnimation(VisualElement owner, StylePropertyId id)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000020E6 File Offset: 0x000002E6
		public void GetAllAnimations(VisualElement owner, List<StylePropertyId> propertyIds)
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000020E6 File Offset: 0x000002E6
		public void Update()
		{
		}
	}
}
