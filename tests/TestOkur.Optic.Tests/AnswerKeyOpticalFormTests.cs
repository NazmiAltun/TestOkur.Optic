namespace TestOkur.Optic.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Form;
	using Xunit;

	public class AnswerKeyOpticalFormTests
	{
		[Fact]
		public void GivenExpand_WhenOnlyOneFormExists_ThenShouldReturnOneForm()
		{
			var form = new AnswerKeyOpticalForm
			{
				Booklet = 'A',
				ExamDate = DateTime.UtcNow.AddDays(-10),
				ExamId = 10,
				ExamName = "Test",
				IncorrectEliminationRate = 4,
			};
			form.AddSection(new AnswerKeyOpticalFormSection(1, "Test")
			{
				Answers = new List<AnswerKeyQuestionAnswer>()
				{
					new AnswerKeyQuestionAnswer(1, 0, 0, 0, 'A'),
					new AnswerKeyQuestionAnswer(2, 0, 0, 0, 'B'),
					new AnswerKeyQuestionAnswer(3, 0, 0, 0, 'C'),
				}
			});

			form.Expand().Should().HaveCount(1);
		}

		[Fact]
		public void GivenExpand_WhenAllFormsMatched_ThenAllFormsShouldReturn()
		{
			var form = new AnswerKeyOpticalForm
			{
				Booklet = 'A',
				ExamDate = DateTime.UtcNow.AddDays(-10),
				ExamId = 10,
				ExamName = "Test",
				IncorrectEliminationRate = 4,
			};
			form.AddSection(
				new AnswerKeyOpticalFormSection(1, "Test")
				{
					Answers = new List<AnswerKeyQuestionAnswer>()
					{
						new AnswerKeyQuestionAnswer(1, 2, 3, 4, 'A'),
						new AnswerKeyQuestionAnswer(2, 3, 4, 1, 'B'),
						new AnswerKeyQuestionAnswer(3, 4, 1, 2, 'C'),
						new AnswerKeyQuestionAnswer(4, 1, 2, 3, 'D'),
						new AnswerKeyQuestionAnswer(5, 6, 7, 8, 'A'),
						new AnswerKeyQuestionAnswer(6, 7, 8, 5, 'C'),
						new AnswerKeyQuestionAnswer(7, 8, 5, 6, 'B'),
						new AnswerKeyQuestionAnswer(8, 5, 6, 7, 'A'),
					}
				});

			var forms = form.Expand();
			forms.Should().Contain(f => f.Booklet == 'A' &&
										string.Join(",", f.Answers.Select(a => a.Answer).ToArray()) == "A,B,C,D,A,C,B,A");
			forms.Should().Contain(f => f.Booklet == 'B' &&
										string.Join(",", f.Answers.Select(a => a.Answer).ToArray()) == "D,A,B,C,A,A,C,B");
			forms.Should().Contain(f => f.Booklet == 'C' &&
										string.Join(",", f.Answers.Select(a => a.Answer).ToArray()) == "C,D,A,B,B,A,A,C");
			forms.Should().Contain(f => f.Booklet == 'D' &&
										string.Join(",", f.Answers.Select(a => a.Answer).ToArray()) == "B,C,D,A,C,B,A,A");
		}
	}
}
