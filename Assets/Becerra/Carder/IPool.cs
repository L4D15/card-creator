using System;

namespace Becerra.Carder
{
    public interface IPool<T> : IDisposable
    {
        T Spawn();
        void Despawn(T element);
        void Reset();
    }
}