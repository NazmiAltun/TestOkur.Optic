namespace TestOkur.Optic.Form
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Score;

	[DataContract]
	public class AnswerKeyOpticalForm : OpticalForm<AnswerKeyOpticalFormSection>
	{
		public AnswerKeyOpticalForm(char booklet, List<ScoreFormula> scoreFormulas)
			: this()
		{
			Booklet = booklet;
			ScoreFormulas = scoreFormulas;
		}

		public AnswerKeyOpticalForm()
		{
			Sections = new List<AnswerKeyOpticalFormSection>();
		}

		private AnswerKeyOpticalForm(AnswerKeyOpticalForm form, char booklet)
			: base(form, booklet)
		{
			IncorrectEliminationRate = form.IncorrectEliminationRate;
			Sections = new List<AnswerKeyOpticalFormSection>();
		}

		public bool Empty => Sections.SelectMany(s => s.Answers).All(a => a.QuestionNo == default);

		[DataMember]
		public int IncorrectEliminationRate { get; set; }

		[DataMember]
		public override List<AnswerKeyOpticalFormSection> Sections { get; set; }

		public IReadOnlyList<AnswerKeyQuestionAnswer> Answers => Sections.SelectMany(s => s.Answers).ToList();

		[DataMember]
		public List<ScoreFormula> ScoreFormulas { get; set; }

		public void AddSection(AnswerKeyOpticalFormSection section)
		{
			Sections.Add(section);
		}

		public void AddAnswer(AnswerKeyOpticalFormSection section, int questionNo, QuestionAnswer questionAnswer)
		{
			if (!Sections.Contains(section))
			{
				Sections.Add(new AnswerKeyOpticalFormSection(section));
			}

			Sections
				.First(s => s.LessonName == section.LessonName)
				.AddAnswer(new AnswerKeyQuestionAnswer(questionNo, questionAnswer));
		}

		public List<AnswerKeyOpticalForm> Expand()
		{
			var formDictionary = CreateFormsForAllBooklets();

			foreach (var section in Sections)
			{
				foreach (var answer in section.Answers)
				{
					formDictionary['A'].AddAnswer(section, answer.QuestionNo, answer);
					formDictionary['B'].AddAnswer(section, answer.QuestionNoBookletB, answer);
					formDictionary['C'].AddAnswer(section, answer.QuestionNoBookletC, answer);
					formDictionary['D'].AddAnswer(section, answer.QuestionNoBookletD, answer);
				}
			}

			return formDictionary.Values.Where(a => !a.Empty).ToList();
		}

		private Dictionary<char, AnswerKeyOpticalForm> CreateFormsForAllBooklets()
		{
			var formDictionary = new Dictionary<char, AnswerKeyOpticalForm>()
			{
				{ 'A', new AnswerKeyOpticalForm(this, 'A') },
				{ 'B', new AnswerKeyOpticalForm(this, 'B') },
				{ 'C', new AnswerKeyOpticalForm(this, 'C') },
				{ 'D', new AnswerKeyOpticalForm(this, 'D') },
			};
			return formDictionary;
		}
	}
}
