using System;
using System.Collections.Generic;
using Ink.Runtime;

// Token: 0x02000118 RID: 280
public class JournalInkContentCache : MonoSingleton<JournalInkContentCache>
{
	// Token: 0x0600098B RID: 2443 RVA: 0x00050C78 File Offset: 0x0004EE78
	public string GetFactDescription(InkListItem fact)
	{
		string text = null;
		if (!this.factDescriptionCache.TryGetValue(fact, out text))
		{
			Story inkStory = Narrative.instance.inkStory;
			inkStory.EvaluateFunction("DiscoveryDescription", out text, new object[]
			{
				new InkList(fact, inkStory)
			});
			text = InkStylingUtility.ProcessText(text, true, false);
			text = InkStylingUtility.ParseStyling(text, true, "#FF1800");
			this.factDescriptionCache.Add(fact, text);
		}
		return text;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00050CE4 File Offset: 0x0004EEE4
	public IEnumerable<KeyValuePair<InkListItem, string>> GetAllCachedFactsAndDescriptions()
	{
		foreach (KeyValuePair<InkListItem, string> keyValuePair in this.factDescriptionCache)
		{
			yield return keyValuePair;
		}
		Dictionary<InkListItem, string>.Enumerator enumerator = default(Dictionary<InkListItem, string>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00050CF4 File Offset: 0x0004EEF4
	public string GetTranslation(InkListItem fact)
	{
		string text = null;
		if (!this.translationsCache.TryGetValue(fact, out text))
		{
			Story inkStory = Narrative.instance.inkStory;
			inkStory.EvaluateFunction("DescribeKnownEnglishGaelicName", out text, new object[]
			{
				new InkList(fact, inkStory)
			});
			text = InkStylingUtility.ProcessText(text, true, false);
			text = InkStylingUtility.ParseStyling(text, true, "#FF1800");
			this.translationsCache.Add(fact, text);
		}
		return text;
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00050D60 File Offset: 0x0004EF60
	public IEnumerable<KeyValuePair<InkListItem, string>> GetAllCachedTranslations()
	{
		foreach (KeyValuePair<InkListItem, string> keyValuePair in this.translationsCache)
		{
			yield return keyValuePair;
		}
		Dictionary<InkListItem, string>.Enumerator enumerator = default(Dictionary<InkListItem, string>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x00050D70 File Offset: 0x0004EF70
	private void OnEnable()
	{
		Narrative.onCreateStory += this.SubscribeToStory;
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00050D83 File Offset: 0x0004EF83
	private void OnDisable()
	{
		Narrative.onCreateStory -= this.SubscribeToStory;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x00050D98 File Offset: 0x0004EF98
	private void SubscribeToStory(Story story)
	{
		this.discoveredFactsChangeHandler.RemoveVariableObserver(null);
		this.discoveredFactsChangeHandler.AddVariableObserver(story);
		this.discoveredFactsChangeHandler.RefreshValue(story, false);
		this.knownPeaksChangeHandler.RemoveVariableObserver(null);
		this.knownPeaksChangeHandler.AddVariableObserver(story);
		this.knownPeaksChangeHandler.RefreshValue(story, false);
	}

	// Token: 0x04000B72 RID: 2930
	public InkListChangeHandler discoveredFactsChangeHandler = new InkListChangeHandler("DiscoveredFacts");

	// Token: 0x04000B73 RID: 2931
	private Dictionary<InkListItem, string> factDescriptionCache = new Dictionary<InkListItem, string>();

	// Token: 0x04000B74 RID: 2932
	public InkListChangeHandler knownPeaksChangeHandler = new InkListChangeHandler("KnownPeaks");

	// Token: 0x04000B75 RID: 2933
	private Dictionary<InkListItem, string> translationsCache = new Dictionary<InkListItem, string>();
}
