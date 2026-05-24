using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200006A RID: 106
	public class UnityInputDeviceManager : InputDeviceManager
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x0001253F File Offset: 0x0001073F
		public UnityInputDeviceManager()
		{
			this.systemDeviceProfiles = new List<InputDeviceProfile>(UnityInputDeviceProfileList.Profiles.Length);
			this.customDeviceProfiles = new List<InputDeviceProfile>();
			this.AddSystemDeviceProfiles();
			this.QueryJoystickInfo();
			this.AttachDevices();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00012578 File Offset: 0x00010778
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.deviceRefreshTimer += deltaTime;
			if (this.deviceRefreshTimer >= 1f)
			{
				this.deviceRefreshTimer = 0f;
				this.QueryJoystickInfo();
				if (this.JoystickInfoHasChanged)
				{
					Logger.LogInfo("Change in attached Unity joysticks detected; refreshing device list.");
					this.DetachDevices();
					this.AttachDevices();
				}
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000125D0 File Offset: 0x000107D0
		private void QueryJoystickInfo()
		{
			this.joystickNames = Input.GetJoystickNames();
			this.joystickCount = this.joystickNames.Length;
			this.joystickHash = 527 + this.joystickCount;
			for (int i = 0; i < this.joystickCount; i++)
			{
				this.joystickHash = this.joystickHash * 31 + this.joystickNames[i].GetHashCode();
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00012636 File Offset: 0x00010836
		private bool JoystickInfoHasChanged
		{
			get
			{
				return this.joystickHash != this.lastJoystickHash || this.joystickCount != this.lastJoystickCount;
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001265C File Offset: 0x0001085C
		private void AttachDevices()
		{
			try
			{
				for (int i = 0; i < this.joystickCount; i++)
				{
					this.DetectJoystickDevice(i + 1, this.joystickNames[i]);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				Logger.LogError(ex.StackTrace);
			}
			this.lastJoystickCount = this.joystickCount;
			this.lastJoystickHash = this.joystickHash;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000126CC File Offset: 0x000108CC
		private void DetachDevices()
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.DetachDevice(this.devices[i]);
			}
			this.devices.Clear();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001270D File Offset: 0x0001090D
		public void ReloadDevices()
		{
			this.QueryJoystickInfo();
			this.DetachDevices();
			this.AttachDevices();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00012721 File Offset: 0x00010921
		private void AttachDevice(UnityInputDevice device)
		{
			this.devices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00012738 File Offset: 0x00010938
		private bool HasAttachedDeviceWithJoystickId(int unityJoystickId)
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				UnityInputDevice unityInputDevice = this.devices[i] as UnityInputDevice;
				if (unityInputDevice != null && unityInputDevice.JoystickId == unityJoystickId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00012780 File Offset: 0x00010980
		private void DetectJoystickDevice(int unityJoystickId, string unityJoystickName)
		{
			if (this.HasAttachedDeviceWithJoystickId(unityJoystickId))
			{
				return;
			}
			if (unityJoystickName.IndexOf("webcam", StringComparison.OrdinalIgnoreCase) != -1)
			{
				return;
			}
			if (InputManager.UnityVersion < new VersionInfo(4, 5, 0, 0) && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) && unityJoystickName == "Unknown Wireless Controller")
			{
				return;
			}
			if (InputManager.UnityVersion >= new VersionInfo(4, 6, 3, 0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && string.IsNullOrEmpty(unityJoystickName))
			{
				return;
			}
			InputDeviceProfile inputDeviceProfile = this.DetectDevice(unityJoystickName);
			if (inputDeviceProfile == null)
			{
				UnityInputDevice unityInputDevice = new UnityInputDevice(unityJoystickId, unityJoystickName);
				this.AttachDevice(unityInputDevice);
				Logger.LogWarning(string.Concat(new string[]
				{
					"Device ",
					unityJoystickId.ToString(),
					" with name \"",
					unityJoystickName,
					"\" does not match any supported profiles and will be considered an unknown controller."
				}));
				return;
			}
			if (!inputDeviceProfile.IsHidden)
			{
				UnityInputDevice unityInputDevice2 = new UnityInputDevice(inputDeviceProfile, unityJoystickId, unityJoystickName);
				this.AttachDevice(unityInputDevice2);
				Logger.LogInfo(string.Concat(new string[]
				{
					"Device ",
					unityJoystickId.ToString(),
					" matched profile ",
					inputDeviceProfile.GetType().Name,
					" (",
					inputDeviceProfile.DeviceName,
					")"
				}));
				return;
			}
			Logger.LogInfo(string.Concat(new string[]
			{
				"Device ",
				unityJoystickId.ToString(),
				" matching profile ",
				inputDeviceProfile.GetType().Name,
				" (",
				inputDeviceProfile.DeviceName,
				") is hidden and will not be attached."
			}));
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00012914 File Offset: 0x00010B14
		private InputDeviceProfile DetectDevice(string unityJoystickName)
		{
			InputDeviceProfile inputDeviceProfile = null;
			InputDeviceInfo deviceInfo = new InputDeviceInfo
			{
				name = unityJoystickName
			};
			return (((inputDeviceProfile ?? this.customDeviceProfiles.Find((InputDeviceProfile profile) => profile.Matches(deviceInfo))) ?? this.systemDeviceProfiles.Find((InputDeviceProfile profile) => profile.Matches(deviceInfo))) ?? this.customDeviceProfiles.Find((InputDeviceProfile profile) => profile.LastResortMatches(deviceInfo))) ?? this.systemDeviceProfiles.Find((InputDeviceProfile profile) => profile.LastResortMatches(deviceInfo));
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000129AB File Offset: 0x00010BAB
		private void AddSystemDeviceProfile(InputDeviceProfile deviceProfile)
		{
			if (deviceProfile != null && deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000129C4 File Offset: 0x00010BC4
		private void AddSystemDeviceProfiles()
		{
			for (int i = 0; i < UnityInputDeviceProfileList.Profiles.Length; i++)
			{
				InputDeviceProfile inputDeviceProfile = InputDeviceProfile.CreateInstanceOfType(UnityInputDeviceProfileList.Profiles[i]);
				this.AddSystemDeviceProfile(inputDeviceProfile);
			}
		}

		// Token: 0x04000460 RID: 1120
		private const float deviceRefreshInterval = 1f;

		// Token: 0x04000461 RID: 1121
		private float deviceRefreshTimer;

		// Token: 0x04000462 RID: 1122
		private readonly List<InputDeviceProfile> systemDeviceProfiles;

		// Token: 0x04000463 RID: 1123
		private readonly List<InputDeviceProfile> customDeviceProfiles;

		// Token: 0x04000464 RID: 1124
		private string[] joystickNames;

		// Token: 0x04000465 RID: 1125
		private int lastJoystickCount;

		// Token: 0x04000466 RID: 1126
		private int lastJoystickHash;

		// Token: 0x04000467 RID: 1127
		private int joystickCount;

		// Token: 0x04000468 RID: 1128
		private int joystickHash;
	}
}
