using System.Collections.Generic;

public class MySingleton<T> where T : new() {
    static T s_instance;
    static public T instance
    {
        get {
            if (null == s_instance) {
                s_instance = new T();
            }
            return s_instance;
        }
    }
}
