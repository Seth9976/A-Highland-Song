using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x02000462 RID: 1122
	public static class LightmapperUtils
	{
		// Token: 0x060027C6 RID: 10182 RVA: 0x00041EE4 File Offset: 0x000400E4
		public static LightMode Extract(LightmapBakeType baketype)
		{
			return (baketype == LightmapBakeType.Realtime) ? LightMode.Realtime : ((baketype == LightmapBakeType.Mixed) ? LightMode.Mixed : LightMode.Baked);
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00041F08 File Offset: 0x00040108
		public static LinearColor ExtractIndirect(Light l)
		{
			return LinearColor.Convert(l.color, l.intensity * l.bounceIntensity);
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00041F34 File Offset: 0x00040134
		public static float ExtractInnerCone(Light l)
		{
			return 2f * Mathf.Atan(Mathf.Tan(l.spotAngle * 0.5f * 0.017453292f) * 46f / 64f);
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00041F74 File Offset: 0x00040174
		private static Color ExtractColorTemperature(Light l)
		{
			Color color = new Color(1f, 1f, 1f);
			bool flag = l.useColorTemperature && GraphicsSettings.lightsUseLinearIntensity;
			if (flag)
			{
				color = Mathf.CorrelatedColorTemperatureToRGB(l.colorTemperature);
			}
			return color;
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00041FBD File Offset: 0x000401BD
		private static void ApplyColorTemperature(Color cct, ref LinearColor lightColor)
		{
			lightColor.red *= cct.r;
			lightColor.green *= cct.g;
			lightColor.blue *= cct.b;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00041FFC File Offset: 0x000401FC
		public static void Extract(Light l, ref DirectionalLight dir)
		{
			dir.instanceID = l.GetInstanceID();
			dir.mode = LightmapperUtils.Extract(l.bakingOutput.lightmapBakeType);
			dir.shadow = l.shadows > LightShadows.None;
			dir.position = l.transform.position;
			dir.orientation = l.transform.rotation;
			Color color = LightmapperUtils.ExtractColorTemperature(l);
			LinearColor linearColor = LinearColor.Convert(l.color, l.intensity);
			LinearColor linearColor2 = LightmapperUtils.ExtractIndirect(l);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor2);
			dir.color = linearColor;
			dir.indirectColor = linearColor2;
			dir.penumbraWidthRadian = 0f;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000420A8 File Offset: 0x000402A8
		public static void Extract(Light l, ref PointLight point)
		{
			point.instanceID = l.GetInstanceID();
			point.mode = LightmapperUtils.Extract(l.bakingOutput.lightmapBakeType);
			point.shadow = l.shadows > LightShadows.None;
			point.position = l.transform.position;
			point.orientation = l.transform.rotation;
			Color color = LightmapperUtils.ExtractColorTemperature(l);
			LinearColor linearColor = LinearColor.Convert(l.color, l.intensity);
			LinearColor linearColor2 = LightmapperUtils.ExtractIndirect(l);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor2);
			point.color = linearColor;
			point.indirectColor = linearColor2;
			point.range = l.range;
			point.sphereRadius = 0f;
			point.falloff = FalloffType.Legacy;
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x00042168 File Offset: 0x00040368
		public static void Extract(Light l, ref SpotLight spot)
		{
			spot.instanceID = l.GetInstanceID();
			spot.mode = LightmapperUtils.Extract(l.bakingOutput.lightmapBakeType);
			spot.shadow = l.shadows > LightShadows.None;
			spot.position = l.transform.position;
			spot.orientation = l.transform.rotation;
			Color color = LightmapperUtils.ExtractColorTemperature(l);
			LinearColor linearColor = LinearColor.Convert(l.color, l.intensity);
			LinearColor linearColor2 = LightmapperUtils.ExtractIndirect(l);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor2);
			spot.color = linearColor;
			spot.indirectColor = linearColor2;
			spot.range = l.range;
			spot.sphereRadius = 0f;
			spot.coneAngle = l.spotAngle * 0.017453292f;
			spot.innerConeAngle = LightmapperUtils.ExtractInnerCone(l);
			spot.falloff = FalloffType.Legacy;
			spot.angularFalloff = AngularFalloffType.LUT;
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x0004224C File Offset: 0x0004044C
		public static void Extract(Light l, ref RectangleLight rect)
		{
			rect.instanceID = l.GetInstanceID();
			rect.mode = LightmapperUtils.Extract(l.bakingOutput.lightmapBakeType);
			rect.shadow = l.shadows > LightShadows.None;
			rect.position = l.transform.position;
			rect.orientation = l.transform.rotation;
			Color color = LightmapperUtils.ExtractColorTemperature(l);
			LinearColor linearColor = LinearColor.Convert(l.color, l.intensity);
			LinearColor linearColor2 = LightmapperUtils.ExtractIndirect(l);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor2);
			rect.color = linearColor;
			rect.indirectColor = linearColor2;
			rect.range = l.range;
			rect.width = 0f;
			rect.height = 0f;
			rect.falloff = FalloffType.Legacy;
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x00042318 File Offset: 0x00040518
		public static void Extract(Light l, ref DiscLight disc)
		{
			disc.instanceID = l.GetInstanceID();
			disc.mode = LightmapperUtils.Extract(l.bakingOutput.lightmapBakeType);
			disc.shadow = l.shadows > LightShadows.None;
			disc.position = l.transform.position;
			disc.orientation = l.transform.rotation;
			Color color = LightmapperUtils.ExtractColorTemperature(l);
			LinearColor linearColor = LinearColor.Convert(l.color, l.intensity);
			LinearColor linearColor2 = LightmapperUtils.ExtractIndirect(l);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor);
			LightmapperUtils.ApplyColorTemperature(color, ref linearColor2);
			disc.color = linearColor;
			disc.indirectColor = linearColor2;
			disc.range = l.range;
			disc.radius = 0f;
			disc.falloff = FalloffType.Legacy;
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000423D8 File Offset: 0x000405D8
		public static void Extract(Light l, out Cookie cookie)
		{
			cookie.instanceID = (l.cookie ? l.cookie.GetInstanceID() : 0);
			cookie.scale = 1f;
			cookie.sizes = ((l.type == LightType.Directional && l.cookie) ? new Vector2(l.cookieSize, l.cookieSize) : new Vector2(1f, 1f));
		}
	}
}
