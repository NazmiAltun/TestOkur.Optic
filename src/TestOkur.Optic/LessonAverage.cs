namespace TestOkur.Optic
{
	using System.Runtime.Serialization;

	[DataContract]
	public class LessonAverage
	{
		public LessonAverage(
			string lessonName,
			float classroomAverage,
			float schoolAverage,
			float districtAverage,
			float cityAverage,
			float generalAverage)
		{
			LessonName = lessonName;
			ClassroomAverage = classroomAverage;
			SchoolAverage = schoolAverage;
			DistrictAverage = districtAverage;
			CityAverage = cityAverage;
			GeneralAverage = generalAverage;
		}

		[DataMember]
		public string LessonName { get; private set; }

		[DataMember]
		public float ClassroomAverage { get; private set; }

		[DataMember]
		public float SchoolAverage { get; private set; }

		[DataMember]
		public float DistrictAverage { get; private set; }

		[DataMember]
		public float CityAverage { get; private set; }

		[DataMember]
		public float GeneralAverage { get; private set; }
	}
}
