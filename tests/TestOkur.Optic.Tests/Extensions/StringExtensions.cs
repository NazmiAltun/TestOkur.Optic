namespace TestOkur.Optic.Tests.Extensions
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;

	public static class StringExtensions
	{
		public static List<AnswerKeyQuestionAnswer> ParseAnswerkeyAnswers(this string answers)
		{
			return answers
				.Select((t, i) => new AnswerKeyQuestionAnswer(i + 1, t))
				.ToList();
		}

		public static List<QuestionAnswer> ParseStudentAnswers(this string studentAnswers,string correctAnswers)
		{
			var list = studentAnswers
				.Select((t, i) => new QuestionAnswer(i + 1, t))
				.ToList();
			var answerList = correctAnswers.ParseAnswerkeyAnswers();
			list.ForEach(x =>
			{
				x.SetCorrectAnswer(answerList.First(a => a.QuestionNo == x.QuestionNo));
			});
			
			return list;
		}
	}
}
