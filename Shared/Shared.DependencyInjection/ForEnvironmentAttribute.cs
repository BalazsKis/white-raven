using System.Collections.Generic;
using WhiteRaven.Shared.Library.Configuration;

namespace WhiteRaven.Shared.DependencyInjection
{
    public class ForEnvironmentAttribute : System.Attribute
    {
        private readonly HashSet<Environment> _environments;

        public ForEnvironmentAttribute(Environment environment, params Environment[] additionalEnvironments)
        {
            _environments = new HashSet<Environment> { environment };

            foreach (var addEnv in additionalEnvironments)
            {
                if (!_environments.Contains(addEnv))
                {
                    _environments.Add(addEnv);
                }
            }
        }

        public bool IsForEnvironment(Environment environment)
        {
            return _environments.Contains(environment);
        }
    }
}