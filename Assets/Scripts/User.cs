using System;
using SorcererRush.Authentication;

namespace DefaultNamespace
{
    [Serializable]
    public class User
    {
        public AuthenticationType authenticatedWith { get; private set; }
       
    }
}