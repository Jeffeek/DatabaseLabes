using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace DatabaseLabes.SharedKernel.DI
{
    public static class DependencyInjection
    {
        public static void AddDbContext(this IContainerRegistry container, string? connectionString, bool inMemory)
        {
            container.RegisterScoped<ApplicationDbContext>(() =>
                                                           {
                                                               if (inMemory)
                                                                   return new ApplicationDbContext(SharedDbContextOptions.GetInMemoryOptions());

                                                               return connectionString == null
                                                                          ? new ApplicationDbContext(SharedDbContextOptions.GetOptions())
                                                                          : new ApplicationDbContext(SharedDbContextOptions.GetOptions(connectionString));
                                                           });
        }

        public static void AddDbContextFactory(this IContainerRegistry container, DbContextOptions<ApplicationDbContext> options)
        {
            container.RegisterSingleton<IDbContextFactory<ApplicationDbContext>>(() =>
                                                                                     new DbContextFactory<ApplicationDbContext>((container as
                                                                                                     IContainerExtension)
                                                                                         .CreateServiceProvider(new ServiceCollection()),
                                                                                         options,
                                                                                         new DbContextFactorySource<ApplicationDbContext>()));
        }

        public static IContainerRegistry AddAutoMapper(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMapper>(() => new MapperConfiguration(x => x.AddProfiles(GetAutoMapperProfilesFromAllAssemblies()))
                                                             .CreateMapper());

            return containerRegistry;
        }

        private static IEnumerable<Profile> GetAutoMapperProfilesFromAllAssemblies() =>
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from aType in assembly.GetTypes()
            where aType.IsClass && aType.GetConstructor(Type.EmptyTypes) != null && !aType.IsAbstract && aType.IsSubclassOf(typeof(Profile))
            select (Profile)Activator.CreateInstance(aType);

    }
}
