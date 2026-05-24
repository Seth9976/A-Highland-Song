using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000441 RID: 1089
	public static class PlayableExtensions
	{
		// Token: 0x060025A7 RID: 9639 RVA: 0x0003F5D8 File Offset: 0x0003D7D8
		public static bool IsNull<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsNull();
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x0003F600 File Offset: 0x0003D800
		public static bool IsValid<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsValid();
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x0003F628 File Offset: 0x0003D828
		public static void Destroy<U>(this U playable) where U : struct, IPlayable
		{
			playable.GetHandle().Destroy();
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x0003F64C File Offset: 0x0003D84C
		public static PlayableGraph GetGraph<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetGraph();
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x0003F674 File Offset: 0x0003D874
		[Obsolete("SetPlayState() has been deprecated. Use Play(), Pause() or SetDelay() instead", false)]
		public static void SetPlayState<U>(this U playable, PlayState value) where U : struct, IPlayable
		{
			bool flag = value == PlayState.Delayed;
			if (flag)
			{
				throw new ArgumentException("Can't set Delayed: use SetDelay() instead");
			}
			if (value != PlayState.Paused)
			{
				if (value == PlayState.Playing)
				{
					playable.GetHandle().Play();
				}
			}
			else
			{
				playable.GetHandle().Pause();
			}
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x0003F6D4 File Offset: 0x0003D8D4
		public static PlayState GetPlayState<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetPlayState();
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x0003F6FC File Offset: 0x0003D8FC
		public static void Play<U>(this U playable) where U : struct, IPlayable
		{
			playable.GetHandle().Play();
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x0003F720 File Offset: 0x0003D920
		public static void Pause<U>(this U playable) where U : struct, IPlayable
		{
			playable.GetHandle().Pause();
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x0003F744 File Offset: 0x0003D944
		public static void SetSpeed<U>(this U playable, double value) where U : struct, IPlayable
		{
			playable.GetHandle().SetSpeed(value);
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x0003F76C File Offset: 0x0003D96C
		public static double GetSpeed<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetSpeed();
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x0003F794 File Offset: 0x0003D994
		public static void SetDuration<U>(this U playable, double value) where U : struct, IPlayable
		{
			playable.GetHandle().SetDuration(value);
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x0003F7BC File Offset: 0x0003D9BC
		public static double GetDuration<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetDuration();
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x0003F7E4 File Offset: 0x0003D9E4
		public static void SetTime<U>(this U playable, double value) where U : struct, IPlayable
		{
			playable.GetHandle().SetTime(value);
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x0003F80C File Offset: 0x0003DA0C
		public static double GetTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetTime();
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x0003F834 File Offset: 0x0003DA34
		public static double GetPreviousTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetPreviousTime();
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x0003F85C File Offset: 0x0003DA5C
		public static void SetDone<U>(this U playable, bool value) where U : struct, IPlayable
		{
			playable.GetHandle().SetDone(value);
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x0003F884 File Offset: 0x0003DA84
		public static bool IsDone<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsDone();
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x0003F8AC File Offset: 0x0003DAAC
		public static void SetPropagateSetTime<U>(this U playable, bool value) where U : struct, IPlayable
		{
			playable.GetHandle().SetPropagateSetTime(value);
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x0003F8D4 File Offset: 0x0003DAD4
		public static bool GetPropagateSetTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetPropagateSetTime();
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x0003F8FC File Offset: 0x0003DAFC
		public static bool CanChangeInputs<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().CanChangeInputs();
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x0003F924 File Offset: 0x0003DB24
		public static bool CanSetWeights<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().CanSetWeights();
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x0003F94C File Offset: 0x0003DB4C
		public static bool CanDestroy<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().CanDestroy();
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x0003F974 File Offset: 0x0003DB74
		public static void SetInputCount<U>(this U playable, int value) where U : struct, IPlayable
		{
			playable.GetHandle().SetInputCount(value);
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x0003F99C File Offset: 0x0003DB9C
		public static int GetInputCount<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetInputCount();
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x0003F9C4 File Offset: 0x0003DBC4
		public static void SetOutputCount<U>(this U playable, int value) where U : struct, IPlayable
		{
			playable.GetHandle().SetOutputCount(value);
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x0003F9EC File Offset: 0x0003DBEC
		public static int GetOutputCount<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetOutputCount();
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x0003FA14 File Offset: 0x0003DC14
		public static Playable GetInput<U>(this U playable, int inputPort) where U : struct, IPlayable
		{
			return playable.GetHandle().GetInput(inputPort);
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x0003FA3C File Offset: 0x0003DC3C
		public static Playable GetOutput<U>(this U playable, int outputPort) where U : struct, IPlayable
		{
			return playable.GetHandle().GetOutput(outputPort);
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x0003FA64 File Offset: 0x0003DC64
		public static void SetInputWeight<U>(this U playable, int inputIndex, float weight) where U : struct, IPlayable
		{
			playable.GetHandle().SetInputWeight(inputIndex, weight);
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x0003FA8C File Offset: 0x0003DC8C
		public static void SetInputWeight<U, V>(this U playable, V input, float weight) where U : struct, IPlayable where V : struct, IPlayable
		{
			playable.GetHandle().SetInputWeight(input.GetHandle(), weight);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x0003FAC0 File Offset: 0x0003DCC0
		public static float GetInputWeight<U>(this U playable, int inputIndex) where U : struct, IPlayable
		{
			return playable.GetHandle().GetInputWeight(inputIndex);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x0003FAE8 File Offset: 0x0003DCE8
		public static void ConnectInput<U, V>(this U playable, int inputIndex, V sourcePlayable, int sourceOutputIndex) where U : struct, IPlayable where V : struct, IPlayable
		{
			playable.ConnectInput(inputIndex, sourcePlayable, sourceOutputIndex, 0f);
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x0003FAFC File Offset: 0x0003DCFC
		public static void ConnectInput<U, V>(this U playable, int inputIndex, V sourcePlayable, int sourceOutputIndex, float weight) where U : struct, IPlayable where V : struct, IPlayable
		{
			playable.GetGraph<U>().Connect<V, U>(sourcePlayable, sourceOutputIndex, playable, inputIndex);
			playable.SetInputWeight(inputIndex, weight);
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x0003FB28 File Offset: 0x0003DD28
		public static void DisconnectInput<U>(this U playable, int inputPort) where U : struct, IPlayable
		{
			playable.GetGraph<U>().Disconnect<U>(playable, inputPort);
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x0003FB48 File Offset: 0x0003DD48
		public static int AddInput<U, V>(this U playable, V sourcePlayable, int sourceOutputIndex, float weight = 0f) where U : struct, IPlayable where V : struct, IPlayable
		{
			int inputCount = playable.GetInputCount<U>();
			playable.SetInputCount(inputCount + 1);
			playable.ConnectInput(inputCount, sourcePlayable, sourceOutputIndex, weight);
			return inputCount;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x0003FB78 File Offset: 0x0003DD78
		[Obsolete("SetDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public static void SetDelay<U>(this U playable, double delay) where U : struct, IPlayable
		{
			playable.GetHandle().SetDelay(delay);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x0003FBA0 File Offset: 0x0003DDA0
		[Obsolete("GetDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public static double GetDelay<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetDelay();
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x0003FBC8 File Offset: 0x0003DDC8
		[Obsolete("IsDelayed is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		public static bool IsDelayed<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().IsDelayed();
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x0003FBF0 File Offset: 0x0003DDF0
		public static void SetLeadTime<U>(this U playable, float value) where U : struct, IPlayable
		{
			playable.GetHandle().SetLeadTime(value);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x0003FC18 File Offset: 0x0003DE18
		public static float GetLeadTime<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetLeadTime();
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x0003FC40 File Offset: 0x0003DE40
		public static PlayableTraversalMode GetTraversalMode<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetTraversalMode();
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x0003FC68 File Offset: 0x0003DE68
		public static void SetTraversalMode<U>(this U playable, PlayableTraversalMode mode) where U : struct, IPlayable
		{
			playable.GetHandle().SetTraversalMode(mode);
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x0003FC90 File Offset: 0x0003DE90
		internal static DirectorWrapMode GetTimeWrapMode<U>(this U playable) where U : struct, IPlayable
		{
			return playable.GetHandle().GetTimeWrapMode();
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x0003FCB8 File Offset: 0x0003DEB8
		internal static void SetTimeWrapMode<U>(this U playable, DirectorWrapMode value) where U : struct, IPlayable
		{
			playable.GetHandle().SetTimeWrapMode(value);
		}
	}
}
