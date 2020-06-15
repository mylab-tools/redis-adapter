namespace MyLab.Redis.CommandModel
{
    public interface IRedisScanCmd<T> : IFuncRedisCommand<ScanResult<T>>
    {

    }
}