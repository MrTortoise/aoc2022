using System.ComponentModel;

namespace Day02
{
    public class UnitTest1
    {
        private const string Data = "A Y\r\nB X\r\nC Z";

        public enum RPS
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        public Dictionary<char, RPS> Opponent = new()
        {
            { 'A', RPS.Rock },
            { 'B', RPS.Paper },
            { 'C', RPS.Scissors }
        };

        public Dictionary<char, RPS> Answer = new()
        {
            { 'X', RPS.Rock },
            { 'Y', RPS.Paper },
            { 'Z', RPS.Scissors }
        };

        [Fact]
        public void Test1()
        {
            Assert.Equal(15, Score(Data));
        }

        public record Game(RPS Opponent, RPS Answer);

        static int ScoreGame(Game g)
        {
            if (g.Answer == g.Opponent) return 3;
            if (g.Opponent == RPS.Rock && g.Answer == RPS.Paper) return 6;
            if (g.Opponent == RPS.Paper && g.Answer == RPS.Scissors) return 6;
            if (g.Opponent == RPS.Scissors && g.Answer == RPS.Rock) return 6;

            return 0;
        }

        static int ScoreGameWithShape(Game g)
        {
            return (int)(g.Answer + ScoreGame(g));
        }

        private int Score(string input)
        {
            Game ToGame(string g) => new(Opponent[g[0]], Answer[g[^1]]);


            return input
                .Split("\r\n")
                .Select(ToGame)
                .Select(ScoreGameWithShape)
                .Sum();
        }

        [Fact]
        public void Day21()
        {
            var input = File.ReadAllText("input.txt");
            Assert.Equal(10718, Score(input));
        }

        [Fact]
        public void Day22Example()
        {
            Assert.Equal(12, ApplyStrategy(Data));
        }

        [Fact]
        public void Day22()
        {
            var input = File.ReadAllText("input.txt");
            Assert.Equal(14652, ApplyStrategy(input));
        }

        private Game StrategyToGame(string input)
        {
            var opp = Opponent[input[0]];
            var strat = input[2];
            var move = strat switch
            {
                'X' => opp switch
                {
                    RPS.Paper => RPS.Rock,
                    RPS.Rock => RPS.Scissors,
                    RPS.Scissors => RPS.Paper,
                    _ => throw new ArgumentOutOfRangeException()
                },
                'Y' => opp,
                _ => opp switch
                {
                    RPS.Paper => RPS.Scissors,
                    RPS.Rock => RPS.Paper,
                    RPS.Scissors => RPS.Rock,
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
            return new Game(opp, move);
        }

        private int ApplyStrategy(string data) =>
            data
                .Split("\r\n")
                .Select(StrategyToGame)
                .Select(ScoreGameWithShape)
                .Sum();
    }
}