namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Defra.Trade.Plants.Model;
using System;
using System.Linq;

public static class RandomHelper
{
    /// <summary>
    /// Generates a future date in next 5 years.
    /// </summary>
    /// <returns>DateTime.</returns>
    public static DateTime GenerateFutureDate()
    {
        return DateTime.Now.AddDays(
                Faker.RandomNumber.Next(1825))
            .AddHours(Faker.RandomNumber.Next(0, 24))
            .AddMinutes(Faker.RandomNumber.Next(0, 60))
            .AddSeconds(Faker.RandomNumber.Next(0, 60))
            .AddMilliseconds(Faker.RandomNumber.Next(0, 1000));
    }

    public static DateTime GetNextAvailableVisitDate(PlantsContext context)
    {
        var futureDataTime = GenerateFutureDate();
        var i = 0;
        while (context.trd_visitSet.Where(p => p.trd_DateScheduled == futureDataTime).ToList().Count != 0)
        {
            futureDataTime = GenerateFutureDate();
            if (i > 60)
            {
                throw new AutomationException("Unable to find visit time which is not in the system");
            }

            i++;
        }

        return futureDataTime;
    }
}