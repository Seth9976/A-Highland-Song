using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000447 RID: 1095
	public static class PlayableOutputExtensions
	{
		// Token: 0x0600268A RID: 9866 RVA: 0x000404DC File Offset: 0x0003E6DC
		public static bool IsOutputNull<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().IsNull();
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x00040504 File Offset: 0x0003E704
		public static bool IsOutputValid<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().IsValid();
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0004052C File Offset: 0x0003E72C
		public static Object GetReferenceObject<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().GetReferenceObject();
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x00040554 File Offset: 0x0003E754
		public static void SetReferenceObject<U>(this U output, Object value) where U : struct, IPlayableOutput
		{
			output.GetHandle().SetReferenceObject(value);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x0004057C File Offset: 0x0003E77C
		public static Object GetUserData<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().GetUserData();
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000405A4 File Offset: 0x0003E7A4
		public static void SetUserData<U>(this U output, Object value) where U : struct, IPlayableOutput
		{
			output.GetHandle().SetUserData(value);
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000405CC File Offset: 0x0003E7CC
		public static Playable GetSourcePlayable<U>(this U output) where U : struct, IPlayableOutput
		{
			return new Playable(output.GetHandle().GetSourcePlayable());
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000405F8 File Offset: 0x0003E7F8
		public static void SetSourcePlayable<U, V>(this U output, V value) where U : struct, IPlayableOutput where V : struct, IPlayable
		{
			output.GetHandle().SetSourcePlayable(value.GetHandle(), output.GetSourceOutputPort<U>());
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x00040630 File Offset: 0x0003E830
		public static void SetSourcePlayable<U, V>(this U output, V value, int port) where U : struct, IPlayableOutput where V : struct, IPlayable
		{
			output.GetHandle().SetSourcePlayable(value.GetHandle(), port);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x00040664 File Offset: 0x0003E864
		public static int GetSourceOutputPort<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().GetSourceOutputPort();
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0004068C File Offset: 0x0003E88C
		public static float GetWeight<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().GetWeight();
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000406B4 File Offset: 0x0003E8B4
		public static void SetWeight<U>(this U output, float value) where U : struct, IPlayableOutput
		{
			output.GetHandle().SetWeight(value);
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000406DC File Offset: 0x0003E8DC
		public static void PushNotification<U>(this U output, Playable origin, INotification notification, object context = null) where U : struct, IPlayableOutput
		{
			output.GetHandle().PushNotification(origin.GetHandle(), notification, context);
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0004070C File Offset: 0x0003E90C
		public static INotificationReceiver[] GetNotificationReceivers<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().GetNotificationReceivers();
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x00040734 File Offset: 0x0003E934
		public static void AddNotificationReceiver<U>(this U output, INotificationReceiver receiver) where U : struct, IPlayableOutput
		{
			output.GetHandle().AddNotificationReceiver(receiver);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0004075C File Offset: 0x0003E95C
		public static void RemoveNotificationReceiver<U>(this U output, INotificationReceiver receiver) where U : struct, IPlayableOutput
		{
			output.GetHandle().RemoveNotificationReceiver(receiver);
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x00040784 File Offset: 0x0003E984
		[Obsolete("Method GetSourceInputPort has been renamed to GetSourceOutputPort (UnityUpgradable) -> GetSourceOutputPort<U>(*)", false)]
		public static int GetSourceInputPort<U>(this U output) where U : struct, IPlayableOutput
		{
			return output.GetHandle().GetSourceOutputPort();
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000407AB File Offset: 0x0003E9AB
		[Obsolete("Method SetSourceInputPort has been deprecated. Use SetSourcePlayable(Playable, Port) instead.", false)]
		public static void SetSourceInputPort<U>(this U output, int value) where U : struct, IPlayableOutput
		{
			output.SetSourcePlayable(output.GetSourcePlayable<U>(), value);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000407AB File Offset: 0x0003E9AB
		[Obsolete("Method SetSourceOutputPort has been deprecated. Use SetSourcePlayable(Playable, Port) instead.", false)]
		public static void SetSourceOutputPort<U>(this U output, int value) where U : struct, IPlayableOutput
		{
			output.SetSourcePlayable(output.GetSourcePlayable<U>(), value);
		}
	}
}
