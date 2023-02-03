using UnityEngine;

/// <summary>
/// An abstract class that provides base functionalities of a singleton for its derived classes
/// </summary>
/// <typeparam name="T">The type of singleton instance</typeparam>
public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
        set
        {
            if(null == instance)
            {
                instance = value;
                DontDestroyOnLoad(instance.gameObject);
            }
            else if (instance != value)
            {
                Destroy(value.gameObject);
            }
        }
    }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
