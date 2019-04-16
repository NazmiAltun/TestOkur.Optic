namespace TestOkur.Optic.Score
{
	using System.Collections.Generic;
	using System.Diagnostics;

	[DebuggerDisplay("{FormulaType}-{ScoreName}")]
	public class ScoreFormula
	{
		public ScoreFormula()
		{
			Coefficients = new List<LessonCoefficient>();
		}

		public int Id { get; set; }

		public float BasePoint { get; set; }

		public int Grade { get; set; }

		public int FormulaTypeId { get; set; }

		public string FormulaType { get; set; }

		public string ScoreName { get; set; }

		public int ExamId { get; set; }

		public List<LessonCoefficient> Coefficients { get; set; }
	}
}
