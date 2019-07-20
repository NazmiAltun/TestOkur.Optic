namespace TestOkur.Optic.Form
{
	using System.Runtime.Serialization;

	[DataContract]
	public class Average
	{
		public Average(string name, float general, float city, float district, float school, float classroom)
		{
			Name = name;
			General = general;
			City = city;
			District = district;
			School = school;
			Classroom = classroom;
		}

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public float General { get; private set; }

		[DataMember]
		public float City { get; private set; }

		[DataMember]
		public float District { get; private set; }

		[DataMember]
		public float School { get; private set; }

		[DataMember]
		public float Classroom { get; private set; }
	}
}
