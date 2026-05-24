using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x0200038D RID: 909
	[Serializable]
	internal class PlayerEditorConnectionEvents
	{
		// Token: 0x06001EDF RID: 7903 RVA: 0x0003234C File Offset: 0x0003054C
		public void InvokeMessageIdSubscribers(Guid messageId, byte[] data, int playerId)
		{
			IEnumerable<PlayerEditorConnectionEvents.MessageTypeSubscribers> enumerable = Enumerable.Where<PlayerEditorConnectionEvents.MessageTypeSubscribers>(this.messageTypeSubscribers, (PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			bool flag = !Enumerable.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>(enumerable);
			if (flag)
			{
				string text = "No actions found for messageId: ";
				Guid messageId2 = messageId;
				Debug.LogError(text + messageId2.ToString());
			}
			else
			{
				MessageEventArgs messageEventArgs = new MessageEventArgs
				{
					playerId = playerId,
					data = data
				};
				foreach (PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers in enumerable)
				{
					messageTypeSubscribers.messageCallback.Invoke(messageEventArgs);
				}
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x00032414 File Offset: 0x00030614
		public UnityEvent<MessageEventArgs> AddAndCreate(Guid messageId)
		{
			PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers = Enumerable.SingleOrDefault<PlayerEditorConnectionEvents.MessageTypeSubscribers>(this.messageTypeSubscribers, (PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			bool flag = messageTypeSubscribers == null;
			if (flag)
			{
				messageTypeSubscribers = new PlayerEditorConnectionEvents.MessageTypeSubscribers
				{
					MessageTypeId = messageId,
					messageCallback = new PlayerEditorConnectionEvents.MessageEvent()
				};
				this.messageTypeSubscribers.Add(messageTypeSubscribers);
			}
			messageTypeSubscribers.subscriberCount++;
			return messageTypeSubscribers.messageCallback;
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x00032494 File Offset: 0x00030694
		public void UnregisterManagedCallback(Guid messageId, UnityAction<MessageEventArgs> callback)
		{
			PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers = Enumerable.SingleOrDefault<PlayerEditorConnectionEvents.MessageTypeSubscribers>(this.messageTypeSubscribers, (PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			bool flag = messageTypeSubscribers == null;
			if (!flag)
			{
				messageTypeSubscribers.subscriberCount--;
				messageTypeSubscribers.messageCallback.RemoveListener(callback);
				bool flag2 = messageTypeSubscribers.subscriberCount <= 0;
				if (flag2)
				{
					this.messageTypeSubscribers.Remove(messageTypeSubscribers);
				}
			}
		}

		// Token: 0x04000A27 RID: 2599
		[SerializeField]
		public List<PlayerEditorConnectionEvents.MessageTypeSubscribers> messageTypeSubscribers = new List<PlayerEditorConnectionEvents.MessageTypeSubscribers>();

		// Token: 0x04000A28 RID: 2600
		[SerializeField]
		public PlayerEditorConnectionEvents.ConnectionChangeEvent connectionEvent = new PlayerEditorConnectionEvents.ConnectionChangeEvent();

		// Token: 0x04000A29 RID: 2601
		[SerializeField]
		public PlayerEditorConnectionEvents.ConnectionChangeEvent disconnectionEvent = new PlayerEditorConnectionEvents.ConnectionChangeEvent();

		// Token: 0x0200038E RID: 910
		[Serializable]
		public class MessageEvent : UnityEvent<MessageEventArgs>
		{
		}

		// Token: 0x0200038F RID: 911
		[Serializable]
		public class ConnectionChangeEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000390 RID: 912
		[Serializable]
		public class MessageTypeSubscribers
		{
			// Token: 0x170005F0 RID: 1520
			// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x00032548 File Offset: 0x00030748
			// (set) Token: 0x06001EE6 RID: 7910 RVA: 0x00032565 File Offset: 0x00030765
			public Guid MessageTypeId
			{
				get
				{
					return new Guid(this.m_messageTypeId);
				}
				set
				{
					this.m_messageTypeId = value.ToString();
				}
			}

			// Token: 0x04000A2A RID: 2602
			[SerializeField]
			private string m_messageTypeId;

			// Token: 0x04000A2B RID: 2603
			public int subscriberCount = 0;

			// Token: 0x04000A2C RID: 2604
			public PlayerEditorConnectionEvents.MessageEvent messageCallback = new PlayerEditorConnectionEvents.MessageEvent();
		}
	}
}
