namespace TestOkur.Optic.Answer
{
	public class AnswerKeyQuestionAnswer : QuestionAnswer
	{
		public AnswerKeyQuestionAnswer()
		{
		}

		public AnswerKeyQuestionAnswer(
			int questionNo,
			char answer)
		{
			QuestionNo = questionNo;
			Answer = answer;
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
			int questionNo,
			int questionNoBookletB,
			int questionNoBookletC,
			int questionNoBookletD,
			char answer)
		{
			QuestionNo = questionNo;
			QuestionNoBookletB = questionNoBookletB;
			QuestionNoBookletC = questionNoBookletC;
			QuestionNoBookletD = questionNoBookletD;
			Answer = answer;
		}

		public int QuestionNoBookletB { get; set; }

		public int QuestionNoBookletC { get; set; }

		public int QuestionNoBookletD { get; set; }

		public QuestionAnswerCancelAction QuestionAnswerCancelAction { get; set; }
	}
}