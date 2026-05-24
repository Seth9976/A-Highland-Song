using System;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x02000245 RID: 581
	public class PanelSettings : ScriptableObject
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x00042CDC File Offset: 0x00040EDC
		// (set) Token: 0x06001146 RID: 4422 RVA: 0x00042CF4 File Offset: 0x00040EF4
		public ThemeStyleSheet themeStyleSheet
		{
			get
			{
				return this.themeUss;
			}
			set
			{
				this.themeUss = value;
				this.ApplyThemeStyleSheet(null);
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x00042D06 File Offset: 0x00040F06
		// (set) Token: 0x06001148 RID: 4424 RVA: 0x00042D0E File Offset: 0x00040F0E
		public RenderTexture targetTexture
		{
			get
			{
				return this.m_TargetTexture;
			}
			set
			{
				this.m_TargetTexture = value;
				this.m_PanelAccess.SetTargetTexture();
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x00042D24 File Offset: 0x00040F24
		// (set) Token: 0x0600114A RID: 4426 RVA: 0x00042D2C File Offset: 0x00040F2C
		public PanelScaleMode scaleMode
		{
			get
			{
				return this.m_ScaleMode;
			}
			set
			{
				this.m_ScaleMode = value;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x00042D35 File Offset: 0x00040F35
		// (set) Token: 0x0600114C RID: 4428 RVA: 0x00042D3D File Offset: 0x00040F3D
		public float scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00042D46 File Offset: 0x00040F46
		// (set) Token: 0x0600114E RID: 4430 RVA: 0x00042D4E File Offset: 0x00040F4E
		public float referenceDpi
		{
			get
			{
				return this.m_ReferenceDpi;
			}
			set
			{
				this.m_ReferenceDpi = ((value >= 1f) ? value : 96f);
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x00042D66 File Offset: 0x00040F66
		// (set) Token: 0x06001150 RID: 4432 RVA: 0x00042D6E File Offset: 0x00040F6E
		public float fallbackDpi
		{
			get
			{
				return this.m_FallbackDpi;
			}
			set
			{
				this.m_FallbackDpi = ((value >= 1f) ? value : 96f);
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x00042D86 File Offset: 0x00040F86
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x00042D8E File Offset: 0x00040F8E
		public Vector2Int referenceResolution
		{
			get
			{
				return this.m_ReferenceResolution;
			}
			set
			{
				this.m_ReferenceResolution = value;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x00042D97 File Offset: 0x00040F97
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x00042D9F File Offset: 0x00040F9F
		public PanelScreenMatchMode screenMatchMode
		{
			get
			{
				return this.m_ScreenMatchMode;
			}
			set
			{
				this.m_ScreenMatchMode = value;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x00042DA8 File Offset: 0x00040FA8
		// (set) Token: 0x06001156 RID: 4438 RVA: 0x00042DB0 File Offset: 0x00040FB0
		public float match
		{
			get
			{
				return this.m_Match;
			}
			set
			{
				this.m_Match = value;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x00042DB9 File Offset: 0x00040FB9
		// (set) Token: 0x06001158 RID: 4440 RVA: 0x00042DC1 File Offset: 0x00040FC1
		public float sortingOrder
		{
			get
			{
				return this.m_SortingOrder;
			}
			set
			{
				this.m_SortingOrder = value;
				this.ApplySortingOrder();
			}
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00042DD2 File Offset: 0x00040FD2
		internal void ApplySortingOrder()
		{
			this.m_PanelAccess.SetSortingPriority();
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x00042DE1 File Offset: 0x00040FE1
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x00042DE9 File Offset: 0x00040FE9
		public int targetDisplay
		{
			get
			{
				return this.m_TargetDisplay;
			}
			set
			{
				this.m_TargetDisplay = value;
				this.m_PanelAccess.SetTargetDisplay();
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00042DFF File Offset: 0x00040FFF
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x00042E07 File Offset: 0x00041007
		public bool clearDepthStencil
		{
			get
			{
				return this.m_ClearDepthStencil;
			}
			set
			{
				this.m_ClearDepthStencil = value;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00042E10 File Offset: 0x00041010
		public float depthClearValue
		{
			get
			{
				return 0.99f;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x00042E17 File Offset: 0x00041017
		// (set) Token: 0x06001160 RID: 4448 RVA: 0x00042E1F File Offset: 0x0004101F
		public bool clearColor
		{
			get
			{
				return this.m_ClearColor;
			}
			set
			{
				this.m_ClearColor = value;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x00042E28 File Offset: 0x00041028
		// (set) Token: 0x06001162 RID: 4450 RVA: 0x00042E30 File Offset: 0x00041030
		public Color colorClearValue
		{
			get
			{
				return this.m_ColorClearValue;
			}
			set
			{
				this.m_ColorClearValue = value;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x00042E39 File Offset: 0x00041039
		internal BaseRuntimePanel panel
		{
			get
			{
				return this.m_PanelAccess.panel;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x00042E46 File Offset: 0x00041046
		internal VisualElement visualTree
		{
			get
			{
				return this.m_PanelAccess.panel.visualTree;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00042E58 File Offset: 0x00041058
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x00042E60 File Offset: 0x00041060
		public DynamicAtlasSettings dynamicAtlasSettings
		{
			get
			{
				return this.m_DynamicAtlasSettings;
			}
			set
			{
				this.m_DynamicAtlasSettings = value;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00042E6C File Offset: 0x0004106C
		private PanelSettings()
		{
			this.m_PanelAccess = new PanelSettings.RuntimePanelAccess(this);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x000020E6 File Offset: 0x000002E6
		private void Reset()
		{
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00042F14 File Offset: 0x00041114
		private void OnEnable()
		{
			bool flag = this.themeUss == null;
			if (flag)
			{
				Debug.LogWarning("No Theme Style Sheet set to PanelSettings " + base.name + ", UI will not render properly", this);
			}
			this.UpdateScreenDPI();
			this.InitializeShaders();
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00042F60 File Offset: 0x00041160
		private void OnDisable()
		{
			this.m_PanelAccess.DisposePanel();
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00042F60 File Offset: 0x00041160
		internal void DisposePanel()
		{
			this.m_PanelAccess.DisposePanel();
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x00042F6F File Offset: 0x0004116F
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x00042F77 File Offset: 0x00041177
		private float ScreenDPI { get; set; }

		// Token: 0x0600116E RID: 4462 RVA: 0x00042F80 File Offset: 0x00041180
		internal void UpdateScreenDPI()
		{
			this.ScreenDPI = Screen.dpi;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00042F90 File Offset: 0x00041190
		private void ApplyThemeStyleSheet(VisualElement root = null)
		{
			bool flag = !this.m_PanelAccess.isInitialized;
			if (!flag)
			{
				bool flag2 = root == null;
				if (flag2)
				{
					root = this.visualTree;
				}
				bool flag3 = this.m_OldThemeUss != this.themeUss && this.m_OldThemeUss != null;
				if (flag3)
				{
					if (root != null)
					{
						root.styleSheets.Remove(this.m_OldThemeUss);
					}
				}
				bool flag4 = this.themeUss != null;
				if (flag4)
				{
					this.themeUss.isDefaultStyleSheet = true;
					if (root != null)
					{
						root.styleSheets.Add(this.themeUss);
					}
				}
				this.m_OldThemeUss = this.themeUss;
			}
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00043054 File Offset: 0x00041254
		private void InitializeShaders()
		{
			bool flag = this.m_AtlasBlitShader == null;
			if (flag)
			{
				this.m_AtlasBlitShader = Shader.Find(Shaders.k_AtlasBlit);
			}
			bool flag2 = this.m_RuntimeShader == null;
			if (flag2)
			{
				this.m_RuntimeShader = Shader.Find(Shaders.k_Runtime);
			}
			bool flag3 = this.m_RuntimeWorldShader == null;
			if (flag3)
			{
				this.m_RuntimeWorldShader = Shader.Find(Shaders.k_RuntimeWorld);
			}
			this.m_PanelAccess.SetTargetTexture();
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000430D4 File Offset: 0x000412D4
		internal void ApplyPanelSettings()
		{
			Rect targetRect = this.m_TargetRect;
			float resolvedScale = this.m_ResolvedScale;
			this.UpdateScreenDPI();
			this.m_TargetRect = this.GetDisplayRect();
			this.m_ResolvedScale = this.ResolveScale(this.m_TargetRect, this.ScreenDPI);
			bool flag = this.visualTree.style.width.value == 0f || this.m_ResolvedScale != resolvedScale || this.m_TargetRect.width != targetRect.width || this.m_TargetRect.height != targetRect.height;
			if (flag)
			{
				this.panel.scale = ((this.m_ResolvedScale == 0f) ? 0f : (1f / this.m_ResolvedScale));
				this.visualTree.style.left = 0f;
				this.visualTree.style.top = 0f;
				this.visualTree.style.width = this.m_TargetRect.width * this.m_ResolvedScale;
				this.visualTree.style.height = this.m_TargetRect.height * this.m_ResolvedScale;
			}
			this.panel.targetTexture = this.targetTexture;
			this.panel.targetDisplay = this.targetDisplay;
			this.panel.drawToCameras = false;
			this.panel.clearSettings = new PanelClearSettings
			{
				clearColor = this.m_ClearColor,
				clearDepthStencil = this.m_ClearDepthStencil,
				color = this.m_ColorClearValue
			};
			DynamicAtlas dynamicAtlas = this.panel.atlas as DynamicAtlas;
			bool flag2 = dynamicAtlas != null;
			if (flag2)
			{
				dynamicAtlas.minAtlasSize = this.dynamicAtlasSettings.minAtlasSize;
				dynamicAtlas.maxAtlasSize = this.dynamicAtlasSettings.maxAtlasSize;
				dynamicAtlas.maxSubTextureSize = this.dynamicAtlasSettings.maxSubTextureSize;
				dynamicAtlas.activeFilters = this.dynamicAtlasSettings.activeFilters;
				dynamicAtlas.customFilter = this.dynamicAtlasSettings.customFilter;
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0004331B File Offset: 0x0004151B
		public void SetScreenToPanelSpaceFunction(Func<Vector2, Vector2> screentoPanelSpaceFunction)
		{
			this.m_AssignedScreenToPanel = screentoPanelSpaceFunction;
			this.panel.screenToPanelSpace = this.m_AssignedScreenToPanel;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00043338 File Offset: 0x00041538
		internal float ResolveScale(Rect targetRect, float screenDpi)
		{
			float num = 1f;
			switch (this.scaleMode)
			{
			case PanelScaleMode.ConstantPhysicalSize:
			{
				float num2 = ((screenDpi == 0f) ? this.fallbackDpi : screenDpi);
				bool flag = num2 != 0f;
				if (flag)
				{
					num = this.referenceDpi / num2;
				}
				break;
			}
			case PanelScaleMode.ScaleWithScreenSize:
			{
				bool flag2 = this.referenceResolution.x * this.referenceResolution.y != 0;
				if (flag2)
				{
					Vector2 vector = this.referenceResolution;
					Vector2 vector2 = new Vector2(targetRect.width / vector.x, targetRect.height / vector.y);
					PanelScreenMatchMode screenMatchMode = this.screenMatchMode;
					PanelScreenMatchMode panelScreenMatchMode = screenMatchMode;
					float num4;
					if (panelScreenMatchMode != PanelScreenMatchMode.Shrink)
					{
						if (panelScreenMatchMode != PanelScreenMatchMode.Expand)
						{
							float num3 = Mathf.Clamp01(this.match);
							num4 = Mathf.Lerp(vector2.x, vector2.y, num3);
						}
						else
						{
							num4 = Mathf.Min(vector2.x, vector2.y);
						}
					}
					else
					{
						num4 = Mathf.Max(vector2.x, vector2.y);
					}
					bool flag3 = num4 != 0f;
					if (flag3)
					{
						num = 1f / num4;
					}
				}
				break;
			}
			}
			bool flag4 = this.scale > 0f;
			if (flag4)
			{
				num /= this.scale;
			}
			else
			{
				num = 0f;
			}
			return num;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x000434C4 File Offset: 0x000416C4
		internal Rect GetDisplayRect()
		{
			bool flag = this.m_TargetTexture != null;
			Rect rect;
			if (flag)
			{
				rect = new Rect(0f, 0f, (float)this.m_TargetTexture.width, (float)this.m_TargetTexture.height);
			}
			else
			{
				bool flag2 = this.targetDisplay > 0 && this.targetDisplay < Display.displays.Length;
				if (flag2)
				{
					rect = new Rect(0f, 0f, (float)Display.displays[this.targetDisplay].renderingWidth, (float)Display.displays[this.targetDisplay].renderingHeight);
				}
				else
				{
					rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
				}
			}
			return rect;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00043584 File Offset: 0x00041784
		internal void AttachAndInsertUIDocumentToVisualTree(UIDocument uiDocument)
		{
			bool flag = this.m_AttachedUIDocumentsList == null;
			if (flag)
			{
				this.m_AttachedUIDocumentsList = new UIDocumentList();
			}
			else
			{
				this.m_AttachedUIDocumentsList.RemoveFromListAndFromVisualTree(uiDocument);
			}
			this.m_AttachedUIDocumentsList.AddToListAndToVisualTree(uiDocument, this.visualTree, 0);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x000435D4 File Offset: 0x000417D4
		internal void DetachUIDocument(UIDocument uiDocument)
		{
			bool flag = this.m_AttachedUIDocumentsList == null;
			if (!flag)
			{
				this.m_AttachedUIDocumentsList.RemoveFromListAndFromVisualTree(uiDocument);
				bool flag2 = this.m_AttachedUIDocumentsList.m_AttachedUIDocuments.Count == 0;
				if (flag2)
				{
					this.m_PanelAccess.MarkPotentiallyEmpty();
				}
			}
		}

		// Token: 0x040007A4 RID: 1956
		private const int k_DefaultSortingOrder = 0;

		// Token: 0x040007A5 RID: 1957
		private const float k_DefaultScaleValue = 1f;

		// Token: 0x040007A6 RID: 1958
		internal const string k_DefaultStyleSheetPath = "Packages/com.unity.ui/PackageResources/StyleSheets/Generated/Default.tss.asset";

		// Token: 0x040007A7 RID: 1959
		[SerializeField]
		private ThemeStyleSheet themeUss;

		// Token: 0x040007A8 RID: 1960
		[SerializeField]
		private RenderTexture m_TargetTexture;

		// Token: 0x040007A9 RID: 1961
		[SerializeField]
		private PanelScaleMode m_ScaleMode = PanelScaleMode.ConstantPhysicalSize;

		// Token: 0x040007AA RID: 1962
		[SerializeField]
		private float m_Scale = 1f;

		// Token: 0x040007AB RID: 1963
		private const float DefaultDpi = 96f;

		// Token: 0x040007AC RID: 1964
		[SerializeField]
		private float m_ReferenceDpi = 96f;

		// Token: 0x040007AD RID: 1965
		[SerializeField]
		private float m_FallbackDpi = 96f;

		// Token: 0x040007AE RID: 1966
		[SerializeField]
		private Vector2Int m_ReferenceResolution = new Vector2Int(1200, 800);

		// Token: 0x040007AF RID: 1967
		[SerializeField]
		private PanelScreenMatchMode m_ScreenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;

		// Token: 0x040007B0 RID: 1968
		[Range(0f, 1f)]
		[SerializeField]
		private float m_Match = 0f;

		// Token: 0x040007B1 RID: 1969
		[SerializeField]
		private float m_SortingOrder = 0f;

		// Token: 0x040007B2 RID: 1970
		[SerializeField]
		private int m_TargetDisplay = 0;

		// Token: 0x040007B3 RID: 1971
		[SerializeField]
		private bool m_ClearDepthStencil = true;

		// Token: 0x040007B4 RID: 1972
		[SerializeField]
		private bool m_ClearColor;

		// Token: 0x040007B5 RID: 1973
		[SerializeField]
		private Color m_ColorClearValue = Color.clear;

		// Token: 0x040007B6 RID: 1974
		private PanelSettings.RuntimePanelAccess m_PanelAccess;

		// Token: 0x040007B7 RID: 1975
		internal UIDocumentList m_AttachedUIDocumentsList;

		// Token: 0x040007B8 RID: 1976
		[HideInInspector]
		[SerializeField]
		private DynamicAtlasSettings m_DynamicAtlasSettings = DynamicAtlasSettings.defaults;

		// Token: 0x040007B9 RID: 1977
		[SerializeField]
		[HideInInspector]
		private Shader m_AtlasBlitShader;

		// Token: 0x040007BA RID: 1978
		[SerializeField]
		[HideInInspector]
		private Shader m_RuntimeShader;

		// Token: 0x040007BB RID: 1979
		[HideInInspector]
		[SerializeField]
		private Shader m_RuntimeWorldShader;

		// Token: 0x040007BC RID: 1980
		[SerializeField]
		public PanelTextSettings textSettings;

		// Token: 0x040007BD RID: 1981
		private Rect m_TargetRect;

		// Token: 0x040007BE RID: 1982
		private float m_ResolvedScale;

		// Token: 0x040007BF RID: 1983
		private StyleSheet m_OldThemeUss;

		// Token: 0x040007C0 RID: 1984
		internal int m_EmptyPanelCounter = 0;

		// Token: 0x040007C2 RID: 1986
		private Func<Vector2, Vector2> m_AssignedScreenToPanel;

		// Token: 0x02000246 RID: 582
		private class RuntimePanelAccess
		{
			// Token: 0x06001177 RID: 4471 RVA: 0x00043622 File Offset: 0x00041822
			internal RuntimePanelAccess(PanelSettings settings)
			{
				this.m_Settings = settings;
			}

			// Token: 0x170003ED RID: 1005
			// (get) Token: 0x06001178 RID: 4472 RVA: 0x00043633 File Offset: 0x00041833
			internal bool isInitialized
			{
				get
				{
					return this.m_RuntimePanel != null;
				}
			}

			// Token: 0x170003EE RID: 1006
			// (get) Token: 0x06001179 RID: 4473 RVA: 0x00043640 File Offset: 0x00041840
			internal BaseRuntimePanel panel
			{
				get
				{
					bool flag = this.m_RuntimePanel == null;
					if (flag)
					{
						this.m_RuntimePanel = this.CreateRelatedRuntimePanel();
						this.m_RuntimePanel.sortingPriority = this.m_Settings.m_SortingOrder;
						this.m_RuntimePanel.targetDisplay = this.m_Settings.m_TargetDisplay;
						VisualElement visualTree = this.m_RuntimePanel.visualTree;
						visualTree.name = this.m_Settings.name;
						this.m_Settings.ApplyThemeStyleSheet(visualTree);
						bool flag2 = this.m_Settings.m_TargetTexture != null;
						if (flag2)
						{
							this.m_RuntimePanel.targetTexture = this.m_Settings.m_TargetTexture;
						}
						bool flag3 = this.m_Settings.m_AssignedScreenToPanel != null;
						if (flag3)
						{
							this.m_Settings.SetScreenToPanelSpaceFunction(this.m_Settings.m_AssignedScreenToPanel);
						}
					}
					return this.m_RuntimePanel;
				}
			}

			// Token: 0x0600117A RID: 4474 RVA: 0x0004372C File Offset: 0x0004192C
			internal void DisposePanel()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.DisposeRelatedPanel();
					this.m_RuntimePanel = null;
				}
			}

			// Token: 0x0600117B RID: 4475 RVA: 0x00043758 File Offset: 0x00041958
			internal void SetTargetTexture()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.m_RuntimePanel.targetTexture = this.m_Settings.targetTexture;
				}
			}

			// Token: 0x0600117C RID: 4476 RVA: 0x0004378C File Offset: 0x0004198C
			internal void SetSortingPriority()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.m_RuntimePanel.sortingPriority = this.m_Settings.m_SortingOrder;
				}
			}

			// Token: 0x0600117D RID: 4477 RVA: 0x000437C0 File Offset: 0x000419C0
			internal void SetTargetDisplay()
			{
				bool flag = this.m_RuntimePanel != null;
				if (flag)
				{
					this.m_RuntimePanel.targetDisplay = this.m_Settings.m_TargetDisplay;
				}
			}

			// Token: 0x0600117E RID: 4478 RVA: 0x000437F4 File Offset: 0x000419F4
			private BaseRuntimePanel CreateRelatedRuntimePanel()
			{
				return (RuntimePanel)UIElementsRuntimeUtility.FindOrCreateRuntimePanel(this.m_Settings, new UIElementsRuntimeUtility.CreateRuntimePanelDelegate(RuntimePanel.Create));
			}

			// Token: 0x0600117F RID: 4479 RVA: 0x00043824 File Offset: 0x00041A24
			private void DisposeRelatedPanel()
			{
				UIElementsRuntimeUtility.DisposeRuntimePanel(this.m_Settings);
			}

			// Token: 0x06001180 RID: 4480 RVA: 0x00043833 File Offset: 0x00041A33
			internal void MarkPotentiallyEmpty()
			{
				UIElementsRuntimeUtility.MarkPotentiallyEmpty(this.m_Settings);
			}

			// Token: 0x040007C3 RID: 1987
			private readonly PanelSettings m_Settings;

			// Token: 0x040007C4 RID: 1988
			private BaseRuntimePanel m_RuntimePanel;
		}
	}
}
