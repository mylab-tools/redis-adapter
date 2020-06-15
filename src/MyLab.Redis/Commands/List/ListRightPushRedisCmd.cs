namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// RPUSH redis command
    /// </summary>
    public class ListRightPushRedisCmd : ListItemsRedisCmd
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListRightPushRedisCmd"/>
        /// </summary>
        public ListRightPushRedisCmd(string key) : base("RPUSH", key)
        {
        }
    }
}
