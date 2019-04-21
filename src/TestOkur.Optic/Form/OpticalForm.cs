namespace TestOkur.Optic.Form
{
	using System;
	using System.Collections.Generic;
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

		[DataMember]
		public List<FormLessonSection> Sections { get; set; }

		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public int UserId { get; set; }

		[DataMember]
		public int ExamId { get; set; }

		[DataMember]
		public DateTime ExamDate { get; set; }

		[DataMember]
		public string ExamName { get; set; }

		[DataMember]
		public char Booklet { get; set; }

		[DataMember]
		public DateTime CreateDateTimeUtc { get; set; }
	}
}
