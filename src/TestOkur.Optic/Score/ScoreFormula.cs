namespace TestOkur.Optic.Score
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	[DebuggerDisplay("{ScoreName}")]
	[DataContract]
	public class ScoreFormula
	{
		public ScoreFormula(float basePoint, string scoreName)
		{
			BasePoint = basePoint;
			ScoreName = scoreName;
			Coefficients = new List<LessonCoefficient>();
		}

		public ScoreFormula()
		{
		}

		[DataMember]
		public float BasePoint { get; private set; }

		[DataMember]
		public string ScoreName { get; private set; }

		[DataMember]
		public List<LessonCoefficient> Coefficients { get; set; }
	}
}
