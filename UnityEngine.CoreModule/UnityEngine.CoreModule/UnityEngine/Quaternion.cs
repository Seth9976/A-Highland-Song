using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001C6 RID: 454
	[NativeHeader("Runtime/Math/MathScripting.h")]
	[UsedByNativeCode]
	[Il2CppEagerStaticClassConstruction]
	[NativeType(Header = "Runtime/Math/Quaternion.h")]
	public struct Quaternion : IEquatable<Quaternion>, IFormattable
	{
		// Token: 0x06001447 RID: 5191 RVA: 0x0001EE94 File Offset: 0x0001D094
		[FreeFunction("FromToQuaternionSafe", IsThreadSafe = true)]
		public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			Quaternion quaternion;
			Quaternion.FromToRotation_Injected(ref fromDirection, ref toDirection, out quaternion);
			return quaternion;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0001EEB0 File Offset: 0x0001D0B0
		[FreeFunction(IsThreadSafe = true)]
		public static Quaternion Inverse(Quaternion rotation)
		{
			Quaternion quaternion;
			Quaternion.Inverse_Injected(ref rotation, out quaternion);
			return quaternion;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0001EEC8 File Offset: 0x0001D0C8
		[FreeFunction("QuaternionScripting::Slerp", IsThreadSafe = true)]
		public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
		{
			Quaternion quaternion;
			Quaternion.Slerp_Injected(ref a, ref b, t, out quaternion);
			return quaternion;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0001EEE4 File Offset: 0x0001D0E4
		[FreeFunction("QuaternionScripting::SlerpUnclamped", IsThreadSafe = true)]
		public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			Quaternion quaternion;
			Quaternion.SlerpUnclamped_Injected(ref a, ref b, t, out quaternion);
			return quaternion;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0001EF00 File Offset: 0x0001D100
		[FreeFunction("QuaternionScripting::Lerp", IsThreadSafe = true)]
		public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
		{
			Quaternion quaternion;
			Quaternion.Lerp_Injected(ref a, ref b, t, out quaternion);
			return quaternion;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0001EF1C File Offset: 0x0001D11C
		[FreeFunction("QuaternionScripting::LerpUnclamped", IsThreadSafe = true)]
		public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			Quaternion quaternion;
			Quaternion.LerpUnclamped_Injected(ref a, ref b, t, out quaternion);
			return quaternion;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0001EF38 File Offset: 0x0001D138
		[FreeFunction("EulerToQuaternion", IsThreadSafe = true)]
		private static Quaternion Internal_FromEulerRad(Vector3 euler)
		{
			Quaternion quaternion;
			Quaternion.Internal_FromEulerRad_Injected(ref euler, out quaternion);
			return quaternion;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0001EF50 File Offset: 0x0001D150
		[FreeFunction("QuaternionScripting::ToEuler", IsThreadSafe = true)]
		private static Vector3 Internal_ToEulerRad(Quaternion rotation)
		{
			Vector3 vector;
			Quaternion.Internal_ToEulerRad_Injected(ref rotation, out vector);
			return vector;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0001EF67 File Offset: 0x0001D167
		[FreeFunction("QuaternionScripting::ToAxisAngle", IsThreadSafe = true)]
		private static void Internal_ToAxisAngleRad(Quaternion q, out Vector3 axis, out float angle)
		{
			Quaternion.Internal_ToAxisAngleRad_Injected(ref q, out axis, out angle);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0001EF74 File Offset: 0x0001D174
		[FreeFunction("QuaternionScripting::AngleAxis", IsThreadSafe = true)]
		public static Quaternion AngleAxis(float angle, Vector3 axis)
		{
			Quaternion quaternion;
			Quaternion.AngleAxis_Injected(angle, ref axis, out quaternion);
			return quaternion;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0001EF8C File Offset: 0x0001D18C
		[FreeFunction("QuaternionScripting::LookRotation", IsThreadSafe = true)]
		public static Quaternion LookRotation(Vector3 forward, [DefaultValue("Vector3.up")] Vector3 upwards)
		{
			Quaternion quaternion;
			Quaternion.LookRotation_Injected(ref forward, ref upwards, out quaternion);
			return quaternion;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0001EFA8 File Offset: 0x0001D1A8
		[ExcludeFromDocs]
		public static Quaternion LookRotation(Vector3 forward)
		{
			return Quaternion.LookRotation(forward, Vector3.up);
		}

		// Token: 0x17000420 RID: 1056
		public float this[int index]
		{
			[MethodImpl(256)]
			get
			{
				float num;
				switch (index)
				{
				case 0:
					num = this.x;
					break;
				case 1:
					num = this.y;
					break;
				case 2:
					num = this.z;
					break;
				case 3:
					num = this.w;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
				return num;
			}
			[MethodImpl(256)]
			set
			{
				switch (index)
				{
				case 0:
					this.x = value;
					break;
				case 1:
					this.y = value;
					break;
				case 2:
					this.z = value;
					break;
				case 3:
					this.w = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0001F07D File Offset: 0x0001D27D
		[MethodImpl(256)]
		public Quaternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0001F07D File Offset: 0x0001D27D
		[MethodImpl(256)]
		public void Set(float newX, float newY, float newZ, float newW)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
			this.w = newW;
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0001F0A0 File Offset: 0x0001D2A0
		public static Quaternion identity
		{
			[MethodImpl(256)]
			get
			{
				return Quaternion.identityQuaternion;
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0001F0B8 File Offset: 0x0001D2B8
		[MethodImpl(256)]
		public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
		{
			return new Quaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		public static Vector3 operator *(Quaternion rotation, Vector3 point)
		{
			float num = rotation.x * 2f;
			float num2 = rotation.y * 2f;
			float num3 = rotation.z * 2f;
			float num4 = rotation.x * num;
			float num5 = rotation.y * num2;
			float num6 = rotation.z * num3;
			float num7 = rotation.x * num2;
			float num8 = rotation.x * num3;
			float num9 = rotation.y * num3;
			float num10 = rotation.w * num;
			float num11 = rotation.w * num2;
			float num12 = rotation.w * num3;
			Vector3 vector;
			vector.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
			vector.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
			vector.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
			return vector;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0001F2DC File Offset: 0x0001D4DC
		[MethodImpl(256)]
		private static bool IsEqualUsingDot(float dot)
		{
			return dot > 0.999999f;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0001F2F8 File Offset: 0x0001D4F8
		[MethodImpl(256)]
		public static bool operator ==(Quaternion lhs, Quaternion rhs)
		{
			return Quaternion.IsEqualUsingDot(Quaternion.Dot(lhs, rhs));
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0001F318 File Offset: 0x0001D518
		[MethodImpl(256)]
		public static bool operator !=(Quaternion lhs, Quaternion rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0001F334 File Offset: 0x0001D534
		[MethodImpl(256)]
		public static float Dot(Quaternion a, Quaternion b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0001F380 File Offset: 0x0001D580
		[ExcludeFromDocs]
		[MethodImpl(256)]
		public void SetLookRotation(Vector3 view)
		{
			Vector3 up = Vector3.up;
			this.SetLookRotation(view, up);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0001F39D File Offset: 0x0001D59D
		[MethodImpl(256)]
		public void SetLookRotation(Vector3 view, [DefaultValue("Vector3.up")] Vector3 up)
		{
			this = Quaternion.LookRotation(view, up);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0001F3B0 File Offset: 0x0001D5B0
		[MethodImpl(256)]
		public static float Angle(Quaternion a, Quaternion b)
		{
			float num = Mathf.Min(Mathf.Abs(Quaternion.Dot(a, b)), 1f);
			return Quaternion.IsEqualUsingDot(num) ? 0f : (Mathf.Acos(num) * 2f * 57.29578f);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0001F3FC File Offset: 0x0001D5FC
		private static Vector3 Internal_MakePositive(Vector3 euler)
		{
			float num = -0.005729578f;
			float num2 = 360f + num;
			bool flag = euler.x < num;
			if (flag)
			{
				euler.x += 360f;
			}
			else
			{
				bool flag2 = euler.x > num2;
				if (flag2)
				{
					euler.x -= 360f;
				}
			}
			bool flag3 = euler.y < num;
			if (flag3)
			{
				euler.y += 360f;
			}
			else
			{
				bool flag4 = euler.y > num2;
				if (flag4)
				{
					euler.y -= 360f;
				}
			}
			bool flag5 = euler.z < num;
			if (flag5)
			{
				euler.z += 360f;
			}
			else
			{
				bool flag6 = euler.z > num2;
				if (flag6)
				{
					euler.z -= 360f;
				}
			}
			return euler;
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x0001F4DC File Offset: 0x0001D6DC
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x0001F508 File Offset: 0x0001D708
		public Vector3 eulerAngles
		{
			[MethodImpl(256)]
			get
			{
				return Quaternion.Internal_MakePositive(Quaternion.Internal_ToEulerRad(this) * 57.29578f);
			}
			[MethodImpl(256)]
			set
			{
				this = Quaternion.Internal_FromEulerRad(value * 0.017453292f);
			}
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0001F524 File Offset: 0x0001D724
		[MethodImpl(256)]
		public static Quaternion Euler(float x, float y, float z)
		{
			return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z) * 0.017453292f);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0001F550 File Offset: 0x0001D750
		[MethodImpl(256)]
		public static Quaternion Euler(Vector3 euler)
		{
			return Quaternion.Internal_FromEulerRad(euler * 0.017453292f);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0001F572 File Offset: 0x0001D772
		[MethodImpl(256)]
		public void ToAngleAxis(out float angle, out Vector3 axis)
		{
			Quaternion.Internal_ToAxisAngleRad(this, out axis, out angle);
			angle *= 57.29578f;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0001F58D File Offset: 0x0001D78D
		[MethodImpl(256)]
		public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			this = Quaternion.FromToRotation(fromDirection, toDirection);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0001F5A0 File Offset: 0x0001D7A0
		[MethodImpl(256)]
		public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
		{
			float num = Quaternion.Angle(from, to);
			bool flag = num == 0f;
			Quaternion quaternion;
			if (flag)
			{
				quaternion = to;
			}
			else
			{
				quaternion = Quaternion.SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / num));
			}
			return quaternion;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0001F5E0 File Offset: 0x0001D7E0
		[MethodImpl(256)]
		public static Quaternion Normalize(Quaternion q)
		{
			float num = Mathf.Sqrt(Quaternion.Dot(q, q));
			bool flag = num < Mathf.Epsilon;
			Quaternion quaternion;
			if (flag)
			{
				quaternion = Quaternion.identity;
			}
			else
			{
				quaternion = new Quaternion(q.x / num, q.y / num, q.z / num, q.w / num);
			}
			return quaternion;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0001F638 File Offset: 0x0001D838
		[MethodImpl(256)]
		public void Normalize()
		{
			this = Quaternion.Normalize(this);
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x0001F64C File Offset: 0x0001D84C
		public Quaternion normalized
		{
			[MethodImpl(256)]
			get
			{
				return Quaternion.Normalize(this);
			}
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0001F66C File Offset: 0x0001D86C
		[MethodImpl(256)]
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ (this.y.GetHashCode() << 2) ^ (this.z.GetHashCode() >> 2) ^ (this.w.GetHashCode() >> 1);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0001F6B4 File Offset: 0x0001D8B4
		[MethodImpl(256)]
		public override bool Equals(object other)
		{
			bool flag = !(other is Quaternion);
			return !flag && this.Equals((Quaternion)other);
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0001F6E8 File Offset: 0x0001D8E8
		[MethodImpl(256)]
		public bool Equals(Quaternion other)
		{
			return this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z) && this.w.Equals(other.w);
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0001F748 File Offset: 0x0001D948
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0001F764 File Offset: 0x0001D964
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0001F780 File Offset: 0x0001D980
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = string.IsNullOrEmpty(format);
			if (flag)
			{
				format = "F5";
			}
			bool flag2 = formatProvider == null;
			if (flag2)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format, formatProvider),
				this.y.ToString(format, formatProvider),
				this.z.ToString(format, formatProvider),
				this.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0001F808 File Offset: 0x0001DA08
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public static Quaternion EulerRotation(float x, float y, float z)
		{
			return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0001F828 File Offset: 0x0001DA28
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public static Quaternion EulerRotation(Vector3 euler)
		{
			return Quaternion.Internal_FromEulerRad(euler);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0001F840 File Offset: 0x0001DA40
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public void SetEulerRotation(float x, float y, float z)
		{
			this = Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0001F856 File Offset: 0x0001DA56
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public void SetEulerRotation(Vector3 euler)
		{
			this = Quaternion.Internal_FromEulerRad(euler);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0001F868 File Offset: 0x0001DA68
		[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public Vector3 ToEuler()
		{
			return Quaternion.Internal_ToEulerRad(this);
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0001F888 File Offset: 0x0001DA88
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public static Quaternion EulerAngles(float x, float y, float z)
		{
			return Quaternion.Internal_FromEulerRad(new Vector3(x, y, z));
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0001F8A8 File Offset: 0x0001DAA8
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public static Quaternion EulerAngles(Vector3 euler)
		{
			return Quaternion.Internal_FromEulerRad(euler);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0001F8C0 File Offset: 0x0001DAC0
		[Obsolete("Use Quaternion.ToAngleAxis instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public void ToAxisAngle(out Vector3 axis, out float angle)
		{
			Quaternion.Internal_ToAxisAngleRad(this, out axis, out angle);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0001F8D1 File Offset: 0x0001DAD1
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public void SetEulerAngles(float x, float y, float z)
		{
			this.SetEulerRotation(new Vector3(x, y, z));
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0001F8E3 File Offset: 0x0001DAE3
		[Obsolete("Use Quaternion.Euler instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public void SetEulerAngles(Vector3 euler)
		{
			this = Quaternion.EulerRotation(euler);
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0001F8F4 File Offset: 0x0001DAF4
		[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public static Vector3 ToEulerAngles(Quaternion rotation)
		{
			return Quaternion.Internal_ToEulerRad(rotation);
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0001F90C File Offset: 0x0001DB0C
		[Obsolete("Use Quaternion.eulerAngles instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public Vector3 ToEulerAngles()
		{
			return Quaternion.Internal_ToEulerRad(this);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0001F929 File Offset: 0x0001DB29
		[Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees.")]
		[MethodImpl(256)]
		public void SetAxisAngle(Vector3 axis, float angle)
		{
			this = Quaternion.AxisAngle(axis, angle);
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0001F93C File Offset: 0x0001DB3C
		[Obsolete("Use Quaternion.AngleAxis instead. This function was deprecated because it uses radians instead of degrees")]
		[MethodImpl(256)]
		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			return Quaternion.AngleAxis(57.29578f * angle, axis);
		}

		// Token: 0x06001481 RID: 5249
		[MethodImpl(4096)]
		private static extern void FromToRotation_Injected(ref Vector3 fromDirection, ref Vector3 toDirection, out Quaternion ret);

		// Token: 0x06001482 RID: 5250
		[MethodImpl(4096)]
		private static extern void Inverse_Injected(ref Quaternion rotation, out Quaternion ret);

		// Token: 0x06001483 RID: 5251
		[MethodImpl(4096)]
		private static extern void Slerp_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

		// Token: 0x06001484 RID: 5252
		[MethodImpl(4096)]
		private static extern void SlerpUnclamped_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

		// Token: 0x06001485 RID: 5253
		[MethodImpl(4096)]
		private static extern void Lerp_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

		// Token: 0x06001486 RID: 5254
		[MethodImpl(4096)]
		private static extern void LerpUnclamped_Injected(ref Quaternion a, ref Quaternion b, float t, out Quaternion ret);

		// Token: 0x06001487 RID: 5255
		[MethodImpl(4096)]
		private static extern void Internal_FromEulerRad_Injected(ref Vector3 euler, out Quaternion ret);

		// Token: 0x06001488 RID: 5256
		[MethodImpl(4096)]
		private static extern void Internal_ToEulerRad_Injected(ref Quaternion rotation, out Vector3 ret);

		// Token: 0x06001489 RID: 5257
		[MethodImpl(4096)]
		private static extern void Internal_ToAxisAngleRad_Injected(ref Quaternion q, out Vector3 axis, out float angle);

		// Token: 0x0600148A RID: 5258
		[MethodImpl(4096)]
		private static extern void AngleAxis_Injected(float angle, ref Vector3 axis, out Quaternion ret);

		// Token: 0x0600148B RID: 5259
		[MethodImpl(4096)]
		private static extern void LookRotation_Injected(ref Vector3 forward, [DefaultValue("Vector3.up")] ref Vector3 upwards, out Quaternion ret);

		// Token: 0x0400076F RID: 1903
		public float x;

		// Token: 0x04000770 RID: 1904
		public float y;

		// Token: 0x04000771 RID: 1905
		public float z;

		// Token: 0x04000772 RID: 1906
		public float w;

		// Token: 0x04000773 RID: 1907
		private static readonly Quaternion identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);

		// Token: 0x04000774 RID: 1908
		public const float kEpsilon = 1E-06f;
	}
}
