namespace TestOkur.Optic.Form
{
	public abstract class FormLessonSection
	{
		public int LessonId { get; set; }

		public string LessonName { get; set; }

		public int MaxQuestionCount { get; set; }

		public int FormPart { get; set; }

		public int ListOrder { get; set; }
	}
}
