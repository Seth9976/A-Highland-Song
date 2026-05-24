using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000008 RID: 8
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Quad")]
	public class Quad : ShapeRenderer
	{
		// Token: 0x17000051 RID: 81
		public Vector3 this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.A;
				case 1:
					return this.B;
				case 2:
					return this.C;
				case 3:
					return this.D;
				default:
					throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to access element {0}", index));
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.A = value;
					return;
				case 1:
					this.B = value;
					return;
				case 2:
					this.C = value;
					return;
				case 3:
					this.D = value;
					return;
				default:
					throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to set element {0}", index));
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003BC6 File Offset: 0x00001DC6
		public Vector3 GetQuadVertex(int index)
		{
			return this[index];
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public Vector3 SetQuadVertex(int index, Vector3 value)
		{
			this[index] = value;
			return value;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003BE8 File Offset: 0x00001DE8
		public Color GetQuadColor(int index)
		{
			switch (index)
			{
			case 0:
				return this.Color;
			case 1:
				return this.ColorB;
			case 2:
				return this.ColorC;
			case 3:
				return this.ColorD;
			default:
				throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to access element {0}", index));
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003C40 File Offset: 0x00001E40
		public void SetQuadColor(int index, Color color)
		{
			switch (index)
			{
			case 0:
				this.Color = color;
				return;
			case 1:
				this.ColorB = color;
				return;
			case 2:
				this.ColorC = color;
				return;
			case 3:
				this.ColorD = color;
				return;
			default:
				throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to set element {0}", index));
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003C9A File Offset: 0x00001E9A
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003CA2 File Offset: 0x00001EA2
		public Quad.QuadColorMode ColorMode
		{
			get
			{
				return this.colorMode;
			}
			set
			{
				this.colorMode = value;
				base.ApplyProperties();
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003CB1 File Offset: 0x00001EB1
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003CBC File Offset: 0x00001EBC
		public Vector3 A
		{
			get
			{
				return this.a;
			}
			set
			{
				int propA = ShapesMaterialUtils.propA;
				this.a = value;
				base.SetVector3Now(propA, value);
				this.CheckAutoSetD();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003CE4 File Offset: 0x00001EE4
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003CEC File Offset: 0x00001EEC
		public Vector3 B
		{
			get
			{
				return this.b;
			}
			set
			{
				int propB = ShapesMaterialUtils.propB;
				this.b = value;
				base.SetVector3Now(propB, value);
				this.CheckAutoSetD();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003D14 File Offset: 0x00001F14
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00003D1C File Offset: 0x00001F1C
		public Vector3 C
		{
			get
			{
				return this.c;
			}
			set
			{
				int propC = ShapesMaterialUtils.propC;
				this.c = value;
				base.SetVector3Now(propC, value);
				this.CheckAutoSetD();
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003D44 File Offset: 0x00001F44
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00003D4C File Offset: 0x00001F4C
		public Vector3 D
		{
			get
			{
				return this.d;
			}
			set
			{
				if (this.autoSetD)
				{
					Debug.LogWarning("tried to set D when auto-set is enabled, you might want to turn off auto-set on this object", base.gameObject);
					return;
				}
				int propD = ShapesMaterialUtils.propD;
				this.d = value;
				base.SetVector3Now(propD, value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003D87 File Offset: 0x00001F87
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00003D8F File Offset: 0x00001F8F
		public bool IsUsingAutoD
		{
			get
			{
				return this.autoSetD;
			}
			set
			{
				this.autoSetD = value;
				this.AutoSetD();
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00003D9E File Offset: 0x00001F9E
		public Vector3 DAuto
		{
			get
			{
				return this.A + (this.C - this.B);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003DBC File Offset: 0x00001FBC
		private void AutoSetD()
		{
			base.SetVector3(ShapesMaterialUtils.propD, this.DAuto);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003DCF File Offset: 0x00001FCF
		private void CheckAutoSetD()
		{
			if (this.autoSetD)
			{
				this.AutoSetD();
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00003DDF File Offset: 0x00001FDF
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00003DE8 File Offset: 0x00001FE8
		public override Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColor(propColorB, value);
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColor(propColorC, value);
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColorNow(propColorD, value);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003E49 File Offset: 0x00002049
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00003E54 File Offset: 0x00002054
		public Color ColorLeft
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColorNow(propColorB, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003E8B File Offset: 0x0000208B
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00003E94 File Offset: 0x00002094
		public Color ColorTop
		{
			get
			{
				return this.colorB;
			}
			set
			{
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColor(propColorB, value);
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003ECB File Offset: 0x000020CB
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003ED4 File Offset: 0x000020D4
		public Color ColorRight
		{
			get
			{
				return this.colorC;
			}
			set
			{
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColor(propColorC, value);
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColorNow(propColorD, value);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003F0B File Offset: 0x0000210B
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00003F14 File Offset: 0x00002114
		public Color ColorBottom
		{
			get
			{
				return this.colorD;
			}
			set
			{
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColor(propColorD, value);
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003F4B File Offset: 0x0000214B
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00003F54 File Offset: 0x00002154
		public Color ColorA
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00003F76 File Offset: 0x00002176
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00003F80 File Offset: 0x00002180
		public Color ColorB
		{
			get
			{
				return this.colorB;
			}
			set
			{
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColorNow(propColorB, value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003FA2 File Offset: 0x000021A2
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00003FAC File Offset: 0x000021AC
		public Color ColorC
		{
			get
			{
				return this.colorC;
			}
			set
			{
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00003FCE File Offset: 0x000021CE
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00003FD8 File Offset: 0x000021D8
		public Color ColorD
		{
			get
			{
				return this.colorD;
			}
			set
			{
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColorNow(propColorD, value);
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003FFC File Offset: 0x000021FC
		private protected override void SetAllMaterialProperties()
		{
			base.SetVector3(ShapesMaterialUtils.propA, this.a);
			base.SetVector3(ShapesMaterialUtils.propB, this.b);
			base.SetVector3(ShapesMaterialUtils.propC, this.c);
			if (this.autoSetD)
			{
				this.AutoSetD();
			}
			else
			{
				base.SetVector3(ShapesMaterialUtils.propD, this.d);
			}
			switch (this.colorMode)
			{
			case Quad.QuadColorMode.Single:
				base.SetColor(ShapesMaterialUtils.propColorB, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorC, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorD, this.Color);
				return;
			case Quad.QuadColorMode.Horizontal:
				base.SetColor(ShapesMaterialUtils.propColorB, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorC);
				base.SetColor(ShapesMaterialUtils.propColorD, this.colorC);
				return;
			case Quad.QuadColorMode.Vertical:
				base.SetColor(ShapesMaterialUtils.propColor, this.colorD);
				base.SetColor(ShapesMaterialUtils.propColorB, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorD, this.colorD);
				return;
			case Quad.QuadColorMode.PerCorner:
				base.SetColor(ShapesMaterialUtils.propColorB, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorC);
				base.SetColor(ShapesMaterialUtils.propColorD, this.colorD);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004165 File Offset: 0x00002365
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004168 File Offset: 0x00002368
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000416B File Offset: 0x0000236B
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.QuadMesh[0];
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004174 File Offset: 0x00002374
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matQuad[base.BlendMode] };
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004190 File Offset: 0x00002390
		private protected override Bounds GetBounds_Internal()
		{
			Vector3 vector = Vector3.Min(Vector3.Min(this.a, this.b), this.c);
			Vector3 vector2 = Vector3.Max(Vector3.Max(this.a, this.b), this.c);
			return new Bounds((vector + vector2) / 2f, ShapesMath.Abs(vector2 - vector));
		}

		// Token: 0x0400002B RID: 43
		[SerializeField]
		private Quad.QuadColorMode colorMode;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private Vector3 a = new Vector2(-0.5f, -0.5f);

		// Token: 0x0400002D RID: 45
		[SerializeField]
		private Vector3 b = new Vector2(-0.5f, 0.5f);

		// Token: 0x0400002E RID: 46
		[SerializeField]
		private Vector3 c = new Vector2(0.5f, 0.5f);

		// Token: 0x0400002F RID: 47
		[SerializeField]
		private Vector3 d = new Vector2(0.5f, -0.5f);

		// Token: 0x04000030 RID: 48
		[SerializeField]
		private bool autoSetD;

		// Token: 0x04000031 RID: 49
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorB = Color.white;

		// Token: 0x04000032 RID: 50
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorC = Color.white;

		// Token: 0x04000033 RID: 51
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorD = Color.white;

		// Token: 0x0200006A RID: 106
		public enum QuadColorMode
		{
			// Token: 0x0400025C RID: 604
			Single,
			// Token: 0x0400025D RID: 605
			Horizontal,
			// Token: 0x0400025E RID: 606
			Vertical,
			// Token: 0x0400025F RID: 607
			PerCorner
		}
	}
}
