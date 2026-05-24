using System;
using System.IO;

namespace InControl
{
	// Token: 0x0200001F RID: 31
	public class KeyBindingSource : BindingSource
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004508 File Offset: 0x00002708
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00004510 File Offset: 0x00002710
		public KeyCombo Control { get; protected set; }

		// Token: 0x060000A9 RID: 169 RVA: 0x00004519 File Offset: 0x00002719
		internal KeyBindingSource()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004521 File Offset: 0x00002721
		public KeyBindingSource(KeyCombo keyCombo)
		{
			this.Control = keyCombo;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004530 File Offset: 0x00002730
		public KeyBindingSource(params Key[] keys)
		{
			this.Control = new KeyCombo(keys);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004544 File Offset: 0x00002744
		public override float GetValue(InputDevice inputDevice)
		{
			if (!this.GetState(inputDevice))
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000455C File Offset: 0x0000275C
		public override bool GetState(InputDevice inputDevice)
		{
			return this.Control.IsPressed;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004578 File Offset: 0x00002778
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004599 File Offset: 0x00002799
		public override string DeviceName
		{
			get
			{
				return "Keyboard";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000045A0 File Offset: 0x000027A0
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Keyboard;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000045A3 File Offset: 0x000027A3
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000045A8 File Offset: 0x000027A8
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000045E4 File Offset: 0x000027E4
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000461C File Offset: 0x0000281C
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000463D File Offset: 0x0000283D
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.KeyBindingSource;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004640 File Offset: 0x00002840
		public override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			KeyCombo keyCombo = default(KeyCombo);
			keyCombo.Load(reader, dataFormatVersion);
			this.Control = keyCombo;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004668 File Offset: 0x00002868
		public override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
