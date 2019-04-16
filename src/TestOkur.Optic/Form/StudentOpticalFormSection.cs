namespace TestOkur.Optic.Form
{
	using System;

	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;

	public class StudentOpticalFormSection
	{
		public StudentOpticalFormSection(string lessonName, int lessonId)
		 : this()
		{
			LessonName = lessonName;
			LessonId = lessonId;
		}

		public StudentOpticalFormSection()
		{
			Answers = new List<QuestionAnswer>();
		}

		public List<QuestionAnswer> Answers { get; set; }

		public string LessonName { get; set; }

		public int LessonId { get; set; }

		public int EmptyCount { get; set; }

		public int WrongCount { get; set; }

		public int CorrectCount { get; set; }

		public float Net { get; set; }

		public float SuccessPercent => Answers.Count == 0 ? 0 : Net / Answers.Count * 100;

		public void Evaluate(IList<AnswerKeyQuestionAnswer> answerKeyQuestionAnswers, int incorrectEliminationRate)
		{
			Validate(answerKeyQuestionAnswers);
			SetCorrectAnswers(answerKeyQuestionAnswers);
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

		private void SetCorrectAnswers(IEnumerable<AnswerKeyQuestionAnswer> answerKeyQuestionAnswers)
		{
			foreach (var answer in answerKeyQuestionAnswers)
			{
				Answers.First(a => $"{LessonName}_{a.QuestionNo}" == answer.Key)
					.SetCorrectAnswer(answer);
			}
		}

		private void Validate(IEnumerable<AnswerKeyQuestionAnswer> answerKeyQuestionAnswers)
		{
			if (answerKeyQuestionAnswers == null || !answerKeyQuestionAnswers.Any())
			{
				throw new ArgumentNullException(nameof(answerKeyQuestionAnswers));
			}

			if (Answers == null || !Answers.Any())
			{
				throw new ArgumentNullException(nameof(Answers));
			}
		}
	}
}
