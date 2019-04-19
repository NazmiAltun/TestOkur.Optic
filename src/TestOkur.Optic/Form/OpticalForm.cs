namespace TestOkur.Optic.Form
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public abstract class OpticalForm
	{
		protected OpticalForm()
		{
			CreateDateTimeUtc = DateTime.UtcNow;
		}

		protected OpticalForm(OpticalForm form, char booklet)
		{
			ExamId = form.ExamId;
			ExamDate = form.ExamDate;
			ExamName = form.ExamName;
			Booklet = booklet;
		}

		public string Id { get; set; }

		public int ExamId { get; set; }

		public DateTime ExamDate { get; set; }

		public string ExamName { get; set; }

		public char Booklet { get; set; }

		public DateTime CreateDateTimeUtc { get; set; }
	}
}
