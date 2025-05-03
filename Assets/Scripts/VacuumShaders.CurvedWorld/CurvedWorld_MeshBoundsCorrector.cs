using System;
using System.Collections.Generic;
using UnityEngine;

namespace VacuumShaders.CurvedWorld
{
	// Token: 0x02000005 RID: 5
	[ExecuteInEditMode]
	[AddComponentMenu("VacuumShaders/Curved World/Mesh Bounds Corrector")]
	public class CurvedWorld_MeshBoundsCorrector : MonoBehaviour
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002A6F File Offset: 0x00000C6F
		private void OnEnable()
		{
			this.currentMeshBoundsScale = -1f;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A7C File Offset: 0x00000C7C
		private void Start()
		{
			if (CurvedWorld_MeshBoundsCorrector.boundsDictionary == null)
			{
				CurvedWorld_MeshBoundsCorrector.boundsDictionary = new Dictionary<int, Bounds>();
			}
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.skinnedMeshRenderer = base.GetComponent<SkinnedMeshRenderer>();
			if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				this.bIsSkinned = false;
				if (CurvedWorld_MeshBoundsCorrector.boundsDictionary.ContainsKey(this.meshFilter.sharedMesh.GetInstanceID()))
				{
					this.origBounds = CurvedWorld_MeshBoundsCorrector.boundsDictionary[this.meshFilter.sharedMesh.GetInstanceID()];
				}
				else
				{
					this.origBounds = this.meshFilter.sharedMesh.bounds;
					CurvedWorld_MeshBoundsCorrector.boundsDictionary.Add(this.meshFilter.sharedMesh.GetInstanceID(), this.origBounds);
				}
				this.boundsSize = this.origBounds.size;
				float num = 1f;
				if (this.boundsSize.x > num)
				{
					num = this.boundsSize.x;
				}
				if (this.boundsSize.y > num)
				{
					num = this.boundsSize.y;
				}
				if (this.boundsSize.z > num)
				{
					num = this.boundsSize.z;
				}
				this.boundsSize.x = (this.boundsSize.y = (this.boundsSize.z = num));
			}
			else if (this.skinnedMeshRenderer != null && this.skinnedMeshRenderer.sharedMesh != null)
			{
				this.bIsSkinned = true;
				if (CurvedWorld_MeshBoundsCorrector.boundsDictionary.ContainsKey(this.skinnedMeshRenderer.sharedMesh.GetInstanceID()))
				{
					this.origBounds = CurvedWorld_MeshBoundsCorrector.boundsDictionary[this.skinnedMeshRenderer.sharedMesh.GetInstanceID()];
				}
				else
				{
					this.origBounds = this.skinnedMeshRenderer.sharedMesh.bounds;
					CurvedWorld_MeshBoundsCorrector.boundsDictionary.Add(this.skinnedMeshRenderer.sharedMesh.GetInstanceID(), this.origBounds);
				}
				this.boundsSize = this.origBounds.size;
				float num2 = 1f;
				if (this.boundsSize.x > num2)
				{
					num2 = this.boundsSize.x;
				}
				if (this.boundsSize.y > num2)
				{
					num2 = this.boundsSize.y;
				}
				if (this.boundsSize.z > num2)
				{
					num2 = this.boundsSize.z;
				}
				this.boundsSize.x = (this.boundsSize.y = (this.boundsSize.z = num2));
			}
			else
			{
				Debug.LogWarning("CurvedWorld_MeshBoundsCorrector: " + base.gameObject.name + " has no mesh.", base.gameObject);
				base.enabled = false;
			}
			this.currentMeshBoundsScale = 0f;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002D54 File Offset: 0x00000F54
		private void Update()
		{
			if (this.currentMeshBoundsScale != this.meshBoundsScale)
			{
				if (this.meshBoundsScale < 0f)
				{
					this.meshBoundsScale = 0f;
				}
				this.currentMeshBoundsScale = this.meshBoundsScale;
				if (this.bIsSkinned)
				{
					if (this.skinnedMeshRenderer != null)
					{
						this.skinnedMeshRenderer.localBounds = new Bounds(this.skinnedMeshRenderer.localBounds.center, this.boundsSize * this.meshBoundsScale);
						return;
					}
				}
				else if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
				{
					this.meshFilter.sharedMesh.bounds = new Bounds(this.meshFilter.sharedMesh.bounds.center, this.boundsSize * this.meshBoundsScale);
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002E44 File Offset: 0x00001044
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			if (this.bIsSkinned && this.skinnedMeshRenderer != null && this.skinnedMeshRenderer.sharedMesh != null)
			{
				Gizmos.DrawWireCube(base.transform.TransformPoint(this.skinnedMeshRenderer.localBounds.center), this.boundsSize * this.meshBoundsScale);
				return;
			}
			if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				Gizmos.DrawWireCube(base.transform.TransformPoint(this.meshFilter.sharedMesh.bounds.center), this.boundsSize * this.meshBoundsScale);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002F14 File Offset: 0x00001114
		private void OnDisable()
		{
			if (this.bIsSkinned)
			{
				if (this.skinnedMeshRenderer != null)
				{
					this.skinnedMeshRenderer.sharedMesh.bounds = this.origBounds;
					return;
				}
			}
			else if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				this.meshFilter.sharedMesh.bounds = this.origBounds;
			}
		}

		// Token: 0x0400001E RID: 30
		public float meshBoundsScale = 1f;

		// Token: 0x0400001F RID: 31
		private float currentMeshBoundsScale;

		// Token: 0x04000020 RID: 32
		private Vector3 boundsSize;

		// Token: 0x04000021 RID: 33
		private Bounds origBounds;

		// Token: 0x04000022 RID: 34
		private SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x04000023 RID: 35
		private MeshFilter meshFilter;

		// Token: 0x04000024 RID: 36
		private bool bIsSkinned;

		// Token: 0x04000025 RID: 37
		private static Dictionary<int, Bounds> boundsDictionary;
	}
}
