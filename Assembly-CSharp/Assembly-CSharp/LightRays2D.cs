using System;
using UnityEngine;

// Token: 0x02000222 RID: 546
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class LightRays2D : MonoBehaviour
{
	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x060013A3 RID: 5027 RVA: 0x00089BB6 File Offset: 0x00087DB6
	private MaterialPropertyBlock propBlock
	{
		get
		{
			if (this._propBlock == null)
			{
				this._propBlock = new MaterialPropertyBlock();
			}
			return this._propBlock;
		}
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x060013A4 RID: 5028 RVA: 0x00089BD1 File Offset: 0x00087DD1
	// (set) Token: 0x060013A5 RID: 5029 RVA: 0x00089BD9 File Offset: 0x00087DD9
	public Color color1
	{
		get
		{
			return this._color1;
		}
		set
		{
			if (this._color1 == value)
			{
				return;
			}
			this._color1 = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x060013A6 RID: 5030 RVA: 0x00089BF7 File Offset: 0x00087DF7
	// (set) Token: 0x060013A7 RID: 5031 RVA: 0x00089BFF File Offset: 0x00087DFF
	public Color color2
	{
		get
		{
			return this._color2;
		}
		set
		{
			if (this._color2 == value)
			{
				return;
			}
			this._color2 = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x060013A8 RID: 5032 RVA: 0x00089C1D File Offset: 0x00087E1D
	// (set) Token: 0x060013A9 RID: 5033 RVA: 0x00089C25 File Offset: 0x00087E25
	public float speed
	{
		get
		{
			return this._speed;
		}
		set
		{
			if (this._speed == value)
			{
				return;
			}
			this._speed = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x060013AA RID: 5034 RVA: 0x00089C3E File Offset: 0x00087E3E
	// (set) Token: 0x060013AB RID: 5035 RVA: 0x00089C46 File Offset: 0x00087E46
	public float size
	{
		get
		{
			return this._size;
		}
		set
		{
			if (this._size == value)
			{
				return;
			}
			this._size = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x00089C5F File Offset: 0x00087E5F
	// (set) Token: 0x060013AD RID: 5037 RVA: 0x00089C67 File Offset: 0x00087E67
	public float skew
	{
		get
		{
			return this._skew;
		}
		set
		{
			if (this._skew == value)
			{
				return;
			}
			this._skew = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x060013AE RID: 5038 RVA: 0x00089C80 File Offset: 0x00087E80
	// (set) Token: 0x060013AF RID: 5039 RVA: 0x00089C88 File Offset: 0x00087E88
	public float shear
	{
		get
		{
			return this._shear;
		}
		set
		{
			if (this._shear == value)
			{
				return;
			}
			this._shear = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00089CA1 File Offset: 0x00087EA1
	// (set) Token: 0x060013B1 RID: 5041 RVA: 0x00089CA9 File Offset: 0x00087EA9
	public float fade
	{
		get
		{
			return this._fade;
		}
		set
		{
			if (this._fade == value)
			{
				return;
			}
			this._fade = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00089CC2 File Offset: 0x00087EC2
	// (set) Token: 0x060013B3 RID: 5043 RVA: 0x00089CCA File Offset: 0x00087ECA
	public float contrast
	{
		get
		{
			return this._contrast;
		}
		set
		{
			if (this._contrast == value)
			{
				return;
			}
			this._contrast = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00089CE3 File Offset: 0x00087EE3
	// (set) Token: 0x060013B5 RID: 5045 RVA: 0x00089CEB File Offset: 0x00087EEB
	public Texture2D maskTexture
	{
		get
		{
			return this._maskTexture;
		}
		set
		{
			if (this._maskTexture == value)
			{
				return;
			}
			this._maskTexture = value;
			this.RefreshMaterialPropertyBlock();
		}
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x00089D09 File Offset: 0x00087F09
	private void OnEnable()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.RefreshMaterialPropertyBlock();
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x00089D1D File Offset: 0x00087F1D
	private void OnValidate()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.RefreshMaterialPropertyBlock();
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x00089D31 File Offset: 0x00087F31
	private void RefreshMaterialPropertyBlock()
	{
		if (this.meshRenderer != null)
		{
			this.meshRenderer.GetPropertyBlock(this.propBlock);
			this.PopulatePropertyBlock();
			this.meshRenderer.SetPropertyBlock(this.propBlock);
		}
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x00089D6C File Offset: 0x00087F6C
	private void PopulatePropertyBlock()
	{
		this.propBlock.Clear();
		if (this.maskTexture != null)
		{
			this.propBlock.SetTexture("_MaskTex", this.maskTexture);
		}
		this.propBlock.SetColor("_Color1", this.color1);
		this.propBlock.SetColor("_Color2", this.color2);
		this.propBlock.SetFloat("_Speed", this.speed);
		this.propBlock.SetFloat("_Size", this.size);
		this.propBlock.SetFloat("_Skew", this.skew);
		this.propBlock.SetFloat("_Shear", this.shear);
		this.propBlock.SetFloat("_Fade", this.fade);
		this.propBlock.SetFloat("_Contrast", this.contrast);
	}

	// Token: 0x040012D4 RID: 4820
	private MeshRenderer meshRenderer;

	// Token: 0x040012D5 RID: 4821
	private MaterialPropertyBlock _propBlock;

	// Token: 0x040012D6 RID: 4822
	public const string color1ID = "_Color1";

	// Token: 0x040012D7 RID: 4823
	[SerializeField]
	private Color _color1 = Color.white;

	// Token: 0x040012D8 RID: 4824
	public const string color2ID = "_Color2";

	// Token: 0x040012D9 RID: 4825
	[SerializeField]
	private Color _color2 = Color.white;

	// Token: 0x040012DA RID: 4826
	public const string speedID = "_Speed";

	// Token: 0x040012DB RID: 4827
	[SerializeField]
	private float _speed = 0.5f;

	// Token: 0x040012DC RID: 4828
	public const string sizeID = "_Size";

	// Token: 0x040012DD RID: 4829
	[SerializeField]
	private float _size = 15f;

	// Token: 0x040012DE RID: 4830
	public const string skewID = "_Skew";

	// Token: 0x040012DF RID: 4831
	[SerializeField]
	private float _skew = 0.5f;

	// Token: 0x040012E0 RID: 4832
	public const string shearID = "_Shear";

	// Token: 0x040012E1 RID: 4833
	[SerializeField]
	private float _shear = 0.5f;

	// Token: 0x040012E2 RID: 4834
	public const string fadeID = "_Fade";

	// Token: 0x040012E3 RID: 4835
	[SerializeField]
	private float _fade = 1f;

	// Token: 0x040012E4 RID: 4836
	public const string contrastID = "_Contrast";

	// Token: 0x040012E5 RID: 4837
	[SerializeField]
	private float _contrast = 1f;

	// Token: 0x040012E6 RID: 4838
	public const string maskTextureID = "_MaskTex";

	// Token: 0x040012E7 RID: 4839
	[SerializeField]
	private Texture2D _maskTexture;
}
