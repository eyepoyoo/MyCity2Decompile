using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity.UI
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public class Image
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000026C8 File Offset: 0x000008C8
		public Image(Texture2D texture, Image.RetinaFactor retinaFactor, RectOffset border, RectOffset overflow)
		{
			this.texture = texture;
			this.retina = retinaFactor;
			this.border = border;
			this.overflow = overflow;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000026F0 File Offset: 0x000008F0
		public int Height
		{
			get
			{
				switch (this.retina)
				{
				case Image.RetinaFactor.One:
					return this.Owner.ScaleFactor * this.texture.height;
				case Image.RetinaFactor.Two:
					return this.Owner.ScaleFactor * this.texture.height / 2;
				case Image.RetinaFactor.Three:
					return this.Owner.ScaleFactor * this.texture.height / 3;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000276C File Offset: 0x0000096C
		public int Width
		{
			get
			{
				switch (this.retina)
				{
				case Image.RetinaFactor.One:
					return this.Owner.ScaleFactor * this.texture.width;
				case Image.RetinaFactor.Two:
					return this.Owner.ScaleFactor * this.texture.width / 2;
				case Image.RetinaFactor.Three:
					return this.Owner.ScaleFactor * this.texture.width / 3;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027E8 File Offset: 0x000009E8
		public static implicit operator Texture2D(Image i)
		{
			return i.texture;
		}

		// Token: 0x0400001B RID: 27
		public Texture2D texture;

		// Token: 0x0400001C RID: 28
		public Image.RetinaFactor retina;

		// Token: 0x0400001D RID: 29
		public RectOffset border;

		// Token: 0x0400001E RID: 30
		public RectOffset overflow;

		// Token: 0x0400001F RID: 31
		public UnityGUIComponent Owner;

		// Token: 0x0200000F RID: 15
		public enum RetinaFactor
		{
			// Token: 0x04000021 RID: 33
			One,
			// Token: 0x04000022 RID: 34
			Two,
			// Token: 0x04000023 RID: 35
			Three
		}
	}
}
