using System;
using TestOkur.Optic.Tests.Extensions;

namespace TestOkur.Optic.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Form;
	using TestOkur.Optic.Score;
	using Xunit;

	public class TytScoreTest
	{
		[Fact]
		public void ShouldCalculateCorrectScore()
		{
			var scoreFormula = new ScoreFormula(100, "TYT");
			scoreFormula.Coefficients.Add(new LessonCoefficient("Turkish", 3.333f));
			scoreFormula.Coefficients.Add(new LessonCoefficient("Social Science", 3.333f));
			scoreFormula.Coefficients.Add(new LessonCoefficient("Basic Mathematics", 3.334f));
			scoreFormula.Coefficients.Add(new LessonCoefficient("Science", 3.334f));

			var answerKeyForm = new AnswerKeyOpticalForm(
				'A',
				new List<ScoreFormula> { scoreFormula });

			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(1, "Turkish", 40, 1, 1)
			{
				Answers = "CECBEEBADEEBACBDBBEDDAEDDACEABDEDCBDBABD".ParseAnswers()
			});
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(6, "Social Science", 20, 1, 2)
			{
				Answers = "EAEADADECEEDCDEAEDBA".ParseAnswers()
			});
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(5, "Basic Mathematics", 40, 2, 3)
			{
				Answers = "DDBECACAACBCBECAEAADCDCDEDABAACCBDBAEDCB".ParseAnswers()
			});
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(2, "Science", 20, 2, 4)
			{
				Answers = "DBCCEEBAEECAACBBDECE".ParseAnswers()
			});

			var studentForm = new StudentOpticalForm('A');
			studentForm.SetFromScanOutput(new ScanOutput("CE EEE       C   EA DAE  ADEBEDEDCADEBAEDBE     D  B DEAEAAA", 1), answerKeyForm);
			studentForm.SetFromScanOutput(new ScanOutput("CB                                      A     CE  D         ", 2), answerKeyForm);
			studentForm.Evaluate(4, answerKeyForm.ScoreFormulas);
			studentForm.Net.Should().Be(15.25f);
			studentForm.Score.Should().Be(150.83f);
			Math.Round(studentForm.SuccessPercent).Should().Be(13f);
		}
	}
}
