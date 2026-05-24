using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000236 RID: 566
	[RequiredByNativeCode]
	public interface ISerializationCallbackReceiver
	{
		// Token: 0x060017FB RID: 6139
		[RequiredByNativeCode]
		void OnBeforeSerialize();

		// Token: 0x060017FC RID: 6140
		[RequiredByNativeCode]
		void OnAfterDeserialize();
	}
}
