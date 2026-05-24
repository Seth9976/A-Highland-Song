using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200021D RID: 541
	[NativeHeader("Runtime/Scripting/TextAsset.h")]
	public class TextAsset : Object
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x0600176F RID: 5999
		public extern byte[] bytes
		{
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06001770 RID: 6000
		[MethodImpl(4096)]
		private extern byte[] GetPreviewBytes(int maxByteCount);

		// Token: 0x06001771 RID: 6001
		[MethodImpl(4096)]
		private static extern void Internal_CreateInstance([Writable] TextAsset self, string text);

		// Token: 0x06001772 RID: 6002
		[MethodImpl(4096)]
		private extern IntPtr GetDataPtr();

		// Token: 0x06001773 RID: 6003
		[MethodImpl(4096)]
		private extern long GetDataSize();

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001774 RID: 6004 RVA: 0x00025D68 File Offset: 0x00023F68
		public string text
		{
			get
			{
				return TextAsset.DecodeString(this.bytes);
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x00025D85 File Offset: 0x00023F85
		public long dataSize
		{
			get
			{
				return this.GetDataSize();
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00025D90 File Offset: 0x00023F90
		public override string ToString()
		{
			return this.text;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00025DA8 File Offset: 0x00023FA8
		public TextAsset()
			: this(TextAsset.CreateOptions.CreateNativeObject, null)
		{
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00025DB4 File Offset: 0x00023FB4
		public TextAsset(string text)
			: this(TextAsset.CreateOptions.CreateNativeObject, text)
		{
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00025DC0 File Offset: 0x00023FC0
		internal TextAsset(TextAsset.CreateOptions options, string text)
		{
			bool flag = options == TextAsset.CreateOptions.CreateNativeObject;
			if (flag)
			{
				TextAsset.Internal_CreateInstance(this, text);
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00025DE8 File Offset: 0x00023FE8
		public unsafe NativeArray<T> GetData<T>() where T : struct
		{
			long dataSize = this.GetDataSize();
			long num = (long)UnsafeUtility.SizeOf<T>();
			bool flag = dataSize % num != 0L;
			if (flag)
			{
				throw new ArgumentException(string.Format("Type passed to {0} can't capture the asset data. Data size is {1} which is not a multiple of type size {2}", "GetData", dataSize, num));
			}
			long num2 = dataSize / num;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.GetDataPtr(), (int)num2, Allocator.None);
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00025E54 File Offset: 0x00024054
		internal string GetPreview(int maxChars)
		{
			return TextAsset.DecodeString(this.GetPreviewBytes(maxChars * 4));
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00025E74 File Offset: 0x00024074
		internal static string DecodeString(byte[] bytes)
		{
			int num = TextAsset.EncodingUtility.encodingLookup.Length;
			int i = 0;
			int num2;
			while (i < num)
			{
				byte[] key = TextAsset.EncodingUtility.encodingLookup[i].Key;
				num2 = key.Length;
				bool flag = bytes.Length >= num2;
				if (flag)
				{
					for (int j = 0; j < num2; j++)
					{
						bool flag2 = key[j] != bytes[j];
						if (flag2)
						{
							num2 = -1;
						}
					}
					bool flag3 = num2 < 0;
					if (!flag3)
					{
						try
						{
							Encoding value = TextAsset.EncodingUtility.encodingLookup[i].Value;
							return value.GetString(bytes, num2, bytes.Length - num2);
						}
						catch
						{
						}
					}
				}
				IL_00A2:
				i++;
				continue;
				goto IL_00A2;
			}
			num2 = 0;
			Encoding targetEncoding = TextAsset.EncodingUtility.targetEncoding;
			return targetEncoding.GetString(bytes, num2, bytes.Length - num2);
		}

		// Token: 0x0200021E RID: 542
		internal enum CreateOptions
		{
			// Token: 0x0400080C RID: 2060
			None,
			// Token: 0x0400080D RID: 2061
			CreateNativeObject
		}

		// Token: 0x0200021F RID: 543
		private static class EncodingUtility
		{
			// Token: 0x0600177D RID: 6013 RVA: 0x00025F60 File Offset: 0x00024160
			static EncodingUtility()
			{
				Encoding encoding = new UTF32Encoding(true, true, true);
				Encoding encoding2 = new UTF32Encoding(false, true, true);
				Encoding encoding3 = new UnicodeEncoding(true, true, true);
				Encoding encoding4 = new UnicodeEncoding(false, true, true);
				Encoding encoding5 = new UTF8Encoding(true, true);
				TextAsset.EncodingUtility.encodingLookup = new KeyValuePair<byte[], Encoding>[]
				{
					new KeyValuePair<byte[], Encoding>(encoding.GetPreamble(), encoding),
					new KeyValuePair<byte[], Encoding>(encoding2.GetPreamble(), encoding2),
					new KeyValuePair<byte[], Encoding>(encoding3.GetPreamble(), encoding3),
					new KeyValuePair<byte[], Encoding>(encoding4.GetPreamble(), encoding4),
					new KeyValuePair<byte[], Encoding>(encoding5.GetPreamble(), encoding5)
				};
			}

			// Token: 0x0400080E RID: 2062
			internal static readonly KeyValuePair<byte[], Encoding>[] encodingLookup;

			// Token: 0x0400080F RID: 2063
			internal static readonly Encoding targetEncoding = Encoding.GetEncoding(Encoding.UTF8.CodePage, new EncoderReplacementFallback("\ufffd"), new DecoderReplacementFallback("\ufffd"));
		}
	}
}
