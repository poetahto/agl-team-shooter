using System;
using FishNet;
using FishNet.Object.Synchronizing;
using UniRx;

/// <summary>
/// A wrapper around a FishNet synced list, that provides observables for common
/// list operations.
/// </summary>
/// <typeparam name="T">The type to be stored in the list.</typeparam>
public class SyncReactiveList<T>
{
    private readonly ReactiveCollection<T> _collection = new ReactiveCollection<T>();
    public IReadOnlyReactiveCollection<T> ReactiveCollection => _collection;

    public SyncReactiveList(SyncList<T> list)
    {
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
        if (InstanceFinder.IsHost && !asServer)
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
}
