using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Careful.Core.Ioc
{
    public sealed class ContainerResolutionErrorCollection : IEnumerable<KeyValuePair<Type, Exception>>
    {
        private readonly List<KeyValuePair<Type, Exception>> _errors = new List<KeyValuePair<Type, Exception>>();

        internal void Add(Type type, Exception exception) =>
            _errors.Add(new KeyValuePair<Type, Exception>(type, exception));

        public IEnumerable<Type> Types => _errors.Select(x => x.Key).Distinct();

        IEnumerator<KeyValuePair<Type, Exception>> IEnumerable<KeyValuePair<Type,Exception>>.GetEnumerator() =>
            _errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            _errors.GetEnumerator();
    }
}
