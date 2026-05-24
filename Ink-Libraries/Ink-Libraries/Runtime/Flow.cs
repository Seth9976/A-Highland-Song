using System;
using System.Collections.Generic;

namespace Ink.Runtime
{
	// Token: 0x02000019 RID: 25
	public class Flow
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00008CC9 File Offset: 0x00006EC9
		public Flow(string name, Story story)
		{
			this.name = name;
			this.callStack = new CallStack(story);
			this.outputStream = new List<Object>();
			this.currentChoices = new List<Choice>();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008CFC File Offset: 0x00006EFC
		public Flow(string name, Story story, Dictionary<string, object> jObject)
		{
			this.name = name;
			this.callStack = new CallStack(story);
			this.callStack.SetJsonToken((Dictionary<string, object>)jObject["callstack"], story);
			this.outputStream = Json.JArrayToRuntimeObjList((List<object>)jObject["outputStream"], false);
			this.currentChoices = Json.JArrayToRuntimeObjList<Choice>((List<object>)jObject["currentChoices"], false);
			object obj;
			jObject.TryGetValue("choiceThreads", out obj);
			this.LoadFlowChoiceThreads((Dictionary<string, object>)obj, story);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008D94 File Offset: 0x00006F94
		public void WriteJson(SimpleJson.Writer writer)
		{
			writer.WriteObjectStart();
			writer.WriteProperty("callstack", new Action<SimpleJson.Writer>(this.callStack.WriteJson));
			writer.WriteProperty("outputStream", delegate(SimpleJson.Writer w)
			{
				Json.WriteListRuntimeObjs(w, this.outputStream);
			});
			bool flag = false;
			foreach (Choice choice in this.currentChoices)
			{
				choice.originalThreadIndex = choice.threadAtGeneration.threadIndex;
				if (this.callStack.ThreadWithIndex(choice.originalThreadIndex) == null)
				{
					if (!flag)
					{
						flag = true;
						writer.WritePropertyStart("choiceThreads");
						writer.WriteObjectStart();
					}
					writer.WritePropertyStart(choice.originalThreadIndex);
					choice.threadAtGeneration.WriteJson(writer);
					writer.WritePropertyEnd();
				}
			}
			if (flag)
			{
				writer.WriteObjectEnd();
				writer.WritePropertyEnd();
			}
			writer.WriteProperty("currentChoices", delegate(SimpleJson.Writer w)
			{
				w.WriteArrayStart();
				foreach (Choice choice2 in this.currentChoices)
				{
					Json.WriteChoice(w, choice2);
				}
				w.WriteArrayEnd();
			});
			writer.WriteObjectEnd();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008EA0 File Offset: 0x000070A0
		public void LoadFlowChoiceThreads(Dictionary<string, object> jChoiceThreads, Story story)
		{
			foreach (Choice choice in this.currentChoices)
			{
				CallStack.Thread thread = this.callStack.ThreadWithIndex(choice.originalThreadIndex);
				if (thread != null)
				{
					choice.threadAtGeneration = thread.Copy();
				}
				else
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)jChoiceThreads[choice.originalThreadIndex.ToString()];
					choice.threadAtGeneration = new CallStack.Thread(dictionary, story);
				}
			}
		}

		// Token: 0x0400006E RID: 110
		public string name;

		// Token: 0x0400006F RID: 111
		public CallStack callStack;

		// Token: 0x04000070 RID: 112
		public List<Object> outputStream;

		// Token: 0x04000071 RID: 113
		public List<Choice> currentChoices;
	}
}
