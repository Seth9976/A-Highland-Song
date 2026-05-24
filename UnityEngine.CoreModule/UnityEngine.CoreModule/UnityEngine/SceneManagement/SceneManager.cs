using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.SceneManagement
{
	// Token: 0x020002E4 RID: 740
	[NativeHeader("Runtime/Export/SceneManager/SceneManager.bindings.h")]
	[RequiredByNativeCode]
	public class SceneManager
	{
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001E30 RID: 7728
		public static extern int sceneCount
		{
			[NativeHeader("Runtime/SceneManager/SceneManager.h")]
			[NativeMethod("GetSceneCount")]
			[StaticAccessor("GetSceneManager()", StaticAccessorType.Dot)]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x00030FAC File Offset: 0x0002F1AC
		public static int sceneCountInBuildSettings
		{
			get
			{
				return SceneManagerAPI.ActiveAPI.GetNumScenesInBuildSettings();
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00030FC8 File Offset: 0x0002F1C8
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static Scene GetActiveScene()
		{
			Scene scene;
			SceneManager.GetActiveScene_Injected(out scene);
			return scene;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x00030FDD File Offset: 0x0002F1DD
		[NativeThrows]
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static bool SetActiveScene(Scene scene)
		{
			return SceneManager.SetActiveScene_Injected(ref scene);
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x00030FE8 File Offset: 0x0002F1E8
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static Scene GetSceneByPath(string scenePath)
		{
			Scene scene;
			SceneManager.GetSceneByPath_Injected(scenePath, out scene);
			return scene;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x00031000 File Offset: 0x0002F200
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static Scene GetSceneByName(string name)
		{
			Scene scene;
			SceneManager.GetSceneByName_Injected(name, out scene);
			return scene;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x00031018 File Offset: 0x0002F218
		public static Scene GetSceneByBuildIndex(int buildIndex)
		{
			return SceneManagerAPI.ActiveAPI.GetSceneByBuildIndex(buildIndex);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x00031038 File Offset: 0x0002F238
		[NativeThrows]
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static Scene GetSceneAt(int index)
		{
			Scene scene;
			SceneManager.GetSceneAt_Injected(index, out scene);
			return scene;
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x00031050 File Offset: 0x0002F250
		[NativeThrows]
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static Scene CreateScene([NotNull("ArgumentNullException")] string sceneName, CreateSceneParameters parameters)
		{
			Scene scene;
			SceneManager.CreateScene_Injected(sceneName, ref parameters, out scene);
			return scene;
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x00031068 File Offset: 0x0002F268
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		[NativeThrows]
		private static bool UnloadSceneInternal(Scene scene, UnloadSceneOptions options)
		{
			return SceneManager.UnloadSceneInternal_Injected(ref scene, options);
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x00031072 File Offset: 0x0002F272
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		[NativeThrows]
		private static AsyncOperation UnloadSceneAsyncInternal(Scene scene, UnloadSceneOptions options)
		{
			return SceneManager.UnloadSceneAsyncInternal_Injected(ref scene, options);
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0003107C File Offset: 0x0002F27C
		private static AsyncOperation LoadSceneAsyncNameIndexInternal(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
		{
			bool flag = !SceneManager.s_AllowLoadScene;
			AsyncOperation asyncOperation;
			if (flag)
			{
				asyncOperation = null;
			}
			else
			{
				asyncOperation = SceneManagerAPI.ActiveAPI.LoadSceneAsyncByNameOrIndex(sceneName, sceneBuildIndex, parameters, mustCompleteNextFrame);
			}
			return asyncOperation;
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000310AC File Offset: 0x0002F2AC
		private static AsyncOperation UnloadSceneNameIndexInternal(string sceneName, int sceneBuildIndex, bool immediately, UnloadSceneOptions options, out bool outSuccess)
		{
			bool flag = !SceneManager.s_AllowLoadScene;
			AsyncOperation asyncOperation;
			if (flag)
			{
				outSuccess = false;
				asyncOperation = null;
			}
			else
			{
				asyncOperation = SceneManagerAPI.ActiveAPI.UnloadSceneAsyncByNameOrIndex(sceneName, sceneBuildIndex, immediately, options, out outSuccess);
			}
			return asyncOperation;
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000310E3 File Offset: 0x0002F2E3
		[NativeThrows]
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		public static void MergeScenes(Scene sourceScene, Scene destinationScene)
		{
			SceneManager.MergeScenes_Injected(ref sourceScene, ref destinationScene);
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000310EE File Offset: 0x0002F2EE
		[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
		[NativeThrows]
		public static void MoveGameObjectToScene([NotNull("ArgumentNullException")] GameObject go, Scene scene)
		{
			SceneManager.MoveGameObjectToScene_Injected(go, ref scene);
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000310F8 File Offset: 0x0002F2F8
		[RequiredByNativeCode]
		internal static AsyncOperation LoadFirstScene_Internal(bool async)
		{
			return SceneManagerAPI.ActiveAPI.LoadFirstScene(async);
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001E40 RID: 7744 RVA: 0x00031118 File Offset: 0x0002F318
		// (remove) Token: 0x06001E41 RID: 7745 RVA: 0x0003114C File Offset: 0x0002F34C
		[field: DebuggerBrowsable(0)]
		public static event UnityAction<Scene, LoadSceneMode> sceneLoaded;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001E42 RID: 7746 RVA: 0x00031180 File Offset: 0x0002F380
		// (remove) Token: 0x06001E43 RID: 7747 RVA: 0x000311B4 File Offset: 0x0002F3B4
		[field: DebuggerBrowsable(0)]
		public static event UnityAction<Scene> sceneUnloaded;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001E44 RID: 7748 RVA: 0x000311E8 File Offset: 0x0002F3E8
		// (remove) Token: 0x06001E45 RID: 7749 RVA: 0x0003121C File Offset: 0x0002F41C
		[field: DebuggerBrowsable(0)]
		public static event UnityAction<Scene, Scene> activeSceneChanged;

		// Token: 0x06001E46 RID: 7750 RVA: 0x00031250 File Offset: 0x0002F450
		[Obsolete("Use SceneManager.sceneCount and SceneManager.GetSceneAt(int index) to loop the all scenes instead.")]
		public static Scene[] GetAllScenes()
		{
			Scene[] array = new Scene[SceneManager.sceneCount];
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				array[i] = SceneManager.GetSceneAt(i);
			}
			return array;
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00031294 File Offset: 0x0002F494
		public static Scene CreateScene(string sceneName)
		{
			CreateSceneParameters createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.None);
			return SceneManager.CreateScene(sceneName, createSceneParameters);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000312B8 File Offset: 0x0002F4B8
		public static void LoadScene(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(mode);
			SceneManager.LoadScene(sceneName, loadSceneParameters);
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000312D8 File Offset: 0x0002F4D8
		[ExcludeFromDocs]
		public static void LoadScene(string sceneName)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Single);
			SceneManager.LoadScene(sceneName, loadSceneParameters);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000312F8 File Offset: 0x0002F4F8
		public static Scene LoadScene(string sceneName, LoadSceneParameters parameters)
		{
			SceneManager.LoadSceneAsyncNameIndexInternal(sceneName, -1, parameters, true);
			return SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x00031320 File Offset: 0x0002F520
		public static void LoadScene(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(mode);
			SceneManager.LoadScene(sceneBuildIndex, loadSceneParameters);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00031340 File Offset: 0x0002F540
		[ExcludeFromDocs]
		public static void LoadScene(int sceneBuildIndex)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Single);
			SceneManager.LoadScene(sceneBuildIndex, loadSceneParameters);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x00031360 File Offset: 0x0002F560
		public static Scene LoadScene(int sceneBuildIndex, LoadSceneParameters parameters)
		{
			SceneManager.LoadSceneAsyncNameIndexInternal(null, sceneBuildIndex, parameters, true);
			return SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00031388 File Offset: 0x0002F588
		public static AsyncOperation LoadSceneAsync(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(mode);
			return SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneParameters);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000313AC File Offset: 0x0002F5AC
		[ExcludeFromDocs]
		public static AsyncOperation LoadSceneAsync(int sceneBuildIndex)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Single);
			return SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneParameters);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000313D0 File Offset: 0x0002F5D0
		public static AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneParameters parameters)
		{
			return SceneManager.LoadSceneAsyncNameIndexInternal(null, sceneBuildIndex, parameters, false);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000313EC File Offset: 0x0002F5EC
		public static AsyncOperation LoadSceneAsync(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(mode);
			return SceneManager.LoadSceneAsync(sceneName, loadSceneParameters);
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00031410 File Offset: 0x0002F610
		[ExcludeFromDocs]
		public static AsyncOperation LoadSceneAsync(string sceneName)
		{
			LoadSceneParameters loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Single);
			return SceneManager.LoadSceneAsync(sceneName, loadSceneParameters);
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00031434 File Offset: 0x0002F634
		public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneParameters parameters)
		{
			return SceneManager.LoadSceneAsyncNameIndexInternal(sceneName, -1, parameters, false);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x00031450 File Offset: 0x0002F650
		[Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
		public static bool UnloadScene(Scene scene)
		{
			return SceneManager.UnloadSceneInternal(scene, UnloadSceneOptions.None);
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0003146C File Offset: 0x0002F66C
		[Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
		public static bool UnloadScene(int sceneBuildIndex)
		{
			bool flag;
			SceneManager.UnloadSceneNameIndexInternal("", sceneBuildIndex, true, UnloadSceneOptions.None, out flag);
			return flag;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x00031490 File Offset: 0x0002F690
		[Obsolete("Use SceneManager.UnloadSceneAsync. This function is not safe to use during triggers and under other circumstances. See Scripting reference for more details.")]
		public static bool UnloadScene(string sceneName)
		{
			bool flag;
			SceneManager.UnloadSceneNameIndexInternal(sceneName, -1, true, UnloadSceneOptions.None, out flag);
			return flag;
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000314B0 File Offset: 0x0002F6B0
		public static AsyncOperation UnloadSceneAsync(int sceneBuildIndex)
		{
			bool flag;
			return SceneManager.UnloadSceneNameIndexInternal("", sceneBuildIndex, false, UnloadSceneOptions.None, out flag);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000314D4 File Offset: 0x0002F6D4
		public static AsyncOperation UnloadSceneAsync(string sceneName)
		{
			bool flag;
			return SceneManager.UnloadSceneNameIndexInternal(sceneName, -1, false, UnloadSceneOptions.None, out flag);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000314F4 File Offset: 0x0002F6F4
		public static AsyncOperation UnloadSceneAsync(Scene scene)
		{
			return SceneManager.UnloadSceneAsyncInternal(scene, UnloadSceneOptions.None);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x00031510 File Offset: 0x0002F710
		public static AsyncOperation UnloadSceneAsync(int sceneBuildIndex, UnloadSceneOptions options)
		{
			bool flag;
			return SceneManager.UnloadSceneNameIndexInternal("", sceneBuildIndex, false, options, out flag);
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00031534 File Offset: 0x0002F734
		public static AsyncOperation UnloadSceneAsync(string sceneName, UnloadSceneOptions options)
		{
			bool flag;
			return SceneManager.UnloadSceneNameIndexInternal(sceneName, -1, false, options, out flag);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00031554 File Offset: 0x0002F754
		public static AsyncOperation UnloadSceneAsync(Scene scene, UnloadSceneOptions options)
		{
			return SceneManager.UnloadSceneAsyncInternal(scene, options);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x00031570 File Offset: 0x0002F770
		[RequiredByNativeCode]
		private static void Internal_SceneLoaded(Scene scene, LoadSceneMode mode)
		{
			bool flag = SceneManager.sceneLoaded != null;
			if (flag)
			{
				SceneManager.sceneLoaded(scene, mode);
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0003159C File Offset: 0x0002F79C
		[RequiredByNativeCode]
		private static void Internal_SceneUnloaded(Scene scene)
		{
			bool flag = SceneManager.sceneUnloaded != null;
			if (flag)
			{
				SceneManager.sceneUnloaded(scene);
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000315C4 File Offset: 0x0002F7C4
		[RequiredByNativeCode]
		private static void Internal_ActiveSceneChanged(Scene previousActiveScene, Scene newActiveScene)
		{
			bool flag = SceneManager.activeSceneChanged != null;
			if (flag)
			{
				SceneManager.activeSceneChanged(previousActiveScene, newActiveScene);
			}
		}

		// Token: 0x06001E62 RID: 7778
		[MethodImpl(4096)]
		private static extern void GetActiveScene_Injected(out Scene ret);

		// Token: 0x06001E63 RID: 7779
		[MethodImpl(4096)]
		private static extern bool SetActiveScene_Injected(ref Scene scene);

		// Token: 0x06001E64 RID: 7780
		[MethodImpl(4096)]
		private static extern void GetSceneByPath_Injected(string scenePath, out Scene ret);

		// Token: 0x06001E65 RID: 7781
		[MethodImpl(4096)]
		private static extern void GetSceneByName_Injected(string name, out Scene ret);

		// Token: 0x06001E66 RID: 7782
		[MethodImpl(4096)]
		private static extern void GetSceneAt_Injected(int index, out Scene ret);

		// Token: 0x06001E67 RID: 7783
		[MethodImpl(4096)]
		private static extern void CreateScene_Injected(string sceneName, ref CreateSceneParameters parameters, out Scene ret);

		// Token: 0x06001E68 RID: 7784
		[MethodImpl(4096)]
		private static extern bool UnloadSceneInternal_Injected(ref Scene scene, UnloadSceneOptions options);

		// Token: 0x06001E69 RID: 7785
		[MethodImpl(4096)]
		private static extern AsyncOperation UnloadSceneAsyncInternal_Injected(ref Scene scene, UnloadSceneOptions options);

		// Token: 0x06001E6A RID: 7786
		[MethodImpl(4096)]
		private static extern void MergeScenes_Injected(ref Scene sourceScene, ref Scene destinationScene);

		// Token: 0x06001E6B RID: 7787
		[MethodImpl(4096)]
		private static extern void MoveGameObjectToScene_Injected(GameObject go, ref Scene scene);

		// Token: 0x040009E3 RID: 2531
		internal static bool s_AllowLoadScene = true;
	}
}
