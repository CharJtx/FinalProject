
using UnityEngine;
using System;
using System.Reflection;

public class MyTools : MonoBehaviour
{
    public static Component[] GetComponentByName(GameObject obj, string componentName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        //Type type = Type.GetType("UnityEngine." + componentName + ", UnityEngine");
        foreach (Assembly assembly in assemblies)
        {
            Type type = assembly.GetType(componentName);
            if (type != null)
            {
                return obj.GetComponentsInChildren(type,true);
            }
        }

        Debug.LogError("Type not found: " + componentName);
        return null;

    }

    public static void CallMethod(Component[] components, string methodName, params object[] parameters)
    {
        foreach (Component component in components)
        {
            Type type = component.GetType();
            MethodInfo method = type.GetMethod(methodName);
            if (method != null)
            {
                method.Invoke(component, parameters);
            }
            else
            {
                Debug.LogError("Method not found: " + methodName);
            }
        }
        
    }
}
