using UnityEngine;

namespace QuickStereoServer
{
      public abstract class ServerConnect : MonoBehaviour
      {
            private string _info;

            protected string Info
            {
                  set { _info = value; }
            }

            public string GetInfo()
            {
                  return _info;      
            }
            public abstract bool Success();
            public abstract void ResetSuccess();
      }
}
