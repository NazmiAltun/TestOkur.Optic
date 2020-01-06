namespace TestOkur.Optic.Form
{
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

		public Average()
        {
        }

		public string Name { get; set; }

		public float General { get; set; }

		public float City { get; set; }

		public float District { get; set; }

		public float School { get; set; }

		public float Classroom { get; set; }
	}
}
