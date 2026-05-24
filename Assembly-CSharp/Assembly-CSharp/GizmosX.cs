using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000206 RID: 518
public static class GizmosX
{
	// Token: 0x0600130E RID: 4878 RVA: 0x000876CD File Offset: 0x000858CD
	public static void BeginColor(Color color)
	{
		GizmosX.colors.Push(Gizmos.color);
		Gizmos.color = color;
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x000876E4 File Offset: 0x000858E4
	public static void EndColor()
	{
		Gizmos.color = GizmosX.colors.Pop();
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x000876F5 File Offset: 0x000858F5
	public static void BeginMatrix(Matrix4x4 matrix)
	{
		GizmosX.matricies.Push(Gizmos.matrix);
		Gizmos.matrix = matrix;
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x0008770C File Offset: 0x0008590C
	public static void EndMatrix()
	{
		Gizmos.matrix = GizmosX.matricies.Pop();
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00087720 File Offset: 0x00085920
	public static void DrawCircle(Vector3 position, float radius, int numLines = 16)
	{
		float num = 6.2831855f / (float)numLines;
		Vector3 vector = new Vector3(position.x + radius * Mathf.Sin(0f), position.y + radius * Mathf.Cos(0f), position.z);
		for (int i = 0; i < numLines; i++)
		{
			Vector3 vector2 = new Vector3(position.x + radius * Mathf.Sin((float)(i + 1) * num), position.y + radius * Mathf.Cos((float)(i + 1) * num), position.z);
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x000877B4 File Offset: 0x000859B4
	public static void DrawCrosshair(Vector3 center, float armLength)
	{
		Gizmos.DrawLine(center - armLength * Vector3.right, center + armLength * Vector3.right);
		Gizmos.DrawLine(center - armLength * Vector3.up, center + armLength * Vector3.up);
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x0008780F File Offset: 0x00085A0F
	public static void DrawWirePolygon(Vector3 position, Quaternion rotation, Vector3 scale, IList<Vector2> points)
	{
		GizmosX.BeginMatrix(Matrix4x4.TRS(position, rotation, scale));
		GizmosX.DrawWirePolygon(points);
		GizmosX.EndMatrix();
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x0008782C File Offset: 0x00085A2C
	public static void DrawWirePolygon(IList<Vector2> points)
	{
		if (points.Count <= 1)
		{
			return;
		}
		for (int i = 0; i < points.Count; i++)
		{
			Gizmos.DrawLine(points[i], points[(i + 1) % points.Count]);
		}
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x0008787C File Offset: 0x00085A7C
	public static void DrawWireCylinder(Vector3 position, Quaternion rotation, float radius, float height, int numColumns = 32)
	{
		GizmosX.BeginMatrix(Matrix4x4.TRS(position, rotation, Vector3.one));
		Vector3 vector = Vector3.zero;
		int i = 0;
		float num = 1f / (float)numColumns * 2f * 3.1415927f;
		Vector3 vector2 = new Vector3(0f, height, 0f);
		Vector3 vector3 = new Vector3(0f, height * 0.5f, 0f);
		float num2 = (float)i * num;
		Vector3 vector4 = new Vector3(Mathf.Sin(num2) * radius, 0f, Mathf.Cos(num2) * radius) - vector3;
		Vector3 vector5 = vector4 + vector2;
		for (i = 0; i < numColumns; i++)
		{
			num2 = (float)(i + 1) * num;
			Vector3 vector6 = new Vector3(Mathf.Sin(num2) * radius, 0f, Mathf.Cos(num2) * radius) - vector3;
			vector = vector6 + vector2;
			Gizmos.DrawLine(vector4, vector5);
			Gizmos.DrawLine(vector4, vector6);
			Gizmos.DrawLine(vector5, vector);
			vector4 = vector6;
			vector5 = vector;
		}
		GizmosX.EndMatrix();
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x00087980 File Offset: 0x00085B80
	public static void DrawArrowLine(IList<Vector3> positions)
	{
		for (int i = 0; i < positions.Count - 1; i++)
		{
			GizmosX.DrawArrowLine(positions[i], positions[i + 1]);
		}
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x000879B5 File Offset: 0x00085BB5
	public static void DrawArrowLine(Vector3 fromPosition, Vector3 toPosition)
	{
		GizmosX.DrawArrowLine(fromPosition, toPosition, Vector3.up);
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x000879C4 File Offset: 0x00085BC4
	public static void DrawArrowLine(Vector3 fromPosition, Vector3 toPosition, Vector3 crossVector)
	{
		if (fromPosition == toPosition)
		{
			return;
		}
		Gizmos.DrawLine(fromPosition, toPosition);
		Vector3 vector = toPosition - fromPosition;
		GizmosX.DrawArrow(fromPosition + vector * 0.75f, Quaternion.LookRotation(vector, crossVector), vector.magnitude * 0.05f);
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00087A14 File Offset: 0x00085C14
	public static void DrawArrow(Vector3 position, Quaternion rotation, float arrowSize)
	{
		Vector3 vector = position + rotation * Vector3.back * arrowSize;
		Vector3 vector2 = position + rotation * Vector3.forward * arrowSize;
		Gizmos.DrawLine(vector + rotation * Vector3.left * arrowSize, vector2);
		Gizmos.DrawLine(vector + rotation * Vector3.right * arrowSize, vector2);
	}

	// Token: 0x040012A8 RID: 4776
	private static Stack<Color> colors = new Stack<Color>();

	// Token: 0x040012A9 RID: 4777
	private static Stack<Matrix4x4> matricies = new Stack<Matrix4x4>();
}
