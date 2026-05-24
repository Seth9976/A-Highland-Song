using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200008F RID: 143
[RequireComponent(typeof(AudioSource))]
public class NarrativePresenter : MonoBehaviour
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x060004BE RID: 1214 RVA: 0x00026CDD File Offset: 0x00024EDD
	public static NarrativePresenter instance
	{
		get
		{
			return GSR.NarrativePresenter;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x00026CE4 File Offset: 0x00024EE4
	public static bool presenting
	{
		get
		{
			return NarrativePresenter.instance != null && NarrativePresenter.instance._presentingCoroutine != null;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00026D02 File Offset: 0x00024F02
	public static bool showingPropChoices
	{
		get
		{
			return NarrativePresenter.instance != null && NarrativePresenter.instance.activePropChoiceWidget != null;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00026D23 File Offset: 0x00024F23
	public static bool hasRestChoice
	{
		get
		{
			ChoicesWidget activeChoicesWidget = NarrativePresenter.activeChoicesWidget;
			return activeChoicesWidget != null && activeChoicesWidget.hasRestChoice;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00026D35 File Offset: 0x00024F35
	public static bool showingSubtitles
	{
		get
		{
			return NarrativePresenter.instance._subtitle != null;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00026D47 File Offset: 0x00024F47
	public bool hasExitTriggerZoneChoices
	{
		get
		{
			return this._exitTriggerZoneChoices.Count > 0;
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00026D58 File Offset: 0x00024F58
	private PropWidget activePropChoiceWidget
	{
		get
		{
			if (this._activeProp == null)
			{
				return null;
			}
			PropWidget propWidget;
			if (!this._propWidgetsByProp.TryGetValue(this._activeProp, out propWidget))
			{
				Debug.LogError("Couldn't find _activeProp (" + this._activeProp.name + ") in _propWidgetsByProp. This should never happen!", this._activeProp);
				return null;
			}
			return propWidget;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00026DB2 File Offset: 0x00024FB2
	private static ChoicesWidget activeChoicesWidget
	{
		get
		{
			if (NarrativePresenter.instance == null)
			{
				return null;
			}
			PropWidget activePropChoiceWidget = NarrativePresenter.instance.activePropChoiceWidget;
			ChoicesWidget choicesWidget;
			if ((choicesWidget = ((activePropChoiceWidget != null) ? activePropChoiceWidget.choicesWidget : null)) == null)
			{
				NarrativeChoicesWidget narrativeChoicesWidget = NarrativePresenter.instance._narrativeChoicesWidget;
				if (narrativeChoicesWidget == null)
				{
					return null;
				}
				choicesWidget = narrativeChoicesWidget.choicesWidget;
			}
			return choicesWidget;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00026DF2 File Offset: 0x00024FF2
	public bool playingAudio
	{
		get
		{
			return this._playingAudio;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00026DFA File Offset: 0x00024FFA
	public bool showingNarrativeChoiceWidget
	{
		get
		{
			return this._narrativeChoicesWidget != null;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00026E08 File Offset: 0x00025008
	public bool showingHideableChoiceWidget
	{
		get
		{
			return this.activePropChoiceWidget != null;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00026E16 File Offset: 0x00025016
	public bool hasAnyChoices
	{
		get
		{
			return this.showingNarrativeChoiceWidget || NarrativePresenter.showingPropChoices;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060004CA RID: 1226 RVA: 0x00026E28 File Offset: 0x00025028
	public static int activeChoiceCount
	{
		get
		{
			ChoicesWidget activeChoicesWidget = NarrativePresenter.activeChoicesWidget;
			if (!(activeChoicesWidget == null))
			{
				return activeChoicesWidget.choiceCount;
			}
			return 0;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060004CB RID: 1227 RVA: 0x00026E4C File Offset: 0x0002504C
	public Prop mapMarkerPropWithVisibleChoices
	{
		get
		{
			if (NarrativePresenter.instance == null || NarrativePresenter.instance.activePropChoiceWidget == null)
			{
				return null;
			}
			if (!NarrativePresenter.instance.activePropChoiceWidget.choicesWidget.hasMapChoice)
			{
				return null;
			}
			return this._activeProp;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x00026E98 File Offset: 0x00025098
	public NarrativePresenterSettings settings
	{
		get
		{
			return this._settings;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060004CD RID: 1229 RVA: 0x00026EA0 File Offset: 0x000250A0
	private float subtitleTargetY
	{
		get
		{
			if (Blackout.isFullyVisible)
			{
				return this.settings.subtitlesYBlackout;
			}
			return this.settings.subtitlesDefaultY;
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00026EC0 File Offset: 0x000250C0
	public void SetPaused(bool paused)
	{
		this._paused = paused;
		if (this._paused)
		{
			this._sourcePaused = false;
			if (this.source.isPlaying)
			{
				this.source.Pause();
				this._sourcePaused = true;
			}
		}
		else if (this._sourcePaused)
		{
			this.source.Play();
			this._sourcePaused = false;
		}
		this.RefreshTimeScaleMultiplier();
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00026F24 File Offset: 0x00025124
	public static IEnumerator Narrate(string text, string audioClipName)
	{
		NarrativePresenter.instance.AddItem(new NarrativePresenter.Item
		{
			text = text,
			audioClipName = audioClipName
		});
		while (NarrativePresenter.presenting)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00026F3A File Offset: 0x0002513A
	public static IEnumerator DialogueBubble(string sourceCharName, string text, string audioClipName)
	{
		NarrativePresenter.instance.AddItem(new NarrativePresenter.Item
		{
			sourceCharacterName = sourceCharName,
			audioClipName = audioClipName,
			text = text
		});
		while (NarrativePresenter.presenting)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00026F57 File Offset: 0x00025157
	public static IEnumerator Choices(List<GameChoice> choices)
	{
		NarrativePresenter.instance.AddItem(new NarrativePresenter.Item
		{
			choices = choices
		});
		while (NarrativePresenter.presenting)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00026F66 File Offset: 0x00025166
	private void AddItem(NarrativePresenter.Item item)
	{
		this._queue.Enqueue(item);
		if (this._presentingCoroutine == null)
		{
			this._presentingCoroutine = this.PresentationCoroutine();
			if (this._presentingCoroutine != null)
			{
				base.StartCoroutine(this._presentingCoroutine);
			}
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00026F9D File Offset: 0x0002519D
	private IEnumerator PresentationCoroutine()
	{
		while (this._queue.Count > 0)
		{
			while (this._paused)
			{
				yield return null;
			}
			NarrativePresenter.Item item = this._queue.Dequeue();
			bool flag = item.text != null && item.sourceCharacterName == null && item.choices == null;
			bool isDialogueBubble = item.sourceCharacterName != null && item.text != null && item.choices == null;
			if (isDialogueBubble && item.sourceCharacterName == "MOIRA_VO")
			{
				isDialogueBubble = false;
				flag = true;
				item.sourceCharacterName = "MOIRA";
			}
			if (isDialogueBubble && item.sourceCharacterName == "VOICES_VO")
			{
				isDialogueBubble = false;
				item.sourceCharacterName = "VOICES";
			}
			else if (isDialogueBubble && (item.sourceCharacterName == "WOMAN_VO" || item.sourceCharacterName == "VOICE_VO"))
			{
				isDialogueBubble = false;
				item.sourceCharacterName = "VOICE";
			}
			else if (isDialogueBubble && item.sourceCharacterName == "HAMISH_VO")
			{
				isDialogueBubble = false;
				item.sourceCharacterName = "HAMISH";
			}
			else if (flag)
			{
				item.sourceCharacterName = "MOIRA";
			}
			bool isChoices = item.choices != null;
			if (item.audioClipName != null)
			{
				ResourceRequest req = Resources.LoadAsync(item.audioClipName);
				yield return req;
				while (this._paused)
				{
					yield return null;
				}
				AudioClip audioClip = req.asset as AudioClip;
				if (audioClip == null)
				{
					Debug.LogError("Speech clip " + item.audioClipName + " failed to load. Is it missing? Line: " + item.text);
				}
				else
				{
					this.source.clip = audioClip;
					this.source.volume = 1f;
					this.source.time = 0f;
					this.source.Play();
				}
				req = null;
			}
			if (isDialogueBubble)
			{
				if (item.sourceCharacterName == "MOIRA")
				{
					item.text = InkStylingUtility.ExhaustifyDialogueText(item.text, Runner.instance.stamina < 0.25f, Runner.instance.health.currentHealth < 0.5f, Runner.instance.health.GetWeatherHealthEffects(true).HasFlag(WeatherHealthEffect.Freezing));
					this.recentMoiraDialogue.Add(item.text);
					while (this.recentMoiraDialogue.Count > 10)
					{
						this.recentMoiraDialogue.RemoveAt(0);
					}
				}
				DialogueBubbleView dialogueBubbleView = this._dialogueBubbleProto.Instantiate<DialogueBubbleView>(null);
				StoryCharacter storyCharacter = StoryCharacter.WithName(item.sourceCharacterName);
				if (storyCharacter == null)
				{
					Debug.LogError(string.Concat(new string[] { "Couldn't find StoryCharacter with name ", item.sourceCharacterName, " for dialogue line '", item.text, "'" }));
				}
				else
				{
					dialogueBubbleView.Setup(storyCharacter, item.text);
				}
				this._activeBubbles.Add(dialogueBubbleView);
			}
			else if (isChoices)
			{
				List<GameChoice> choices = item.choices;
				this._exitTriggerZoneChoices.Clear();
				item.choices.RemoveAllOrderedAnd((GameChoice choice) => choice.type == GameChoiceType.ExitZone, delegate(GameChoice choice)
				{
					this._exitTriggerZoneChoices.Add(choice);
				});
				bool canRunOff = Narrative.instance.canRunOff;
				this._narrativeChoicesWidget = this._narrativeChoicesProto.Instantiate<NarrativeChoicesWidget>(null);
				this._narrativeChoicesWidget.Setup(Runner.instance.transform, choices, 3, canRunOff);
				if (canRunOff && Narrative.instance.runOffTutorialIsSticky)
				{
					this._narrativeChoicesWidget.choicesWidget.ShowRunOffTutorial(1.5f);
				}
				if (canRunOff)
				{
					Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.NarrativeChoicesWithNoExit;
				}
				else
				{
					Runner.instance.playerControlDisabled |= PlayerControlDisableReason.NarrativeChoicesWithNoExit;
				}
			}
			else
			{
				this._subtitle = this._subtitleProto.Instantiate<AnimatedTextView>(null);
				string text = item.text;
				if ((!(item.sourceCharacterName == "MOIRA") || Runner.instance.hidden) && !(item.sourceCharacterName == "VOICES") && !Blackout.showing)
				{
					text = item.sourceCharacterName + ": " + text;
				}
				this._subtitle.Setup(text, this.source.isPlaying ? this._settings.subtitleToAudioMinimumTiming : this._settings.subtitleWithoutAudioTiming, null);
				this._subtitle.layout.y = this.subtitleTargetY;
				this._subtitle.layout.centerX = 0.5f * this._subtitle.layout.parentRect.width;
				while ((this._subtitle != null && !this._subtitle.minimumReadTimeElapsed) || this._paused)
				{
					yield return null;
				}
			}
			if (!isChoices)
			{
				while (this._activeBubbles.Count > 0)
				{
					yield return null;
				}
				while (this._subtitle != null)
				{
					if (this._subtitle.minimumReadTimeElapsed)
					{
						break;
					}
					this._playingAudio = true;
				}
				while (this.source.isPlaying || this._paused)
				{
					yield return null;
				}
				this._playingAudio = false;
			}
			else
			{
				while (this._narrativeChoicesWidget != null || this._exitTriggerZoneChoices.Count > 0 || this._paused)
				{
					yield return null;
				}
			}
			if (this._subtitle != null)
			{
				AnimatedTextView subtitle = this._subtitle;
				this._subtitle = null;
				subtitle.ReturnToPool(true);
			}
			item = default(NarrativePresenter.Item);
		}
		this._presentingCoroutine = null;
		yield break;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00026FAC File Offset: 0x000251AC
	public void OnReturnBubbleToPool(DialogueBubbleView bubbleView)
	{
		this._activeBubbles.RemoveAll((DialogueBubbleView b) => b == bubbleView);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00026FE0 File Offset: 0x000251E0
	public void SetPropsState(List<Prop> attractRangeProps, List<Prop> triggerRangeProps, Prop activeProp, List<GameChoice> activePropChoices)
	{
		foreach (Prop prop in attractRangeProps)
		{
			this.SetPropWidget(prop, PropWidget.State.Attract, null);
		}
		foreach (Prop prop2 in triggerRangeProps)
		{
			if (prop2 != activeProp)
			{
				this.SetPropWidget(prop2, PropWidget.State.Attract, null);
			}
		}
		if (this._activeProp != null && this._activeProp != activeProp)
		{
			this._activeProp.highlight = false;
		}
		this._activeProp = activeProp;
		if (this._activeProp != null)
		{
			this.SetPropWidget(this._activeProp, (activePropChoices.Count == 0) ? PropWidget.State.Attract : PropWidget.State.Normal, activePropChoices);
			this._activeProp.highlight = true;
		}
		this._propWidgetsToRemoveScratch.Clear();
		foreach (KeyValuePair<Prop, PropWidget> keyValuePair in this._propWidgetsByProp)
		{
			Prop key = keyValuePair.Key;
			PropWidget value = keyValuePair.Value;
			if (!attractRangeProps.Contains(key) && !triggerRangeProps.Contains(key) && key != this._activeProp)
			{
				value.FadeOutAndRemove();
				this._propWidgetsToRemoveScratch.Add(key);
			}
		}
		foreach (Prop prop3 in this._propWidgetsToRemoveScratch)
		{
			this._propWidgetsByProp.Remove(prop3);
		}
		this._propWidgetsToRemoveScratch.Clear();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000271C4 File Offset: 0x000253C4
	public void Clear(bool forDeath = false, bool forInterrupt = false)
	{
		if (!forDeath && !forInterrupt)
		{
			this._pendingAnimatedPropDeactivations.Clear();
		}
		this.ClearChoiceAndPropWidgets();
		this._exitTriggerZoneChoices.Clear();
		Narrative.instance.activeAutoCutZoneName = null;
		if (this.source.isPlaying)
		{
			this.source.Stop();
		}
		this.SetPaused(false);
		foreach (DialogueBubbleView dialogueBubbleView in this._activeBubbles)
		{
			dialogueBubbleView.HideAndMarkForDestroy(0f);
		}
		this._activeBubbles.Clear();
		if (this._subtitle != null)
		{
			this._subtitle.ReturnToPool(forDeath);
		}
		this._subtitle = null;
		this._queue.Clear();
		GameClock.instance.CancelTimePassing();
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000272A8 File Offset: 0x000254A8
	public void ClearChoiceAndPropWidgets()
	{
		if (this._narrativeChoicesWidget != null)
		{
			this._narrativeChoicesWidget.visible = false;
			this._narrativeChoicesWidget = null;
		}
		foreach (PropWidget propWidget in this._propWidgetsByProp.Values)
		{
			propWidget.FadeOutAndRemove();
		}
		this._propWidgetsByProp.Clear();
		this._activeProp = null;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00027330 File Offset: 0x00025530
	public void DeactivatePropWhenAnimationComplete(Prop prop, InkAnimation anim)
	{
		this._pendingAnimatedPropDeactivations.Add(new ValueTuple<Prop, InkAnimation>(prop, anim));
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00027344 File Offset: 0x00025544
	private void SetPropWidget(Prop prop, PropWidget.State state, List<GameChoice> choices = null)
	{
		PropWidget propWidget;
		if (!this._propWidgetsByProp.TryGetValue(prop, out propWidget))
		{
			if (state != PropWidget.State.Hidden)
			{
				propWidget = this._propChoicesProto.Instantiate<PropWidget>(null);
				propWidget.Setup(prop, state, choices);
				this._propWidgetsByProp[prop] = propWidget;
			}
			return;
		}
		if (state == PropWidget.State.Hidden)
		{
			propWidget.FadeOutAndRemove();
			this._propWidgetsByProp.Remove(prop);
			return;
		}
		propWidget.choicesWidget.SetChoices(choices, 3, false, false);
		propWidget.state = state;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x000273B8 File Offset: 0x000255B8
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		float targetSubtitlesAlpha = (float)(((MonoSingleton<JournalController>.instance.visible && !MonoSingleton<JournalController>.instance.mapConfirmActive) || MonoSingleton<SettingsScreen>.instance.visible || PhotoMode.visible) ? 0 : 1);
		if (targetSubtitlesAlpha != this._subtitlesLayout.targetGroupAlpha)
		{
			this._subtitlesLayout.Animate(0.5f, delegate
			{
				this._subtitlesLayout.groupAlpha = targetSubtitlesAlpha;
			});
		}
		this.UpdateInputInteraction();
		this._pendingAnimatedPropDeactivations.UpdateAndRemoveIf(delegate([TupleElementNames(new string[] { "prop", "anim" })] ValueTuple<Prop, InkAnimation> propAndAnim)
		{
			if (propAndAnim.Item1 == null || propAndAnim.Item2 == null)
			{
				return true;
			}
			if (!propAndAnim.Item2.isPlaying)
			{
				propAndAnim.Item1.gameObject.SetActive(false);
				return true;
			}
			return false;
		});
		if (this._subtitle != null)
		{
			float targetY = this.subtitleTargetY;
			if (this._subtitle.layout.targetY != targetY)
			{
				this._subtitle.layout.Animate(0.2f, delegate
				{
					this._subtitle.layout.y = targetY;
				});
			}
		}
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x000274D4 File Offset: 0x000256D4
	private void UpdateInputInteraction()
	{
		if (Runner.instance.dead || Game.gameplayPaused)
		{
			return;
		}
		foreach (GameChoice gameChoice in this._exitTriggerZoneChoices)
		{
			foreach (string text in gameChoice.interactableNames)
			{
				using (List<Prop>.Enumerator enumerator3 = Prop.GetLoadedPropsByInkName(text).GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (!enumerator3.Current.triggerZone.triggering)
						{
							this._exitTriggerZoneChoices.Clear();
							if (this._narrativeChoicesWidget != null)
							{
								this._narrativeChoicesWidget.visible = false;
								this._narrativeChoicesWidget = null;
							}
							NarrativePresenter.onChooseChoice(gameChoice, text);
							return;
						}
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(Narrative.instance.activeAutoCutZoneName) && !Runner.instance.inFinalJumpAndCommittedToJumping)
		{
			bool flag = true;
			using (List<Prop>.Enumerator enumerator3 = Prop.GetLoadedPropsByInkName(Narrative.instance.activeAutoCutZoneName).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.triggerZone.triggering)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				if (this._narrativeChoicesWidget != null)
				{
					this._narrativeChoicesWidget.visible = false;
					this._narrativeChoicesWidget = null;
				}
				Narrative.instance.LeftAutoCutZone();
			}
		}
		PropWidget activePropChoiceWidget = this.activePropChoiceWidget;
		ChoicesWidget choicesWidget;
		if ((choicesWidget = ((activePropChoiceWidget != null) ? activePropChoiceWidget.choicesWidget : null)) == null)
		{
			NarrativeChoicesWidget narrativeChoicesWidget = this._narrativeChoicesWidget;
			choicesWidget = ((narrativeChoicesWidget != null) ? narrativeChoicesWidget.choicesWidget : null);
		}
		ChoicesWidget choicesWidget2 = choicesWidget;
		if (choicesWidget2 != null)
		{
			bool flag2 = Runner.instance.running || Runner.instance.jumping || Runner.instance.falling || Runner.instance.hasInkPose || Runner.instance.stoneSkimming || Runner.instance.sitting || NarrativePresenter.presenting;
			if (MonoSingleton<RestStateController>.instance.sitting && activePropChoiceWidget != null)
			{
				flag2 = false;
			}
			if (GameCamera.instance.playerZoomState.zooming && activePropChoiceWidget != null)
			{
				flag2 = false;
			}
			if (MonoSingleton<MapsViewController>.instance.isBusy)
			{
				flag2 = false;
			}
			if (Runner.instance.inFinalJumpAndLeftLand)
			{
				flag2 = false;
			}
			if (this._narrativeChoicesWidget != null)
			{
				this._narrativeChoicesWidget.visible = flag2;
			}
			choicesWidget2.visible = flag2;
			if (!flag2)
			{
				return;
			}
			if (choicesWidget2 != null && choicesWidget2.visible)
			{
				if (GameInput.selectUp)
				{
					choicesWidget2.TryCycleHighlight(-1);
				}
				if (GameInput.selectDown)
				{
					choicesWidget2.TryCycleHighlight(1);
				}
			}
			GameChoice gameChoice2;
			bool flag3 = choicesWidget2.TryGetRestChoice(out gameChoice2);
			bool flag4 = GameInput.restPressed && flag3;
			if (choicesWidget2.visible && (GameInput.selectChoice || flag4))
			{
				GameInput.ClearInputState();
				if (activePropChoiceWidget != null)
				{
					Runner.instance.TurnToFace(activePropChoiceWidget.prop);
				}
				GameChoice gameChoice3 = (flag4 ? gameChoice2 : choicesWidget2.highlightedChoice);
				string text2 = null;
				if (activePropChoiceWidget != null)
				{
					text2 = activePropChoiceWidget.prop.inkListItemName;
					NarrativePresenter.onWillChooseChoiceOnProp(activePropChoiceWidget.prop);
					activePropChoiceWidget.choicesWidget.highlightedView.ChooseAnimation();
					if (gameChoice3.isMapChoice)
					{
						Map map = MonoSingleton<MapsViewController>.instance.StartMapConfirm(activePropChoiceWidget.prop);
						Narrative.instance.SetConfirmedMap(map.targetInkPropName);
					}
				}
				else
				{
					this._narrativeChoicesWidget.choicesWidget.highlightedView.ChooseAnimation();
					this._narrativeChoicesWidget.visible = false;
					this._narrativeChoicesWidget = null;
				}
				this._exitTriggerZoneChoices.Clear();
				if (gameChoice3.specialType == GameChoiceSpecialType.Rest && !MonoSingleton<RestStateController>.instance.active)
				{
					this.ClearChoiceAndPropWidgets();
					MonoSingleton<RestStateController>.instance.Enter();
				}
				NarrativePresenter.onChooseChoice(gameChoice3, text2);
			}
		}
		else
		{
			bool flag5 = MonoSingleton<RestStateController>.instance.active && GameInput.activeInputType == GameInput.InputType.KeyboardAndMouse;
			if (!NarrativePresenter.instance.showingNarrativeChoiceWidget && !NarrativePresenter.instance.playingAudio && !flag5 && GameInput.skipDialogue)
			{
				if (this._activeBubbles.Count > 0)
				{
					using (List<DialogueBubbleView>.Enumerator enumerator4 = this._activeBubbles.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							DialogueBubbleView dialogueBubbleView = enumerator4.Current;
							if (dialogueBubbleView.animTextView.canPlayerSkipToShown)
							{
								dialogueBubbleView.animTextView.SkipToShown();
							}
							else if (dialogueBubbleView.animTextView.canPlayerSkip)
							{
								MonoSingleton<GameInput>.instance.mapping.skipDialogue.ClearInputState();
								MonoSingleton<GameInput>.instance.mapping.back.ClearInputState();
								dialogueBubbleView.HideAndMarkForDestroy(0f);
								break;
							}
						}
						goto IL_054B;
					}
				}
				if (this._subtitle != null)
				{
					if (this._subtitle.canPlayerSkip)
					{
						GameInput.ClearInputState();
						if (this.source.isPlaying)
						{
							this.source.Stop();
						}
						AnimatedTextView subtitle = this._subtitle;
						this._subtitle = null;
						subtitle.ReturnToPool(true);
					}
				}
				else if (GameInput.debugFast && this.source.isPlaying)
				{
					this.source.Stop();
				}
			}
		}
		IL_054B:
		if (GameInput.hideChoices && GameInput.HasControl(null) && activePropChoiceWidget != null && !PropsController.instance.explicitlyHiddenProps.Contains(activePropChoiceWidget.prop))
		{
			GameInput.ClearInputState();
			PropsController.instance.explicitlyHiddenProps.Add(activePropChoiceWidget.prop);
			PropsController.instance.Refresh();
		}
		if (GameInput.selectChoice && !this.hasAnyChoices && PropsController.instance.explicitlyHiddenProps.Count != 0)
		{
			GameInput.ClearInputState();
			foreach (KeyValuePair<Prop, PropWidget> keyValuePair in this._propWidgetsByProp)
			{
				Prop key = keyValuePair.Key;
				if (keyValuePair.Value.state == PropWidget.State.Attract && PropsController.instance.explicitlyHiddenProps.Contains(key))
				{
					PropsController.instance.explicitlyHiddenProps.Clear();
					PropsController.instance.Refresh();
					break;
				}
			}
		}
		this.RefreshTimeScaleMultiplier();
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00027B74 File Offset: 0x00025D74
	private void RefreshTimeScaleMultiplier()
	{
		int num = (this._paused ? 0 : 1);
		this._timeScalar.timeScaleMultiplier = (float)num * this.animSpeedScalar * (GameInput.skipDialogueHeld ? this._settings.skipDialogueScalar : 1f) * Game.instance.debugFastTimeScalar;
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060004DD RID: 1245 RVA: 0x00027BC7 File Offset: 0x00025DC7
	public float readingSpeedScalar
	{
		get
		{
			return this.ApplyReadingSpeedPrefToScalarRange(this._settings.playerReadingSpeedScalarRange);
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x00027BDA File Offset: 0x00025DDA
	public float animSpeedScalar
	{
		get
		{
			return this.ApplyReadingSpeedPrefToScalarRange(this._settings.textAnimSpeedScalarRange);
		}
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00027BF0 File Offset: 0x00025DF0
	private float ApplyReadingSpeedPrefToScalarRange(Range minMax)
	{
		float @float = PlayerPrefsX.GetFloat("readingSpeed", 0.5f);
		float num;
		if (@float < 0.5f)
		{
			num = Mathf.Lerp(Mathf.Log(minMax.min, 2f), 0f, 2f * @float);
		}
		else
		{
			float num2 = Mathf.Log(minMax.max, 2f);
			num = Mathf.Lerp(0f, num2, 2f * (@float - 0.5f));
		}
		return Mathf.Pow(2f, num);
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00027C6E File Offset: 0x00025E6E
	private AudioSource source
	{
		get
		{
			if (this._source == null)
			{
				this._source = base.GetComponent<AudioSource>();
			}
			return this._source;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00027C90 File Offset: 0x00025E90
	public Prototype dialogueBubbleProto
	{
		get
		{
			return this._dialogueBubbleProto;
		}
	}

	// Token: 0x04000599 RID: 1433
	public static NarrativePresenter.ChooseChoiceDelegate onChooseChoice;

	// Token: 0x0400059A RID: 1434
	public static NarrativePresenter.WillChooseChoiceOnPropDelegate onWillChooseChoiceOnProp;

	// Token: 0x0400059B RID: 1435
	public List<string> recentMoiraDialogue = new List<string>();

	// Token: 0x0400059C RID: 1436
	private List<Prop> _propWidgetsToRemoveScratch = new List<Prop>();

	// Token: 0x0400059D RID: 1437
	public const string readingSpeedPrefName = "readingSpeed";

	// Token: 0x0400059E RID: 1438
	private AudioSource _source;

	// Token: 0x0400059F RID: 1439
	public Queue<NarrativePresenter.Item> _queue = new Queue<NarrativePresenter.Item>();

	// Token: 0x040005A0 RID: 1440
	private List<DialogueBubbleView> _activeBubbles = new List<DialogueBubbleView>();

	// Token: 0x040005A1 RID: 1441
	private bool _playingAudio;

	// Token: 0x040005A2 RID: 1442
	private IEnumerator _presentingCoroutine;

	// Token: 0x040005A3 RID: 1443
	private Prop _activeProp;

	// Token: 0x040005A4 RID: 1444
	private PropWidget _propChoicesWidget;

	// Token: 0x040005A5 RID: 1445
	private NarrativeChoicesWidget _narrativeChoicesWidget;

	// Token: 0x040005A6 RID: 1446
	private AnimatedTextView _subtitle;

	// Token: 0x040005A7 RID: 1447
	private Dictionary<Prop, PropWidget> _propWidgetsByProp = new Dictionary<Prop, PropWidget>();

	// Token: 0x040005A8 RID: 1448
	[TupleElementNames(new string[] { "prop", "anim" })]
	private List<ValueTuple<Prop, InkAnimation>> _pendingAnimatedPropDeactivations = new List<ValueTuple<Prop, InkAnimation>>();

	// Token: 0x040005A9 RID: 1449
	private List<GameChoice> _exitTriggerZoneChoices = new List<GameChoice>();

	// Token: 0x040005AA RID: 1450
	private bool _paused;

	// Token: 0x040005AB RID: 1451
	private bool _sourcePaused;

	// Token: 0x040005AC RID: 1452
	[SerializeField]
	private Prototype _subtitleProto;

	// Token: 0x040005AD RID: 1453
	[SerializeField]
	private Prototype _dialogueBubbleProto;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	private Prototype _propChoicesProto;

	// Token: 0x040005AF RID: 1455
	[SerializeField]
	private Prototype _narrativeChoicesProto;

	// Token: 0x040005B0 RID: 1456
	[SerializeField]
	private SLayout _subtitlesLayout;

	// Token: 0x040005B1 RID: 1457
	[SerializeField]
	private NarrativePresenterSettings _settings;

	// Token: 0x040005B2 RID: 1458
	[SerializeField]
	private SLayoutCanvasTimeScalar _timeScalar;

	// Token: 0x020002BF RID: 703
	// (Invoke) Token: 0x06001614 RID: 5652
	public delegate void ChooseChoiceDelegate(GameChoice choice, string optionalInterctableName);

	// Token: 0x020002C0 RID: 704
	// (Invoke) Token: 0x06001618 RID: 5656
	public delegate void WillChooseChoiceOnPropDelegate(Prop prop);

	// Token: 0x020002C1 RID: 705
	public struct Item
	{
		// Token: 0x04001612 RID: 5650
		public string text;

		// Token: 0x04001613 RID: 5651
		public string audioClipName;

		// Token: 0x04001614 RID: 5652
		public string sourceCharacterName;

		// Token: 0x04001615 RID: 5653
		public List<GameChoice> choices;
	}
}
