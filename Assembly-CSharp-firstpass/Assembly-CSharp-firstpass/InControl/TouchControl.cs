using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200005F RID: 95
	public abstract class TouchControl : MonoBehaviour
	{
		// Token: 0x06000452 RID: 1106
		public abstract void CreateControl();

		// Token: 0x06000453 RID: 1107
		public abstract void DestroyControl();

		// Token: 0x06000454 RID: 1108
		public abstract void ConfigureControl();

		// Token: 0x06000455 RID: 1109
		public abstract void SubmitControlState(ulong updateTick, float deltaTime);

		// Token: 0x06000456 RID: 1110
		public abstract void CommitControlState(ulong updateTick, float deltaTime);

		// Token: 0x06000457 RID: 1111
		public abstract void TouchBegan(Touch touch);

		// Token: 0x06000458 RID: 1112
		public abstract void TouchMoved(Touch touch);

		// Token: 0x06000459 RID: 1113
		public abstract void TouchEnded(Touch touch);

		// Token: 0x0600045A RID: 1114
		public abstract void DrawGizmos();

		// Token: 0x0600045B RID: 1115 RVA: 0x000104F5 File Offset: 0x0000E6F5
		private void OnEnable()
		{
			TouchManager.OnSetup += this.Setup;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00010508 File Offset: 0x0000E708
		private void OnDisable()
		{
			this.DestroyControl();
			Resources.UnloadUnusedAssets();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00010516 File Offset: 0x0000E716
		private void Setup()
		{
			if (!base.enabled)
			{
				return;
			}
			this.CreateControl();
			this.ConfigureControl();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00010530 File Offset: 0x0000E730
		protected Vector3 OffsetToWorldPosition(TouchControlAnchor anchor, Vector2 offset, TouchUnitType offsetUnitType, bool lockAspectRatio)
		{
			Vector3 vector;
			if (offsetUnitType == TouchUnitType.Pixels)
			{
				vector = TouchUtility.RoundVector(offset) * TouchManager.PixelToWorld;
			}
			else if (lockAspectRatio)
			{
				vector = offset * TouchManager.PercentToWorld;
			}
			else
			{
				vector = Vector3.Scale(offset, TouchManager.ViewSize);
			}
			return TouchManager.ViewToWorldPoint(TouchUtility.AnchorToViewPoint(anchor)) + vector;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00010594 File Offset: 0x0000E794
		protected void SubmitButtonState(TouchControl.ButtonTarget target, bool state, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.UpdateWithState(state, updateTick, deltaTime);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000105D0 File Offset: 0x0000E7D0
		protected void SubmitButtonValue(TouchControl.ButtonTarget target, float value, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.UpdateWithValue(value, updateTick, deltaTime);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001060C File Offset: 0x0000E80C
		protected void CommitButton(TouchControl.ButtonTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null && control != InputControl.Null)
			{
				control.Commit();
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00010644 File Offset: 0x0000E844
		protected void SubmitAnalogValue(TouchControl.AnalogTarget target, Vector2 value, float lowerDeadZone, float upperDeadZone, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			Vector2 vector = DeadZone.Circular(value.x, value.y, lowerDeadZone, upperDeadZone);
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithValue(vector, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithValue(vector, updateTick, deltaTime);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001069F File Offset: 0x0000E89F
		protected void CommitAnalog(TouchControl.AnalogTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitLeftStick();
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitRightStick();
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000106D0 File Offset: 0x0000E8D0
		protected void SubmitRawAnalogValue(TouchControl.AnalogTarget target, Vector2 rawValue, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.AnalogTarget.None)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithRawValue(rawValue, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithRawValue(rawValue, updateTick, deltaTime);
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001070C File Offset: 0x0000E90C
		protected static Vector3 SnapTo(Vector2 vector, TouchControl.SnapAngles snapAngles)
		{
			if (snapAngles == TouchControl.SnapAngles.None)
			{
				return vector;
			}
			float num = 360f / (float)snapAngles;
			return TouchControl.SnapTo(vector, num);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00010734 File Offset: 0x0000E934
		protected static Vector3 SnapTo(Vector2 vector, float snapAngle)
		{
			float num = Vector2.Angle(vector, Vector2.up);
			if (num < snapAngle / 2f)
			{
				return Vector2.up * vector.magnitude;
			}
			if (num > 180f - snapAngle / 2f)
			{
				return -Vector2.up * vector.magnitude;
			}
			float num2 = Mathf.Round(num / snapAngle) * snapAngle - num;
			Vector3 vector2 = Vector3.Cross(Vector2.up, vector);
			return Quaternion.AngleAxis(num2, vector2) * vector;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000107CD File Offset: 0x0000E9CD
		private void OnDrawGizmosSelected()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.WhenSelected)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00010804 File Offset: 0x0000EA04
		private void OnDrawGizmos()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos == TouchManager.GizmoShowOption.UnlessPlaying)
			{
				if (Application.isPlaying)
				{
					return;
				}
			}
			else if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.Always)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x0200021F RID: 543
		public enum ButtonTarget
		{
			// Token: 0x040004CE RID: 1230
			None,
			// Token: 0x040004CF RID: 1231
			DPadDown = 12,
			// Token: 0x040004D0 RID: 1232
			DPadLeft,
			// Token: 0x040004D1 RID: 1233
			DPadRight,
			// Token: 0x040004D2 RID: 1234
			DPadUp = 11,
			// Token: 0x040004D3 RID: 1235
			LeftTrigger = 15,
			// Token: 0x040004D4 RID: 1236
			RightTrigger,
			// Token: 0x040004D5 RID: 1237
			LeftBumper,
			// Token: 0x040004D6 RID: 1238
			RightBumper,
			// Token: 0x040004D7 RID: 1239
			Action1,
			// Token: 0x040004D8 RID: 1240
			Action2,
			// Token: 0x040004D9 RID: 1241
			Action3,
			// Token: 0x040004DA RID: 1242
			Action4,
			// Token: 0x040004DB RID: 1243
			Action5,
			// Token: 0x040004DC RID: 1244
			Action6,
			// Token: 0x040004DD RID: 1245
			Action7,
			// Token: 0x040004DE RID: 1246
			Action8,
			// Token: 0x040004DF RID: 1247
			Action9,
			// Token: 0x040004E0 RID: 1248
			Action10,
			// Token: 0x040004E1 RID: 1249
			Action11,
			// Token: 0x040004E2 RID: 1250
			Action12,
			// Token: 0x040004E3 RID: 1251
			Menu = 106,
			// Token: 0x040004E4 RID: 1252
			Button0 = 500,
			// Token: 0x040004E5 RID: 1253
			Button1,
			// Token: 0x040004E6 RID: 1254
			Button2,
			// Token: 0x040004E7 RID: 1255
			Button3,
			// Token: 0x040004E8 RID: 1256
			Button4,
			// Token: 0x040004E9 RID: 1257
			Button5,
			// Token: 0x040004EA RID: 1258
			Button6,
			// Token: 0x040004EB RID: 1259
			Button7,
			// Token: 0x040004EC RID: 1260
			Button8,
			// Token: 0x040004ED RID: 1261
			Button9,
			// Token: 0x040004EE RID: 1262
			Button10,
			// Token: 0x040004EF RID: 1263
			Button11,
			// Token: 0x040004F0 RID: 1264
			Button12,
			// Token: 0x040004F1 RID: 1265
			Button13,
			// Token: 0x040004F2 RID: 1266
			Button14,
			// Token: 0x040004F3 RID: 1267
			Button15,
			// Token: 0x040004F4 RID: 1268
			Button16,
			// Token: 0x040004F5 RID: 1269
			Button17,
			// Token: 0x040004F6 RID: 1270
			Button18,
			// Token: 0x040004F7 RID: 1271
			Button19
		}

		// Token: 0x02000220 RID: 544
		public enum AnalogTarget
		{
			// Token: 0x040004F9 RID: 1273
			None,
			// Token: 0x040004FA RID: 1274
			LeftStick,
			// Token: 0x040004FB RID: 1275
			RightStick,
			// Token: 0x040004FC RID: 1276
			Both
		}

		// Token: 0x02000221 RID: 545
		public enum SnapAngles
		{
			// Token: 0x040004FE RID: 1278
			None,
			// Token: 0x040004FF RID: 1279
			Four = 4,
			// Token: 0x04000500 RID: 1280
			Eight = 8,
			// Token: 0x04000501 RID: 1281
			Sixteen = 16
		}
	}
}
