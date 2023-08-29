using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object.Synchronizing;
using UniRx;

public class SyncReactiveList<T> : IEnumerable<T>, ICollection<T>
{
    private readonly ReactiveCollection<T> _collection = new ReactiveCollection<T>();
    private readonly SyncList<T> _syncList;

    public SyncReactiveList(SyncList<T> list)
    {
        _syncList = list;
        list.OnChange += HandleOnChange;
    }

    public IObservable<CollectionAddEvent<T>> ObserveAdd() =>
        _collection.ObserveAdd();

    public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) =>
        _collection.ObserveCountChanged(notifyCurrentCount);

    public IObservable<CollectionMoveEvent<T>> ObserveMove() =>
        _collection.ObserveMove();

    public IObservable<CollectionRemoveEvent<T>> ObserveRemove() =>
        _collection.ObserveRemove();

    public IObservable<CollectionReplaceEvent<T>> ObserveReplace() =>
        _collection.ObserveReplace();

    public IObservable<Unit> ObserveReset() =>
        _collection.ObserveReset();

    private void HandleOnChange(SyncListOperation op, int index, T oldItem, T newItem, bool asServer)
    {
        if (InstanceFinder.IsServer && InstanceFinder.IsClient && !asServer)
            return;

        switch (op)
        {
            case SyncListOperation.Add:
                _collection.Add(newItem);
                break;
            case SyncListOperation.Insert:
                _collection.Insert(index, newItem);
                break;
            case SyncListOperation.RemoveAt:
                _collection.RemoveAt(index);
                break;
            case SyncListOperation.Clear:
                _collection.Clear();
                break;
            case SyncListOperation.Set:
                _collection[index] = newItem;
                break;
            case SyncListOperation.Complete:
                // Do nothing.
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(op), op, null);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _syncList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _syncList.Add(item);
    }

    public void Clear()
    {
        _syncList.Clear();
    }

    public bool Contains(T item)
    {
        return _syncList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _syncList.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _syncList.Remove(item);
    }

    public int Count => _syncList.Count;
    public bool IsReadOnly => _syncList.IsReadOnly;
}
