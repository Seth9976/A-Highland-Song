using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002C RID: 44
	public class PointPath<T> : DisposableMesh
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x00022BFE File Offset: 0x00020DFE
		public int Count
		{
			get
			{
				return this.path.Count;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00022C0B File Offset: 0x00020E0B
		public T LastPoint
		{
			get
			{
				return this.path[this.path.Count - 1];
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00022C25 File Offset: 0x00020E25
		protected void OnSetFirstDataPoint()
		{
			this.hasSetFirstPoint = true;
			base.EnsureMeshExists();
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00022C34 File Offset: 0x00020E34
		public void ClearAllPoints()
		{
			this.path.Clear();
			this.hasSetFirstPoint = false;
		}

		// Token: 0x1700012D RID: 301
		public T this[int i]
		{
			get
			{
				return this.path[i];
			}
			set
			{
				this.path[i] = value;
				this.meshDirty = true;
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00022C6C File Offset: 0x00020E6C
		public void SetPoint(int index, T point)
		{
			this.path[index] = point;
			this.meshDirty = true;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00022C82 File Offset: 0x00020E82
		public void AddPoint(T p)
		{
			if (!this.hasSetFirstPoint)
			{
				this.OnSetFirstDataPoint();
			}
			this.path.Add(p);
			this.meshDirty = true;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00022CA5 File Offset: 0x00020EA5
		public void AddPoints(params T[] pts)
		{
			this.AddPoints(pts);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00022CB0 File Offset: 0x00020EB0
		public void AddPoints(IEnumerable<T> ptsToAdd)
		{
			int count = this.path.Count;
			this.path.AddRange(ptsToAdd);
			if (this.path.Count - count > 0 && !this.hasSetFirstPoint)
			{
				this.OnSetFirstDataPoint();
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00022CF3 File Offset: 0x00020EF3
		protected bool CheckCanAddContinuePoint([CallerMemberName] string callerName = null)
		{
			if (!this.hasSetFirstPoint)
			{
				Debug.LogWarning(callerName + " requires adding a point before calling it, to determine starting point");
				return true;
			}
			return false;
		}

		// Token: 0x04000134 RID: 308
		protected List<T> path = new List<T>();

		// Token: 0x04000135 RID: 309
		protected bool hasSetFirstPoint;
	}
}
