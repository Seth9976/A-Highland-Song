using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200025A RID: 602
	[NativeClass("UI::RectTransform")]
	[NativeHeader("Runtime/Transform/RectTransform.h")]
	public sealed class RectTransform : Transform
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060019FA RID: 6650 RVA: 0x0002A004 File Offset: 0x00028204
		// (remove) Token: 0x060019FB RID: 6651 RVA: 0x0002A038 File Offset: 0x00028238
		[field: DebuggerBrowsable(0)]
		public static event RectTransform.ReapplyDrivenProperties reapplyDrivenProperties;

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0002A06C File Offset: 0x0002826C
		public Rect rect
		{
			get
			{
				Rect rect;
				this.get_rect_Injected(out rect);
				return rect;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0002A084 File Offset: 0x00028284
		// (set) Token: 0x060019FE RID: 6654 RVA: 0x0002A09A File Offset: 0x0002829A
		public Vector2 anchorMin
		{
			get
			{
				Vector2 vector;
				this.get_anchorMin_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchorMin_Injected(ref value);
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0002A0A4 File Offset: 0x000282A4
		// (set) Token: 0x06001A00 RID: 6656 RVA: 0x0002A0BA File Offset: 0x000282BA
		public Vector2 anchorMax
		{
			get
			{
				Vector2 vector;
				this.get_anchorMax_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchorMax_Injected(ref value);
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0002A0C4 File Offset: 0x000282C4
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x0002A0DA File Offset: 0x000282DA
		public Vector2 anchoredPosition
		{
			get
			{
				Vector2 vector;
				this.get_anchoredPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchoredPosition_Injected(ref value);
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0002A0E4 File Offset: 0x000282E4
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x0002A0FA File Offset: 0x000282FA
		public Vector2 sizeDelta
		{
			get
			{
				Vector2 vector;
				this.get_sizeDelta_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_sizeDelta_Injected(ref value);
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0002A104 File Offset: 0x00028304
		// (set) Token: 0x06001A06 RID: 6662 RVA: 0x0002A11A File Offset: 0x0002831A
		public Vector2 pivot
		{
			get
			{
				Vector2 vector;
				this.get_pivot_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_pivot_Injected(ref value);
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0002A124 File Offset: 0x00028324
		// (set) Token: 0x06001A08 RID: 6664 RVA: 0x0002A15C File Offset: 0x0002835C
		public Vector3 anchoredPosition3D
		{
			get
			{
				Vector2 anchoredPosition = this.anchoredPosition;
				return new Vector3(anchoredPosition.x, anchoredPosition.y, base.localPosition.z);
			}
			set
			{
				this.anchoredPosition = new Vector2(value.x, value.y);
				Vector3 localPosition = base.localPosition;
				localPosition.z = value.z;
				base.localPosition = localPosition;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0002A1A0 File Offset: 0x000283A0
		// (set) Token: 0x06001A0A RID: 6666 RVA: 0x0002A1D0 File Offset: 0x000283D0
		public Vector2 offsetMin
		{
			get
			{
				return this.anchoredPosition - Vector2.Scale(this.sizeDelta, this.pivot);
			}
			set
			{
				Vector2 vector = value - (this.anchoredPosition - Vector2.Scale(this.sizeDelta, this.pivot));
				this.sizeDelta -= vector;
				this.anchoredPosition += Vector2.Scale(vector, Vector2.one - this.pivot);
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x0002A23C File Offset: 0x0002843C
		// (set) Token: 0x06001A0C RID: 6668 RVA: 0x0002A274 File Offset: 0x00028474
		public Vector2 offsetMax
		{
			get
			{
				return this.anchoredPosition + Vector2.Scale(this.sizeDelta, Vector2.one - this.pivot);
			}
			set
			{
				Vector2 vector = value - (this.anchoredPosition + Vector2.Scale(this.sizeDelta, Vector2.one - this.pivot));
				this.sizeDelta += vector;
				this.anchoredPosition += Vector2.Scale(vector, this.pivot);
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001A0D RID: 6669
		// (set) Token: 0x06001A0E RID: 6670
		public extern Object drivenByObject
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			internal set;
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001A0F RID: 6671
		// (set) Token: 0x06001A10 RID: 6672
		internal extern DrivenTransformProperties drivenProperties
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06001A11 RID: 6673
		[NativeMethod("UpdateIfTransformDispatchIsDirty")]
		[MethodImpl(4096)]
		public extern void ForceUpdateRectTransforms();

		// Token: 0x06001A12 RID: 6674 RVA: 0x0002A2E0 File Offset: 0x000284E0
		public void GetLocalCorners(Vector3[] fourCornersArray)
		{
			bool flag = fourCornersArray == null || fourCornersArray.Length < 4;
			if (flag)
			{
				Debug.LogError("Calling GetLocalCorners with an array that is null or has less than 4 elements.");
			}
			else
			{
				Rect rect = this.rect;
				float x = rect.x;
				float y = rect.y;
				float xMax = rect.xMax;
				float yMax = rect.yMax;
				fourCornersArray[0] = new Vector3(x, y, 0f);
				fourCornersArray[1] = new Vector3(x, yMax, 0f);
				fourCornersArray[2] = new Vector3(xMax, yMax, 0f);
				fourCornersArray[3] = new Vector3(xMax, y, 0f);
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0002A384 File Offset: 0x00028584
		public void GetWorldCorners(Vector3[] fourCornersArray)
		{
			bool flag = fourCornersArray == null || fourCornersArray.Length < 4;
			if (flag)
			{
				Debug.LogError("Calling GetWorldCorners with an array that is null or has less than 4 elements.");
			}
			else
			{
				this.GetLocalCorners(fourCornersArray);
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				for (int i = 0; i < 4; i++)
				{
					fourCornersArray[i] = localToWorldMatrix.MultiplyPoint(fourCornersArray[i]);
				}
			}
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0002A3EC File Offset: 0x000285EC
		public void SetInsetAndSizeFromParentEdge(RectTransform.Edge edge, float inset, float size)
		{
			int num = ((edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Bottom) ? 1 : 0);
			bool flag = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Right;
			float num2 = (float)(flag ? 1 : 0);
			Vector2 vector = this.anchorMin;
			vector[num] = num2;
			this.anchorMin = vector;
			vector = this.anchorMax;
			vector[num] = num2;
			this.anchorMax = vector;
			Vector2 sizeDelta = this.sizeDelta;
			sizeDelta[num] = size;
			this.sizeDelta = sizeDelta;
			Vector2 anchoredPosition = this.anchoredPosition;
			anchoredPosition[num] = (flag ? (-inset - size * (1f - this.pivot[num])) : (inset + size * this.pivot[num]));
			this.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0002A4B8 File Offset: 0x000286B8
		public void SetSizeWithCurrentAnchors(RectTransform.Axis axis, float size)
		{
			Vector2 sizeDelta = this.sizeDelta;
			sizeDelta[(int)axis] = size - this.GetParentSize()[(int)axis] * (this.anchorMax[(int)axis] - this.anchorMin[(int)axis]);
			this.sizeDelta = sizeDelta;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0002A511 File Offset: 0x00028711
		[RequiredByNativeCode]
		internal static void SendReapplyDrivenProperties(RectTransform driven)
		{
			RectTransform.ReapplyDrivenProperties reapplyDrivenProperties = RectTransform.reapplyDrivenProperties;
			if (reapplyDrivenProperties != null)
			{
				reapplyDrivenProperties(driven);
			}
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0002A528 File Offset: 0x00028728
		internal Rect GetRectInParentSpace()
		{
			Rect rect = this.rect;
			Vector2 vector = this.offsetMin + Vector2.Scale(this.pivot, rect.size);
			bool flag = base.transform.parent;
			if (flag)
			{
				RectTransform component = base.transform.parent.GetComponent<RectTransform>();
				bool flag2 = component;
				if (flag2)
				{
					vector += Vector2.Scale(this.anchorMin, component.rect.size);
				}
			}
			rect.x += vector.x;
			rect.y += vector.y;
			return rect;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0002A5E0 File Offset: 0x000287E0
		private Vector2 GetParentSize()
		{
			RectTransform rectTransform = base.parent as RectTransform;
			bool flag = !rectTransform;
			Vector2 vector;
			if (flag)
			{
				vector = Vector2.zero;
			}
			else
			{
				vector = rectTransform.rect.size;
			}
			return vector;
		}

		// Token: 0x06001A1A RID: 6682
		[MethodImpl(4096)]
		private extern void get_rect_Injected(out Rect ret);

		// Token: 0x06001A1B RID: 6683
		[MethodImpl(4096)]
		private extern void get_anchorMin_Injected(out Vector2 ret);

		// Token: 0x06001A1C RID: 6684
		[MethodImpl(4096)]
		private extern void set_anchorMin_Injected(ref Vector2 value);

		// Token: 0x06001A1D RID: 6685
		[MethodImpl(4096)]
		private extern void get_anchorMax_Injected(out Vector2 ret);

		// Token: 0x06001A1E RID: 6686
		[MethodImpl(4096)]
		private extern void set_anchorMax_Injected(ref Vector2 value);

		// Token: 0x06001A1F RID: 6687
		[MethodImpl(4096)]
		private extern void get_anchoredPosition_Injected(out Vector2 ret);

		// Token: 0x06001A20 RID: 6688
		[MethodImpl(4096)]
		private extern void set_anchoredPosition_Injected(ref Vector2 value);

		// Token: 0x06001A21 RID: 6689
		[MethodImpl(4096)]
		private extern void get_sizeDelta_Injected(out Vector2 ret);

		// Token: 0x06001A22 RID: 6690
		[MethodImpl(4096)]
		private extern void set_sizeDelta_Injected(ref Vector2 value);

		// Token: 0x06001A23 RID: 6691
		[MethodImpl(4096)]
		private extern void get_pivot_Injected(out Vector2 ret);

		// Token: 0x06001A24 RID: 6692
		[MethodImpl(4096)]
		private extern void set_pivot_Injected(ref Vector2 value);

		// Token: 0x0200025B RID: 603
		public enum Edge
		{
			// Token: 0x040008AE RID: 2222
			Left,
			// Token: 0x040008AF RID: 2223
			Right,
			// Token: 0x040008B0 RID: 2224
			Top,
			// Token: 0x040008B1 RID: 2225
			Bottom
		}

		// Token: 0x0200025C RID: 604
		public enum Axis
		{
			// Token: 0x040008B3 RID: 2227
			Horizontal,
			// Token: 0x040008B4 RID: 2228
			Vertical
		}

		// Token: 0x0200025D RID: 605
		// (Invoke) Token: 0x06001A26 RID: 6694
		public delegate void ReapplyDrivenProperties(RectTransform driven);
	}
}
