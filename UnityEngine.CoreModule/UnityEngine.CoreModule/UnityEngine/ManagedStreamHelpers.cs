using System;
using System.IO;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200020D RID: 525
	internal static class ManagedStreamHelpers
	{
		// Token: 0x0600171B RID: 5915 RVA: 0x00025258 File Offset: 0x00023458
		internal static void ValidateLoadFromStream(Stream stream)
		{
			bool flag = stream == null;
			if (flag)
			{
				throw new ArgumentNullException("ManagedStream object must be non-null", "stream");
			}
			bool flag2 = !stream.CanRead;
			if (flag2)
			{
				throw new ArgumentException("ManagedStream object must be readable (stream.CanRead must return true)", "stream");
			}
			bool flag3 = !stream.CanSeek;
			if (flag3)
			{
				throw new ArgumentException("ManagedStream object must be seekable (stream.CanSeek must return true)", "stream");
			}
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x000252B8 File Offset: 0x000234B8
		[RequiredByNativeCode]
		internal unsafe static void ManagedStreamRead(byte[] buffer, int offset, int count, Stream stream, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			ManagedStreamHelpers.ValidateLoadFromStream(stream);
			*(int*)(void*)returnValueAddress = stream.Read(buffer, offset, count);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00025300 File Offset: 0x00023500
		[RequiredByNativeCode]
		internal unsafe static void ManagedStreamSeek(long offset, uint origin, Stream stream, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			ManagedStreamHelpers.ValidateLoadFromStream(stream);
			*(long*)(void*)returnValueAddress = stream.Seek(offset, origin);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x00025344 File Offset: 0x00023544
		[RequiredByNativeCode]
		internal unsafe static void ManagedStreamLength(Stream stream, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			ManagedStreamHelpers.ValidateLoadFromStream(stream);
			*(long*)(void*)returnValueAddress = stream.Length;
		}
	}
}
