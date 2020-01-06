namespace TestOkur.Optic.Form
{
	using System;

	public abstract class OpticalForm
	{
        protected OpticalForm()
		{
			CreateDateTimeUtc = DateTime.UtcNow;
		}

        protected OpticalForm(OpticalForm form, char booklet)
		{
			ExamId = form.ExamId;
			UserId = form.UserId;
			SchoolId = form.SchoolId;
			ExamDate = form.ExamDate;
			ExamName = form.ExamName;
			Booklet = booklet;
		}

        public string Id { get; set; }

        public string UserId { get; set; }

        public int SchoolId { get; set; }

        public int ExamId { get; set; }

        public DateTime ExamDate { get; set; }

        public string ExamName { get; set; }

        public string ExamTypeName { get; set; }

        public char Booklet { get; set; }

        public DateTime CreateDateTimeUtc { get; set; }
	}
}
