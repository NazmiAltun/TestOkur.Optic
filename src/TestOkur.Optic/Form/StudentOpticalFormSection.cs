namespace TestOkur.Optic.Form
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using TestOkur.Optic.Answer;

	[DataContract]
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

		[DataMember]
		public List<QuestionAnswer> Answers { get; set; }

		[DataMember]
		public int EmptyCount { get; set; }

		[DataMember]
		public int WrongCount { get; set; }

		[DataMember]
		public int CorrectCount { get; set; }

		[DataMember]
		public float Net { get; set; }

		public float SuccessPercent => AnswerCount == 0 ? 0 : Net / AnswerCount * 100;

		private int AnswerCount => Answers.Count(a => a.Result != QuestionAnswerResult.NoResult);

		internal void Evaluate(int incorrectEliminationRate)
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
