using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ink.Runtime
{
	// Token: 0x02000025 RID: 37
	public class Profiler
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000CEBF File Offset: 0x0000B0BF
		public ProfileNode rootNode
		{
			get
			{
				return this._rootNode;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000CEC7 File Offset: 0x0000B0C7
		public Profiler()
		{
			this._rootNode = new ProfileNode();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000CF08 File Offset: 0x0000B108
		public string Report()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0} CONTINUES / LINES:\n", this._numContinues);
			stringBuilder.AppendFormat("TOTAL TIME: {0}\n", Profiler.FormatMillisecs(this._continueTotal));
			stringBuilder.AppendFormat("SNAPSHOTTING: {0}\n", Profiler.FormatMillisecs(this._snapTotal));
			stringBuilder.AppendFormat("OTHER: {0}\n", Profiler.FormatMillisecs(this._continueTotal - (this._stepTotal + this._snapTotal)));
			stringBuilder.Append(this._rootNode.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000CF9B File Offset: 0x0000B19B
		public void PreContinue()
		{
			this._continueWatch.Reset();
			this._continueWatch.Start();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000CFB3 File Offset: 0x0000B1B3
		public void PostContinue()
		{
			this._continueWatch.Stop();
			this._continueTotal += this.Millisecs(this._continueWatch);
			this._numContinues++;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000CFE7 File Offset: 0x0000B1E7
		public void PreStep()
		{
			this._currStepStack = null;
			this._stepWatch.Reset();
			this._stepWatch.Start();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000D008 File Offset: 0x0000B208
		public void Step(CallStack callstack)
		{
			this._stepWatch.Stop();
			string[] array = new string[callstack.elements.Count];
			for (int i = 0; i < array.Length; i++)
			{
				string text = "";
				if (!callstack.elements[i].currentPointer.isNull)
				{
					Path path = callstack.elements[i].currentPointer.path;
					for (int j = 0; j < path.length; j++)
					{
						Path.Component component = path.GetComponent(j);
						if (!component.isIndex)
						{
							text = component.name;
							break;
						}
					}
				}
				array[i] = text;
			}
			this._currStepStack = array;
			Object @object = callstack.currentElement.currentPointer.Resolve();
			ControlCommand controlCommand = @object as ControlCommand;
			string text2;
			if (controlCommand)
			{
				text2 = controlCommand.commandType.ToString() + " CC";
			}
			else
			{
				text2 = @object.GetType().Name;
			}
			this._currStepDetails = new Profiler.StepDetails
			{
				type = text2,
				obj = @object
			};
			this._stepWatch.Start();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000D13C File Offset: 0x0000B33C
		public void PostStep()
		{
			this._stepWatch.Stop();
			double num = this.Millisecs(this._stepWatch);
			this._stepTotal += num;
			this._rootNode.AddSample(this._currStepStack, num);
			this._currStepDetails.time = num;
			this._stepDetails.Add(this._currStepDetails);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
		public string StepLengthReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("TOTAL: " + this._rootNode.totalMillisecs.ToString() + "ms");
			string[] array = (from s in this._stepDetails
				group s by s.type into typeToDetails
				select new KeyValuePair<string, double>(typeToDetails.Key, typeToDetails.Average((Profiler.StepDetails d) => d.time)) into stepTypeToAverage
				orderby stepTypeToAverage.Value descending
				select stepTypeToAverage.Key + ": " + stepTypeToAverage.Value.ToString() + "ms").ToArray<string>();
			stringBuilder.AppendLine("AVERAGE STEP TIMES: " + string.Join(", ", array));
			string[] array2 = (from s in this._stepDetails
				group s by s.type into typeToDetails
				select new KeyValuePair<string, double>(typeToDetails.Key + " (x" + typeToDetails.Count<Profiler.StepDetails>().ToString() + ")", typeToDetails.Sum((Profiler.StepDetails d) => d.time)) into stepTypeToAccum
				orderby stepTypeToAccum.Value descending
				select stepTypeToAccum.Key + ": " + stepTypeToAccum.Value.ToString()).ToArray<string>();
			stringBuilder.AppendLine("ACCUMULATED STEP TIMES: " + string.Join(", ", array2));
			return stringBuilder.ToString();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000D350 File Offset: 0x0000B550
		public string Megalog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Step type\tDescription\tPath\tTime");
			foreach (Profiler.StepDetails stepDetails in this._stepDetails)
			{
				stringBuilder.Append(stepDetails.type);
				stringBuilder.Append("\t");
				stringBuilder.Append(stepDetails.obj.ToString());
				stringBuilder.Append("\t");
				stringBuilder.Append(stepDetails.obj.path);
				stringBuilder.Append("\t");
				stringBuilder.AppendLine(stepDetails.time.ToString("F8"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000D424 File Offset: 0x0000B624
		public void PreSnapshot()
		{
			this._snapWatch.Reset();
			this._snapWatch.Start();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000D43C File Offset: 0x0000B63C
		public void PostSnapshot()
		{
			this._snapWatch.Stop();
			this._snapTotal += this.Millisecs(this._snapWatch);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000D462 File Offset: 0x0000B662
		private double Millisecs(Stopwatch watch)
		{
			return (double)watch.ElapsedTicks * Profiler._millisecsPerTick;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000D474 File Offset: 0x0000B674
		public static string FormatMillisecs(double num)
		{
			if (num > 5000.0)
			{
				return string.Format("{0:N1} secs", num / 1000.0);
			}
			if (num > 1000.0)
			{
				return string.Format("{0:N2} secs", num / 1000.0);
			}
			if (num > 100.0)
			{
				return string.Format("{0:N0} ms", num);
			}
			if (num > 1.0)
			{
				return string.Format("{0:N1} ms", num);
			}
			if (num > 0.01)
			{
				return string.Format("{0:N3} ms", num);
			}
			return string.Format("{0:N} ms", num);
		}

		// Token: 0x040000AB RID: 171
		private Stopwatch _continueWatch = new Stopwatch();

		// Token: 0x040000AC RID: 172
		private Stopwatch _stepWatch = new Stopwatch();

		// Token: 0x040000AD RID: 173
		private Stopwatch _snapWatch = new Stopwatch();

		// Token: 0x040000AE RID: 174
		private double _continueTotal;

		// Token: 0x040000AF RID: 175
		private double _snapTotal;

		// Token: 0x040000B0 RID: 176
		private double _stepTotal;

		// Token: 0x040000B1 RID: 177
		private string[] _currStepStack;

		// Token: 0x040000B2 RID: 178
		private Profiler.StepDetails _currStepDetails;

		// Token: 0x040000B3 RID: 179
		private ProfileNode _rootNode;

		// Token: 0x040000B4 RID: 180
		private int _numContinues;

		// Token: 0x040000B5 RID: 181
		private List<Profiler.StepDetails> _stepDetails = new List<Profiler.StepDetails>();

		// Token: 0x040000B6 RID: 182
		private static double _millisecsPerTick = 1000.0 / (double)Stopwatch.Frequency;

		// Token: 0x02000091 RID: 145
		private struct StepDetails
		{
			// Token: 0x0400025A RID: 602
			public string type;

			// Token: 0x0400025B RID: 603
			public Object obj;

			// Token: 0x0400025C RID: 604
			public double time;
		}
	}
}
