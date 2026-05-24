using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200014F RID: 335
[NullableContext(1)]
[Nullable(0)]
public class StaminaEdgeVignette : MonoBehaviour
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0005B8BE File Offset: 0x00059ABE
	private SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0005B8E0 File Offset: 0x00059AE0
	private void OnEnable()
	{
		this.layout.alpha = (this._fade = 0f);
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0005B908 File Offset: 0x00059B08
	private void Update()
	{
		Runner instance = Runner.instance;
		if (instance == null)
		{
			return;
		}
		this._fade = Mathf.MoveTowards(this._fade, (float)(instance.staminaIsLow ? 1 : 0), Time.deltaTime / this.settings.vignette.fadeTime);
		float num = (instance.staminaIsVeryLow ? this.settings.vignette.flashPeriodFast : this.settings.vignette.flashPeriod);
		Range range = (instance.staminaIsVeryLow ? this.settings.vignette.flashAlphaFast : this.settings.vignette.flashAlpha);
		float num2 = (this._flashUpwards ? range.max : range.min);
		float num3 = 0.5f * Time.deltaTime / num;
		this._flashAlpha = Mathf.MoveTowards(this._flashAlpha, num2, num3);
		if (this._flashAlpha == num2)
		{
			this._flashUpwards = !this._flashUpwards;
		}
		this.layout.alpha = this._fade * this._flashAlpha * this.settings.vignette.fadeMasterAlpha;
	}

	// Token: 0x04000D7F RID: 3455
	public StaminaSettings settings = Presume<StaminaSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\UI\\StaminaEdgeVignette.cs", 9);

	// Token: 0x04000D80 RID: 3456
	[Nullable(2)]
	private SLayout _layout;

	// Token: 0x04000D81 RID: 3457
	private float _fade;

	// Token: 0x04000D82 RID: 3458
	private float _flashAlpha;

	// Token: 0x04000D83 RID: 3459
	private bool _flashUpwards;
}
