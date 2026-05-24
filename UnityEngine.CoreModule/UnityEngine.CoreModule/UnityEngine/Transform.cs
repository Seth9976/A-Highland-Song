using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200025F RID: 607
	[NativeHeader("Configuration/UnityConfigure.h")]
	[NativeHeader("Runtime/Transform/Transform.h")]
	[NativeHeader("Runtime/Transform/ScriptBindings/TransformScriptBindings.h")]
	[RequiredByNativeCode]
	public class Transform : Component, IEnumerable
	{
		// Token: 0x06001A29 RID: 6697 RVA: 0x0002A62A File Offset: 0x0002882A
		protected Transform()
		{
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x0002A634 File Offset: 0x00028834
		// (set) Token: 0x06001A2B RID: 6699 RVA: 0x0002A64A File Offset: 0x0002884A
		public Vector3 position
		{
			get
			{
				Vector3 vector;
				this.get_position_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_position_Injected(ref value);
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x0002A654 File Offset: 0x00028854
		// (set) Token: 0x06001A2D RID: 6701 RVA: 0x0002A66A File Offset: 0x0002886A
		public Vector3 localPosition
		{
			get
			{
				Vector3 vector;
				this.get_localPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_localPosition_Injected(ref value);
			}
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0002A674 File Offset: 0x00028874
		internal Vector3 GetLocalEulerAngles(RotationOrder order)
		{
			Vector3 vector;
			this.GetLocalEulerAngles_Injected(order, out vector);
			return vector;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0002A68B File Offset: 0x0002888B
		internal void SetLocalEulerAngles(Vector3 euler, RotationOrder order)
		{
			this.SetLocalEulerAngles_Injected(ref euler, order);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0002A696 File Offset: 0x00028896
		[NativeConditional("UNITY_EDITOR")]
		internal void SetLocalEulerHint(Vector3 euler)
		{
			this.SetLocalEulerHint_Injected(ref euler);
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x0002A6A0 File Offset: 0x000288A0
		// (set) Token: 0x06001A32 RID: 6706 RVA: 0x0002A6C0 File Offset: 0x000288C0
		public Vector3 eulerAngles
		{
			get
			{
				return this.rotation.eulerAngles;
			}
			set
			{
				this.rotation = Quaternion.Euler(value);
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0002A6D0 File Offset: 0x000288D0
		// (set) Token: 0x06001A34 RID: 6708 RVA: 0x0002A6F0 File Offset: 0x000288F0
		public Vector3 localEulerAngles
		{
			get
			{
				return this.localRotation.eulerAngles;
			}
			set
			{
				this.localRotation = Quaternion.Euler(value);
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x0002A700 File Offset: 0x00028900
		// (set) Token: 0x06001A36 RID: 6710 RVA: 0x0002A722 File Offset: 0x00028922
		public Vector3 right
		{
			get
			{
				return this.rotation * Vector3.right;
			}
			set
			{
				this.rotation = Quaternion.FromToRotation(Vector3.right, value);
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0002A738 File Offset: 0x00028938
		// (set) Token: 0x06001A38 RID: 6712 RVA: 0x0002A75A File Offset: 0x0002895A
		public Vector3 up
		{
			get
			{
				return this.rotation * Vector3.up;
			}
			set
			{
				this.rotation = Quaternion.FromToRotation(Vector3.up, value);
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x0002A770 File Offset: 0x00028970
		// (set) Token: 0x06001A3A RID: 6714 RVA: 0x0002A792 File Offset: 0x00028992
		public Vector3 forward
		{
			get
			{
				return this.rotation * Vector3.forward;
			}
			set
			{
				this.rotation = Quaternion.LookRotation(value);
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x0002A7A4 File Offset: 0x000289A4
		// (set) Token: 0x06001A3C RID: 6716 RVA: 0x0002A7BA File Offset: 0x000289BA
		public Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				this.get_rotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_rotation_Injected(ref value);
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x0002A7C4 File Offset: 0x000289C4
		// (set) Token: 0x06001A3E RID: 6718 RVA: 0x0002A7DA File Offset: 0x000289DA
		public Quaternion localRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_localRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_localRotation_Injected(ref value);
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0002A7E4 File Offset: 0x000289E4
		// (set) Token: 0x06001A40 RID: 6720 RVA: 0x0002A7FC File Offset: 0x000289FC
		[NativeConditional("UNITY_EDITOR")]
		internal RotationOrder rotationOrder
		{
			get
			{
				return (RotationOrder)this.GetRotationOrderInternal();
			}
			set
			{
				this.SetRotationOrderInternal(value);
			}
		}

		// Token: 0x06001A41 RID: 6721
		[NativeConditional("UNITY_EDITOR")]
		[NativeMethod("GetRotationOrder")]
		[MethodImpl(4096)]
		internal extern int GetRotationOrderInternal();

		// Token: 0x06001A42 RID: 6722
		[NativeMethod("SetRotationOrder")]
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(4096)]
		internal extern void SetRotationOrderInternal(RotationOrder rotationOrder);

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x0002A808 File Offset: 0x00028A08
		// (set) Token: 0x06001A44 RID: 6724 RVA: 0x0002A81E File Offset: 0x00028A1E
		public Vector3 localScale
		{
			get
			{
				Vector3 vector;
				this.get_localScale_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_localScale_Injected(ref value);
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x0002A828 File Offset: 0x00028A28
		// (set) Token: 0x06001A46 RID: 6726 RVA: 0x0002A840 File Offset: 0x00028A40
		public Transform parent
		{
			get
			{
				return this.parentInternal;
			}
			set
			{
				bool flag = this is RectTransform;
				if (flag)
				{
					Debug.LogWarning("Parent of RectTransform is being set with parent property. Consider using the SetParent method instead, with the worldPositionStays argument set to false. This will retain local orientation and scale rather than world orientation and scale, which can prevent common UI scaling issues.", this);
				}
				this.parentInternal = value;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0002A870 File Offset: 0x00028A70
		// (set) Token: 0x06001A48 RID: 6728 RVA: 0x0002A888 File Offset: 0x00028A88
		internal Transform parentInternal
		{
			get
			{
				return this.GetParent();
			}
			set
			{
				this.SetParent(value);
			}
		}

		// Token: 0x06001A49 RID: 6729
		[MethodImpl(4096)]
		private extern Transform GetParent();

		// Token: 0x06001A4A RID: 6730 RVA: 0x0002A893 File Offset: 0x00028A93
		public void SetParent(Transform p)
		{
			this.SetParent(p, true);
		}

		// Token: 0x06001A4B RID: 6731
		[FreeFunction("SetParent", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void SetParent(Transform parent, bool worldPositionStays);

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x0002A8A0 File Offset: 0x00028AA0
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_worldToLocalMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0002A8B8 File Offset: 0x00028AB8
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_localToWorldMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0002A8CE File Offset: 0x00028ACE
		public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			this.SetPositionAndRotation_Injected(ref position, ref rotation);
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0002A8DA File Offset: 0x00028ADA
		public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
		{
			this.SetLocalPositionAndRotation_Injected(ref localPosition, ref localRotation);
		}

		// Token: 0x06001A50 RID: 6736
		[MethodImpl(4096)]
		public extern void GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

		// Token: 0x06001A51 RID: 6737
		[MethodImpl(4096)]
		public extern void GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);

		// Token: 0x06001A52 RID: 6738 RVA: 0x0002A8E8 File Offset: 0x00028AE8
		public void Translate(Vector3 translation, [DefaultValue("Space.Self")] Space relativeTo)
		{
			bool flag = relativeTo == Space.World;
			if (flag)
			{
				this.position += translation;
			}
			else
			{
				this.position += this.TransformDirection(translation);
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0002A92C File Offset: 0x00028B2C
		public void Translate(Vector3 translation)
		{
			this.Translate(translation, Space.Self);
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0002A938 File Offset: 0x00028B38
		public void Translate(float x, float y, float z, [DefaultValue("Space.Self")] Space relativeTo)
		{
			this.Translate(new Vector3(x, y, z), relativeTo);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0002A94C File Offset: 0x00028B4C
		public void Translate(float x, float y, float z)
		{
			this.Translate(new Vector3(x, y, z), Space.Self);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0002A960 File Offset: 0x00028B60
		public void Translate(Vector3 translation, Transform relativeTo)
		{
			bool flag = relativeTo;
			if (flag)
			{
				this.position += relativeTo.TransformDirection(translation);
			}
			else
			{
				this.position += translation;
			}
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0002A9A6 File Offset: 0x00028BA6
		public void Translate(float x, float y, float z, Transform relativeTo)
		{
			this.Translate(new Vector3(x, y, z), relativeTo);
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0002A9BC File Offset: 0x00028BBC
		public void Rotate(Vector3 eulers, [DefaultValue("Space.Self")] Space relativeTo)
		{
			Quaternion quaternion = Quaternion.Euler(eulers.x, eulers.y, eulers.z);
			bool flag = relativeTo == Space.Self;
			if (flag)
			{
				this.localRotation *= quaternion;
			}
			else
			{
				this.rotation *= Quaternion.Inverse(this.rotation) * quaternion * this.rotation;
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0002AA2F File Offset: 0x00028C2F
		public void Rotate(Vector3 eulers)
		{
			this.Rotate(eulers, Space.Self);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0002AA3B File Offset: 0x00028C3B
		public void Rotate(float xAngle, float yAngle, float zAngle, [DefaultValue("Space.Self")] Space relativeTo)
		{
			this.Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0002AA4F File Offset: 0x00028C4F
		public void Rotate(float xAngle, float yAngle, float zAngle)
		{
			this.Rotate(new Vector3(xAngle, yAngle, zAngle), Space.Self);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x0002AA62 File Offset: 0x00028C62
		[NativeMethod("RotateAround")]
		internal void RotateAroundInternal(Vector3 axis, float angle)
		{
			this.RotateAroundInternal_Injected(ref axis, angle);
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0002AA70 File Offset: 0x00028C70
		public void Rotate(Vector3 axis, float angle, [DefaultValue("Space.Self")] Space relativeTo)
		{
			bool flag = relativeTo == Space.Self;
			if (flag)
			{
				this.RotateAroundInternal(base.transform.TransformDirection(axis), angle * 0.017453292f);
			}
			else
			{
				this.RotateAroundInternal(axis, angle * 0.017453292f);
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0002AAB1 File Offset: 0x00028CB1
		public void Rotate(Vector3 axis, float angle)
		{
			this.Rotate(axis, angle, Space.Self);
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0002AAC0 File Offset: 0x00028CC0
		public void RotateAround(Vector3 point, Vector3 axis, float angle)
		{
			Vector3 vector = this.position;
			Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
			Vector3 vector2 = vector - point;
			vector2 = quaternion * vector2;
			vector = point + vector2;
			this.position = vector;
			this.RotateAroundInternal(axis, angle * 0.017453292f);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0002AB0C File Offset: 0x00028D0C
		public void LookAt(Transform target, [DefaultValue("Vector3.up")] Vector3 worldUp)
		{
			bool flag = target;
			if (flag)
			{
				this.LookAt(target.position, worldUp);
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0002AB34 File Offset: 0x00028D34
		public void LookAt(Transform target)
		{
			bool flag = target;
			if (flag)
			{
				this.LookAt(target.position, Vector3.up);
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0002AB5E File Offset: 0x00028D5E
		public void LookAt(Vector3 worldPosition, [DefaultValue("Vector3.up")] Vector3 worldUp)
		{
			this.Internal_LookAt(worldPosition, worldUp);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0002AB6A File Offset: 0x00028D6A
		public void LookAt(Vector3 worldPosition)
		{
			this.Internal_LookAt(worldPosition, Vector3.up);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0002AB7A File Offset: 0x00028D7A
		[FreeFunction("Internal_LookAt", HasExplicitThis = true)]
		private void Internal_LookAt(Vector3 worldPosition, Vector3 worldUp)
		{
			this.Internal_LookAt_Injected(ref worldPosition, ref worldUp);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0002AB88 File Offset: 0x00028D88
		public Vector3 TransformDirection(Vector3 direction)
		{
			Vector3 vector;
			this.TransformDirection_Injected(ref direction, out vector);
			return vector;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0002ABA0 File Offset: 0x00028DA0
		public Vector3 TransformDirection(float x, float y, float z)
		{
			return this.TransformDirection(new Vector3(x, y, z));
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x0002ABC0 File Offset: 0x00028DC0
		public Vector3 InverseTransformDirection(Vector3 direction)
		{
			Vector3 vector;
			this.InverseTransformDirection_Injected(ref direction, out vector);
			return vector;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x0002ABD8 File Offset: 0x00028DD8
		public Vector3 InverseTransformDirection(float x, float y, float z)
		{
			return this.InverseTransformDirection(new Vector3(x, y, z));
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0002ABF8 File Offset: 0x00028DF8
		public Vector3 TransformVector(Vector3 vector)
		{
			Vector3 vector2;
			this.TransformVector_Injected(ref vector, out vector2);
			return vector2;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0002AC10 File Offset: 0x00028E10
		public Vector3 TransformVector(float x, float y, float z)
		{
			return this.TransformVector(new Vector3(x, y, z));
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0002AC30 File Offset: 0x00028E30
		public Vector3 InverseTransformVector(Vector3 vector)
		{
			Vector3 vector2;
			this.InverseTransformVector_Injected(ref vector, out vector2);
			return vector2;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0002AC48 File Offset: 0x00028E48
		public Vector3 InverseTransformVector(float x, float y, float z)
		{
			return this.InverseTransformVector(new Vector3(x, y, z));
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0002AC68 File Offset: 0x00028E68
		public Vector3 TransformPoint(Vector3 position)
		{
			Vector3 vector;
			this.TransformPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0002AC80 File Offset: 0x00028E80
		public Vector3 TransformPoint(float x, float y, float z)
		{
			return this.TransformPoint(new Vector3(x, y, z));
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0002ACA0 File Offset: 0x00028EA0
		public Vector3 InverseTransformPoint(Vector3 position)
		{
			Vector3 vector;
			this.InverseTransformPoint_Injected(ref position, out vector);
			return vector;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0002ACB8 File Offset: 0x00028EB8
		public Vector3 InverseTransformPoint(float x, float y, float z)
		{
			return this.InverseTransformPoint(new Vector3(x, y, z));
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0002ACD8 File Offset: 0x00028ED8
		public Transform root
		{
			get
			{
				return this.GetRoot();
			}
		}

		// Token: 0x06001A72 RID: 6770
		[MethodImpl(4096)]
		private extern Transform GetRoot();

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001A73 RID: 6771
		public extern int childCount
		{
			[NativeMethod("GetChildrenCount")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x06001A74 RID: 6772
		[FreeFunction("DetachChildren", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void DetachChildren();

		// Token: 0x06001A75 RID: 6773
		[MethodImpl(4096)]
		public extern void SetAsFirstSibling();

		// Token: 0x06001A76 RID: 6774
		[MethodImpl(4096)]
		public extern void SetAsLastSibling();

		// Token: 0x06001A77 RID: 6775
		[MethodImpl(4096)]
		public extern void SetSiblingIndex(int index);

		// Token: 0x06001A78 RID: 6776
		[NativeMethod("MoveAfterSiblingInternal")]
		[MethodImpl(4096)]
		internal extern void MoveAfterSibling(Transform transform, bool notifyEditorAndMarkDirty);

		// Token: 0x06001A79 RID: 6777
		[MethodImpl(4096)]
		public extern int GetSiblingIndex();

		// Token: 0x06001A7A RID: 6778
		[FreeFunction]
		[MethodImpl(4096)]
		private static extern Transform FindRelativeTransformWithPath([NotNull("NullExceptionObject")] Transform transform, string path, [DefaultValue("false")] bool isActiveOnly);

		// Token: 0x06001A7B RID: 6779 RVA: 0x0002ACF0 File Offset: 0x00028EF0
		public Transform Find(string n)
		{
			bool flag = n == null;
			if (flag)
			{
				throw new ArgumentNullException("Name cannot be null");
			}
			return Transform.FindRelativeTransformWithPath(this, n, false);
		}

		// Token: 0x06001A7C RID: 6780
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(4096)]
		internal extern void SendTransformChangedScale();

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0002AD20 File Offset: 0x00028F20
		public Vector3 lossyScale
		{
			[NativeMethod("GetWorldScaleLossy")]
			get
			{
				Vector3 vector;
				this.get_lossyScale_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x06001A7E RID: 6782
		[FreeFunction("Internal_IsChildOrSameTransform", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern bool IsChildOf([NotNull("ArgumentNullException")] Transform parent);

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001A7F RID: 6783
		// (set) Token: 0x06001A80 RID: 6784
		[NativeProperty("HasChangedDeprecated")]
		public extern bool hasChanged
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x0002AD38 File Offset: 0x00028F38
		[Obsolete("FindChild has been deprecated. Use Find instead (UnityUpgradable) -> Find([mscorlib] System.String)", false)]
		public Transform FindChild(string n)
		{
			return this.Find(n);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0002AD54 File Offset: 0x00028F54
		public IEnumerator GetEnumerator()
		{
			return new Transform.Enumerator(this);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0002AD6C File Offset: 0x00028F6C
		[Obsolete("warning use Transform.Rotate instead.")]
		public void RotateAround(Vector3 axis, float angle)
		{
			this.RotateAround_Injected(ref axis, angle);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0002AD77 File Offset: 0x00028F77
		[Obsolete("warning use Transform.Rotate instead.")]
		public void RotateAroundLocal(Vector3 axis, float angle)
		{
			this.RotateAroundLocal_Injected(ref axis, angle);
		}

		// Token: 0x06001A85 RID: 6789
		[FreeFunction("GetChild", HasExplicitThis = true)]
		[NativeThrows]
		[MethodImpl(4096)]
		public extern Transform GetChild(int index);

		// Token: 0x06001A86 RID: 6790
		[Obsolete("warning use Transform.childCount instead (UnityUpgradable) -> Transform.childCount", false)]
		[NativeMethod("GetChildrenCount")]
		[MethodImpl(4096)]
		public extern int GetChildCount();

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x0002AD84 File Offset: 0x00028F84
		// (set) Token: 0x06001A88 RID: 6792 RVA: 0x0002AD9C File Offset: 0x00028F9C
		public int hierarchyCapacity
		{
			get
			{
				return this.internal_getHierarchyCapacity();
			}
			set
			{
				this.internal_setHierarchyCapacity(value);
			}
		}

		// Token: 0x06001A89 RID: 6793
		[FreeFunction("GetHierarchyCapacity", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern int internal_getHierarchyCapacity();

		// Token: 0x06001A8A RID: 6794
		[FreeFunction("SetHierarchyCapacity", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void internal_setHierarchyCapacity(int value);

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x0002ADA8 File Offset: 0x00028FA8
		public int hierarchyCount
		{
			get
			{
				return this.internal_getHierarchyCount();
			}
		}

		// Token: 0x06001A8C RID: 6796
		[FreeFunction("GetHierarchyCount", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern int internal_getHierarchyCount();

		// Token: 0x06001A8D RID: 6797
		[NativeConditional("UNITY_EDITOR")]
		[FreeFunction("IsNonUniformScaleTransform", HasExplicitThis = true)]
		[MethodImpl(4096)]
		internal extern bool IsNonUniformScaleTransform();

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0002ADC0 File Offset: 0x00028FC0
		// (set) Token: 0x06001A8F RID: 6799 RVA: 0x0002ADC8 File Offset: 0x00028FC8
		[NativeConditional("UNITY_EDITOR")]
		internal bool constrainProportionsScale
		{
			get
			{
				return this.IsConstrainProportionsScale();
			}
			set
			{
				this.SetConstrainProportionsScale(value);
			}
		}

		// Token: 0x06001A90 RID: 6800
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(4096)]
		private extern void SetConstrainProportionsScale(bool isLinked);

		// Token: 0x06001A91 RID: 6801
		[NativeConditional("UNITY_EDITOR")]
		[MethodImpl(4096)]
		private extern bool IsConstrainProportionsScale();

		// Token: 0x06001A92 RID: 6802
		[MethodImpl(4096)]
		private extern void get_position_Injected(out Vector3 ret);

		// Token: 0x06001A93 RID: 6803
		[MethodImpl(4096)]
		private extern void set_position_Injected(ref Vector3 value);

		// Token: 0x06001A94 RID: 6804
		[MethodImpl(4096)]
		private extern void get_localPosition_Injected(out Vector3 ret);

		// Token: 0x06001A95 RID: 6805
		[MethodImpl(4096)]
		private extern void set_localPosition_Injected(ref Vector3 value);

		// Token: 0x06001A96 RID: 6806
		[MethodImpl(4096)]
		private extern void GetLocalEulerAngles_Injected(RotationOrder order, out Vector3 ret);

		// Token: 0x06001A97 RID: 6807
		[MethodImpl(4096)]
		private extern void SetLocalEulerAngles_Injected(ref Vector3 euler, RotationOrder order);

		// Token: 0x06001A98 RID: 6808
		[MethodImpl(4096)]
		private extern void SetLocalEulerHint_Injected(ref Vector3 euler);

		// Token: 0x06001A99 RID: 6809
		[MethodImpl(4096)]
		private extern void get_rotation_Injected(out Quaternion ret);

		// Token: 0x06001A9A RID: 6810
		[MethodImpl(4096)]
		private extern void set_rotation_Injected(ref Quaternion value);

		// Token: 0x06001A9B RID: 6811
		[MethodImpl(4096)]
		private extern void get_localRotation_Injected(out Quaternion ret);

		// Token: 0x06001A9C RID: 6812
		[MethodImpl(4096)]
		private extern void set_localRotation_Injected(ref Quaternion value);

		// Token: 0x06001A9D RID: 6813
		[MethodImpl(4096)]
		private extern void get_localScale_Injected(out Vector3 ret);

		// Token: 0x06001A9E RID: 6814
		[MethodImpl(4096)]
		private extern void set_localScale_Injected(ref Vector3 value);

		// Token: 0x06001A9F RID: 6815
		[MethodImpl(4096)]
		private extern void get_worldToLocalMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06001AA0 RID: 6816
		[MethodImpl(4096)]
		private extern void get_localToWorldMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06001AA1 RID: 6817
		[MethodImpl(4096)]
		private extern void SetPositionAndRotation_Injected(ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06001AA2 RID: 6818
		[MethodImpl(4096)]
		private extern void SetLocalPositionAndRotation_Injected(ref Vector3 localPosition, ref Quaternion localRotation);

		// Token: 0x06001AA3 RID: 6819
		[MethodImpl(4096)]
		private extern void RotateAroundInternal_Injected(ref Vector3 axis, float angle);

		// Token: 0x06001AA4 RID: 6820
		[MethodImpl(4096)]
		private extern void Internal_LookAt_Injected(ref Vector3 worldPosition, ref Vector3 worldUp);

		// Token: 0x06001AA5 RID: 6821
		[MethodImpl(4096)]
		private extern void TransformDirection_Injected(ref Vector3 direction, out Vector3 ret);

		// Token: 0x06001AA6 RID: 6822
		[MethodImpl(4096)]
		private extern void InverseTransformDirection_Injected(ref Vector3 direction, out Vector3 ret);

		// Token: 0x06001AA7 RID: 6823
		[MethodImpl(4096)]
		private extern void TransformVector_Injected(ref Vector3 vector, out Vector3 ret);

		// Token: 0x06001AA8 RID: 6824
		[MethodImpl(4096)]
		private extern void InverseTransformVector_Injected(ref Vector3 vector, out Vector3 ret);

		// Token: 0x06001AA9 RID: 6825
		[MethodImpl(4096)]
		private extern void TransformPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06001AAA RID: 6826
		[MethodImpl(4096)]
		private extern void InverseTransformPoint_Injected(ref Vector3 position, out Vector3 ret);

		// Token: 0x06001AAB RID: 6827
		[MethodImpl(4096)]
		private extern void get_lossyScale_Injected(out Vector3 ret);

		// Token: 0x06001AAC RID: 6828
		[MethodImpl(4096)]
		private extern void RotateAround_Injected(ref Vector3 axis, float angle);

		// Token: 0x06001AAD RID: 6829
		[MethodImpl(4096)]
		private extern void RotateAroundLocal_Injected(ref Vector3 axis, float angle);

		// Token: 0x02000260 RID: 608
		private class Enumerator : IEnumerator
		{
			// Token: 0x06001AAE RID: 6830 RVA: 0x0002ADD2 File Offset: 0x00028FD2
			internal Enumerator(Transform outer)
			{
				this.outer = outer;
			}

			// Token: 0x17000556 RID: 1366
			// (get) Token: 0x06001AAF RID: 6831 RVA: 0x0002ADEC File Offset: 0x00028FEC
			public object Current
			{
				get
				{
					return this.outer.GetChild(this.currentIndex);
				}
			}

			// Token: 0x06001AB0 RID: 6832 RVA: 0x0002AE10 File Offset: 0x00029010
			public bool MoveNext()
			{
				int childCount = this.outer.childCount;
				int num = this.currentIndex + 1;
				this.currentIndex = num;
				return num < childCount;
			}

			// Token: 0x06001AB1 RID: 6833 RVA: 0x0002AE42 File Offset: 0x00029042
			public void Reset()
			{
				this.currentIndex = -1;
			}

			// Token: 0x040008BC RID: 2236
			private Transform outer;

			// Token: 0x040008BD RID: 2237
			private int currentIndex = -1;
		}
	}
}
