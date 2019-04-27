namespace TestOkur.Optic.Tests.Extensions
{
	using System;
	using System.Linq;

	public static class RandomExtensions
	{
		public static string RandomString(this Random random, int length)
		{
			return RandomString(random, length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
		}

		public static string RandomString(this Random random, int length, string charSet)
		{
			return new string(Enumerable.Repeat(charSet, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
