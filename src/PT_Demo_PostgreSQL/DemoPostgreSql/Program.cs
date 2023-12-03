namespace DemoPostgreSql;

public class Program
{
    public static async Task<int> Main()
    {
        var sqlManager = new SqlManager();

        await sqlManager.CreateTablePersonsAsync();

        await sqlManager.InsertIntoPersonsAsync("Johnny", "Cash", "0", "Burgas", "123 Krum Popov str.");
        await sqlManager.InsertIntoPersonsAsync("Marie", "Curie", "1", "Stara Zagora", "456 Tri Ushi str.");
        await sqlManager.InsertIntoPersonsAsync("Maya", "Papaya", "1", "Sofia", "234 Kosta Vrachanski str.");

        await sqlManager.UpdatePersonsRowAsync();

        var people = await sqlManager.GetAllPersonsAsync();

        await PrintAllPersonEntitiesAsync(people);

        return 0;
    }

    private static async Task PrintAllPersonEntitiesAsync(List<PersonEntity> people)
    {
        foreach (var person in people)
        {
            await Console.Out.WriteLineAsync($"{person.FirstName} {person.LastName}, {person.Gender}, lives in {person.City}, {person.Address}.");
        }
    }
}