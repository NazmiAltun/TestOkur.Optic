namespace TestOkur.Optic.Form
{
    using System.Collections.Generic;
    using System.Linq;

    public class ScanOutput
    {
        private int _index;

        public ScanOutput(IEnumerable<char> answers, int formPart, int studentNumber, char booklet)
        {
            Answers = answers.ToArray();
            FormPart = formPart;
            StudentNumber = studentNumber;
            Booklet = booklet;
        }

        public ScanOutput()
        {
        }

        public char[] Answers { get; set; }

        public int FormPart { get; set; }

        public int StudentNumber { get; set; }

        public char Booklet { get; set; }

        public char Next()
        {
            return _index == Answers.Length ? ' ' : Answers[_index++];
        }

        public void Skip(int count)
        {
            _index += count;
        }
    }
}
