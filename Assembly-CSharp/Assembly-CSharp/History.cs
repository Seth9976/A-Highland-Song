using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020000BB RID: 187
public static class History
{
	// Token: 0x060005E2 RID: 1506 RVA: 0x0002E114 File Offset: 0x0002C314
	public static void Clear()
	{
		History.frames.Clear();
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0002E120 File Offset: 0x0002C320
	[Conditional("DEBUG")]
	public static void StartNewFrame()
	{
		if (!History.enabled)
		{
			return;
		}
		History.frames.Enqueue(History._currentFrame);
		float time = Time.time;
		History._totalCount++;
		History._currentFrame = new History.Frame
		{
			idx = History._totalCount,
			frameCount = Time.frameCount,
			time = time
		};
		while (History.frames.Count > History.maxFrames)
		{
			History.frames.Dequeue();
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0002E1A2 File Offset: 0x0002C3A2
	[Conditional("DEBUG")]
	public static void LogRaycast(Vector3 from, Vector3 to)
	{
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
	[Conditional("DEBUG")]
	public static void LogRaycast(Vector3 from, Vector3 to, Color color)
	{
		if (!History.enabled)
		{
			return;
		}
		History._currentFrame.raycasts = History._currentFrame.raycasts ?? new List<History.Raycast>();
		History._currentFrame.raycasts.Add(new History.Raycast
		{
			from = from,
			to = to,
			color = color
		});
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002E208 File Offset: 0x0002C408
	[Conditional("DEBUG")]
	public static void LogSceneLabel(Vector3 pos, string label, int yOffset = 0)
	{
		if (!History.enabled)
		{
			return;
		}
		History._currentFrame.sceneLabels = History._currentFrame.sceneLabels ?? new List<History.SceneLabel>();
		History._currentFrame.sceneLabels.Add(new History.SceneLabel
		{
			pos = pos,
			label = label,
			yOffset = (float)yOffset
		});
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002E26B File Offset: 0x0002C46B
	public static void Log(string name)
	{
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002E270 File Offset: 0x0002C470
	[Conditional("DEBUG")]
	public static void Log(string name, object valueOrObject, int priority = 0)
	{
		if (!History.enabled)
		{
			return;
		}
		History.NamedObject namedObject = new History.NamedObject
		{
			name = name,
			obj = valueOrObject,
			priority = priority
		};
		if (History._currentGroup != null)
		{
			namedObject.originalIdx = History._currentGroup.children.Count;
			History._currentGroup.children.Add(namedObject);
			return;
		}
		History._currentFrame.namedObjects = History._currentFrame.namedObjects ?? new List<History.NamedObject>();
		namedObject.originalIdx = History._currentFrame.namedObjects.Count;
		History._currentFrame.namedObjects.Add(namedObject);
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0002E318 File Offset: 0x0002C518
	[Conditional("DEBUG")]
	public static void BeginGroup(string name, int priority = 0)
	{
		if (!History.enabled)
		{
			return;
		}
		if (History._currentGroup != null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"History: Mismatched group: ",
				History._currentGroup.name,
				" is already active when ",
				name,
				" was started."
			}));
		}
		else
		{
			History._currentGroup = new History.Group
			{
				children = new List<History.NamedObject>()
			};
		}
		History._currentGroup.name = name;
		History._currentGroup.priority = priority;
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0002E39A File Offset: 0x0002C59A
	[Conditional("DEBUG")]
	public static void EndGroup()
	{
		if (!History.enabled)
		{
			return;
		}
		if (History._currentGroup == null)
		{
			Debug.LogError("History: Mismatched group - EndGroup called when none is active.");
			return;
		}
		History.Group currentGroup = History._currentGroup;
		History._currentGroup = null;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0002E3C4 File Offset: 0x0002C5C4
	[Conditional("DEBUG")]
	public static void LogBox(Vector3 position, float rotation, Vector2 size)
	{
		if (!History.enabled)
		{
			return;
		}
		History._currentFrame.boxes = History._currentFrame.boxes ?? new List<History.Box>();
		History._currentFrame.boxes.Add(new History.Box
		{
			position = position,
			rotation = rotation,
			size = size
		});
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0002E428 File Offset: 0x0002C628
	[Conditional("DEBUG")]
	public static void LogCircle(Vector3 position, float radius, float yScale = 1f, Color color = default(Color))
	{
		if (!History.enabled)
		{
			return;
		}
		if (color == default(Color))
		{
			color = Color.white;
		}
		History._currentFrame.circles = History._currentFrame.circles ?? new List<History.Circle>();
		History._currentFrame.circles.Add(new History.Circle
		{
			position = position,
			radius = radius,
			yScale = yScale,
			color = color
		});
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002E4AC File Offset: 0x0002C6AC
	[Conditional("DEBUG")]
	public static void LogSprite(Sprite sprite, Vector3 position, float rotation, Vector2 scale)
	{
		if (!History.enabled)
		{
			return;
		}
		History._currentFrame.sprites = History._currentFrame.sprites ?? new List<History.ActiveSprite>();
		History._currentFrame.sprites.Add(new History.ActiveSprite
		{
			sprite = sprite,
			position = position,
			rotation = rotation,
			scale = scale
		});
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0002E516 File Offset: 0x0002C716
	[Conditional("DEBUG")]
	public static void LogDot(Vector3 position)
	{
		if (!History.enabled)
		{
			return;
		}
		History._currentFrame.dots = History._currentFrame.dots ?? new List<Vector3>();
		History._currentFrame.dots.Add(position);
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0002E54D File Offset: 0x0002C74D
	[Conditional("DEBUG")]
	public static void LogState(string stateName)
	{
		History._currentFrame.stateName = stateName;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0002E55C File Offset: 0x0002C75C
	public static History.MiniTracePos MiniTrace(uint frameOffset = 0U)
	{
		StackTrace stackTrace = new StackTrace(true);
		for (int i = 0; i < stackTrace.FrameCount; i++)
		{
			if (stackTrace.GetFrame(i).GetMethod().DeclaringType.Name != "History")
			{
				StackFrame frame = stackTrace.GetFrame(i + (int)frameOffset);
				return new History.MiniTracePos
				{
					sourceFilePath = frame.GetFileName(),
					funcName = frame.GetMethod().Name,
					lineNum = frame.GetFileLineNumber()
				};
			}
		}
		return default(History.MiniTracePos);
	}

	// Token: 0x040006ED RID: 1773
	public static bool enabled;

	// Token: 0x040006EE RID: 1774
	public static Queue<History.Frame> frames = new Queue<History.Frame>();

	// Token: 0x040006EF RID: 1775
	public static int maxFrames = int.MaxValue;

	// Token: 0x040006F0 RID: 1776
	private static History.Group _currentGroup = null;

	// Token: 0x040006F1 RID: 1777
	private static int _totalCount;

	// Token: 0x040006F2 RID: 1778
	private static History.Frame _currentFrame;

	// Token: 0x020002DB RID: 731
	[Serializable]
	public struct MiniTracePos
	{
		// Token: 0x06001656 RID: 5718 RVA: 0x00098595 File Offset: 0x00096795
		public override string ToString()
		{
			return string.Format("{0} line {1}", this.funcName, this.lineNum);
		}

		// Token: 0x04001678 RID: 5752
		public string funcName;

		// Token: 0x04001679 RID: 5753
		public string sourceFilePath;

		// Token: 0x0400167A RID: 5754
		public int lineNum;
	}

	// Token: 0x020002DC RID: 732
	[Serializable]
	public struct Raycast
	{
		// Token: 0x0400167B RID: 5755
		public Vector3 from;

		// Token: 0x0400167C RID: 5756
		public Vector3 to;

		// Token: 0x0400167D RID: 5757
		public Color color;
	}

	// Token: 0x020002DD RID: 733
	[Serializable]
	public struct SceneLabel
	{
		// Token: 0x0400167E RID: 5758
		public Vector3 pos;

		// Token: 0x0400167F RID: 5759
		public string label;

		// Token: 0x04001680 RID: 5760
		public float yOffset;
	}

	// Token: 0x020002DE RID: 734
	public struct NamedObject
	{
		// Token: 0x04001681 RID: 5761
		public string name;

		// Token: 0x04001682 RID: 5762
		public object obj;

		// Token: 0x04001683 RID: 5763
		public int priority;

		// Token: 0x04001684 RID: 5764
		public int originalIdx;

		// Token: 0x04001685 RID: 5765
		public static object NoObject = new object();
	}

	// Token: 0x020002DF RID: 735
	[Serializable]
	public struct SerializedNamedObject
	{
		// Token: 0x04001686 RID: 5766
		public string name;

		// Token: 0x04001687 RID: 5767
		public string typeName;

		// Token: 0x04001688 RID: 5768
		public string data;

		// Token: 0x04001689 RID: 5769
		public int priority;

		// Token: 0x0400168A RID: 5770
		public int originalIdx;
	}

	// Token: 0x020002E0 RID: 736
	[Serializable]
	public struct Box
	{
		// Token: 0x0400168B RID: 5771
		public Vector3 position;

		// Token: 0x0400168C RID: 5772
		public float rotation;

		// Token: 0x0400168D RID: 5773
		public Vector2 size;
	}

	// Token: 0x020002E1 RID: 737
	[Serializable]
	public struct Circle
	{
		// Token: 0x0400168E RID: 5774
		public Vector3 position;

		// Token: 0x0400168F RID: 5775
		public float radius;

		// Token: 0x04001690 RID: 5776
		public float yScale;

		// Token: 0x04001691 RID: 5777
		public Color color;
	}

	// Token: 0x020002E2 RID: 738
	[Serializable]
	public struct ActiveSprite
	{
		// Token: 0x04001692 RID: 5778
		public Vector3 position;

		// Token: 0x04001693 RID: 5779
		public float rotation;

		// Token: 0x04001694 RID: 5780
		public Vector2 scale;

		// Token: 0x04001695 RID: 5781
		public Sprite sprite;

		// Token: 0x04001696 RID: 5782
		public string serializedAssetPath;
	}

	// Token: 0x020002E3 RID: 739
	[Serializable]
	public struct Frame
	{
		// Token: 0x04001697 RID: 5783
		public int frameCount;

		// Token: 0x04001698 RID: 5784
		public int idx;

		// Token: 0x04001699 RID: 5785
		public float time;

		// Token: 0x0400169A RID: 5786
		public string stateName;

		// Token: 0x0400169B RID: 5787
		public List<History.Raycast> raycasts;

		// Token: 0x0400169C RID: 5788
		public List<History.SceneLabel> sceneLabels;

		// Token: 0x0400169D RID: 5789
		public List<History.NamedObject> namedObjects;

		// Token: 0x0400169E RID: 5790
		public List<History.Box> boxes;

		// Token: 0x0400169F RID: 5791
		public List<History.Circle> circles;

		// Token: 0x040016A0 RID: 5792
		public List<History.ActiveSprite> sprites;

		// Token: 0x040016A1 RID: 5793
		public List<Vector3> dots;

		// Token: 0x040016A2 RID: 5794
		public List<History.SerializedNamedObject> serializedNamedObjects;
	}

	// Token: 0x020002E4 RID: 740
	[Serializable]
	public class Group
	{
		// Token: 0x040016A3 RID: 5795
		public string name;

		// Token: 0x040016A4 RID: 5796
		public int priority;

		// Token: 0x040016A5 RID: 5797
		public List<History.NamedObject> children;
	}
}
