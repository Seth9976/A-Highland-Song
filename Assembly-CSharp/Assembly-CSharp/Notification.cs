using System;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class Notification : MonoBehaviour
{
	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0005524C File Offset: 0x0005344C
	public bool expired
	{
		get
		{
			return this.begun && Time.unscaledTime > this._creationTime + this._settings.visibleTime;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00055271 File Offset: 0x00053471
	private bool begun
	{
		get
		{
			return this._creationTime > 0f;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00055280 File Offset: 0x00053480
	public SLayout layout
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

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000A21 RID: 2593 RVA: 0x000552A2 File Offset: 0x000534A2
	public Prototype prototype
	{
		get
		{
			if (this._prototype == null)
			{
				this._prototype = base.GetComponent<Prototype>();
			}
			return this._prototype;
		}
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x000552C4 File Offset: 0x000534C4
	public void Setup(string title, string body, string body2, NotificationType type, string inkName, Sprite sprite)
	{
		this.type = type;
		this.inkName = inkName;
		this._title.textMeshPro.text = title;
		if (this._image != null)
		{
			this._image.rotation = 0f;
			if (sprite != null)
			{
				this._image.image.sprite = sprite;
			}
		}
		this._bodyText = body;
		this._bodyText2 = body2;
		if (this._defaultHeight == 0f)
		{
			this._defaultHeight = this.layout.height;
		}
		else
		{
			this.layout.height = this._defaultHeight;
		}
		this._creationTime = 0f;
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00055378 File Offset: 0x00053578
	public void Begin()
	{
		this._creationTime = Time.unscaledTime;
		this._body.Setup(this._bodyText, NarrativePresenter.instance.settings.textReadSettings, null);
		AnimatedTextView bottomItem = this._body;
		if (this._bodyText2 != null)
		{
			this._body2.gameObject.SetActive(true);
			this._body2.layout.After(this._settings.delayBeforeGaelic, delegate
			{
				this._body2.Setup(this._bodyText2, NarrativePresenter.instance.settings.textReadSettings, null);
			});
			bottomItem = this._body2;
		}
		else if (this._body2 != null)
		{
			this._body2.gameObject.SetActive(false);
		}
		this.layout.Animate(0.5f, delegate
		{
			this.layout.height = bottomItem.layout.bottomY + this._settings.bottomMargin;
		});
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00055458 File Offset: 0x00053658
	public void StopIfNecessary()
	{
		if (!this._body.completedAnimIn)
		{
			this._body.SkipToShown();
		}
		if (this._body2 != null && this._body2.gameObject.activeInHierarchy && !this._body2.completedAnimIn)
		{
			this._body2.SkipToShown();
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x000554B8 File Offset: 0x000536B8
	public void ReturnToPool()
	{
		this._body.CompleteAndReset(false);
		if (this._body2 != null && this._body2.gameObject.activeInHierarchy)
		{
			this._body2.CompleteAndReset(false);
		}
		this.prototype.ReturnToPool();
		this._creationTime = 0f;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00055513 File Offset: 0x00053713
	private void Update()
	{
		if (!this.begun)
		{
			return;
		}
		if (this._image == null)
		{
			return;
		}
		this._image.rotation = this.imageRotateMax * Mathf.Sin(this.imageRotateSpeed * Time.unscaledTime);
	}

	// Token: 0x04000C45 RID: 3141
	[NonSerialized]
	public NotificationType type;

	// Token: 0x04000C46 RID: 3142
	[NonSerialized]
	public string inkName;

	// Token: 0x04000C47 RID: 3143
	public float imageRotateMax = 10f;

	// Token: 0x04000C48 RID: 3144
	public float imageRotateSpeed = 5f;

	// Token: 0x04000C49 RID: 3145
	private SLayout _layout;

	// Token: 0x04000C4A RID: 3146
	private Prototype _prototype;

	// Token: 0x04000C4B RID: 3147
	private string _bodyText;

	// Token: 0x04000C4C RID: 3148
	private string _bodyText2;

	// Token: 0x04000C4D RID: 3149
	private float _creationTime;

	// Token: 0x04000C4E RID: 3150
	private float _defaultHeight;

	// Token: 0x04000C4F RID: 3151
	[SerializeField]
	private SLayout _title;

	// Token: 0x04000C50 RID: 3152
	[SerializeField]
	private SLayout _image;

	// Token: 0x04000C51 RID: 3153
	[SerializeField]
	private AnimatedTextView _body;

	// Token: 0x04000C52 RID: 3154
	[SerializeField]
	private AnimatedTextView _body2;

	// Token: 0x04000C53 RID: 3155
	[SerializeField]
	private NotificationsSettings _settings;
}
