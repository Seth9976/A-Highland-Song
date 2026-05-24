using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200005B RID: 91
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class ShapesColorFieldAttribute : PropertyAttribute
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x0002448B File Offset: 0x0002268B
		public ShapesColorFieldAttribute(bool showAlpha)
		{
			this.showAlpha = showAlpha;
		}

		// Token: 0x040001D6 RID: 470
		public readonly bool showAlpha = true;
	}
}
