using System;
using System.Collections.Generic;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200005D RID: 93
	public class Sequence : Object
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x0001869C File Offset: 0x0001689C
		public Sequence(List<ContentList> elementContentLists, SequenceType sequenceType)
		{
			this.sequenceType = sequenceType;
			this.sequenceElements = new List<Object>();
			foreach (ContentList contentList in elementContentLists)
			{
				List<Object> content = contentList.content;
				Object @object;
				if (content == null || content.Count == 0)
				{
					@object = contentList;
				}
				else
				{
					@object = new Weave(content, -1);
				}
				this.sequenceElements.Add(@object);
				base.AddContent<Object>(@object);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00018730 File Offset: 0x00016930
		public override Object GenerateRuntimeObject()
		{
			Container container = new Container();
			container.visitsShouldBeCounted = true;
			container.countingAtStartOnly = true;
			this._sequenceDivertsToResove = new List<Sequence.SequenceDivertToResolve>();
			container.AddContent(ControlCommand.EvalStart());
			container.AddContent(ControlCommand.VisitIndex());
			bool flag = (this.sequenceType & SequenceType.Once) > (SequenceType)0;
			bool flag2 = (this.sequenceType & SequenceType.Cycle) > (SequenceType)0;
			bool flag3 = (this.sequenceType & SequenceType.Stopping) > (SequenceType)0;
			bool flag4 = (this.sequenceType & SequenceType.Shuffle) > (SequenceType)0;
			int num = this.sequenceElements.Count;
			if (flag)
			{
				num++;
			}
			if (flag3 || flag)
			{
				container.AddContent(new IntValue(num - 1));
				container.AddContent(NativeFunctionCall.CallWithName("MIN"));
			}
			else if (flag2)
			{
				container.AddContent(new IntValue(this.sequenceElements.Count));
				container.AddContent(NativeFunctionCall.CallWithName("%"));
			}
			if (flag4)
			{
				ControlCommand controlCommand = ControlCommand.NoOp();
				if (flag || flag3)
				{
					int num2 = (flag3 ? (this.sequenceElements.Count - 1) : this.sequenceElements.Count);
					container.AddContent(ControlCommand.Duplicate());
					container.AddContent(new IntValue(num2));
					container.AddContent(NativeFunctionCall.CallWithName("=="));
					Divert divert = new Divert();
					divert.isConditional = true;
					container.AddContent(divert);
					this.AddDivertToResolve(divert, controlCommand);
				}
				int num3 = this.sequenceElements.Count;
				if (flag3)
				{
					num3--;
				}
				container.AddContent(new IntValue(num3));
				container.AddContent(ControlCommand.SequenceShuffleIndex());
				if (flag || flag3)
				{
					container.AddContent(controlCommand);
				}
			}
			container.AddContent(ControlCommand.EvalEnd());
			ControlCommand controlCommand2 = ControlCommand.NoOp();
			for (int i = 0; i < num; i++)
			{
				container.AddContent(ControlCommand.EvalStart());
				container.AddContent(ControlCommand.Duplicate());
				container.AddContent(new IntValue(i));
				container.AddContent(NativeFunctionCall.CallWithName("=="));
				container.AddContent(ControlCommand.EvalEnd());
				Divert divert2 = new Divert();
				divert2.isConditional = true;
				container.AddContent(divert2);
				Container container2;
				if (i < this.sequenceElements.Count)
				{
					container2 = (Container)this.sequenceElements[i].runtimeObject;
				}
				else
				{
					container2 = new Container();
				}
				container2.name = "s" + i.ToString();
				container2.InsertContent(ControlCommand.PopEvaluatedValue(), 0);
				Divert divert3 = new Divert();
				container2.AddContent(divert3);
				container.AddToNamedContentOnly(container2);
				this.AddDivertToResolve(divert2, container2);
				this.AddDivertToResolve(divert3, controlCommand2);
			}
			container.AddContent(controlCommand2);
			return container;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000189C3 File Offset: 0x00016BC3
		private void AddDivertToResolve(Divert divert, Object targetContent)
		{
			this._sequenceDivertsToResove.Add(new Sequence.SequenceDivertToResolve
			{
				divert = divert,
				targetContent = targetContent
			});
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000189E4 File Offset: 0x00016BE4
		public override void ResolveReferences(Story context)
		{
			base.ResolveReferences(context);
			foreach (Sequence.SequenceDivertToResolve sequenceDivertToResolve in this._sequenceDivertsToResove)
			{
				sequenceDivertToResolve.divert.targetPath = sequenceDivertToResolve.targetContent.path;
			}
		}

		// Token: 0x0400017E RID: 382
		public List<Object> sequenceElements;

		// Token: 0x0400017F RID: 383
		public SequenceType sequenceType;

		// Token: 0x04000180 RID: 384
		private List<Sequence.SequenceDivertToResolve> _sequenceDivertsToResove;

		// Token: 0x020000AB RID: 171
		private class SequenceDivertToResolve
		{
			// Token: 0x04000296 RID: 662
			public Divert divert;

			// Token: 0x04000297 RID: 663
			public Object targetContent;
		}
	}
}
