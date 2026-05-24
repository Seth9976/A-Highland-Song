using System;

namespace UnityEngine
{
	// Token: 0x0200023E RID: 574
	public static class Snapping
	{
		// Token: 0x06001898 RID: 6296 RVA: 0x00027EF4 File Offset: 0x000260F4
		internal static bool IsCardinalDirection(Vector3 direction)
		{
			return (Mathf.Abs(direction.x) > 0f && Mathf.Approximately(direction.y, 0f) && Mathf.Approximately(direction.z, 0f)) || (Mathf.Abs(direction.y) > 0f && Mathf.Approximately(direction.x, 0f) && Mathf.Approximately(direction.z, 0f)) || (Mathf.Abs(direction.z) > 0f && Mathf.Approximately(direction.x, 0f) && Mathf.Approximately(direction.y, 0f));
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00027FAC File Offset: 0x000261AC
		public static float Snap(float val, float snap)
		{
			bool flag = snap == 0f;
			float num;
			if (flag)
			{
				num = val;
			}
			else
			{
				num = snap * Mathf.Round(val / snap);
			}
			return num;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00027FD8 File Offset: 0x000261D8
		public static Vector2 Snap(Vector2 val, Vector2 snap)
		{
			return new Vector3((Mathf.Abs(snap.x) < Mathf.Epsilon) ? val.x : (snap.x * Mathf.Round(val.x / snap.x)), (Mathf.Abs(snap.y) < Mathf.Epsilon) ? val.y : (snap.y * Mathf.Round(val.y / snap.y)));
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0002805C File Offset: 0x0002625C
		public static Vector3 Snap(Vector3 val, Vector3 snap, SnapAxis axis = SnapAxis.All)
		{
			return new Vector3(((axis & SnapAxis.X) == SnapAxis.X) ? Snapping.Snap(val.x, snap.x) : val.x, ((axis & SnapAxis.Y) == SnapAxis.Y) ? Snapping.Snap(val.y, snap.y) : val.y, ((axis & SnapAxis.Z) == SnapAxis.Z) ? Snapping.Snap(val.z, snap.z) : val.z);
		}
	}
}
