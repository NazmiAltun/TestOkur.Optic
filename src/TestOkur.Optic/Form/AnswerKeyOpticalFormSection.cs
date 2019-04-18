namespace TestOkur.Optic.Form
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;

	public class AnswerKeyOpticalFormSection : FormLessonSection
	{
		public AnswerKeyOpticalFormSection(AnswerKeyOpticalFormSection answerKeyOpticalFormSection)
		 : this()
		{
			LessonName = answerKeyOpticalFormSection.LessonName;
			LessonId = answerKeyOpticalFormSection.LessonId;
			ListOrder = answerKeyOpticalFormSection.ListOrder;
			MaxQuestionCount = answerKeyOpticalFormSection.MaxQuestionCount;
			FormPart = answerKeyOpticalFormSection.FormPart;
		}

		public AnswerKeyOpticalFormSection(
			int lessonId,
			string lessonName,
			int maxQuestionCount,
			int formPart,
			int listOrder)
			: this(lessonId, lessonName)
		{
			MaxQuestionCount = maxQuestionCount;
			FormPart = formPart;
			ListOrder = listOrder;
		}

		public AnswerKeyOpticalFormSection(int lessonId, string lessonName)
		 : this()
		{
			LessonName = lessonName;
			LessonId = lessonId;
		}

		public AnswerKeyOpticalFormSection()
		{
			Answers = new List<AnswerKeyQuestionAnswer>();
		}

		public List<AnswerKeyQuestionAnswer> Answers { get; set; }

		public void AddAnswer(AnswerKeyQuestionAnswer answerKeyQuestionAnswer)
		{
			Answers.Add(answerKeyQuestionAnswer);
			Answers = Answers.OrderBy(a => a.QuestionNo).ToList();
		}
	}
}
