namespace TestOkur.Optic.Score
{
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

        public string Lesson { get; set; }

        public float Coefficient { get; set; }
	}
}
