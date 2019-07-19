using FluentAssertions;
using TestOkur.Optic.Score;
using Xunit;

namespace TestOkur.Optic.Tests
{
	public class ScoreFormulaTests
	{
		[Fact]
		public void DotShouldBeReplacedWithAWhiteSpaceInScoreName()
		{
			var formula = new ScoreFormula(200, "5. Grade");
			formula.ScoreName.Should().Be("5 Grade");
		}
	}
}
