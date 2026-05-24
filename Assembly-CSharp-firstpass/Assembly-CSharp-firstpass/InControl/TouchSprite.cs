using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public class TouchSprite
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x000116CD File Offset: 0x0000F8CD
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x000116D5 File Offset: 0x0000F8D5
		public bool Dirty { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x000116DE File Offset: 0x0000F8DE
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x000116E6 File Offset: 0x0000F8E6
		public bool Ready { get; set; }

		// Token: 0x060004AA RID: 1194 RVA: 0x000116F0 File Offset: 0x0000F8F0
		public TouchSprite()
		{
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00011760 File Offset: 0x0000F960
		public TouchSprite(float size)
		{
			this.size = Vector2.one * size;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000117E0 File Offset: 0x0000F9E0
		public void Create(string gameObjectName, Transform parentTransform, int sortingOrder)
		{
			this.spriteGameObject = this.CreateSpriteGameObject(gameObjectName, parentTransform);
			this.spriteRenderer = this.CreateSpriteRenderer(this.spriteGameObject, this.idleSprite, sortingOrder);
			this.spriteRenderer.color = this.idleColor;
			this.Ready = true;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001182C File Offset: 0x0000FA2C
		public void Delete()
		{
			this.Ready = false;
			Object.Destroy(this.spriteGameObject);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00011840 File Offset: 0x0000FA40
		public void Update()
		{
			this.Update(false);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001184C File Offset: 0x0000FA4C
		public void Update(bool forceUpdate)
		{
			if (this.Dirty || forceUpdate)
			{
				if (this.spriteRenderer != null)
				{
					this.spriteRenderer.sprite = (this.State ? this.busySprite : this.idleSprite);
				}
				if (this.sizeUnitType == TouchUnitType.Pixels)
				{
					Vector2 vector = TouchUtility.RoundVector(this.size);
					this.ScaleSpriteInPixels(this.spriteGameObject, this.spriteRenderer, vector);
					this.worldSize = vector * TouchManager.PixelToWorld;
				}
				else
				{
					this.ScaleSpriteInPercent(this.spriteGameObject, this.spriteRenderer, this.size);
					if (this.lockAspectRatio)
					{
						this.worldSize = this.size * TouchManager.PercentToWorld;
					}
					else
					{
						this.worldSize = Vector2.Scale(this.size, TouchManager.ViewSize);
					}
				}
				this.Dirty = false;
			}
			if (this.spriteRenderer != null)
			{
				Color color = (this.State ? this.busyColor : this.idleColor);
				if (this.spriteRenderer.color != color)
				{
					this.spriteRenderer.color = Utility.MoveColorTowards(this.spriteRenderer.color, color, 5f * Time.unscaledDeltaTime);
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001198C File Offset: 0x0000FB8C
		private GameObject CreateSpriteGameObject(string name, Transform parentTransform)
		{
			return new GameObject(name)
			{
				transform = 
				{
					parent = parentTransform,
					localPosition = Vector3.zero,
					localScale = Vector3.one
				},
				layer = parentTransform.gameObject.layer
			};
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000119DC File Offset: 0x0000FBDC
		private SpriteRenderer CreateSpriteRenderer(GameObject spriteGameObject, Sprite sprite, int sortingOrder)
		{
			if (!TouchSprite.spriteRendererMaterial)
			{
				TouchSprite.spriteRendererShader = Shader.Find("Sprites/Default");
				TouchSprite.spriteRendererMaterial = new Material(TouchSprite.spriteRendererShader);
				TouchSprite.spriteRendererPixelSnapId = Shader.PropertyToID("PixelSnap");
			}
			SpriteRenderer spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
			spriteRenderer.sortingOrder = sortingOrder;
			spriteRenderer.sharedMaterial = TouchSprite.spriteRendererMaterial;
			spriteRenderer.sharedMaterial.SetFloat(TouchSprite.spriteRendererPixelSnapId, 1f);
			return spriteRenderer;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00011A58 File Offset: 0x0000FC58
		private void ScaleSpriteInPixels(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			float num = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.bounds.size.x;
			float num2 = TouchManager.PixelToWorld * num;
			float num3 = num2 * size.x / spriteRenderer.sprite.rect.width;
			float num4 = num2 * size.y / spriteRenderer.sprite.rect.height;
			spriteGameObject.transform.localScale = new Vector3(num3, num4);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00011B08 File Offset: 0x0000FD08
		private void ScaleSpriteInPercent(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			if (this.lockAspectRatio)
			{
				float num = Mathf.Min(TouchManager.ViewSize.x, TouchManager.ViewSize.y);
				float num2 = num * size.x / spriteRenderer.sprite.bounds.size.x;
				float num3 = num * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(num2, num3);
				return;
			}
			float num4 = TouchManager.ViewSize.x * size.x / spriteRenderer.sprite.bounds.size.x;
			float num5 = TouchManager.ViewSize.y * size.y / spriteRenderer.sprite.bounds.size.y;
			spriteGameObject.transform.localScale = new Vector3(num4, num5);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00011C18 File Offset: 0x0000FE18
		public bool Contains(Vector2 testWorldPoint)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				float num = (testWorldPoint.x - this.Position.x) / this.worldSize.x;
				float num2 = (testWorldPoint.y - this.Position.y) / this.worldSize.y;
				return num * num + num2 * num2 < 0.25f;
			}
			float num3 = Utility.Abs(testWorldPoint.x - this.Position.x) * 2f;
			float num4 = Utility.Abs(testWorldPoint.y - this.Position.y) * 2f;
			return num3 <= this.worldSize.x && num4 <= this.worldSize.y;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00011CD3 File Offset: 0x0000FED3
		public bool Contains(Touch touch)
		{
			return this.Contains(TouchManager.ScreenToWorldPoint(touch.position));
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00011CEB File Offset: 0x0000FEEB
		public void DrawGizmos(Vector3 position, Color color)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				Utility.DrawOvalGizmo(position, this.WorldSize, color);
				return;
			}
			Utility.DrawRectGizmo(position, this.WorldSize, color);
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00011D1A File Offset: 0x0000FF1A
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00011D22 File Offset: 0x0000FF22
		public bool State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					this.state = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00011D3B File Offset: 0x0000FF3B
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00011D43 File Offset: 0x0000FF43
		public Sprite BusySprite
		{
			get
			{
				return this.busySprite;
			}
			set
			{
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00011D61 File Offset: 0x0000FF61
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00011D69 File Offset: 0x0000FF69
		public Sprite IdleSprite
		{
			get
			{
				return this.idleSprite;
			}
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x00011D87 File Offset: 0x0000FF87
		public Sprite Sprite
		{
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00011DC1 File Offset: 0x0000FFC1
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00011DC9 File Offset: 0x0000FFC9
		public Color BusyColor
		{
			get
			{
				return this.busyColor;
			}
			set
			{
				if (this.busyColor != value)
				{
					this.busyColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00011DE7 File Offset: 0x0000FFE7
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00011DEF File Offset: 0x0000FFEF
		public Color IdleColor
		{
			get
			{
				return this.idleColor;
			}
			set
			{
				if (this.idleColor != value)
				{
					this.idleColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00011E0D File Offset: 0x0001000D
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00011E15 File Offset: 0x00010015
		public TouchSpriteShape Shape
		{
			get
			{
				return this.shape;
			}
			set
			{
				if (this.shape != value)
				{
					this.shape = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00011E2E File Offset: 0x0001002E
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00011E36 File Offset: 0x00010036
		public TouchUnitType SizeUnitType
		{
			get
			{
				return this.sizeUnitType;
			}
			set
			{
				if (this.sizeUnitType != value)
				{
					this.sizeUnitType = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00011E4F File Offset: 0x0001004F
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00011E57 File Offset: 0x00010057
		public Vector2 Size
		{
			get
			{
				return this.size;
			}
			set
			{
				if (this.size != value)
				{
					this.size = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00011E75 File Offset: 0x00010075
		public Vector2 WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00011E7D File Offset: 0x0001007D
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00011EA2 File Offset: 0x000100A2
		public Vector3 Position
		{
			get
			{
				if (!this.spriteGameObject)
				{
					return Vector3.zero;
				}
				return this.spriteGameObject.transform.position;
			}
			set
			{
				if (this.spriteGameObject)
				{
					this.spriteGameObject.transform.position = value;
				}
			}
		}

		// Token: 0x04000440 RID: 1088
		[SerializeField]
		private Sprite idleSprite;

		// Token: 0x04000441 RID: 1089
		[SerializeField]
		private Sprite busySprite;

		// Token: 0x04000442 RID: 1090
		[SerializeField]
		private Color idleColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04000443 RID: 1091
		[SerializeField]
		private Color busyColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000444 RID: 1092
		[SerializeField]
		private TouchSpriteShape shape;

		// Token: 0x04000445 RID: 1093
		[SerializeField]
		private TouchUnitType sizeUnitType;

		// Token: 0x04000446 RID: 1094
		[SerializeField]
		private Vector2 size = new Vector2(10f, 10f);

		// Token: 0x04000447 RID: 1095
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x04000448 RID: 1096
		[SerializeField]
		[HideInInspector]
		private Vector2 worldSize;

		// Token: 0x04000449 RID: 1097
		private GameObject spriteGameObject;

		// Token: 0x0400044A RID: 1098
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400044B RID: 1099
		private bool state;

		// Token: 0x0400044E RID: 1102
		private static Shader spriteRendererShader;

		// Token: 0x0400044F RID: 1103
		private static Material spriteRendererMaterial;

		// Token: 0x04000450 RID: 1104
		private static int spriteRendererPixelSnapId;
	}
}
