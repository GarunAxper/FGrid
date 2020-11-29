using System;
using System.Linq;
using System.Threading.Tasks;
using FGrid.Persistence.Models;
using RandomGen;

namespace FGrid.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleData(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                for (var i = 0; i < 1000; i++)
                {
                    await context.Users.AddAsync(new TestUser()
                    {
                        Name = i % 2 == 0 ? Gen.Random.Names.Male()() : Gen.Random.Names.Female()(),
                        Lastname = Gen.Random.Names.Surname()(),
                        Address = new Address()
                        {
                            Country = Gen.Random.Countries()(),
                            ZipCode = Gen.Random.Numbers.Integers(10000, 99999)().ToString(),
                            Street = Gen.Random.Numbers.Integers(10000, 99999)().ToString(),
                        },
                        Age = Gen.Random.Numbers.Integers(10, 80)(),
                        Phone = Gen.Random.PhoneNumbers.WithRandomFormat()(),
                        Gender = Gen.Random.Enum<Gender>()(),
                        Notes = Gen.Random.Text.Short()(),
                        CreationDate = Gen.Random.Time.Dates(DateTime.Now.AddYears(-100), DateTime.Now)()
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}