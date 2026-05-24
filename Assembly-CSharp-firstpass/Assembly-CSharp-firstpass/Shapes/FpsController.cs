using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200000B RID: 11
	[ExecuteAlways]
	public class FpsController : ImmediateModeShapeDrawer
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00003448 File Offset: 0x00001648
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.InputFocus = true;
			base.StartCoroutine(this.FixedSteps());
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003468 File Offset: 0x00001668
		public override void DrawShapes(Camera cam)
		{
			if (cam != this.cam)
			{
				return;
			}
			using (Draw.Command(cam, CameraEvent.BeforeImageEffects))
			{
				Draw.ZTest = CompareFunction.Always;
				Draw.Matrix = this.crosshairTransform.localToWorldMatrix;
				Draw.BlendMode = ShapesBlendMode.Transparent;
				Draw.LineGeometry = LineGeometry.Flat2D;
				this.crosshair.DrawCrosshair();
				float num = this.ammoBarRadius + this.fireSidebarRadiusPunchAmount * this.crosshair.fireDecayer.value;
				this.ammoBar.DrawBar(this, num);
				this.chargeBar.DrawBar(this, num);
				this.compass.DrawCompass(this.head.transform.forward);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000352C File Offset: 0x0000172C
		private IEnumerator FixedSteps()
		{
			for (;;)
			{
				this.FixedUpdateManual();
				yield return new WaitForSeconds(0.01f);
			}
			yield break;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000353C File Offset: 0x0000173C
		public static void DrawRoundedArcOutline(Vector2 origin, float radius, float thickness, float outlineThickness, float angStart, float angEnd)
		{
			float num = radius - thickness / 2f;
			float num2 = radius + thickness / 2f;
			Draw.Arc(origin, num, outlineThickness, angStart - 0.01f, angEnd + 0.01f);
			Draw.Arc(origin, num2, outlineThickness, angStart - 0.01f, angEnd + 0.01f);
			Vector2 vector = origin + ShapesMath.AngToDir(angStart) * radius;
			Vector2 vector2 = origin + ShapesMath.AngToDir(angEnd) * radius;
			Draw.Arc(vector, thickness / 2f, outlineThickness, angStart, angStart - 3.1415927f);
			Draw.Arc(vector2, thickness / 2f, outlineThickness, angEnd, angEnd + 3.1415927f);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000035F8 File Offset: 0x000017F8
		public Vector2 GetShake(float speed, float amp)
		{
			float num = ShapesMath.Frac(Time.time * speed);
			float num2 = this.shakeAnimX.Evaluate(num);
			float num3 = this.shakeAnimY.Evaluate(num);
			return new Vector2(num2, num3) * amp;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003637 File Offset: 0x00001837
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003641 File Offset: 0x00001841
		private bool InputFocus
		{
			get
			{
				return !Cursor.visible;
			}
			set
			{
				Cursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None);
				Cursor.visible = !value;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003658 File Offset: 0x00001858
		private void FixedUpdateManual()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.InputFocus)
			{
				Vector3 right = this.head.right;
				Vector3 forward = this.head.forward;
				forward.y = 0f;
				this.moveVel += (this.moveInput.y * forward + this.moveInput.x * right) * (Time.fixedDeltaTime * this.moveSpeed);
			}
			base.transform.position += this.moveVel * Time.deltaTime;
			this.moveVel *= this.smoof;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003720 File Offset: 0x00001920
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.crosshair.UpdateCrosshairDecay();
			this.chargeBar.UpdateCharge();
			if (this.InputFocus)
			{
				this.yaw += Input.GetAxis("Mouse X") * this.lookSensitivity;
				this.pitch -= Input.GetAxis("Mouse Y") * this.lookSensitivity;
				this.pitch = Mathf.Clamp(this.pitch, -90f, 90f);
				this.head.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
				this.chargeBar.isCharging = Input.GetMouseButton(1);
				if (Input.GetKey(KeyCode.R))
				{
					this.ammoBar.Reload();
				}
				if (Input.GetMouseButtonDown(0) && this.ammoBar.HasBulletsLeft)
				{
					this.ammoBar.Fire();
					this.crosshair.Fire();
					RaycastHit raycastHit;
					if (Physics.Raycast(new Ray(this.head.position, this.head.forward), out raycastHit) && raycastHit.collider.gameObject.name == "Enemy")
					{
						this.crosshair.FireHit();
					}
				}
				this.moveInput = Vector2.zero;
				this.<Update>g__DoInput|30_0(KeyCode.W, Vector2.up);
				this.<Update>g__DoInput|30_0(KeyCode.S, Vector2.down);
				this.<Update>g__DoInput|30_0(KeyCode.D, Vector2.right);
				this.<Update>g__DoInput|30_0(KeyCode.A, Vector2.left);
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.InputFocus = false;
					return;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.InputFocus = true;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003960 File Offset: 0x00001B60
		[CompilerGenerated]
		private void <Update>g__DoInput|30_0(KeyCode key, Vector2 dir)
		{
			if (Input.GetKey(key))
			{
				this.moveInput += dir;
			}
		}

		// Token: 0x0400004A RID: 74
		public Transform head;

		// Token: 0x0400004B RID: 75
		public Camera cam;

		// Token: 0x0400004C RID: 76
		public Crosshair crosshair;

		// Token: 0x0400004D RID: 77
		public ChargeBar chargeBar;

		// Token: 0x0400004E RID: 78
		public AmmoBar ammoBar;

		// Token: 0x0400004F RID: 79
		public Compass compass;

		// Token: 0x04000050 RID: 80
		public Transform crosshairTransform;

		// Token: 0x04000051 RID: 81
		[Header("Player Movement")]
		[Range(0.8f, 1f)]
		public float smoof = 0.99f;

		// Token: 0x04000052 RID: 82
		public float moveSpeed = 1f;

		// Token: 0x04000053 RID: 83
		public float lookSensitivity = 1f;

		// Token: 0x04000054 RID: 84
		private float yaw;

		// Token: 0x04000055 RID: 85
		private float pitch;

		// Token: 0x04000056 RID: 86
		private Vector2 moveInput = Vector2.zero;

		// Token: 0x04000057 RID: 87
		private Vector3 moveVel = Vector3.zero;

		// Token: 0x04000058 RID: 88
		[Header("Sidebar Style")]
		[Range(0f, 3.1415927f)]
		public float ammoBarAngularSpanRad;

		// Token: 0x04000059 RID: 89
		[Range(0f, 0.05f)]
		public float ammoBarOutlineThickness = 0.1f;

		// Token: 0x0400005A RID: 90
		[Range(0f, 0.2f)]
		public float ammoBarThickness;

		// Token: 0x0400005B RID: 91
		[Range(0f, 0.2f)]
		public float ammoBarRadius;

		// Token: 0x0400005C RID: 92
		[Header("Animation")]
		[Range(0f, 0.3f)]
		public float fireSidebarRadiusPunchAmount = 0.1f;

		// Token: 0x0400005D RID: 93
		public AnimationCurve shakeAnimX = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x0400005E RID: 94
		public AnimationCurve shakeAnimY = AnimationCurve.Constant(0f, 1f, 0f);
	}
}
