namespace TestOkur.Optic.Answer
{
	public class AnswerKeyQuestionAnswer : QuestionAnswer
	{
		public AnswerKeyQuestionAnswer()
		{
		}

		public AnswerKeyQuestionAnswer(
			int questionNo,
			QuestionAnswer questionAnswer)
		{
			QuestionNo = questionNo;
			Answer = questionAnswer.Answer;
			SubjectName = questionAnswer.SubjectName;
			SubjectId = questionAnswer.SubjectId;
		}

		public AnswerKeyQuestionAnswer(
			int lessonId,
			string lessonName,
			int lessonOrder,
			int questionNo,
			int questionNoBookletB,
			int questionNoBookletC,
			int questionNoBookletD,
			char answer)
		{
			QuestionNo = questionNo;
			LessonId = lessonId;
			LessonName = lessonName;
			LessonOrder = lessonOrder;
			QuestionNoBookletB = questionNoBookletB;
			QuestionNoBookletC = questionNoBookletC;
			QuestionNoBookletD = questionNoBookletD;
			Answer = answer;
		}

		public string LessonName { get; set; }

		public int LessonId { get; set; }

		public int QuestionNoBookletB { get; set; }

		public int QuestionNoBookletC { get; set; }

		public int QuestionNoBookletD { get; set; }

		public int LessonOrder { get; set; }

		public QuestionAnswerCancelAction QuestionAnswerCancelAction { get; set; }

		public string Key => $"{LessonName}_{QuestionNo}";
	}
}