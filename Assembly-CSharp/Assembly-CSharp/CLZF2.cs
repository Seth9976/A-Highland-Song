using System;

// Token: 0x02000065 RID: 101
public class CLZF2
{
	// Token: 0x060002C6 RID: 710 RVA: 0x00016C33 File Offset: 0x00014E33
	public static byte[] Compress(byte[] inputBytes)
	{
		return CLZF2.Compress(inputBytes, inputBytes.Length);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00016C40 File Offset: 0x00014E40
	public static byte[] Compress(byte[] inputBytes, int inputLength)
	{
		byte[] array = null;
		int num = CLZF2.Compress(inputBytes, ref array, inputLength);
		byte[] array2 = new byte[num];
		Buffer.BlockCopy(array, 0, array2, 0, num);
		return array2;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00016C6B File Offset: 0x00014E6B
	public static int Compress(byte[] inputBytes, ref byte[] outputBuffer)
	{
		return CLZF2.Compress(inputBytes, ref outputBuffer, inputBytes.Length);
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00016C78 File Offset: 0x00014E78
	public static int Compress(byte[] inputBytes, ref byte[] outputBuffer, int inputLength)
	{
		int num = inputBytes.Length * 2;
		if (outputBuffer == null || outputBuffer.Length < num)
		{
			outputBuffer = new byte[num];
		}
		int num2;
		for (num2 = CLZF2.lzf_compress(inputBytes, ref outputBuffer, inputLength); num2 == 0; num2 = CLZF2.lzf_compress(inputBytes, ref outputBuffer, inputLength))
		{
			num *= 2;
			outputBuffer = new byte[num];
		}
		return num2;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00016CC2 File Offset: 0x00014EC2
	public static byte[] Decompress(byte[] inputBytes)
	{
		return CLZF2.Decompress(inputBytes, inputBytes.Length);
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00016CD0 File Offset: 0x00014ED0
	public static byte[] Decompress(byte[] inputBytes, int inputLength)
	{
		byte[] array = null;
		int num = CLZF2.Decompress(inputBytes, ref array, inputLength);
		byte[] array2 = new byte[num];
		Buffer.BlockCopy(array, 0, array2, 0, num);
		return array2;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00016CFB File Offset: 0x00014EFB
	public static int Decompress(byte[] inputBytes, ref byte[] outputBuffer)
	{
		return CLZF2.Decompress(inputBytes, ref outputBuffer, inputBytes.Length);
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00016D08 File Offset: 0x00014F08
	public static int Decompress(byte[] inputBytes, ref byte[] outputBuffer, int inputLength)
	{
		int num = inputBytes.Length * 2;
		if (outputBuffer == null || outputBuffer.Length < num)
		{
			outputBuffer = new byte[num];
		}
		int num2;
		for (num2 = CLZF2.lzf_decompress(inputBytes, ref outputBuffer, inputLength); num2 == 0; num2 = CLZF2.lzf_decompress(inputBytes, ref outputBuffer, inputLength))
		{
			num *= 2;
			outputBuffer = new byte[num];
		}
		return num2;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00016D54 File Offset: 0x00014F54
	private static int lzf_compress(byte[] input, ref byte[] output, int inputLength)
	{
		int num = output.Length;
		uint num2 = 0U;
		uint num3 = 0U;
		uint num4 = (uint)(((int)input[(int)num2] << 8) | (int)input[(int)(num2 + 1U)]);
		int num5 = 0;
		object obj = CLZF2.locker;
		lock (obj)
		{
			Array.Clear(CLZF2.HashTable, 0, 16384);
			for (;;)
			{
				if ((ulong)num2 < (ulong)((long)(inputLength - 2)))
				{
					num4 = (num4 << 8) | (uint)input[(int)(num2 + 2U)];
					long num6 = (long)((ulong)(((num4 ^ (num4 << 5)) >> (int)(10U - num4 * 5U)) & 16383U));
					checked
					{
						long num7 = CLZF2.HashTable[(int)((IntPtr)num6)];
						CLZF2.HashTable[(int)((IntPtr)num6)] = (long)(unchecked((ulong)num2));
						long num8;
						if (unchecked((num8 = (long)((ulong)num2 - (ulong)num7 - 1UL)) < 8192L && (ulong)(num2 + 4U) < (ulong)((long)inputLength)) && num7 > 0L && input[(int)((IntPtr)num7)] == input[(int)num2] && input[(int)((IntPtr)(unchecked(num7 + 1L)))] == input[(int)(unchecked(num2 + 1U))] && input[(int)((IntPtr)(unchecked(num7 + 2L)))] == input[(int)(unchecked(num2 + 2U))])
						{
							uint num9 = 2U;
							unchecked
							{
								uint num10 = (uint)(inputLength - (int)num2 - (int)num9);
								num10 = ((num10 > 264U) ? 264U : num10);
								if ((ulong)num3 + (ulong)((long)num5) + 1UL + 3UL >= (ulong)((long)num))
								{
									break;
								}
								do
								{
									num9 += 1U;
								}
								while (num9 < num10 && input[(int)(checked((IntPtr)(unchecked(num7 + (long)((ulong)num9)))))] == input[(int)(num2 + num9)]);
								if (num5 != 0)
								{
									output[(int)num3++] = (byte)(num5 - 1);
									num5 = -num5;
									do
									{
										output[(int)num3++] = input[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)num5)))))];
									}
									while (++num5 != 0);
								}
								num9 -= 2U;
								num2 += 1U;
								if (num9 < 7U)
								{
									output[(int)num3++] = (byte)((num8 >> 8) + (long)((ulong)((ulong)num9 << 5)));
								}
								else
								{
									output[(int)num3++] = (byte)((num8 >> 8) + 224L);
									output[(int)num3++] = (byte)(num9 - 7U);
								}
								output[(int)num3++] = (byte)num8;
								num2 += num9 - 1U;
								num4 = (uint)(((int)input[(int)num2] << 8) | (int)input[(int)(num2 + 1U)]);
								num4 = (num4 << 8) | (uint)input[(int)(num2 + 2U)];
								CLZF2.HashTable[(int)(((num4 ^ (num4 << 5)) >> (int)(10U - num4 * 5U)) & 16383U)] = (long)((ulong)num2);
								num2 += 1U;
								num4 = (num4 << 8) | (uint)input[(int)(num2 + 2U)];
								CLZF2.HashTable[(int)(((num4 ^ (num4 << 5)) >> (int)(10U - num4 * 5U)) & 16383U)] = (long)((ulong)num2);
								num2 += 1U;
								continue;
							}
						}
					}
				}
				else if ((ulong)num2 == (ulong)((long)inputLength))
				{
					goto Block_17;
				}
				num5++;
				num2 += 1U;
				if ((long)num5 == 32L)
				{
					if ((ulong)(num3 + 1U + 32U) >= (ulong)((long)num))
					{
						goto Block_19;
					}
					output[(int)num3++] = 31;
					num5 = -num5;
					do
					{
						output[(int)num3++] = input[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)num5)))))];
					}
					while (++num5 != 0);
				}
			}
			return 0;
			Block_17:
			goto IL_0290;
			Block_19:
			return 0;
		}
		IL_0290:
		if (num5 != 0)
		{
			if ((ulong)num3 + (ulong)((long)num5) + 1UL >= (ulong)((long)num))
			{
				return 0;
			}
			output[(int)num3++] = (byte)(num5 - 1);
			num5 = -num5;
			do
			{
				output[(int)num3++] = input[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)num5)))))];
			}
			while (++num5 != 0);
		}
		return (int)num3;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00017050 File Offset: 0x00015250
	private static int lzf_decompress(byte[] input, ref byte[] output, int inputLength)
	{
		int num = output.Length;
		uint num2 = 0U;
		uint num3 = 0U;
		for (;;)
		{
			uint num4 = (uint)input[(int)num2++];
			if (num4 < 32U)
			{
				num4 += 1U;
				if ((ulong)(num3 + num4) > (ulong)((long)num))
				{
					break;
				}
				do
				{
					output[(int)num3++] = input[(int)num2++];
				}
				while ((num4 -= 1U) != 0U);
			}
			else
			{
				uint num5 = num4 >> 5;
				int num6 = (int)(num3 - ((num4 & 31U) << 8) - 1U);
				if (num5 == 7U)
				{
					num5 += (uint)input[(int)num2++];
				}
				num6 -= (int)input[(int)num2++];
				if ((ulong)(num3 + num5 + 2U) > (ulong)((long)num))
				{
					return 0;
				}
				if (num6 < 0)
				{
					return 0;
				}
				output[(int)num3++] = output[num6++];
				output[(int)num3++] = output[num6++];
				do
				{
					output[(int)num3++] = output[num6++];
				}
				while ((num5 -= 1U) != 0U);
			}
			if ((ulong)num2 >= (ulong)((long)inputLength))
			{
				return (int)num3;
			}
		}
		return 0;
	}

	// Token: 0x04000400 RID: 1024
	private const int BUFFER_SIZE_ESTIMATE = 2;

	// Token: 0x04000401 RID: 1025
	private const uint HLOG = 14U;

	// Token: 0x04000402 RID: 1026
	private const uint HSIZE = 16384U;

	// Token: 0x04000403 RID: 1027
	private const uint MAX_LIT = 32U;

	// Token: 0x04000404 RID: 1028
	private const uint MAX_OFF = 8192U;

	// Token: 0x04000405 RID: 1029
	private const uint MAX_REF = 264U;

	// Token: 0x04000406 RID: 1030
	private static readonly long[] HashTable = new long[16384];

	// Token: 0x04000407 RID: 1031
	private static readonly object locker = new object();
}
