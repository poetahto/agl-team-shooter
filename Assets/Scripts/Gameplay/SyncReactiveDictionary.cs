using System;
using FishNet.Object.Synchronizing;
using UniRx;

public class SyncReactiveDictionary<TKey, TValue>
{
    private readonly ReactiveDictionary<TKey, TValue> _reactiveDictionary = new ReactiveDictionary<TKey, TValue>();
    private readonly bool _isHost;

    public SyncReactiveDictionary(SyncIDictionary<TKey, TValue> dictionary, bool isHost)
    {
        _isHost = isHost;
        dictionary.OnChange += HandleOnChange;
    }

    public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd() =>
        _reactiveDictionary.ObserveAdd();

    public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) =>
        _reactiveDictionary.ObserveCountChanged();

    public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove() =>
        _reactiveDictionary.ObserveRemove();

    public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace() =>
        _reactiveDictionary.ObserveReplace();

    public IObservable<Unit> ObserveReset() =>
        _reactiveDictionary.ObserveReset();

    private void HandleOnChange(SyncDictionaryOperation operation, TKey key, TValue value, bool asServer)
    {
        // Prevent the host from accidentally handling doubled messages.
        if (_isHost && asServer)
            return;

        switch (operation)
        {
            case SyncDictionaryOperation.Add:
                _reactiveDictionary.Add(key, value);
                break;
            case SyncDictionaryOperation.Remove:
                _reactiveDictionary.Remove(key);
                break;
            case SyncDictionaryOperation.Clear:
                _reactiveDictionary.Clear();
                break;
            case SyncDictionaryOperation.Set:
                _reactiveDictionary[key] = value;
                break;
            case SyncDictionaryOperation.Complete:
                // Do nothing.
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
        }
    }
}
