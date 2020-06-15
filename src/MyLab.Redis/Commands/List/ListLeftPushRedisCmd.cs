namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// LPUSH redis command
    /// </summary>
    public class ListLeftPushRedisCmd : ListItemsRedisCmd
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListRightPushRedisCmd"/>
        /// </summary>
        public ListLeftPushRedisCmd(string key) : base("LPUSH", key)
        {
        }
    }

    
}