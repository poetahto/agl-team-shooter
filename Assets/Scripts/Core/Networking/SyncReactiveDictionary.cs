using System;
using FishNet;
using FishNet.Object.Synchronizing;
using UniRx;

/// <summary>
/// A wrapper around a FishNet synced dictionary, that provides observables for common
/// dictionary operations.
/// </summary>
/// <typeparam name="TKey">The key for the dictionary.</typeparam>
/// <typeparam name="TValue">The value for the dictionary.</typeparam>
public class SyncReactiveDictionary<TKey, TValue>
{
    private readonly ReactiveDictionary<TKey, TValue> _reactiveReactiveDictionary = new ReactiveDictionary<TKey, TValue>();
    public IReadOnlyReactiveDictionary<TKey, TValue> ReactiveDictionary => _reactiveReactiveDictionary;

    public SyncReactiveDictionary(SyncIDictionary<TKey, TValue> dictionary)
    {
        dictionary.OnChange += HandleOnChange;
    }

    public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd() =>
        _reactiveReactiveDictionary.ObserveAdd();

    public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) =>
        _reactiveReactiveDictionary.ObserveCountChanged();

    public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove() =>
        _reactiveReactiveDictionary.ObserveRemove();

    public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace() =>
        _reactiveReactiveDictionary.ObserveReplace();

    public IObservable<Unit> ObserveReset() =>
        _reactiveReactiveDictionary.ObserveReset();

    private void HandleOnChange(SyncDictionaryOperation operation, TKey key, TValue value, bool asServer)
    {
        if (InstanceFinder.IsHost && !asServer)
            return;

        switch (operation)
        {
            case SyncDictionaryOperation.Add:
                _reactiveReactiveDictionary.Add(key, value);
                break;
            case SyncDictionaryOperation.Remove:
                _reactiveReactiveDictionary.Remove(key);
                break;
            case SyncDictionaryOperation.Clear:
                _reactiveReactiveDictionary.Clear();
                break;
            case SyncDictionaryOperation.Set:
                _reactiveReactiveDictionary[key] = value;
                break;
            case SyncDictionaryOperation.Complete:
                // Do nothing.
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
        }
    }
}
