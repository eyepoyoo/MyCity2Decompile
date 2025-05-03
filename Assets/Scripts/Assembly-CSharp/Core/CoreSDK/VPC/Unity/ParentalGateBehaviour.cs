using System;
using System.Collections.Generic;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Unity.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LEGO.CoreSDK.VPC.Unity
{
	// Token: 0x02000006 RID: 6
	public class ParentalGateBehaviour : LEGOBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002320 File Offset: 0x00000520
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002328 File Offset: 0x00000528
		public ParentalGateModel? ParentalGate { get; set; }

		// Token: 0x0600000A RID: 10 RVA: 0x00002334 File Offset: 0x00000534
		protected override void Start()
		{
			base.Start();
			if (this.ParentalGate == null)
			{
				LEGOSDK.Log.Error("Parental Gate Component initialised without a ParentalGate. Challenge failed.");
				base.gameObject.SafeDestroy();
			}
			else
			{
				ParentalGateModel value = this.ParentalGate.Value;
				this.QuestionField.text = string.Concat(new object[] { value.LeftHandSide, " ", value.Sign, " ", value.RightHandSide, " = ___ ?" });
				List<int> list = value.PossibleAnswers(this.PossibleAnswerTexts.Length);
				for (int i = 0; i < this.PossibleAnswerTexts.Length; i++)
				{
					Text text = this.PossibleAnswerTexts[i];
					text.text = list[i].ToString();
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000242C File Offset: 0x0000062C
		private void Update()
		{
			this.MinifigureImage.gameObject.SetActive(this.ShouldMinifigureImageBeActive());
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002444 File Offset: 0x00000644
		private bool ShouldMinifigureImageBeActive()
		{
			float num = this.CanvasRectTransform.rect.height;
			num -= this.LogoImageRectTransform.rect.height + Math.Abs(this.LogoImageRectTransform.rect.y);
			num -= this.QuestionFieldRectTransform.rect.height + 20f;
			num -= this.AnswersPanelRectTransform.rect.height + this.AnswersPanelRectTransform.rect.y;
			return this.MinifigureImageRectTransform.rect.height < num;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024FC File Offset: 0x000006FC
		public void DidTapAnswerButton(Text text)
		{
			int num;
			if (int.TryParse(text.text, out num))
			{
				this.ParentalGate.Value.Validate(num);
			}
			else
			{
				LEGOSDK.Log.Warning("Failed to parse: '" + text.text + "' into a number. Challenge failed.");
			}
			base.gameObject.SafeDestroy();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002564 File Offset: 0x00000764
		public void DidTapCancel()
		{
			this.ParentalGate.Value.CompletionHandler(Result.UserCancelled);
			base.gameObject.SafeDestroy();
		}

		// Token: 0x0400000F RID: 15
		public RectTransform CanvasRectTransform;

		// Token: 0x04000010 RID: 16
		public RectTransform LogoImageRectTransform;

		// Token: 0x04000011 RID: 17
		public Text QuestionField;

		// Token: 0x04000012 RID: 18
		public RectTransform QuestionFieldRectTransform;

		// Token: 0x04000013 RID: 19
		public global::UnityEngine.UI.Image MinifigureImage;

		// Token: 0x04000014 RID: 20
		public RectTransform MinifigureImageRectTransform;

		// Token: 0x04000015 RID: 21
		public RectTransform AnswersPanelRectTransform;

		// Token: 0x04000016 RID: 22
		public Text[] PossibleAnswerTexts;
	}
}
