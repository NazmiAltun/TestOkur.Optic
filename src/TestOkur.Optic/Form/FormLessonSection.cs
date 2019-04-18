namespace TestOkur.Optic.Form
{
	using System.Runtime.Serialization;

	[DataContract]
	public abstract class FormLessonSection
	{
		[DataMember]
		public int LessonId { get; protected set; }

		[DataMember]
		public string LessonName { get; protected set; }

		[DataMember]
		public int MaxQuestionCount { get; protected set; }

		[DataMember]
		public int FormPart { get; protected set; }

		[DataMember]
		public int ListOrder { get; protected set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (GetType() != obj.GetType())
			{
				return false;
			}

			return obj is FormLessonSection other &&
			       other.LessonName == LessonName;
		}

		public override int GetHashCode()
		{
			return LessonName != null ? LessonName.GetHashCode() : 0;
		}
	}
}
