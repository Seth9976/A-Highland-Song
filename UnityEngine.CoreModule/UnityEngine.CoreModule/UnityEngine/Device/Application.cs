using System;
using UnityEngine.Events;

namespace UnityEngine.Device
{
	// Token: 0x0200044F RID: 1103
	public static class Application
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x00040DE8 File Offset: 0x0003EFE8
		public static string absoluteURL
		{
			get
			{
				return Application.absoluteURL;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x00040DEF File Offset: 0x0003EFEF
		// (set) Token: 0x060026E5 RID: 9957 RVA: 0x00040DF6 File Offset: 0x0003EFF6
		public static ThreadPriority backgroundLoadingPriority
		{
			get
			{
				return Application.backgroundLoadingPriority;
			}
			set
			{
				Application.backgroundLoadingPriority = value;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x00040DFF File Offset: 0x0003EFFF
		public static string buildGUID
		{
			get
			{
				return Application.buildGUID;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x00040E06 File Offset: 0x0003F006
		public static string cloudProjectId
		{
			get
			{
				return Application.cloudProjectId;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x00040E0D File Offset: 0x0003F00D
		public static string companyName
		{
			get
			{
				return Application.companyName;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x00040E14 File Offset: 0x0003F014
		public static string consoleLogPath
		{
			get
			{
				return Application.consoleLogPath;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x00040E1B File Offset: 0x0003F01B
		public static string dataPath
		{
			get
			{
				return Application.dataPath;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x00040E22 File Offset: 0x0003F022
		public static bool genuine
		{
			get
			{
				return Application.genuine;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x00040E29 File Offset: 0x0003F029
		public static bool genuineCheckAvailable
		{
			get
			{
				return Application.genuineCheckAvailable;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x00040E30 File Offset: 0x0003F030
		public static string identifier
		{
			get
			{
				return Application.identifier;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x00040E37 File Offset: 0x0003F037
		public static string installerName
		{
			get
			{
				return Application.installerName;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x00040E3E File Offset: 0x0003F03E
		public static ApplicationInstallMode installMode
		{
			get
			{
				return Application.installMode;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x00040E45 File Offset: 0x0003F045
		public static NetworkReachability internetReachability
		{
			get
			{
				return Application.internetReachability;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x00040E4C File Offset: 0x0003F04C
		public static bool isBatchMode
		{
			get
			{
				return Application.isBatchMode;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x00040E53 File Offset: 0x0003F053
		public static bool isConsolePlatform
		{
			get
			{
				return Application.isConsolePlatform;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x00040E5A File Offset: 0x0003F05A
		public static bool isEditor
		{
			get
			{
				return Application.isEditor;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x00040E61 File Offset: 0x0003F061
		public static bool isFocused
		{
			get
			{
				return Application.isFocused;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x00040E68 File Offset: 0x0003F068
		public static bool isMobilePlatform
		{
			get
			{
				return Application.isMobilePlatform;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x00040E6F File Offset: 0x0003F06F
		public static bool isPlaying
		{
			get
			{
				return Application.isPlaying;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x00040E76 File Offset: 0x0003F076
		public static string persistentDataPath
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x00040E7D File Offset: 0x0003F07D
		public static RuntimePlatform platform
		{
			get
			{
				return Application.platform;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x00040E84 File Offset: 0x0003F084
		public static string productName
		{
			get
			{
				return Application.productName;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060026FA RID: 9978 RVA: 0x00040E8B File Offset: 0x0003F08B
		// (set) Token: 0x060026FB RID: 9979 RVA: 0x00040E92 File Offset: 0x0003F092
		public static bool runInBackground
		{
			get
			{
				return Application.runInBackground;
			}
			set
			{
				Application.runInBackground = value;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060026FC RID: 9980 RVA: 0x00040E9B File Offset: 0x0003F09B
		public static ApplicationSandboxType sandboxType
		{
			get
			{
				return Application.sandboxType;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x00040EA2 File Offset: 0x0003F0A2
		public static string streamingAssetsPath
		{
			get
			{
				return Application.streamingAssetsPath;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060026FE RID: 9982 RVA: 0x00040EA9 File Offset: 0x0003F0A9
		public static SystemLanguage systemLanguage
		{
			get
			{
				return Application.systemLanguage;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x00040EB0 File Offset: 0x0003F0B0
		// (set) Token: 0x06002700 RID: 9984 RVA: 0x00040EB7 File Offset: 0x0003F0B7
		public static int targetFrameRate
		{
			get
			{
				return Application.targetFrameRate;
			}
			set
			{
				Application.targetFrameRate = value;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x00040EC0 File Offset: 0x0003F0C0
		public static string temporaryCachePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002702 RID: 9986 RVA: 0x00040EC7 File Offset: 0x0003F0C7
		public static string unityVersion
		{
			get
			{
				return Application.unityVersion;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x00040ECE File Offset: 0x0003F0CE
		public static string version
		{
			get
			{
				return Application.version;
			}
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06002704 RID: 9988 RVA: 0x00040ED5 File Offset: 0x0003F0D5
		// (remove) Token: 0x06002705 RID: 9989 RVA: 0x00040EDE File Offset: 0x0003F0DE
		public static event Action<string> deepLinkActivated
		{
			add
			{
				Application.deepLinkActivated += value;
			}
			remove
			{
				Application.deepLinkActivated -= value;
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06002706 RID: 9990 RVA: 0x00040EE7 File Offset: 0x0003F0E7
		// (remove) Token: 0x06002707 RID: 9991 RVA: 0x00040EF0 File Offset: 0x0003F0F0
		public static event Action<bool> focusChanged
		{
			add
			{
				Application.focusChanged += value;
			}
			remove
			{
				Application.focusChanged -= value;
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06002708 RID: 9992 RVA: 0x00040EF9 File Offset: 0x0003F0F9
		// (remove) Token: 0x06002709 RID: 9993 RVA: 0x00040F02 File Offset: 0x0003F102
		public static event Application.LogCallback logMessageReceived
		{
			add
			{
				Application.logMessageReceived += value;
			}
			remove
			{
				Application.logMessageReceived -= value;
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600270A RID: 9994 RVA: 0x00040F0B File Offset: 0x0003F10B
		// (remove) Token: 0x0600270B RID: 9995 RVA: 0x00040F14 File Offset: 0x0003F114
		public static event Application.LogCallback logMessageReceivedThreaded
		{
			add
			{
				Application.logMessageReceivedThreaded += value;
			}
			remove
			{
				Application.logMessageReceivedThreaded -= value;
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600270C RID: 9996 RVA: 0x00040F1D File Offset: 0x0003F11D
		// (remove) Token: 0x0600270D RID: 9997 RVA: 0x00040F26 File Offset: 0x0003F126
		public static event Application.LowMemoryCallback lowMemory
		{
			add
			{
				Application.lowMemory += value;
			}
			remove
			{
				Application.lowMemory -= value;
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600270E RID: 9998 RVA: 0x00040F2F File Offset: 0x0003F12F
		// (remove) Token: 0x0600270F RID: 9999 RVA: 0x00040F38 File Offset: 0x0003F138
		public static event UnityAction onBeforeRender
		{
			add
			{
				Application.onBeforeRender += value;
			}
			remove
			{
				Application.onBeforeRender -= value;
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06002710 RID: 10000 RVA: 0x00040F41 File Offset: 0x0003F141
		// (remove) Token: 0x06002711 RID: 10001 RVA: 0x00040F4A File Offset: 0x0003F14A
		public static event Action quitting
		{
			add
			{
				Application.quitting += value;
			}
			remove
			{
				Application.quitting -= value;
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06002712 RID: 10002 RVA: 0x00040F53 File Offset: 0x0003F153
		// (remove) Token: 0x06002713 RID: 10003 RVA: 0x00040F5C File Offset: 0x0003F15C
		public static event Func<bool> wantsToQuit
		{
			add
			{
				Application.wantsToQuit += value;
			}
			remove
			{
				Application.wantsToQuit -= value;
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06002714 RID: 10004 RVA: 0x00040F65 File Offset: 0x0003F165
		// (remove) Token: 0x06002715 RID: 10005 RVA: 0x00040F6E File Offset: 0x0003F16E
		public static event Action unloading
		{
			add
			{
				Application.unloading += value;
			}
			remove
			{
				Application.unloading -= value;
			}
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x00040F78 File Offset: 0x0003F178
		public static bool CanStreamedLevelBeLoaded(int levelIndex)
		{
			return Application.CanStreamedLevelBeLoaded(levelIndex);
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x00040F90 File Offset: 0x0003F190
		public static bool CanStreamedLevelBeLoaded(string levelName)
		{
			return Application.CanStreamedLevelBeLoaded(levelName);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x00040FA8 File Offset: 0x0003F1A8
		public static string[] GetBuildTags()
		{
			return Application.GetBuildTags();
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x00040FC0 File Offset: 0x0003F1C0
		public static StackTraceLogType GetStackTraceLogType(LogType logType)
		{
			return Application.GetStackTraceLogType(logType);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x00040FD8 File Offset: 0x0003F1D8
		public static bool HasProLicense()
		{
			return Application.HasProLicense();
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x00040FF0 File Offset: 0x0003F1F0
		public static bool HasUserAuthorization(UserAuthorization mode)
		{
			return Application.HasUserAuthorization(mode);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x00041008 File Offset: 0x0003F208
		public static bool IsPlaying(Object obj)
		{
			return Application.IsPlaying(obj);
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x00041020 File Offset: 0x0003F220
		public static void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0004102A File Offset: 0x0003F22A
		public static void Quit()
		{
			Application.Quit();
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x00041033 File Offset: 0x0003F233
		public static void Quit(int exitCode)
		{
			Application.Quit(exitCode);
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x00041040 File Offset: 0x0003F240
		public static bool RequestAdvertisingIdentifierAsync(Application.AdvertisingIdentifierCallback delegateMethod)
		{
			return Application.RequestAdvertisingIdentifierAsync(delegateMethod);
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x00041058 File Offset: 0x0003F258
		public static AsyncOperation RequestUserAuthorization(UserAuthorization mode)
		{
			return Application.RequestUserAuthorization(mode);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x00041070 File Offset: 0x0003F270
		public static void SetBuildTags(string[] buildTags)
		{
			Application.SetBuildTags(buildTags);
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x0004107A File Offset: 0x0003F27A
		public static void SetStackTraceLogType(LogType logType, StackTraceLogType stackTraceType)
		{
			Application.SetStackTraceLogType(logType, stackTraceType);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x00041085 File Offset: 0x0003F285
		public static void Unload()
		{
			Application.Unload();
		}
	}
}
