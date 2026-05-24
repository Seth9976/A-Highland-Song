using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000046 RID: 70
	[Preserve]
	[Serializable]
	public class InputDeviceProfile
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000A2EA File Offset: 0x000084EA
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000A2F2 File Offset: 0x000084F2
		public InputDeviceProfileType ProfileType
		{
			get
			{
				return this.profileType;
			}
			protected set
			{
				this.profileType = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000A2FB File Offset: 0x000084FB
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000A303 File Offset: 0x00008503
		public string DeviceName
		{
			get
			{
				return this.deviceName;
			}
			protected set
			{
				this.deviceName = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000A30C File Offset: 0x0000850C
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000A314 File Offset: 0x00008514
		public string DeviceNotes
		{
			get
			{
				return this.deviceNotes;
			}
			protected set
			{
				this.deviceNotes = value;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000A31D File Offset: 0x0000851D
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000A325 File Offset: 0x00008525
		public InputDeviceClass DeviceClass
		{
			get
			{
				return this.deviceClass;
			}
			protected set
			{
				this.deviceClass = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000A32E File Offset: 0x0000852E
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000A336 File Offset: 0x00008536
		public InputDeviceStyle DeviceStyle
		{
			get
			{
				return this.deviceStyle;
			}
			protected set
			{
				this.deviceStyle = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000A33F File Offset: 0x0000853F
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000A347 File Offset: 0x00008547
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			protected set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000A355 File Offset: 0x00008555
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000A35D File Offset: 0x0000855D
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			protected set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000A36B File Offset: 0x0000856B
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000A373 File Offset: 0x00008573
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			protected set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000A381 File Offset: 0x00008581
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000A389 File Offset: 0x00008589
		public InputControlMapping[] AnalogMappings
		{
			get
			{
				return this.analogMappings;
			}
			protected set
			{
				this.analogMappings = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000A392 File Offset: 0x00008592
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000A39A File Offset: 0x0000859A
		public InputControlMapping[] ButtonMappings
		{
			get
			{
				return this.buttonMappings;
			}
			protected set
			{
				this.buttonMappings = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000A3A3 File Offset: 0x000085A3
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000A3AB File Offset: 0x000085AB
		public string[] IncludePlatforms
		{
			get
			{
				return this.includePlatforms;
			}
			protected set
			{
				this.includePlatforms = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000A3B4 File Offset: 0x000085B4
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000A3BC File Offset: 0x000085BC
		public string[] ExcludePlatforms
		{
			get
			{
				return this.excludePlatforms;
			}
			protected set
			{
				this.excludePlatforms = value;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000A3C5 File Offset: 0x000085C5
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000A3CD File Offset: 0x000085CD
		public int MinSystemBuildNumber
		{
			get
			{
				return this.minSystemBuildNumber;
			}
			protected set
			{
				this.minSystemBuildNumber = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000A3D6 File Offset: 0x000085D6
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000A3DE File Offset: 0x000085DE
		public int MaxSystemBuildNumber
		{
			get
			{
				return this.maxSystemBuildNumber;
			}
			protected set
			{
				this.maxSystemBuildNumber = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000A3E7 File Offset: 0x000085E7
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000A3EF File Offset: 0x000085EF
		public VersionInfo MinUnityVersion
		{
			get
			{
				return this.minUnityVersion;
			}
			protected set
			{
				this.minUnityVersion = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000A3F8 File Offset: 0x000085F8
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000A400 File Offset: 0x00008600
		public VersionInfo MaxUnityVersion
		{
			get
			{
				return this.maxUnityVersion;
			}
			protected set
			{
				this.maxUnityVersion = value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000A409 File Offset: 0x00008609
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000A411 File Offset: 0x00008611
		public InputDeviceMatcher[] Matchers
		{
			get
			{
				return this.matchers;
			}
			protected set
			{
				this.matchers = value;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000A41A File Offset: 0x0000861A
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000A422 File Offset: 0x00008622
		public InputDeviceMatcher[] LastResortMatchers
		{
			get
			{
				return this.lastResortMatchers;
			}
			protected set
			{
				this.lastResortMatchers = value;
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000A42B File Offset: 0x0000862B
		public static InputDeviceProfile CreateInstanceOfType(Type type)
		{
			InputDeviceProfile inputDeviceProfile = (InputDeviceProfile)Activator.CreateInstance(type);
			inputDeviceProfile.Define();
			return inputDeviceProfile;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000A440 File Offset: 0x00008640
		public static InputDeviceProfile CreateInstanceOfType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type == null)
			{
				Logger.LogWarning("Cannot find type: " + typeName + "(is the IL2CPP stripping level too high?)");
				return null;
			}
			return InputDeviceProfile.CreateInstanceOfType(type);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000A47C File Offset: 0x0000867C
		public virtual void Define()
		{
			this.profileType = ((base.GetType().GetCustomAttributes(typeof(NativeInputDeviceProfileAttribute), false).Length != 0) ? InputDeviceProfileType.Native : InputDeviceProfileType.Unity);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000A4B1 File Offset: 0x000086B1
		public bool Matches(InputDeviceInfo deviceInfo)
		{
			return this.Matches(deviceInfo, this.Matchers);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000A4C0 File Offset: 0x000086C0
		public bool LastResortMatches(InputDeviceInfo deviceInfo)
		{
			return this.Matches(deviceInfo, this.LastResortMatchers);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000A4D0 File Offset: 0x000086D0
		public bool Matches(InputDeviceInfo deviceInfo, InputDeviceMatcher[] matchers)
		{
			if (matchers != null)
			{
				int num = matchers.Length;
				for (int i = 0; i < num; i++)
				{
					if (matchers[i].Matches(deviceInfo))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000A504 File Offset: 0x00008704
		public bool IsSupportedOnThisPlatform
		{
			get
			{
				VersionInfo versionInfo = VersionInfo.UnityVersion();
				if (versionInfo < this.MinUnityVersion || versionInfo > this.MaxUnityVersion)
				{
					return false;
				}
				int systemBuildNumber = Utility.GetSystemBuildNumber();
				if (this.MaxSystemBuildNumber > 0 && systemBuildNumber > this.MaxSystemBuildNumber)
				{
					return false;
				}
				if (this.MinSystemBuildNumber > 0 && systemBuildNumber < this.MinSystemBuildNumber)
				{
					return false;
				}
				if (this.ExcludePlatforms != null)
				{
					int num = this.ExcludePlatforms.Length;
					for (int i = 0; i < num; i++)
					{
						if (InputManager.Platform.Contains(this.ExcludePlatforms[i].ToUpper()))
						{
							return false;
						}
					}
				}
				if (this.IncludePlatforms == null || this.IncludePlatforms.Length == 0)
				{
					return true;
				}
				if (this.IncludePlatforms != null)
				{
					int num2 = this.IncludePlatforms.Length;
					for (int j = 0; j < num2; j++)
					{
						if (InputManager.Platform.Contains(this.IncludePlatforms[j].ToUpper()))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000A5EE File Offset: 0x000087EE
		public static void Hide(Type type)
		{
			InputDeviceProfile.hiddenProfiles.Add(type);
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000A5FC File Offset: 0x000087FC
		public bool IsHidden
		{
			get
			{
				return InputDeviceProfile.hiddenProfiles.Contains(base.GetType());
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000A60E File Offset: 0x0000880E
		public bool IsNotHidden
		{
			get
			{
				return !this.IsHidden;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000A619 File Offset: 0x00008819
		public int AnalogCount
		{
			get
			{
				return this.AnalogMappings.Length;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000A623 File Offset: 0x00008823
		public int ButtonCount
		{
			get
			{
				return this.ButtonMappings.Length;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A62D File Offset: 0x0000882D
		protected static InputControlSource Button(int index)
		{
			return new InputControlSource(InputControlSourceType.Button, index);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A636 File Offset: 0x00008836
		protected static InputControlSource Analog(int index)
		{
			return new InputControlSource(InputControlSourceType.Analog, index);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000A63F File Offset: 0x0000883F
		protected static InputControlMapping LeftStickLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000A672 File Offset: 0x00008872
		protected static InputControlMapping LeftStickRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000A6A5 File Offset: 0x000088A5
		protected static InputControlMapping LeftStickUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000A6D8 File Offset: 0x000088D8
		protected static InputControlMapping LeftStickDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000A70B File Offset: 0x0000890B
		protected static InputControlMapping LeftStickUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Name = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000A73E File Offset: 0x0000893E
		protected static InputControlMapping LeftStickDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Name = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000A771 File Offset: 0x00008971
		protected static InputControlMapping RightStickLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000A7A4 File Offset: 0x000089A4
		protected static InputControlMapping RightStickRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000A7D8 File Offset: 0x000089D8
		protected static InputControlMapping RightStickUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000A80B File Offset: 0x00008A0B
		protected static InputControlMapping RightStickDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000A83E File Offset: 0x00008A3E
		protected static InputControlMapping RightStickUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Name = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000A871 File Offset: 0x00008A71
		protected static InputControlMapping RightStickDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Name = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000A8A4 File Offset: 0x00008AA4
		protected static InputControlMapping LeftTriggerMapping(int analog, string name = "Left Trigger")
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.LeftTrigger,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000A8DB File Offset: 0x00008ADB
		protected static InputControlMapping RightTriggerMapping(int analog, string name = "Right Trigger")
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.RightTrigger,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000A912 File Offset: 0x00008B12
		protected static InputControlMapping DPadLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000A946 File Offset: 0x00008B46
		protected static InputControlMapping DPadRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000A97A File Offset: 0x00008B7A
		protected static InputControlMapping DPadUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000A9AE File Offset: 0x00008BAE
		protected static InputControlMapping DPadDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Name = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000A9E2 File Offset: 0x00008BE2
		protected static InputControlMapping DPadUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Name = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000AA16 File Offset: 0x00008C16
		protected static InputControlMapping DPadDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Name = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = InputDeviceProfile.Analog(analog),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x04000306 RID: 774
		private static readonly HashSet<Type> hiddenProfiles = new HashSet<Type>();

		// Token: 0x04000307 RID: 775
		[SerializeField]
		private InputDeviceProfileType profileType;

		// Token: 0x04000308 RID: 776
		[SerializeField]
		private string deviceName = "";

		// Token: 0x04000309 RID: 777
		[SerializeField]
		[TextArea]
		private string deviceNotes = "";

		// Token: 0x0400030A RID: 778
		[SerializeField]
		private InputDeviceClass deviceClass;

		// Token: 0x0400030B RID: 779
		[SerializeField]
		private InputDeviceStyle deviceStyle;

		// Token: 0x0400030C RID: 780
		[SerializeField]
		private float sensitivity = 1f;

		// Token: 0x0400030D RID: 781
		[SerializeField]
		private float lowerDeadZone = 0.2f;

		// Token: 0x0400030E RID: 782
		[SerializeField]
		private float upperDeadZone = 0.9f;

		// Token: 0x0400030F RID: 783
		[SerializeField]
		private string[] includePlatforms = new string[0];

		// Token: 0x04000310 RID: 784
		[SerializeField]
		private string[] excludePlatforms = new string[0];

		// Token: 0x04000311 RID: 785
		[SerializeField]
		private int minSystemBuildNumber;

		// Token: 0x04000312 RID: 786
		[SerializeField]
		private int maxSystemBuildNumber;

		// Token: 0x04000313 RID: 787
		[SerializeField]
		private VersionInfo minUnityVersion = VersionInfo.Min;

		// Token: 0x04000314 RID: 788
		[SerializeField]
		private VersionInfo maxUnityVersion = VersionInfo.Max;

		// Token: 0x04000315 RID: 789
		[SerializeField]
		private InputDeviceMatcher[] matchers = new InputDeviceMatcher[0];

		// Token: 0x04000316 RID: 790
		[SerializeField]
		private InputDeviceMatcher[] lastResortMatchers = new InputDeviceMatcher[0];

		// Token: 0x04000317 RID: 791
		[SerializeField]
		private InputControlMapping[] analogMappings = new InputControlMapping[0];

		// Token: 0x04000318 RID: 792
		[SerializeField]
		private InputControlMapping[] buttonMappings = new InputControlMapping[0];

		// Token: 0x04000319 RID: 793
		protected static readonly InputControlSource MenuKey = new InputControlSource(KeyCode.Menu);

		// Token: 0x0400031A RID: 794
		protected static readonly InputControlSource EscapeKey = new InputControlSource(KeyCode.Escape);
	}
}
