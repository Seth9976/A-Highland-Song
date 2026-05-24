using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x0200007B RID: 123
	public static class Utility
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x000142A4 File Offset: 0x000124A4
		public static void DrawCircleGizmo(Vector2 center, float radius)
		{
			Vector2 vector = Utility.circleVertexList[0] * radius + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(vector, vector = Utility.circleVertexList[i] * radius + center);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00014306 File Offset: 0x00012506
		public static void DrawCircleGizmo(Vector2 center, float radius, Color color)
		{
			Gizmos.color = color;
			Utility.DrawCircleGizmo(center, radius);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00014318 File Offset: 0x00012518
		public static void DrawOvalGizmo(Vector2 center, Vector2 size)
		{
			Vector2 vector = size / 2f;
			Vector2 vector2 = Vector2.Scale(Utility.circleVertexList[0], vector) + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(vector2, vector2 = Vector2.Scale(Utility.circleVertexList[i], vector) + center);
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00014386 File Offset: 0x00012586
		public static void DrawOvalGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawOvalGizmo(center, size);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00014398 File Offset: 0x00012598
		public static void DrawRectGizmo(Rect rect)
		{
			Vector3 vector = new Vector3(rect.xMin, rect.yMin);
			Vector3 vector2 = new Vector3(rect.xMax, rect.yMin);
			Vector3 vector3 = new Vector3(rect.xMax, rect.yMax);
			Vector3 vector4 = new Vector3(rect.xMin, rect.yMax);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00014415 File Offset: 0x00012615
		public static void DrawRectGizmo(Rect rect, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(rect);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00014424 File Offset: 0x00012624
		public static void DrawRectGizmo(Vector2 center, Vector2 size)
		{
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			Vector3 vector = new Vector3(center.x - num, center.y - num2);
			Vector3 vector2 = new Vector3(center.x + num, center.y - num2);
			Vector3 vector3 = new Vector3(center.x + num, center.y + num2);
			Vector3 vector4 = new Vector3(center.x - num, center.y + num2);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000144C7 File Offset: 0x000126C7
		public static void DrawRectGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(center, size);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000144D6 File Offset: 0x000126D6
		public static bool GameObjectIsCulledOnCurrentCamera(GameObject gameObject)
		{
			return (Camera.current.cullingMask & (1 << gameObject.layer)) == 0;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000144F4 File Offset: 0x000126F4
		public static Color MoveColorTowards(Color color0, Color color1, float maxDelta)
		{
			float num = Mathf.MoveTowards(color0.r, color1.r, maxDelta);
			float num2 = Mathf.MoveTowards(color0.g, color1.g, maxDelta);
			float num3 = Mathf.MoveTowards(color0.b, color1.b, maxDelta);
			float num4 = Mathf.MoveTowards(color0.a, color1.a, maxDelta);
			return new Color(num, num2, num3, num4);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00014554 File Offset: 0x00012754
		public static float ApplyDeadZone(float value, float lowerDeadZone, float upperDeadZone)
		{
			float num = upperDeadZone - lowerDeadZone;
			if (value < 0f)
			{
				if (value > -lowerDeadZone)
				{
					return 0f;
				}
				if (value < -upperDeadZone)
				{
					return -1f;
				}
				return (value + lowerDeadZone) / num;
			}
			else
			{
				if (value < lowerDeadZone)
				{
					return 0f;
				}
				if (value > upperDeadZone)
				{
					return 1f;
				}
				return (value - lowerDeadZone) / num;
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000145A4 File Offset: 0x000127A4
		public static float ApplySmoothing(float thisValue, float lastValue, float deltaTime, float sensitivity)
		{
			if (Utility.Approximately(sensitivity, 1f))
			{
				return thisValue;
			}
			float num = deltaTime * sensitivity * 100f;
			if (Utility.IsNotZero(thisValue) && Utility.Sign(lastValue) != Utility.Sign(thisValue))
			{
				lastValue = 0f;
			}
			return Mathf.MoveTowards(lastValue, thisValue, num);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000145EF File Offset: 0x000127EF
		public static float ApplySnapping(float value, float threshold)
		{
			if (value < -threshold)
			{
				return -1f;
			}
			if (value > threshold)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001460B File Offset: 0x0001280B
		internal static bool TargetIsButton(InputControlType target)
		{
			return (target >= InputControlType.Action1 && target <= InputControlType.Action12) || (target >= InputControlType.Button0 && target <= InputControlType.Button19);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001462E File Offset: 0x0001282E
		internal static bool TargetIsStandard(InputControlType target)
		{
			return (target >= InputControlType.LeftStickUp && target <= InputControlType.Action12) || (target >= InputControlType.Command && target <= InputControlType.RightCommand);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00014650 File Offset: 0x00012850
		internal static bool TargetIsAlias(InputControlType target)
		{
			return target >= InputControlType.Command && target <= InputControlType.RightCommand;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00014668 File Offset: 0x00012868
		public static string ReadFromFile(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			return text;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014688 File Offset: 0x00012888
		public static void WriteToFile(string path, string data)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(data);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000146A2 File Offset: 0x000128A2
		public static float Abs(float value)
		{
			if (value >= 0f)
			{
				return value;
			}
			return -value;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000146B0 File Offset: 0x000128B0
		public static bool Approximately(float v1, float v2)
		{
			float num = v1 - v2;
			return num >= -1E-07f && num <= 1E-07f;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000146D6 File Offset: 0x000128D6
		public static bool Approximately(Vector2 v1, Vector2 v2)
		{
			return Utility.Approximately(v1.x, v2.x) && Utility.Approximately(v1.y, v2.y);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000146FE File Offset: 0x000128FE
		public static bool IsNotZero(float value)
		{
			return value < -1E-07f || value > 1E-07f;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00014712 File Offset: 0x00012912
		public static bool IsZero(float value)
		{
			return value >= -1E-07f && value <= 1E-07f;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00014729 File Offset: 0x00012929
		public static int Sign(float f)
		{
			if ((double)f >= 0.0)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001473B File Offset: 0x0001293B
		public static bool AbsoluteIsOverThreshold(float value, float threshold)
		{
			return value < -threshold || value > threshold;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00014748 File Offset: 0x00012948
		public static float NormalizeAngle(float angle)
		{
			while (angle < 0f)
			{
				angle += 360f;
			}
			while (angle > 360f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00014771 File Offset: 0x00012971
		public static float VectorToAngle(Vector2 vector)
		{
			if (Utility.IsZero(vector.x) && Utility.IsZero(vector.y))
			{
				return 0f;
			}
			return Utility.NormalizeAngle(Mathf.Atan2(vector.x, vector.y) * 57.29578f);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000147AF File Offset: 0x000129AF
		public static float Min(float v0, float v1)
		{
			if (v0 < v1)
			{
				return v0;
			}
			return v1;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000147B8 File Offset: 0x000129B8
		public static float Max(float v0, float v1)
		{
			if (v0 > v1)
			{
				return v0;
			}
			return v1;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000147C4 File Offset: 0x000129C4
		public static float Min(float v0, float v1, float v2, float v3)
		{
			float num = ((v0 >= v1) ? v1 : v0);
			float num2 = ((v2 >= v3) ? v3 : v2);
			if (num < num2)
			{
				return num;
			}
			return num2;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000147EC File Offset: 0x000129EC
		public static float Max(float v0, float v1, float v2, float v3)
		{
			float num = ((v0 <= v1) ? v1 : v0);
			float num2 = ((v2 <= v3) ? v3 : v2);
			if (num > num2)
			{
				return num;
			}
			return num2;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00014814 File Offset: 0x00012A14
		internal static float ValueFromSides(float negativeSide, float positiveSide)
		{
			float num = Utility.Abs(negativeSide);
			float num2 = Utility.Abs(positiveSide);
			if (Utility.Approximately(num, num2))
			{
				return 0f;
			}
			if (num <= num2)
			{
				return num2;
			}
			return -num;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00014846 File Offset: 0x00012A46
		internal static float ValueFromSides(float negativeSide, float positiveSide, bool invertSides)
		{
			if (invertSides)
			{
				return Utility.ValueFromSides(positiveSide, negativeSide);
			}
			return Utility.ValueFromSides(negativeSide, positiveSide);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001485A File Offset: 0x00012A5A
		public static void ArrayResize<T>(ref T[] array, int capacity)
		{
			if (array == null || capacity > array.Length)
			{
				Array.Resize<T>(ref array, Utility.NextPowerOfTwo(capacity));
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014873 File Offset: 0x00012A73
		public static void ArrayExpand<T>(ref T[] array, int capacity)
		{
			if (array == null || capacity > array.Length)
			{
				array = new T[Utility.NextPowerOfTwo(capacity)];
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001488D File Offset: 0x00012A8D
		public static void ArrayAppend<T>(ref T[] array, T item)
		{
			if (array == null)
			{
				array = new T[1];
				array[0] = item;
				return;
			}
			Array.Resize<T>(ref array, array.Length + 1);
			array[array.Length - 1] = item;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000148BF File Offset: 0x00012ABF
		public static void ArrayAppend<T>(ref T[] array, T[] items)
		{
			if (array == null)
			{
				array = new T[items.Length];
				Array.Copy(items, array, items.Length);
				return;
			}
			Array.Resize<T>(ref array, array.Length + items.Length);
			Array.ConstrainedCopy(items, 0, array, array.Length - items.Length, items.Length);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000148FD File Offset: 0x00012AFD
		public static int NextPowerOfTwo(int value)
		{
			if (value > 0)
			{
				value--;
				value |= value >> 1;
				value |= value >> 2;
				value |= value >> 4;
				value |= value >> 8;
				value |= value >> 16;
				value++;
				return value;
			}
			return 0;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00014934 File Offset: 0x00012B34
		internal static bool Is32Bit
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001493E File Offset: 0x00012B3E
		internal static bool Is64Bit
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00014948 File Offset: 0x00012B48
		public static string GetPlatformName(bool uppercase = true)
		{
			string windowsVersion = Utility.GetWindowsVersion();
			if (!uppercase)
			{
				return windowsVersion;
			}
			return windowsVersion.ToUpper();
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00014968 File Offset: 0x00012B68
		private static string GetHumanUnderstandableWindowsVersion()
		{
			Version version = Environment.OSVersion.Version;
			if (version.Major == 6)
			{
				switch (version.Minor)
				{
				case 1:
					return "7";
				case 2:
					return "8";
				case 3:
					return "8.1";
				default:
					return "Vista";
				}
			}
			else
			{
				if (version.Major != 5)
				{
					return version.Major.ToString();
				}
				int minor = version.Minor;
				if (minor - 1 <= 1)
				{
					return "XP";
				}
				return "2000";
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000149F0 File Offset: 0x00012BF0
		public static string GetWindowsVersion()
		{
			string humanUnderstandableWindowsVersion = Utility.GetHumanUnderstandableWindowsVersion();
			string text = (Utility.Is32Bit ? "32Bit" : "64Bit");
			return string.Concat(new string[]
			{
				"Windows ",
				humanUnderstandableWindowsVersion,
				" ",
				text,
				" Build ",
				Utility.GetSystemBuildNumber().ToString()
			});
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00014A52 File Offset: 0x00012C52
		public static int GetSystemBuildNumber()
		{
			return Environment.OSVersion.Version.Build;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00014A63 File Offset: 0x00012C63
		public static void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014A6B File Offset: 0x00012C6B
		internal static string PluginFileExtension()
		{
			return ".dll";
		}

		// Token: 0x04000482 RID: 1154
		public const float Epsilon = 1E-07f;

		// Token: 0x04000483 RID: 1155
		private static readonly Vector2[] circleVertexList = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(0.2588f, 0.9659f),
			new Vector2(0.5f, 0.866f),
			new Vector2(0.7071f, 0.7071f),
			new Vector2(0.866f, 0.5f),
			new Vector2(0.9659f, 0.2588f),
			new Vector2(1f, 0f),
			new Vector2(0.9659f, -0.2588f),
			new Vector2(0.866f, -0.5f),
			new Vector2(0.7071f, -0.7071f),
			new Vector2(0.5f, -0.866f),
			new Vector2(0.2588f, -0.9659f),
			new Vector2(0f, -1f),
			new Vector2(-0.2588f, -0.9659f),
			new Vector2(-0.5f, -0.866f),
			new Vector2(-0.7071f, -0.7071f),
			new Vector2(-0.866f, -0.5f),
			new Vector2(-0.9659f, -0.2588f),
			new Vector2(-1f, -0f),
			new Vector2(-0.9659f, 0.2588f),
			new Vector2(-0.866f, 0.5f),
			new Vector2(-0.7071f, 0.7071f),
			new Vector2(-0.5f, 0.866f),
			new Vector2(-0.2588f, 0.9659f),
			new Vector2(0f, 1f)
		};
	}
}
