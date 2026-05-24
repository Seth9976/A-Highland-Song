using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200004C RID: 76
	public static class InputManager
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000345 RID: 837 RVA: 0x0000B858 File Offset: 0x00009A58
		// (remove) Token: 0x06000346 RID: 838 RVA: 0x0000B88C File Offset: 0x00009A8C
		public static event Action OnSetup;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000347 RID: 839 RVA: 0x0000B8C0 File Offset: 0x00009AC0
		// (remove) Token: 0x06000348 RID: 840 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		public static event Action<ulong, float> OnUpdate;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000349 RID: 841 RVA: 0x0000B928 File Offset: 0x00009B28
		// (remove) Token: 0x0600034A RID: 842 RVA: 0x0000B95C File Offset: 0x00009B5C
		public static event Action OnReset;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600034B RID: 843 RVA: 0x0000B990 File Offset: 0x00009B90
		// (remove) Token: 0x0600034C RID: 844 RVA: 0x0000B9C4 File Offset: 0x00009BC4
		public static event Action<InputDevice> OnDeviceAttached;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600034D RID: 845 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		// (remove) Token: 0x0600034E RID: 846 RVA: 0x0000BA2C File Offset: 0x00009C2C
		public static event Action<InputDevice> OnDeviceDetached;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600034F RID: 847 RVA: 0x0000BA60 File Offset: 0x00009C60
		// (remove) Token: 0x06000350 RID: 848 RVA: 0x0000BA94 File Offset: 0x00009C94
		public static event Action<InputDevice> OnActiveDeviceChanged;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000351 RID: 849 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		// (remove) Token: 0x06000352 RID: 850 RVA: 0x0000BAFC File Offset: 0x00009CFC
		internal static event Action<ulong, float> OnUpdateDevices;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000353 RID: 851 RVA: 0x0000BB30 File Offset: 0x00009D30
		// (remove) Token: 0x06000354 RID: 852 RVA: 0x0000BB64 File Offset: 0x00009D64
		internal static event Action<ulong, float> OnCommitDevices;

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000BB97 File Offset: 0x00009D97
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000BB9E File Offset: 0x00009D9E
		public static bool CommandWasPressed { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000BBA6 File Offset: 0x00009DA6
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000BBAD File Offset: 0x00009DAD
		public static bool InvertYAxis { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000BBB5 File Offset: 0x00009DB5
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000BBBC File Offset: 0x00009DBC
		public static bool IsSetup { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000BBC4 File Offset: 0x00009DC4
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000BBCB File Offset: 0x00009DCB
		public static IMouseProvider MouseProvider { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000BBD3 File Offset: 0x00009DD3
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0000BBDA File Offset: 0x00009DDA
		public static IKeyboardProvider KeyboardProvider { get; private set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000BBE2 File Offset: 0x00009DE2
		// (set) Token: 0x06000360 RID: 864 RVA: 0x0000BBE9 File Offset: 0x00009DE9
		internal static string Platform { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000BBF1 File Offset: 0x00009DF1
		[Obsolete("Use InputManager.CommandWasPressed instead.")]
		public static bool MenuWasPressed
		{
			get
			{
				return InputManager.CommandWasPressed;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000BBF8 File Offset: 0x00009DF8
		internal static bool SetupInternal()
		{
			if (InputManager.IsSetup)
			{
				return false;
			}
			Logger.LogInfo("InControl (version " + InputManager.Version.ToString() + ")");
			InputManager.Platform = Utility.GetPlatformName(true);
			InputManager.enabled = true;
			InputManager.initialTime = 0f;
			InputManager.currentTime = 0f;
			InputManager.lastUpdateTime = 0f;
			InputManager.currentTick = 0UL;
			InputManager.applicationIsFocused = true;
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
			InputManager.devices.Clear();
			InputManager.Devices = InputManager.devices.AsReadOnly();
			InputManager.activeDevice = InputDevice.Null;
			InputManager.activeDevices.Clear();
			InputManager.ActiveDevices = InputManager.activeDevices.AsReadOnly();
			InputManager.playerActionSets.Clear();
			InputManager.MouseProvider = new UnityMouseProvider();
			InputManager.MouseProvider.Setup();
			InputManager.KeyboardProvider = new UnityKeyboardProvider();
			InputManager.KeyboardProvider.Setup();
			InputManager.IsSetup = true;
			bool flag = true;
			if (InputManager.EnableNativeInput && NativeInputDeviceManager.Enable())
			{
				Logger.LogInfo("[InControl] NativeInputDeviceManager enabled.");
				flag = false;
			}
			if (InputManager.EnableXInput && flag)
			{
				XInputDeviceManager.Enable();
				Logger.LogInfo("[InControl] XInputDeviceManager enabled.");
			}
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			if (flag)
			{
				InputManager.AddDeviceManager<UnityInputDeviceManager>();
				Logger.LogInfo("UnityInputDeviceManager enabled.");
			}
			return true;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000BD58 File Offset: 0x00009F58
		internal static void ResetInternal()
		{
			if (InputManager.OnReset != null)
			{
				InputManager.OnReset();
			}
			InputManager.OnSetup = null;
			InputManager.OnUpdate = null;
			InputManager.OnReset = null;
			InputManager.OnActiveDeviceChanged = null;
			InputManager.OnDeviceAttached = null;
			InputManager.OnDeviceDetached = null;
			InputManager.OnUpdateDevices = null;
			InputManager.OnCommitDevices = null;
			InputManager.DestroyDeviceManagers();
			InputManager.DestroyDevices();
			InputManager.playerActionSets.Clear();
			InputManager.MouseProvider.Reset();
			InputManager.KeyboardProvider.Reset();
			InputManager.IsSetup = false;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000BDD4 File Offset: 0x00009FD4
		public static void Update()
		{
			InputManager.UpdateInternal();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000BDDC File Offset: 0x00009FDC
		internal static void UpdateInternal()
		{
			InputManager.AssertIsSetup();
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			if (!InputManager.enabled)
			{
				return;
			}
			if (InputManager.SuspendInBackground && !InputManager.applicationIsFocused)
			{
				return;
			}
			InputManager.currentTick += 1UL;
			InputManager.UpdateCurrentTime();
			float num = InputManager.currentTime - InputManager.lastUpdateTime;
			InputManager.MouseProvider.Update();
			InputManager.KeyboardProvider.Update();
			InputManager.UpdateDeviceManagers(num);
			InputManager.CommandWasPressed = false;
			InputManager.UpdateDevices(num);
			InputManager.CommitDevices(num);
			InputDevice inputDevice = InputManager.ActiveDevice;
			InputManager.UpdateActiveDevice();
			InputManager.UpdatePlayerActionSets(num);
			if (inputDevice != InputManager.ActiveDevice && InputManager.OnActiveDeviceChanged != null)
			{
				InputManager.OnActiveDeviceChanged(InputManager.ActiveDevice);
			}
			if (InputManager.OnUpdate != null)
			{
				InputManager.OnUpdate(InputManager.currentTick, num);
			}
			InputManager.lastUpdateTime = InputManager.currentTime;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000BEB4 File Offset: 0x0000A0B4
		public static void Reload()
		{
			InputManager.ResetInternal();
			InputManager.SetupInternal();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000BEC1 File Offset: 0x0000A0C1
		private static void AssertIsSetup()
		{
			if (!InputManager.IsSetup)
			{
				throw new Exception("InputManager is not initialized. Call InputManager.Setup() first.");
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		private static void SetZeroTickOnAllControls()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				ReadOnlyCollection<InputControl> controls = InputManager.devices[i].Controls;
				int count2 = controls.Count;
				for (int j = 0; j < count2; j++)
				{
					InputControl inputControl = controls[j];
					if (inputControl != null)
					{
						inputControl.SetZeroTick();
					}
				}
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000BF3C File Offset: 0x0000A13C
		public static void ClearInputState()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.devices[i].ClearInputState();
			}
			int count2 = InputManager.playerActionSets.Count;
			for (int j = 0; j < count2; j++)
			{
				InputManager.playerActionSets[j].ClearInputState();
			}
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000BFA1 File Offset: 0x0000A1A1
		internal static void OnApplicationFocus(bool focusState)
		{
			if (!focusState)
			{
				if (InputManager.SuspendInBackground)
				{
					InputManager.ClearInputState();
				}
				InputManager.SetZeroTickOnAllControls();
			}
			InputManager.applicationIsFocused = focusState;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000BFBD File Offset: 0x0000A1BD
		internal static void OnApplicationPause(bool pauseState)
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000BFBF File Offset: 0x0000A1BF
		internal static void OnApplicationQuit()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000BFC6 File Offset: 0x0000A1C6
		internal static void OnLevelWasLoaded()
		{
			InputManager.SetZeroTickOnAllControls();
			InputManager.UpdateInternal();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public static void AddDeviceManager(InputDeviceManager deviceManager)
		{
			InputManager.AssertIsSetup();
			Type type = deviceManager.GetType();
			if (InputManager.deviceManagerTable.ContainsKey(type))
			{
				Logger.LogError("A device manager of type '" + type.Name + "' already exists; cannot add another.");
				return;
			}
			InputManager.deviceManagers.Add(deviceManager);
			InputManager.deviceManagerTable.Add(type, deviceManager);
			deviceManager.Update(InputManager.currentTick, InputManager.currentTime - InputManager.lastUpdateTime);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C042 File Offset: 0x0000A242
		public static void AddDeviceManager<T>() where T : InputDeviceManager, new()
		{
			InputManager.AddDeviceManager(new T());
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C054 File Offset: 0x0000A254
		public static T GetDeviceManager<T>() where T : InputDeviceManager
		{
			InputDeviceManager inputDeviceManager;
			if (InputManager.deviceManagerTable.TryGetValue(typeof(T), out inputDeviceManager))
			{
				return inputDeviceManager as T;
			}
			return default(T);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C08E File Offset: 0x0000A28E
		public static bool HasDeviceManager<T>() where T : InputDeviceManager
		{
			return InputManager.deviceManagerTable.ContainsKey(typeof(T));
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		private static void UpdateCurrentTime()
		{
			if (InputManager.initialTime < 1E-45f)
			{
				InputManager.initialTime = Time.realtimeSinceStartup;
			}
			InputManager.currentTime = Mathf.Max(0f, Time.realtimeSinceStartup - InputManager.initialTime);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		private static void UpdateDeviceManagers(float deltaTime)
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C114 File Offset: 0x0000A314
		private static void DestroyDeviceManagers()
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Destroy();
			}
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C15C File Offset: 0x0000A35C
		private static void DestroyDevices()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.devices[i].OnDetached();
			}
			InputManager.devices.Clear();
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		private static void UpdateDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.devices[i].Update(InputManager.currentTick, deltaTime);
			}
			if (InputManager.OnUpdateDevices != null)
			{
				InputManager.OnUpdateDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		private static void CommitDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Commit(InputManager.currentTick, deltaTime);
				if (inputDevice.CommandWasPressed)
				{
					InputManager.CommandWasPressed = true;
				}
			}
			if (InputManager.OnCommitDevices != null)
			{
				InputManager.OnCommitDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000C258 File Offset: 0x0000A458
		private static void UpdateActiveDevice()
		{
			InputManager.activeDevices.Clear();
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				if (inputDevice.LastInputAfter(InputManager.ActiveDevice) && !inputDevice.Passive)
				{
					InputManager.ActiveDevice = inputDevice;
				}
				if (inputDevice.IsActive)
				{
					InputManager.activeDevices.Add(inputDevice);
				}
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000C2C0 File Offset: 0x0000A4C0
		public static void AttachDevice(InputDevice inputDevice)
		{
			InputManager.AssertIsSetup();
			if (!inputDevice.IsSupportedOnThisPlatform)
			{
				return;
			}
			if (inputDevice.IsAttached)
			{
				return;
			}
			if (!InputManager.devices.Contains(inputDevice))
			{
				InputManager.devices.Add(inputDevice);
				InputManager.devices.Sort((InputDevice d1, InputDevice d2) => d1.SortOrder.CompareTo(d2.SortOrder));
			}
			inputDevice.OnAttached();
			if (InputManager.OnDeviceAttached != null)
			{
				InputManager.OnDeviceAttached(inputDevice);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C340 File Offset: 0x0000A540
		public static void DetachDevice(InputDevice inputDevice)
		{
			if (!InputManager.IsSetup)
			{
				return;
			}
			if (!inputDevice.IsAttached)
			{
				return;
			}
			InputManager.devices.Remove(inputDevice);
			if (InputManager.ActiveDevice == inputDevice)
			{
				InputManager.ActiveDevice = InputDevice.Null;
			}
			inputDevice.OnDetached();
			if (InputManager.OnDeviceDetached != null)
			{
				InputManager.OnDeviceDetached(inputDevice);
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000C394 File Offset: 0x0000A594
		public static void HideDevicesWithProfile(Type type)
		{
			if (type.IsSubclassOf(typeof(InputDeviceProfile)))
			{
				InputDeviceProfile.Hide(type);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000C3AE File Offset: 0x0000A5AE
		internal static void AttachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			if (!InputManager.playerActionSets.Contains(playerActionSet))
			{
				InputManager.playerActionSets.Add(playerActionSet);
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		internal static void DetachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			InputManager.playerActionSets.Remove(playerActionSet);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
		internal static void UpdatePlayerActionSets(float deltaTime)
		{
			int count = InputManager.playerActionSets.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.playerActionSets[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000C414 File Offset: 0x0000A614
		public static bool AnyKeyIsPressed
		{
			get
			{
				return KeyCombo.Detect(true).IncludeCount > 0;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000C432 File Offset: 0x0000A632
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000C442 File Offset: 0x0000A642
		public static InputDevice ActiveDevice
		{
			get
			{
				return InputManager.activeDevice ?? InputDevice.Null;
			}
			private set
			{
				InputManager.activeDevice = value ?? InputDevice.Null;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000C453 File Offset: 0x0000A653
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000C45A File Offset: 0x0000A65A
		public static bool Enabled
		{
			get
			{
				return InputManager.enabled;
			}
			set
			{
				if (InputManager.enabled != value)
				{
					if (value)
					{
						InputManager.SetZeroTickOnAllControls();
						InputManager.UpdateInternal();
					}
					else
					{
						InputManager.ClearInputState();
						InputManager.SetZeroTickOnAllControls();
					}
					InputManager.enabled = value;
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000C483 File Offset: 0x0000A683
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0000C48A File Offset: 0x0000A68A
		public static bool SuspendInBackground { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000C492 File Offset: 0x0000A692
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000C499 File Offset: 0x0000A699
		public static bool EnableNativeInput { get; internal set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000C4A1 File Offset: 0x0000A6A1
		// (set) Token: 0x06000389 RID: 905 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		public static bool EnableXInput { get; internal set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000C4B7 File Offset: 0x0000A6B7
		public static uint XInputUpdateRate { get; internal set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000C4BF File Offset: 0x0000A6BF
		// (set) Token: 0x0600038D RID: 909 RVA: 0x0000C4C6 File Offset: 0x0000A6C6
		public static uint XInputBufferSize { get; internal set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000C4CE File Offset: 0x0000A6CE
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000C4D5 File Offset: 0x0000A6D5
		public static bool NativeInputEnableXInput { get; internal set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000C4DD File Offset: 0x0000A6DD
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
		public static bool NativeInputEnableMFi { get; internal set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000C4EC File Offset: 0x0000A6EC
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000C4F3 File Offset: 0x0000A6F3
		public static bool NativeInputPreventSleep { get; internal set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000C4FB File Offset: 0x0000A6FB
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000C502 File Offset: 0x0000A702
		public static uint NativeInputUpdateRate { get; internal set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000C50A File Offset: 0x0000A70A
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000C511 File Offset: 0x0000A711
		public static bool EnableICade { get; internal set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000C519 File Offset: 0x0000A719
		internal static VersionInfo UnityVersion
		{
			get
			{
				if (InputManager.unityVersion == null)
				{
					InputManager.unityVersion = new VersionInfo?(VersionInfo.UnityVersion());
				}
				return InputManager.unityVersion.Value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000C540 File Offset: 0x0000A740
		public static ulong CurrentTick
		{
			get
			{
				return InputManager.currentTick;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000C547 File Offset: 0x0000A747
		public static float CurrentTime
		{
			get
			{
				return InputManager.currentTime;
			}
		}

		// Token: 0x04000358 RID: 856
		public static readonly VersionInfo Version = VersionInfo.InControlVersion();

		// Token: 0x04000361 RID: 865
		private static readonly List<InputDeviceManager> deviceManagers = new List<InputDeviceManager>();

		// Token: 0x04000362 RID: 866
		private static readonly Dictionary<Type, InputDeviceManager> deviceManagerTable = new Dictionary<Type, InputDeviceManager>();

		// Token: 0x04000363 RID: 867
		private static readonly List<InputDevice> devices = new List<InputDevice>();

		// Token: 0x04000364 RID: 868
		private static InputDevice activeDevice = InputDevice.Null;

		// Token: 0x04000365 RID: 869
		private static readonly List<InputDevice> activeDevices = new List<InputDevice>();

		// Token: 0x04000366 RID: 870
		private static readonly List<PlayerActionSet> playerActionSets = new List<PlayerActionSet>();

		// Token: 0x04000367 RID: 871
		public static ReadOnlyCollection<InputDevice> Devices;

		// Token: 0x04000368 RID: 872
		public static ReadOnlyCollection<InputDevice> ActiveDevices;

		// Token: 0x0400036F RID: 879
		private static bool applicationIsFocused;

		// Token: 0x04000370 RID: 880
		private static float initialTime;

		// Token: 0x04000371 RID: 881
		private static float currentTime;

		// Token: 0x04000372 RID: 882
		private static float lastUpdateTime;

		// Token: 0x04000373 RID: 883
		private static ulong currentTick;

		// Token: 0x04000374 RID: 884
		private static VersionInfo? unityVersion;

		// Token: 0x04000375 RID: 885
		private static bool enabled;
	}
}
