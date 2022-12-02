namespace Day01;

public class UnitTest1
{
    private const string Data = "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000";

    [Fact]
    public void Test1()
    {
        var calories = GetCaloriesMax(Data);
        Assert.Equal(24000, calories);
    }

    private int GetCaloriesMax(string input)
    {
        return GetCalories(input).First();
    }


    private int GetCaloriesTop3(string input)
    {
        return GetCalories(input).Take(3).Sum();
    }

    private static IOrderedEnumerable<int> GetCalories(string input)
    {
        return input.Split("\n\n")
            .Select(elf =>
                elf.Split("\n")
                    .Select(food => food.Trim())
                    .Select(int.Parse).Sum())
            .OrderDescending();
    }

    [Fact]
    public void Day11()
    {
        var input = File.ReadAllText("input.txt");
        Assert.Equal(67450, GetCaloriesMax(input));
    }

    [Fact]
    public void Top3()
    {
        Assert.Equal(45000, GetCaloriesTop3(Data));
    }

    [Fact]
    public void Day22()
    {
        var input = File.ReadAllText("input.txt");
        Assert.Equal(199357, GetCaloriesTop3(input));
    }
}