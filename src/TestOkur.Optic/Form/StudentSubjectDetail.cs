namespace TestOkur.Optic.Form
{
    using System.Runtime.Serialization;

    [DataContract]
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

        [DataMember]
        public string Lesson { get; internal set; }

        [DataMember]
        public string Subject { get; internal set; }

        [DataMember]
        public string QuestionNos { get; internal set; }

        [DataMember]
        public int CorrectCount { get; internal set; }

        [DataMember]
        public int WrongCount { get; internal set; }

        [DataMember]
        public int EmptyCount { get; internal set; }

        [DataMember]
        public int SuccessPercent { get; internal set; }
    }
}