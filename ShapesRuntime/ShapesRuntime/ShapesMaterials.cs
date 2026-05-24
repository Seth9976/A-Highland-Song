using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200005F RID: 95
	internal class ShapesMaterials
	{
		// Token: 0x17000133 RID: 307
		public Material this[ShapesBlendMode type]
		{
			get
			{
				return this.materials[(int)type];
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002481C File Offset: 0x00022A1C
		public ShapesMaterials(string shaderName, params string[] keywords)
		{
			int num = Enum.GetNames(typeof(ShapesBlendMode)).Length;
			this.materials = new Material[num];
			for (int i = 0; i < num; i++)
			{
				Material[] array = this.materials;
				int num2 = i;
				ShapesBlendMode shapesBlendMode = (ShapesBlendMode)i;
				array[num2] = ShapesMaterials.InitMaterial(shaderName, shapesBlendMode.ToString(), keywords);
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00024878 File Offset: 0x00022A78
		public static string GetMaterialName(string shaderName, string blendModeSuffix, params string[] keywords)
		{
			string text = "";
			if (keywords != null && keywords.Length != 0)
			{
				text = " [" + string.Join("][", keywords) + "]";
			}
			return shaderName + " " + blendModeSuffix + text;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000248BA File Offset: 0x00022ABA
		public static void ApplyDefaultGlobalProperties(Material mat)
		{
			mat.SetInt(ShapesMaterialUtils.propZTest, 4);
			mat.SetFloat(ShapesMaterialUtils.propZOffsetFactor, 0f);
			mat.SetInt(ShapesMaterialUtils.propZOffsetUnits, 0);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000248E4 File Offset: 0x00022AE4
		private static Material CreateShapesMaterial(Shader shader, HideFlags hideFlags, params string[] keywords)
		{
			Material material = new Material(shader)
			{
				hideFlags = hideFlags,
				enableInstancing = true
			};
			if (keywords != null)
			{
				foreach (string text in keywords)
				{
					material.EnableKeyword(text);
				}
			}
			ShapesMaterials.ApplyDefaultGlobalProperties(material);
			return material;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0002492C File Offset: 0x00022B2C
		private static Material InitMaterial(string shaderName, string blendModeSuffix, params string[] keywords)
		{
			shaderName = "Shapes/" + shaderName + " " + blendModeSuffix;
			Shader shader = Shader.Find(shaderName);
			if (shader == null)
			{
				Debug.LogError("Could not find shader " + shaderName);
				return null;
			}
			return ShapesMaterials.CreateShapesMaterial(shader, HideFlags.HideAndDontSave, keywords);
		}

		// Token: 0x040001EF RID: 495
		private const bool USE_INSTANCING = true;

		// Token: 0x040001F0 RID: 496
		public const string SHAPES_SHADER_PATH_PREFIX = "Shapes/";

		// Token: 0x040001F1 RID: 497
		private readonly Material[] materials;
	}
}
