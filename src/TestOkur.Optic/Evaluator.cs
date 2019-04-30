namespace TestOkur.Optic
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Form;

	public class Evaluator
	{
		private readonly List<AnswerKeyOpticalForm> _answerKeyOpticalForms;

		public Evaluator(List<AnswerKeyOpticalForm> answerKeyOpticalForms)
		{
			_answerKeyOpticalForms = answerKeyOpticalForms;

			if (_answerKeyOpticalForms.Count == 1)
			{
				_answerKeyOpticalForms = _answerKeyOpticalForms.First().Expand();
			}
		}

		public List<StudentOpticalForm> Evaluate(List<StudentOpticalForm> forms)
		{
			EvaluateForms(forms);
			SetOrdersAndAverages(forms);

			return forms;
		}

		private void SetOrdersAndAverages(IReadOnlyCollection<StudentOpticalForm> forms)
		{
			var orderLists = CreateOrderLists(forms);
			var netAverageList = new AverageList("NET", forms, s => s.Net);

			foreach (var form in forms)
			{
				foreach (var orderList in orderLists)
				{
					form.AddStudentOrder(orderList.GetStudentOrder(form));
				}

				foreach (var section in form.Sections)
				{
					netAverageList.GetClassroomAverage(section.LessonName, form.ClassroomId);
					section.AddLessonAverage(netAverageList.Get(form, section.LessonName));
				}
			}
		}

		private void EvaluateForms(IEnumerable<StudentOpticalForm> forms)
		{
			var answerFormKeyDict = _answerKeyOpticalForms
				.ToDictionary(x => x.Booklet, x => x);

			foreach (var form in forms)
			{
				var answerKeyForm = answerFormKeyDict[form.Booklet];
				form.Evaluate(answerKeyForm.IncorrectEliminationRate, answerKeyForm.ScoreFormulas);
			}
		}

		private List<StudentOrderList> CreateOrderLists(
			IReadOnlyCollection<StudentOpticalForm> forms)
		{
			return _answerKeyOpticalForms.First()
				.ScoreFormulas
				.Select(f => new StudentOrderList(f.ScoreName, forms, s => s.Scores[f.ScoreName]))
				.Concat(new[] { new StudentOrderList("NET", forms, f => f.Net) })
				.ToList();
		}
	}
}
