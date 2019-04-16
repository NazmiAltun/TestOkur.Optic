namespace TestOkur.Optic.Form
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;

	public class StudentOpticalFormSection : FormLessonSection
	{
		public StudentOpticalFormSection(int lessonId, string lessonName)
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

		public int EmptyCount { get; set; }

		public int WrongCount { get; set; }

		public int CorrectCount { get; set; }

		public float Net { get; set; }

		public float SuccessPercent => Answers.Count == 0 ? 0 : Net / Answers.Count * 100;

		public void Evaluate(AnswerKeyOpticalForm answerKeyOpticalForm, int incorrectEliminationRate)
		{
			var answerKeyQuestionAnswers = answerKeyOpticalForm
				.Sections
				.FirstOrDefault(s => s.LessonName == LessonName)
				?.Answers;
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
				Answers.First(a => a.QuestionNo == answer.QuestionNo)
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
