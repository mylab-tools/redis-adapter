using MyLab.Redis.CommandModel;

namespace MyLab.Redis.Commands.List
{
    /// <summary>
    /// LTRIM redis command
    /// </summary>
    public class ListTrimRedisCmd : ActionRedisCommand
    {
        /// <summary>
        /// Start index
        /// </summary>
        public int StartIndex { get; set; } = 0;
        /// <summary>
        /// End index
        /// </summary>
        public int EndIndex { get; set; } = -1;
        
        /// <inheritdoc />
        public ListTrimRedisCmd(string key) : base((string) "LTRIM", key)
        {
        }
        
        /// <inheritdoc />
        protected override void GetParameters(CommandParameters parameters)
        {
            parameters.Add(StartIndex.ToString());
            parameters.Add(EndIndex.ToString());
        }
    }
}