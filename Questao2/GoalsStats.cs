using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2
{
    public static class GoalsStats
    {
        public static void CalculateGoalsByYear(string teamName, int year)
        {
            int totalGoals = ClientHandler.getTotalScoredGoalsAsync(teamName, year, 1).Result + ClientHandler.getTotalScoredGoalsAsync(teamName, year, 2).Result;
            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
        }
    }
}
