namespace TestOkur.Optic.Form
{
	public class StudentOrder
	{
		public StudentOrder(string name, int classroomOrder, int schoolOrder, int districtOrder, int cityOrder, int generalOrder)
		{
			Name = name;
			ClassroomOrder = classroomOrder;
			SchoolOrder = schoolOrder;
			DistrictOrder = districtOrder;
			CityOrder = cityOrder;
			GeneralOrder = generalOrder;
		}

		public StudentOrder()
        {
        }

		public string Name { get; set; }

		public int ClassroomOrder { get; set; }

		public int SchoolOrder { get; set; }

		public int DistrictOrder { get; set; }

		public int CityOrder { get; set; }

		public int GeneralOrder { get; set; }
	}
}
