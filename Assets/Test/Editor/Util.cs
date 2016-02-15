using NUnit.Framework;
using Kuchen.Util;

namespace Kuchen.Test
{
	public class PatternMatchTest
	{
		[Test]
		public void 文字列()
		{
			Assert.IsTrue(PatternMatch.IsMatch("abcde", "abcde"));
			
			Assert.IsFalse(PatternMatch.IsMatch("abcde", "fghijk"));
			Assert.IsFalse(PatternMatch.IsMatch("abcd", "abcde"));
			Assert.IsFalse(PatternMatch.IsMatch("abcde", "abcd"));
		}
		
		[Test]
		public void はてな()
		{
			Assert.IsTrue(PatternMatch.IsMatch("abcde?", "abcdef"));
			Assert.IsTrue(PatternMatch.IsMatch("abc?ef", "abcdef"));
			Assert.IsTrue(PatternMatch.IsMatch("?????", "12345"));
			
			Assert.IsFalse(PatternMatch.IsMatch("abc?ef", "abccdef"));
			Assert.IsFalse(PatternMatch.IsMatch("?????", "123456"));
		}
		
		[Test]
		public void あすたりすく()
		{
			Assert.IsTrue(PatternMatch.IsMatch("abcde*", "abcdef"));
			Assert.IsTrue(PatternMatch.IsMatch("*def*", "abcdefghi"));
			Assert.IsTrue(PatternMatch.IsMatch("*abc", "abc"));
			Assert.IsTrue(PatternMatch.IsMatch("*", "123456"));
			
			Assert.IsFalse(PatternMatch.IsMatch("*abc", "bc"));
			Assert.IsFalse(PatternMatch.IsMatch("*abc*", "ac"));
			Assert.IsFalse(PatternMatch.IsMatch("*abc*", "ab"));
		}

	}
}
