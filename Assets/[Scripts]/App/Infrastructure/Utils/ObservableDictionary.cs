using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Serjbal.Utils
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyDictionaryChanged<TKey, TValue>
    {
        protected IDictionary<TKey, TValue> _dictionaryImplimentation;

        // Конструкторы
        public ObservableDictionary() => _dictionaryImplimentation = new Dictionary<TKey, TValue>();

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) => _dictionaryImplimentation = new Dictionary<TKey, TValue>(dictionary);

        public ObservableDictionary(IEqualityComparer<TKey> comparer) => _dictionaryImplimentation = new Dictionary<TKey, TValue>(comparer);

        public ObservableDictionary(int capacity) => _dictionaryImplimentation = new Dictionary<TKey, TValue>(capacity);

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) => _dictionaryImplimentation = new Dictionary<TKey, TValue>(dictionary, comparer);

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) => _dictionaryImplimentation = new Dictionary<TKey, TValue>(capacity, comparer);

        // Индексатор
        public TValue this[TKey key]
        {
            get => _dictionaryImplimentation[key];
            set
            {
                bool exists = _dictionaryImplimentation.TryGetValue(key, out TValue oldValue);
                _dictionaryImplimentation[key] = value;

                if (exists)
                {
                    // Обновление существующего значения
                    OnDictionaryChanged(new NotifyDictionaryChangedEventArgs<TKey, TValue>(
                        NotifyDictionaryChangedAction.Replace,
                        key: key,
                        newValue: value,
                        oldValue: oldValue));
                } else
                {
                    // Добавление нового значения
                    OnDictionaryChanged(new NotifyDictionaryChangedEventArgs<TKey, TValue>(
                        NotifyDictionaryChangedAction.Add,
                        key: key,
                        newValue: value));
                }

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Values));
            }
        }

        // Коллекции ключей и значений
        public ICollection<TKey> Keys => _dictionaryImplimentation.Keys;
        public ICollection<TValue> Values => _dictionaryImplimentation.Values;

        // Основные свойства
        public int Count => _dictionaryImplimentation.Count;
        public bool IsReadOnly => _dictionaryImplimentation.IsReadOnly;

        // События
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> DictionaryChanged;

        // Методы добавления
        public void Add(TKey key, TValue value)
        {
            _dictionaryImplimentation.Add(key, value);

            OnDictionaryChanged(new NotifyDictionaryChangedEventArgs<TKey, TValue>(
                NotifyDictionaryChangedAction.Add,
                key: key,
                newValue: value));

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        // Методы удаления
        public bool Remove(TKey key)
        {
            if (_dictionaryImplimentation.TryGetValue(key, out TValue value))
            {
                bool removed = _dictionaryImplimentation.Remove(key);
                if (removed)
                {
                    OnDictionaryChanged(new NotifyDictionaryChangedEventArgs<TKey, TValue>(
                        NotifyDictionaryChangedAction.Remove,
                        key: key,
                        oldValue: value));

                    OnPropertyChanged(nameof(Count));
                    OnPropertyChanged(nameof(Keys));
                    OnPropertyChanged(nameof(Values));
                }
                return removed;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool removed = _dictionaryImplimentation.Remove(item);
            if (removed)
            {
                OnDictionaryChanged(new NotifyDictionaryChangedEventArgs<TKey, TValue>(
                    NotifyDictionaryChangedAction.Remove,
                    key: item.Key,
                    oldValue: item.Value));

                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Keys));
                OnPropertyChanged(nameof(Values));
            }
            return removed;
        }

        // Методы проверки
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionaryImplimentation.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionaryImplimentation.ContainsKey(key);
        }

        // Методы получения значений
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionaryImplimentation.TryGetValue(key, out value);
        }

        // Методы очистки
        public void Clear()
        {
            if (_dictionaryImplimentation.Count == 0)
                return;

            var oldItems = _dictionaryImplimentation.ToArray();
            _dictionaryImplimentation.Clear();

            OnDictionaryChanged(new NotifyDictionaryChangedEventArgs<TKey, TValue>(
                NotifyDictionaryChangedAction.Reset,
                oldItems: oldItems));

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        // Методы копирования
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionaryImplimentation.CopyTo(array, arrayIndex);
        }

        // Перечислители
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionaryImplimentation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Дополнительные полезные методы
        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                Add(item.Key, item.Value);
            }
        }

        public TValue GetOrAdd(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                Add(key, value);
                return value;
            }
            return this[key];
        }

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
        {
            if (valueFactory == null)
                throw new ArgumentNullException(nameof(valueFactory));

            if (!ContainsKey(key))
            {
                TValue value = valueFactory();
                Add(key, value);
                return value;
            }
            return this[key];
        }

        // Методы вызова событий
        protected virtual void OnDictionaryChanged(NotifyDictionaryChangedEventArgs<TKey, TValue> e)
        {
            DictionaryChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Безопасные методы для работы из разных потоков
        public void SafeAdd(TKey key, TValue value)
        {
            lock (((ICollection)_dictionaryImplimentation).SyncRoot)
            {
                Add(key, value);
            }
        }

        public bool SafeRemove(TKey key)
        {
            lock (((ICollection)_dictionaryImplimentation).SyncRoot)
            {
                return Remove(key);
            }
        }

        public bool SafeTryGetValue(TKey key, out TValue value)
        {
            lock (((ICollection)_dictionaryImplimentation).SyncRoot)
            {
                return TryGetValue(key, out value);
            }
        }
    }
}