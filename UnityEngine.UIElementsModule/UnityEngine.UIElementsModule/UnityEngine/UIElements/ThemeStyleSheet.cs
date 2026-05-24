using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B3 RID: 691
	[Serializable]
	public class ThemeStyleSheet : StyleSheet
	{
		// Token: 0x06001732 RID: 5938 RVA: 0x0005EC7B File Offset: 0x0005CE7B
		internal override void OnEnable()
		{
			base.isDefaultStyleSheet = true;
			base.OnEnable();
		}
	}
}
