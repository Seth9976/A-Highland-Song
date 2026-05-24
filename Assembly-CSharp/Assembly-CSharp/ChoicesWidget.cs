using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class ChoicesWidget : MonoBehaviour
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0004964F File Offset: 0x0004784F
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

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00049671 File Offset: 0x00047871
	public ChoicesWidgetSettings settings
	{
		get
		{
			return this._settings;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x00049679 File Offset: 0x00047879
	// (set) Token: 0x060008A6 RID: 2214 RVA: 0x00049681 File Offset: 0x00047881
	public bool visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			if (value != this.visible)
			{
				this._visible = value;
				this.layout.Animate(value ? 0.2f : 0.5f, delegate
				{
					this.PerformLayout(true);
				});
			}
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x060008A7 RID: 2215 RVA: 0x000496BA File Offset: 0x000478BA
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

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x060008A8 RID: 2216 RVA: 0x000496DC File Offset: 0x000478DC
	public int choiceCount
	{
		get
		{
			return this._choiceItems.Count;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x060008A9 RID: 2217 RVA: 0x000496E9 File Offset: 0x000478E9
	public ChoicesWidgetChoice highlightedView
	{
		get
		{
			if (this._highlightedIdx >= this._choiceItems.Count)
			{
				return null;
			}
			return this._choiceItems[this._highlightedIdx];
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x060008AA RID: 2218 RVA: 0x00049711 File Offset: 0x00047911
	public GameChoice highlightedChoice
	{
		get
		{
			return this.highlightedView.choice;
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00049720 File Offset: 0x00047920
	private bool HasChoices(List<GameChoice> choices, int choiceCount)
	{
		if (choices == null)
		{
			return this._choiceItems.Count == 0;
		}
		if (this._choiceItems.Count != choiceCount)
		{
			return false;
		}
		for (int i = 0; i < choiceCount; i++)
		{
			GameChoice gameChoice = choices[i];
			GameChoice choice = this._choiceItems[i].choice;
			if (gameChoice.inkChoiceIdx != choice.inkChoiceIdx)
			{
				return false;
			}
			if (gameChoice.text != choice.text)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0004979C File Offset: 0x0004799C
	public void SetChoices(List<GameChoice> choices, int maxToDisplay = 2147483647, bool highlightForTutorial = false, bool canRunOff = false)
	{
		int num = Mathf.Min((choices == null) ? 0 : choices.Count, maxToDisplay);
		if (this.HasChoices(choices, num) && canRunOff == this._canRunOff)
		{
			return;
		}
		foreach (ChoicesWidgetChoice choicesWidgetChoice in this._choiceItems)
		{
			choicesWidgetChoice.GetComponent<Prototype>().ReturnToPool();
		}
		this._choiceItems.Clear();
		if (choices == null || choices.Count == 0)
		{
			return;
		}
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			GameChoice gameChoice = choices[i];
			ChoicesWidgetChoice choicesWidgetChoice2 = this._choiceProto.Instantiate<ChoicesWidgetChoice>(null);
			choicesWidgetChoice2.Setup(gameChoice, this.settings);
			num2 = Mathf.Max(num2, choicesWidgetChoice2.preferredWidth);
			this._choiceItems.Add(choicesWidgetChoice2);
		}
		foreach (ChoicesWidgetChoice choicesWidgetChoice3 in this._choiceItems)
		{
			choicesWidgetChoice3.targetWidth = num2;
		}
		this._canRunOff = canRunOff;
		this._runOffTutorialVisible = false;
		this._highlightedIdx = 0;
		this._hasTutorialHighlight = highlightForTutorial;
		if (this._tutorialGlow != null)
		{
			this._tutorialGlow.alpha = 0f;
		}
		SLayout.WithoutAnimating(delegate
		{
			this.visible = false;
			this.PerformLayout(true);
		});
		this.visible = true;
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00049918 File Offset: 0x00047B18
	public void TryCycleHighlight(int direction)
	{
		if (direction < 0 && !Narrative.instance.runOffTutorialIsSticky)
		{
			this.HideRunOffTutorial();
		}
		if (direction < 0 && this._highlightedIdx == 0)
		{
			return;
		}
		if (direction > 0 && this._highlightedIdx == this._choiceItems.Count - 1)
		{
			if (this._canRunOff)
			{
				this.ShowRunOffTutorial(0f);
			}
			return;
		}
		this.layout.Animate(0.1f, delegate
		{
			this._highlightedIdx = Mathf.Clamp(this._highlightedIdx + direction, 0, this._choiceItems.Count - 1);
			this.PerformLayout(false);
		});
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x000499B8 File Offset: 0x00047BB8
	public bool TryGetRestChoice(out GameChoice restChoice)
	{
		foreach (ChoicesWidgetChoice choicesWidgetChoice in this._choiceItems)
		{
			if (choicesWidgetChoice.choice.specialType == GameChoiceSpecialType.Rest)
			{
				restChoice = choicesWidgetChoice.choice;
				return true;
			}
		}
		restChoice = default(GameChoice);
		return false;
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x00049A2C File Offset: 0x00047C2C
	public bool hasMapChoice
	{
		get
		{
			using (List<ChoicesWidgetChoice>.Enumerator enumerator = this._choiceItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.choice.isMapChoice)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00049A8C File Offset: 0x00047C8C
	public bool hasRestChoice
	{
		get
		{
			GameChoice gameChoice;
			return this.TryGetRestChoice(out gameChoice);
		}
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00049AA4 File Offset: 0x00047CA4
	public void Clear()
	{
		foreach (ChoicesWidgetChoice choicesWidgetChoice in this._choiceItems)
		{
			choicesWidgetChoice.GetComponent<Prototype>().ReturnToPool();
		}
		this._choiceItems.Clear();
		this.layout.CancelAnimations();
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00049B10 File Offset: 0x00047D10
	private void Update()
	{
		if (this._tutorialGlow != null && this._hasTutorialHighlight)
		{
			float num = 0.5f * this._settings.tutorialHighlightPulseAmount;
			this._tutorialGlow.alpha = 1f - num + num * Mathf.Sin(this._settings.tutorialHighlightPulseSpeed * Time.time);
		}
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00049B70 File Offset: 0x00047D70
	public void ShowRunOffTutorial(float delay = 0f)
	{
		if (this._runOffIcon != null && this._runOffIcon.groupAlpha > 0f)
		{
			this.layout.Animate(0.4f, delay, SLayout.thereAndBack, delegate
			{
				this._runOffIcon.scale = 1.4f;
				this._runOffIcon.groupAlpha = 1f;
			});
		}
		if (this._runOffTutorial != null && !this._runOffTutorialVisible)
		{
			this._runOffTutorialVisible = true;
			this.layout.Animate(0.5f, 0.15f + delay, delegate
			{
				this._runOffTutorial.groupAlpha = 1f;
			});
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00049C01 File Offset: 0x00047E01
	public void HideRunOffTutorial()
	{
		if (!this._runOffTutorialVisible || this._runOffIcon == null)
		{
			return;
		}
		this._runOffTutorialVisible = false;
		this.layout.Animate(0.5f, delegate
		{
			this._runOffTutorial.groupAlpha = 0f;
		});
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00049C3E File Offset: 0x00047E3E
	[ContextMenu("Refresh")]
	private void ForceLayoutForContextMenu()
	{
		this.PerformLayout(false);
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00049C48 File Offset: 0x00047E48
	private void PerformLayout(bool allowDelayPerItem = true)
	{
		if (this._choiceItems.Count == 0)
		{
			return;
		}
		float targetWidth = this._choiceItems[0].targetWidth;
		this.layout.width = targetWidth;
		float num = this.settings.firstChoiceY;
		for (int i = 0; i < this._choiceItems.Count; i++)
		{
			ChoicesWidgetChoice choicesWidgetChoice = this._choiceItems[i];
			choicesWidgetChoice.visible = this._visible;
			choicesWidgetChoice.highlighted = i == this._highlightedIdx;
			if (allowDelayPerItem)
			{
				this.layout.AddDelay(this._settings.delayPerItem);
			}
			choicesWidgetChoice.layout.y = num;
			num += this._settings.heightPerItemExpanded;
		}
		if (this._canRunOff)
		{
			if (this._runOffIcon != null)
			{
				num += this._settings.marginToRunOff;
				this._runOffIcon.y = num;
				if (this._visible)
				{
					this._runOffIcon.groupAlpha = this._settings.runOffUnhighlightedAlpha;
				}
				else
				{
					this._runOffIcon.groupAlpha = 0f;
				}
				num += this._settings.heightPerItemExpanded;
				this._runOffTutorial.originY = this._runOffIcon.centerY;
				string text = "... or walk away.";
				if (Narrative.instance.hasCustomRunOffTutorialText)
				{
					text = Narrative.instance.customRunOffTutorialText;
				}
				this._runOffTutorialText.textMeshPro.text = text;
				float x = this._runOffTutorialText.textMeshPro.GetPreferredValues(text).x;
				this._runOffTutorialText.width = x;
				this._runOffTutorialText.textMeshPro.ForceMeshUpdate(false, false);
				float num2 = 0.5f * this._runOffTutorialText.parentRect.width;
				this._runOffTutorialText.centerX = num2;
				this._runOffTutorialBg.width = x + 80f;
				this._runOffTutorialBg.centerX = num2;
				int num3 = (this._runOffTutorialVisible ? 1 : 0);
				if (this._runOffTutorial.targetGroupAlpha != (float)num3)
				{
					this._runOffTutorial.groupAlpha = (float)(this._runOffTutorialVisible ? 1 : 0);
				}
				num += this._settings.runOffTutorialHeight;
			}
			else
			{
				string text2 = string.Join(", ", this._choiceItems.Select((ChoicesWidgetChoice item) => item.choice.text));
				Debug.LogError("Wanted 'canRunOff' but this choices widget (" + base.name + ") has no runOffIcon. Choices: " + text2, this);
			}
		}
		else if (this._runOffIcon != null)
		{
			this._runOffIcon.groupAlpha = 0f;
			this._runOffTutorial.groupAlpha = 0f;
		}
		this.layout.height = num + 30f;
		if (this._arrow != null)
		{
			this._arrow.centerX = this.layout.width * 0.5f;
			this._arrow.topY = this.settings.arrowOffsetY;
			this._arrow.rotation = 0f;
			this._arrow.alpha = (float)(this._visible ? 1 : 0);
		}
		if (this._tutorialGlow != null)
		{
			this._tutorialGlow.groupAlpha = (float)(this._hasTutorialHighlight ? 1 : 0);
		}
	}

	// Token: 0x04000A5F RID: 2655
	private SLayout _layout;

	// Token: 0x04000A60 RID: 2656
	[SerializeField]
	[Disable]
	private bool _visible;

	// Token: 0x04000A61 RID: 2657
	private Prototype _prototype;

	// Token: 0x04000A62 RID: 2658
	[SerializeField]
	private Prototype _choiceProto;

	// Token: 0x04000A63 RID: 2659
	[SerializeField]
	private ChoicesWidgetSettings _settings;

	// Token: 0x04000A64 RID: 2660
	[SerializeField]
	private SLayout _arrow;

	// Token: 0x04000A65 RID: 2661
	[SerializeField]
	[Disable]
	private List<ChoicesWidgetChoice> _choiceItems = new List<ChoicesWidgetChoice>();

	// Token: 0x04000A66 RID: 2662
	[SerializeField]
	private SLayout _tutorialGlow;

	// Token: 0x04000A67 RID: 2663
	[SerializeField]
	private SLayout _runOffIcon;

	// Token: 0x04000A68 RID: 2664
	[SerializeField]
	private SLayout _runOffTutorial;

	// Token: 0x04000A69 RID: 2665
	[SerializeField]
	private SLayout _runOffTutorialText;

	// Token: 0x04000A6A RID: 2666
	[SerializeField]
	private SLayout _runOffTutorialBg;

	// Token: 0x04000A6B RID: 2667
	private int _highlightedIdx;

	// Token: 0x04000A6C RID: 2668
	private bool _hasTutorialHighlight;

	// Token: 0x04000A6D RID: 2669
	private bool _canRunOff;

	// Token: 0x04000A6E RID: 2670
	private bool _runOffTutorialVisible;
}
