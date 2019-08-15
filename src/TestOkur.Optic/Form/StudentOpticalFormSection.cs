namespace TestOkur.Optic.Form
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using TestOkur.Optic.Answer;

	[DataContract]
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

		[DataMember]
		public List<QuestionAnswer> Answers { get; set; }

		[DataMember]
		public int EmptyCount { get; set; }

		[DataMember]
		public int WrongCount { get; set; }

		[DataMember]
		public int CorrectCount { get; set; }

		[DataMember]
		public int QuestionCount => Answers.Count;

		[DataMember]
		public float Net { get; set; }

		[DataMember]
		public List<Average> Averages { get; private set; }

		[DataMember]
		public float SuccessPercent => AnswerCount == 0 ? 0 : Net / AnswerCount * 100;

		[DataMember]
		public float ClassroomAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.Classroom ?? 0;

		[DataMember]
		public float SchoolAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.School ?? 0;

		[DataMember]
		public float DistrictAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.District ?? 0;

		[DataMember]
		public float CityAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.City ?? 0;

		[DataMember]
		public float GeneralAverageNet => Averages?.FirstOrDefault(a => a.Name == "NET")?.General ?? 0;

		private int AnswerCount => Answers.Count(a => a.Result != QuestionAnswerResult.NoResult);

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
