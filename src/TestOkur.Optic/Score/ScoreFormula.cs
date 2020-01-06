namespace TestOkur.Optic.Score
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{ScoreName}")]
    public class ScoreFormula
    {
        public ScoreFormula(float basePoint, string scoreName, int grade)
        {
            BasePoint = basePoint;
            Grade = grade;
            ScoreName = scoreName.Replace(".", string.Empty);
            Coefficients = new List<LessonCoefficient>();
        }

        public ScoreFormula()
        {
        }

        public float BasePoint { get; set; }

        public string ScoreName { get; set; }

        public int Grade { get; set; }

        public List<LessonCoefficient> Coefficients { get; set; }
    }
}
