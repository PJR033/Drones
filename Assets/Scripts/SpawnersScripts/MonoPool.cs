using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> where T : MonoBehaviour
{
    private bool _autoExpand;
    private T _prefab;
    private Transform _container;
    private Queue<T> _pool = new Queue<T>();

    public MonoPool(T prefab, int count, Transform container, bool autoExpand = true)
    {
        _prefab = prefab;
        _container = container;
        _autoExpand = autoExpand;

        CreatePool(count);
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out T element))
        {
            return element;
        }
        else if (_autoExpand)
        {
            T newObject = CreateObject(true);
            return newObject;
        }

        return null;
    }

    public void PutElement(T element)
    {
        element.transform.SetParent(_container);
        element.gameObject.SetActive(false);
        _pool.Enqueue(element);
    }

    private bool HasFreeElement(out T element)
    {
        foreach (T mono in _pool)
        {
            if (mono.gameObject.activeInHierarchy == false)
            {
                element = mono;
                element.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    private void CreatePool(int count)
    {
        _pool = new Queue<T>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isDefaultActive = false)
    {
        T createdObject = Object.Instantiate(_prefab, _container);
        createdObject.gameObject.SetActive(isDefaultActive);
        _pool.Enqueue(createdObject);
        return createdObject;
    }
}