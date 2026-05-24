using System;

namespace Ink.Runtime
{
	// Token: 0x02000016 RID: 22
	public class ControlCommand : Object
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000871E File Offset: 0x0000691E
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00008726 File Offset: 0x00006926
		public ControlCommand.CommandType commandType { get; protected set; }

		// Token: 0x06000155 RID: 341 RVA: 0x0000872F File Offset: 0x0000692F
		public ControlCommand(ControlCommand.CommandType commandType)
		{
			this.commandType = commandType;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000873E File Offset: 0x0000693E
		public ControlCommand()
			: this(ControlCommand.CommandType.NotSet)
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008747 File Offset: 0x00006947
		public override Object Copy()
		{
			return new ControlCommand(this.commandType);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008754 File Offset: 0x00006954
		public static ControlCommand EvalStart()
		{
			return new ControlCommand(ControlCommand.CommandType.EvalStart);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000875C File Offset: 0x0000695C
		public static ControlCommand EvalOutput()
		{
			return new ControlCommand(ControlCommand.CommandType.EvalOutput);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008764 File Offset: 0x00006964
		public static ControlCommand EvalEnd()
		{
			return new ControlCommand(ControlCommand.CommandType.EvalEnd);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000876C File Offset: 0x0000696C
		public static ControlCommand Duplicate()
		{
			return new ControlCommand(ControlCommand.CommandType.Duplicate);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008774 File Offset: 0x00006974
		public static ControlCommand PopEvaluatedValue()
		{
			return new ControlCommand(ControlCommand.CommandType.PopEvaluatedValue);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000877C File Offset: 0x0000697C
		public static ControlCommand PopFunction()
		{
			return new ControlCommand(ControlCommand.CommandType.PopFunction);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008784 File Offset: 0x00006984
		public static ControlCommand PopTunnel()
		{
			return new ControlCommand(ControlCommand.CommandType.PopTunnel);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000878C File Offset: 0x0000698C
		public static ControlCommand BeginString()
		{
			return new ControlCommand(ControlCommand.CommandType.BeginString);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008794 File Offset: 0x00006994
		public static ControlCommand EndString()
		{
			return new ControlCommand(ControlCommand.CommandType.EndString);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000879C File Offset: 0x0000699C
		public static ControlCommand NoOp()
		{
			return new ControlCommand(ControlCommand.CommandType.NoOp);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000087A5 File Offset: 0x000069A5
		public static ControlCommand ChoiceCount()
		{
			return new ControlCommand(ControlCommand.CommandType.ChoiceCount);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000087AE File Offset: 0x000069AE
		public static ControlCommand Turns()
		{
			return new ControlCommand(ControlCommand.CommandType.Turns);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000087B7 File Offset: 0x000069B7
		public static ControlCommand TurnsSince()
		{
			return new ControlCommand(ControlCommand.CommandType.TurnsSince);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000087C0 File Offset: 0x000069C0
		public static ControlCommand ReadCount()
		{
			return new ControlCommand(ControlCommand.CommandType.ReadCount);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000087C9 File Offset: 0x000069C9
		public static ControlCommand Random()
		{
			return new ControlCommand(ControlCommand.CommandType.Random);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000087D2 File Offset: 0x000069D2
		public static ControlCommand SeedRandom()
		{
			return new ControlCommand(ControlCommand.CommandType.SeedRandom);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000087DB File Offset: 0x000069DB
		public static ControlCommand VisitIndex()
		{
			return new ControlCommand(ControlCommand.CommandType.VisitIndex);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000087E4 File Offset: 0x000069E4
		public static ControlCommand SequenceShuffleIndex()
		{
			return new ControlCommand(ControlCommand.CommandType.SequenceShuffleIndex);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000087ED File Offset: 0x000069ED
		public static ControlCommand StartThread()
		{
			return new ControlCommand(ControlCommand.CommandType.StartThread);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000087F6 File Offset: 0x000069F6
		public static ControlCommand Done()
		{
			return new ControlCommand(ControlCommand.CommandType.Done);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000087FF File Offset: 0x000069FF
		public static ControlCommand End()
		{
			return new ControlCommand(ControlCommand.CommandType.End);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008808 File Offset: 0x00006A08
		public static ControlCommand ListFromInt()
		{
			return new ControlCommand(ControlCommand.CommandType.ListFromInt);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008811 File Offset: 0x00006A11
		public static ControlCommand ListRange()
		{
			return new ControlCommand(ControlCommand.CommandType.ListRange);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000881A File Offset: 0x00006A1A
		public static ControlCommand ListRandom()
		{
			return new ControlCommand(ControlCommand.CommandType.ListRandom);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008824 File Offset: 0x00006A24
		public override string ToString()
		{
			return this.commandType.ToString();
		}

		// Token: 0x0200008B RID: 139
		public enum CommandType
		{
			// Token: 0x040001F8 RID: 504
			NotSet = -1,
			// Token: 0x040001F9 RID: 505
			EvalStart,
			// Token: 0x040001FA RID: 506
			EvalOutput,
			// Token: 0x040001FB RID: 507
			EvalEnd,
			// Token: 0x040001FC RID: 508
			Duplicate,
			// Token: 0x040001FD RID: 509
			PopEvaluatedValue,
			// Token: 0x040001FE RID: 510
			PopFunction,
			// Token: 0x040001FF RID: 511
			PopTunnel,
			// Token: 0x04000200 RID: 512
			BeginString,
			// Token: 0x04000201 RID: 513
			EndString,
			// Token: 0x04000202 RID: 514
			NoOp,
			// Token: 0x04000203 RID: 515
			ChoiceCount,
			// Token: 0x04000204 RID: 516
			Turns,
			// Token: 0x04000205 RID: 517
			TurnsSince,
			// Token: 0x04000206 RID: 518
			ReadCount,
			// Token: 0x04000207 RID: 519
			Random,
			// Token: 0x04000208 RID: 520
			SeedRandom,
			// Token: 0x04000209 RID: 521
			VisitIndex,
			// Token: 0x0400020A RID: 522
			SequenceShuffleIndex,
			// Token: 0x0400020B RID: 523
			StartThread,
			// Token: 0x0400020C RID: 524
			Done,
			// Token: 0x0400020D RID: 525
			End,
			// Token: 0x0400020E RID: 526
			ListFromInt,
			// Token: 0x0400020F RID: 527
			ListRange,
			// Token: 0x04000210 RID: 528
			ListRandom,
			// Token: 0x04000211 RID: 529
			TOTAL_VALUES
		}
	}
}
