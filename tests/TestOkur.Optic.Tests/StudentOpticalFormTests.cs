namespace TestOkur.Optic.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Form;
	using TestOkur.Optic.Score;
	using Xunit;

	public class StudentOpticalFormTests
	{
		[Fact]
		public void Given_AddSections_When_SectionsExist_Then_Sections_ShouldBeOverriden()
		{
			var form = new StudentOpticalForm()
			{
				Sections = new List<StudentOpticalFormSection>()
				{
					new StudentOpticalFormSection(1, "Turkce")
					{
						Answers = new List<QuestionAnswer>()
						{
							new QuestionAnswer(1, 'A'),
							new QuestionAnswer(2, 'B'),
							new QuestionAnswer(3, 'C'),
						}
					},
					new StudentOpticalFormSection(2, "Matematik")
					{
						Answers = new List<QuestionAnswer>()
						{
							new QuestionAnswer(1, 'D'),
							new QuestionAnswer(2, 'C'),
							new QuestionAnswer(3, 'D'),
							new QuestionAnswer(4, 'B'),
							new QuestionAnswer(5, 'A'),
						}
					}
				}
			};
			var sections = new List<StudentOpticalFormSection>()
			{
				new StudentOpticalFormSection(1, "Turkce")
				{
					Answers = new List<QuestionAnswer>()
					{
						new QuestionAnswer(1, 'E'),
						new QuestionAnswer(2, 'D')
					}
				},
				new StudentOpticalFormSection(2, "Matematik")
				{
					Answers = new List<QuestionAnswer>()
					{
						new QuestionAnswer(1, 'A'),
						new QuestionAnswer(2, 'B'),
						new QuestionAnswer(3, 'E'),
					}
				},
				new StudentOpticalFormSection(3, "Fen")
				{
					Answers = new List<QuestionAnswer>()
					{
						new QuestionAnswer(1, 'B'),
						new QuestionAnswer(2, 'C')
					}
				},
			};
			form.AddSections(sections);
			form.Sections.Should()
				.HaveCount(sections.Count)
				.And
				.Contain(s => s.LessonName == "Turkce" &&
							  string.Join(',', s.Answers.Select(a => a.Answer)) == "E,D")
				.And
				.Contain(s => s.LessonName == "Matematik" &&
							  string.Join(',', s.Answers.Select(a => a.Answer)) == "A,B,E")
				.And
				.Contain(s => s.LessonName == "Fen" &&
							  string.Join(',', s.Answers.Select(a => a.Answer)) == "B,C");
		}

		[Fact]
		public void Given_AddSections_When_SectionsDoNotExist_Then_Sections_ShouldBeAdded()
		{
			var form = new StudentOpticalForm();
			var sections = new List<StudentOpticalFormSection>()
			{
				new StudentOpticalFormSection(1, "Turkce"),
				new StudentOpticalFormSection(2, "Matematik")
			};
			form.AddSections(sections);
			form.Sections.Should()
				.Contain(s => s.LessonName == "Turkce" && s.LessonId == 1)
				.And
				.Contain(s => s.LessonName == "Matematik" && s.LessonId == 2);
		}

		[Fact]
		public void Given_Evaluate_WhenScoreFormulaProvided_Score_ShouldBeCalculated()
		{
			var answerKeyOpticalForm = new AnswerKeyOpticalForm
			{
				Booklet = 'A',
				IncorrectEliminationRate = 0,
				ScoreFormulas = new List<ScoreFormula>()
				{
					new ScoreFormula(175, "Scholarship")
					{
						Coefficients = new List<LessonCoefficient>()
						{
							new LessonCoefficient("Tr", 4.33f),
							new LessonCoefficient("Math", 3.296f),
							new LessonCoefficient("Science", 2.601f),
							new LessonCoefficient("Social Science", 2.773f)
						}
					}
				}
			};
			answerKeyOpticalForm.AddSection(CreateSection(1, "Tr", 15));
			answerKeyOpticalForm.AddSection(CreateSection(2, "Math", 15));
			answerKeyOpticalForm.AddSection(CreateSection(3, "Science", 10));
			answerKeyOpticalForm.AddSection(CreateSection(4, "Social Science", 8));
			var answers = Enumerable.Repeat('A', 48);
			var studentForm = new StudentOpticalForm('A');
			studentForm.SetFromScanOutput(new ScanOutput(answers, 0), answerKeyOpticalForm);

			studentForm.Evaluate(answerKeyOpticalForm.IncorrectEliminationRate, answerKeyOpticalForm.ScoreFormulas);
			studentForm.Net.Should().Be(48);
			studentForm.Score.Should().Be(337.58f);
		}

		[Fact]
		public void Given_Evaluate_ShouldCalculateExpectedly()
		{
			var answerKeyOpticalForm = new AnswerKeyOpticalForm
			{
				Booklet = 'A',
				IncorrectEliminationRate = 4,
			};
			answerKeyOpticalForm.AddSection(new AnswerKeyOpticalFormSection(1, "Test", 20, default, default)
			{
				Answers = new List<AnswerKeyQuestionAnswer>()
				{
					new AnswerKeyQuestionAnswer(1, 0, 0, 0, 'A'),
					new AnswerKeyQuestionAnswer(2, 0, 0, 0, 'B'),
					new AnswerKeyQuestionAnswer(3, 0, 0, 0, 'C'),
					new AnswerKeyQuestionAnswer(4, 0, 0, 0, 'D'),
					new AnswerKeyQuestionAnswer(5, 0, 0, 0, 'E'),
					new AnswerKeyQuestionAnswer(6, 0, 0, 0, 'A'),
					new AnswerKeyQuestionAnswer(7, 0, 0, 0, 'B'),
					new AnswerKeyQuestionAnswer(8, 0, 0, 0, 'C'),
					new AnswerKeyQuestionAnswer(9, 0, 0, 0, 'D'),
					new AnswerKeyQuestionAnswer(10, 0, 0, 0, 'E'),
					new AnswerKeyQuestionAnswer(11, 0, 0, 0, 'B'),
					new AnswerKeyQuestionAnswer(12, 0, 0, 0, 'C'),
					new AnswerKeyQuestionAnswer(13, 0, 0, 0, 'A'),
					new AnswerKeyQuestionAnswer(14, 0, 0, 0, 'A'),
					new AnswerKeyQuestionAnswer(15, 0, 0, 0, 'A'),
				}
			});

			var studentForm = new StudentOpticalForm('A');
			studentForm.SetFromScanOutput(new ScanOutput("AAAAAAG AAA AAA    ", 0), answerKeyOpticalForm);
			studentForm.Evaluate(answerKeyOpticalForm.IncorrectEliminationRate, answerKeyOpticalForm.ScoreFormulas);
			studentForm.CorrectCount.Should().Be(5);
			studentForm.WrongCount.Should().Be(8);
			studentForm.EmptyCount.Should().Be(2);
			studentForm.Net.Should().Be(3);
			studentForm.SuccessPercent.Should().Be(20);
			studentForm.Score.Should().Be(20);
		}

		private AnswerKeyOpticalFormSection CreateSection(int lessonId, string lessonName, int count)
		{
			var list = new List<AnswerKeyQuestionAnswer>();

			for (var i = 0; i < count; i++)
			{
				list.Add(new AnswerKeyQuestionAnswer(i + 1, 0, 0, 0, 'A'));
			}

			return new AnswerKeyOpticalFormSection(lessonId, lessonName, count, default, default)
			{
				Answers = list
			};
		}
	}
}
