using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class ReadyToSprintUI : MonoBehaviour
{
	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0005872B File Offset: 0x0005692B
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

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0005874D File Offset: 0x0005694D
	private void Start()
	{
		this.layout.groupAlpha = 0f;
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00058760 File Offset: 0x00056960
	private void Update()
	{
		if (MonoSingleton<RunTrack>.instance.readyToSprint && this.layout.targetGroupAlpha == 0f)
		{
			this.layout.CancelAnimations();
			this.layout.groupAlpha = 1f;
			return;
		}
		if (!MonoSingleton<RunTrack>.instance.readyToSprint && this.layout.targetGroupAlpha > 0f)
		{
			this.layout.Animate(0.2f, delegate
			{
				this.layout.groupAlpha = 0f;
			});
		}
	}

	// Token: 0x04000D01 RID: 3329
	private SLayout _layout;
}
