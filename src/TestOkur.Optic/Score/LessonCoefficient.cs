namespace TestOkur.Optic.Score
{
	using System.Runtime.Serialization;

	[DataContract]
	public class LessonCoefficient
	{
		public LessonCoefficient(string lesson, float coefficient)
		{
			Lesson = lesson;
			Coefficient = coefficient;
		}

		[DataMember]
		public string Lesson { get; private set; }

		[DataMember]
		public float Coefficient { get; private set; }
	}
}
