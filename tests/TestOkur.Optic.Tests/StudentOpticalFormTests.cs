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

    public class StudentOpticalFormTests
    {
        [Fact]
        public void EmptyCorrectAnswers_Should_NotAddedToScoreAndSuccessPercentCalculations()
        {
            var studentOpticalForm = new StudentOpticalForm()
            {
                Sections = new List<StudentOpticalFormSection>
                {
                    new StudentOpticalFormSection()
                    {
                        Answers =  "ABCDA CDAA".ParseStudentAnswers("ABCDA CDAA")
                    }
                }
            };
            studentOpticalForm.Evaluate(0, null);
            studentOpticalForm.Score.Should().Be(100);
            studentOpticalForm.SuccessPercent.Should().Be(100);
        }
        [Fact]
        public void Give_Evaluate_When_FormulaChanges_ThenCalculatedFormulas_Should_Be_Changed()
        {
            var answerKeyOpticalForm = new AnswerKeyOpticalForm
            {
                Booklet = 'A',
                IncorrectEliminationRate = 0,
                ScoreFormulas = new List<ScoreFormula>()
                {
                    new ScoreFormula(100, "LGS",8)
                    {
                        Coefficients = new List<LessonCoefficient>()
                        {
                            new LessonCoefficient("Tr", 4),
                            new LessonCoefficient("Math",3)
                        }
                    }
                }
            };
            answerKeyOpticalForm.AddSection(CreateSection(1, "Tr", 3));
            answerKeyOpticalForm.AddSection(CreateSection(2, "Math", 3));

            var random = new Random();
            var answers = Enumerable.Repeat('A', 6);
            var studentForm = new StudentOpticalForm('A');
            studentForm.Grade = 8;
            studentForm.SetFromScanOutput(new ScanOutput(answers, 0,random.Next(1000),'A'), answerKeyOpticalForm);
            studentForm.Evaluate(0, answerKeyOpticalForm.ScoreFormulas);
            var previousScoreName = studentForm.Scores.First().Key;
            var previousScore = studentForm.Scores.First().Value;
            answerKeyOpticalForm.ScoreFormulas = new List<ScoreFormula>()
            {
                new ScoreFormula(100, "TYT",10)
                {
                    Coefficients = new List<LessonCoefficient>()
                    {
                        new LessonCoefficient("Tr", 1),
                        new LessonCoefficient("Math", 1)
                    }
                }
            };
            studentForm.Evaluate(0, answerKeyOpticalForm.ScoreFormulas);
            studentForm.Scores.First().Key.Should().NotBe(previousScoreName);
            studentForm.Scores.First().Value.Should().NotBe(previousScore);
        }

        [Fact]
        public void Given_UpdateAnswers_WhenAnyCorrectAnswerChanged_Then_ResultsShouldChange()
        {
            var answerKeyForm = new AnswerKeyOpticalForm('A', null);
            answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(1, "Mat", 3, 1, 1)
            {
                Answers = "AAA".ParseAnswerkeyAnswers()
            });
            answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(2, "TR", 3, 1, 2)
            {
                Answers = "BBB".ParseAnswerkeyAnswers()
            });
            var form = new StudentOpticalForm('A');
            var random = new Random();

            form.SetFromScanOutput(new ScanOutput("AAABBB", 1,random.Next(1000),'A'), answerKeyForm);
            form.Evaluate(0, null);
            form.CorrectCount.Should().Be(6);
            answerKeyForm.Sections.First().Answers = "CCC".ParseAnswerkeyAnswers();
            form.UpdateCorrectAnswers(answerKeyForm);
            form.Evaluate(0, null);
            form.CorrectCount.Should().Be(3);
        }

        [Fact]
        public void Given_AddSections_When_SectionsExist_Then_Sections_ShouldBeOverriden()
        {
            var form = new StudentOpticalForm()
            {
                Sections = new List<StudentOpticalFormSection>()
                {
                    new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(1, "Turkce"))
                    {
                        Answers = new List<QuestionAnswer>()
                        {
                            new QuestionAnswer(1, 'A'),
                            new QuestionAnswer(2, 'B'),
                            new QuestionAnswer(3, 'C'),
                        }
                    },
                    new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(2, "Matematik"))
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
                new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(1, "Turkce"))
                {
                    Answers = new List<QuestionAnswer>()
                    {
                        new QuestionAnswer(1, 'E'),
                        new QuestionAnswer(2, 'D')
                    }
                },
                new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(2, "Matematik"))
                {
                    Answers = new List<QuestionAnswer>()
                    {
                        new QuestionAnswer(1, 'A'),
                        new QuestionAnswer(2, 'B'),
                        new QuestionAnswer(3, 'E'),
                    }
                },
                new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(3, "Fen"))
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
                new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(1, "Turkce")),
                new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(2, "Matematik"))
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
                    new ScoreFormula(175, "Scholarship",11)
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
            var random = new Random();

            studentForm.SetFromScanOutput(new ScanOutput(answers, 0,random.Next(1000),'A'), answerKeyOpticalForm);

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
                    new AnswerKeyQuestionAnswer(1, 0, 0, 0, 'A',0,string.Empty),
                    new AnswerKeyQuestionAnswer(2, 0, 0, 0, 'B',0,string.Empty),
                    new AnswerKeyQuestionAnswer(3, 0, 0, 0, 'C',0,string.Empty),
                    new AnswerKeyQuestionAnswer(4, 0, 0, 0, 'D',0,string.Empty),
                    new AnswerKeyQuestionAnswer(5, 0, 0, 0, 'E',0,string.Empty),
                    new AnswerKeyQuestionAnswer(6, 0, 0, 0, 'A',0,string.Empty),
                    new AnswerKeyQuestionAnswer(7, 0, 0, 0, 'B',0,string.Empty),
                    new AnswerKeyQuestionAnswer(8, 0, 0, 0, 'C',0,string.Empty),
                    new AnswerKeyQuestionAnswer(9, 0, 0, 0, 'D',0,string.Empty),
                    new AnswerKeyQuestionAnswer(10, 0, 0, 0, 'E',0,string.Empty),
                    new AnswerKeyQuestionAnswer(11, 0, 0, 0, 'B',0,string.Empty),
                    new AnswerKeyQuestionAnswer(12, 0, 0, 0, 'C',0,string.Empty),
                    new AnswerKeyQuestionAnswer(13, 0, 0, 0, 'A',0,string.Empty),
                    new AnswerKeyQuestionAnswer(14, 0, 0, 0, 'A',0,string.Empty),
                    new AnswerKeyQuestionAnswer(15, 0, 0, 0, 'A',0,string.Empty),
                }
            });

            var studentForm = new StudentOpticalForm('A');
            var random = new Random();

            studentForm.SetFromScanOutput(new ScanOutput("AAAAAAG AAA AAA    ", 0,random.Next(),'A'), answerKeyOpticalForm);
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
                list.Add(new AnswerKeyQuestionAnswer(i + 1, 0, 0, 0, 'A', 0, string.Empty));
            }

            return new AnswerKeyOpticalFormSection(lessonId, lessonName, count, default, default)
            {
                Answers = list
            };
        }
    }
}
