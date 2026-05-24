using System;

namespace UnityEngine.Android
{
	// Token: 0x02000017 RID: 23
	public class RequestToUseMobileDataAsyncOperation : CustomYieldInstruction
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00008014 File Offset: 0x00006214
		public override bool keepWaiting
		{
			get
			{
				object operationLock = this.m_OperationLock;
				bool flag;
				lock (operationLock)
				{
					flag = this.m_RequestResult == null;
				}
				return flag;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007B50 File Offset: 0x00005D50
		public bool isDone
		{
			get
			{
				return !this.keepWaiting;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00008058 File Offset: 0x00006258
		public AndroidAssetPackUseMobileDataRequestResult result
		{
			get
			{
				object operationLock = this.m_OperationLock;
				AndroidAssetPackUseMobileDataRequestResult requestResult;
				lock (operationLock)
				{
					requestResult = this.m_RequestResult;
				}
				return requestResult;
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008098 File Offset: 0x00006298
		internal RequestToUseMobileDataAsyncOperation()
		{
			this.m_OperationLock = new object();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000080B0 File Offset: 0x000062B0
		internal void OnResult(AndroidAssetPackUseMobileDataRequestResult result)
		{
			object operationLock = this.m_OperationLock;
			lock (operationLock)
			{
				this.m_RequestResult = result;
			}
		}

		// Token: 0x04000047 RID: 71
		private AndroidAssetPackUseMobileDataRequestResult m_RequestResult;

		// Token: 0x04000048 RID: 72
		private readonly object m_OperationLock;
	}
}
