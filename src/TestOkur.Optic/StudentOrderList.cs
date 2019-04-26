namespace TestOkur.Optic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Form;

	public class StudentOrderList
	{
		private readonly string _orderName;
		private readonly List<StudentOpticalForm> _forms;
		private readonly Func<StudentOpticalForm, float> _selector;
		private List<float> _generalOrderList;
		private Dictionary<int, List<float>> _districtOrderList;
		private Dictionary<int, List<float>> _classroomOrderList;
		private Dictionary<int, List<float>> _schoolOrderList;

		public StudentOrderList(
			string orderName,
			List<StudentOpticalForm> forms,
			Func<StudentOpticalForm, float> selector)
		{
			_forms = forms;
			_selector = selector;
			_orderName = orderName;
			Create();
		}

		public StudentOrder GetStudentOrder(int studentId)
		{
			var form = _forms.First(f => f.StudentId == studentId);

			return new StudentOrder(
				_orderName,
				_classroomOrderList[form.ClassroomId].IndexOf(_selector(form)) + 1,
				_schoolOrderList[form.UserId].IndexOf(_selector(form)) + 1,
				_districtOrderList[form.DistrictId].IndexOf(_selector(form)) + 1,
				_generalOrderList.IndexOf(_selector(form)) + 1);
		}

		private void Create()
		{
			CreateGeneralList();
			CreateDistrictList();
			CreateClassroomList();
			CreateSchoolList();
		}

		private void CreateSchoolList()
		{
			_schoolOrderList = _forms
				.GroupBy(f => f.UserId)
				.ToDictionary(g => g.Key, g =>
					g.Select(_selector)
						.OrderByDescending(x => x)
						.Distinct()
						.ToList());
		}

		private void CreateClassroomList()
		{
			_classroomOrderList = _forms
				.GroupBy(f => f.ClassroomId)
				.ToDictionary(g => g.Key, g =>
					g.Select(_selector)
						.OrderByDescending(x => x)
						.Distinct()
						.ToList());
		}

		private void CreateDistrictList()
		{
			_districtOrderList = _forms
				.GroupBy(f => f.DistrictId)
				.ToDictionary(g => g.Key, g =>
					g.Select(_selector)
						.OrderByDescending(x => x)
						.Distinct()
						.ToList());
		}

		private void CreateGeneralList()
		{
			_generalOrderList = _forms.Select(_selector)
				.OrderByDescending(x => x)
				.Distinct()
				.ToList();
		}
	}
}
