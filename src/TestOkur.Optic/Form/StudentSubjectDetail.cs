namespace TestOkur.Optic.Form
{
    public class StudentSubjectDetail
    {
        public StudentSubjectDetail(string lesson, string subject, string questionNos, int correctCount, int wrongCount, int emptyCount)
        {
            Lesson = lesson;
            Subject = subject;
            QuestionNos = questionNos;
            CorrectCount = correctCount;
            WrongCount = wrongCount;
            EmptyCount = emptyCount;
            var total = correctCount + wrongCount + emptyCount;
            SuccessPercent = total == 0 ? 0 : CorrectCount * 100 / total;
        }

        public StudentSubjectDetail()
        {
        }

        public string Lesson { get; set; }

        public string Subject { get; set; }

        public string QuestionNos { get; set; }

        public int CorrectCount { get; set; }

        public int WrongCount { get; set; }

        public int EmptyCount { get; set; }

        public int SuccessPercent { get; set; }
    }
}