using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class OpeningQuote : MonoSingleton<OpeningQuote>
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00055A91 File Offset: 0x00053C91
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

	// Token: 0x06000A39 RID: 2617 RVA: 0x00055AB3 File Offset: 0x00053CB3
	public void Show()
	{
		base.gameObject.SetActive(true);
		this._showCoroutine = this.Show_Coroutine();
		base.StartCoroutine(this._showCoroutine);
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x00055ADA File Offset: 0x00053CDA
	private IEnumerator Show_Coroutine()
	{
		this.layout.groupAlpha = 1f;
		this._attribution.alpha = 0f;
		this._titleAnimText.Setup("The Pilgrims' Aiding", this._titleTiming, null);
		this._titleAnimText.layout.centerX = 0.5f * this._titleAnimText.layout.parentRect.width;
		yield return new WaitForSeconds(this.titleToBodyPause);
		this._mainAnimText.Setup("Spirit be with thee on every stream,\nHeadland and ride and lawn;\n\nEach sea and land, each moor and meadow,\nEach lying down, each rising up,\n\nIn the trough of the waves, on the crest of the billows,\nEach step of the journey thou goest.\n", this._mainTiming, null);
		this._mainAnimText.layout.centerX = 0.5f * this._mainAnimText.layout.parentRect.width;
		while (!this._mainAnimText.completedAnimIn)
		{
			yield return null;
		}
		yield return new WaitForSeconds(this.bodyToAttributionPause);
		this._attribution.Animate(2f, delegate
		{
			this._attribution.alpha = 1f;
		});
		yield break;
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00055AEC File Offset: 0x00053CEC
	public void Hide()
	{
		base.StopCoroutine(this._showCoroutine);
		this._showCoroutine = null;
		this.layout.Animate(2f, delegate
		{
			this.layout.groupAlpha = 0f;
		}).Then(delegate
		{
			this._titleAnimText.CompleteAndReset(false);
			this._mainAnimText.CompleteAndReset(false);
		});
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00055B3A File Offset: 0x00053D3A
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000C65 RID: 3173
	public float titleToBodyPause = 3f;

	// Token: 0x04000C66 RID: 3174
	public float bodyToAttributionPause = 1f;

	// Token: 0x04000C67 RID: 3175
	private SLayout _layout;

	// Token: 0x04000C68 RID: 3176
	private IEnumerator _showCoroutine;

	// Token: 0x04000C69 RID: 3177
	[SerializeField]
	private AnimatedTextView _titleAnimText;

	// Token: 0x04000C6A RID: 3178
	[SerializeField]
	private AnimatedTextView _mainAnimText;

	// Token: 0x04000C6B RID: 3179
	[SerializeField]
	private SLayout _attribution;

	// Token: 0x04000C6C RID: 3180
	[SerializeField]
	private TextReadSettings _titleTiming;

	// Token: 0x04000C6D RID: 3181
	[SerializeField]
	private TextReadSettings _mainTiming;
}
