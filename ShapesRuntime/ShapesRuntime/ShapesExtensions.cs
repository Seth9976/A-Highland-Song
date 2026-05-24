using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200005D RID: 93
	internal static class ShapesExtensions
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x0002460C File Offset: 0x0002280C
		public static void ForEach<T>(this IEnumerable<T> elems, Action<T> action)
		{
			foreach (T t in elems)
			{
				action(t);
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00024654 File Offset: 0x00022854
		public static Vector3 Rot90CCW(this Vector3 v)
		{
			return new Vector3(-v.y, v.x);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00024668 File Offset: 0x00022868
		public static int AsInt(this bool b)
		{
			if (!b)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00024670 File Offset: 0x00022870
		public static Vector4 ToVector4(this Rect r)
		{
			return new Vector4(r.x, r.y, r.width, r.height);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00024693 File Offset: 0x00022893
		public static float TaxicabMagnitude(this Vector3 v)
		{
			return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x000246B8 File Offset: 0x000228B8
		public static float AvgComponentMagnitude(this Vector3 v)
		{
			return (Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z)) / 3f;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x000246E3 File Offset: 0x000228E3
		internal static Color ColorSpaceAdjusted(this Color c)
		{
			if (QualitySettings.activeColorSpace != ColorSpace.Linear)
			{
				return c;
			}
			return c.linear;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x000246F6 File Offset: 0x000228F6
		public static void DestroyBranched(this Object obj)
		{
			Object.Destroy(obj);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x000246FE File Offset: 0x000228FE
		public static void TryDestroyInOnDestroy(this Object caller, Object obj)
		{
			if (obj == null)
			{
				return;
			}
			Object.Destroy(obj);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00024710 File Offset: 0x00022910
		public static int Product<T>(this IEnumerable<T> arr, Func<T, int> mulVal)
		{
			int num = 1;
			foreach (T t in arr)
			{
				num *= mulVal(t);
			}
			return num;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00024760 File Offset: 0x00022960
		public static float Product<T>(this IEnumerable<T> arr, Func<T, float> mulVal)
		{
			float num = 1f;
			foreach (T t in arr)
			{
				num *= mulVal(t);
			}
			return num;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000247B4 File Offset: 0x000229B4
		public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(this IEnumerable<T1> source, IEnumerable<T2> second, IEnumerable<T3> third, Func<T1, T2, T3, TResult> func)
		{
			using (IEnumerator<T1> e = source.GetEnumerator())
			{
				using (IEnumerator<T2> e2 = second.GetEnumerator())
				{
					using (IEnumerator<T3> e3 = third.GetEnumerator())
					{
						while (e.MoveNext() && e2.MoveNext() && e3.MoveNext())
						{
							yield return func(e.Current, e2.Current, e3.Current);
						}
					}
					IEnumerator<T3> e3 = null;
				}
				IEnumerator<T2> e2 = null;
			}
			IEnumerator<T1> e = null;
			yield break;
			yield break;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000247D9 File Offset: 0x000229D9
		public static int PopCount(this uint i)
		{
			i -= (i >> 1) & 1431655765U;
			i = (i & 858993459U) + ((i >> 2) & 858993459U);
			i = ((i + (i >> 4)) & 252645135U) * 16843009U >> 24;
			return (int)i;
		}
	}
}
