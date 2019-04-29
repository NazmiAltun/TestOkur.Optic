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
			var answerFormKeyDict = _answerKeyOpticalForms
				.ToDictionary(x => x.Booklet, x => x);

			foreach (var form in forms)
			{
				var answerKeyForm = answerFormKeyDict[form.Booklet];
				form.Evaluate(answerKeyForm.IncorrectEliminationRate, answerKeyForm.ScoreFormulas);
			}

			var netOrderList = new StudentOrderList("NET", forms, f => f.Net);
			var netAverageList = new AverageList("NET", forms, s => s.Net);

			foreach (var form in forms)
			{
				form.AddStudentOrder(netOrderList.GetStudentOrder(form));

				foreach (var section in form.Sections)
				{
					netAverageList.GetClassroomAverage(section.LessonName, form.ClassroomId);
					section.AddLessonAverage(netAverageList.Get(form, section.LessonName));
				}
			}

			return forms;
		}
	}
}
