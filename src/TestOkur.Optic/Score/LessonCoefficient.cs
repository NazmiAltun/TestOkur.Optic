namespace TestOkur.Optic.Score
{
	using System.Runtime.Serialization;

	public class LessonCoefficient
	{
		public LessonCoefficient(string lesson, float coefficient)
		{
			Lesson = lesson;
			Coefficient = coefficient;
		}

		[DataMember]
		public string Lesson { get; }

		[DataMember]
		public float Coefficient { get; }
	}
}
