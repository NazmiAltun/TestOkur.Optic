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

		public LessonCoefficient()
		{
		}

		[DataMember]
		public string Lesson { get; set; }

		[DataMember]
		public float Coefficient { get; set; }
	}
}
