using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Android
{
	// Token: 0x02000014 RID: 20
	public class DownloadAssetPackAsyncOperation : CustomYieldInstruction
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00007A90 File Offset: 0x00005C90
		public override bool keepWaiting
		{
			get
			{
				Dictionary<string, AndroidAssetPackInfo> assetPackInfos = this.m_AssetPackInfos;
				bool flag3;
				lock (assetPackInfos)
				{
					foreach (AndroidAssetPackInfo androidAssetPackInfo in this.m_AssetPackInfos.Values)
					{
						bool flag = androidAssetPackInfo == null;
						if (flag)
						{
							return true;
						}
						bool flag2 = androidAssetPackInfo.status != AndroidAssetPackStatus.Canceled && androidAssetPackInfo.status != AndroidAssetPackStatus.Completed && androidAssetPackInfo.status != AndroidAssetPackStatus.Failed && androidAssetPackInfo.status > AndroidAssetPackStatus.Unknown;
						if (flag2)
						{
							return true;
						}
					}
					flag3 = false;
				}
				return flag3;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007B50 File Offset: 0x00005D50
		public bool isDone
		{
			get
			{
				return !this.keepWaiting;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007B5C File Offset: 0x00005D5C
		public float progress
		{
			get
			{
				Dictionary<string, AndroidAssetPackInfo> assetPackInfos = this.m_AssetPackInfos;
				float num4;
				lock (assetPackInfos)
				{
					float num = 0f;
					float num2 = 0f;
					foreach (AndroidAssetPackInfo androidAssetPackInfo in this.m_AssetPackInfos.Values)
					{
						bool flag = androidAssetPackInfo == null;
						if (!flag)
						{
							bool flag2 = androidAssetPackInfo.status == AndroidAssetPackStatus.Canceled || androidAssetPackInfo.status == AndroidAssetPackStatus.Completed || androidAssetPackInfo.status == AndroidAssetPackStatus.Failed || androidAssetPackInfo.status == AndroidAssetPackStatus.Unknown;
							if (flag2)
							{
								num += 1f;
								num2 += 1f;
							}
							else
							{
								double num3 = androidAssetPackInfo.bytesDownloaded / androidAssetPackInfo.size;
								num += (float)num3;
								num2 += androidAssetPackInfo.transferProgress;
							}
						}
					}
					num4 = Mathf.Clamp((num * 0.8f + num2 * 0.2f) / (float)this.m_AssetPackInfos.Count, 0f, 1f);
				}
				return num4;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007C94 File Offset: 0x00005E94
		public string[] downloadedAssetPacks
		{
			get
			{
				Dictionary<string, AndroidAssetPackInfo> assetPackInfos = this.m_AssetPackInfos;
				string[] array;
				lock (assetPackInfos)
				{
					List<string> list = new List<string>();
					foreach (AndroidAssetPackInfo androidAssetPackInfo in this.m_AssetPackInfos.Values)
					{
						bool flag = androidAssetPackInfo == null;
						if (!flag)
						{
							bool flag2 = androidAssetPackInfo.status == AndroidAssetPackStatus.Completed;
							if (flag2)
							{
								list.Add(androidAssetPackInfo.name);
							}
						}
					}
					array = list.ToArray();
				}
				return array;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00007D4C File Offset: 0x00005F4C
		public string[] downloadFailedAssetPacks
		{
			get
			{
				Dictionary<string, AndroidAssetPackInfo> assetPackInfos = this.m_AssetPackInfos;
				string[] array;
				lock (assetPackInfos)
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, AndroidAssetPackInfo> keyValuePair in this.m_AssetPackInfos)
					{
						AndroidAssetPackInfo value = keyValuePair.Value;
						bool flag = value == null;
						if (flag)
						{
							list.Add(keyValuePair.Key);
						}
						else
						{
							bool flag2 = value.status == AndroidAssetPackStatus.Canceled || value.status == AndroidAssetPackStatus.Failed || value.status == AndroidAssetPackStatus.Unknown;
							if (flag2)
							{
								list.Add(value.name);
							}
						}
					}
					array = list.ToArray();
				}
				return array;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007E30 File Offset: 0x00006030
		internal DownloadAssetPackAsyncOperation(string[] assetPackNames)
		{
			this.m_AssetPackInfos = Enumerable.ToDictionary<string, string, AndroidAssetPackInfo>(assetPackNames, (string name) => name, (string name) => null);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007E90 File Offset: 0x00006090
		internal void OnUpdate(AndroidAssetPackInfo info)
		{
			Dictionary<string, AndroidAssetPackInfo> assetPackInfos = this.m_AssetPackInfos;
			lock (assetPackInfos)
			{
				this.m_AssetPackInfos[info.name] = info;
			}
		}

		// Token: 0x04000040 RID: 64
		private Dictionary<string, AndroidAssetPackInfo> m_AssetPackInfos;
	}
}
