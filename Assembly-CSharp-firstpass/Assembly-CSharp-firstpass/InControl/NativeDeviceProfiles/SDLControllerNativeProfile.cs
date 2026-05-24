using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001E0 RID: 480
	[Preserve]
	[NativeInputDeviceProfile]
	public class SDLControllerNativeProfile : InputDeviceProfile
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x00045AF0 File Offset: 0x00043CF0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "{NAME}";
			base.DeviceNotes = "";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[] { "OS X", "Windows" };
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00045B3C File Offset: 0x00043D3C
		protected static InputControlMapping Action1Mapping(string name)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.Action1,
				Source = InputDeviceProfile.Button(0)
			};
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00045B5E File Offset: 0x00043D5E
		protected static InputControlMapping Action2Mapping(string name)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.Action2,
				Source = InputDeviceProfile.Button(1)
			};
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00045B80 File Offset: 0x00043D80
		protected static InputControlMapping Action3Mapping(string name)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.Action3,
				Source = InputDeviceProfile.Button(2)
			};
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00045BA2 File Offset: 0x00043DA2
		protected static InputControlMapping Action4Mapping(string name)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.Action4,
				Source = InputDeviceProfile.Button(3)
			};
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00045BC4 File Offset: 0x00043DC4
		protected static InputControlMapping LeftCommandMapping(string name, InputControlType target)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = target,
				Source = InputDeviceProfile.Button(4)
			};
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00045BE5 File Offset: 0x00043DE5
		protected static InputControlMapping SystemMapping(string name, InputControlType target)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = target,
				Source = InputDeviceProfile.Button(5)
			};
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00045C06 File Offset: 0x00043E06
		protected static InputControlMapping RightCommandMapping(string name, InputControlType target)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = target,
				Source = InputDeviceProfile.Button(6)
			};
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00045C27 File Offset: 0x00043E27
		protected static InputControlMapping LeftStickButtonMapping()
		{
			return new InputControlMapping
			{
				Name = "Left Stick Button",
				Target = InputControlType.LeftStickButton,
				Source = InputDeviceProfile.Button(7)
			};
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00045C4C File Offset: 0x00043E4C
		protected static InputControlMapping RightStickButtonMapping()
		{
			return new InputControlMapping
			{
				Name = "Right Stick Button",
				Target = InputControlType.RightStickButton,
				Source = InputDeviceProfile.Button(8)
			};
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00045C72 File Offset: 0x00043E72
		protected static InputControlMapping LeftBumperMapping(string name = "Left Bumper")
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.LeftBumper,
				Source = InputDeviceProfile.Button(9)
			};
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00045C95 File Offset: 0x00043E95
		protected static InputControlMapping RightBumperMapping(string name = "Right Bumper")
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.RightBumper,
				Source = InputDeviceProfile.Button(10)
			};
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00045CB8 File Offset: 0x00043EB8
		protected static InputControlMapping DPadUpMapping()
		{
			return new InputControlMapping
			{
				Name = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = InputDeviceProfile.Button(11)
			};
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00045CDF File Offset: 0x00043EDF
		protected static InputControlMapping DPadDownMapping()
		{
			return new InputControlMapping
			{
				Name = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = InputDeviceProfile.Button(12)
			};
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00045D06 File Offset: 0x00043F06
		protected static InputControlMapping DPadLeftMapping()
		{
			return new InputControlMapping
			{
				Name = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = InputDeviceProfile.Button(13)
			};
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00045D2D File Offset: 0x00043F2D
		protected static InputControlMapping DPadRightMapping()
		{
			return new InputControlMapping
			{
				Name = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = InputDeviceProfile.Button(14)
			};
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00045D54 File Offset: 0x00043F54
		protected static InputControlMapping Misc1Mapping(string name, InputControlType target)
		{
			return new InputControlMapping
			{
				Name = name,
				Target = target,
				Source = InputDeviceProfile.Button(15)
			};
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00045D76 File Offset: 0x00043F76
		protected static InputControlMapping Paddle1Mapping()
		{
			return new InputControlMapping
			{
				Name = "Paddle 1",
				Target = InputControlType.Paddle1,
				Source = InputDeviceProfile.Button(16)
			};
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00045DA0 File Offset: 0x00043FA0
		protected static InputControlMapping Paddle2Mapping()
		{
			return new InputControlMapping
			{
				Name = "Paddle 2",
				Target = InputControlType.Paddle2,
				Source = InputDeviceProfile.Button(17)
			};
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00045DCA File Offset: 0x00043FCA
		protected static InputControlMapping Paddle3Mapping()
		{
			return new InputControlMapping
			{
				Name = "Paddle 3",
				Target = InputControlType.Paddle3,
				Source = InputDeviceProfile.Button(18)
			};
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00045DF4 File Offset: 0x00043FF4
		protected static InputControlMapping Paddle4Mapping()
		{
			return new InputControlMapping
			{
				Name = "Paddle 4",
				Target = InputControlType.Paddle4,
				Source = InputDeviceProfile.Button(19)
			};
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00045E1E File Offset: 0x0004401E
		protected static InputControlMapping TouchPadButtonMapping()
		{
			return new InputControlMapping
			{
				Name = "Touch Pad Button",
				Target = InputControlType.TouchPadButton,
				Source = InputDeviceProfile.Button(20)
			};
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00045E48 File Offset: 0x00044048
		protected static InputControlMapping LeftStickLeftMapping()
		{
			return new InputControlMapping
			{
				Name = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = InputDeviceProfile.Analog(0),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00045E7B File Offset: 0x0004407B
		protected static InputControlMapping LeftStickRightMapping()
		{
			return new InputControlMapping
			{
				Name = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = InputDeviceProfile.Analog(0),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00045EAE File Offset: 0x000440AE
		protected static InputControlMapping LeftStickUpMapping()
		{
			return new InputControlMapping
			{
				Name = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = InputDeviceProfile.Analog(1),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00045EE1 File Offset: 0x000440E1
		protected static InputControlMapping LeftStickDownMapping()
		{
			return new InputControlMapping
			{
				Name = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = InputDeviceProfile.Analog(1),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00045F14 File Offset: 0x00044114
		protected static InputControlMapping RightStickLeftMapping()
		{
			return new InputControlMapping
			{
				Name = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = InputDeviceProfile.Analog(2),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00045F47 File Offset: 0x00044147
		protected static InputControlMapping RightStickRightMapping()
		{
			return new InputControlMapping
			{
				Name = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = InputDeviceProfile.Analog(2),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00045F7B File Offset: 0x0004417B
		protected static InputControlMapping RightStickUpMapping()
		{
			return new InputControlMapping
			{
				Name = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = InputDeviceProfile.Analog(3),
				SourceRange = InputRangeType.ZeroToMinusOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00045FAE File Offset: 0x000441AE
		protected static InputControlMapping RightStickDownMapping()
		{
			return new InputControlMapping
			{
				Name = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = InputDeviceProfile.Analog(3),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00045FE1 File Offset: 0x000441E1
		protected static InputControlMapping LeftTriggerMapping(string name = "Left Trigger")
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.LeftTrigger,
				Source = InputDeviceProfile.Analog(4),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00046011 File Offset: 0x00044211
		protected static InputControlMapping RightTriggerMapping(string name = "Right Trigger")
		{
			return new InputControlMapping
			{
				Name = name,
				Target = InputControlType.RightTrigger,
				Source = InputDeviceProfile.Analog(5),
				SourceRange = InputRangeType.ZeroToOne,
				TargetRange = InputRangeType.ZeroToOne
			};
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00046041 File Offset: 0x00044241
		protected static InputControlMapping AccelerometerXMapping()
		{
			return new InputControlMapping
			{
				Name = "Accelerometer X",
				Target = InputControlType.AccelerometerX,
				Source = InputDeviceProfile.Analog(6),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.MinusOneToOne,
				Passive = true
			};
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0004607F File Offset: 0x0004427F
		protected static InputControlMapping AccelerometerYMapping()
		{
			return new InputControlMapping
			{
				Name = "Accelerometer Y",
				Target = InputControlType.AccelerometerY,
				Source = InputDeviceProfile.Analog(7),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.MinusOneToOne,
				Passive = true
			};
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000460BD File Offset: 0x000442BD
		protected static InputControlMapping AccelerometerZMapping()
		{
			return new InputControlMapping
			{
				Name = "Accelerometer Z",
				Target = InputControlType.AccelerometerZ,
				Source = InputDeviceProfile.Analog(8),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.MinusOneToOne,
				Passive = true
			};
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000460FB File Offset: 0x000442FB
		protected static InputControlMapping GyroscopeXMapping()
		{
			return new InputControlMapping
			{
				Name = "Gyroscope X",
				Target = InputControlType.TiltX,
				Source = InputDeviceProfile.Analog(9),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.MinusOneToOne,
				Passive = true
			};
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0004613A File Offset: 0x0004433A
		protected static InputControlMapping GyroscopeYMapping()
		{
			return new InputControlMapping
			{
				Name = "Gyroscope Y",
				Target = InputControlType.TiltY,
				Source = InputDeviceProfile.Analog(10),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.MinusOneToOne,
				Passive = true
			};
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00046179 File Offset: 0x00044379
		protected static InputControlMapping GyroscopeZMapping()
		{
			return new InputControlMapping
			{
				Name = "Gyroscope Z",
				Target = InputControlType.TiltZ,
				Source = InputDeviceProfile.Analog(11),
				SourceRange = InputRangeType.MinusOneToOne,
				TargetRange = InputRangeType.MinusOneToOne,
				Passive = true
			};
		}

		// Token: 0x02000226 RID: 550
		protected enum SDLButtonType
		{
			// Token: 0x0400050F RID: 1295
			SDL_CONTROLLER_BUTTON_INVALID = -1,
			// Token: 0x04000510 RID: 1296
			SDL_CONTROLLER_BUTTON_A,
			// Token: 0x04000511 RID: 1297
			SDL_CONTROLLER_BUTTON_B,
			// Token: 0x04000512 RID: 1298
			SDL_CONTROLLER_BUTTON_X,
			// Token: 0x04000513 RID: 1299
			SDL_CONTROLLER_BUTTON_Y,
			// Token: 0x04000514 RID: 1300
			SDL_CONTROLLER_BUTTON_BACK,
			// Token: 0x04000515 RID: 1301
			SDL_CONTROLLER_BUTTON_GUIDE,
			// Token: 0x04000516 RID: 1302
			SDL_CONTROLLER_BUTTON_START,
			// Token: 0x04000517 RID: 1303
			SDL_CONTROLLER_BUTTON_LEFTSTICK,
			// Token: 0x04000518 RID: 1304
			SDL_CONTROLLER_BUTTON_RIGHTSTICK,
			// Token: 0x04000519 RID: 1305
			SDL_CONTROLLER_BUTTON_LEFTSHOULDER,
			// Token: 0x0400051A RID: 1306
			SDL_CONTROLLER_BUTTON_RIGHTSHOULDER,
			// Token: 0x0400051B RID: 1307
			SDL_CONTROLLER_BUTTON_DPAD_UP,
			// Token: 0x0400051C RID: 1308
			SDL_CONTROLLER_BUTTON_DPAD_DOWN,
			// Token: 0x0400051D RID: 1309
			SDL_CONTROLLER_BUTTON_DPAD_LEFT,
			// Token: 0x0400051E RID: 1310
			SDL_CONTROLLER_BUTTON_DPAD_RIGHT,
			// Token: 0x0400051F RID: 1311
			SDL_CONTROLLER_BUTTON_MISC1,
			// Token: 0x04000520 RID: 1312
			SDL_CONTROLLER_BUTTON_PADDLE1,
			// Token: 0x04000521 RID: 1313
			SDL_CONTROLLER_BUTTON_PADDLE2,
			// Token: 0x04000522 RID: 1314
			SDL_CONTROLLER_BUTTON_PADDLE3,
			// Token: 0x04000523 RID: 1315
			SDL_CONTROLLER_BUTTON_PADDLE4,
			// Token: 0x04000524 RID: 1316
			SDL_CONTROLLER_BUTTON_TOUCHPAD,
			// Token: 0x04000525 RID: 1317
			SDL_CONTROLLER_BUTTON_MAX
		}
	}
}
