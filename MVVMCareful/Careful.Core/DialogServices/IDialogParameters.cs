using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.DialogServices
{
    public interface IDialogParameters
    {
        void Add(string key, object value);

        bool ContainsKey(string key);

        int Count { get; }

        IEnumerable<string> Keys { get; }

        T GetValue<T>(string key);

        IEnumerable<T> GetValues<T>(string key);

        bool TryGetValue<T>(string key, out T value);
    }
}
