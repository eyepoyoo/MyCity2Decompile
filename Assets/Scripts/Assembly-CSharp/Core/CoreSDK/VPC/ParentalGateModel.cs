using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK.VPC
{
	// Token: 0x02000005 RID: 5
	public struct ParentalGateModel
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000021A4 File Offset: 0x000003A4
		public ParentalGateModel(Action<Result> completionHandler, Func<QuestionType> pickRandom = null)
		{
			pickRandom = pickRandom ?? new Func<QuestionType>(EnumExtensions.RandomEnum<QuestionType>);
			this.CompletionHandler = completionHandler;
			Random random = new Random();
			this.LeftHandSide = random.Next(1, 10);
			this.RightHandSide = random.Next(1, 10);
			this.QuestionType = pickRandom();
			QuestionType questionType = this.QuestionType;
			if (questionType != QuestionType.Multiplication)
			{
				if (questionType != QuestionType.Division)
				{
					this.Sign = string.Empty;
					this.Answer = 0;
				}
				else
				{
					this.Sign = "/";
					while (this.LeftHandSide % this.RightHandSide != 0)
					{
						this.LeftHandSide = random.Next(1, 10);
					}
					this.Answer = this.LeftHandSide / this.RightHandSide;
				}
			}
			else
			{
				this.Sign = "x";
				this.Answer = this.LeftHandSide * this.RightHandSide;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000229C File Offset: 0x0000049C
		public void Validate(int answer)
		{
			this.CompletionHandler((answer != this.Answer) ? Result.IsChild : Result.IsParent);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022BC File Offset: 0x000004BC
		public List<int> PossibleAnswers(int answerCount)
		{
			Random random = new Random();
			List<int> list = new List<int>();
			list.Add(this.Answer);
			while (list.Count != answerCount)
			{
				int num = random.Next(1, 100);
				if (!list.Contains(num))
				{
					list.Add(num);
				}
			}
			list.Shuffle<int>();
			return list;
		}

		// Token: 0x04000009 RID: 9
		public readonly int LeftHandSide;

		// Token: 0x0400000A RID: 10
		public readonly int RightHandSide;

		// Token: 0x0400000B RID: 11
		public readonly string Sign;

		// Token: 0x0400000C RID: 12
		internal readonly QuestionType QuestionType;

		// Token: 0x0400000D RID: 13
		private readonly int Answer;

		// Token: 0x0400000E RID: 14
		public readonly Action<Result> CompletionHandler;
	}
}
