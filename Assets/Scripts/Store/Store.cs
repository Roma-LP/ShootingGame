using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Store<T, U> : ScriptableObject
{
    [SerializeField, ArrayElementTitle("enumValue")] private StoreElement[] storeElements;

    public StoreElement this[int index]
    {
        get => storeElements[index];
        set => storeElements[index] = value;
    }

    public int Length => storeElements.Length;

    public int GetIndexByEnum(T enumValue) => Array.FindIndex(storeElements, t => t.enumValue.Equals(enumValue));

    [Serializable]
    public class StoreElement
    {
        public T enumValue;
        public U value;
    }
}
