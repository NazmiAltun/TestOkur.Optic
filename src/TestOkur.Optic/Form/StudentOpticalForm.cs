namespace TestOkur.Optic.Form
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Score;
	using static System.Math;

	[DataContract]
	public class StudentOpticalForm : OpticalForm
	{
		private const char DefaultBooklet = 'A';

		public StudentOpticalForm(char booklet)
			: this()
		{
			Booklet = char.IsWhiteSpace(booklet) ? DefaultBooklet : booklet;
		}

		public StudentOpticalForm()
		{
			Sections = new List<StudentOpticalFormSection>();
			Scores = new Dictionary<string, float>();
			Orders = new List<StudentOrder>();
		}

		[DataMember]
		public int StudentId { get; set; }

		[DataMember]
		public List<StudentOpticalFormSection> Sections { get; set; }

		[DataMember]
		public Dictionary<string, float> Scores { get; private set; }

		[DataMember]
		public Guid ScanSessionReportId { get; set; }

		[DataMember]
		public int StudentNumber { get; set; }

		[DataMember]
		public string StudentFirstName { get; set; }

		[DataMember]
		public string StudentLastName { get; set; }

		[DataMember]
		public string SchoolName { get; set; }

		[DataMember]
		public int ClassroomId { get; set; }

		[DataMember]
		public string Classroom { get; set; }

		[DataMember]
		public string CityName { get; set; }

		[DataMember]
		public int CityId { get; set; }

		[DataMember]
		public string DistrictName { get; set; }

		[DataMember]
		public int DistrictId { get; set; }

		[DataMember]
		public int GeneralAttendanceCount { get; set; }

		[DataMember]
		public int CityAttendanceCount { get; internal set; }

		[DataMember]
		public int SchoolAttendanceCount { get; internal set; }

		[DataMember]
		public int ClassroomAttendanceCount { get; internal set; }

		[DataMember]
		public List<StudentOrder> Orders { get; private set; }

		public int EmptyCount => Sections.Select(s => s.EmptyCount).Sum();

		public int WrongCount => Sections.Select(s => s.WrongCount).Sum();

		public int CorrectCount => Sections.Select(s => s.CorrectCount).Sum();

		public int QuestionCount => Sections.SelectMany(s => s.Answers).Count();

		public float Net => Sections.Select(s => s.Net).Sum();

		public float SuccessPercent => (float)Round(
			                               Sections.Select(s => s.SuccessPercent).Average() *
			                               100) / 100;

		public float ClassroomAverageNet => Sections
			.SelectMany(s => s.Averages)
			.Where(a => a.Name == "NET")
			.Select(a => a.Classroom).Sum();

		public float SchoolAverageNet => Sections
			.SelectMany(s => s.Averages)
			.Where(a => a.Name == "NET")
			.Select(a => a.School).Sum();

		public float DistrictAverageNet => Sections
			.SelectMany(s => s.Averages)
			.Where(a => a.Name == "NET")
			.Select(a => a.District).Sum();

		public float CityAverageNet => Sections
			.SelectMany(s => s.Averages)
			.Where(a => a.Name == "NET")
			.Select(a => a.City).Sum();

		public float GeneralAverageNet => Sections
			.SelectMany(s => s.Averages)
			.Where(a => a.Name == "NET")
			.Select(a => a.General).Sum();

		public float Score => Scores.Any() ? Scores.First().Value : SuccessPercent;

		public void SetFromScanOutput(ScanOutput scanOutput, AnswerKeyOpticalForm answerKeyOpticalForm)
		{
			foreach (var answerKeyOpticalFormSection in answerKeyOpticalForm
				.Sections
				.Where(s => s.FormPart == scanOutput.FormPart))
			{
				var studentOpticalFormSection = new StudentOpticalFormSection(answerKeyOpticalFormSection.LessonId, answerKeyOpticalFormSection.LessonName);

				for (var i = 0; i < answerKeyOpticalFormSection.MaxQuestionCount; i++)
				{
					var correctAnswer = answerKeyOpticalFormSection.Answers.ElementAtOrDefault(i);
					var questionAnswer = new QuestionAnswer(i + 1, scanOutput.Next());
					questionAnswer.SetCorrectAnswer(correctAnswer);
					studentOpticalFormSection.Answers.Add(questionAnswer);
				}

				Sections.Add(studentOpticalFormSection);
			}
		}

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

		public void Evaluate(int incorrectEliminationRate, List<ScoreFormula> scoreFormulas)
		{
			EvaluateSections(incorrectEliminationRate);
			CalculateScore(scoreFormulas);
		}

		internal void ClearOrders() => Orders.Clear();

		internal void AddStudentOrder(StudentOrder item)
		{
			Orders.Add(item);
		}

		private void EvaluateSections(int incorrectEliminationRate)
		{
			foreach (var section in Sections)
			{
				section.Evaluate(incorrectEliminationRate);
			}
		}

		private void CalculateScore(List<ScoreFormula> scoreFormulas)
		{
			if (scoreFormulas == null)
			{
				return;
			}

			foreach (var formula in scoreFormulas)
			{
				var score = formula.BasePoint +
							formula.Coefficients
								.Select(c => c.Coefficient * Sections.FirstOrDefault(s => s.LessonName == c.Lesson)?.Net ?? 0)
								.Sum();
				Scores.Add(formula.ScoreName, (float)Round(score * 100) / 100);
			}
		}
	}
}
