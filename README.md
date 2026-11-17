# MyLab.Redis

[![NuGet](https://img.shields.io/nuget/v/MyLab.Redis.svg)](https://www.nuget.org/packages/MyLab.Redis/)

## Обзор

Библиотека для `.NET 10`. Даёт асинхронный доступ к ключам Redis, распределённую блокировку, кэш, Lua-скрипты и проверку работоспособности. Основана на [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis).

Команды базы данных собраны в объекты ключей:

```csharp
await redis.Db().String("foo").SetAsync("bar");
```

Точка входа после запуска приложения — `IRedisService`.

## Интеграция

`AddRedis` регистрирует `IRedisService` и фоновую службу, которая подключается к Redis при старте приложения.

```csharp
services
    .AddRedis()
    .ConfigureRedis(Configuration);
```

Настройки по умолчанию читаются из секции `Redis`. Имя секции можно заменить:

```csharp
services.ConfigureRedis(Configuration, "CustomSection");
```

Те же параметры задаются в коде:

```csharp
services.ConfigureRedis(opt =>
{
    opt.ConnectionString = "localhost:6379";
    opt.User = "app";
    opt.Password = "***";
});
```

Если `ConnectionString` не задан, подключение не открывается. При `Optional: true` проверка работоспособности в этом случае остаётся успешной.

Пример исполдьзования:

```csharp
public class PingService(IRedisService redis)
{
    public Task<TimeSpan> PingAsync() => redis.Server().PingAsync();
}
```

## Конфигурация

```json
{
  "Redis": {
    "ConnectionString": "localhost:6379",
    "User": "app",
    "Password": "***",
    "Optional": false,
    "Cache": {
      "ExpirySec": 60,
      "DbIndex": -1,
      "BaseKey": "cache",
      "LockIdPrefix": "cache",
      "Overrides": [
        {
          "Id": "orders",
          "ExpirySec": 300
        }
      ]
    },
    "Redlock": {
      "ExpirySec": 60,
      "LockingTimeoutSec": 5,
      "RetryPeriodSec": 1,
      "BaseKey": "redlock",
      "DbIndex": -1
    }
  }
}
```

Секция по умолчанию называется `Redis`. Если раздела `Cache` или `Redlock` нет, используются значения по умолчанию соответствующих классов.

### `ConnectionString`

Строка подключения [StackExchange.Redis](https://stackexchange.github.io/StackExchange.Redis/Configuration.html): адреса, база по умолчанию, таймауты, `allowAdmin` и остальные параметры клиента. Пример: `localhost:6379,defaultDatabase=0`. Несколько адресов через запятую задают несколько серверов одного подключения.

Если значение не задано, фоновая служба не открывает соединение. При `Optional: false` в журнал пишется ошибка. При `Optional: true` пропуск молчаливый.

### `User`

Имя пользователя ACL. Если задано, подставляется в параметры подключения и заменяет пользователя из `ConnectionString`. Если не задано, остаётся значение из строки подключения.

### `Password`

Пароль ACL. Если задан, заменяет пароль из `ConnectionString`. Если не задан, остаётся значение из строки подключения.

### `Optional`

Обязательность Redis для приложения. По умолчанию `false`.

Проверка работоспособности `redis` успешна, когда соединение установлено. Если соединения нет и `Optional` равен `true`, проверка тоже успешна. Если соединения нет и `Optional` равен `false`, проверка неуспешна.

### `Cache`

Общие настройки кэша. `Cache(cacheId)` берёт этот раздел. Если среди `Overrides` есть элемент с тем же `Id`, к нему применяется правило замены, описанное ниже.

#### `Cache.ExpirySec`

Время жизни записанного элемента в секундах. По умолчанию `0`: ключ хранится без срока, `TtlRemainingAsync` для такого ключа возвращает `TimeSpan.Zero`. Положительное значение передаётся в `SET` как срок `PX`.

#### `Cache.DbIndex`

Номер базы, в которой лежат элементы кэша и по которой `GetItemsCountAsync` считает ключи. По умолчанию `-1`: база из `ConnectionString` (`defaultDatabase`), а если она там не указана — база `0`. Ноль и положительное число выбирают эту базу явно.

#### `Cache.BaseKey`

Префикс ключа элемента. Ключ собирается как `{BaseKey}:{itemId}`. По умолчанию префикс `cache`, поэтому элемент `42` получает ключ `cache:42`.

`GetItemsCountAsync` считает ключи, имя которых начинается с этого префикса.

#### `Cache.LockIdPrefix`

Префикс идентификатора блокировки, которую `SetAsync` берёт перед записью. Идентификатор собирается как `{LockIdPrefix}:{itemId}`. По умолчанию префикс `cache`.

Дальше этот идентификатор проходит через настройки `Redlock`: ключ в Redis будет `{Redlock.BaseKey}:{LockIdPrefix}:{itemId}`. При значениях по умолчанию это `redlock:cache:42`.

#### `Cache.Overrides`

Список настроек отдельных кэшей. У элемента обязательное поле `Id`: оно сравнивается с аргументом `Cache(cacheId)`. Берётся первое совпадение.

Поля элемента те же, что у корневого `Cache`: `ExpirySec`, `DbIndex`, `BaseKey`, `LockIdPrefix`. В результат попадают только три из них, и только когда в корне ещё стоит значение по умолчанию:

* `ExpirySec` берётся из элемента, если в корне `0`;
* `DbIndex` берётся из элемента, если в корне `-1`;
* `BaseKey` берётся из элемента, если в корне `cache`;
* `LockIdPrefix` берётся из элемента, если в корне `cache`.

Если корень уже изменён, остаётся корневое значение, а поле элемента игнорируется. `LockIdPrefix` при замене не переносится: у кэша с совпавшим `Id` префикс блокировки снова `cache`.

Кэш без совпадения в `Overrides` использует корневой раздел целиком, включая заданный там `LockIdPrefix`.

### `Redlock`

Настройки распределённой блокировки. Один набор на всё подключение: и для `Lock(lockId)`, и для блокировки записи в кэш. Если раздела нет, действуют значения по умолчанию.

#### `Redlock.ExpirySec`

Срок ключа блокировки в секундах, если её не сняли и не продлили. По умолчанию `60`. Тот же срок задаёт `ProlongAsync`.

Из срока вычитаются время захвата и допуск на расхождение часов (1% срока плюс 2 мс). Блокировка считается взятой, только если после этого остаток положительный.

#### `Redlock.LockingTimeoutSec`

Сколько секунд `TryAcquireAsync()` без аргумента повторяет захват. По умолчанию `5`. `TryAcquireOnceAsync` это поле не использует: у него одна попытка. `TryAcquireAsync(timeout)` использует переданный предел вместо этого поля.

#### `Redlock.RetryPeriodSec`

Пауза в секундах между неудачными попытками внутри `TryAcquireAsync`. По умолчанию `1`. Если до конца ожидания осталось меньше, пауза сокращается до остатка.

#### `Redlock.BaseKey`

Префикс ключа блокировки. Ключ собирается как `{BaseKey}:{lockId}`. По умолчанию префикс `redlock`, поэтому `Lock("orders:42")` пишет ключ `redlock:orders:42`.

Для записи в кэш `lockId` уже содержит префикс кэша, поэтому ключ получается `{BaseKey}:{LockIdPrefix}:{itemId}`.

#### `Redlock.DbIndex`

Номер базы, в которой создаются ключи блокировки. По умолчанию `-1`: база из `ConnectionString`, иначе база `0`. Ноль и положительное число выбирают эту базу явно. Команды уходят на каждый мастер подключения.

## Сервер

`Server()` возвращает первый сервер из строки подключения. Конкретный сервер выбирается перегрузкой с адресом, `host:port` или парой хост и порт. `GetAllServers()` возвращает все известные серверы.

```csharp
await redis.Server().PingAsync();
await redis.Server("localhost", 6379).EchoAsync("redis");

await foreach (var key in redis.Server().GetKeysAsync("order:*"))
    Console.WriteLine(key);
```

`GetKeysAsync` обходит ключи командой `SCAN`. `pattern` — шаблон glob, `batchSize` — подсказка `COUNT`. `FlushAllAsync` удаляет ключи во всех базах сервера и требует административного подключения.

## База данных и ключи

`Db()` открывает базу по умолчанию. `Db(index)` открывает базу с указанным индексом.

```csharp
await redis.Db().String("foo").SetAsync("bar", TimeSpan.FromMinutes(1));
await redis.Db().Hash("user:1").SetAsync("name", "Ann");
await redis.Db().List("queue").RightPushAsync("job");
await redis.Db().Set("tags").AddAsync("redis");
await redis.Db().SortedSet("rating").AddAsync("ann", 10);
```

Методы соответствуют типам ключей:

* `String` — строка;
* `Hash` — хэш;
* `Set` — множество;
* `SortedSet` — упорядоченное множество;
* `List` — список.

Общие операции ключа доступны на каждом из этих объектов: существование, удаление, тип, срок жизни и время простоя. Срок жизни задаётся через `SetExpirationAsync`.

## Кэш

Кэш берётся у сервиса по идентификатору. Настройки раздела `Cache` и переопределение с тем же `Id` применяются к этому кэшу.

Ключ элемента — `{BaseKey}:{itemId}`. По умолчанию это `cache:{itemId}`. Значение хранится как JSON. Перед записью берётся одноразовая блокировка с идентификатором `{LockIdPrefix}:{itemId}`.

`ExpirySec` задаёт время жизни в секундах. Ноль означает хранение без срока.

```csharp
var orders = redis.Cache("orders");
var entry = orders.Item("42");

var saved = await entry.SetAsync(order);

var found = await entry.GetAsync<Order>();
if (found.Hit)
{
    var order = found.Value;
}

var loaded = await entry.GetAsync<Order>(id => LoadOrderAsync(id));

var ttl = await entry.TtlRemainingAsync();
var removed = await entry.InvalidateAsync();
var count = await orders.GetItemsCountAsync();
```

`GetAsync<T>()` при отсутствии ключа возвращает `Hit = false` и значение по умолчанию. Перегрузка с поставщиком в этом случае получает значение и сохраняет его тем же способом, что `SetAsync`. `Hit` остаётся `false`, если блокировку взять не удалось и значение не записано.

`TtlRemainingAsync` возвращает оставшийся срок. Для отсутствующего ключа и ключа без срока результат — `TimeSpan.Zero`. `GetItemsCountAsync` считает ключи, имя которых начинается с `BaseKey`.

## Блокировка

Блокировка следует алгоритму [Redlock](https://redis.io/docs/latest/develop/use/patterns/distributed-locks/): один и тот же ключ записывается на мастеры подключения и считается взятым, когда его приняло большинство, а оставшееся время жизни положительно.

Ключ блокировки — `{Redlock.BaseKey}:{lockId}`, по умолчанию `redlock:{lockId}`.

`ExpirySec` — срок ключа, если блокировку не сняли и не продлили. По умолчанию 60 секунд. `LockingTimeoutSec` — сколько ждать захват в `TryAcquireAsync()`. По умолчанию 5 секунд. `RetryPeriodSec` — пауза между попытками. По умолчанию 1 секунда.

```csharp
var locker = redis.Lock("orders:42");

await using var lease = await locker.TryAcquireOnceAsync();
if (lease is not null)
{
    while (HasWork())
    {
        await DoWorkAsync();
        await lease.ProlongAsync();
    }
}
```

`TryAcquireOnceAsync` делает одну попытку. `TryAcquireAsync()` повторяет её до `LockingTimeoutSec`. `TryAcquireAsync(timeout)` использует переданный предел. Пустой результат означает, что большинство мастеров блокировку не отдало.

`ProlongAsync` продлевает срок, только если большинство всё ещё хранит токен этой аренды. Освобождение `lease` снимает блокировку на мастерах. `IsAcquiredAsync` проверяет, что токен ещё у большинства.

Для долгого цикла `ExpirySec` должен покрывать одну итерацию между вызовами `ProlongAsync`.

## Lua-скрипты

Скрипты доступны через `Db().Script()`.

Скрипт передаётся текстом (`EVAL`) или по SHA1 уже загруженного скрипта (`EVALSHA`):

```csharp
await redis.Db().Script()
    .Inline("return redis.call('set', KEYS[1], ARGV[1])")
    .WithKey(key)
    .WithArgs("foo")
    .EvaluateAsync();

await redis.Db().Script()
    .BySha("eb29cbbc2f5f39dfd02b7b8d3046d9ee7754d966")
    .WithArgs("foo")
    .EvaluateAsync();
```

Остальные команды кэша скриптов:

```csharp
var sha1 = await redis.Db().Script().LoadAsync("return 10");

bool exists = await redis.Db().Script().ExistsAsync(sha1);

bool[] found = await redis.Db().Script().ExistsAsync(sha1, anotherSha1);

await redis.Db().Script().KillCurrentAsync();
await redis.Db().Script().FlushCacheAsync();
```

`LoadAsync` выполняет `SCRIPT LOAD`. `ExistsAsync` выполняет `SCRIPT EXISTS`. `KillCurrentAsync` выполняет `SCRIPT KILL`. `FlushCacheAsync` выполняет `SCRIPT FLUSH SYNC`.

## Проверка работоспособности

Проверка называется `redis`. Результат успешный, когда подключение установлено. Если подключения нет и `Optional` равен `true`, результат тоже успешный. Иначе проверка неуспешна.

```csharp
services.AddHealthChecks().AddRedis();
```
