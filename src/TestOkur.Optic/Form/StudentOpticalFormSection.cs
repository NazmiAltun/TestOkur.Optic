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
			Averages = new List<Average>();
		}

		public List<QuestionAnswer> Answers { get; set; }

		public int EmptyCount { get; set; }

		public int WrongCount { get; set; }

		public int CorrectCount { get; set; }

		public int QuestionCount => Answers.Count(a => a.Result != QuestionAnswerResult.NoResult);

		public float Net { get; set; }

		public List<Average> Averages { get; set; }

		public float SuccessPercent => AnswerCount == 0 ? 0 : Net / AnswerCount * 100;

		public float ClassroomAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.Classroom ?? 0;

		public float SchoolAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.School ?? 0;

		public float DistrictAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.District ?? 0;

		public float CityAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.City ?? 0;

		public float GeneralAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.General ?? 0;

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

		public void ClearLessonAverages() => Averages.Clear();

		public void AddLessonAverage(Average average)
		{
			Averages.Add(average);
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
