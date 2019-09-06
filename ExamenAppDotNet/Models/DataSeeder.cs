using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenAppDotNet.Models
{
    public class DataSeeder
    {
        public static void Initialize(ExamDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }
        }
    }
}
