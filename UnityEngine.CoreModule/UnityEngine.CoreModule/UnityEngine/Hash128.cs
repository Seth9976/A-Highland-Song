using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001AD RID: 429
	[NativeHeader("Runtime/Export/Hashing/Hash128.bindings.h")]
	[NativeHeader("Runtime/Utilities/Hash128.h")]
	[UsedByNativeCode]
	[Serializable]
	public struct Hash128 : IComparable, IComparable<Hash128>, IEquatable<Hash128>
	{
		// Token: 0x060012E7 RID: 4839 RVA: 0x00019595 File Offset: 0x00017795
		public Hash128(uint u32_0, uint u32_1, uint u32_2, uint u32_3)
		{
			this.u64_0 = ((ulong)u32_1 << 32) | (ulong)u32_0;
			this.u64_1 = ((ulong)u32_3 << 32) | (ulong)u32_2;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x000195B5 File Offset: 0x000177B5
		public Hash128(ulong u64_0, ulong u64_1)
		{
			this.u64_0 = u64_0;
			this.u64_1 = u64_1;
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x000195C6 File Offset: 0x000177C6
		public bool isValid
		{
			get
			{
				return this.u64_0 != 0UL || this.u64_1 > 0UL;
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x000195E0 File Offset: 0x000177E0
		public int CompareTo(Hash128 rhs)
		{
			bool flag = this < rhs;
			int num;
			if (flag)
			{
				num = -1;
			}
			else
			{
				bool flag2 = this > rhs;
				if (flag2)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0001961C File Offset: 0x0001781C
		public override string ToString()
		{
			return Hash128.Hash128ToStringImpl(this);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0001963C File Offset: 0x0001783C
		[FreeFunction("StringToHash128", IsThreadSafe = true)]
		public static Hash128 Parse(string hashString)
		{
			Hash128 hash;
			Hash128.Parse_Injected(hashString, out hash);
			return hash;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00019652 File Offset: 0x00017852
		[FreeFunction("Hash128ToString", IsThreadSafe = true)]
		private static string Hash128ToStringImpl(Hash128 hash)
		{
			return Hash128.Hash128ToStringImpl_Injected(ref hash);
		}

		// Token: 0x060012EE RID: 4846
		[FreeFunction("ComputeHash128FromScriptString", IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern void ComputeFromString(string data, ref Hash128 hash);

		// Token: 0x060012EF RID: 4847
		[FreeFunction("ComputeHash128FromScriptPointer", IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern void ComputeFromPtr(IntPtr data, int start, int count, int elemSize, ref Hash128 hash);

		// Token: 0x060012F0 RID: 4848
		[FreeFunction("ComputeHash128FromScriptArray", IsThreadSafe = true)]
		[MethodImpl(4096)]
		private static extern void ComputeFromArray(Array data, int start, int count, int elemSize, ref Hash128 hash);

		// Token: 0x060012F1 RID: 4849 RVA: 0x0001965C File Offset: 0x0001785C
		public static Hash128 Compute(string data)
		{
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromString(data, ref hash);
			return hash;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00019680 File Offset: 0x00017880
		public static Hash128 Compute<T>(NativeArray<T> data) where T : struct
		{
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, data.Length, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000196BC File Offset: 0x000178BC
		public static Hash128 Compute<T>(NativeArray<T> data, int start, int count) where T : struct
		{
			bool flag = start < 0 || count < 0 || start + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), start, count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00019728 File Offset: 0x00017928
		public static Hash128 Compute<T>(T[] data) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Compute must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(data, 0, data.Length, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00019778 File Offset: 0x00017978
		public static Hash128 Compute<T>(T[] data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Compute must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Length;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(data, start, count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000197FC File Offset: 0x000179FC
		public static Hash128 Compute<T>(List<T> data) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Compute", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), 0, data.Count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00019864 File Offset: 0x00017A64
		public static Hash128 Compute<T>(List<T> data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Compute", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Count;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), start, count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000198FC File Offset: 0x00017AFC
		public unsafe static Hash128 Compute<[IsUnmanaged] T>(ref T val) where T : struct, ValueType
		{
			fixed (T* ptr = &val)
			{
				void* ptr2 = (void*)ptr;
				Hash128 hash = default(Hash128);
				Hash128.ComputeFromPtr((IntPtr)ptr2, 0, 1, UnsafeUtility.SizeOf<T>(), ref hash);
				return hash;
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00019934 File Offset: 0x00017B34
		public static Hash128 Compute(int val)
		{
			Hash128 hash = default(Hash128);
			hash.Append(val);
			return hash;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00019958 File Offset: 0x00017B58
		public static Hash128 Compute(float val)
		{
			Hash128 hash = default(Hash128);
			hash.Append(val);
			return hash;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x0001997C File Offset: 0x00017B7C
		public unsafe static Hash128 Compute(void* data, ulong size)
		{
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromPtr(new IntPtr(data), 0, (int)size, 1, ref hash);
			return hash;
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x000199A9 File Offset: 0x00017BA9
		public void Append(string data)
		{
			Hash128.ComputeFromString(data, ref this);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x000199B4 File Offset: 0x00017BB4
		public void Append<T>(NativeArray<T> data) where T : struct
		{
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, data.Length, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x000199D8 File Offset: 0x00017BD8
		public void Append<T>(NativeArray<T> data, int start, int count) where T : struct
		{
			bool flag = start < 0 || count < 0 || start + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), start, count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00019A38 File Offset: 0x00017C38
		public void Append<T>(T[] data) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Append must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			Hash128.ComputeFromArray(data, 0, data.Length, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00019A7C File Offset: 0x00017C7C
		public void Append<T>(T[] data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Append must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Length;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128.ComputeFromArray(data, start, count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00019AF0 File Offset: 0x00017CF0
		public void Append<T>(List<T> data) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Append", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), 0, data.Count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00019B48 File Offset: 0x00017D48
		public void Append<T>(List<T> data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Append", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Count;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), start, count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00019BD0 File Offset: 0x00017DD0
		public unsafe void Append<[IsUnmanaged] T>(ref T val) where T : struct, ValueType
		{
			fixed (T* ptr = &val)
			{
				void* ptr2 = (void*)ptr;
				Hash128.ComputeFromPtr((IntPtr)ptr2, 0, 1, UnsafeUtility.SizeOf<T>(), ref this);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00019BFC File Offset: 0x00017DFC
		public void Append(int val)
		{
			this.ShortHash4((uint)val);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00019C07 File Offset: 0x00017E07
		public unsafe void Append(float val)
		{
			this.ShortHash4(*(uint*)(&val));
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00019C15 File Offset: 0x00017E15
		public unsafe void Append(void* data, ulong size)
		{
			Hash128.ComputeFromPtr(new IntPtr(data), 0, (int)size, 1, ref this);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00019C2C File Offset: 0x00017E2C
		public override bool Equals(object obj)
		{
			return obj is Hash128 && this == (Hash128)obj;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00019C5C File Offset: 0x00017E5C
		public bool Equals(Hash128 obj)
		{
			return this == obj;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00019C7C File Offset: 0x00017E7C
		public override int GetHashCode()
		{
			return this.u64_0.GetHashCode() ^ this.u64_1.GetHashCode();
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00019CA8 File Offset: 0x00017EA8
		public int CompareTo(object obj)
		{
			bool flag = obj == null || !(obj is Hash128);
			int num;
			if (flag)
			{
				num = 1;
			}
			else
			{
				Hash128 hash = (Hash128)obj;
				num = this.CompareTo(hash);
			}
			return num;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x00019CE4 File Offset: 0x00017EE4
		public static bool operator ==(Hash128 hash1, Hash128 hash2)
		{
			return hash1.u64_0 == hash2.u64_0 && hash1.u64_1 == hash2.u64_1;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00019D18 File Offset: 0x00017F18
		public static bool operator !=(Hash128 hash1, Hash128 hash2)
		{
			return !(hash1 == hash2);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00019D34 File Offset: 0x00017F34
		public static bool operator <(Hash128 x, Hash128 y)
		{
			bool flag = x.u64_0 != y.u64_0;
			bool flag2;
			if (flag)
			{
				flag2 = x.u64_0 < y.u64_0;
			}
			else
			{
				flag2 = x.u64_1 < y.u64_1;
			}
			return flag2;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00019D7C File Offset: 0x00017F7C
		public static bool operator >(Hash128 x, Hash128 y)
		{
			bool flag = x < y;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = x == y;
				flag2 = !flag3;
			}
			return flag2;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00019DB0 File Offset: 0x00017FB0
		private void ShortHash4(uint data)
		{
			ulong num = this.u64_0;
			ulong num2 = this.u64_1;
			ulong num3 = 16045690984833335023UL;
			ulong num4 = 16045690984833335023UL;
			num4 += 288230376151711744UL;
			num3 += (ulong)data;
			Hash128.ShortEnd(ref num, ref num2, ref num3, ref num4);
			this.u64_0 = num;
			this.u64_1 = num2;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00019E10 File Offset: 0x00018010
		private static void ShortEnd(ref ulong h0, ref ulong h1, ref ulong h2, ref ulong h3)
		{
			h3 ^= h2;
			Hash128.Rot64(ref h2, 15);
			h3 += h2;
			h0 ^= h3;
			Hash128.Rot64(ref h3, 52);
			h0 += h3;
			h1 ^= h0;
			Hash128.Rot64(ref h0, 26);
			h1 += h0;
			h2 ^= h1;
			Hash128.Rot64(ref h1, 51);
			h2 += h1;
			h3 ^= h2;
			Hash128.Rot64(ref h2, 28);
			h3 += h2;
			h0 ^= h3;
			Hash128.Rot64(ref h3, 9);
			h0 += h3;
			h1 ^= h0;
			Hash128.Rot64(ref h0, 47);
			h1 += h0;
			h2 ^= h1;
			Hash128.Rot64(ref h1, 54);
			h2 += h1;
			h3 ^= h2;
			Hash128.Rot64(ref h2, 32);
			h3 += h2;
			h0 ^= h3;
			Hash128.Rot64(ref h3, 25);
			h0 += h3;
			h1 ^= h0;
			Hash128.Rot64(ref h0, 63);
			h1 += h0;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00019F1B File Offset: 0x0001811B
		private static void Rot64(ref ulong x, int k)
		{
			x = (x << k) | (x >> 64 - k);
		}

		// Token: 0x06001312 RID: 4882
		[MethodImpl(4096)]
		private static extern void Parse_Injected(string hashString, out Hash128 ret);

		// Token: 0x06001313 RID: 4883
		[MethodImpl(4096)]
		private static extern string Hash128ToStringImpl_Injected(ref Hash128 hash);

		// Token: 0x040005D4 RID: 1492
		internal ulong u64_0;

		// Token: 0x040005D5 RID: 1493
		internal ulong u64_1;

		// Token: 0x040005D6 RID: 1494
		private const ulong kConst = 16045690984833335023UL;
	}
}
