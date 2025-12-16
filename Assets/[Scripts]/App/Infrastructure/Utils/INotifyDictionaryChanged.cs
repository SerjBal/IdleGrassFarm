using System;
using System.Collections.Generic;


    public interface INotifyDictionaryChanged<TKey, TValue>
    {
        event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> DictionaryChanged;
    }

    public class NotifyDictionaryChangedEventArgs<TKey, TValue> : EventArgs
    {
        public NotifyDictionaryChangedAction Action { get; }
        public TKey Key { get; }
        public TValue NewValue { get; }
        public TValue OldValue { get; }
        public KeyValuePair<TKey, TValue>[] OldItems { get; }

        public NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction action,
            TKey key = default, TValue newValue = default, TValue oldValue = default,
            KeyValuePair<TKey, TValue>[] oldItems = null)
        {
            Action = action;
            Key = key;
            NewValue = newValue;
            OldValue = oldValue;
            OldItems = oldItems;
        }
    }

    public enum NotifyDictionaryChangedAction
    {
        Add,
        Remove,
        Replace,
        Reset
    }
