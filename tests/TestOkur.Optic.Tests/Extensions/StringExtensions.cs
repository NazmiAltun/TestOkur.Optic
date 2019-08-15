namespace TestOkur.Optic.Tests.Extensions
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;

	public static class StringExtensions
	{
		public static List<AnswerKeyQuestionAnswer> ParseAnswers(this string answers)
		{
			return answers
				.Select((t, i) => new AnswerKeyQuestionAnswer(i + 1, t))
				.ToList();
		}
	}
}
