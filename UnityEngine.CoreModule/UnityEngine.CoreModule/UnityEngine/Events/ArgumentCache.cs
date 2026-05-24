using System;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	// Token: 0x020002B6 RID: 694
	[Serializable]
	internal class ArgumentCache : ISerializationCallbackReceiver
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x0002E8AC File Offset: 0x0002CAAC
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
		public Object unityObjectArgument
		{
			get
			{
				return this.m_ObjectArgument;
			}
			set
			{
				this.m_ObjectArgument = value;
				this.m_ObjectArgumentAssemblyTypeName = ((value != null) ? value.GetType().AssemblyQualifiedName : string.Empty);
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x0002E8F0 File Offset: 0x0002CAF0
		public string unityObjectArgumentAssemblyTypeName
		{
			get
			{
				return this.m_ObjectArgumentAssemblyTypeName;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x0002E908 File Offset: 0x0002CB08
		// (set) Token: 0x06001D15 RID: 7445 RVA: 0x0002E920 File Offset: 0x0002CB20
		public int intArgument
		{
			get
			{
				return this.m_IntArgument;
			}
			set
			{
				this.m_IntArgument = value;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x0002E92C File Offset: 0x0002CB2C
		// (set) Token: 0x06001D17 RID: 7447 RVA: 0x0002E944 File Offset: 0x0002CB44
		public float floatArgument
		{
			get
			{
				return this.m_FloatArgument;
			}
			set
			{
				this.m_FloatArgument = value;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x0002E950 File Offset: 0x0002CB50
		// (set) Token: 0x06001D19 RID: 7449 RVA: 0x0002E968 File Offset: 0x0002CB68
		public string stringArgument
		{
			get
			{
				return this.m_StringArgument;
			}
			set
			{
				this.m_StringArgument = value;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x0002E974 File Offset: 0x0002CB74
		// (set) Token: 0x06001D1B RID: 7451 RVA: 0x0002E98C File Offset: 0x0002CB8C
		public bool boolArgument
		{
			get
			{
				return this.m_BoolArgument;
			}
			set
			{
				this.m_BoolArgument = value;
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x0002E996 File Offset: 0x0002CB96
		public void OnBeforeSerialize()
		{
			this.m_ObjectArgumentAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_ObjectArgumentAssemblyTypeName);
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0002E996 File Offset: 0x0002CB96
		public void OnAfterDeserialize()
		{
			this.m_ObjectArgumentAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_ObjectArgumentAssemblyTypeName);
		}

		// Token: 0x0400098E RID: 2446
		[FormerlySerializedAs("objectArgument")]
		[SerializeField]
		private Object m_ObjectArgument;

		// Token: 0x0400098F RID: 2447
		[SerializeField]
		[FormerlySerializedAs("objectArgumentAssemblyTypeName")]
		private string m_ObjectArgumentAssemblyTypeName;

		// Token: 0x04000990 RID: 2448
		[FormerlySerializedAs("intArgument")]
		[SerializeField]
		private int m_IntArgument;

		// Token: 0x04000991 RID: 2449
		[FormerlySerializedAs("floatArgument")]
		[SerializeField]
		private float m_FloatArgument;

		// Token: 0x04000992 RID: 2450
		[FormerlySerializedAs("stringArgument")]
		[SerializeField]
		private string m_StringArgument;

		// Token: 0x04000993 RID: 2451
		[SerializeField]
		private bool m_BoolArgument;
	}
}
