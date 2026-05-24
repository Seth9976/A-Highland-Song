using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class AnimatedTextView : MonoBehaviour
{
	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600054C RID: 1356 RVA: 0x0002A57F File Offset: 0x0002877F
	public bool minimumReadTimeElapsed
	{
		get
		{
			return Time.unscaledTime > this._minimumReadingTimeEnd;
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x0600054D RID: 1357 RVA: 0x0002A58E File Offset: 0x0002878E
	public bool canPlayerSkipToShown
	{
		get
		{
			return Time.unscaledTime > this._skippableToShownTimeAllowed && !this._returningToPool && !this.canPlayerSkip && this.layout.isAnimating;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x0600054E RID: 1358 RVA: 0x0002A5BA File Offset: 0x000287BA
	public bool canPlayerSkip
	{
		get
		{
			return Time.unscaledTime > this._skippableTimeEnd && !this._returningToPool;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x0600054F RID: 1359 RVA: 0x0002A5D4 File Offset: 0x000287D4
	public bool returningToPool
	{
		get
		{
			return this._returningToPool;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000550 RID: 1360 RVA: 0x0002A5DC File Offset: 0x000287DC
	public int wordCount
	{
		get
		{
			return this._wordLayouts.Count;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000551 RID: 1361 RVA: 0x0002A5E9 File Offset: 0x000287E9
	// (set) Token: 0x06000552 RID: 1362 RVA: 0x0002A5F1 File Offset: 0x000287F1
	public bool completedAnimIn { get; private set; }

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000553 RID: 1363 RVA: 0x0002A5FA File Offset: 0x000287FA
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

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000554 RID: 1364 RVA: 0x0002A61C File Offset: 0x0002881C
	private Prototype prototype
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

	// Token: 0x06000555 RID: 1365 RVA: 0x0002A63E File Offset: 0x0002883E
	private void OnEnable()
	{
		this._maxWordAlpha = this._wordProto.GetComponent<SLayout>().alpha;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0002A658 File Offset: 0x00028858
	public void Setup(string text, TextReadSettings textReadTiming, Action onSetupComplete = null)
	{
		text = text.TrimEnd();
		string[] array = text.Split('\n', StringSplitOptions.None);
		List<LineBreaker.Line> list = null;
		if (array.Length > 1)
		{
			list = new List<LineBreaker.Line>(32);
		}
		int num = 0;
		foreach (string text2 in array)
		{
			LineBreaker.Line line = new LineBreaker.Line
			{
				firstWordIdx = num,
				lastWordIdx = num,
				width = 0f
			};
			foreach (string text3 in text2.Split(' ', StringSplitOptions.None))
			{
				SLayout slayout = this._wordProto.Instantiate<SLayout>(null);
				slayout.textMeshPro.text = text3;
				float x = slayout.textMeshPro.GetPreferredValues(text3).x;
				slayout.width = x;
				slayout.textMeshPro.ForceMeshUpdate(false, false);
				slayout.alpha = 0f;
				this._wordLayouts.Add(slayout);
				this._wordWidths.Add(x);
				line.lastWordIdx = num;
				if (line.width > 0f)
				{
					line.width += this._settings.spaceWidth;
				}
				line.width += x;
				num++;
			}
			if (list != null)
			{
				list.Add(line);
			}
		}
		this._wordPositions.Clear();
		if (list != null)
		{
			this.layout.size = LineBreaker.PositionWords(list, this._wordWidths, this._settings.spaceWidth, this._settings.lineHeight, this._settings.maxWidth, this._wordPositions, this.align);
		}
		else
		{
			this.layout.size = LineBreaker.Layout(this._wordWidths, this._settings.maxWidth, this._settings.lineHeight, this._settings.spaceWidth, this._wordPositions, this.align);
		}
		for (int k = 0; k < this._wordPositions.Count; k++)
		{
			this._wordLayouts[k].x = this._wordPositions[k].x;
			this._wordLayouts[k].y = this._wordPositions[k].y;
		}
		StringBuilder textWithoutTags = new StringBuilder();
		SLayoutAnimation slayoutAnimation = this.layout.Animate(this._settings.fadeTimePerWord, delegate
		{
			foreach (SLayout slayout2 in this._wordLayouts)
			{
				slayout2.alpha = this._maxWordAlpha;
				this.layout.AddDelay(this._settings.timeBetweenWords);
				string parsedText = slayout2.textMeshPro.GetParsedText();
				textWithoutTags.Append(parsedText).Append(" ");
				if (parsedText.Contains(",") || parsedText.Contains(";") || parsedText.Contains(":"))
				{
					this.layout.AddDelay(this._settings.punctuationPause);
				}
				else if (parsedText.Contains(".") || parsedText.Contains("!") || parsedText.Contains("?"))
				{
					this.layout.AddDelay(this._settings.terminatorPause);
				}
			}
		});
		float num2 = this._settings.fadeTimePerWord + (float)Mathf.Max(this._wordLayouts.Count - 1, 0) * this._settings.timeBetweenWords;
		if (this._background != null)
		{
			this._background.alpha = 0f;
			this.layout.Animate(0.5f * num2, 0.2f * num2, delegate
			{
				this._background.alpha = this._settings.backgroundOpacity;
			});
		}
		this.completedAnimIn = false;
		slayoutAnimation.Then(delegate
		{
			this.completedAnimIn = true;
			if (onSetupComplete != null)
			{
				onSetupComplete();
			}
		});
		float num3 = NarrativePresenter.instance.readingSpeedScalar * Game.instance.debugFastTimeScalar;
		float num4 = 0.6f * num2 + StoryUtils.GetEstimatedTimeToRead(textWithoutTags.ToString(), textReadTiming);
		num4 /= num3;
		this._minimumReadingTimeEnd = Time.unscaledTime + num4;
		float num5 = Mathf.Max(this._settings.skipTimeMinSeconds, this._settings.skipTimeMinProportionOfFadeIn * num2);
		num5 /= num3;
		this._skippableTimeEnd = Time.unscaledTime + num5;
		this._skippableToShownTimeAllowed = Time.unscaledTime + 0.3f;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0002AA00 File Offset: 0x00028C00
	public void SkipToShown()
	{
		this.layout.CompleteAnimations();
		this._skippableTimeEnd = 0f;
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0002AA18 File Offset: 0x00028C18
	public void CompleteAndReset(bool animated)
	{
		if (this.prototype != null)
		{
			this._returningToPool = true;
		}
		this.layout.CancelAnimations();
		this.layout.Animate(animated ? this._settings.fadeOutTime : 0f, delegate
		{
			foreach (SLayout slayout in this._wordLayouts)
			{
				slayout.alpha = 0f;
			}
			if (this._background != null)
			{
				this._background.alpha = 0f;
			}
		}).Then(delegate
		{
			foreach (SLayout slayout2 in this._wordLayouts)
			{
				slayout2.GetComponent<Prototype>().ReturnToPool();
			}
			this._wordLayouts.Clear();
			this._wordWidths.Clear();
			this._wordPositions.Clear();
			if (this.prototype != null)
			{
				this.prototype.ReturnToPool();
				this._returningToPool = false;
			}
		});
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0002AA83 File Offset: 0x00028C83
	public void ReturnToPool(bool animated)
	{
		if (this._returningToPool)
		{
			Debug.LogError("AnimatedTextView " + base.name + " was already returning to pool when ReturnToPool was called", this);
			return;
		}
		this.CompleteAndReset(animated);
	}

	// Token: 0x0400060D RID: 1549
	public TextAlignment align = TextAlignment.Center;

	// Token: 0x0400060E RID: 1550
	private SLayout _layout;

	// Token: 0x0400060F RID: 1551
	private Prototype _prototype;

	// Token: 0x04000610 RID: 1552
	private List<SLayout> _wordLayouts = new List<SLayout>();

	// Token: 0x04000611 RID: 1553
	private List<float> _wordWidths = new List<float>();

	// Token: 0x04000612 RID: 1554
	private List<Vector2> _wordPositions = new List<Vector2>();

	// Token: 0x04000613 RID: 1555
	private float _minimumReadingTimeEnd;

	// Token: 0x04000614 RID: 1556
	private float _skippableTimeEnd;

	// Token: 0x04000615 RID: 1557
	private float _skippableToShownTimeAllowed;

	// Token: 0x04000616 RID: 1558
	private bool _returningToPool;

	// Token: 0x04000617 RID: 1559
	private float _maxWordAlpha = 1f;

	// Token: 0x04000618 RID: 1560
	[SerializeField]
	private AnimatedTextViewSettings _settings;

	// Token: 0x04000619 RID: 1561
	[SerializeField]
	private Prototype _wordProto;

	// Token: 0x0400061A RID: 1562
	[SerializeField]
	private SLayout _background;
}
