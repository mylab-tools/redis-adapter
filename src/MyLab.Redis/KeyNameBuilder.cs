using System.Text;

namespace MyLab.Redis
{
    class KeyNameBuilder
    {
        private readonly string _name;

        public string Prefix { get; set; }
        public string Suffix { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="KeyNameBuilder"/>
        /// </summary>
        public KeyNameBuilder(string name)
        {
            _name = name;
        }

        public string Build()
        {
            var b = new StringBuilder(_name);

            if (Prefix != null) b.Insert(0, Prefix + ":");
            if (Suffix != null) b.Append(":" + Suffix);

            return b.ToString();
        }
    }
}