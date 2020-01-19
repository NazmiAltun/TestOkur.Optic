namespace TestOkur.Optic.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Answer;
    using TestOkur.Optic.Score;
    using static System.Math;

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

        public int StudentId { get; set; }

        public List<StudentOpticalFormSection> Sections { get; set; }

        public List<StudentSubjectDetail> StudentSubjectDetails { get; set; }

        public Dictionary<string, float> Scores { get; set; }

        public Guid ScanSessionReportId { get; set; }

        public int StudentNumber { get; set; }

        public string CitizenshipIdentity { get; set; }

        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        public string SchoolName { get; set; }

        public int ClassroomId { get; set; }

        public string Classroom { get; set; }

        public string CityName { get; set; }

        public int CityId { get; set; }

        public string DistrictName { get; set; }

        public int DistrictId { get; set; }

        public List<StudentOrder> Orders { get; set; }

        public int EmptyCount => Sections.Select(s => s.EmptyCount).Sum();

        public int WrongCount => Sections.Select(s => s.WrongCount).Sum();

        public int CorrectCount => Sections.Select(s => s.CorrectCount).Sum();

        public int QuestionCount => Sections.SelectMany(s => s.Answers)
            .Count(q => q.Result != QuestionAnswerResult.NoResult);

        public float Net => Sections.Select(s => s.Net).Sum();

        public float SuccessPercent => CalculateSuccessPercent();
        
        public int ClassOrder => Orders?.FirstOrDefault(o => o.Name == "NET")?.ClassroomOrder ?? 0;

        public int SchoolOrder => Orders?.FirstOrDefault(o => o.Name == "NET")?.SchoolOrder ?? 0;

        public int DistrictOrder => Orders?.FirstOrDefault(o => o.Name == "NET")?.DistrictOrder ?? 0;

        public int CityOrder => Orders?.FirstOrDefault(o => o.Name == "NET")?.CityOrder ?? 0;

        public int GeneralOrder => Orders?.FirstOrDefault(o => o.Name == "NET")?.GeneralOrder ?? 0;

        public float Score => Scores.Any() ? Scores.First().Value : SuccessPercent;

        public int Grade { get; set; }
        
        public ScanOutput ScanOutput { get; set; }

        public void UpdateCorrectAnswers(AnswerKeyOpticalForm answerKeyOpticalForm)
        {
            foreach (var answerKeyOpticalFormSection in answerKeyOpticalForm.Sections)
            {
                Sections
                    .FirstOrDefault(s => s.LessonName == answerKeyOpticalFormSection.LessonName)
                    ?.UpdateAnswers(answerKeyOpticalFormSection);
            }
        }

        public void SetFromScanOutput(ScanOutput scanOutput, AnswerKeyOpticalForm answerKeyOpticalForm)
        {
            ScanOutput = scanOutput;
            foreach (var answerKeyOpticalFormSection in answerKeyOpticalForm
                .Sections
                .Where(s => s.FormPart == scanOutput.FormPart))
            {
                var studentOpticalFormSection = new StudentOpticalFormSection(answerKeyOpticalFormSection);

                for (var i = 0; i < answerKeyOpticalFormSection.Answers.Count; i++)
                {
                    var correctAnswer = answerKeyOpticalFormSection.Answers.ElementAt(i);
                    var questionAnswer = new QuestionAnswer(i + 1, scanOutput.Next());
                    questionAnswer.SetCorrectAnswer(correctAnswer);
                    studentOpticalFormSection.Answers.Add(questionAnswer);
                }

                scanOutput.Skip(answerKeyOpticalFormSection.MaxQuestionCount - answerKeyOpticalFormSection.Answers.Count);
                Sections.Add(studentOpticalFormSection);
            }
        }

        public StudentOpticalForm Merge(StudentOpticalForm form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            AddSections(form.Sections);

            return this;
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

            Sections = Sections
                .OrderBy(s => s.FormPart)
                .ThenBy(s => s.ListOrder)
                .ToList();
        }

        public void Evaluate(int incorrectEliminationRate, List<ScoreFormula> scoreFormulas)
        {
            EvaluateSections(incorrectEliminationRate);
            CalculateScore(scoreFormulas);
            SetSubjectDetails();
        }

        public bool ContainsSection(int lessonId) => Sections.Any(s => s.LessonId == lessonId);

        public void ClearOrders() => Orders.Clear();

        public void AddStudentOrder(StudentOrder item)
        {
            Orders.Add(item);
        }

        public void AddEmptySection(AnswerKeyOpticalFormSection answerKeyOpticalFormSection)
        {
            var section = new StudentOpticalFormSection(answerKeyOpticalFormSection)
            {
                Answers = answerKeyOpticalFormSection.Answers
                    .Select(a => new QuestionAnswer(a.QuestionNo, QuestionAnswer.Empty))
                    .ToList(),
            };
            Sections.Add(section);
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

            Scores.Clear();
            var filteredScoreFormulas = scoreFormulas
                .Where(s => s.Grade == Grade).ToList();

            if (filteredScoreFormulas.Count == 0)
            {
                filteredScoreFormulas = scoreFormulas;
            }

            foreach (var formula in filteredScoreFormulas)
            {
                var score = formula.BasePoint +
                            formula.Coefficients
                                .Select(c => c.Coefficient * Sections.FirstOrDefault(s => s.LessonName == c.Lesson)?.Net ?? 0)
                                .Sum();
                Scores.Add(formula.ScoreName.ToUpper(), (float)Round(score * 100) / 100);
            }
        }

        private float CalculateSuccessPercent()
        {
            if (QuestionCount == 0)
            {
                return 0;
            }

            var percent = Net * 100 / QuestionCount;

            return percent < 0 ? 0 : percent;
        }

        private void SetSubjectDetails()
        {
            StudentSubjectDetails = Sections
                .SelectMany(s => s.Answers, (s, a) => new { s.LessonName, a })
                .GroupBy(x => x.a.SubjectName)
                .Select(x => new StudentSubjectDetail(
                    x.First().LessonName,
                    x.Key,
                    string.Join(", ", x.Select(y => y.a.QuestionNo)),
                    x.Count(y => y.a.Result == QuestionAnswerResult.Correct),
                    x.Count(y => y.a.Result == QuestionAnswerResult.Wrong ||
                                 y.a.Result == QuestionAnswerResult.Invalid),
                    x.Count(y => y.a.Result == QuestionAnswerResult.Empty)))
                .Where(s => !string.IsNullOrEmpty(s.Subject))
                .OrderBy(s => s.Lesson)
                .ThenByDescending(s => s.QuestionNos.Length)
                .ThenByDescending(s => s.SuccessPercent)
                .ToList();
        }
    }
}
