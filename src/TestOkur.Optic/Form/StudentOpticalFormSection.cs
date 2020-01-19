namespace TestOkur.Optic.Form
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;

	public class StudentOpticalFormSection : FormLessonSection
	{
		public StudentOpticalFormSection(AnswerKeyOpticalFormSection section)
		 : this()
		{
			MaxQuestionCount = section.MaxQuestionCount;
			ListOrder = section.ListOrder;
			FormPart = section.FormPart;
			LessonName = section.LessonName;
			LessonId = section.LessonId;
		}

		public StudentOpticalFormSection()
		{
			Answers = new List<QuestionAnswer>();
		}

		public List<QuestionAnswer> Answers { get; set; }

		public int EmptyCount { get; set; }

		public int WrongCount { get; set; }

		public int CorrectCount { get; set; }

		public int QuestionCount => Answers.Count(a => a.Result != QuestionAnswerResult.NoResult);

		public float Net { get; set; }

		public float SuccessPercent => AnswerCount == 0 ? 0 : Net / AnswerCount * 100;

		private int AnswerCount => Answers.Count(a => a.Result != QuestionAnswerResult.NoResult &&
													  a.CorrectAnswer != QuestionAnswer.Empty);

		public void UpdateAnswers(AnswerKeyOpticalFormSection section)
		{
			foreach (var answer in section.Answers)
			{
				Answers
					.FirstOrDefault(a => a.QuestionNo == answer.QuestionNo)
					?.SetCorrectAnswer(answer);
			}
		}

		public void Evaluate(int incorrectEliminationRate)
		{
			CalculateResult(incorrectEliminationRate);
		}

		private void CalculateResult(int incorrectEliminationRate)
		{
			EmptyCount = Answers
				.Count(a => a.Result == QuestionAnswerResult.Empty);

			WrongCount = Answers
				.Count(a => a.Result == QuestionAnswerResult.Wrong || a.Result == QuestionAnswerResult.Invalid);

			CorrectCount = Answers
				.Count(a => a.Result == QuestionAnswerResult.Correct);

			Net = incorrectEliminationRate == 0
				? CorrectCount
				: CorrectCount - ((float)WrongCount / incorrectEliminationRate);
		}
	}
}
