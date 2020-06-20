using UnityEngine;

namespace Simplei18n.Utils
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component
    {
	
        #region Fields
        private static T _instance;

        #endregion

        #region Properties

        protected static T Instance
        {
            get
            {
                if ( _instance == null )
                {
                    GameObject obj = new GameObject ();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T> ();
                }
                return _instance;
            }
        }

        #endregion

        private void Awake ()
        {
            if ( _instance == null )
            {
                _instance = this as T;
                DontDestroyOnLoad ( gameObject );
            }
            else
            {
                Destroy ( gameObject );
            }
        }
    }
}