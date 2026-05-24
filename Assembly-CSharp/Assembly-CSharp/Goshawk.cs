using System;

// Token: 0x0200018C RID: 396
public class Goshawk : MonoInstancer<Goshawk>
{
	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06000CEF RID: 3311 RVA: 0x000679F8 File Offset: 0x00065BF8
	public Bird bird
	{
		get
		{
			if (this._bird == null)
			{
				this._bird = base.GetComponentInChildren<Bird>();
			}
			return this._bird;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00067A1A File Offset: 0x00065C1A
	public Flock flock
	{
		get
		{
			if (this._flock == null)
			{
				this._flock = base.GetComponent<Flock>();
			}
			return this._flock;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00067A3C File Offset: 0x00065C3C
	public string guid
	{
		get
		{
			if (this._guid == null)
			{
				GuidComponent component = base.GetComponent<GuidComponent>();
				this._guid = component.GetGuid().ToString();
			}
			return this._guid;
		}
	}

	// Token: 0x04000FC3 RID: 4035
	private Bird _bird;

	// Token: 0x04000FC4 RID: 4036
	private Flock _flock;

	// Token: 0x04000FC5 RID: 4037
	private string _guid;
}
