using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D
{
	// Token: 0x02000271 RID: 625
	[MovedFrom("UnityEngine.Experimental.U2D")]
	[NativeHeader("Runtime/2D/Common/SpriteDataMarshalling.h")]
	[NativeType(CodegenOptions.Custom, "ScriptingSpriteBone")]
	[NativeHeader("Runtime/2D/Common/SpriteDataAccess.h")]
	[RequiredByNativeCode]
	[Serializable]
	public struct SpriteBone
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0002B6EC File Offset: 0x000298EC
		// (set) Token: 0x06001B16 RID: 6934 RVA: 0x0002B704 File Offset: 0x00029904
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0002B710 File Offset: 0x00029910
		// (set) Token: 0x06001B18 RID: 6936 RVA: 0x0002B728 File Offset: 0x00029928
		public string guid
		{
			get
			{
				return this.m_Guid;
			}
			set
			{
				this.m_Guid = value;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0002B734 File Offset: 0x00029934
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x0002B74C File Offset: 0x0002994C
		public Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x0002B758 File Offset: 0x00029958
		// (set) Token: 0x06001B1C RID: 6940 RVA: 0x0002B770 File Offset: 0x00029970
		public Quaternion rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				this.m_Rotation = value;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x0002B77C File Offset: 0x0002997C
		// (set) Token: 0x06001B1E RID: 6942 RVA: 0x0002B794 File Offset: 0x00029994
		public float length
		{
			get
			{
				return this.m_Length;
			}
			set
			{
				this.m_Length = value;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x0002B7A0 File Offset: 0x000299A0
		// (set) Token: 0x06001B20 RID: 6944 RVA: 0x0002B7B8 File Offset: 0x000299B8
		public int parentId
		{
			get
			{
				return this.m_ParentId;
			}
			set
			{
				this.m_ParentId = value;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x0002B7C4 File Offset: 0x000299C4
		// (set) Token: 0x06001B22 RID: 6946 RVA: 0x0002B7DC File Offset: 0x000299DC
		public Color32 color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
			}
		}

		// Token: 0x040008EC RID: 2284
		[SerializeField]
		[NativeName("name")]
		private string m_Name;

		// Token: 0x040008ED RID: 2285
		[NativeName("guid")]
		[SerializeField]
		private string m_Guid;

		// Token: 0x040008EE RID: 2286
		[NativeName("position")]
		[SerializeField]
		private Vector3 m_Position;

		// Token: 0x040008EF RID: 2287
		[SerializeField]
		[NativeName("rotation")]
		private Quaternion m_Rotation;

		// Token: 0x040008F0 RID: 2288
		[SerializeField]
		[NativeName("length")]
		private float m_Length;

		// Token: 0x040008F1 RID: 2289
		[SerializeField]
		[NativeName("parentId")]
		private int m_ParentId;

		// Token: 0x040008F2 RID: 2290
		[SerializeField]
		[NativeName("color")]
		private Color32 m_Color;
	}
}
