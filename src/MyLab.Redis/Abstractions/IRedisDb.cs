using MyLab.Redis.Scripting;

namespace MyLab.Redis.Abstractions;

public interface IRedisDb
{
    IRedisScriptTools Script();
}