using System;
using System.Linq;

namespace UnityEngine.Recorder
{
	// Token: 0x02000002 RID: 2
	[ExecuteInEditMode]
	public class RecorderBindings : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void SetBindingValue(string id, Object value)
		{
			this.m_References.dictionary[id] = value;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		public Object GetBindingValue(string id)
		{
			Object @object;
			if (!this.m_References.dictionary.TryGetValue(id, out @object))
			{
				return null;
			}
			return @object;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002089 File Offset: 0x00000289
		public bool HasBindingValue(string id)
		{
			return this.m_References.dictionary.ContainsKey(id);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000209C File Offset: 0x0000029C
		public void RemoveBinding(string id)
		{
			if (this.m_References.dictionary.ContainsKey(id))
			{
				this.m_References.dictionary.Remove(id);
				this.MarkSceneDirty();
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020C9 File Offset: 0x000002C9
		public bool IsEmpty()
		{
			return this.m_References == null || !this.m_References.dictionary.Keys.Any<string>();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020ED File Offset: 0x000002ED
		public void DuplicateBinding(string src, string dst)
		{
			if (this.m_References.dictionary.ContainsKey(src))
			{
				this.m_References.dictionary[dst] = this.m_References.dictionary[src];
				this.MarkSceneDirty();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000212A File Offset: 0x0000032A
		private void MarkSceneDirty()
		{
		}

		// Token: 0x04000001 RID: 1
		[SerializeField]
		private RecorderBindings.PropertyObjects m_References = new RecorderBindings.PropertyObjects();

		// Token: 0x02000004 RID: 4
		[Serializable]
		private class PropertyObjects : SerializedDictionary<string, Object>
		{
		}
	}
}
