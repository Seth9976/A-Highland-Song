using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x0200002D RID: 45
	public class StoryState
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060002BF RID: 703 RVA: 0x00011050 File Offset: 0x0000F250
		// (remove) Token: 0x060002C0 RID: 704 RVA: 0x00011088 File Offset: 0x0000F288
		public event Action onDidLoadState;

		// Token: 0x060002C1 RID: 705 RVA: 0x000110C0 File Offset: 0x0000F2C0
		public string ToJson()
		{
			SimpleJson.Writer writer = new SimpleJson.Writer();
			this.WriteJson(writer);
			return writer.ToString();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000110E0 File Offset: 0x0000F2E0
		public void ToJson(Stream stream)
		{
			SimpleJson.Writer writer = new SimpleJson.Writer(stream);
			this.WriteJson(writer);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000110FC File Offset: 0x0000F2FC
		public void LoadJson(string json)
		{
			Dictionary<string, object> dictionary = SimpleJson.TextToDictionary(json);
			this.LoadJsonObj(dictionary);
			if (this.onDidLoadState != null)
			{
				this.onDidLoadState();
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001112C File Offset: 0x0000F32C
		public int VisitCountAtPathString(string pathString)
		{
			int num;
			if (this._patch != null)
			{
				Container container = this.story.ContentAtPath(new Path(pathString)).container;
				if (container == null)
				{
					throw new Exception("Content at path not found: " + pathString);
				}
				if (this._patch.TryGetVisitCount(container, out num))
				{
					return num;
				}
			}
			if (this._visitCounts.TryGetValue(pathString, out num))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0001119C File Offset: 0x0000F39C
		public int VisitCountForContainer(Container container)
		{
			if (!container.visitsShouldBeCounted)
			{
				Story story = this.story;
				string[] array = new string[5];
				array[0] = "Read count for target (";
				array[1] = container.name;
				array[2] = " - on ";
				int num = 3;
				DebugMetadata debugMetadata = container.debugMetadata;
				array[num] = ((debugMetadata != null) ? debugMetadata.ToString() : null);
				array[4] = ") unknown.";
				story.Error(string.Concat(array), false);
				return 0;
			}
			int num2 = 0;
			if (this._patch != null && this._patch.TryGetVisitCount(container, out num2))
			{
				return num2;
			}
			string text = container.path.ToString();
			this._visitCounts.TryGetValue(text, out num2);
			return num2;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00011238 File Offset: 0x0000F438
		public void IncrementVisitCountForContainer(Container container)
		{
			if (this._patch != null)
			{
				int num = this.VisitCountForContainer(container);
				num++;
				this._patch.SetVisitCount(container, num);
				return;
			}
			int num2 = 0;
			string text = container.path.ToString();
			this._visitCounts.TryGetValue(text, out num2);
			num2++;
			this._visitCounts[text] = num2;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00011298 File Offset: 0x0000F498
		public void RecordTurnIndexVisitToContainer(Container container)
		{
			if (this._patch != null)
			{
				this._patch.SetTurnIndex(container, this.currentTurnIndex);
				return;
			}
			string text = container.path.ToString();
			this._turnIndices[text] = this.currentTurnIndex;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000112E0 File Offset: 0x0000F4E0
		public int TurnsSinceForContainer(Container container)
		{
			if (!container.turnIndexShouldBeCounted)
			{
				Story story = this.story;
				string[] array = new string[5];
				array[0] = "TURNS_SINCE() for target (";
				array[1] = container.name;
				array[2] = " - on ";
				int num = 3;
				DebugMetadata debugMetadata = container.debugMetadata;
				array[num] = ((debugMetadata != null) ? debugMetadata.ToString() : null);
				array[4] = ") unknown.";
				story.Error(string.Concat(array), false);
			}
			int num2 = 0;
			if (this._patch != null && this._patch.TryGetTurnIndex(container, out num2))
			{
				return this.currentTurnIndex - num2;
			}
			string text = container.path.ToString();
			if (this._turnIndices.TryGetValue(text, out num2))
			{
				return this.currentTurnIndex - num2;
			}
			return -1;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0001138B File Offset: 0x0000F58B
		public int callstackDepth
		{
			get
			{
				return this.callStack.depth;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00011398 File Offset: 0x0000F598
		public List<Object> outputStream
		{
			get
			{
				return this._currentFlow.outputStream;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002CB RID: 715 RVA: 0x000113A5 File Offset: 0x0000F5A5
		public List<Choice> currentChoices
		{
			get
			{
				if (this.canContinue)
				{
					return new List<Choice>();
				}
				return this._currentFlow.currentChoices;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002CC RID: 716 RVA: 0x000113C0 File Offset: 0x0000F5C0
		public List<Choice> generatedChoices
		{
			get
			{
				return this._currentFlow.currentChoices;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002CD RID: 717 RVA: 0x000113CD File Offset: 0x0000F5CD
		// (set) Token: 0x060002CE RID: 718 RVA: 0x000113D5 File Offset: 0x0000F5D5
		public List<string> currentErrors { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002CF RID: 719 RVA: 0x000113DE File Offset: 0x0000F5DE
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x000113E6 File Offset: 0x0000F5E6
		public List<string> currentWarnings { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x000113EF File Offset: 0x0000F5EF
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x000113F7 File Offset: 0x0000F5F7
		public VariablesState variablesState { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00011400 File Offset: 0x0000F600
		public CallStack callStack
		{
			get
			{
				return this._currentFlow.callStack;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0001140D File Offset: 0x0000F60D
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00011415 File Offset: 0x0000F615
		public List<Object> evaluationStack { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0001141E File Offset: 0x0000F61E
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00011426 File Offset: 0x0000F626
		public Pointer divertedPointer { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0001142F File Offset: 0x0000F62F
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00011437 File Offset: 0x0000F637
		public int currentTurnIndex { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00011440 File Offset: 0x0000F640
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00011448 File Offset: 0x0000F648
		public int storySeed { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00011451 File Offset: 0x0000F651
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00011459 File Offset: 0x0000F659
		public int previousRandom { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00011462 File Offset: 0x0000F662
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0001146A File Offset: 0x0000F66A
		public bool didSafeExit { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00011473 File Offset: 0x0000F673
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0001147B File Offset: 0x0000F67B
		public Story story { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00011484 File Offset: 0x0000F684
		public string currentPathString
		{
			get
			{
				Pointer currentPointer = this.currentPointer;
				if (currentPointer.isNull)
				{
					return null;
				}
				return currentPointer.path.ToString();
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x000114B0 File Offset: 0x0000F6B0
		public string previousPathString
		{
			get
			{
				Pointer previousPointer = this.previousPointer;
				if (previousPointer.isNull)
				{
					return null;
				}
				return previousPointer.path.ToString();
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x000114DB File Offset: 0x0000F6DB
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x000114ED File Offset: 0x0000F6ED
		public Pointer currentPointer
		{
			get
			{
				return this.callStack.currentElement.currentPointer;
			}
			set
			{
				this.callStack.currentElement.currentPointer = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00011500 File Offset: 0x0000F700
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00011512 File Offset: 0x0000F712
		public Pointer previousPointer
		{
			get
			{
				return this.callStack.currentThread.previousPointer;
			}
			set
			{
				this.callStack.currentThread.previousPointer = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00011528 File Offset: 0x0000F728
		public bool canContinue
		{
			get
			{
				return !this.currentPointer.isNull && !this.hasError;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00011550 File Offset: 0x0000F750
		public bool hasError
		{
			get
			{
				return this.currentErrors != null && this.currentErrors.Count > 0;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0001156A File Offset: 0x0000F76A
		public bool hasWarning
		{
			get
			{
				return this.currentWarnings != null && this.currentWarnings.Count > 0;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00011584 File Offset: 0x0000F784
		public string currentText
		{
			get
			{
				if (this._outputStreamTextDirty)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (Object @object in this.outputStream)
					{
						StringValue stringValue = @object as StringValue;
						if (stringValue != null)
						{
							stringBuilder.Append(stringValue.value);
						}
					}
					this._currentText = this.CleanOutputWhitespace(stringBuilder.ToString());
					this._outputStreamTextDirty = false;
				}
				return this._currentText;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00011618 File Offset: 0x0000F818
		private string CleanOutputWhitespace(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length);
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				bool flag = c == ' ' || c == '\t';
				if (flag && num == -1)
				{
					num = i;
				}
				if (!flag)
				{
					if (c != '\n' && num > 0 && num != num2)
					{
						stringBuilder.Append(' ');
					}
					num = -1;
				}
				if (c == '\n')
				{
					num2 = i + 1;
				}
				if (!flag)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000116A0 File Offset: 0x0000F8A0
		public List<string> currentTags
		{
			get
			{
				if (this._outputStreamTagsDirty)
				{
					this._currentTags = new List<string>();
					foreach (Object @object in this.outputStream)
					{
						Tag tag = @object as Tag;
						if (tag != null)
						{
							this._currentTags.Add(tag.text);
						}
					}
					this._outputStreamTagsDirty = false;
				}
				return this._currentTags;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0001172C File Offset: 0x0000F92C
		public string currentFlowName
		{
			get
			{
				return this._currentFlow.name;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00011739 File Offset: 0x0000F939
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x0001174B File Offset: 0x0000F94B
		public bool inExpressionEvaluation
		{
			get
			{
				return this.callStack.currentElement.inExpressionEvaluation;
			}
			set
			{
				this.callStack.currentElement.inExpressionEvaluation = value;
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00011760 File Offset: 0x0000F960
		public StoryState(Story story)
		{
			this.story = story;
			this._currentFlow = new Flow("DEFAULT_FLOW", story);
			this.OutputStreamDirty();
			this.evaluationStack = new List<Object>();
			this.variablesState = new VariablesState(this.callStack, story.listDefinitions);
			this._visitCounts = new Dictionary<string, int>();
			this._turnIndices = new Dictionary<string, int>();
			this.currentTurnIndex = -1;
			int millisecond = DateTime.Now.Millisecond;
			this.storySeed = new Random(millisecond).Next() % 100;
			this.previousRandom = 0;
			this.GoToStart();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001180D File Offset: 0x0000FA0D
		public void GoToStart()
		{
			this.callStack.currentElement.currentPointer = Pointer.StartOf(this.story.mainContentContainer);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00011830 File Offset: 0x0000FA30
		internal void SwitchFlow_Internal(string flowName)
		{
			if (flowName == null)
			{
				throw new Exception("Must pass a non-null string to Story.SwitchFlow");
			}
			if (this._namedFlows == null)
			{
				this._namedFlows = new Dictionary<string, Flow>();
				this._namedFlows["DEFAULT_FLOW"] = this._currentFlow;
			}
			if (flowName == this._currentFlow.name)
			{
				return;
			}
			Flow flow;
			if (!this._namedFlows.TryGetValue(flowName, out flow))
			{
				flow = new Flow(flowName, this.story);
				this._namedFlows[flowName] = flow;
			}
			this._currentFlow = flow;
			this.variablesState.callStack = this._currentFlow.callStack;
			this.OutputStreamDirty();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000118D5 File Offset: 0x0000FAD5
		internal void SwitchToDefaultFlow_Internal()
		{
			if (this._namedFlows == null)
			{
				return;
			}
			this.SwitchFlow_Internal("DEFAULT_FLOW");
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000118EC File Offset: 0x0000FAEC
		internal void RemoveFlow_Internal(string flowName)
		{
			if (flowName == null)
			{
				throw new Exception("Must pass a non-null string to Story.DestroyFlow");
			}
			if (flowName == "DEFAULT_FLOW")
			{
				throw new Exception("Cannot destroy default flow");
			}
			if (this._currentFlow.name == flowName)
			{
				this.SwitchToDefaultFlow_Internal();
			}
			this._namedFlows.Remove(flowName);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00011948 File Offset: 0x0000FB48
		public StoryState CopyAndStartPatching(bool forBackgroundSave)
		{
			StoryState storyState = new StoryState(this.story);
			storyState._patch = new StatePatch(this._patch);
			storyState._currentFlow.name = this._currentFlow.name;
			storyState._currentFlow.callStack = new CallStack(this._currentFlow.callStack);
			storyState._currentFlow.outputStream.AddRange(this._currentFlow.outputStream);
			storyState.OutputStreamDirty();
			if (forBackgroundSave)
			{
				using (List<Choice>.Enumerator enumerator = this._currentFlow.currentChoices.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Choice choice = enumerator.Current;
						storyState._currentFlow.currentChoices.Add(choice.Clone());
					}
					goto IL_00D7;
				}
			}
			storyState._currentFlow.currentChoices.AddRange(this._currentFlow.currentChoices);
			IL_00D7:
			if (this._namedFlows != null)
			{
				storyState._namedFlows = new Dictionary<string, Flow>();
				foreach (KeyValuePair<string, Flow> keyValuePair in this._namedFlows)
				{
					storyState._namedFlows[keyValuePair.Key] = keyValuePair.Value;
				}
				storyState._namedFlows[this._currentFlow.name] = storyState._currentFlow;
			}
			if (this.hasError)
			{
				storyState.currentErrors = new List<string>();
				storyState.currentErrors.AddRange(this.currentErrors);
			}
			if (this.hasWarning)
			{
				storyState.currentWarnings = new List<string>();
				storyState.currentWarnings.AddRange(this.currentWarnings);
			}
			storyState.variablesState = this.variablesState;
			storyState.variablesState.callStack = storyState.callStack;
			storyState.variablesState.patch = storyState._patch;
			storyState.evaluationStack.AddRange(this.evaluationStack);
			if (!this.divertedPointer.isNull)
			{
				storyState.divertedPointer = this.divertedPointer;
			}
			storyState.previousPointer = this.previousPointer;
			storyState._visitCounts = this._visitCounts;
			storyState._turnIndices = this._turnIndices;
			storyState.currentTurnIndex = this.currentTurnIndex;
			storyState.storySeed = this.storySeed;
			storyState.previousRandom = this.previousRandom;
			storyState.didSafeExit = this.didSafeExit;
			return storyState;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00011BBC File Offset: 0x0000FDBC
		public void RestoreAfterPatch()
		{
			this.variablesState.callStack = this.callStack;
			this.variablesState.patch = this._patch;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00011BE0 File Offset: 0x0000FDE0
		public void ApplyAnyPatch()
		{
			if (this._patch == null)
			{
				return;
			}
			this.variablesState.ApplyPatch();
			foreach (KeyValuePair<Container, int> keyValuePair in this._patch.visitCounts)
			{
				this.ApplyCountChanges(keyValuePair.Key, keyValuePair.Value, true);
			}
			foreach (KeyValuePair<Container, int> keyValuePair2 in this._patch.turnIndices)
			{
				this.ApplyCountChanges(keyValuePair2.Key, keyValuePair2.Value, false);
			}
			this._patch = null;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00011CB8 File Offset: 0x0000FEB8
		private void ApplyCountChanges(Container container, int newCount, bool isVisit)
		{
			(isVisit ? this._visitCounts : this._turnIndices)[container.path.ToString()] = newCount;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00011CDC File Offset: 0x0000FEDC
		private void WriteJson(SimpleJson.Writer writer)
		{
			writer.WriteObjectStart();
			writer.WritePropertyStart("flows");
			writer.WriteObjectStart();
			if (this._namedFlows != null)
			{
				using (Dictionary<string, Flow>.Enumerator enumerator = this._namedFlows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, Flow> keyValuePair = enumerator.Current;
						writer.WriteProperty(keyValuePair.Key, new Action<SimpleJson.Writer>(keyValuePair.Value.WriteJson));
					}
					goto IL_008F;
				}
			}
			writer.WriteProperty(this._currentFlow.name, new Action<SimpleJson.Writer>(this._currentFlow.WriteJson));
			IL_008F:
			writer.WriteObjectEnd();
			writer.WritePropertyEnd();
			writer.WriteProperty("currentFlowName", this._currentFlow.name);
			writer.WriteProperty("variablesState", new Action<SimpleJson.Writer>(this.variablesState.WriteJson));
			writer.WriteProperty("evalStack", delegate(SimpleJson.Writer w)
			{
				Json.WriteListRuntimeObjs(w, this.evaluationStack);
			});
			if (!this.divertedPointer.isNull)
			{
				writer.WriteProperty("currentDivertTarget", this.divertedPointer.path.componentsString);
			}
			writer.WriteProperty("visitCounts", delegate(SimpleJson.Writer w)
			{
				Json.WriteIntDictionary(w, this._visitCounts);
			});
			writer.WriteProperty("turnIndices", delegate(SimpleJson.Writer w)
			{
				Json.WriteIntDictionary(w, this._turnIndices);
			});
			writer.WriteProperty("turnIdx", this.currentTurnIndex);
			writer.WriteProperty("storySeed", this.storySeed);
			writer.WriteProperty("previousRandom", this.previousRandom);
			writer.WriteProperty("inkSaveVersion", 9);
			writer.WriteProperty("inkFormatVersion", 20);
			writer.WriteObjectEnd();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00011E8C File Offset: 0x0001008C
		private void LoadJsonObj(Dictionary<string, object> jObject)
		{
			object obj = null;
			if (!jObject.TryGetValue("inkSaveVersion", out obj))
			{
				throw new Exception("ink save format incorrect, can't load.");
			}
			if ((int)obj < 8)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Ink save format isn't compatible with the current version (saw '",
					(obj != null) ? obj.ToString() : null,
					"', but minimum is ",
					8.ToString(),
					"), so can't load."
				}));
			}
			object obj2 = null;
			if (jObject.TryGetValue("flows", out obj2))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj2;
				if (dictionary.Count == 1)
				{
					this._namedFlows = null;
				}
				else if (this._namedFlows == null)
				{
					this._namedFlows = new Dictionary<string, Flow>();
				}
				else
				{
					this._namedFlows.Clear();
				}
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					string key = keyValuePair.Key;
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)keyValuePair.Value;
					Flow flow = new Flow(key, this.story, dictionary2);
					if (dictionary.Count == 1)
					{
						this._currentFlow = new Flow(key, this.story, dictionary2);
					}
					else
					{
						this._namedFlows[key] = flow;
					}
				}
				if (this._namedFlows != null && this._namedFlows.Count > 1)
				{
					string text = (string)jObject["currentFlowName"];
					this._currentFlow = this._namedFlows[text];
				}
			}
			else
			{
				this._namedFlows = null;
				this._currentFlow.name = "DEFAULT_FLOW";
				this._currentFlow.callStack.SetJsonToken((Dictionary<string, object>)jObject["callstackThreads"], this.story);
				this._currentFlow.outputStream = Json.JArrayToRuntimeObjList((List<object>)jObject["outputStream"], false);
				this._currentFlow.currentChoices = Json.JArrayToRuntimeObjList<Choice>((List<object>)jObject["currentChoices"], false);
				object obj3 = null;
				jObject.TryGetValue("choiceThreads", out obj3);
				this._currentFlow.LoadFlowChoiceThreads((Dictionary<string, object>)obj3, this.story);
			}
			this.OutputStreamDirty();
			this.variablesState.SetJsonToken((Dictionary<string, object>)jObject["variablesState"]);
			this.variablesState.callStack = this._currentFlow.callStack;
			this.evaluationStack = Json.JArrayToRuntimeObjList((List<object>)jObject["evalStack"], false);
			object obj4;
			if (jObject.TryGetValue("currentDivertTarget", out obj4))
			{
				Path path = new Path(obj4.ToString());
				this.divertedPointer = this.story.PointerAtPath(path);
			}
			this._visitCounts = Json.JObjectToIntDictionary((Dictionary<string, object>)jObject["visitCounts"]);
			this._turnIndices = Json.JObjectToIntDictionary((Dictionary<string, object>)jObject["turnIndices"]);
			this.currentTurnIndex = (int)jObject["turnIdx"];
			this.storySeed = (int)jObject["storySeed"];
			object obj5 = null;
			if (jObject.TryGetValue("previousRandom", out obj5))
			{
				this.previousRandom = (int)obj5;
				return;
			}
			this.previousRandom = 0;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000121E0 File Offset: 0x000103E0
		public void ResetErrors()
		{
			this.currentErrors = null;
			this.currentWarnings = null;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000121F0 File Offset: 0x000103F0
		public void ResetOutput(List<Object> objs = null)
		{
			this.outputStream.Clear();
			if (objs != null)
			{
				this.outputStream.AddRange(objs);
			}
			this.OutputStreamDirty();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00012214 File Offset: 0x00010414
		public void PushToOutputStream(Object obj)
		{
			StringValue stringValue = obj as StringValue;
			if (stringValue)
			{
				List<StringValue> list = this.TrySplittingHeadTailWhitespace(stringValue);
				if (list != null)
				{
					foreach (StringValue stringValue2 in list)
					{
						this.PushToOutputStreamIndividual(stringValue2);
					}
					this.OutputStreamDirty();
					return;
				}
			}
			this.PushToOutputStreamIndividual(obj);
			this.OutputStreamDirty();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00012290 File Offset: 0x00010490
		public void PopFromOutputStream(int count)
		{
			this.outputStream.RemoveRange(this.outputStream.Count - count, count);
			this.OutputStreamDirty();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000122B4 File Offset: 0x000104B4
		private List<StringValue> TrySplittingHeadTailWhitespace(StringValue single)
		{
			string value = single.value;
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				if (c == '\n')
				{
					if (num == -1)
					{
						num = i;
					}
					num2 = i;
				}
				else if (c != ' ' && c != '\t')
				{
					break;
				}
			}
			int num3 = -1;
			int num4 = -1;
			for (int j = value.Length - 1; j >= 0; j--)
			{
				char c2 = value[j];
				if (c2 == '\n')
				{
					if (num3 == -1)
					{
						num3 = j;
					}
					num4 = j;
				}
				else if (c2 != ' ' && c2 != '\t')
				{
					break;
				}
			}
			if (num == -1 && num3 == -1)
			{
				return null;
			}
			List<StringValue> list = new List<StringValue>();
			int num5 = 0;
			int num6 = value.Length;
			if (num != -1)
			{
				if (num > 0)
				{
					StringValue stringValue = new StringValue(value.Substring(0, num));
					list.Add(stringValue);
				}
				list.Add(new StringValue("\n"));
				num5 = num2 + 1;
			}
			if (num3 != -1)
			{
				num6 = num4;
			}
			if (num6 > num5)
			{
				string text = value.Substring(num5, num6 - num5);
				list.Add(new StringValue(text));
			}
			if (num3 != -1 && num4 > num2)
			{
				list.Add(new StringValue("\n"));
				if (num3 < value.Length - 1)
				{
					int num7 = value.Length - num3 - 1;
					StringValue stringValue2 = new StringValue(value.Substring(num3 + 1, num7));
					list.Add(stringValue2);
				}
			}
			return list;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001241C File Offset: 0x0001061C
		private void PushToOutputStreamIndividual(Object obj)
		{
			Object @object = obj as Glue;
			StringValue stringValue = obj as StringValue;
			bool flag = true;
			if (@object)
			{
				this.TrimNewlinesFromOutputStream();
				flag = true;
			}
			else if (stringValue)
			{
				int num = -1;
				CallStack.Element currentElement = this.callStack.currentElement;
				if (currentElement.type == PushPopType.Function)
				{
					num = currentElement.functionStartInOuputStream;
				}
				int num2 = -1;
				int i = this.outputStream.Count - 1;
				while (i >= 0)
				{
					Object object2 = this.outputStream[i];
					ControlCommand controlCommand = object2 as ControlCommand;
					if (object2 as Glue)
					{
						num2 = i;
						break;
					}
					if (controlCommand && controlCommand.commandType == ControlCommand.CommandType.BeginString)
					{
						if (i >= num)
						{
							num = -1;
							break;
						}
						break;
					}
					else
					{
						i--;
					}
				}
				int num3;
				if (num2 != -1 && num != -1)
				{
					num3 = Math.Min(num, num2);
				}
				else if (num2 != -1)
				{
					num3 = num2;
				}
				else
				{
					num3 = num;
				}
				if (num3 != -1)
				{
					if (stringValue.isNewline)
					{
						flag = false;
					}
					else if (stringValue.isNonWhitespace)
					{
						if (num2 > -1)
						{
							this.RemoveExistingGlue();
						}
						if (num > -1)
						{
							List<CallStack.Element> elements = this.callStack.elements;
							for (int j = elements.Count - 1; j >= 0; j--)
							{
								CallStack.Element element = elements[j];
								if (element.type != PushPopType.Function)
								{
									break;
								}
								element.functionStartInOuputStream = -1;
							}
						}
					}
				}
				else if (stringValue.isNewline && (this.outputStreamEndsInNewline || !this.outputStreamContainsContent))
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.outputStream.Add(obj);
				this.OutputStreamDirty();
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00012598 File Offset: 0x00010798
		private void TrimNewlinesFromOutputStream()
		{
			int num = -1;
			for (int i = this.outputStream.Count - 1; i >= 0; i--)
			{
				Object @object = this.outputStream[i];
				ControlCommand controlCommand = @object as ControlCommand;
				StringValue stringValue = @object as StringValue;
				if (controlCommand || (stringValue && stringValue.isNonWhitespace))
				{
					break;
				}
				if (stringValue && stringValue.isNewline)
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				int i = num;
				while (i < this.outputStream.Count)
				{
					if (this.outputStream[i] as StringValue)
					{
						this.outputStream.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			this.OutputStreamDirty();
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00012648 File Offset: 0x00010848
		private void RemoveExistingGlue()
		{
			for (int i = this.outputStream.Count - 1; i >= 0; i--)
			{
				Object @object = this.outputStream[i];
				if (@object is Glue)
				{
					this.outputStream.RemoveAt(i);
				}
				else if (@object is ControlCommand)
				{
					break;
				}
			}
			this.OutputStreamDirty();
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000304 RID: 772 RVA: 0x000126A0 File Offset: 0x000108A0
		public bool outputStreamEndsInNewline
		{
			get
			{
				if (this.outputStream.Count > 0)
				{
					int num = this.outputStream.Count - 1;
					while (num >= 0 && !(this.outputStream[num] is ControlCommand))
					{
						StringValue stringValue = this.outputStream[num] as StringValue;
						if (stringValue)
						{
							if (stringValue.isNewline)
							{
								return true;
							}
							if (stringValue.isNonWhitespace)
							{
								break;
							}
						}
						num--;
					}
				}
				return false;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00012714 File Offset: 0x00010914
		public bool outputStreamContainsContent
		{
			get
			{
				using (List<Object>.Enumerator enumerator = this.outputStream.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is StringValue)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00012770 File Offset: 0x00010970
		public bool inStringEvaluation
		{
			get
			{
				for (int i = this.outputStream.Count - 1; i >= 0; i--)
				{
					ControlCommand controlCommand = this.outputStream[i] as ControlCommand;
					if (controlCommand && controlCommand.commandType == ControlCommand.CommandType.BeginString)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000127BC File Offset: 0x000109BC
		public void PushEvaluationStack(Object obj)
		{
			ListValue listValue = obj as ListValue;
			if (listValue)
			{
				InkList value = listValue.value;
				if (value.originNames != null)
				{
					if (value.origins == null)
					{
						value.origins = new List<ListDefinition>();
					}
					value.origins.Clear();
					foreach (string text in value.originNames)
					{
						ListDefinition listDefinition = null;
						this.story.listDefinitions.TryListGetDefinition(text, out listDefinition);
						if (!value.origins.Contains(listDefinition))
						{
							value.origins.Add(listDefinition);
						}
					}
				}
			}
			this.evaluationStack.Add(obj);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001288C File Offset: 0x00010A8C
		public Object PopEvaluationStack()
		{
			Object @object = this.evaluationStack[this.evaluationStack.Count - 1];
			this.evaluationStack.RemoveAt(this.evaluationStack.Count - 1);
			return @object;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000128BE File Offset: 0x00010ABE
		public Object PeekEvaluationStack()
		{
			return this.evaluationStack[this.evaluationStack.Count - 1];
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000128D8 File Offset: 0x00010AD8
		public List<Object> PopEvaluationStack(int numberOfObjects)
		{
			if (numberOfObjects > this.evaluationStack.Count)
			{
				throw new Exception("trying to pop too many objects");
			}
			List<Object> range = this.evaluationStack.GetRange(this.evaluationStack.Count - numberOfObjects, numberOfObjects);
			this.evaluationStack.RemoveRange(this.evaluationStack.Count - numberOfObjects, numberOfObjects);
			return range;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00012930 File Offset: 0x00010B30
		public void ForceEnd()
		{
			this.callStack.Reset();
			this._currentFlow.currentChoices.Clear();
			this.currentPointer = Pointer.Null;
			this.previousPointer = Pointer.Null;
			this.didSafeExit = true;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0001296C File Offset: 0x00010B6C
		private void TrimWhitespaceFromFunctionEnd()
		{
			int num = this.callStack.currentElement.functionStartInOuputStream;
			if (num == -1)
			{
				num = 0;
			}
			for (int i = this.outputStream.Count - 1; i >= num; i--)
			{
				Object @object = this.outputStream[i];
				StringValue stringValue = @object as StringValue;
				ControlCommand controlCommand = @object as ControlCommand;
				if (stringValue)
				{
					if (controlCommand || (!stringValue.isNewline && !stringValue.isInlineWhitespace))
					{
						break;
					}
					this.outputStream.RemoveAt(i);
					this.OutputStreamDirty();
				}
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000129F3 File Offset: 0x00010BF3
		public void PopCallstack(PushPopType? popType = null)
		{
			if (this.callStack.currentElement.type == PushPopType.Function)
			{
				this.TrimWhitespaceFromFunctionEnd();
			}
			this.callStack.Pop(popType);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00012A1C File Offset: 0x00010C1C
		public void SetChosenPath(Path path, bool incrementingTurnIndex)
		{
			this._currentFlow.currentChoices.Clear();
			Pointer pointer = this.story.PointerAtPath(path);
			if (!pointer.isNull && pointer.index == -1)
			{
				pointer.index = 0;
			}
			this.currentPointer = pointer;
			if (incrementingTurnIndex)
			{
				int currentTurnIndex = this.currentTurnIndex;
				this.currentTurnIndex = currentTurnIndex + 1;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00012A7A File Offset: 0x00010C7A
		public void StartFunctionEvaluationFromGame(Container funcContainer, params object[] arguments)
		{
			this.callStack.Push(PushPopType.FunctionEvaluationFromGame, this.evaluationStack.Count, 0);
			this.callStack.currentElement.currentPointer = Pointer.StartOf(funcContainer);
			this.PassArgumentsToEvaluationStack(arguments);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00012AB4 File Offset: 0x00010CB4
		public void PassArgumentsToEvaluationStack(params object[] arguments)
		{
			if (arguments != null)
			{
				for (int i = 0; i < arguments.Length; i++)
				{
					if (!(arguments[i] is int) && !(arguments[i] is float) && !(arguments[i] is string) && !(arguments[i] is bool) && !(arguments[i] is InkList))
					{
						throw new ArgumentException("ink arguments when calling EvaluateFunction / ChoosePathStringWithParameters must be int, float, string, bool or InkList. Argument was " + ((arguments[i] == null) ? "null" : arguments[i].GetType().Name));
					}
					this.PushEvaluationStack(Value.Create(arguments[i]));
				}
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00012B3B File Offset: 0x00010D3B
		public bool TryExitFunctionEvaluationFromGame()
		{
			if (this.callStack.currentElement.type == PushPopType.FunctionEvaluationFromGame)
			{
				this.currentPointer = Pointer.Null;
				this.didSafeExit = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00012B68 File Offset: 0x00010D68
		public object CompleteFunctionEvaluationFromGame()
		{
			if (this.callStack.currentElement.type != PushPopType.FunctionEvaluationFromGame)
			{
				throw new Exception("Expected external function evaluation to be complete. Stack trace: " + this.callStack.callStackTrace);
			}
			int evaluationStackHeightWhenPushed = this.callStack.currentElement.evaluationStackHeightWhenPushed;
			Object @object = null;
			while (this.evaluationStack.Count > evaluationStackHeightWhenPushed)
			{
				Object object2 = this.PopEvaluationStack();
				if (@object == null)
				{
					@object = object2;
				}
			}
			this.PopCallstack(new PushPopType?(PushPopType.FunctionEvaluationFromGame));
			if (!@object)
			{
				return null;
			}
			if (@object is Void)
			{
				return null;
			}
			Value value = @object as Value;
			if (value.valueType == ValueType.DivertTarget)
			{
				return value.valueObject.ToString();
			}
			return value.valueObject;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00012C1C File Offset: 0x00010E1C
		public void AddError(string message, bool isWarning)
		{
			if (!isWarning)
			{
				if (this.currentErrors == null)
				{
					this.currentErrors = new List<string>();
				}
				this.currentErrors.Add(message);
				return;
			}
			if (this.currentWarnings == null)
			{
				this.currentWarnings = new List<string>();
			}
			this.currentWarnings.Add(message);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00012C6B File Offset: 0x00010E6B
		private void OutputStreamDirty()
		{
			this._outputStreamTextDirty = true;
			this._outputStreamTagsDirty = true;
		}

		// Token: 0x040000E0 RID: 224
		public const int kInkSaveStateVersion = 9;

		// Token: 0x040000E1 RID: 225
		private const int kMinCompatibleLoadVersion = 8;

		// Token: 0x040000ED RID: 237
		private string _currentText;

		// Token: 0x040000EE RID: 238
		private List<string> _currentTags;

		// Token: 0x040000EF RID: 239
		private Dictionary<string, int> _visitCounts;

		// Token: 0x040000F0 RID: 240
		private Dictionary<string, int> _turnIndices;

		// Token: 0x040000F1 RID: 241
		private bool _outputStreamTextDirty = true;

		// Token: 0x040000F2 RID: 242
		private bool _outputStreamTagsDirty = true;

		// Token: 0x040000F3 RID: 243
		private StatePatch _patch;

		// Token: 0x040000F4 RID: 244
		private Flow _currentFlow;

		// Token: 0x040000F5 RID: 245
		private Dictionary<string, Flow> _namedFlows;

		// Token: 0x040000F6 RID: 246
		private const string kDefaultFlowName = "DEFAULT_FLOW";
	}
}
