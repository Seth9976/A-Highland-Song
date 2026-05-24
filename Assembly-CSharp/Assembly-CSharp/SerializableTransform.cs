using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
[Serializable]
public struct SerializableTransform
{
	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x0600124F RID: 4687 RVA: 0x0008464A File Offset: 0x0008284A
	public static SerializableTransform identity
	{
		get
		{
			return new SerializableTransform(Vector3.zero, Quaternion.identity, Vector3.one);
		}
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x00084660 File Offset: 0x00082860
	public SerializableTransform(Vector3 _position)
	{
		this._position = _position;
		this._rotation = Quaternion.identity;
		this._localScale = Vector3.one;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x000846D4 File Offset: 0x000828D4
	public SerializableTransform(Vector3 _position, Quaternion _rotation)
	{
		this._position = _position;
		this._rotation = _rotation;
		this._localScale = Vector3.one;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x00084744 File Offset: 0x00082944
	public SerializableTransform(Vector3 _position, Vector3 _eulerAngles)
	{
		this._position = _position;
		this._rotation = Quaternion.Euler(_eulerAngles);
		this._localScale = Vector3.one;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x000847B8 File Offset: 0x000829B8
	public SerializableTransform(Vector3 _position, Quaternion _rotation, Vector3 _localScale)
	{
		this._position = _position;
		this._rotation = _rotation;
		this._localScale = _localScale;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x00084824 File Offset: 0x00082A24
	public SerializableTransform(Vector3 _position, Vector3 _eulerAngles, Vector3 _localScale)
	{
		this._position = _position;
		this._rotation = Quaternion.Euler(_eulerAngles);
		this._localScale = _localScale;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x00084894 File Offset: 0x00082A94
	public SerializableTransform(Transform transform)
	{
		this._position = transform.position;
		this._rotation = transform.rotation;
		this._localScale = transform.localScale;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x00084910 File Offset: 0x00082B10
	public SerializableTransform(SerializableTransform transform)
	{
		this._position = transform.position;
		this._rotation = transform.rotation;
		this._localScale = transform.localScale;
		this._localToWorldMatrix = Matrix4x4.identity;
		this._localToWorldMatrixSet = false;
		this._worldToLocalMatrix = Matrix4x4.identity;
		this._worldToLocalMatrixSet = false;
		this._localToWorldDirectionMatrix = Matrix4x4.identity;
		this._localToWorldDirectionMatrixSet = false;
		this._worldToLocalDirectionMatrix = Matrix4x4.identity;
		this._worldToLocalDirectionMatrixSet = false;
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x0008498C File Offset: 0x00082B8C
	public void ApplyFrom(Transform transform)
	{
		this.position = transform.position;
		this.rotation = transform.rotation;
		this.localScale = transform.localScale;
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x000849B2 File Offset: 0x00082BB2
	public void ApplyFrom(SerializableTransform transform)
	{
		this.position = transform.position;
		this.rotation = transform.rotation;
		this.localScale = transform.localScale;
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x000849DB File Offset: 0x00082BDB
	public void ApplyTo(Transform transform)
	{
		transform.position = this.position;
		transform.rotation = this.rotation;
		transform.localScale = this.localScale;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x00084A01 File Offset: 0x00082C01
	public void ApplyToLocal(Transform transform)
	{
		transform.localPosition = this.position;
		transform.localRotation = this.rotation;
		transform.localScale = this.localScale;
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x00084A28 File Offset: 0x00082C28
	public static SerializableTransform FromLocal(Transform transform)
	{
		return new SerializableTransform
		{
			position = transform.localPosition,
			rotation = transform.localRotation,
			localScale = transform.localScale
		};
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00084A68 File Offset: 0x00082C68
	public static SerializableTransform Lerp(SerializableTransform t1, SerializableTransform t2, float lerp)
	{
		return new SerializableTransform
		{
			position = Vector3.Lerp(t1.position, t2.position, lerp),
			rotation = Quaternion.Lerp(t1.rotation, t2.rotation, lerp),
			localScale = Vector3.Lerp(t1.localScale, t2.localScale, lerp)
		};
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x00084AD0 File Offset: 0x00082CD0
	public static SerializableTransform LerpUnclamped(SerializableTransform t1, SerializableTransform t2, float lerp)
	{
		return new SerializableTransform
		{
			position = Vector3.LerpUnclamped(t1.position, t2.position, lerp),
			rotation = Quaternion.LerpUnclamped(t1.rotation, t2.rotation, lerp),
			localScale = Vector3.LerpUnclamped(t1.localScale, t2.localScale, lerp)
		};
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x0600125E RID: 4702 RVA: 0x00084B37 File Offset: 0x00082D37
	// (set) Token: 0x0600125F RID: 4703 RVA: 0x00084B3F File Offset: 0x00082D3F
	public Vector3 position
	{
		get
		{
			return this._position;
		}
		set
		{
			this._position = value;
			this._localToWorldMatrixSet = false;
			this._worldToLocalMatrixSet = false;
			this._localToWorldDirectionMatrixSet = false;
			this._worldToLocalDirectionMatrixSet = false;
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06001260 RID: 4704 RVA: 0x00084B64 File Offset: 0x00082D64
	// (set) Token: 0x06001261 RID: 4705 RVA: 0x00084B6C File Offset: 0x00082D6C
	public Quaternion rotation
	{
		get
		{
			return this._rotation;
		}
		set
		{
			this._rotation = value;
			this._localToWorldMatrixSet = false;
			this._worldToLocalMatrixSet = false;
			this._localToWorldDirectionMatrixSet = false;
			this._worldToLocalDirectionMatrixSet = false;
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06001262 RID: 4706 RVA: 0x00084B94 File Offset: 0x00082D94
	// (set) Token: 0x06001263 RID: 4707 RVA: 0x00084BAF File Offset: 0x00082DAF
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

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x00084BBD File Offset: 0x00082DBD
	// (set) Token: 0x06001265 RID: 4709 RVA: 0x00084BC5 File Offset: 0x00082DC5
	public Vector3 localScale
	{
		get
		{
			return this._localScale;
		}
		set
		{
			this._localScale = value;
			this._localToWorldMatrixSet = false;
			this._worldToLocalMatrixSet = false;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06001266 RID: 4710 RVA: 0x00084BDC File Offset: 0x00082DDC
	// (set) Token: 0x06001267 RID: 4711 RVA: 0x00084BEE File Offset: 0x00082DEE
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

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06001268 RID: 4712 RVA: 0x00084BFC File Offset: 0x00082DFC
	// (set) Token: 0x06001269 RID: 4713 RVA: 0x00084C0E File Offset: 0x00082E0E
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

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x0600126A RID: 4714 RVA: 0x00084C21 File Offset: 0x00082E21
	// (set) Token: 0x0600126B RID: 4715 RVA: 0x00084C33 File Offset: 0x00082E33
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

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x0600126C RID: 4716 RVA: 0x00084C48 File Offset: 0x00082E48
	public Matrix4x4 worldToLocalMatrix
	{
		get
		{
			if (!this._worldToLocalMatrixSet)
			{
				this._worldToLocalMatrixSet = true;
				this._worldToLocalMatrix = this.localToWorldMatrix.inverse;
			}
			return this._worldToLocalMatrix;
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x0600126D RID: 4717 RVA: 0x00084C7E File Offset: 0x00082E7E
	public Matrix4x4 localToWorldMatrix
	{
		get
		{
			if (!this._localToWorldMatrixSet)
			{
				this._localToWorldMatrixSet = true;
				this._localToWorldMatrix = Matrix4x4.TRS(this.position, this.rotation, this.localScale);
			}
			return this._localToWorldMatrix;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x0600126E RID: 4718 RVA: 0x00084CB4 File Offset: 0x00082EB4
	public Matrix4x4 worldToLocalDirectionMatrix
	{
		get
		{
			if (!this._worldToLocalDirectionMatrixSet)
			{
				this._worldToLocalDirectionMatrixSet = true;
				this._worldToLocalDirectionMatrix = this.localToWorldDirectionMatrix.inverse;
			}
			return this._worldToLocalDirectionMatrix;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x0600126F RID: 4719 RVA: 0x00084CEA File Offset: 0x00082EEA
	public Matrix4x4 localToWorldDirectionMatrix
	{
		get
		{
			if (!this._localToWorldDirectionMatrixSet)
			{
				this._localToWorldDirectionMatrixSet = true;
				this._localToWorldDirectionMatrix = Matrix4x4.TRS(this.position, this.rotation, Vector3.one);
			}
			return this._localToWorldDirectionMatrix;
		}
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x00084D1D File Offset: 0x00082F1D
	public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self)
	{
		this.Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00084D2F File Offset: 0x00082F2F
	public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self)
	{
		if (relativeTo == Space.World)
		{
			this.rotation = Quaternion.AngleAxis(angle, axis) * this.rotation;
			return;
		}
		this.rotation *= Quaternion.AngleAxis(angle, axis);
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x00084D65 File Offset: 0x00082F65
	public void Rotate(Vector3 eulerAngles, Space relativeTo = Space.Self)
	{
		if (relativeTo == Space.World)
		{
			this.rotation = Quaternion.Euler(eulerAngles) * this.rotation;
			return;
		}
		this.rotation *= Quaternion.Euler(eulerAngles);
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x00084D9C File Offset: 0x00082F9C
	public void RotateAround(Vector3 point, Vector3 axis, float angle)
	{
		Vector3 vector = this.position;
		Quaternion quaternion = Quaternion.AngleAxis(angle, axis);
		Vector3 vector2 = vector - point;
		vector2 = quaternion * vector2;
		vector = point + vector2;
		this.position = vector;
		this.Rotate(axis, angle, Space.World);
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x00084DDE File Offset: 0x00082FDE
	public void LookAt(Vector3 point, Vector3 up)
	{
		this.rotation = Quaternion.LookRotation(point - this.position, up);
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x00084DF8 File Offset: 0x00082FF8
	public void Translate(Vector3 translation, Transform relativeTo)
	{
		if (relativeTo)
		{
			this.position += relativeTo.TransformDirection(translation);
			return;
		}
		this.position += translation;
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x00084E2D File Offset: 0x0008302D
	public void Translate(float x, float y, float z, Transform relativeTo)
	{
		this.Translate(new Vector3(x, y, z), relativeTo);
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x00084E3F File Offset: 0x0008303F
	public void Translate(float x, float y, float z, Space relativeTo = Space.Self)
	{
		this.Translate(new Vector3(x, y, z), relativeTo);
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x00084E51 File Offset: 0x00083051
	public void Translate(Vector3 translation, Space relativeTo = Space.Self)
	{
		if (relativeTo == Space.World)
		{
			this.position += translation;
			return;
		}
		this.position += this.rotation * translation;
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x00084E86 File Offset: 0x00083086
	public Vector3 InverseTransformDirection(float x, float y, float z)
	{
		return this.InverseTransformDirection(new Vector3(x, y, z));
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00084E98 File Offset: 0x00083098
	public Vector3 InverseTransformDirection(Vector3 direction)
	{
		return this.worldToLocalDirectionMatrix.MultiplyVector(direction);
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00084EB4 File Offset: 0x000830B4
	public Vector3 InverseTransformPoint(float x, float y, float z)
	{
		return this.InverseTransformPoint(new Vector3(x, y, z));
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00084EC4 File Offset: 0x000830C4
	public Vector3 InverseTransformPoint(Vector3 position)
	{
		return this.worldToLocalMatrix.MultiplyPoint(position);
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x00084EE0 File Offset: 0x000830E0
	public Vector3 InverseTransformVector(float x, float y, float z)
	{
		return this.InverseTransformVector(new Vector3(x, y, z));
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x00084EF0 File Offset: 0x000830F0
	public Vector3 InverseTransformVector(Vector3 vector)
	{
		return this.worldToLocalMatrix.MultiplyVector(vector);
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00084F0C File Offset: 0x0008310C
	public Vector3 TransformDirection(float x, float y, float z)
	{
		return this.TransformDirection(new Vector3(x, y, z));
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00084F1C File Offset: 0x0008311C
	public Vector3 TransformDirection(Vector3 direction)
	{
		return this.localToWorldDirectionMatrix.MultiplyVector(direction);
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x00084F38 File Offset: 0x00083138
	public Vector3 TransformPoint(float x, float y, float z)
	{
		return this.TransformPoint(new Vector3(x, y, z));
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x00084F48 File Offset: 0x00083148
	public Vector3 TransformPoint(Vector3 position)
	{
		return this.localToWorldMatrix.MultiplyPoint(position);
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x00084F64 File Offset: 0x00083164
	public Vector3 TransformVector(Vector3 vector)
	{
		return this.localToWorldMatrix.MultiplyVector(vector);
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x00084F80 File Offset: 0x00083180
	public Vector3 TransformVector(float x, float y, float z)
	{
		return this.TransformVector(new Vector3(x, y, z));
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x00084F90 File Offset: 0x00083190
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		SerializableTransform serializableTransform = (SerializableTransform)obj;
		return serializableTransform != null && this.Equals(serializableTransform);
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x00084FBC File Offset: 0x000831BC
	public bool Equals(SerializableTransform p)
	{
		return p != null && (this.position == p.position && this.rotation == p.rotation) && this.localScale == p.localScale;
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x00085010 File Offset: 0x00083210
	public override int GetHashCode()
	{
		return 27 * this.position.GetHashCode() * this.rotation.GetHashCode() * this.localScale.GetHashCode();
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x0008505E File Offset: 0x0008325E
	public static bool operator ==(SerializableTransform left, SerializableTransform right)
	{
		return left == right || (left != null && right != null && left.Equals(right));
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x0008508A File Offset: 0x0008328A
	public static bool operator !=(SerializableTransform left, SerializableTransform right)
	{
		return !(left == right);
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x00085098 File Offset: 0x00083298
	public override string ToString()
	{
		return string.Format("[{0}] position:{1} rotation:{2} localScale:{3}", new object[]
		{
			base.GetType().Name,
			this.position,
			this.eulerAngles,
			this.localScale
		});
	}

	// Token: 0x0400128E RID: 4750
	[SerializeField]
	private Vector3 _position;

	// Token: 0x0400128F RID: 4751
	[SerializeField]
	private Quaternion _rotation;

	// Token: 0x04001290 RID: 4752
	[SerializeField]
	private Vector3 _localScale;

	// Token: 0x04001291 RID: 4753
	private bool _worldToLocalMatrixSet;

	// Token: 0x04001292 RID: 4754
	private Matrix4x4 _worldToLocalMatrix;

	// Token: 0x04001293 RID: 4755
	private bool _localToWorldMatrixSet;

	// Token: 0x04001294 RID: 4756
	private Matrix4x4 _localToWorldMatrix;

	// Token: 0x04001295 RID: 4757
	private bool _worldToLocalDirectionMatrixSet;

	// Token: 0x04001296 RID: 4758
	private Matrix4x4 _worldToLocalDirectionMatrix;

	// Token: 0x04001297 RID: 4759
	private bool _localToWorldDirectionMatrixSet;

	// Token: 0x04001298 RID: 4760
	private Matrix4x4 _localToWorldDirectionMatrix;
}
