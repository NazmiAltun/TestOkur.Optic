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
			AnswerKeyQuestionAnswer questionAnswer)
		{
			QuestionAnswerCancelAction = questionAnswer.QuestionAnswerCancelAction;
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
			char answer,
			int subjectId,
			string subjectName)
			: this(questionNo, questionNoBookletB, questionNoBookletC, questionNoBookletD, answer, subjectId, subjectName, QuestionAnswerCancelAction.None)
		{
		}

		public AnswerKeyQuestionAnswer(
			int questionNo,
			int questionNoBookletB,
			int questionNoBookletC,
			int questionNoBookletD,
			char answer,
			int subjectId,
			string subjectName,
			QuestionAnswerCancelAction questionAnswerCancelAction)
		{
			QuestionNo = questionNo;
			QuestionNoBookletB = questionNoBookletB;
			QuestionNoBookletC = questionNoBookletC;
			QuestionNoBookletD = questionNoBookletD;
			Answer = answer;
			QuestionAnswerCancelAction = questionAnswerCancelAction;
			SubjectId = subjectId;
			SubjectName = subjectName;
		}

		public int QuestionNoBookletB { get; set; }

		public int QuestionNoBookletC { get; set; }

		public int QuestionNoBookletD { get; set; }

		public QuestionAnswerCancelAction QuestionAnswerCancelAction { get; set; }
	}
}