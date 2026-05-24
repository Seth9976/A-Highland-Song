using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x020000EC RID: 236
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060007BC RID: 1980 RVA: 0x000455DE File Offset: 0x000437DE
	protected static SteamManager Instance
	{
		get
		{
			if (!MonoSingleton<BuildSetupManager>.instance.setup.steamEnabled)
			{
				return null;
			}
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x00045618 File Offset: 0x00043818
	public static bool Initialized
	{
		get
		{
			SteamManager instance = SteamManager.Instance;
			return instance != null && instance.m_bInitialized;
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0004563C File Offset: 0x0004383C
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00045644 File Offset: 0x00043844
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00045654 File Offset: 0x00043854
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(new AppId_t(1240060U)))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string text = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00045738 File Offset: 0x00043938
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00045786 File Offset: 0x00043986
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000457AA File Offset: 0x000439AA
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x040009A4 RID: 2468
	protected static bool s_EverInitialized;

	// Token: 0x040009A5 RID: 2469
	protected static SteamManager s_instance;

	// Token: 0x040009A6 RID: 2470
	protected bool m_bInitialized;

	// Token: 0x040009A7 RID: 2471
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
