namespace TestOkur.Optic.Score
{
	public class LessonCoefficient
	{
		public LessonCoefficient(string lesson, float coefficient)
		{
			Lesson = lesson;
			Coefficient = coefficient;
		}

		public string Lesson { get; }

		public float Coefficient { get; }
	}
}
