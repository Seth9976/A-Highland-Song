using System;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class CurrentVersionSO : ScriptableObject
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06001374 RID: 4980 RVA: 0x00088E84 File Offset: 0x00087084
	public static CurrentVersionSO Instance
	{
		get
		{
			if (CurrentVersionSO._Instance == null)
			{
				CurrentVersionSO._Instance = Resources.Load<CurrentVersionSO>(typeof(CurrentVersionSO).Name);
			}
			if (CurrentVersionSO._Instance == null)
			{
				Debug.LogWarning("No instance of " + typeof(CurrentVersionSO).Name + " found, using default values");
				CurrentVersionSO._Instance = ScriptableObject.CreateInstance<CurrentVersionSO>();
			}
			return CurrentVersionSO._Instance;
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x00088EF6 File Offset: 0x000870F6
	protected virtual void OnEnable()
	{
		if (CurrentVersionSO._Instance == null)
		{
			CurrentVersionSO._Instance = this;
		}
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x00088F0B File Offset: 0x0008710B
	protected virtual void OnDisable()
	{
		if (CurrentVersionSO._Instance == this)
		{
			CurrentVersionSO._Instance = null;
		}
	}

	// Token: 0x040012B3 RID: 4787
	private static CurrentVersionSO _Instance;

	// Token: 0x040012B4 RID: 4788
	public Version version;
}
