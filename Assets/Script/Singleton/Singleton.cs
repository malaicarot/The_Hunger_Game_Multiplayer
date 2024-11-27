using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T _instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    SetupInstance();

                }
            }
            else
            {
                string typeName = typeof(T).Name;
                // Debug.Log($"Singleton {typeName} already created!");
            }
            return instance;

        }
    }

    private static void SetupInstance(){
        if(instance == null){
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }

    }

    private void Awake(){
        RemoveDuplicates();

    }

    private void RemoveDuplicates(){
        if(instance == null){
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }

    }


}
