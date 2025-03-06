using System;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyLab.Redis;
using MyLab.Redis.Connection;
using MyLab.Redis.Options;
using MyLab.Redis.Services;
using Xunit;

namespace UnitTests
{
    public class ServiceCollectionExtensionsBehavior
    {
        private readonly Fixture _fixture = new ();

        [Fact]
        public void ShouldPassWhenOptionsFilledWrite()
        {
            //Arrange
            var cmd = new AutoPropertiesCommand();

            var services = new ServiceCollection()
                .AddRedis(new LazyRedisConnectionPolicy())
                .ConfigureRedis(o => cmd.Execute(o, new SpecimenContext(_fixture)))
                .BuildServiceProvider();

            //Act & Assert
            services.GetRequiredService<IRedisService>();
        }

        [Theory]
        [MemberData(nameof(GetWrongConfigurators))]
        public void ShouldFailWhenOptionsFilledWrong(Action<RedisOptions> configurator)
        {
            //Arrange
            var cmd = new AutoPropertiesCommand();

            var services = new ServiceCollection()
                .AddRedis(new LazyRedisConnectionPolicy())
                .ConfigureRedis(o => cmd.Execute(o, new SpecimenContext(_fixture)))
                .ConfigureRedis(configurator)
                .BuildServiceProvider();

            //Act & Assert
            Assert.Throws<OptionsValidationException> (() => services.GetRequiredService<IRedisService>());
        }

        public static object[][] GetWrongConfigurators()
        {
            return new []
            {
                new object[] { (Action<RedisOptions>)(opt => opt.ConnectionString = null) },
                new object[] { (Action<RedisOptions>)(opt => opt.Caching!.Caches![0].Name = null) },
                new object[] { (Action<RedisOptions>)(opt => opt.Locking!.Locks![0].Name = null) }
            };
        }
    }
}
