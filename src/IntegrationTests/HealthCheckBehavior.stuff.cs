using MyLab.ApiClient.Test;
using Xunit;

namespace IntegrationTests
{
    public partial class HealthCheckBehavior : IClassFixture<TestApiFixture<>>
    {
        interface IHealthCheck
        {

        }
    }
}