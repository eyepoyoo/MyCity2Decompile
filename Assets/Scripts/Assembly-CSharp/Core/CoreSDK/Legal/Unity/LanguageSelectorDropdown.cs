using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Unity.UI;
using UnityEngine;

namespace LEGO.CoreSDK.Legal.Unity
{
	// Token: 0x02000047 RID: 71
	public class LanguageSelectorDropdown : UnityGUIComponent
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00004F50 File Offset: 0x00003150
		public void Setup(Rect rect, Vector2 buttonSize, List<PolicyBehaviour.SupportedPolicyLocale> regionImages, GUIStyle buttonStyle, GUIStyle scrollbarStyle, Action<PolicyBehaviour.SupportedPolicyLocale> completionHandler)
		{
			base.gameObject.AddComponent<FullScreenButton>().Setup(delegate
			{
				this.SafeDestroy();
			});
			this.ButtonSize = buttonSize;
			this.Rect = rect;
			this.RegionImages = regionImages;
			this.LanguageButtonStyle = buttonStyle;
			this.ScrollbarStyle = scrollbarStyle;
			this.CompletionHandler = completionHandler;
			this.RegionImagesContent = new List<GUIContent>();
			this.RegionImagesRect = new List<Rect>();
			int num = 0;
			foreach (PolicyBehaviour.SupportedPolicyLocale supportedPolicyLocale in this.RegionImages)
			{
				GUIContent guicontent = new GUIContent(" " + supportedPolicyLocale.DisplayText, supportedPolicyLocale.Flag);
				this.RegionImagesContent.Add(guicontent);
				this.RegionImagesRect.Add(new Rect(0f, (float)num, this.ButtonSize.x, this.ButtonSize.y));
				num += (int)this.ButtonSize.y;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000507C File Offset: 0x0000327C
		protected override void OnGUI()
		{
			GUI.depth = -3;
			float num = this.ButtonSize.x + this.ScrollbarStyle.fixedWidth;
			Rect rect = new Rect(this.Rect.x + this.Rect.width - this.ButtonSize.x, this.Rect.y, num, this.Rect.height);
			if (rect.x < 0f)
			{
				rect.x = this.Rect.x;
			}
			rect = this.RectIntegral(rect);
			Rect rect2 = new Rect(rect.x - 10f, rect.y, rect.width + 20f, rect.height);
			GUI.DrawTexture(rect2, Texture2D.whiteTexture);
			this.LanguageSelectorScrollPosition = GUI.BeginScrollView(rect, this.LanguageSelectorScrollPosition, new Rect(0f, 0f, this.ButtonSize.x, (float)this.RegionImages.Count<PolicyBehaviour.SupportedPolicyLocale>() * this.ButtonSize.y));
			for (int i = 0; i < this.RegionImagesContent.Count; i++)
			{
				if (GUI.Button(this.RegionImagesRect[i], this.RegionImagesContent[i], this.LanguageButtonStyle))
				{
					PolicyBehaviour.SupportedPolicyLocale supportedPolicyLocale = this.RegionImages.ToList<PolicyBehaviour.SupportedPolicyLocale>()[i];
					this.CompletionHandler(supportedPolicyLocale);
					this.SafeDestroy();
				}
			}
			GUI.EndScrollView();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005204 File Offset: 0x00003404
		private void Update()
		{
			if (Input.touchCount > 0)
			{
				this.LatestTouch = Input.touches[0];
				if (this.LatestTouch.phase == TouchPhase.Moved)
				{
					this.LanguageSelectorScrollPosition.y = this.LanguageSelectorScrollPosition.y + this.LatestTouch.deltaPosition.y;
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005268 File Offset: 0x00003468
		private Rect RectIntegral(Rect rect)
		{
			return new Rect((float)Mathf.RoundToInt(rect.x), (float)Mathf.RoundToInt(rect.y), (float)Mathf.RoundToInt(rect.width), (float)Mathf.RoundToInt(rect.height));
		}

		// Token: 0x0400008C RID: 140
		private Rect Rect;

		// Token: 0x0400008D RID: 141
		private List<PolicyBehaviour.SupportedPolicyLocale> RegionImages;

		// Token: 0x0400008E RID: 142
		private List<GUIContent> RegionImagesContent;

		// Token: 0x0400008F RID: 143
		private List<Rect> RegionImagesRect;

		// Token: 0x04000090 RID: 144
		private GUIStyle LanguageButtonStyle;

		// Token: 0x04000091 RID: 145
		private GUIStyle ScrollbarStyle;

		// Token: 0x04000092 RID: 146
		private Vector2 LanguageSelectorScrollPosition;

		// Token: 0x04000093 RID: 147
		private Action<PolicyBehaviour.SupportedPolicyLocale> CompletionHandler;

		// Token: 0x04000094 RID: 148
		private Vector2 ButtonSize;

		// Token: 0x04000095 RID: 149
		private Touch LatestTouch;
	}
}
