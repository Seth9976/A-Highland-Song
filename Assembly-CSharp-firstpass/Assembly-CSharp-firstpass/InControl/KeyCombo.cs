using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InControl
{
	// Token: 0x02000021 RID: 33
	public struct KeyCombo
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000472C File Offset: 0x0000292C
		public KeyCombo(params Key[] keys)
		{
			this.includeData = 0UL;
			this.includeSize = 0;
			this.excludeData = 0UL;
			this.excludeSize = 0;
			for (int i = 0; i < keys.Length; i++)
			{
				this.AddInclude(keys[i]);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000476E File Offset: 0x0000296E
		private void AddIncludeInt(int key)
		{
			if (this.includeSize == 8)
			{
				return;
			}
			this.includeData |= (ulong)((ulong)((long)key & 255L) << this.includeSize * 8);
			this.includeSize++;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000047AA File Offset: 0x000029AA
		private int GetIncludeInt(int index)
		{
			return (int)((this.includeData >> index * 8) & 255UL);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000047C1 File Offset: 0x000029C1
		[Obsolete("Use KeyCombo.AddInclude instead.")]
		public void Add(Key key)
		{
			this.AddInclude(key);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000047CA File Offset: 0x000029CA
		[Obsolete("Use KeyCombo.GetInclude instead.")]
		public Key Get(int index)
		{
			return this.GetInclude(index);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000047D3 File Offset: 0x000029D3
		public void AddInclude(Key key)
		{
			this.AddIncludeInt((int)key);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000047DC File Offset: 0x000029DC
		public Key GetInclude(int index)
		{
			if (index < 0 || index >= this.includeSize)
			{
				throw new IndexOutOfRangeException("Index " + index.ToString() + " is out of the range 0.." + this.includeSize.ToString());
			}
			return (Key)this.GetIncludeInt(index);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004819 File Offset: 0x00002A19
		private void AddExcludeInt(int key)
		{
			if (this.excludeSize == 8)
			{
				return;
			}
			this.excludeData |= (ulong)((ulong)((long)key & 255L) << this.excludeSize * 8);
			this.excludeSize++;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004855 File Offset: 0x00002A55
		private int GetExcludeInt(int index)
		{
			return (int)((this.excludeData >> index * 8) & 255UL);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000486C File Offset: 0x00002A6C
		public void AddExclude(Key key)
		{
			this.AddExcludeInt((int)key);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004875 File Offset: 0x00002A75
		public Key GetExclude(int index)
		{
			if (index < 0 || index >= this.excludeSize)
			{
				throw new IndexOutOfRangeException("Index " + index.ToString() + " is out of the range 0.." + this.excludeSize.ToString());
			}
			return (Key)this.GetExcludeInt(index);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000048B2 File Offset: 0x00002AB2
		public static KeyCombo With(params Key[] keys)
		{
			return new KeyCombo(keys);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000048BC File Offset: 0x00002ABC
		public KeyCombo AndNot(params Key[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				this.AddExclude(keys[i]);
			}
			return this;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000048E6 File Offset: 0x00002AE6
		public void Clear()
		{
			this.includeData = 0UL;
			this.includeSize = 0;
			this.excludeData = 0UL;
			this.excludeSize = 0;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004906 File Offset: 0x00002B06
		[Obsolete("Use KeyCombo.IncludeCount instead.")]
		public int Count
		{
			get
			{
				return this.includeSize;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000490E File Offset: 0x00002B0E
		public int IncludeCount
		{
			get
			{
				return this.includeSize;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004916 File Offset: 0x00002B16
		public int ExcludeCount
		{
			get
			{
				return this.excludeSize;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004920 File Offset: 0x00002B20
		public bool IsPressed
		{
			get
			{
				if (this.includeSize == 0)
				{
					return false;
				}
				IKeyboardProvider keyboardProvider = InputManager.KeyboardProvider;
				bool flag = true;
				for (int i = 0; i < this.includeSize; i++)
				{
					Key include = this.GetInclude(i);
					flag = flag && keyboardProvider.GetKeyIsPressed(include);
				}
				for (int j = 0; j < this.excludeSize; j++)
				{
					Key exclude = this.GetExclude(j);
					if (keyboardProvider.GetKeyIsPressed(exclude))
					{
						return false;
					}
				}
				return flag;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004994 File Offset: 0x00002B94
		public static KeyCombo Detect(bool modifiersAsKeys)
		{
			KeyCombo empty = KeyCombo.Empty;
			IKeyboardProvider keyboardProvider = InputManager.KeyboardProvider;
			if (keyboardProvider == null)
			{
				return empty;
			}
			if (modifiersAsKeys)
			{
				for (Key key = Key.LeftShift; key <= Key.RightControl; key++)
				{
					if (keyboardProvider.GetKeyIsPressed(key))
					{
						empty.AddInclude(key);
						if (key == Key.LeftControl && keyboardProvider.GetKeyIsPressed(Key.RightAlt))
						{
							empty.AddInclude(Key.RightAlt);
						}
						return empty;
					}
				}
			}
			else
			{
				for (Key key2 = Key.Shift; key2 <= Key.Control; key2++)
				{
					if (keyboardProvider.GetKeyIsPressed(key2))
					{
						empty.AddInclude(key2);
					}
				}
			}
			for (Key key3 = Key.Escape; key3 <= Key.QuestionMark; key3++)
			{
				if (keyboardProvider.GetKeyIsPressed(key3))
				{
					empty.AddInclude(key3);
					return empty;
				}
			}
			empty.Clear();
			return empty;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004A40 File Offset: 0x00002C40
		public override string ToString()
		{
			string text;
			if (!KeyCombo.cachedStrings.TryGetValue(this.includeData, out text))
			{
				KeyCombo.cachedStringBuilder.Clear();
				for (int i = 0; i < this.includeSize; i++)
				{
					if (i != 0)
					{
						KeyCombo.cachedStringBuilder.Append(" ");
					}
					Key include = this.GetInclude(i);
					KeyCombo.cachedStringBuilder.Append(InputManager.KeyboardProvider.GetNameForKey(include));
				}
				text = KeyCombo.cachedStringBuilder.ToString();
				KeyCombo.cachedStrings[this.includeData] = text;
			}
			return text;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004ACB File Offset: 0x00002CCB
		public static bool operator ==(KeyCombo a, KeyCombo b)
		{
			return a.includeData == b.includeData && a.excludeData == b.excludeData;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004AEB File Offset: 0x00002CEB
		public static bool operator !=(KeyCombo a, KeyCombo b)
		{
			return a.includeData != b.includeData || a.excludeData != b.excludeData;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004B10 File Offset: 0x00002D10
		public override bool Equals(object other)
		{
			if (other is KeyCombo)
			{
				KeyCombo keyCombo = (KeyCombo)other;
				return this.includeData == keyCombo.includeData && this.excludeData == keyCombo.excludeData;
			}
			return false;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004B4C File Offset: 0x00002D4C
		public override int GetHashCode()
		{
			return (17 * 31 + this.includeData.GetHashCode()) * 31 + this.excludeData.GetHashCode();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004B70 File Offset: 0x00002D70
		internal void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			if (dataFormatVersion == 1)
			{
				this.includeSize = reader.ReadInt32();
				this.includeData = reader.ReadUInt64();
				return;
			}
			if (dataFormatVersion != 2)
			{
				throw new InControlException("Unknown data format version: " + dataFormatVersion.ToString());
			}
			this.includeSize = reader.ReadInt32();
			this.includeData = reader.ReadUInt64();
			this.excludeSize = reader.ReadInt32();
			this.excludeData = reader.ReadUInt64();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004BE7 File Offset: 0x00002DE7
		internal void Save(BinaryWriter writer)
		{
			writer.Write(this.includeSize);
			writer.Write(this.includeData);
			writer.Write(this.excludeSize);
			writer.Write(this.excludeData);
		}

		// Token: 0x04000134 RID: 308
		public static readonly KeyCombo Empty = default(KeyCombo);

		// Token: 0x04000135 RID: 309
		private int includeSize;

		// Token: 0x04000136 RID: 310
		private ulong includeData;

		// Token: 0x04000137 RID: 311
		private int excludeSize;

		// Token: 0x04000138 RID: 312
		private ulong excludeData;

		// Token: 0x04000139 RID: 313
		private static readonly Dictionary<ulong, string> cachedStrings = new Dictionary<ulong, string>();

		// Token: 0x0400013A RID: 314
		private static readonly StringBuilder cachedStringBuilder = new StringBuilder(256);
	}
}
