using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using InControl.NativeDeviceProfiles;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000053 RID: 83
	public class NativeInputDeviceManager : InputDeviceManager
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x0000DD80 File Offset: 0x0000BF80
		public NativeInputDeviceManager()
		{
			this.attachedDevices = new List<NativeInputDevice>();
			this.detachedDevices = new List<NativeInputDevice>();
			this.systemDeviceProfiles = new List<InputDeviceProfile>(NativeInputDeviceProfileList.Profiles.Length);
			this.customDeviceProfiles = new List<InputDeviceProfile>();
			this.deviceEvents = new uint[32];
			this.AddSystemDeviceProfiles();
			NativeInputOptions nativeInputOptions = default(NativeInputOptions);
			nativeInputOptions.enableXInput = (InputManager.NativeInputEnableXInput ? 1 : 0);
			nativeInputOptions.enableMFi = (InputManager.NativeInputEnableMFi ? 1 : 0);
			nativeInputOptions.preventSleep = (InputManager.NativeInputPreventSleep ? 1 : 0);
			if (InputManager.NativeInputUpdateRate > 0U)
			{
				nativeInputOptions.updateRate = (ushort)InputManager.NativeInputUpdateRate;
			}
			else
			{
				nativeInputOptions.updateRate = (ushort)Mathf.FloorToInt(1f / Time.fixedDeltaTime);
			}
			Native.Init(nativeInputOptions);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000DE4C File Offset: 0x0000C04C
		public override void Destroy()
		{
			Native.Stop();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000DE54 File Offset: 0x0000C054
		public override void Update(ulong updateTick, float deltaTime)
		{
			IntPtr intPtr;
			int num = Native.GetDeviceEvents(out intPtr);
			if (num > 0)
			{
				Utility.ArrayExpand<uint>(ref this.deviceEvents, num);
				MarshalUtility.Copy(intPtr, this.deviceEvents, num);
				int num2 = 0;
				uint num3 = this.deviceEvents[num2++];
				int num4 = 0;
				while ((long)num4 < (long)((ulong)num3))
				{
					uint num5 = this.deviceEvents[num2++];
					StringBuilder stringBuilder = new StringBuilder(256);
					stringBuilder.Append("Attached native device with handle " + num5.ToString() + ":\n");
					InputDeviceInfo inputDeviceInfo;
					if (Native.GetDeviceInfo(num5, out inputDeviceInfo))
					{
						stringBuilder.AppendFormat("Name: {0}\n", inputDeviceInfo.name);
						stringBuilder.AppendFormat("Driver Type: {0}\n", inputDeviceInfo.driverType);
						stringBuilder.AppendFormat("Location ID: {0}\n", inputDeviceInfo.location);
						stringBuilder.AppendFormat("Serial Number: {0}\n", inputDeviceInfo.serialNumber);
						stringBuilder.AppendFormat("Vendor ID: 0x{0:x}\n", inputDeviceInfo.vendorID);
						stringBuilder.AppendFormat("Product ID: 0x{0:x}\n", inputDeviceInfo.productID);
						stringBuilder.AppendFormat("Version Number: 0x{0:x}\n", inputDeviceInfo.versionNumber);
						stringBuilder.AppendFormat("Buttons: {0}\n", inputDeviceInfo.numButtons);
						stringBuilder.AppendFormat("Analogs: {0}\n", inputDeviceInfo.numAnalogs);
						this.DetectDevice(num5, inputDeviceInfo);
					}
					Logger.LogInfo(stringBuilder.ToString());
					num4++;
				}
				uint num6 = this.deviceEvents[num2++];
				int num7 = 0;
				while ((long)num7 < (long)((ulong)num6))
				{
					uint num8 = this.deviceEvents[num2++];
					Logger.LogInfo("Detached native device with handle " + num8.ToString() + ":");
					NativeInputDevice nativeInputDevice = this.FindAttachedDevice(num8);
					if (nativeInputDevice != null)
					{
						this.DetachDevice(nativeInputDevice);
					}
					else
					{
						Logger.LogWarning("Couldn't find device to detach with handle: " + num8.ToString());
					}
					num7++;
				}
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000E058 File Offset: 0x0000C258
		private void DetectDevice(uint deviceHandle, InputDeviceInfo deviceInfo)
		{
			InputDeviceProfile inputDeviceProfile = null;
			inputDeviceProfile = inputDeviceProfile ?? this.customDeviceProfiles.Find((InputDeviceProfile profile) => profile.Matches(deviceInfo));
			inputDeviceProfile = inputDeviceProfile ?? this.systemDeviceProfiles.Find((InputDeviceProfile profile) => profile.Matches(deviceInfo));
			inputDeviceProfile = inputDeviceProfile ?? this.customDeviceProfiles.Find((InputDeviceProfile profile) => profile.LastResortMatches(deviceInfo));
			inputDeviceProfile = inputDeviceProfile ?? this.systemDeviceProfiles.Find((InputDeviceProfile profile) => profile.LastResortMatches(deviceInfo));
			if (inputDeviceProfile == null || inputDeviceProfile.IsNotHidden)
			{
				NativeInputDevice nativeInputDevice = this.FindDetachedDevice(deviceInfo) ?? new NativeInputDevice();
				nativeInputDevice.Initialize(deviceHandle, deviceInfo, inputDeviceProfile);
				this.AttachDevice(nativeInputDevice);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000E11E File Offset: 0x0000C31E
		private void AttachDevice(NativeInputDevice device)
		{
			this.detachedDevices.Remove(device);
			this.attachedDevices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000E13F File Offset: 0x0000C33F
		private void DetachDevice(NativeInputDevice device)
		{
			this.attachedDevices.Remove(device);
			this.detachedDevices.Add(device);
			InputManager.DetachDevice(device);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000E160 File Offset: 0x0000C360
		private NativeInputDevice FindAttachedDevice(uint deviceHandle)
		{
			int count = this.attachedDevices.Count;
			for (int i = 0; i < count; i++)
			{
				NativeInputDevice nativeInputDevice = this.attachedDevices[i];
				if (nativeInputDevice.Handle == deviceHandle)
				{
					return nativeInputDevice;
				}
			}
			return null;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		private NativeInputDevice FindDetachedDevice(InputDeviceInfo deviceInfo)
		{
			ReadOnlyCollection<NativeInputDevice> readOnlyCollection = new ReadOnlyCollection<NativeInputDevice>(this.detachedDevices);
			if (NativeInputDeviceManager.CustomFindDetachedDevice != null)
			{
				return NativeInputDeviceManager.CustomFindDetachedDevice(deviceInfo, readOnlyCollection);
			}
			return NativeInputDeviceManager.SystemFindDetachedDevice(deviceInfo, readOnlyCollection);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
		private static NativeInputDevice SystemFindDetachedDevice(InputDeviceInfo deviceInfo, ReadOnlyCollection<NativeInputDevice> detachedDevices)
		{
			int count = detachedDevices.Count;
			for (int i = 0; i < count; i++)
			{
				NativeInputDevice nativeInputDevice = detachedDevices[i];
				if (nativeInputDevice.Info.HasSameVendorID(deviceInfo) && nativeInputDevice.Info.HasSameProductID(deviceInfo) && nativeInputDevice.Info.HasSameSerialNumber(deviceInfo))
				{
					return nativeInputDevice;
				}
			}
			for (int j = 0; j < count; j++)
			{
				NativeInputDevice nativeInputDevice2 = detachedDevices[j];
				if (nativeInputDevice2.Info.HasSameVendorID(deviceInfo) && nativeInputDevice2.Info.HasSameProductID(deviceInfo) && nativeInputDevice2.Info.HasSameLocation(deviceInfo))
				{
					return nativeInputDevice2;
				}
			}
			for (int k = 0; k < count; k++)
			{
				NativeInputDevice nativeInputDevice3 = detachedDevices[k];
				if (nativeInputDevice3.Info.HasSameVendorID(deviceInfo) && nativeInputDevice3.Info.HasSameProductID(deviceInfo) && nativeInputDevice3.Info.HasSameVersionNumber(deviceInfo))
				{
					return nativeInputDevice3;
				}
			}
			for (int l = 0; l < count; l++)
			{
				NativeInputDevice nativeInputDevice4 = detachedDevices[l];
				if (nativeInputDevice4.Info.HasSameLocation(deviceInfo))
				{
					return nativeInputDevice4;
				}
			}
			return null;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000E307 File Offset: 0x0000C507
		private void AddSystemDeviceProfile(InputDeviceProfile deviceProfile)
		{
			if (deviceProfile != null && deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000E320 File Offset: 0x0000C520
		private void AddSystemDeviceProfiles()
		{
			for (int i = 0; i < NativeInputDeviceProfileList.Profiles.Length; i++)
			{
				InputDeviceProfile inputDeviceProfile = InputDeviceProfile.CreateInstanceOfType(NativeInputDeviceProfileList.Profiles[i]);
				this.AddSystemDeviceProfile(inputDeviceProfile);
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000E354 File Offset: 0x0000C554
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.OSXPlayer && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS)
			{
				return false;
			}
			try
			{
				NativeVersionInfo nativeVersionInfo;
				Native.GetVersionInfo(out nativeVersionInfo);
				Logger.LogInfo(string.Concat(new string[]
				{
					"InControl Native (version ",
					nativeVersionInfo.major.ToString(),
					".",
					nativeVersionInfo.minor.ToString(),
					".",
					nativeVersionInfo.patch.ToString(),
					")"
				}));
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + Utility.PluginFileExtension() + " could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000E430 File Offset: 0x0000C630
		internal static bool Enable()
		{
			List<string> list = new List<string>();
			if (NativeInputDeviceManager.CheckPlatformSupport(list))
			{
				if (InputManager.NativeInputEnableMFi)
				{
					InputManager.HideDevicesWithProfile(typeof(XboxOneSBluetoothMacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(XboxSeriesXBluetoothMacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(PlayStation4MacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(PlayStation5USBMacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(PlayStation5BluetoothMacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(SteelseriesNimbusMacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(HoriPadUltimateMacNativeProfile));
					InputManager.HideDevicesWithProfile(typeof(NintendoSwitchProMacNativeProfile));
				}
				InputManager.AddDeviceManager<NativeInputDeviceManager>();
				return true;
			}
			foreach (string text in list)
			{
				Logger.LogError("Error enabling NativeInputDeviceManager: " + text);
			}
			return false;
		}

		// Token: 0x040003A5 RID: 933
		public static Func<InputDeviceInfo, ReadOnlyCollection<NativeInputDevice>, NativeInputDevice> CustomFindDetachedDevice;

		// Token: 0x040003A6 RID: 934
		private readonly List<NativeInputDevice> attachedDevices;

		// Token: 0x040003A7 RID: 935
		private readonly List<NativeInputDevice> detachedDevices;

		// Token: 0x040003A8 RID: 936
		private readonly List<InputDeviceProfile> systemDeviceProfiles;

		// Token: 0x040003A9 RID: 937
		private readonly List<InputDeviceProfile> customDeviceProfiles;

		// Token: 0x040003AA RID: 938
		private uint[] deviceEvents;
	}
}
