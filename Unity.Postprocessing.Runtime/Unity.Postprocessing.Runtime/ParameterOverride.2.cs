using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class ParameterOverride<T> : ParameterOverride
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000094E0 File Offset: 0x000076E0
		public ParameterOverride()
			: this(default(T), false)
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000094FD File Offset: 0x000076FD
		public ParameterOverride(T value)
			: this(value, false)
		{
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00009507 File Offset: 0x00007707
		public ParameterOverride(T value, bool overrideState)
		{
			this.value = value;
			this.overrideState = overrideState;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000951D File Offset: 0x0000771D
		internal override void Interp(ParameterOverride from, ParameterOverride to, float t)
		{
			this.Interp(from.GetValue<T>(), to.GetValue<T>(), t);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00009532 File Offset: 0x00007732
		public virtual void Interp(T from, T to, float t)
		{
			this.value = ((t > 0f) ? to : from);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00009546 File Offset: 0x00007746
		public void Override(T x)
		{
			this.overrideState = true;
			this.value = x;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00009556 File Offset: 0x00007756
		internal override void SetValue(ParameterOverride parameter)
		{
			this.value = parameter.GetValue<T>();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00009564 File Offset: 0x00007764
		public override int GetHash()
		{
			return (17 * 23 + this.overrideState.GetHashCode()) * 23 + this.value.GetHashCode();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000958C File Offset: 0x0000778C
		public static implicit operator T(ParameterOverride<T> prop)
		{
			return prop.value;
		}

		// Token: 0x040000FB RID: 251
		public T value;
	}
}
