using System.Collections.Generic;

namespace PurePrivacy.Core
{
    public class Path
    {
        public const char WindowsPathSeparator = '\\';
        public const char UnixPathSeparator = '/';

        private readonly IEnumerable<string> _pathElements;

        public Path(string path)
        {
            _pathElements = path.Split(WindowsPathSeparator, UnixPathSeparator);
        }

        public override string ToString()
        {
            return string.Join(UnixPathSeparator.ToString(), _pathElements);
        }
    }
}
