using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x0200002B RID: 43
	public class Story : Object
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000D958 File Offset: 0x0000BB58
		public List<Choice> currentChoices
		{
			get
			{
				List<Choice> list = new List<Choice>();
				foreach (Choice choice in this._state.currentChoices)
				{
					if (!choice.isInvisibleDefault)
					{
						choice.index = list.Count;
						list.Add(choice);
					}
				}
				return list;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		public string currentText
		{
			get
			{
				this.IfAsyncWeCant("call currentText since it's a work in progress");
				return this.state.currentText;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000D9E4 File Offset: 0x0000BBE4
		public List<string> currentTags
		{
			get
			{
				this.IfAsyncWeCant("call currentTags since it's a work in progress");
				return this.state.currentTags;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000D9FC File Offset: 0x0000BBFC
		public List<string> currentErrors
		{
			get
			{
				return this.state.currentErrors;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000DA09 File Offset: 0x0000BC09
		public List<string> currentWarnings
		{
			get
			{
				return this.state.currentWarnings;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000DA16 File Offset: 0x0000BC16
		public string currentFlowName
		{
			get
			{
				return this.state.currentFlowName;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000DA23 File Offset: 0x0000BC23
		public bool hasError
		{
			get
			{
				return this.state.hasError;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000DA30 File Offset: 0x0000BC30
		public bool hasWarning
		{
			get
			{
				return this.state.hasWarning;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000DA3D File Offset: 0x0000BC3D
		public VariablesState variablesState
		{
			get
			{
				return this.state.variablesState;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000DA4A File Offset: 0x0000BC4A
		public ListDefinitionsOrigin listDefinitions
		{
			get
			{
				return this._listDefinitions;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000DA52 File Offset: 0x0000BC52
		public StoryState state
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600025C RID: 604 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		// (remove) Token: 0x0600025D RID: 605 RVA: 0x0000DA94 File Offset: 0x0000BC94
		public event ErrorHandler onError;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600025E RID: 606 RVA: 0x0000DACC File Offset: 0x0000BCCC
		// (remove) Token: 0x0600025F RID: 607 RVA: 0x0000DB04 File Offset: 0x0000BD04
		public event Action onDidContinue;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000260 RID: 608 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		// (remove) Token: 0x06000261 RID: 609 RVA: 0x0000DB74 File Offset: 0x0000BD74
		public event Action<Choice> onMakeChoice;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000262 RID: 610 RVA: 0x0000DBAC File Offset: 0x0000BDAC
		// (remove) Token: 0x06000263 RID: 611 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
		public event Action<string, object[]> onEvaluateFunction;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000264 RID: 612 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		// (remove) Token: 0x06000265 RID: 613 RVA: 0x0000DC54 File Offset: 0x0000BE54
		public event Action<string, object[], string, object> onCompleteEvaluateFunction;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000266 RID: 614 RVA: 0x0000DC8C File Offset: 0x0000BE8C
		// (remove) Token: 0x06000267 RID: 615 RVA: 0x0000DCC4 File Offset: 0x0000BEC4
		public event Action<string, object[]> onChoosePathString;

		// Token: 0x06000268 RID: 616 RVA: 0x0000DCF9 File Offset: 0x0000BEF9
		public Profiler StartProfiling()
		{
			this.IfAsyncWeCant("start profiling");
			this._profiler = new Profiler();
			return this._profiler;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000DD17 File Offset: 0x0000BF17
		public void EndProfiling()
		{
			this._profiler = null;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000DD20 File Offset: 0x0000BF20
		public Story(Container contentContainer, List<ListDefinition> lists = null)
		{
			this._mainContentContainer = contentContainer;
			if (lists != null)
			{
				this._listDefinitions = new ListDefinitionsOrigin(lists);
			}
			this._externals = new Dictionary<string, Story.ExternalFunctionDef>();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000DD54 File Offset: 0x0000BF54
		public Story(string jsonString)
			: this(null, null)
		{
			Dictionary<string, object> dictionary = SimpleJson.TextToDictionary(jsonString);
			object obj = dictionary["inkVersion"];
			if (obj == null)
			{
				throw new Exception("ink version number not found. Are you sure it's a valid .ink.json file?");
			}
			int num = (int)obj;
			if (num > 20)
			{
				throw new Exception("Version of ink used to build story was newer than the current version of the engine");
			}
			if (num < 18)
			{
				throw new Exception("Version of ink used to build story is too old to be loaded by this version of the engine");
			}
			object obj2 = dictionary["root"];
			if (obj2 == null)
			{
				throw new Exception("Root node for ink not found. Are you sure it's a valid .ink.json file?");
			}
			object obj3;
			if (dictionary.TryGetValue("listDefs", out obj3))
			{
				this._listDefinitions = Json.JTokenToListDefinitions(obj3);
			}
			this._mainContentContainer = Json.JTokenToRuntimeObject(obj2) as Container;
			this.ResetState();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000DE00 File Offset: 0x0000C000
		public string ToJson()
		{
			SimpleJson.Writer writer = new SimpleJson.Writer();
			this.ToJson(writer);
			return writer.ToString();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000DE20 File Offset: 0x0000C020
		public void ToJson(Stream stream)
		{
			SimpleJson.Writer writer = new SimpleJson.Writer(stream);
			this.ToJson(writer);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000DE3C File Offset: 0x0000C03C
		private void ToJson(SimpleJson.Writer writer)
		{
			writer.WriteObjectStart();
			writer.WriteProperty("inkVersion", 20);
			writer.WriteProperty("root", delegate(SimpleJson.Writer w)
			{
				Json.WriteRuntimeContainer(w, this._mainContentContainer, false);
			});
			if (this._listDefinitions != null)
			{
				writer.WritePropertyStart("listDefs");
				writer.WriteObjectStart();
				foreach (ListDefinition listDefinition in this._listDefinitions.lists)
				{
					writer.WritePropertyStart(listDefinition.name);
					writer.WriteObjectStart();
					foreach (KeyValuePair<InkListItem, int> keyValuePair in listDefinition.items)
					{
						InkListItem key = keyValuePair.Key;
						int value = keyValuePair.Value;
						writer.WriteProperty(key.itemName, value);
					}
					writer.WriteObjectEnd();
					writer.WritePropertyEnd();
				}
				writer.WriteObjectEnd();
				writer.WritePropertyEnd();
			}
			writer.WriteObjectEnd();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000DF60 File Offset: 0x0000C160
		public void ResetState()
		{
			this.IfAsyncWeCant("ResetState");
			this._state = new StoryState(this);
			this._state.variablesState.variableChangedEvent += this.VariableStateDidChangeEvent;
			this.ResetGlobals();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000DF9B File Offset: 0x0000C19B
		public void ResetErrors()
		{
			this._state.ResetErrors();
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		public void ResetCallstack()
		{
			this.IfAsyncWeCant("ResetCallstack");
			this._state.ForceEnd();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		private void ResetGlobals()
		{
			if (this._mainContentContainer.namedContent.ContainsKey("global decl"))
			{
				Pointer currentPointer = this.state.currentPointer;
				this.ChoosePath(new Path("global decl"), false);
				this.ContinueInternal(0f);
				this.state.currentPointer = currentPointer;
			}
			this.state.variablesState.SnapshotDefaultGlobals();
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E028 File Offset: 0x0000C228
		public void SwitchFlow(string flowName)
		{
			this.IfAsyncWeCant("switch flow");
			if (this._asyncSaving)
			{
				throw new Exception("Story is already in background saving mode, can't switch flow to " + flowName);
			}
			this.state.SwitchFlow_Internal(flowName);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000E05A File Offset: 0x0000C25A
		public void RemoveFlow(string flowName)
		{
			this.state.RemoveFlow_Internal(flowName);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000E068 File Offset: 0x0000C268
		public void SwitchToDefaultFlow()
		{
			this.state.SwitchToDefaultFlow_Internal();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E075 File Offset: 0x0000C275
		public string Continue()
		{
			this.ContinueAsync(0f);
			return this.currentText;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000E088 File Offset: 0x0000C288
		public bool canContinue
		{
			get
			{
				return this.state.canContinue;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000E095 File Offset: 0x0000C295
		public bool asyncContinueComplete
		{
			get
			{
				return !this._asyncContinueActive;
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
		public void ContinueAsync(float millisecsLimitAsync)
		{
			if (!this._hasValidatedExternals)
			{
				this.ValidateExternalBindings();
			}
			this.ContinueInternal(millisecsLimitAsync);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
		private void ContinueInternal(float millisecsLimitAsync = 0f)
		{
			if (this._profiler != null)
			{
				this._profiler.PreContinue();
			}
			bool flag = millisecsLimitAsync > 0f;
			this._recursiveContinueCount++;
			if (!this._asyncContinueActive)
			{
				this._asyncContinueActive = flag;
				if (!this.canContinue)
				{
					throw new Exception("Can't continue - should check canContinue before calling Continue");
				}
				this._state.didSafeExit = false;
				this._state.ResetOutput(null);
				if (this._recursiveContinueCount == 1)
				{
					this._state.variablesState.StartVariableObservation();
				}
			}
			else if (this._asyncContinueActive && !flag)
			{
				this._asyncContinueActive = false;
			}
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			bool flag2 = false;
			this._sawLookaheadUnsafeFunctionAfterNewline = false;
			do
			{
				try
				{
					flag2 = this.ContinueSingleStep();
				}
				catch (StoryException ex)
				{
					this.AddError(ex.Message, false, ex.useEndLineNumber);
					break;
				}
			}
			while (!flag2 && (!this._asyncContinueActive || (float)stopwatch.ElapsedMilliseconds <= millisecsLimitAsync) && this.canContinue);
			stopwatch.Stop();
			Dictionary<string, Object> dictionary = null;
			if (flag2 || !this.canContinue)
			{
				if (this._stateSnapshotAtLastNewline != null)
				{
					this.RestoreStateSnapshot();
				}
				if (!this.canContinue)
				{
					if (this.state.callStack.canPopThread)
					{
						this.AddError("Thread available to pop, threads should always be flat by the end of evaluation?", false, false);
					}
					if (this.state.generatedChoices.Count == 0 && !this.state.didSafeExit && this._temporaryEvaluationContainer == null)
					{
						if (this.state.callStack.CanPop(new PushPopType?(PushPopType.Tunnel)))
						{
							this.AddError("unexpectedly reached end of content. Do you need a '->->' to return from a tunnel?", false, false);
						}
						else if (this.state.callStack.CanPop(new PushPopType?(PushPopType.Function)))
						{
							this.AddError("unexpectedly reached end of content. Do you need a '~ return'?", false, false);
						}
						else if (!this.state.callStack.canPop)
						{
							this.AddError("ran out of content. Do you need a '-> DONE' or '-> END'?", false, false);
						}
						else
						{
							this.AddError("unexpectedly reached end of content for unknown reason. Please debug compiler!", false, false);
						}
					}
				}
				this.state.didSafeExit = false;
				this._sawLookaheadUnsafeFunctionAfterNewline = false;
				if (this._recursiveContinueCount == 1)
				{
					dictionary = this._state.variablesState.CompleteVariableObservation();
				}
				this._asyncContinueActive = false;
				if (this.onDidContinue != null)
				{
					this.onDidContinue();
				}
			}
			this._recursiveContinueCount--;
			if (this._profiler != null)
			{
				this._profiler.PostContinue();
			}
			if (this.state.hasError || this.state.hasWarning)
			{
				if (this.onError == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("Ink had ");
					if (this.state.hasError)
					{
						stringBuilder.Append(this.state.currentErrors.Count);
						stringBuilder.Append((this.state.currentErrors.Count == 1) ? " error" : " errors");
						if (this.state.hasWarning)
						{
							stringBuilder.Append(" and ");
						}
					}
					if (this.state.hasWarning)
					{
						stringBuilder.Append(this.state.currentWarnings.Count);
						stringBuilder.Append((this.state.currentWarnings.Count == 1) ? " warning" : " warnings");
					}
					stringBuilder.Append(". It is strongly suggested that you assign an error handler to story.onError. The first issue was: ");
					stringBuilder.Append(this.state.hasError ? this.state.currentErrors[0] : this.state.currentWarnings[0]);
					throw new StoryException(stringBuilder.ToString());
				}
				if (this.state.hasError)
				{
					foreach (string text in this.state.currentErrors)
					{
						this.onError(text, ErrorType.Error);
					}
				}
				if (this.state.hasWarning)
				{
					foreach (string text2 in this.state.currentWarnings)
					{
						this.onError(text2, ErrorType.Warning);
					}
				}
				this.ResetErrors();
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				foreach (KeyValuePair<string, Object> keyValuePair in dictionary)
				{
					this._state.variablesState.NotifyObservers(dictionary);
				}
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000E57C File Offset: 0x0000C77C
		private bool ContinueSingleStep()
		{
			if (this._profiler != null)
			{
				this._profiler.PreStep();
			}
			this.Step();
			if (this._profiler != null)
			{
				this._profiler.PostStep();
			}
			if (!this.canContinue && !this.state.callStack.elementIsEvaluateFromGame)
			{
				this.TryFollowDefaultInvisibleChoice();
			}
			if (this._profiler != null)
			{
				this._profiler.PreSnapshot();
			}
			if (!this.state.inStringEvaluation)
			{
				if (this._stateSnapshotAtLastNewline != null)
				{
					Story.OutputStateChange outputStateChange = this.CalculateNewlineOutputStateChange(this._stateSnapshotAtLastNewline.currentText, this.state.currentText, this._stateSnapshotAtLastNewline.currentTags.Count, this.state.currentTags.Count);
					if (outputStateChange == Story.OutputStateChange.ExtendedBeyondNewline || this._sawLookaheadUnsafeFunctionAfterNewline)
					{
						this.RestoreStateSnapshot();
						return true;
					}
					if (outputStateChange == Story.OutputStateChange.NewlineRemoved)
					{
						this.DiscardSnapshot();
					}
				}
				if (this.state.outputStreamEndsInNewline)
				{
					if (this.canContinue)
					{
						if (this._stateSnapshotAtLastNewline == null)
						{
							this.StateSnapshot();
						}
					}
					else
					{
						this.DiscardSnapshot();
					}
				}
			}
			if (this._profiler != null)
			{
				this._profiler.PostSnapshot();
			}
			return false;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000E69C File Offset: 0x0000C89C
		private Story.OutputStateChange CalculateNewlineOutputStateChange(string prevText, string currText, int prevTagCount, int currTagCount)
		{
			bool flag = currText.Length >= prevText.Length && currText[prevText.Length - 1] == '\n';
			if (prevTagCount == currTagCount && prevText.Length == currText.Length && flag)
			{
				return Story.OutputStateChange.NoChange;
			}
			if (!flag)
			{
				return Story.OutputStateChange.NewlineRemoved;
			}
			if (currTagCount > prevTagCount)
			{
				return Story.OutputStateChange.ExtendedBeyondNewline;
			}
			for (int i = prevText.Length; i < currText.Length; i++)
			{
				char c = currText[i];
				if (c != ' ' && c != '\t')
				{
					return Story.OutputStateChange.ExtendedBeyondNewline;
				}
			}
			return Story.OutputStateChange.NoChange;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000E720 File Offset: 0x0000C920
		public string ContinueMaximally()
		{
			this.IfAsyncWeCant("ContinueMaximally");
			StringBuilder stringBuilder = new StringBuilder();
			while (this.canContinue)
			{
				stringBuilder.Append(this.Continue());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000E75B File Offset: 0x0000C95B
		public SearchResult ContentAtPath(Path path)
		{
			return this.mainContentContainer.ContentAtPath(path, 0, -1);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E76C File Offset: 0x0000C96C
		public Container KnotContainerWithName(string name)
		{
			INamedContent namedContent;
			if (this.mainContentContainer.namedContent.TryGetValue(name, out namedContent))
			{
				return namedContent as Container;
			}
			return null;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000E798 File Offset: 0x0000C998
		public Pointer PointerAtPath(Path path)
		{
			if (path.length == 0)
			{
				return Pointer.Null;
			}
			Pointer pointer = default(Pointer);
			int num = path.length;
			SearchResult searchResult;
			if (path.lastComponent.isIndex)
			{
				num = path.length - 1;
				searchResult = this.mainContentContainer.ContentAtPath(path, 0, num);
				pointer.container = searchResult.container;
				pointer.index = path.lastComponent.index;
			}
			else
			{
				searchResult = this.mainContentContainer.ContentAtPath(path, 0, -1);
				pointer.container = searchResult.container;
				pointer.index = -1;
			}
			if (searchResult.obj == null || (searchResult.obj == this.mainContentContainer && num > 0))
			{
				this.Error("Failed to find content at path '" + ((path != null) ? path.ToString() : null) + "', and no approximation of it was possible.", false);
			}
			else if (searchResult.approximate)
			{
				string[] array = new string[5];
				array[0] = "Failed to find content at path '";
				array[1] = ((path != null) ? path.ToString() : null);
				array[2] = "', so it was approximated to: '";
				int num2 = 3;
				Path path2 = searchResult.obj.path;
				array[num2] = ((path2 != null) ? path2.ToString() : null);
				array[4] = "'.";
				this.Warning(string.Concat(array));
			}
			return pointer;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		private void StateSnapshot()
		{
			this._stateSnapshotAtLastNewline = this._state;
			this._state = this._state.CopyAndStartPatching(false);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		private void RestoreStateSnapshot()
		{
			this._stateSnapshotAtLastNewline.RestoreAfterPatch();
			this._state = this._stateSnapshotAtLastNewline;
			this._stateSnapshotAtLastNewline = null;
			if (!this._asyncSaving)
			{
				this._state.ApplyAnyPatch();
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000E927 File Offset: 0x0000CB27
		private void DiscardSnapshot()
		{
			if (!this._asyncSaving)
			{
				this._state.ApplyAnyPatch();
			}
			this._stateSnapshotAtLastNewline = null;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E943 File Offset: 0x0000CB43
		public StoryState CopyStateForBackgroundThreadSave()
		{
			this.IfAsyncWeCant("start saving on a background thread");
			if (this._asyncSaving)
			{
				throw new Exception("Story is already in background saving mode, can't call CopyStateForBackgroundThreadSave again!");
			}
			StoryState state = this._state;
			this._state = this._state.CopyAndStartPatching(true);
			this._asyncSaving = true;
			return state;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E982 File Offset: 0x0000CB82
		public void BackgroundSaveComplete()
		{
			if (this._stateSnapshotAtLastNewline == null)
			{
				this._state.ApplyAnyPatch();
			}
			this._asyncSaving = false;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E9A0 File Offset: 0x0000CBA0
		private void Step()
		{
			bool flag = true;
			Pointer pointer = this.state.currentPointer;
			if (pointer.isNull)
			{
				return;
			}
			Container container = pointer.Resolve() as Container;
			while (container)
			{
				this.VisitContainer(container, true);
				if (container.content.Count == 0)
				{
					break;
				}
				pointer = Pointer.StartOf(container);
				container = pointer.Resolve() as Container;
			}
			this.state.currentPointer = pointer;
			if (this._profiler != null)
			{
				this._profiler.Step(this.state.callStack);
			}
			Object @object = pointer.Resolve();
			bool flag2 = this.PerformLogicAndFlowControl(@object);
			if (this.state.currentPointer.isNull)
			{
				return;
			}
			if (flag2)
			{
				flag = false;
			}
			ChoicePoint choicePoint = @object as ChoicePoint;
			if (choicePoint)
			{
				Choice choice = this.ProcessChoice(choicePoint);
				if (choice)
				{
					this.state.generatedChoices.Add(choice);
				}
				@object = null;
				flag = false;
			}
			if (@object is Container)
			{
				flag = false;
			}
			if (flag)
			{
				VariablePointerValue variablePointerValue = @object as VariablePointerValue;
				if (variablePointerValue && variablePointerValue.contextIndex == -1)
				{
					int num = this.state.callStack.ContextForVariableNamed(variablePointerValue.variableName);
					@object = new VariablePointerValue(variablePointerValue.variableName, num);
				}
				if (this.state.inExpressionEvaluation)
				{
					this.state.PushEvaluationStack(@object);
				}
				else
				{
					this.state.PushToOutputStream(@object);
				}
			}
			this.NextContent();
			ControlCommand controlCommand = @object as ControlCommand;
			if (controlCommand && controlCommand.commandType == ControlCommand.CommandType.StartThread)
			{
				this.state.callStack.PushThread();
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000EB40 File Offset: 0x0000CD40
		private void VisitContainer(Container container, bool atStart)
		{
			if (!container.countingAtStartOnly || atStart)
			{
				if (container.visitsShouldBeCounted)
				{
					this.state.IncrementVisitCountForContainer(container);
				}
				if (container.turnIndexShouldBeCounted)
				{
					this.state.RecordTurnIndexVisitToContainer(container);
				}
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000EB78 File Offset: 0x0000CD78
		private void VisitChangedContainersDueToDivert()
		{
			Pointer previousPointer = this.state.previousPointer;
			Pointer currentPointer = this.state.currentPointer;
			if (currentPointer.isNull || currentPointer.index == -1)
			{
				return;
			}
			this._prevContainers.Clear();
			if (!previousPointer.isNull)
			{
				Container container = (previousPointer.Resolve() as Container) ?? previousPointer.container;
				while (container)
				{
					this._prevContainers.Add(container);
					container = container.parent as Container;
				}
			}
			Object @object = currentPointer.Resolve();
			if (@object == null)
			{
				return;
			}
			Container container2 = @object.parent as Container;
			bool flag = true;
			while (container2 && (!this._prevContainers.Contains(container2) || container2.countingAtStartOnly))
			{
				bool flag2 = container2.content.Count > 0 && @object == container2.content[0] && flag;
				if (!flag2)
				{
					flag = false;
				}
				this.VisitContainer(container2, flag2);
				@object = container2;
				container2 = container2.parent as Container;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000EC8C File Offset: 0x0000CE8C
		private Choice ProcessChoice(ChoicePoint choicePoint)
		{
			bool flag = true;
			if (choicePoint.hasCondition)
			{
				Object @object = this.state.PopEvaluationStack();
				if (!this.IsTruthy(@object))
				{
					flag = false;
				}
			}
			string text = "";
			string text2 = "";
			if (choicePoint.hasChoiceOnlyContent)
			{
				text2 = (this.state.PopEvaluationStack() as StringValue).value;
			}
			if (choicePoint.hasStartContent)
			{
				text = (this.state.PopEvaluationStack() as StringValue).value;
			}
			if (choicePoint.onceOnly && this.state.VisitCountForContainer(choicePoint.choiceTarget) > 0)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return new Choice
			{
				targetPath = choicePoint.pathOnChoice,
				sourcePath = choicePoint.path.ToString(),
				isInvisibleDefault = choicePoint.isInvisibleDefault,
				threadAtGeneration = this.state.callStack.ForkThread(),
				text = (text + text2).Trim(new char[] { ' ', '\t' })
			};
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000ED90 File Offset: 0x0000CF90
		private bool IsTruthy(Object obj)
		{
			bool flag = false;
			if (!(obj is Value))
			{
				return flag;
			}
			Value value = (Value)obj;
			if (value is DivertTargetValue)
			{
				DivertTargetValue divertTargetValue = (DivertTargetValue)value;
				string text = "Shouldn't use a divert target (to ";
				Path targetPath = divertTargetValue.targetPath;
				this.Error(text + ((targetPath != null) ? targetPath.ToString() : null) + ") as a conditional value. Did you intend a function call 'likeThis()' or a read count check 'likeThis'? (no arrows)", false);
				return false;
			}
			return value.isTruthy;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
		private bool PerformLogicAndFlowControl(Object contentObj)
		{
			if (contentObj == null)
			{
				return false;
			}
			if (contentObj is Divert)
			{
				Divert divert = (Divert)contentObj;
				if (divert.isConditional)
				{
					Object @object = this.state.PopEvaluationStack();
					if (!this.IsTruthy(@object))
					{
						return true;
					}
				}
				if (divert.hasVariableTarget)
				{
					string variableDivertName = divert.variableDivertName;
					Object variableWithName = this.state.variablesState.GetVariableWithName(variableDivertName);
					if (variableWithName == null)
					{
						this.Error("Tried to divert using a target from a variable that could not be found (" + variableDivertName + ")", false);
					}
					else if (!(variableWithName is DivertTargetValue))
					{
						IntValue intValue = variableWithName as IntValue;
						string text = "Tried to divert to a target from a variable, but the variable (" + variableDivertName + ") didn't contain a divert target, it ";
						if (intValue && intValue.value == 0)
						{
							text += "was empty/null (the value 0).";
						}
						else
						{
							string text2 = text;
							string text3 = "contained '";
							Object object2 = variableWithName;
							text = text2 + text3 + ((object2 != null) ? object2.ToString() : null) + "'.";
						}
						this.Error(text, false);
					}
					DivertTargetValue divertTargetValue = (DivertTargetValue)variableWithName;
					this.state.divertedPointer = this.PointerAtPath(divertTargetValue.targetPath);
				}
				else
				{
					if (divert.isExternal)
					{
						this.CallExternalFunction(divert.targetPathString, divert.externalArgs);
						return true;
					}
					this.state.divertedPointer = divert.targetPointer;
				}
				if (divert.pushesToStack)
				{
					this.state.callStack.Push(divert.stackPushType, 0, this.state.outputStream.Count);
				}
				if (this.state.divertedPointer.isNull && !divert.isExternal)
				{
					if (divert && divert.debugMetadata.sourceName != null)
					{
						this.Error("Divert target doesn't exist: " + divert.debugMetadata.sourceName, false);
					}
					else
					{
						string text4 = "Divert resolution failed: ";
						Divert divert2 = divert;
						this.Error(text4 + ((divert2 != null) ? divert2.ToString() : null), false);
					}
				}
				return true;
			}
			if (contentObj is ControlCommand)
			{
				ControlCommand controlCommand = (ControlCommand)contentObj;
				switch (controlCommand.commandType)
				{
				case ControlCommand.CommandType.EvalStart:
					this.Assert(!this.state.inExpressionEvaluation, "Already in expression evaluation?", Array.Empty<object>());
					this.state.inExpressionEvaluation = true;
					break;
				case ControlCommand.CommandType.EvalOutput:
					if (this.state.evaluationStack.Count > 0)
					{
						Object object3 = this.state.PopEvaluationStack();
						if (!(object3 is Void))
						{
							StringValue stringValue = new StringValue(object3.ToString());
							this.state.PushToOutputStream(stringValue);
						}
					}
					break;
				case ControlCommand.CommandType.EvalEnd:
					this.Assert(this.state.inExpressionEvaluation, "Not in expression evaluation mode", Array.Empty<object>());
					this.state.inExpressionEvaluation = false;
					break;
				case ControlCommand.CommandType.Duplicate:
					this.state.PushEvaluationStack(this.state.PeekEvaluationStack());
					break;
				case ControlCommand.CommandType.PopEvaluatedValue:
					this.state.PopEvaluationStack();
					break;
				case ControlCommand.CommandType.PopFunction:
				case ControlCommand.CommandType.PopTunnel:
				{
					PushPopType pushPopType = ((controlCommand.commandType == ControlCommand.CommandType.PopFunction) ? PushPopType.Function : PushPopType.Tunnel);
					DivertTargetValue divertTargetValue2 = null;
					if (pushPopType == PushPopType.Tunnel)
					{
						Object object4 = this.state.PopEvaluationStack();
						divertTargetValue2 = object4 as DivertTargetValue;
						if (divertTargetValue2 == null)
						{
							this.Assert(object4 is Void, "Expected void if ->-> doesn't override target", Array.Empty<object>());
						}
					}
					if (!this.state.TryExitFunctionEvaluationFromGame())
					{
						if (this.state.callStack.currentElement.type != pushPopType || !this.state.callStack.canPop)
						{
							Dictionary<PushPopType, string> dictionary = new Dictionary<PushPopType, string>();
							dictionary[PushPopType.Function] = "function return statement (~ return)";
							dictionary[PushPopType.Tunnel] = "tunnel onwards statement (->->)";
							string text5 = dictionary[this.state.callStack.currentElement.type];
							if (!this.state.callStack.canPop)
							{
								text5 = "end of flow (-> END or choice)";
							}
							string text6 = string.Format("Found {0}, when expected {1}", dictionary[pushPopType], text5);
							this.Error(text6, false);
						}
						else
						{
							this.state.PopCallstack(null);
							if (divertTargetValue2)
							{
								this.state.divertedPointer = this.PointerAtPath(divertTargetValue2.targetPath);
							}
						}
					}
					break;
				}
				case ControlCommand.CommandType.BeginString:
					this.state.PushToOutputStream(controlCommand);
					this.Assert(this.state.inExpressionEvaluation, "Expected to be in an expression when evaluating a string", Array.Empty<object>());
					this.state.inExpressionEvaluation = false;
					break;
				case ControlCommand.CommandType.EndString:
				{
					Stack<Object> stack = new Stack<Object>();
					int num = 0;
					for (int i = this.state.outputStream.Count - 1; i >= 0; i--)
					{
						Object object5 = this.state.outputStream[i];
						num++;
						ControlCommand controlCommand2 = object5 as ControlCommand;
						if (controlCommand2 != null && controlCommand2.commandType == ControlCommand.CommandType.BeginString)
						{
							break;
						}
						if (object5 is StringValue)
						{
							stack.Push(object5);
						}
					}
					this.state.PopFromOutputStream(num);
					StringBuilder stringBuilder = new StringBuilder();
					foreach (Object object6 in stack)
					{
						stringBuilder.Append(object6.ToString());
					}
					this.state.inExpressionEvaluation = true;
					this.state.PushEvaluationStack(new StringValue(stringBuilder.ToString()));
					break;
				}
				case ControlCommand.CommandType.NoOp:
				case ControlCommand.CommandType.StartThread:
					break;
				case ControlCommand.CommandType.ChoiceCount:
				{
					int count = this.state.generatedChoices.Count;
					this.state.PushEvaluationStack(new IntValue(count));
					break;
				}
				case ControlCommand.CommandType.Turns:
					this.state.PushEvaluationStack(new IntValue(this.state.currentTurnIndex + 1));
					break;
				case ControlCommand.CommandType.TurnsSince:
				case ControlCommand.CommandType.ReadCount:
				{
					Object object7 = this.state.PopEvaluationStack();
					if (!(object7 is DivertTargetValue))
					{
						string text7 = "";
						if (object7 is IntValue)
						{
							text7 = ". Did you accidentally pass a read count ('knot_name') instead of a target ('-> knot_name')?";
						}
						string text8 = "TURNS_SINCE expected a divert target (knot, stitch, label name), but saw ";
						Object object8 = object7;
						this.Error(text8 + ((object8 != null) ? object8.ToString() : null) + text7, false);
					}
					else
					{
						DivertTargetValue divertTargetValue3 = object7 as DivertTargetValue;
						Container container = this.ContentAtPath(divertTargetValue3.targetPath).correctObj as Container;
						int num2;
						if (container != null)
						{
							if (controlCommand.commandType == ControlCommand.CommandType.TurnsSince)
							{
								num2 = this.state.TurnsSinceForContainer(container);
							}
							else
							{
								num2 = this.state.VisitCountForContainer(container);
							}
						}
						else
						{
							if (controlCommand.commandType == ControlCommand.CommandType.TurnsSince)
							{
								num2 = -1;
							}
							else
							{
								num2 = 0;
							}
							this.Warning("Failed to find container for " + controlCommand.ToString() + " lookup at " + divertTargetValue3.targetPath.ToString());
						}
						this.state.PushEvaluationStack(new IntValue(num2));
					}
					break;
				}
				case ControlCommand.CommandType.Random:
				{
					IntValue intValue2 = this.state.PopEvaluationStack() as IntValue;
					IntValue intValue3 = this.state.PopEvaluationStack() as IntValue;
					if (intValue3 == null)
					{
						this.Error("Invalid value for minimum parameter of RANDOM(min, max)", false);
					}
					if (intValue2 == null)
					{
						this.Error("Invalid value for maximum parameter of RANDOM(min, max)", false);
					}
					int num3;
					try
					{
						num3 = checked(intValue2.value - intValue3.value + 1);
					}
					catch (OverflowException)
					{
						num3 = int.MaxValue;
						this.Error("RANDOM was called with a range that exceeds the size that ink numbers can use.", false);
					}
					if (num3 <= 0)
					{
						this.Error(string.Concat(new string[]
						{
							"RANDOM was called with minimum as ",
							intValue3.value.ToString(),
							" and maximum as ",
							intValue2.value.ToString(),
							". The maximum must be larger"
						}), false);
					}
					int num4 = new Random(this.state.storySeed + this.state.previousRandom).Next();
					int num5 = num4 % num3 + intValue3.value;
					this.state.PushEvaluationStack(new IntValue(num5));
					this.state.previousRandom = num4;
					break;
				}
				case ControlCommand.CommandType.SeedRandom:
				{
					IntValue intValue4 = this.state.PopEvaluationStack() as IntValue;
					if (intValue4 == null)
					{
						this.Error("Invalid value passed to SEED_RANDOM", false);
					}
					this.state.storySeed = intValue4.value;
					this.state.previousRandom = 0;
					this.state.PushEvaluationStack(new Void());
					break;
				}
				case ControlCommand.CommandType.VisitIndex:
				{
					int num6 = this.state.VisitCountForContainer(this.state.currentPointer.container) - 1;
					this.state.PushEvaluationStack(new IntValue(num6));
					break;
				}
				case ControlCommand.CommandType.SequenceShuffleIndex:
				{
					int num7 = this.NextSequenceShuffleIndex();
					this.state.PushEvaluationStack(new IntValue(num7));
					break;
				}
				case ControlCommand.CommandType.Done:
					if (this.state.callStack.canPopThread)
					{
						this.state.callStack.PopThread();
					}
					else
					{
						this.state.didSafeExit = true;
						this.state.currentPointer = Pointer.Null;
					}
					break;
				case ControlCommand.CommandType.End:
					this.state.ForceEnd();
					break;
				case ControlCommand.CommandType.ListFromInt:
				{
					IntValue intValue5 = this.state.PopEvaluationStack() as IntValue;
					StringValue stringValue2 = this.state.PopEvaluationStack() as StringValue;
					if (intValue5 == null)
					{
						throw new StoryException("Passed non-integer when creating a list element from a numerical value.");
					}
					ListValue listValue = null;
					ListDefinition listDefinition;
					if (!this.listDefinitions.TryListGetDefinition(stringValue2.value, out listDefinition))
					{
						throw new StoryException("Failed to find LIST called " + stringValue2.value);
					}
					InkListItem inkListItem;
					if (listDefinition.TryGetItemWithValue(intValue5.value, out inkListItem))
					{
						listValue = new ListValue(inkListItem, intValue5.value);
					}
					if (listValue == null)
					{
						listValue = new ListValue();
					}
					this.state.PushEvaluationStack(listValue);
					break;
				}
				case ControlCommand.CommandType.ListRange:
				{
					Value value = this.state.PopEvaluationStack() as Value;
					Value value2 = this.state.PopEvaluationStack() as Value;
					ListValue listValue2 = this.state.PopEvaluationStack() as ListValue;
					if (listValue2 == null || value2 == null || value == null)
					{
						throw new StoryException("Expected list, minimum and maximum for LIST_RANGE");
					}
					InkList inkList = listValue2.value.ListWithSubRange(value2.valueObject, value.valueObject);
					this.state.PushEvaluationStack(new ListValue(inkList));
					break;
				}
				case ControlCommand.CommandType.ListRandom:
				{
					ListValue listValue3 = this.state.PopEvaluationStack() as ListValue;
					if (listValue3 == null)
					{
						throw new StoryException("Expected list for LIST_RANDOM");
					}
					InkList value3 = listValue3.value;
					InkList inkList2;
					if (value3.Count == 0)
					{
						inkList2 = new InkList();
					}
					else
					{
						int num8 = new Random(this.state.storySeed + this.state.previousRandom).Next();
						int num9 = num8 % value3.Count;
						Dictionary<InkListItem, int>.Enumerator enumerator2 = value3.GetEnumerator();
						for (int j = 0; j <= num9; j++)
						{
							enumerator2.MoveNext();
						}
						KeyValuePair<InkListItem, int> keyValuePair = enumerator2.Current;
						inkList2 = new InkList(keyValuePair.Key.originName, this);
						inkList2.Add(keyValuePair.Key, keyValuePair.Value);
						this.state.previousRandom = num8;
					}
					this.state.PushEvaluationStack(new ListValue(inkList2));
					break;
				}
				default:
				{
					string text9 = "unhandled ControlCommand: ";
					ControlCommand controlCommand3 = controlCommand;
					this.Error(text9 + ((controlCommand3 != null) ? controlCommand3.ToString() : null), false);
					break;
				}
				}
				return true;
			}
			if (contentObj is VariableAssignment)
			{
				VariableAssignment variableAssignment = (VariableAssignment)contentObj;
				Object object9 = this.state.PopEvaluationStack();
				this.state.variablesState.Assign(variableAssignment, object9);
				return true;
			}
			if (contentObj is VariableReference)
			{
				VariableReference variableReference = (VariableReference)contentObj;
				Object object10;
				if (variableReference.pathForCount != null)
				{
					Container containerForCount = variableReference.containerForCount;
					object10 = new IntValue(this.state.VisitCountForContainer(containerForCount));
				}
				else
				{
					object10 = this.state.variablesState.GetVariableWithName(variableReference.name);
					if (object10 == null)
					{
						this.Warning("Variable not found: '" + variableReference.name + "'. Using default value of 0 (false). This can happen with temporary variables if the declaration hasn't yet been hit. Globals are always given a default value on load if a value doesn't exist in the save state.");
						object10 = new IntValue(0);
					}
				}
				this.state.PushEvaluationStack(object10);
				return true;
			}
			if (contentObj is NativeFunctionCall)
			{
				NativeFunctionCall nativeFunctionCall = (NativeFunctionCall)contentObj;
				List<Object> list = this.state.PopEvaluationStack(nativeFunctionCall.numberOfParameters);
				Object object11 = nativeFunctionCall.Call(list);
				this.state.PushEvaluationStack(object11);
				return true;
			}
			return false;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public void ChoosePathString(string path, bool resetCallstack = true, params object[] arguments)
		{
			this.IfAsyncWeCant("call ChoosePathString right now");
			if (this.onChoosePathString != null)
			{
				this.onChoosePathString(path, arguments);
			}
			if (resetCallstack)
			{
				this.ResetCallstack();
			}
			else if (this.state.callStack.currentElement.type == PushPopType.Function)
			{
				string text = "";
				Container container = this.state.callStack.currentElement.currentPointer.container;
				if (container != null)
				{
					text = "(" + container.path.ToString() + ") ";
				}
				throw new Exception(string.Concat(new string[]
				{
					"Story was running a function ",
					text,
					"when you called ChoosePathString(",
					path,
					") - this is almost certainly not not what you want! Full stack trace: \n",
					this.state.callStack.callStackTrace
				}));
			}
			this.state.PassArgumentsToEvaluationStack(arguments);
			this.ChoosePath(new Path(path), true);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000FB80 File Offset: 0x0000DD80
		private void IfAsyncWeCant(string activityStr)
		{
			if (this._asyncContinueActive)
			{
				throw new Exception("Can't " + activityStr + ". Story is in the middle of a ContinueAsync(). Make more ContinueAsync() calls or a single Continue() call beforehand.");
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000FBA0 File Offset: 0x0000DDA0
		public void ChoosePath(Path p, bool incrementingTurnIndex = true)
		{
			this.state.SetChosenPath(p, incrementingTurnIndex);
			this.VisitChangedContainersDueToDivert();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000FBB8 File Offset: 0x0000DDB8
		public void ChooseChoiceIndex(int choiceIdx)
		{
			List<Choice> currentChoices = this.currentChoices;
			this.Assert(choiceIdx >= 0 && choiceIdx < currentChoices.Count, "choice out of range", Array.Empty<object>());
			Choice choice = currentChoices[choiceIdx];
			if (this.onMakeChoice != null)
			{
				this.onMakeChoice(choice);
			}
			this.state.callStack.currentThread = choice.threadAtGeneration;
			this.ChoosePath(choice.targetPath, true);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		public bool HasFunction(string functionName)
		{
			bool flag;
			try
			{
				flag = this.KnotContainerWithName(functionName) != null;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000FC60 File Offset: 0x0000DE60
		public object EvaluateFunction(string functionName, params object[] arguments)
		{
			string text;
			return this.EvaluateFunction(functionName, out text, arguments);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000FC78 File Offset: 0x0000DE78
		public object EvaluateFunction(string functionName, out string textOutput, params object[] arguments)
		{
			if (this.onEvaluateFunction != null)
			{
				this.onEvaluateFunction(functionName, arguments);
			}
			this.IfAsyncWeCant("evaluate a function");
			if (functionName == null)
			{
				throw new Exception("Function is null");
			}
			if (functionName == string.Empty || functionName.Trim() == string.Empty)
			{
				throw new Exception("Function is empty or white space.");
			}
			Container container = this.KnotContainerWithName(functionName);
			if (container == null)
			{
				throw new Exception("Function doesn't exist: '" + functionName + "'");
			}
			List<Object> list = new List<Object>(this.state.outputStream);
			this._state.ResetOutput(null);
			this.state.StartFunctionEvaluationFromGame(container, arguments);
			StringBuilder stringBuilder = new StringBuilder();
			while (this.canContinue)
			{
				stringBuilder.Append(this.Continue());
			}
			textOutput = stringBuilder.ToString();
			this._state.ResetOutput(list);
			object obj = this.state.CompleteFunctionEvaluationFromGame();
			if (this.onCompleteEvaluateFunction != null)
			{
				this.onCompleteEvaluateFunction(functionName, arguments, textOutput, obj);
			}
			return obj;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000FD84 File Offset: 0x0000DF84
		public Object EvaluateExpression(Container exprContainer)
		{
			int count = this.state.callStack.elements.Count;
			this.state.callStack.Push(PushPopType.Tunnel, 0, 0);
			this._temporaryEvaluationContainer = exprContainer;
			this.state.GoToStart();
			int count2 = this.state.evaluationStack.Count;
			this.Continue();
			this._temporaryEvaluationContainer = null;
			if (this.state.callStack.elements.Count > count)
			{
				this.state.PopCallstack(null);
			}
			if (this.state.evaluationStack.Count > count2)
			{
				return this.state.PopEvaluationStack();
			}
			return null;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000FE37 File Offset: 0x0000E037
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000FE3F File Offset: 0x0000E03F
		public bool allowExternalFunctionFallbacks { get; set; }

		// Token: 0x06000296 RID: 662 RVA: 0x0000FE48 File Offset: 0x0000E048
		public bool TryGetExternalFunction(string functionName, out Story.ExternalFunction externalFunction)
		{
			Story.ExternalFunctionDef externalFunctionDef;
			if (this._externals.TryGetValue(functionName, out externalFunctionDef))
			{
				externalFunction = externalFunctionDef.function;
				return true;
			}
			externalFunction = null;
			return false;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000FE74 File Offset: 0x0000E074
		private void CallExternalFunction(string funcName, int numberOfArguments)
		{
			Story.ExternalFunctionDef externalFunctionDef;
			bool flag = this._externals.TryGetValue(funcName, out externalFunctionDef);
			if (flag && !externalFunctionDef.lookaheadSafe && this._stateSnapshotAtLastNewline != null)
			{
				this._sawLookaheadUnsafeFunctionAfterNewline = true;
				return;
			}
			if (!flag)
			{
				if (this.allowExternalFunctionFallbacks)
				{
					Container container = this.KnotContainerWithName(funcName);
					this.Assert(container != null, "Trying to call EXTERNAL function '" + funcName + "' which has not been bound, and fallback ink function could not be found.", Array.Empty<object>());
					this.state.callStack.Push(PushPopType.Function, 0, this.state.outputStream.Count);
					this.state.divertedPointer = Pointer.StartOf(container);
					return;
				}
				this.Assert(false, "Trying to call EXTERNAL function '" + funcName + "' which has not been bound (and ink fallbacks disabled).", Array.Empty<object>());
			}
			List<object> list = new List<object>();
			for (int i = 0; i < numberOfArguments; i++)
			{
				object valueObject = (this.state.PopEvaluationStack() as Value).valueObject;
				list.Add(valueObject);
			}
			list.Reverse();
			object obj = externalFunctionDef.function(list.ToArray());
			Object @object;
			if (obj != null)
			{
				@object = Value.Create(obj);
				bool flag2 = @object != null;
				string text = "Could not create ink value from returned object of type ";
				Type type = obj.GetType();
				this.Assert(flag2, text + ((type != null) ? type.ToString() : null), Array.Empty<object>());
			}
			else
			{
				@object = new Void();
			}
			this.state.PushEvaluationStack(@object);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000FFDC File Offset: 0x0000E1DC
		public void BindExternalFunctionGeneral(string funcName, Story.ExternalFunction func, bool lookaheadSafe = true)
		{
			this.IfAsyncWeCant("bind an external function");
			this.Assert(!this._externals.ContainsKey(funcName), "Function '" + funcName + "' has already been bound.", Array.Empty<object>());
			this._externals[funcName] = new Story.ExternalFunctionDef
			{
				function = func,
				lookaheadSafe = lookaheadSafe
			};
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00010044 File Offset: 0x0000E244
		private object TryCoerce<T>(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is T)
			{
				return (T)((object)value);
			}
			if (value is float && typeof(T) == typeof(int))
			{
				return (int)Math.Round((double)((float)value));
			}
			if (value is int && typeof(T) == typeof(float))
			{
				return (float)((int)value);
			}
			if (value is int && typeof(T) == typeof(bool))
			{
				return (int)value != 0;
			}
			if (value is bool && typeof(T) == typeof(int))
			{
				return ((bool)value) ? 1 : 0;
			}
			if (typeof(T) == typeof(string))
			{
				return value.ToString();
			}
			this.Assert(false, "Failed to cast " + value.GetType().Name + " to " + typeof(T).Name, Array.Empty<object>());
			return null;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00010194 File Offset: 0x0000E394
		public void BindExternalFunction(string funcName, Func<object> func, bool lookaheadSafe = false)
		{
			this.Assert(func != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 0, "External function expected no arguments", Array.Empty<object>());
				return func();
			}, lookaheadSafe);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000101E4 File Offset: 0x0000E3E4
		public void BindExternalFunction(string funcName, Action act, bool lookaheadSafe = false)
		{
			this.Assert(act != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 0, "External function expected no arguments", Array.Empty<object>());
				act();
				return null;
			}, lookaheadSafe);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00010234 File Offset: 0x0000E434
		public void BindExternalFunction<T>(string funcName, Func<T, object> func, bool lookaheadSafe = false)
		{
			this.Assert(func != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 1, "External function expected one argument", Array.Empty<object>());
				return func((T)((object)this.TryCoerce<T>(args[0])));
			}, lookaheadSafe);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00010284 File Offset: 0x0000E484
		public void BindExternalFunction<T>(string funcName, Action<T> act, bool lookaheadSafe = false)
		{
			this.Assert(act != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 1, "External function expected one argument", Array.Empty<object>());
				act((T)((object)this.TryCoerce<T>(args[0])));
				return null;
			}, lookaheadSafe);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000102D4 File Offset: 0x0000E4D4
		public void BindExternalFunction<T1, T2>(string funcName, Func<T1, T2, object> func, bool lookaheadSafe = false)
		{
			this.Assert(func != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 2, "External function expected two arguments", Array.Empty<object>());
				return func((T1)((object)this.TryCoerce<T1>(args[0])), (T2)((object)this.TryCoerce<T2>(args[1])));
			}, lookaheadSafe);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00010324 File Offset: 0x0000E524
		public void BindExternalFunction<T1, T2>(string funcName, Action<T1, T2> act, bool lookaheadSafe = false)
		{
			this.Assert(act != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 2, "External function expected two arguments", Array.Empty<object>());
				act((T1)((object)this.TryCoerce<T1>(args[0])), (T2)((object)this.TryCoerce<T2>(args[1])));
				return null;
			}, lookaheadSafe);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00010374 File Offset: 0x0000E574
		public void BindExternalFunction<T1, T2, T3>(string funcName, Func<T1, T2, T3, object> func, bool lookaheadSafe = false)
		{
			this.Assert(func != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 3, "External function expected three arguments", Array.Empty<object>());
				return func((T1)((object)this.TryCoerce<T1>(args[0])), (T2)((object)this.TryCoerce<T2>(args[1])), (T3)((object)this.TryCoerce<T3>(args[2])));
			}, lookaheadSafe);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000103C4 File Offset: 0x0000E5C4
		public void BindExternalFunction<T1, T2, T3>(string funcName, Action<T1, T2, T3> act, bool lookaheadSafe = false)
		{
			this.Assert(act != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 3, "External function expected three arguments", Array.Empty<object>());
				act((T1)((object)this.TryCoerce<T1>(args[0])), (T2)((object)this.TryCoerce<T2>(args[1])), (T3)((object)this.TryCoerce<T3>(args[2])));
				return null;
			}, lookaheadSafe);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00010414 File Offset: 0x0000E614
		public void BindExternalFunction<T1, T2, T3, T4>(string funcName, Func<T1, T2, T3, T4, object> func, bool lookaheadSafe = false)
		{
			this.Assert(func != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 4, "External function expected four arguments", Array.Empty<object>());
				return func((T1)((object)this.TryCoerce<T1>(args[0])), (T2)((object)this.TryCoerce<T2>(args[1])), (T3)((object)this.TryCoerce<T3>(args[2])), (T4)((object)this.TryCoerce<T4>(args[3])));
			}, lookaheadSafe);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00010464 File Offset: 0x0000E664
		public void BindExternalFunction<T1, T2, T3, T4>(string funcName, Action<T1, T2, T3, T4> act, bool lookaheadSafe = false)
		{
			this.Assert(act != null, "Can't bind a null function", Array.Empty<object>());
			this.BindExternalFunctionGeneral(funcName, delegate(object[] args)
			{
				this.Assert(args.Length == 4, "External function expected four arguments", Array.Empty<object>());
				act((T1)((object)this.TryCoerce<T1>(args[0])), (T2)((object)this.TryCoerce<T2>(args[1])), (T3)((object)this.TryCoerce<T3>(args[2])), (T4)((object)this.TryCoerce<T4>(args[3])));
				return null;
			}, lookaheadSafe);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000104B4 File Offset: 0x0000E6B4
		public void UnbindExternalFunction(string funcName)
		{
			this.IfAsyncWeCant("unbind an external a function");
			this.Assert(this._externals.ContainsKey(funcName), "Function '" + funcName + "' has not been bound.", Array.Empty<object>());
			this._externals.Remove(funcName);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00010500 File Offset: 0x0000E700
		public void ValidateExternalBindings()
		{
			HashSet<string> hashSet = new HashSet<string>();
			this.ValidateExternalBindings(this._mainContentContainer, hashSet);
			this._hasValidatedExternals = true;
			if (hashSet.Count == 0)
			{
				this._hasValidatedExternals = true;
				return;
			}
			string text = string.Format("ERROR: Missing function binding for external{0}: '{1}' {2}", (hashSet.Count > 1) ? "s" : string.Empty, string.Join("', '", hashSet.ToArray<string>()), this.allowExternalFunctionFallbacks ? ", and no fallback ink function found." : " (ink fallbacks disabled)");
			this.Error(text, false);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00010584 File Offset: 0x0000E784
		private void ValidateExternalBindings(Container c, HashSet<string> missingExternals)
		{
			foreach (Object @object in c.content)
			{
				Container container = @object as Container;
				if (container == null || !container.hasValidName)
				{
					this.ValidateExternalBindings(@object, missingExternals);
				}
			}
			foreach (KeyValuePair<string, INamedContent> keyValuePair in c.namedContent)
			{
				this.ValidateExternalBindings(keyValuePair.Value as Object, missingExternals);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00010640 File Offset: 0x0000E840
		private void ValidateExternalBindings(Object o, HashSet<string> missingExternals)
		{
			Container container = o as Container;
			if (container)
			{
				this.ValidateExternalBindings(container, missingExternals);
				return;
			}
			Divert divert = o as Divert;
			if (divert && divert.isExternal)
			{
				string targetPathString = divert.targetPathString;
				if (!this._externals.ContainsKey(targetPathString))
				{
					if (this.allowExternalFunctionFallbacks)
					{
						if (!this.mainContentContainer.namedContent.ContainsKey(targetPathString))
						{
							missingExternals.Add(targetPathString);
							return;
						}
					}
					else
					{
						missingExternals.Add(targetPathString);
					}
				}
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000106C0 File Offset: 0x0000E8C0
		public void ObserveVariable(string variableName, Story.VariableObserver observer)
		{
			this.IfAsyncWeCant("observe a new variable");
			if (this._variableObservers == null)
			{
				this._variableObservers = new Dictionary<string, Story.VariableObserver>();
			}
			if (!this.state.variablesState.GlobalVariableExistsWithName(variableName))
			{
				throw new Exception("Cannot observe variable '" + variableName + "' because it wasn't declared in the ink story.");
			}
			if (this._variableObservers.ContainsKey(variableName))
			{
				Dictionary<string, Story.VariableObserver> variableObservers = this._variableObservers;
				variableObservers[variableName] = (Story.VariableObserver)Delegate.Combine(variableObservers[variableName], observer);
				return;
			}
			this._variableObservers[variableName] = observer;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00010754 File Offset: 0x0000E954
		public void ObserveVariables(IList<string> variableNames, Story.VariableObserver observer)
		{
			foreach (string text in variableNames)
			{
				this.ObserveVariable(text, observer);
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000107A0 File Offset: 0x0000E9A0
		public void RemoveVariableObserver(Story.VariableObserver observer = null, string specificVariableName = null)
		{
			this.IfAsyncWeCant("remove a variable observer");
			if (this._variableObservers == null)
			{
				return;
			}
			if (specificVariableName != null)
			{
				if (this._variableObservers.ContainsKey(specificVariableName))
				{
					if (observer == null)
					{
						this._variableObservers.Remove(specificVariableName);
						return;
					}
					Dictionary<string, Story.VariableObserver> dictionary = this._variableObservers;
					dictionary[specificVariableName] = (Story.VariableObserver)Delegate.Remove(dictionary[specificVariableName], observer);
					if (this._variableObservers[specificVariableName] == null)
					{
						this._variableObservers.Remove(specificVariableName);
						return;
					}
				}
			}
			else if (observer != null)
			{
				foreach (string text in new List<string>(this._variableObservers.Keys))
				{
					Dictionary<string, Story.VariableObserver> dictionary = this._variableObservers;
					string text2 = text;
					dictionary[text2] = (Story.VariableObserver)Delegate.Remove(dictionary[text2], observer);
					if (this._variableObservers[text] == null)
					{
						this._variableObservers.Remove(text);
					}
				}
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000108B0 File Offset: 0x0000EAB0
		private void VariableStateDidChangeEvent(string variableName, Object newValueObj)
		{
			if (this._variableObservers == null)
			{
				return;
			}
			Story.VariableObserver variableObserver = null;
			if (this._variableObservers.TryGetValue(variableName, out variableObserver))
			{
				if (!(newValueObj is Value))
				{
					throw new Exception("Tried to get the value of a variable that isn't a standard type");
				}
				Value value = newValueObj as Value;
				variableObserver(variableName, value.valueObject);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002AC RID: 684 RVA: 0x000108FF File Offset: 0x0000EAFF
		public List<string> globalTags
		{
			get
			{
				return this.TagsAtStartOfFlowContainerWithPathString("");
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0001090C File Offset: 0x0000EB0C
		public List<string> TagsForContentAtPath(string path)
		{
			return this.TagsAtStartOfFlowContainerWithPathString(path);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00010918 File Offset: 0x0000EB18
		private List<string> TagsAtStartOfFlowContainerWithPathString(string pathString)
		{
			Path path = new Path(pathString);
			Container container = this.ContentAtPath(path).container;
			for (;;)
			{
				Object @object = container.content[0];
				if (!(@object is Container))
				{
					break;
				}
				container = (Container)@object;
			}
			List<string> list = null;
			foreach (Object object2 in container.content)
			{
				Tag tag = object2 as Tag;
				if (!tag)
				{
					break;
				}
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(tag.text);
			}
			return list;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000109C8 File Offset: 0x0000EBC8
		public virtual string BuildStringOfHierarchy()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.mainContentContainer.BuildStringOfHierarchy(stringBuilder, 0, this.state.currentPointer.Resolve());
			return stringBuilder.ToString();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00010A04 File Offset: 0x0000EC04
		private string BuildStringOfContainer(Container container)
		{
			StringBuilder stringBuilder = new StringBuilder();
			container.BuildStringOfHierarchy(stringBuilder, 0, this.state.currentPointer.Resolve());
			return stringBuilder.ToString();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00010A38 File Offset: 0x0000EC38
		private void NextContent()
		{
			this.state.previousPointer = this.state.currentPointer;
			if (!this.state.divertedPointer.isNull)
			{
				this.state.currentPointer = this.state.divertedPointer;
				this.state.divertedPointer = Pointer.Null;
				this.VisitChangedContainersDueToDivert();
				if (!this.state.currentPointer.isNull)
				{
					return;
				}
			}
			if (!this.IncrementContentPointer())
			{
				bool flag = false;
				if (this.state.callStack.CanPop(new PushPopType?(PushPopType.Function)))
				{
					this.state.PopCallstack(new PushPopType?(PushPopType.Function));
					if (this.state.inExpressionEvaluation)
					{
						this.state.PushEvaluationStack(new Void());
					}
					flag = true;
				}
				else if (this.state.callStack.canPopThread)
				{
					this.state.callStack.PopThread();
					flag = true;
				}
				else
				{
					this.state.TryExitFunctionEvaluationFromGame();
				}
				if (flag && !this.state.currentPointer.isNull)
				{
					this.NextContent();
				}
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00010B5C File Offset: 0x0000ED5C
		private bool IncrementContentPointer()
		{
			bool flag = true;
			Pointer pointer = this.state.callStack.currentElement.currentPointer;
			pointer.index++;
			while (pointer.index >= pointer.container.content.Count)
			{
				flag = false;
				Container container = pointer.container.parent as Container;
				if (!container)
				{
					break;
				}
				int num = container.content.IndexOf(pointer.container);
				if (num == -1)
				{
					break;
				}
				pointer = new Pointer(container, num);
				pointer.index++;
				flag = true;
			}
			if (!flag)
			{
				pointer = Pointer.Null;
			}
			this.state.callStack.currentElement.currentPointer = pointer;
			return flag;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00010C10 File Offset: 0x0000EE10
		private bool TryFollowDefaultInvisibleChoice()
		{
			List<Choice> currentChoices = this._state.currentChoices;
			List<Choice> list = currentChoices.Where((Choice c) => c.isInvisibleDefault).ToList<Choice>();
			if (list.Count == 0 || currentChoices.Count > list.Count)
			{
				return false;
			}
			Choice choice = list[0];
			this.state.callStack.currentThread = choice.threadAtGeneration;
			if (this._stateSnapshotAtLastNewline != null)
			{
				this.state.callStack.currentThread = this.state.callStack.ForkThread();
			}
			this.ChoosePath(choice.targetPath, false);
			return true;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00010CC0 File Offset: 0x0000EEC0
		private int NextSequenceShuffleIndex()
		{
			IntValue intValue = this.state.PopEvaluationStack() as IntValue;
			if (intValue == null)
			{
				this.Error("expected number of elements in sequence for shuffle index", false);
				return 0;
			}
			Object container = this.state.currentPointer.container;
			int value = intValue.value;
			int value2 = (this.state.PopEvaluationStack() as IntValue).value;
			int num = value2 / value;
			int num2 = value2 % value;
			string text = container.path.ToString();
			int num3 = 0;
			foreach (char c in text)
			{
				num3 += (int)c;
			}
			Random random = new Random(num3 + num + this.state.storySeed);
			List<int> list = new List<int>();
			for (int j = 0; j < value; j++)
			{
				list.Add(j);
			}
			for (int k = 0; k <= num2; k++)
			{
				int num4 = random.Next() % list.Count;
				int num5 = list[num4];
				list.RemoveAt(num4);
				if (k == num2)
				{
					return num5;
				}
			}
			throw new Exception("Should never reach here");
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00010DDD File Offset: 0x0000EFDD
		public void Error(string message, bool useEndLineNumber = false)
		{
			throw new StoryException(message)
			{
				useEndLineNumber = useEndLineNumber
			};
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00010DEC File Offset: 0x0000EFEC
		public void Warning(string message)
		{
			this.AddError(message, true, false);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		private void AddError(string message, bool isWarning = false, bool useEndLineNumber = false)
		{
			DebugMetadata currentDebugMetadata = this.currentDebugMetadata;
			string text = (isWarning ? "WARNING" : "ERROR");
			if (currentDebugMetadata != null)
			{
				int num = (useEndLineNumber ? currentDebugMetadata.endLineNumber : currentDebugMetadata.startLineNumber);
				message = string.Format("RUNTIME {0}: '{1}' line {2}: {3}", new object[] { text, currentDebugMetadata.fileName, num, message });
			}
			else if (!this.state.currentPointer.isNull)
			{
				message = string.Format("RUNTIME {0}: ({1}): {2}", text, this.state.currentPointer.path, message);
			}
			else
			{
				message = "RUNTIME " + text + ": " + message;
			}
			this.state.AddError(message, isWarning);
			if (!isWarning)
			{
				this.state.ForceEnd();
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		private void Assert(bool condition, string message = null, params object[] formatParams)
		{
			if (!condition)
			{
				if (message == null)
				{
					message = "Story assert";
				}
				if (formatParams != null && formatParams.Count<object>() > 0)
				{
					message = string.Format(message, formatParams);
				}
				string text = message;
				string text2 = " ";
				DebugMetadata currentDebugMetadata = this.currentDebugMetadata;
				throw new Exception(text + text2 + ((currentDebugMetadata != null) ? currentDebugMetadata.ToString() : null));
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00010F1C File Offset: 0x0000F11C
		private DebugMetadata currentDebugMetadata
		{
			get
			{
				Pointer pointer = this.state.currentPointer;
				if (!pointer.isNull)
				{
					DebugMetadata debugMetadata = pointer.Resolve().debugMetadata;
					if (debugMetadata != null)
					{
						return debugMetadata;
					}
				}
				for (int i = this.state.callStack.elements.Count - 1; i >= 0; i--)
				{
					pointer = this.state.callStack.elements[i].currentPointer;
					if (!pointer.isNull && pointer.Resolve() != null)
					{
						DebugMetadata debugMetadata = pointer.Resolve().debugMetadata;
						if (debugMetadata != null)
						{
							return debugMetadata;
						}
					}
				}
				for (int j = this.state.outputStream.Count - 1; j >= 0; j--)
				{
					DebugMetadata debugMetadata = this.state.outputStream[j].debugMetadata;
					if (debugMetadata != null)
					{
						return debugMetadata;
					}
				}
				return null;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		private int currentLineNumber
		{
			get
			{
				DebugMetadata currentDebugMetadata = this.currentDebugMetadata;
				if (currentDebugMetadata != null)
				{
					return currentDebugMetadata.startLineNumber;
				}
				return 0;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00011013 File Offset: 0x0000F213
		public Container mainContentContainer
		{
			get
			{
				if (this._temporaryEvaluationContainer)
				{
					return this._temporaryEvaluationContainer;
				}
				return this._mainContentContainer;
			}
		}

		// Token: 0x040000C8 RID: 200
		public const int inkVersionCurrent = 20;

		// Token: 0x040000C9 RID: 201
		private const int inkVersionMinimumCompatible = 18;

		// Token: 0x040000D0 RID: 208
		private List<Container> _prevContainers = new List<Container>();

		// Token: 0x040000D2 RID: 210
		private Container _mainContentContainer;

		// Token: 0x040000D3 RID: 211
		private ListDefinitionsOrigin _listDefinitions;

		// Token: 0x040000D4 RID: 212
		private Dictionary<string, Story.ExternalFunctionDef> _externals;

		// Token: 0x040000D5 RID: 213
		private Dictionary<string, Story.VariableObserver> _variableObservers;

		// Token: 0x040000D6 RID: 214
		private bool _hasValidatedExternals;

		// Token: 0x040000D7 RID: 215
		private Container _temporaryEvaluationContainer;

		// Token: 0x040000D8 RID: 216
		private StoryState _state;

		// Token: 0x040000D9 RID: 217
		private bool _asyncContinueActive;

		// Token: 0x040000DA RID: 218
		private StoryState _stateSnapshotAtLastNewline;

		// Token: 0x040000DB RID: 219
		private bool _sawLookaheadUnsafeFunctionAfterNewline;

		// Token: 0x040000DC RID: 220
		private int _recursiveContinueCount;

		// Token: 0x040000DD RID: 221
		private bool _asyncSaving;

		// Token: 0x040000DE RID: 222
		private Profiler _profiler;

		// Token: 0x02000096 RID: 150
		private enum OutputStateChange
		{
			// Token: 0x04000270 RID: 624
			NoChange,
			// Token: 0x04000271 RID: 625
			ExtendedBeyondNewline,
			// Token: 0x04000272 RID: 626
			NewlineRemoved
		}

		// Token: 0x02000097 RID: 151
		// (Invoke) Token: 0x060005E2 RID: 1506
		public delegate object ExternalFunction(object[] args);

		// Token: 0x02000098 RID: 152
		// (Invoke) Token: 0x060005E6 RID: 1510
		public delegate void VariableObserver(string variableName, object newValue);

		// Token: 0x02000099 RID: 153
		private struct ExternalFunctionDef
		{
			// Token: 0x04000273 RID: 627
			public Story.ExternalFunction function;

			// Token: 0x04000274 RID: 628
			public bool lookaheadSafe;
		}
	}
}
