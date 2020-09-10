using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.AuthorizationManagement
{
    public class DefaultAuthProvider: AuthProvider
    {
        private static List<string> _operations;

        public DefaultAuthProvider(object[] parameters)
        {
            _operations = parameters.Cast<string>().ToList();
        }

        public override bool CheckAccess(string operation)
        {
            if (String.IsNullOrEmpty(operation))
                return false;

            if (_operations != null && _operations.Count > 0)
            {
                return _operations.Any(p => p.ToUpperInvariant() ==
                        operation.ToUpperInvariant());
            }
            return false;
        }

        public override bool CheckAccess(object commandParameter)
        {
            string operation = Convert.ToString(commandParameter);

            return CheckAccess(operation);
        }
    }
}
