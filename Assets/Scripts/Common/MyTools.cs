
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

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

    public static int[] RandomlyGenerateArray (int minLength, int maxLength)
    {
        int randomLength = UnityEngine.Random.Range(minLength, maxLength);

        // generate a array from 0 to maxLength
        int[] numbers = Enumerable.Range(0, maxLength).ToArray();

        // Shuffle an array
        numbers = numbers.OrderBy(x => UnityEngine.Random.value).ToArray();

        return numbers.Skip(0).Take(randomLength).ToArray();
    }

    public static Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
