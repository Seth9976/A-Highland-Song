using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000023 RID: 35
	public class MouseBindingSource : BindingSource
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004C3F File Offset: 0x00002E3F
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004C47 File Offset: 0x00002E47
		public Mouse Control { get; protected set; }

		// Token: 0x060000D8 RID: 216 RVA: 0x00004C50 File Offset: 0x00002E50
		internal MouseBindingSource()
		{
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004C58 File Offset: 0x00002E58
		public MouseBindingSource(Mouse mouseControl)
		{
			this.Control = mouseControl;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004C67 File Offset: 0x00002E67
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ButtonIsPressed(Mouse control)
		{
			return InputManager.MouseProvider.GetButtonIsPressed(control);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004C74 File Offset: 0x00002E74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool NegativeScrollWheelIsActive(float threshold)
		{
			return Mathf.Min(InputManager.MouseProvider.GetDeltaScroll() * MouseBindingSource.ScaleZ, 0f) < -threshold;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004C94 File Offset: 0x00002E94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool PositiveScrollWheelIsActive(float threshold)
		{
			return Mathf.Max(0f, InputManager.MouseProvider.GetDeltaScroll() * MouseBindingSource.ScaleZ) > threshold;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004CB4 File Offset: 0x00002EB4
		internal static float GetValue(Mouse mouseControl)
		{
			switch (mouseControl)
			{
			case Mouse.None:
				return 0f;
			case Mouse.NegativeX:
				return -Mathf.Min(InputManager.MouseProvider.GetDeltaX() * MouseBindingSource.ScaleX, 0f);
			case Mouse.PositiveX:
				return Mathf.Max(0f, InputManager.MouseProvider.GetDeltaX() * MouseBindingSource.ScaleX);
			case Mouse.NegativeY:
				return -Mathf.Min(InputManager.MouseProvider.GetDeltaY() * MouseBindingSource.ScaleY, 0f);
			case Mouse.PositiveY:
				return Mathf.Max(0f, InputManager.MouseProvider.GetDeltaY() * MouseBindingSource.ScaleY);
			case Mouse.PositiveScrollWheel:
				return Mathf.Max(0f, InputManager.MouseProvider.GetDeltaScroll() * MouseBindingSource.ScaleZ);
			case Mouse.NegativeScrollWheel:
				return -Mathf.Min(InputManager.MouseProvider.GetDeltaScroll() * MouseBindingSource.ScaleZ, 0f);
			}
			if (!InputManager.MouseProvider.GetButtonIsPressed(mouseControl))
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004DB7 File Offset: 0x00002FB7
		public override float GetValue(InputDevice inputDevice)
		{
			return MouseBindingSource.GetValue(this.Control);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public override bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00004DF5 File Offset: 0x00002FF5
		public override string DeviceName
		{
			get
			{
				return "Mouse";
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004DFC File Offset: 0x00002FFC
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Mouse;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004DFF File Offset: 0x00002FFF
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004E04 File Offset: 0x00003004
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004E3C File Offset: 0x0000303C
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004E70 File Offset: 0x00003070
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004E91 File Offset: 0x00003091
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.MouseBindingSource;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004E94 File Offset: 0x00003094
		public override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004EA2 File Offset: 0x000030A2
		public override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			this.Control = (Mouse)reader.ReadInt32();
		}

		// Token: 0x0400014D RID: 333
		public static float ScaleX = 0.05f;

		// Token: 0x0400014E RID: 334
		public static float ScaleY = 0.05f;

		// Token: 0x0400014F RID: 335
		public static float ScaleZ = 0.05f;

		// Token: 0x04000150 RID: 336
		public static float JitterThreshold = 0.05f;
	}
}
