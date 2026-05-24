using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200020B RID: 523
	[NativeHeader("Runtime/BaseClasses/BitField.h")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeHeader("Runtime/BaseClasses/TagManager.h")]
	[NativeClass("BitField", "struct BitField;")]
	public struct LayerMask
	{
		// Token: 0x0600170A RID: 5898 RVA: 0x0002501C File Offset: 0x0002321C
		public static implicit operator int(LayerMask mask)
		{
			return mask.m_Mask;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00025034 File Offset: 0x00023234
		public static implicit operator LayerMask(int intVal)
		{
			LayerMask layerMask;
			layerMask.m_Mask = intVal;
			return layerMask;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x00025050 File Offset: 0x00023250
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x00025068 File Offset: 0x00023268
		public int value
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}

		// Token: 0x0600170E RID: 5902
		[StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
		[NativeMethod("LayerToString")]
		[MethodImpl(4096)]
		public static extern string LayerToName(int layer);

		// Token: 0x0600170F RID: 5903
		[NativeMethod("StringToLayer")]
		[StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
		[MethodImpl(4096)]
		public static extern int NameToLayer(string layerName);

		// Token: 0x06001710 RID: 5904 RVA: 0x00025074 File Offset: 0x00023274
		public static int GetMask(params string[] layerNames)
		{
			bool flag = layerNames == null;
			if (flag)
			{
				throw new ArgumentNullException("layerNames");
			}
			int num = 0;
			foreach (string text in layerNames)
			{
				int num2 = LayerMask.NameToLayer(text);
				bool flag2 = num2 != -1;
				if (flag2)
				{
					num |= 1 << num2;
				}
			}
			return num;
		}

		// Token: 0x040007F5 RID: 2037
		[NativeName("m_Bits")]
		private int m_Mask;
	}
}
