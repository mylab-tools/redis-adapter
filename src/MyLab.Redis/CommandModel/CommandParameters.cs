using System.Collections.Generic;

namespace MyLab.Redis.CommandModel
{
    /// <summary>
    /// Containes command parameters 
    /// </summary>
    public class CommandParameters
    {
        readonly List<string> _params = new List<string>();

        /// <summary>
        /// Adds required param
        /// </summary>
        public void Add(string value)
        {
            _params.Add(value);
        }

        /// <summary>
        /// Adds required param with name
        /// </summary>
        public void Add(string name, string value)
        {
            _params.Add(name);
            _params.Add(value);
        }

        /// <summary>
        /// Ads many params
        /// </summary>
        public void AddRange(IEnumerable<string> enumerable)
        {
            _params.AddRange(enumerable);
        }

        /// <summary>
        /// Ad optional param
        /// </summary>
        public void AddOptional(string name, string value)
        {
            if (value != null)
            {
                _params.Add(name);
                _params.Add(value);
            }
        }

        /// <summary>
        /// Provides parameters as array
        /// </summary>
        public string[] ToArray()
        {
            return _params.ToArray();
        }
    }
}