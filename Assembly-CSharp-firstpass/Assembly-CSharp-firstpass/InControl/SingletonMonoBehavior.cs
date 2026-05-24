using System;
using System.Linq;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000079 RID: 121
	public abstract class SingletonMonoBehavior<TComponent> : MonoBehaviour where TComponent : MonoBehaviour
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00013E20 File Offset: 0x00012020
		public static TComponent Instance
		{
			get
			{
				object obj = SingletonMonoBehavior<TComponent>.lockObject;
				TComponent tcomponent;
				lock (obj)
				{
					if (SingletonMonoBehavior<TComponent>.hasInstance)
					{
						tcomponent = SingletonMonoBehavior<TComponent>.instance;
					}
					else
					{
						SingletonMonoBehavior<TComponent>.instance = SingletonMonoBehavior<TComponent>.FindFirstInstance();
						if (SingletonMonoBehavior<TComponent>.instance == null)
						{
							string text = "The instance of singleton component ";
							Type typeFromHandle = typeof(TComponent);
							throw new Exception(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " was requested, but it doesn't appear to exist in the scene.");
						}
						SingletonMonoBehavior<TComponent>.hasInstance = true;
						SingletonMonoBehavior<TComponent>.instanceId = SingletonMonoBehavior<TComponent>.instance.GetInstanceID();
						tcomponent = SingletonMonoBehavior<TComponent>.instance;
					}
				}
				return tcomponent;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00013ED0 File Offset: 0x000120D0
		protected bool EnforceSingleton
		{
			get
			{
				if (base.GetInstanceID() == SingletonMonoBehavior<TComponent>.Instance.GetInstanceID())
				{
					return false;
				}
				if (Application.isPlaying)
				{
					base.enabled = false;
				}
				return true;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00013EFC File Offset: 0x000120FC
		protected bool IsTheSingleton
		{
			get
			{
				object obj = SingletonMonoBehavior<TComponent>.lockObject;
				bool flag2;
				lock (obj)
				{
					flag2 = base.GetInstanceID() == SingletonMonoBehavior<TComponent>.instanceId;
				}
				return flag2;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00013F44 File Offset: 0x00012144
		protected bool IsNotTheSingleton
		{
			get
			{
				object obj = SingletonMonoBehavior<TComponent>.lockObject;
				bool flag2;
				lock (obj)
				{
					flag2 = base.GetInstanceID() != SingletonMonoBehavior<TComponent>.instanceId;
				}
				return flag2;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00013F90 File Offset: 0x00012190
		private static TComponent[] FindInstances()
		{
			TComponent[] array = Object.FindObjectsOfType<TComponent>();
			Array.Sort<TComponent>(array, (TComponent a, TComponent b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
			return array;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00013FBC File Offset: 0x000121BC
		private static TComponent FindFirstInstance()
		{
			TComponent[] array = SingletonMonoBehavior<TComponent>.FindInstances();
			if (array.Length == 0)
			{
				return default(TComponent);
			}
			return array[0];
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00013FE4 File Offset: 0x000121E4
		protected virtual void Awake()
		{
			if (Application.isPlaying && SingletonMonoBehavior<TComponent>.Instance)
			{
				if (base.GetInstanceID() != SingletonMonoBehavior<TComponent>.instanceId)
				{
					base.enabled = false;
				}
				foreach (TComponent tcomponent in from o in SingletonMonoBehavior<TComponent>.FindInstances()
					where o.GetInstanceID() != SingletonMonoBehavior<TComponent>.instanceId
					select o)
				{
					tcomponent.enabled = false;
				}
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00014084 File Offset: 0x00012284
		protected virtual void OnDestroy()
		{
			object obj = SingletonMonoBehavior<TComponent>.lockObject;
			lock (obj)
			{
				if (base.GetInstanceID() == SingletonMonoBehavior<TComponent>.instanceId)
				{
					SingletonMonoBehavior<TComponent>.hasInstance = false;
				}
			}
		}

		// Token: 0x0400047C RID: 1148
		private static TComponent instance;

		// Token: 0x0400047D RID: 1149
		private static bool hasInstance;

		// Token: 0x0400047E RID: 1150
		private static int instanceId;

		// Token: 0x0400047F RID: 1151
		private static readonly object lockObject = new object();
	}
}
