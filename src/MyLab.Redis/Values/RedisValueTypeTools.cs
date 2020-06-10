using System;

namespace MyLab.Redis.Values
{
    public static class RedisValueTypeTools
    {
        public static RedisValueType HeaderByteToRedisType(byte headerByte)
        {
            switch ((char)headerByte)
            {
                case '*': return RedisValueType.Array;
                case '$': return RedisValueType.BulkString;
                case '-': return RedisValueType.Error;
                case ':': return RedisValueType.Integer;
                case '+': return RedisValueType.String;
                default: return RedisValueType.Undefined;
            }
        }

        public static byte RedisTypeToHeaderByte(RedisValueType type)
        {
            switch (type)
            {
                case RedisValueType.String: return (byte) '+';
                case RedisValueType.Integer: return (byte)':';
                case RedisValueType.BulkString: return (byte)'$';
                case RedisValueType.Array: return (byte)'*';
                case RedisValueType.Error: return (byte)'-';
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}