using System;
using System.Collections.Generic;

namespace UnityEngine.Recorder
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	internal class SerializedDictionary<TKey, TValue> : ISerializationCallbackReceiver
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000213F File Offset: 0x0000033F
		public Dictionary<TKey, TValue> dictionary
		{
			get
			{
				return this.m_Dictionary;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002148 File Offset: 0x00000348
		public void OnBeforeSerialize()
		{
			this.m_Keys.Clear();
			this.m_Values.Clear();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.m_Dictionary)
			{
				this.m_Keys.Add(keyValuePair.Key);
				this.m_Values.Add(keyValuePair.Value);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021D0 File Offset: 0x000003D0
		public void OnAfterDeserialize()
		{
			this.m_Dictionary.Clear();
			for (int i = 0; i < this.m_Keys.Count; i++)
			{
				this.m_Dictionary.Add(this.m_Keys[i], this.m_Values[i]);
			}
		}

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private List<TKey> m_Keys = new List<TKey>();

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private List<TValue> m_Values = new List<TValue>();

		// Token: 0x04000004 RID: 4
		private readonly Dictionary<TKey, TValue> m_Dictionary = new Dictionary<TKey, TValue>();
	}
}
