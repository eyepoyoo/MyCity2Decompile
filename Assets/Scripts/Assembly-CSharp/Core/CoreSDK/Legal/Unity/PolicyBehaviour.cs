using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.CoreSDK.Extensions;
using LEGO.CoreSDK.Extensions.Unity;
using LEGO.CoreSDK.Unity.UI;
using UnityEngine;

namespace LEGO.CoreSDK.Legal.Unity
{
	// Token: 0x02000040 RID: 64
	public class PolicyBehaviour : UnityGUIComponent
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003EA0 File Offset: 0x000020A0
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003EA8 File Offset: 0x000020A8
		private LegalPolicies.Policy CurrentPolicy
		{
			get
			{
				return this._currentPolicy;
			}
			set
			{
				this._currentPolicy = value;
				this.CurrentPolicySize = null;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00003ECC File Offset: 0x000020CC
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003ED4 File Offset: 0x000020D4
		private Func<Locale, LegalPolicies> GetPolicies { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003EE0 File Offset: 0x000020E0
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003EE8 File Offset: 0x000020E8
		private LegalPolicies CurrentPolicies { get; set; }

		// Token: 0x060000B5 RID: 181 RVA: 0x00003EF4 File Offset: 0x000020F4
		public override void Start()
		{
			if (!this.didCallSetup)
			{
				base.gameObject.SafeDestroy();
				throw new Exception("The PolicyBehaviour has not been initialized. Please do not use the prefab directly. Instead see the documentation of how to show the policies view.");
			}
			this.SupportedPolicyLocales = this.SupportedLocalesInspector.Select((PolicyBehaviour.SupportedPolicyLocaleInspector x) => new PolicyBehaviour.SupportedPolicyLocale(x, this));
			this.RecalculateButtonSize();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003F48 File Offset: 0x00002148
		private void RecalculateButtonSize()
		{
			int num = 0;
			int num2 = 0;
			foreach (PolicyBehaviour.SupportedPolicyLocale supportedPolicyLocale in this.SupportedPolicyLocales)
			{
				Texture2D texture2D = new Texture2D(supportedPolicyLocale.Flag.Width, supportedPolicyLocale.Flag.Height);
				GUIContent guicontent = new GUIContent(" " + supportedPolicyLocale.DisplayText, texture2D);
				Vector2 vector = base.ButtonStyle.CalcSize(guicontent);
				num = Math.Max((int)vector.x, num);
				num2 = Math.Max((int)vector.y, num2);
				global::UnityEngine.Object.Destroy(texture2D);
			}
			this.LanguageButtonSize = new Vector2((float)num, (float)num2);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004024 File Offset: 0x00002224
		public void Setup(Func<Locale, LegalPolicies> getPolicies, Locale currentLocale, PolicyType? initialPolicyType, Action completionHandler)
		{
			this.didCallSetup = true;
			this.GetPolicies = getPolicies;
			this.CurrentLocale = currentLocale;
			this.CurrentPolicies = getPolicies(currentLocale);
			if (initialPolicyType != null)
			{
				LegalPolicies.Policy? policy = this.CurrentPolicies.Policies.FirstOrNull((LegalPolicies.Policy x) => x.Type == initialPolicyType.GetValueOrDefault() && initialPolicyType != null);
				if (policy != null)
				{
					this.CurrentState = PolicyBehaviour.State.Policy;
					this.CurrentPolicy = policy.Value;
				}
				else
				{
					LEGOSDK.Log.Warning("Failed to find a policy with the type: '" + initialPolicyType + "'");
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000040DC File Offset: 0x000022DC
		protected override void ScaleFactorDidChange(int scaleFactor)
		{
			base.ScaleFactorDidChange(scaleFactor);
			base.ButtonStyle.fontSize = this.ButtonLabelStyle.ActualSize;
			this.RecalculateButtonSize();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000410C File Offset: 0x0000230C
		protected override void OrientationDidChange()
		{
			base.OrientationDidChange();
			this.CurrentPolicySize = null;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004130 File Offset: 0x00002330
		protected override void OnGUI()
		{
			base.OnGUI();
			GUI.depth = -1;
			GUI.skin.verticalScrollbar = base.VerticalScrollbarStyle;
			GUI.skin.verticalScrollbarThumb = base.VerticalScrollbarThumbStyle;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), Texture2D.whiteTexture);
			this.DrawBackground();
			Rect rect = this.DrawToolbar();
			Rect rect2 = new Rect(0f, rect.y + rect.height, (float)Screen.width, (float)Screen.height - (rect.y + rect.height));
			GUI.BeginGroup(rect2);
			Rect rect3 = new Rect(0f, 0f, rect2.width, rect2.height);
			PolicyBehaviour.State currentState = this.CurrentState;
			if (currentState != PolicyBehaviour.State.Menu)
			{
				if (currentState == PolicyBehaviour.State.Policy)
				{
					this.DrawPolicy(rect3);
				}
			}
			else
			{
				this.DrawMenu(rect3);
			}
			GUI.EndGroup();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004230 File Offset: 0x00002430
		private Rect DrawToolbar()
		{
			Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)(this.CloseButton.Height + 2 * this.DefaultMargin));
			GUI.BeginGroup(rect);
			GUI.color = new Color(0.972549f, 0.972549f, 0.972549f);
			GUI.DrawTexture(rect, Texture2D.whiteTexture);
			GUI.color = Color.white;
			Vector2 vector = this.TitleStyle.Style.CalcSize(new GUIContent(this.CurrentPolicies.Title));
			GUI.Label(new Rect((rect.width - vector.x) / 2f, (rect.height - vector.y) / 2f, vector.x, vector.y), this.CurrentPolicies.Title, this.TitleStyle);
			if (GUI.Button(new Rect(rect.width - (float)this.CloseButton.Width - (float)this.DefaultMargin, (rect.height - (float)this.CloseButton.Height) / 2f, (float)this.CloseButton.Width, (float)this.CloseButton.Height), this.CloseButton, GUIStyle.none))
			{
				base.gameObject.SafeDestroy();
			}
			if (this.CurrentState == PolicyBehaviour.State.Policy && GUI.Button(new Rect((float)this.DefaultMargin, (rect.height - (float)this.BackButton.Height) / 2f, (float)this.BackButton.Width, (float)this.BackButton.Height), this.BackButton, GUIStyle.none))
			{
				this.CurrentState = PolicyBehaviour.State.Menu;
			}
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0f, rect.height - (float)(1 * this.ScaleFactor), (float)Screen.width, (float)(1 * this.ScaleFactor)), Texture2D.whiteTexture);
			GUI.color = Color.white;
			GUI.EndGroup();
			return rect;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004450 File Offset: 0x00002650
		private void DrawBackground()
		{
			float num = (float)this.Background.Width;
			float num2 = (float)this.Background.Height;
			if ((float)Screen.width > num)
			{
				num = (float)Screen.width;
				num2 = num2 / (float)this.Background.Width * num;
			}
			if ((float)Screen.height > num2)
			{
				num2 = (float)Screen.height;
				num = num / (float)this.Background.Height * num2;
			}
			Rect rect = new Rect(((float)Screen.width - num) / 2f, (float)Screen.height - num2, num, num2);
			GUI.DrawTexture(rect, this.Background);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000044F0 File Offset: 0x000026F0
		private void DrawMenu(Rect rect)
		{
			GUI.DrawTexture(new Rect(0f, rect.height - (float)this.PoliceMan.Height, (float)this.PoliceMan.Width, (float)this.PoliceMan.Height), this.PoliceMan);
			GUI.DrawTexture(new Rect((float)this.DefaultMargin, (float)this.DefaultMargin, (float)this.LEGOLogo.Width, (float)this.LEGOLogo.Height), this.LEGOLogo);
			Vector2 vector = base.ButtonStyle.CalcLargestSize(this.CurrentPolicies.Policies.Select((LegalPolicies.Policy x) => x.Title));
			float num = (rect.width - vector.x) * 0.5f;
			float num2 = (float)(this.ButtonBackground.Height - this.ButtonBackground.overflow.top - this.ButtonBackground.overflow.bottom) * 0.7f;
			float num3 = num2 * (float)this.CurrentPolicies.Policies.Count<LegalPolicies.Policy>() + (float)(this.DefaultMargin * (this.CurrentPolicies.Policies.Count<LegalPolicies.Policy>() - 1));
			float num4 = (rect.height - num3) * 0.5f;
			foreach (LegalPolicies.Policy policy in this.CurrentPolicies.Policies)
			{
				if (GUI.Button(new Rect(num, num4, vector.x, num2), policy.Title, base.ButtonStyle))
				{
					this.ScrollPosition = Vector2.zero;
					this.CurrentPolicy = policy;
					this.CurrentState = PolicyBehaviour.State.Policy;
				}
				num4 += num2 + (float)this.DefaultMargin;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000046FC File Offset: 0x000028FC
		private void RecalculatePolicySizes(int viewWidth)
		{
			List<PolicyBehaviour.PolicySize.SectionSizes> list = new List<PolicyBehaviour.PolicySize.SectionSizes>();
			int num = 0;
			foreach (LegalPolicies.Policy.Section section in this.CurrentPolicy.Sections)
			{
				PolicyBehaviour.PolicySize.SectionSizes sectionSizes = default(PolicyBehaviour.PolicySize.SectionSizes);
				sectionSizes.Headline = new GUIContent(section.Title);
				int num2 = (int)this.PolicyHeadlineStyle.Style.CalcHeight(sectionSizes.Headline, (float)viewWidth);
				sectionSizes.HeadlineRect = new Rect((float)this.DefaultMargin, (float)num, (float)viewWidth, (float)num2);
				num += num2 + this.DefaultMargin;
				List<PolicyBehaviour.PolicySize.SectionSizes.BodyPart> list2 = new List<PolicyBehaviour.PolicySize.SectionSizes.BodyPart>();
				foreach (string text in section.Text.Split(this.LegalSplitCount))
				{
					PolicyBehaviour.PolicySize.SectionSizes.BodyPart bodyPart = default(PolicyBehaviour.PolicySize.SectionSizes.BodyPart);
					bodyPart.Text = new GUIContent(text);
					int num3 = (int)this.PolicyBodyStyle.Style.CalcHeight(bodyPart.Text, (float)viewWidth);
					bodyPart.Rect = new Rect((float)this.DefaultMargin, (float)num, (float)viewWidth, (float)num3);
					num += num3;
					list2.Add(bodyPart);
				}
				sectionSizes.Parts = list2;
				num += this.DefaultMargin;
				list.Add(sectionSizes);
			}
			this.CurrentPolicySize = new PolicyBehaviour.PolicySize?(new PolicyBehaviour.PolicySize(num + 10, list));
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000048B4 File Offset: 0x00002AB4
		private void DrawPolicy(Rect rect)
		{
			GUI.DrawTexture(rect, Texture2D.whiteTexture);
			Rect rect2 = new Rect((float)this.DefaultMargin, (float)this.DefaultMargin, (float)this.LEGOLogo.Width, (float)this.LEGOLogo.Height);
			GUI.DrawTexture(rect2, this.LEGOLogo);
			Rect rect3 = UnityUI.Label(rect2.VerticalPositionForNextElement(4), this.DefaultMargin, this.TitleStyle, this.CurrentPolicy.Title, 16);
			UnityUI.Label((int)rect3.x, rect3.VerticalPositionForNextElement(0), this.LastUpdatedStyle, this.CurrentPolicy.LastChanged, 16);
			Vector2 vector;
			if ((float)(Screen.width - (rect3.HorizontalPositionForNextElement(8) + 2 * this.DefaultMargin)) >= this.LanguageButtonSize.x)
			{
				vector = new Vector2((float)Screen.width - this.LanguageButtonSize.x - (float)this.DefaultMargin, rect3.y);
			}
			else
			{
				vector = new Vector2(((float)Screen.width - this.LanguageButtonSize.x) / 2f, (float)rect2.VerticalPositionForNextElement(8));
			}
			Rect rect4 = this.DrawLanguageSelector(vector);
			int num = (int)base.VerticalScrollbarStyle.fixedWidth;
			int num2 = (int)(rect.width - (float)(this.DefaultMargin * 2) - (float)num);
			PolicyBehaviour.PolicySize? currentPolicySize = this.CurrentPolicySize;
			if (currentPolicySize == null)
			{
				this.RecalculatePolicySizes(num2);
			}
			int num3 = ((rect2.VerticalPositionForNextElement(8) <= rect4.VerticalPositionForNextElement(8)) ? rect4.VerticalPositionForNextElement(8) : rect2.VerticalPositionForNextElement(8));
			Rect rect5 = new Rect(0f, (float)num3, rect.width - 8f, rect.height - (float)rect2.VerticalPositionForNextElement(8) - (float)this.DefaultMargin);
			Rect rect6 = new Rect(this.ScrollPosition.x, this.ScrollPosition.y, rect5.width, rect5.height);
			this.ScrollPosition = GUI.BeginScrollView(rect5, this.ScrollPosition, new Rect(0f, 0f, (float)num2, (float)this.CurrentPolicySize.Value.EntirePolicyHeight));
			foreach (PolicyBehaviour.PolicySize.SectionSizes sectionSizes in this.CurrentPolicySize.Value.Sections)
			{
				if (rect6.Overlaps(sectionSizes.HeadlineRect))
				{
					GUI.Label(sectionSizes.HeadlineRect, sectionSizes.Headline, this.PolicyHeadlineStyle);
				}
				foreach (PolicyBehaviour.PolicySize.SectionSizes.BodyPart bodyPart in sectionSizes.Parts)
				{
					if (rect6.Overlaps(bodyPart.Rect))
					{
						GUI.Label(bodyPart.Rect, bodyPart.Text, this.PolicyBodyStyle);
					}
				}
			}
			GUI.EndScrollView();
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect5.Contains(Event.current.mousePosition))
			{
				this.isDragging = true;
			}
			if (this.isDragging && Event.current.type == EventType.MouseDrag)
			{
				this.ScrollPosition.y = this.ScrollPosition.y + Event.current.delta.y;
			}
			if (Event.current.type == EventType.MouseUp)
			{
				this.isDragging = false;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004C98 File Offset: 0x00002E98
		private Rect DrawLanguageSelector(Vector2 position)
		{
			PolicyBehaviour.SupportedPolicyLocale supportedPolicyLocale = this.SupportedPolicyLocales.First((PolicyBehaviour.SupportedPolicyLocale x) => x.Locale == this.CurrentLocale);
			GUIContent guicontent = new GUIContent();
			guicontent.image = supportedPolicyLocale.Flag;
			guicontent.text = " " + supportedPolicyLocale.DisplayText;
			Rect rect = new Rect(position.x, position.y, this.LanguageButtonSize.x, this.LanguageButtonSize.y);
			if (GUI.Button(rect, guicontent, base.ButtonStyle))
			{
				Vector2 vector = GUIUtility.GUIToScreenPoint(rect.position);
				float num = (float)Screen.height - vector.y - rect.height - (float)this.DefaultMargin;
				if (num > (float)(this.SupportedPolicyLocales.Count<PolicyBehaviour.SupportedPolicyLocale>() - 1) * this.LanguageButtonSize.y)
				{
					num = (float)(this.SupportedPolicyLocales.Count<PolicyBehaviour.SupportedPolicyLocale>() - 1) * this.LanguageButtonSize.y;
				}
				Rect rect2 = new Rect(vector.x, vector.y + rect.height, rect.width, num);
				List<PolicyBehaviour.SupportedPolicyLocale> list = this.SupportedPolicyLocales.Where((PolicyBehaviour.SupportedPolicyLocale x) => x.Locale != this.CurrentLocale).ToList<PolicyBehaviour.SupportedPolicyLocale>();
				base.gameObject.AddComponent<LanguageSelectorDropdown>().Setup(rect2, this.LanguageButtonSize, list, base.ButtonStyle, base.VerticalScrollbarStyle, delegate(PolicyBehaviour.SupportedPolicyLocale y)
				{
					this.CurrentLocale = y.Locale;
					this.CurrentPolicies = this.GetPolicies(this.CurrentLocale);
					this.CurrentPolicy = this.CurrentPolicies.Policies.First((LegalPolicies.Policy policy) => policy.Type == this.CurrentPolicy.Type);
				});
			}
			return rect;
		}

		// Token: 0x04000066 RID: 102
		private PolicyBehaviour.State CurrentState;

		// Token: 0x04000067 RID: 103
		private LegalPolicies.Policy _currentPolicy;

		// Token: 0x04000068 RID: 104
		private Locale CurrentLocale;

		// Token: 0x04000069 RID: 105
		public Image Background;

		// Token: 0x0400006A RID: 106
		public Image LEGOLogo;

		// Token: 0x0400006B RID: 107
		public LabelStyle TitleStyle;

		// Token: 0x0400006C RID: 108
		public Image CloseButton;

		// Token: 0x0400006D RID: 109
		public Image BackButton;

		// Token: 0x0400006E RID: 110
		public Image PoliceMan;

		// Token: 0x0400006F RID: 111
		public LabelStyle LastUpdatedStyle;

		// Token: 0x04000070 RID: 112
		public LabelStyle PolicyHeadlineStyle;

		// Token: 0x04000071 RID: 113
		public LabelStyle PolicyBodyStyle;

		// Token: 0x04000072 RID: 114
		public PolicyBehaviour.SupportedPolicyLocaleInspector[] SupportedLocalesInspector;

		// Token: 0x04000073 RID: 115
		private IEnumerable<PolicyBehaviour.SupportedPolicyLocale> SupportedPolicyLocales;

		// Token: 0x04000074 RID: 116
		private Vector2 ScrollPosition;

		// Token: 0x04000075 RID: 117
		private Vector2 LanguageButtonSize;

		// Token: 0x04000076 RID: 118
		private PolicyBehaviour.PolicySize? CurrentPolicySize;

		// Token: 0x04000077 RID: 119
		public int LegalSplitCount = 6000;

		// Token: 0x04000078 RID: 120
		private bool didCallSetup;

		// Token: 0x04000079 RID: 121
		private bool isDragging;

		// Token: 0x02000041 RID: 65
		internal struct PolicySize
		{
			// Token: 0x060000C7 RID: 199 RVA: 0x00004EC8 File Offset: 0x000030C8
			internal PolicySize(int fullHeight, IEnumerable<PolicyBehaviour.PolicySize.SectionSizes> sections)
			{
				this.EntirePolicyHeight = fullHeight;
				this.Sections = sections;
			}

			// Token: 0x0400007D RID: 125
			internal int EntirePolicyHeight;

			// Token: 0x0400007E RID: 126
			internal IEnumerable<PolicyBehaviour.PolicySize.SectionSizes> Sections;

			// Token: 0x02000042 RID: 66
			internal struct SectionSizes
			{
				// Token: 0x0400007F RID: 127
				internal GUIContent Headline;

				// Token: 0x04000080 RID: 128
				internal Rect HeadlineRect;

				// Token: 0x04000081 RID: 129
				internal IEnumerable<PolicyBehaviour.PolicySize.SectionSizes.BodyPart> Parts;

				// Token: 0x02000043 RID: 67
				internal struct BodyPart
				{
					// Token: 0x04000082 RID: 130
					public GUIContent Text;

					// Token: 0x04000083 RID: 131
					public Rect Rect;
				}
			}
		}

		// Token: 0x02000044 RID: 68
		private enum State
		{
			// Token: 0x04000085 RID: 133
			Menu,
			// Token: 0x04000086 RID: 134
			Policy
		}

		// Token: 0x02000045 RID: 69
		[Serializable]
		public struct SupportedPolicyLocaleInspector
		{
			// Token: 0x04000087 RID: 135
			public string Locale;

			// Token: 0x04000088 RID: 136
			public Texture2D Flag;
		}

		// Token: 0x02000046 RID: 70
		public struct SupportedPolicyLocale
		{
			// Token: 0x060000C8 RID: 200 RVA: 0x00004ED8 File Offset: 0x000030D8
			public SupportedPolicyLocale(PolicyBehaviour.SupportedPolicyLocaleInspector supportedPolicy, UnityGUIComponent owner)
			{
				ILocaleFactory localeFactory = LEGOSDK.DependencyResolver.Resolve<ILocaleFactory>();
				this.Locale = localeFactory.Create(supportedPolicy.Locale);
				this.Flag = new Image(supportedPolicy.Flag, Image.RetinaFactor.Two, new RectOffset(0, 0, 0, 0), new RectOffset(0, 0, 0, 0));
				this.Flag.Owner = owner;
				this.DisplayText = this.Locale.DisplayName;
			}

			// Token: 0x04000089 RID: 137
			public Locale Locale;

			// Token: 0x0400008A RID: 138
			public Image Flag;

			// Token: 0x0400008B RID: 139
			public string DisplayText;
		}
	}
}
