using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000051 RID: 81
	[Serializable]
	public class PostProcessEffectSettings : ScriptableObject
	{
		// Token: 0x06000113 RID: 275 RVA: 0x0000A4F4 File Offset: 0x000086F4
		private void OnEnable()
		{
			this.parameters = (from t in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
				where t.FieldType.IsSubclassOf(typeof(ParameterOverride))
				orderby t.MetadataToken
				select (ParameterOverride)t.GetValue(this)).ToList<ParameterOverride>().AsReadOnly();
			foreach (ParameterOverride parameterOverride in this.parameters)
			{
				parameterOverride.OnEnable();
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000A5B4 File Offset: 0x000087B4
		private void OnDisable()
		{
			if (this.parameters == null)
			{
				return;
			}
			foreach (ParameterOverride parameterOverride in this.parameters)
			{
				parameterOverride.OnDisable();
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000A608 File Offset: 0x00008808
		public void SetAllOverridesTo(bool state, bool excludeEnabled = true)
		{
			foreach (ParameterOverride parameterOverride in this.parameters)
			{
				if (!excludeEnabled || parameterOverride != this.enabled)
				{
					parameterOverride.overrideState = state;
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000A664 File Offset: 0x00008864
		public virtual bool IsEnabledAndSupported(PostProcessRenderContext context)
		{
			return this.enabled.value;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000A674 File Offset: 0x00008874
		public int GetHash()
		{
			int num = 17;
			foreach (ParameterOverride parameterOverride in this.parameters)
			{
				num = num * 23 + parameterOverride.GetHash();
			}
			return num;
		}

		// Token: 0x0400012C RID: 300
		public bool active = true;

		// Token: 0x0400012D RID: 301
		public BoolParameter enabled = new BoolParameter
		{
			overrideState = true,
			value = false
		};

		// Token: 0x0400012E RID: 302
		internal ReadOnlyCollection<ParameterOverride> parameters;
	}
}
