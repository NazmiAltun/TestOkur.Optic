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

        private ScanOutput()
        {
        }

        public char[] Answers { get; private set; }

        public int FormPart { get; private set; }

        public int StudentNumber { get; private set; }

        public char Booklet { get; private set; }


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
