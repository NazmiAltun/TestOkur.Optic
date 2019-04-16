namespace TestOkur.Optic.Form
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Score;

	public class AnswerKeyOpticalForm : OpticalForm
	{
		private readonly SortedList<int, AnswerKeyQuestionAnswer> _answers;

		public AnswerKeyOpticalForm()
		{
			_answers = new SortedList<int, AnswerKeyQuestionAnswer>();
		}

		private AnswerKeyOpticalForm(AnswerKeyOpticalForm form, char booklet)
			: base(form, booklet)
		{
			IncorrectEliminationRate = form.IncorrectEliminationRate;
			_answers = new SortedList<int, AnswerKeyQuestionAnswer>();
		}

		public int IncorrectEliminationRate { get; set; }

		public IReadOnlyList<AnswerKeyQuestionAnswer> Answers => _answers.Values.ToList();

		public List<ScoreFormula> ScoreFormulas { get; set; }

		public void AddAnswers(IEnumerable<AnswerKeyQuestionAnswer> answers)
		{
			foreach (var answer in answers)
			{
				AddAnswer(answer);
			}
		}

		public void AddAnswer(AnswerKeyQuestionAnswer value)
		{
			if (value.QuestionNo == 0)
			{
				return;
			}

			_answers.Add((value.LessonOrder * 1000) + value.QuestionNo, value);
		}

		public List<AnswerKeyOpticalForm> Expand()
		{
			var formA = new AnswerKeyOpticalForm(this, 'A');
			var formB = new AnswerKeyOpticalForm(this, 'B');
			var formC = new AnswerKeyOpticalForm(this, 'C');
			var formD = new AnswerKeyOpticalForm(this, 'D');

			foreach (var answer in Answers)
			{
				formA.AddAnswer(new AnswerKeyQuestionAnswer(answer.QuestionNo, answer));
				formB.AddAnswer(new AnswerKeyQuestionAnswer(answer.QuestionNoBookletB, answer));
				formC.AddAnswer(new AnswerKeyQuestionAnswer(answer.QuestionNoBookletC, answer));
				formD.AddAnswer(new AnswerKeyQuestionAnswer(answer.QuestionNoBookletD, answer));
			}

			var list = new List<AnswerKeyOpticalForm>()
			{
				formA,
				formB,
				formC,
				formD
			};

			for (var i = 0; i < list.Count; i++)
			{
				if (list[i].Answers.All(a => a.QuestionNo == default))
				{
					list.RemoveAt(i--);
				}
			}

			return list;
		}
	}
}
