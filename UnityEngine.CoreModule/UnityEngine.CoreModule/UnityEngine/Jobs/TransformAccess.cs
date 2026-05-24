using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Jobs
{
	// Token: 0x02000285 RID: 645
	[NativeHeader("Runtime/Transform/ScriptBindings/TransformAccess.bindings.h")]
	public struct TransformAccess
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x0002CED4 File Offset: 0x0002B0D4
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x0002CEF0 File Offset: 0x0002B0F0
		public Vector3 position
		{
			get
			{
				Vector3 vector;
				TransformAccess.GetPosition(ref this, out vector);
				return vector;
			}
			set
			{
				TransformAccess.SetPosition(ref this, ref value);
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x0002CEFC File Offset: 0x0002B0FC
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x0002CF18 File Offset: 0x0002B118
		public Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				TransformAccess.GetRotation(ref this, out quaternion);
				return quaternion;
			}
			set
			{
				TransformAccess.SetRotation(ref this, ref value);
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x0002CF24 File Offset: 0x0002B124
		// (set) Token: 0x06001BE2 RID: 7138 RVA: 0x0002CF40 File Offset: 0x0002B140
		public Vector3 localPosition
		{
			get
			{
				Vector3 vector;
				TransformAccess.GetLocalPosition(ref this, out vector);
				return vector;
			}
			set
			{
				TransformAccess.SetLocalPosition(ref this, ref value);
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x0002CF4C File Offset: 0x0002B14C
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x0002CF68 File Offset: 0x0002B168
		public Quaternion localRotation
		{
			get
			{
				Quaternion quaternion;
				TransformAccess.GetLocalRotation(ref this, out quaternion);
				return quaternion;
			}
			set
			{
				TransformAccess.SetLocalRotation(ref this, ref value);
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0002CF74 File Offset: 0x0002B174
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x0002CF90 File Offset: 0x0002B190
		public Vector3 localScale
		{
			get
			{
				Vector3 vector;
				TransformAccess.GetLocalScale(ref this, out vector);
				return vector;
			}
			set
			{
				TransformAccess.SetLocalScale(ref this, ref value);
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x0002CF9C File Offset: 0x0002B19C
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				TransformAccess.GetLocalToWorldMatrix(ref this, out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x0002CFB8 File Offset: 0x0002B1B8
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				TransformAccess.GetWorldToLocalMatrix(ref this, out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x0002CFD4 File Offset: 0x0002B1D4
		public bool isValid
		{
			get
			{
				return this.hierarchy != IntPtr.Zero;
			}
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0002CFE6 File Offset: 0x0002B1E6
		public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			TransformAccess.SetPositionAndRotation_Internal(ref this, ref position, ref rotation);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0002CFF4 File Offset: 0x0002B1F4
		public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
		{
			TransformAccess.SetLocalPositionAndRotation_Internal(ref this, ref localPosition, ref localRotation);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0002D002 File Offset: 0x0002B202
		public void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
		{
			TransformAccess.GetPositionAndRotation_Internal(ref this, out position, out rotation);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0002D00E File Offset: 0x0002B20E
		public void GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation)
		{
			TransformAccess.GetLocalPositionAndRotation_Internal(ref this, out localPosition, out localRotation);
		}

		// Token: 0x06001BEE RID: 7150
		[NativeMethod(Name = "TransformAccessBindings::SetPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetPositionAndRotation_Internal(ref TransformAccess access, ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06001BEF RID: 7151
		[NativeMethod(Name = "TransformAccessBindings::SetLocalPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetLocalPositionAndRotation_Internal(ref TransformAccess access, ref Vector3 localPosition, ref Quaternion localRotation);

		// Token: 0x06001BF0 RID: 7152
		[NativeMethod(Name = "TransformAccessBindings::GetPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetPositionAndRotation_Internal(ref TransformAccess access, out Vector3 position, out Quaternion rotation);

		// Token: 0x06001BF1 RID: 7153
		[NativeMethod(Name = "TransformAccessBindings::GetLocalPositionAndRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetLocalPositionAndRotation_Internal(ref TransformAccess access, out Vector3 localPosition, out Quaternion localRotation);

		// Token: 0x06001BF2 RID: 7154
		[NativeMethod(Name = "TransformAccessBindings::GetPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetPosition(ref TransformAccess access, out Vector3 p);

		// Token: 0x06001BF3 RID: 7155
		[NativeMethod(Name = "TransformAccessBindings::SetPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetPosition(ref TransformAccess access, ref Vector3 p);

		// Token: 0x06001BF4 RID: 7156
		[NativeMethod(Name = "TransformAccessBindings::GetRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetRotation(ref TransformAccess access, out Quaternion r);

		// Token: 0x06001BF5 RID: 7157
		[NativeMethod(Name = "TransformAccessBindings::SetRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetRotation(ref TransformAccess access, ref Quaternion r);

		// Token: 0x06001BF6 RID: 7158
		[NativeMethod(Name = "TransformAccessBindings::GetLocalPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetLocalPosition(ref TransformAccess access, out Vector3 p);

		// Token: 0x06001BF7 RID: 7159
		[NativeMethod(Name = "TransformAccessBindings::SetLocalPosition", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetLocalPosition(ref TransformAccess access, ref Vector3 p);

		// Token: 0x06001BF8 RID: 7160
		[NativeMethod(Name = "TransformAccessBindings::GetLocalRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetLocalRotation(ref TransformAccess access, out Quaternion r);

		// Token: 0x06001BF9 RID: 7161
		[NativeMethod(Name = "TransformAccessBindings::SetLocalRotation", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetLocalRotation(ref TransformAccess access, ref Quaternion r);

		// Token: 0x06001BFA RID: 7162
		[NativeMethod(Name = "TransformAccessBindings::GetLocalScale", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetLocalScale(ref TransformAccess access, out Vector3 r);

		// Token: 0x06001BFB RID: 7163
		[NativeMethod(Name = "TransformAccessBindings::SetLocalScale", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void SetLocalScale(ref TransformAccess access, ref Vector3 r);

		// Token: 0x06001BFC RID: 7164
		[NativeMethod(Name = "TransformAccessBindings::GetLocalToWorldMatrix", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetLocalToWorldMatrix(ref TransformAccess access, out Matrix4x4 m);

		// Token: 0x06001BFD RID: 7165
		[NativeMethod(Name = "TransformAccessBindings::GetWorldToLocalMatrix", IsThreadSafe = true, IsFreeFunction = true, ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void GetWorldToLocalMatrix(ref TransformAccess access, out Matrix4x4 m);

		// Token: 0x06001BFE RID: 7166 RVA: 0x0002D01C File Offset: 0x0002B21C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(256)]
		internal void CheckHierarchyValid()
		{
			bool flag = !this.isValid;
			if (flag)
			{
				throw new NullReferenceException("The TransformAccess is not valid and points to an invalid hierarchy");
			}
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00004557 File Offset: 0x00002757
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(256)]
		internal void MarkReadWrite()
		{
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00004557 File Offset: 0x00002757
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(256)]
		internal void MarkReadOnly()
		{
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00004557 File Offset: 0x00002757
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(256)]
		private void CheckWriteAccess()
		{
		}

		// Token: 0x04000922 RID: 2338
		private IntPtr hierarchy;

		// Token: 0x04000923 RID: 2339
		private int index;
	}
}
