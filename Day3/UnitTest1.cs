namespace Day3
{
    public static class ExtensionMethods
    {
        public static int ToPriority(this char input)
        {
            var priority = input - 96;
            if (priority < 0) return priority + 31 + 27;
            return priority;
        }

        public static int SackToPriority(this string input)
        {
            var half = input.Length / 2;
            var firstHalf = new HashSet<char>();
            for (var i = 0; i < input.Length; i++)
            {
                var value = input[i];
                if (i < half)
                {
                    firstHalf.Add(value);
                }
                else
                {
                    if (firstHalf.Contains(value)) return value.ToPriority();
                }
            }

            throw new Exception("sack had no problem");
        }
    }

    public class UnitTest1
    {
        [Theory]
        [InlineData('a', 1)]
        [InlineData('z', 26)]
        [InlineData('A', 27)]
        [InlineData('Z', 52)]
        public void PriorityTest(char input, int expected)
        {
            Assert.Equal(expected, input.ToPriority());
        }

        [Fact]
        public void ParseRuckSackTest()
        {
            const string sack = "vJrwpWtwJgWrhcsFMMfFFhFp";
            Assert.Equal(16, sack.SackToPriority());
        }

        private const string Data =
            "vJrwpWtwJgWrhcsFMMfFFhFp\r\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\r\nPmmdzqPrVvPwwTWBwg\r\nwMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\r\nttgJtRGJQctTZtZT\r\nCrZsJsPPZsGzwwsLwLmpwMDw";

        [Fact]
        public void ParseSackTest()
        {
            Assert.Equal(157, SumOfPriorities(Data));
        }

        private static int SumOfPriorities(string data) =>
            data.Split("\r\n").Select(ExtensionMethods.SackToPriority).Sum();

        [Fact]
        public void Day31()
        {
            var input = File.ReadAllText("input.txt");
            Assert.Equal(8240, SumOfPriorities(input));
        }

        [Fact]
        public void Day32Data()
        {
            Assert.Equal(70, SumOfBadges(Data));
        }

        [Fact]
        public void Day32()
        {
            var input = File.ReadAllText("input.txt");
            Assert.Equal(2587, SumOfBadges(input));
        }

        private static int SumOfBadges(string data) => data.Split("\r\n").Chunk(3).Select(GetBadge).Sum();

        private static int GetBadge(string[] input)
        {
            var first = new HashSet<char>(input[0]);
            var candidates = input[1].Where(i => first.Contains(i)).ToHashSet();
            return input[2].First(i => candidates.Contains(i)).ToPriority();
        }
    }
}