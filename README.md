# MyLab.Redis

[![NuGet](https://img.shields.io/nuget/v/MyLab.Redis.svg)](https://www.nuget.org/packages/MyLab.Redis/)

## Обзор

Совместима с платформами `.NET Core 3.1+`.

`MyLab.Redis` предоставляет асинхронную объектную модель для работы с ключами `Redis`, ряд решений на базе этой объектной модели, конфигурирование и интеграцию с использованием механизмов `.NET Core`. Базируется на [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis).

При работе с инструментами `Redis` применяется подход с уклоном в DSL (Domain Specific Language):

 ```C#
await redis.Db().String("foo").SetAsync("bar"); 
 ```

### Пример применения

Интеграция в приложение:

```C#
public class Startup
{
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        //...
        
        serviceCollection
            .AddRedis(RedisConnectionStrategy.Lazy)
            .ConfigureRedis(Configuration);
            
        //...
    }
}
```

Применение:

```C#
class PingService
{
    private readonly IRedisService _redis;

    public PingService(IRedisService redis)
    {
    	_redis = redis;
    }

    public Task<TimeSpan> PingAsync()
    {
    	return _redis.Server().PingAsync();
    }
}
```

## Интеграция

Интеграция осуществляется стандартным способом - добавлением сервисов библиотеки в коллекцию сервисов в методе  `ConfigureService` стартового класса приложения.

```C#
public class Startup
{
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        //...
        
        serviceCollection.AddRedis(RedisConnectionStrategy.Lazy)
            
        //...
    }
}
```

В целевые классы инструменты библиотеки интегрируются через сервис с интерфейсом `MyLab.Redis.IRedisService`.

В методе `AddRedis` требуется указание стратегии подключения к `Redis`  типа `RedisConnectionStrategy`:

* `Lazy` - подключение устанавливается при попытке отправить запрос в `Redis`, если подключение не установлено. Особенности:
  * происодит блокировка потоков в момент проверки состояния подключения и на время процесса подключения;
  * попытка подключения происходит по требованию: если `Redis` не используется, то и подключение не будет установлено;
* `Background` - подключение устанавливается сразу после запуска приложения. Особенности:
  * при неудачах, повторные попытки первичного подключения будут повторяться бесконечно;
  * при разрыве, будут осуществляться бесконечные попытки восстановить подключение;
  * при попытке сделать запрос в `Redis`, если подключение не устновлено, будет выдано исключение `RedisNotConnectedException`.

## Конфигурация

По умолчанию конфигурация загружается из секции `Redis`. Ниже приведён пример указания кастомной секции:

```c#
serviceCollection.ConfigureRedis(Configuration, "CustomSection");
```

 Кроме того, опции настройки сервиса можно определять в коде:

```c#
serviceCollection.ConfigureRedis(opt => opt.Password = "***");
```

Класс настроек:

```C#
/// <summary>
/// Contains Redis configuration
/// </summary>
public class RedisOptions 
{
    /// <summary>
    /// Connection string
    /// </summary>
    /// <remarks>https://stackexchange.github.io/StackExchange.Redis/Configuration</remarks>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Overrides password from <see cref="ConnectionString"/>
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Retry period in seconds when 'background' connection mode
    /// </summary>
    public int BackgroundRetryPeriodSec { get; set; } = 10;

    //...
}
```

Смотрите так же:

* [Конфигурирование кэша](#Конфигурирование-кэша)

## Инструменты сервера

Для выполнения команд уровня сервера, такие как `ECHO`, `PING`, `KEYS`, `SCAN`, получите доступ через метод `Server` сервиса Redis:

```c#
await redis.Server().PingAsync();
```

Метод `Server` предоставляет доступ к серверу по умолчанию - первому в списке серверов в строке подключения. Для выбора конкретного сервера, необходимо воспользоваться перегрузкой этого метода: `Server(EndPoint endPoint)`.

## Инструменты базы данных

Доступ к инструментам уровня баз данных получите через метод `Db` сервиса Redis:

```C#
await redis.Db().String("foo").SetAsync("bar");
```

Метод `Db` предоставляет доступ к базе данных по умолчанию - с индексом 0 или соответствующим значением из строки подключения. Для выбора конкретной бfps данных, необходимо воспользоваться перегрузкой этого метода: `Db(int dbIndex)`.

### Ключи

Команды уровня базы данных для работы с ключами сгруппированы по объектным моделям основных типов ключей. Доступ к этим ключам можно получить через соответствующие методы:

* **String** - `_redis.Db().String(string key)`

* **Hash** - `_redis.Db().Hash(string key)`
* **Set** - `_redis.Db().Set(string key)`
* **SortedSet** - `_redis.Db().SortedSet(string key)`
* **List** - `_redis.Db().List(string key)`

Пример:

```C#
await redis.Db().String("foo").ExpireAsync(TimeSpan.FromMilliseconds(100));
```

### Транзакции

Этот функционал временно недоступен.

### RedisCache 

`RedisCahce` - реализация распределённого кэша на базе `Redis`. 

Для того, чтобы начать работать с кэшем необходимо:

* сконфигурировать кэш
* получить кэш в коде по имени

#### Конфигурирование кэша

Конфигурирование осуществляется через общий узел конфигурации библиотеки - поле `Caching`:

```c#
/// <summary>
/// Contains Redis configuration
/// </summary>
public class RedisOptions 
{
    //...

    /// <summary>
    /// Caching options
    /// </summary>
    public CachingOptions Caching { get; set; }

    //...
}

/// <summary>
/// Contains caching options
/// </summary>
public class CachingOptions
{
    /// <summary>
    /// Gets Redis-key name prefix
    /// </summary>
    public string KeyPrefix { get; set; }

    /// <summary>
    /// Get named cache options
    /// </summary>
    public CacheOptions[] Caches { get; set; }
}

/// <summary>
/// Cache options
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// Cache name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Default expiry for cache items
    /// </summary>
    public string DefaultExpiry { get; set; }
}
```

##### Имя ключа кэша

Имя ключа для хранения кэша формируется из префикса имени ключа `Redis.Caching.KeyPrefix` и имени конкретного кэша `Redis.Caches[].Name` по шаблону `prefix:name`. Или используется только имя, если префикс указан, как пустой. По умолчанию, префикс имеет значение `cache`.

##### Экспирация кэша

Время экспирации указываетя только для конкретного кэша в поле `Redis.Caches[].DefaultExpiry`. Это значение используется по умолчанию для элементов кэша, если  в коде не указано другое значение при добавлении этого элемента.

Значение по умолчанию - 1 мин.

Возможные форматы:

* `int` - число секунд;
* `TimeStemp` - временной интервал. Например `00:00:10` - 10 секунд. [Подробнее тут](https://learn.microsoft.com/en-us/dotnet/api/system.timespan.parse?view=netcore-3.1).

##### Пример конфига кэша

```json
{
    "ConnectionString" : "localhost:9110,allowAdmin=true",
	"Cache": [
        {
            "Name": "test",
            "DefaultExpiry": "24"
        }
    ]
}
```

Смотрите так же: [Конфигурация](#Конфигурация)

#### Применение

##### Добавление элемента `AddAsync`

Добавляет или заменяет значение в кэше, обновляет `TTL` на значение по умолчанию:

```C#
await redis.Db().Cache("test").AddAsync("foo", obj);
```

Можно указать кастомное значение `TTL`:

```C#
await redis.Db().Cache("test").AddAsync("foo", obj, TimeSpan.FromSeconds(5));
```

##### Удаление элемента `RemoveAsync`

Удаляет элемент из кэша:

```C#
await redis.Db().Cache("test").Remove("foo")
```

##### Изменение TTL `UpdateExpiryAsync`

Изменяет время жизни элемента кэша:

```C#
await redis.Db().Cache("test").UpdateExpiryAsync("foo", TimeSpan.FromSeconds(5));
```

##### Попытка получить элемент `TryFetchAsync`

Получает значение из кэша или значение по умолчанию:

```C#
var found = await redis.Db().Cache("test").TryFetchAsync<T>("foo");

if(found != null)
{
    // found
}
else
{
	// not found    
}
```

##### Получение или кэширование `FetchAsync`

Получает значение из кэша или создаёт новый объект, кэширует его и возвращает, как результат:

```C#
var found = await cache.FetchAsync("foo", 
 	() => new CacheItem
    {
    	Id = 1
    });
```

### Redlock

Реализация алгоритма [распределённой блокировки с пирменением Redis](https://redis.io/docs/reference/patterns/distributed-locks/).

Позволяет синхронизировать процессы. При этом состояние блокировки находится в `Redis`. 

В конфигурации приложения объявляются именованные блокировки и объявляются их временные параметры. В коде приложение может воспользоваться блокировкой с указанием её имени. При этом будут использованы соответствующие параметы из конфигурации.

При необходимости синхронизации, приложение может попытаться заблокировать/захватить состояние и таким образом получить возможность выполнить действия, которые требуют синхронизации. Такая попытка может закончиться неудачно, если за отведённое время состояние не освободилось.   

При необходимости длительной блокировки состояния следует периодически продлевать время блокировки. Например, каждую итерацию в задаче, которая выполняет длительный цикл. 

Такая блокировка работает и при использовании в рамках одного процесса. Т.е. в общем случае, если работает несколько экземпляров приложения (процессы), в которых работает несколько потоков, то они могут конкурировать в независимости от того, в каком процессе они находятся. 

#### Конфигурирование блокировок

Конфигурирование осуществляется через общий узел конфигурации библиотеки - поле `Locking`:

```c#
/// <summary>
/// Contains Redis configuration
/// </summary>
public class RedisOptions 
{
	....
        
    /// <summary>
    /// Locking options
    /// </summary>
    public LockingOptions Locking { get; set; }
    
    ....
}

/// <summary>
/// Contains licking options
/// </summary>
public class LockingOptions
{
    /// <summary>
    /// Gets Redis-key name prefix
    /// </summary>
    public string KeyPrefix { get; set; }

    /// <summary>
    /// Gets named lock options
    /// </summary>
    public LockOptions[] Locks { get; set; }
}

/// <summary>
/// Lock options
/// </summary>
public class LockOptions
{
    /// <summary>
    /// Lock name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Determines key expiry
    /// </summary>
    public string Expiry { get; set; }

    /// <summary>
    /// Determines the timeout for a locking attempt
    /// </summary>
    public string DefaultTimeout { get; set; }

    /// <summary>
    /// Determines a waiting period between locking attempts
    /// </summary>
    public string RetryPeriod { get; set; }
}
```

##### Имя ключа блокировки

Имя ключа для организации блокировки формируется из префикса имени ключа `Redis.Locking.KeyPrefix` и имени конкретного кэша `Redis.Locks[].Name` по шаблону `prefix:name`. Или используется только имя, если префикс указан, как пустой. По умолчанию, префикс имеет значение `redlock`.

##### Временные параметры блокировки

Параметры конфиграции:

* `Redis.Locking.Locks[].Expiry` - время жизни ключа, используемого для организации блокировки. Значение по умолчанию 1 мин;
* `Redis.Locking.Locks[].DefaultTimeout` - время, в течении которого происходят попытки блокировки. Используется по умолчанию, если не указано другое значение в коде. Значение по умолчанию - 5 сек;
* `Redis.Locking.Locks[].RetryPeriod` - период повторов попыток блокировки.

Все эти временные параметры могут быть указаны в следующих форматах:

* `int` - число секунд;
* `TimeStemp` - временной интервал. Например `00:00:10` - 10 секунд. [Подробнее тут](https://learn.microsoft.com/en-us/dotnet/api/system.timespan.parse?view=netcore-3.1).

#### Применение блокировок

##### Простая блокировка

```c#
var locker = redis.Db().CreateLocker("foo");

await using var lockAttempt = await locker.TryLockOnceAsync();

if(lockAttempt.Acquired)
{
    // do sync work
}
```

##### Блокировка итеративного процесса

```c#
var locker = redis.Db().CreateLocker("foo");

await using var lockAttempt = await locker.TryLockOnceAsync();

if(lockAttempt.Acquired)
{
	while(/*...*/)
    {
        //do something
        
        await lockAttempt.Lock.ProlongAsync();
    }
}
```

### LUA скрипты

Инструменты для работы со скриптами доступны из объекта БД через метод `Script()`:

```C#
var scripting = redis.Db().Script();
```

#### Выполнение скрипта

Для выполнения скрипта используется DSL-методы сборки и выполнения запроса. DSL выражение начинется с определния целевого скрипта. Целевой скрипт может быть передан в качестве аргумента ([EVAL](https://redis.io/commands/eval)) или предварительно загружен в кэш `Redis` ([EVALSHA](https://redis.io/commands/evalsha)).

Пример указания скрипта по месту:

```C#
redis.Db().Script().Inline("return 10");
```

Пример указания предварительно загруженного к кэш скрипта:

```c#
redis.Db().Script().BySha("eb29cbbc2f5f39dfd02b7b8d3046d9ee7754d966");
```

Остальная часть выражения состоит из необязательных указаний используемых в скрипте ключей и аргументов. Завершает выражение метод выполнения команды.

Пример выполнения скрипта с передачей ключа и аргумента:

```c#
await redis.Db().Script()
    .Inline("return redis.call('set', KEYS[1], ARGV[1])")
    .WithKey(key)
    .WithArgs("foo")
    .EvaluateAsync();
```

#### Загрузка скрипта в кэш

Реализовано с использованием [SCRIPT LOAD](https://redis.io/commands/script-load).

```c#
var sha1 = await redis.Db().Script().LoadAsync("return 10");
```

#### Остановка текущего скрипта

Реализовано с использованием [SCRIPT KILL](https://redis.io/commands/script-kill).

```C#
await redis.Db().Script().KillCurrentAsync();
```

#### Проверка скрипта в кэше

Реализовано с использованием [SCRIPT EXISTS](https://redis.io/commands/script-exists).

Пример для одного аргумента:

```c#
bool exists = await redis.Db().Script().ExistsAsync("eb29cbbc2f5f39dfd02b7b8d3046d9ee7754d966");
```

Пример для проверки нескольких скриптов:

```c#
bool[] exists = await redis.Db().Script().ExistsAsync(
    "eb29cbbc2f5f39dfd02b7b8d3046d9ee7754d966",
    "ed84a94a3ba9d63db7a4255cb8940890ef4a08fc",
    "70a65c9e5784d36b4f42389fa93da5bbe9578be5"
);
```

#### Очистка кэша

Реализовано с использованием [SCRIPT FLUSH SYNC](https://redis.io/commands/script-flush).

```c#
await redis.Db().Script().FlushCacheAsync();
```

### Проверка работоспособности

Проверка работоспособности заключается в проверке наличия подключения к `Redis` и осуществляется через механизм [HealthCheck](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1).

Для подключения проверки необходимо использовать метод `AddRedis` для построителя проверок, как показано на примере ниже:

```c#
services.AddHealthChecks().AddRedis();
```





