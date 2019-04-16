namespace TestOkur.Optic.Form
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using static System.Math;

	public class StudentOpticalForm : OpticalForm
	{
		public StudentOpticalForm()
		{
			Sections = new List<StudentOpticalFormSection>();
			Scores = new Dictionary<string, float>();
		}

		public int StudentId { get; set; }

		public List<StudentOpticalFormSection> Sections { get; set; }

		public Dictionary<string, float> Scores { get; }

		public Guid ScanSessionReportId { get; set; }

		public int StudentNumber { get; set; }

		public string StudentFirstName { get; set; }

		public string StudentLastName { get; set; }

		public string SchoolName { get; set; }

		public int ClassroomId { get; set; }

		public string Classroom { get; set; }

		public string CityName { get; set; }

		public int CityId { get; set; }

		public string DistrictName { get; set; }

		public int DistrictId { get; set; }

		public int EmptyCount => Sections.Select(s => s.EmptyCount).Sum();

		public int WrongCount => Sections.Select(s => s.WrongCount).Sum();

		public int CorrectCount => Sections.Select(s => s.CorrectCount).Sum();

		public int QuestionCount => Sections.SelectMany(s => s.Answers).Count();

		public float Net => Sections.Select(s => s.Net).Sum();

		public float SuccessPercent => Sections.Select(s => s.SuccessPercent).Average();

		public float Score => Scores.Any() ? Scores.First().Value : SuccessPercent;

		public void AddSections(IEnumerable<StudentOpticalFormSection> sections)
		{
			Sections = Sections
				.Where(s => !sections.Select(se => se.LessonName)
					.Contains(s.LessonName))
				.ToList();

			foreach (var section in sections)
			{
				Sections.Add(section);
			}
		}

		public void Evaluate(AnswerKeyOpticalForm answerKeyOpticalForm)
		{
			if (answerKeyOpticalForm == null)
			{
				throw new ArgumentNullException(nameof(answerKeyOpticalForm));
			}

			foreach (var section in Sections)
			{
				section.Evaluate(answerKeyOpticalForm, answerKeyOpticalForm.IncorrectEliminationRate);
			}

			CalculateScore(answerKeyOpticalForm);
		}

		private void CalculateScore(AnswerKeyOpticalForm answerKeyOpticalForm)
		{
			if (answerKeyOpticalForm.ScoreFormulas == null)
			{
				return;
			}

			foreach (var formula in answerKeyOpticalForm.ScoreFormulas)
			{
				var score = formula.BasePoint;

				foreach (var lessonCoefficient in formula.Coefficients)
				{
					score += lessonCoefficient.Coefficient *
							 Sections.FirstOrDefault(s => s.LessonName == lessonCoefficient.Lesson)?.Net ?? 0;
				}

				Scores.Add(formula.ScoreName, (float)Round(score * 100) / 100);
			}
		}
	}
}
