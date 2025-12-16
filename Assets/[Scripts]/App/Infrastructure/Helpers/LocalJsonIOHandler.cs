using System;
using System.IO;
using UnityEngine;

namespace Serjbal
{
    public class JsonLocalStorage<TData> : IFileIO<TData>
    {
        private readonly TData _defaultValue;
        private TData _currentValue;
        private string _currentFilePath;

        public JsonLocalStorage(TData defaultValue) =>
            _defaultValue = defaultValue;

        public Action OnSaved { get; set; }
        public Action<TData> OnLoaded { get; set; }
        public Action<Exception> OnLoadFiled { get; set; }
        public Action<Exception> OnSaveFiled { get; set; }

        public void Save(string filePath, TData data)
        {
            var directory = Directory.GetParent(filePath);
            if (!Directory.Exists(directory.Name))
                Directory.CreateDirectory(directory.Name);

            try
            {
                var json = JsonUtility.ToJson(data);
                File.WriteAllText(filePath, json);
                OnSaved?.Invoke();
            } catch (Exception ex)
            {
                Debug.LogError($"Saving error: {ex.Message}");
                OnSaveFiled?.Invoke(ex);
            }
        }

        public void Load(string filePath, Action<TData> onLoadedAction)
        {
            var item = Load(filePath);
            onLoadedAction?.Invoke(item);
        }

        public TData Load(string filePath)
        {
            if (_currentValue != null && _currentFilePath == filePath)
            {
                OnLoaded?.Invoke(_currentValue);
                return _currentValue;
            }

            try
            {
                _currentFilePath = filePath;
                var json = File.ReadAllText(filePath);
                var item = JsonUtility.FromJson<TData>(json);
                _currentValue = item;
                OnLoaded?.Invoke(item);
                return item;
            } catch (Exception ex)
            {
                Debug.LogError($"Loading error: {ex.Message}");
                OnLoadFiled?.Invoke(ex);
            }
            return _defaultValue;
        }
    }

    public interface IFileIO<TData>
    {
        Action OnSaved { get; set; }
        Action<TData> OnLoaded { get; set; }
        Action<Exception> OnLoadFiled { get; set; }
        Action<Exception> OnSaveFiled { get; set; }

        void Save(string filePath, TData data);
        void Load(string filePath, Action<TData> onLoadedAction);
        TData Load(string filePath);
    }
}