using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public abstract class MonoSingleton<T> : MonoBehaviour, ISerializationCallbackReceiver where T : MonoSingleton<T>
{
	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x00073914 File Offset: 0x00071B14
	public static T instance
	{
		get
		{
			if (!MonoSingleton<T>._hasInstance)
			{
				MonoSingleton<T>._instance = (T)((object)Object.FindObjectOfType(typeof(T), true));
				if (MonoSingleton<T>._instance != null)
				{
					MonoSingleton<T>._hasInstance = true;
				}
			}
			return MonoSingleton<T>._instance;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x00073954 File Offset: 0x00071B54
	public static bool isInstantiated
	{
		get
		{
			return MonoSingleton<T>._hasInstance && MonoSingleton<T>._instance != null;
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0007396F File Offset: 0x00071B6F
	protected virtual void OnDestroy()
	{
		if (MonoSingleton<T>._instance == this)
		{
			MonoSingleton<T>._instance = default(T);
			MonoSingleton<T>._hasInstance = false;
		}
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00073994 File Offset: 0x00071B94
	public void OnAfterDeserialize()
	{
		MonoSingleton<T>._instance = (T)((object)this);
		MonoSingleton<T>._hasInstance = true;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x000739A7 File Offset: 0x00071BA7
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x040011AD RID: 4525
	private static T _instance;

	// Token: 0x040011AE RID: 4526
	private static bool _hasInstance;
}
