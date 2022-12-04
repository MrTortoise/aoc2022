using System.Diagnostics;

namespace Day04
{
    public record ElfRange(int Lower, int Upper);

    public static class Extensions
    {
        public static bool IsPairComparer(this string pair, Func<ElfRange[], bool> comparer)
        {
            return comparer(pair
                .Split(',')
                .Select(p =>
                {
                    var parts = p.Split('-');
                    return new ElfRange(int.Parse(parts[0]), int.Parse(parts[1]));
                })
                .ToArray()
            );
        }
    }

    public class UnitTest1
    {
        public bool FullyContained(ElfRange[] pair)
        {
            var lhs = pair[0];
            var rhs = pair[1];

            if (lhs.Lower == rhs.Lower || lhs.Upper == rhs.Upper) return true;

            if (lhs.Lower <= rhs.Lower)
            {
                return rhs.Upper <= lhs.Upper;
            }
            else
            {
                return lhs.Upper <= rhs.Upper;
            }
        }

        [Theory]
        [InlineData("2-8,3-7", true)] // contains
        [InlineData("3-7,2-8", true)]
        [InlineData("2-3,4-5", false)] // outside
        [InlineData("4-5,2-3", false)]
        [InlineData("5-7,7-9", false)] // overlap high
        [InlineData("7-9,5-7", false)]
        [InlineData("5-7,2-5", false)] // overlap low
        [InlineData("2-5,5-7", false)]
        [InlineData("2-4,6-8", false)] // outside with gap
        [InlineData("6-8,2-4", false)]
        [InlineData("6-6,4-6", true)] // single value top
        [InlineData("4-6,6-6", true)]
        [InlineData("4-4,4-6", true)] // single value bottom
        [InlineData("4-6,4-4", true)]
        [InlineData("4-65,3-4", false)]
        public void TestDataIndividual(string pair, bool expected)
        {
            var result = pair.IsPairComparer(FullyContained);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2-5,7-13", false)]
        [InlineData("5-7,7-13", false)]
        [InlineData("5-10,7-13", false)]
        [InlineData("7-10,7-13", true)]
        [InlineData("5-7,7-9", false)]
        [InlineData("7-13,7-13", true)]
        [InlineData("8-12,7-13", true)]
        [InlineData("8-13,7-13", true)]
        [InlineData("8-15,7-13", false)] // outside with gap
        [InlineData("13-15,7-13", false)]
        [InlineData("14-20,7-13", false)] // single value top
        public void DeliberateTest(string pair, bool expected)
        {
            var result = pair.IsPairComparer(FullyContained);
            Assert.Equal(expected, result);
        }


        private const string Data = "2-4,6-8\r\n2-3,4-5\r\n5-7,7-9\r\n2-8,3-7\r\n6-6,4-6\r\n2-6,4-8";

        [Fact]
        public void TestData()
        {
            int result = Data.Split("\r\n").Select(r => r.IsPairComparer(FullyContained)).Count(i => i);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Day41()
        {
            var input = File.ReadAllText("input.txt");
            int result = input.Split("\r\n").Select(r => r.IsPairComparer(FullyContained)).Count(i => i);
            Assert.Equal(498, result);
        }

        [Theory]
        [InlineData("2-5,7-13", false)]
        [InlineData("5-7,7-13", true)]
        [InlineData("5-10,7-13", true)]
        [InlineData("7-10,7-13", true)]
        [InlineData("5-7,7-9", true)]
        [InlineData("7-13,7-13", true)]
        [InlineData("8-12,7-13", true)]
        [InlineData("8-13,7-13", true)]
        [InlineData("8-15,7-13", true)]
        [InlineData("13-15,7-13", true)]
        [InlineData("14-20,7-13", false)] // single value top
        public void Day42Deliberate(string path, bool expected)
        {
            var result = path.IsPairComparer(Overlap);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Day42Test()
        {
            var result = Data.Split("\r\n").Select(r => r.IsPairComparer(Overlap)).Count(i => i);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Day42Input()
        {
            var input = File.ReadAllText("input.txt");
            var result = input.Split("\r\n").Select(r => r.IsPairComparer(Overlap)).Count(i => i);
            Assert.Equal(859, result);
        }

        private bool Overlap(ElfRange[] arg)
        {
            var lhs = arg[0];
            var rhs = arg[1];
            if (!FullyContained(arg))
            {
                if (lhs.Upper < rhs.Lower || rhs.Upper < lhs.Lower) return false;
                if (lhs.Lower <= rhs.Lower && lhs.Upper <= rhs.Upper) return true;
                if (lhs.Lower >= rhs.Lower && lhs.Upper >= rhs.Upper) return true;

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}