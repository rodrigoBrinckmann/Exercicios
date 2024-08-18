using Newtonsoft.Json;
using Questao2;

public class Program
{
    public static void Main()
    {        
        GoalsStats.CalculateGoalsByYear("Paris Saint-Germain", 2013);
        GoalsStats.CalculateGoalsByYear("Chelsea", 2014);
    }
}