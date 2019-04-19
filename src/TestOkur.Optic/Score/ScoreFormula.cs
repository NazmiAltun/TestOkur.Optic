namespace TestOkur.Optic.Score
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	[DebuggerDisplay("{ScoreName}")]
	public class ScoreFormula
	{
		public ScoreFormula(float basePoint, string scoreName)
		{
			BasePoint = basePoint;
			ScoreName = scoreName;
			Coefficients = new List<LessonCoefficient>();
		}

		[DataMember]
		public float BasePoint { get; }

		[DataMember]
		public string ScoreName { get; }

		[DataMember]
		public List<LessonCoefficient> Coefficients { get; set; }
	}
}
