using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FYP.Helper.ConstantManager;

namespace FYP.Models
{
    public class SeedData
    {
        public static void Initialize (IServiceProvider serviceProvider)
        {
            //var context used for storing the db connection
            using (var context = new FYPContext(
                serviceProvider.GetRequiredService<DbContextOptions<FYPContext>>()))
            {
                if(!context.Account.Any())
                {
                    context.Account.AddRange(
                        new Account
                        {

                            Username = "admin",
                            Password = "1234",
                            Name = "a1",
                            Type = SecurityConstants.ACCOUNT_TYPE_ADMIN

                        },
                        new Account
                        {
                            Username = "l",
                            Password = "1234",
                            Name = "l1",
                            Type = SecurityConstants.ACCOUNT_TYPE_LECTURE
                        },
                        new Account
                        {
                            Username = "s",
                            Password = "1234",
                            Name = "s1",
                            Type = SecurityConstants.ACCOUNT_TYPE_STUDENT
                        }
                    );
                    context.SaveChanges();
                }
                context.SaveChanges(); //must include, otherwise no changes will be done!
            }
        }
    }
}
