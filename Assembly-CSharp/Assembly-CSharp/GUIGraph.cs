using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000062 RID: 98
public static class GUIGraph
{
	// Token: 0x060002A6 RID: 678 RVA: 0x00015F4C File Offset: 0x0001414C
	public static void Draw(float currentValue, Rect position, float min = 3.4028235E+38f, float max = -3.4028235E+38f, float scale = 1f, string tag = "")
	{
		if (GUIGraph._graphsByTag == null)
		{
			GUIGraph._graphsByTag = new Dictionary<string, GUIGraph.Graph>();
		}
		GUIGraph.Graph graph;
		if (!GUIGraph._graphsByTag.TryGetValue(tag, out graph))
		{
			graph = (GUIGraph._graphsByTag[tag] = new GUIGraph.Graph());
		}
		double num = (double)Time.unscaledTime;
		float num2 = 0.016666668f;
		float num3 = 0.033333335f;
		float num4 = position.width * scale * num3;
		double num5 = num - (double)num4;
		if (num >= graph.lastSampleTime + (double)num2)
		{
			graph.lastSampleTime = num;
			graph.values.Enqueue(new GUIGraph.Sample(currentValue, num));
			while (graph.values.Count > 0 && graph.values.Peek().time < num5)
			{
				graph.values.Dequeue();
			}
			graph.lowWaterMark = Mathf.Min(graph.lowWaterMark, currentValue);
			graph.highWaterMark = Mathf.Max(graph.highWaterMark, currentValue);
		}
		if (min == 3.4028235E+38f)
		{
			min = graph.lowWaterMark;
		}
		if (max == -3.4028235E+38f)
		{
			max = graph.highWaterMark;
		}
		Texture2D whiteTexture = Texture2D.whiteTexture;
		GUI.color = Color.black.WithAlpha(0.9f);
		GUI.DrawTexture(position, whiteTexture);
		if (min <= 0f && max >= 0f)
		{
			GUI.color = Color.yellow.WithAlpha(0.3f);
			float num6 = Mathf.InverseLerp(min, max, 0f);
			GUI.DrawTexture(new Rect(position.x, position.yMax - num6 * position.height, position.width, 1f), whiteTexture);
		}
		float num7 = 20f;
		GUI.color = Color.gray;
		GUI.Label(new Rect(position.x, position.y, position.width, num7), max.ToString());
		GUI.Label(new Rect(position.x, position.yMax - num7, position.width, num7), min.ToString());
		if (!string.IsNullOrWhiteSpace(tag))
		{
			GUI.Label(new Rect(position.x, position.yMax - 5f, position.width, num7), tag, GUIGraph.centeredStyle);
		}
		GUI.color = Color.white;
		Vector2 vector = Vector2.zero;
		foreach (GUIGraph.Sample sample in graph.values)
		{
			float num8 = Mathf.InverseLerp(min, max, sample.val);
			double num9 = sample.time - num5;
			float num10 = 0f;
			if (num9 > 0.0)
			{
				num10 = Mathf.Clamp01((float)((sample.time - num5) / (double)num4));
			}
			Vector2 vector2 = new Vector2(position.x + num10 * position.width, position.yMax - num8 * position.height);
			if (vector == Vector2.zero)
			{
				vector = new Vector2(vector2.x - 1f, vector2.y);
			}
			GUIGraph.DrawLine(vector, vector2, whiteTexture);
			vector = vector2;
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0001626C File Offset: 0x0001446C
	public static void Draw(bool boolValue, Rect position, float scale = 1f, string tag = "")
	{
		GUIGraph.Draw((float)(boolValue ? 1 : 0), position, -0.2f, 1.2f, scale, tag);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00016288 File Offset: 0x00014488
	private static void DrawLine(Vector2 start, Vector2 end, Texture tex)
	{
		float num = Vector2.Distance(start, end);
		GUIUtility.RotateAroundPivot(Mathf.Atan2(end.y - start.y, end.x - start.x) * 57.29578f, start);
		GUI.DrawTexture(new Rect(start.x, start.y, num, 1f), tex);
		GUI.matrix = Matrix4x4.identity;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x000162F0 File Offset: 0x000144F0
	public static void Clear(string tag = "")
	{
		if (GUIGraph._graphsByTag == null)
		{
			return;
		}
		GUIGraph.Graph graph;
		if (GUIGraph._graphsByTag.TryGetValue(tag, out graph))
		{
			graph.ClearValues();
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060002AA RID: 682 RVA: 0x0001631A File Offset: 0x0001451A
	private static GUIStyle centeredStyle
	{
		get
		{
			if (GUIGraph._centeredStyle == null)
			{
				GUIGraph._centeredStyle = new GUIStyle(GUI.skin.label);
				GUIGraph._centeredStyle.alignment = TextAnchor.UpperCenter;
			}
			return GUIGraph._centeredStyle;
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00016347 File Offset: 0x00014547
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		GUIGraph._graphsByTag = null;
	}

	// Token: 0x040003DD RID: 989
	private static GUIStyle _centeredStyle;

	// Token: 0x040003DE RID: 990
	private static Dictionary<string, GUIGraph.Graph> _graphsByTag;

	// Token: 0x02000286 RID: 646
	private struct Sample
	{
		// Token: 0x06001586 RID: 5510 RVA: 0x00093DCE File Offset: 0x00091FCE
		public Sample(float val, double time)
		{
			this.val = val;
			this.time = time;
		}

		// Token: 0x040014F4 RID: 5364
		public float val;

		// Token: 0x040014F5 RID: 5365
		public double time;
	}

	// Token: 0x02000287 RID: 647
	private class Graph
	{
		// Token: 0x06001587 RID: 5511 RVA: 0x00093DDE File Offset: 0x00091FDE
		public Graph()
		{
			this.values = new Queue<GUIGraph.Sample>();
			this.lowWaterMark = float.MaxValue;
			this.highWaterMark = float.MinValue;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00093E07 File Offset: 0x00092007
		public void ClearValues()
		{
			this.values.Clear();
			this.lowWaterMark = float.MaxValue;
			this.highWaterMark = float.MinValue;
		}

		// Token: 0x040014F6 RID: 5366
		public float highWaterMark;

		// Token: 0x040014F7 RID: 5367
		public float lowWaterMark;

		// Token: 0x040014F8 RID: 5368
		public Queue<GUIGraph.Sample> values;

		// Token: 0x040014F9 RID: 5369
		public double lastSampleTime;
	}
}
