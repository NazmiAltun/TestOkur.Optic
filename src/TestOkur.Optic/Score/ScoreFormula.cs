namespace TestOkur.Optic.Score
{
	using System.Collections.Generic;
	using System.Diagnostics;

	[DebuggerDisplay("{ScoreName}")]
	public class ScoreFormula
	{
		public ScoreFormula(float basePoint, string scoreName)
		{
			BasePoint = basePoint;
			ScoreName = scoreName;
			Coefficients = new List<LessonCoefficient>();
		}

		public float BasePoint { get; }

		public string ScoreName { get; }

		public List<LessonCoefficient> Coefficients { get; set; }
	}
}
