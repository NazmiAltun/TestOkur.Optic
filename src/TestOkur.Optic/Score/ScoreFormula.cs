namespace TestOkur.Optic.Score
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	[DebuggerDisplay("{ScoreName}")]
	[DataContract]
	public class ScoreFormula
	{
		public ScoreFormula(float basePoint, string scoreName, int grade)
		{
			BasePoint = basePoint;
			Grade = grade;
			ScoreName = scoreName.Replace(".", string.Empty);
			Coefficients = new List<LessonCoefficient>();
		}

		[DataMember]
		public float BasePoint { get; private set; }

		[DataMember]
		public string ScoreName { get; private set; }

		[DataMember]
		public int Grade { get; private set; }

		[DataMember]
		public List<LessonCoefficient> Coefficients { get; set; }
	}
}
