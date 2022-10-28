using System.Collections.Generic;
using UnityEngine;

namespace Becerra.Carder
{
    public class Pool<T> : IPool<T> where T : MonoBehaviour
    {
        #region Definitions

        private class PooledObject
        {
            public T sceneObject;
            public bool isUsed;

            public PooledObject(T sceneObject)
            {
                this.sceneObject = sceneObject;
                this.isUsed = false;
            }
        }

        #endregion

        #region Fields

        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly List<PooledObject> _pooledObjects;

        #endregion

        #region Properties

        public int Size => _pooledObjects.Count;

        #endregion

        #region Constructor

        public Pool(T prefab, Transform parent, int initialSize)
        {
            _prefab = prefab;
            _parent = parent;
            _pooledObjects = new List<PooledObject>(initialSize);

            var children = parent.GetComponentsInChildren<T>(true);

            foreach (var child in children)
            {
                Register(child);
            }

            for (int i = Size; i < initialSize; i++)
            {
                Expand();
            }
        }

        #endregion

        #region Methods
        public T Spawn()
        {
            PooledObject pooledObject;

            pooledObject = FindAvailable();

            if (IsValid(pooledObject) == false)
            {
                pooledObject = Expand();
            }

            pooledObject.isUsed = true;

            pooledObject.sceneObject.gameObject.SetActive(true);

            return pooledObject.sceneObject;
        }

        public void Despawn(T sceneObject)
        {
            PooledObject pooledObject;

            pooledObject = Find(sceneObject);

            if (IsValid(pooledObject))
            {
                pooledObject.isUsed = false;
                pooledObject.sceneObject.transform.SetParent(_parent);
                pooledObject.sceneObject.gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            foreach (var pooledObject in _pooledObjects)
            {
                if (pooledObject.isUsed)
                {
                    Despawn(pooledObject.sceneObject);
                }
            }
        }

        public void Dispose()
        {
            foreach (var pooledObject in _pooledObjects)
            {
                GameObject.Destroy(pooledObject.sceneObject.gameObject);
            }

            _pooledObjects.Clear();
        }

        private PooledObject Expand()
        {
            T sceneObject;
            PooledObject pooledObject;

            sceneObject = GameObject.Instantiate<T>(_prefab, _parent);
            pooledObject = Register(sceneObject);

            return pooledObject;
        }

        private PooledObject Register(T sceneObject)
        {
            PooledObject pooledObject;

            pooledObject = new PooledObject(sceneObject);

            pooledObject.sceneObject.gameObject.SetActive(false);

            _pooledObjects.Add(pooledObject);

            return pooledObject;
        }

        private PooledObject FindAvailable()
        {
            foreach (var pooledObject in _pooledObjects)
            {
                if (pooledObject.isUsed == false) return pooledObject;
            }

            return default;
        }

        private PooledObject Find(T sceneObject)
        {
            foreach (var pooledObject in _pooledObjects)
            {
                if (pooledObject.sceneObject == sceneObject) return pooledObject;
            }

            return null;
        }

        private bool IsValid(PooledObject pooledObject)
        {
            if (pooledObject == null) return false;
            if (pooledObject.sceneObject == null) return false;

            return true;
        }

        #endregion
    }
}