using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Serjbal
{
    public static class GlobalData
    {
        public enum QualityType { middle, high, ultra }
        public static Action OnSettingsApply;

        //settings
        public static SettingData data = new SettingData();
        internal static bool inGame;

        public static void Load()
        {
            Debug.Log("GlobalData.Load");
            LoadSettings();
        }

        private static void LoadSettings()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "Settings.json");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var loadedData = JsonUtility.FromJson<SettingData>(json);
                data = loadedData;
            } else
            {
                Debug.LogError("Settings.json error");
            }
        }

        public static void Save()
        {
            var json = JsonUtility.ToJson(data, true);
            var path = Path.Combine(Application.streamingAssetsPath, "Settings.json");
            File.WriteAllText(path, json);
        }

        [Serializable]
        public class SettingData
        {
            //settings
            public QualityType qualityType = QualityType.middle;
        }


        private static List<T> ReadJsonFilesFromAssets<T>(string folderPath, string searchPattern = "*.json")
        {
            CleanMacOSMetadataFiles(folderPath);
            string fullPath = Path.Combine(Application.streamingAssetsPath, folderPath);

            if (!Directory.Exists(fullPath))
            {
                Debug.LogError($"Папка не найдена: {fullPath}");
                return null;
            }

            var results = new List<T>();
            var jsonFiles = Directory.GetFiles(fullPath, searchPattern, SearchOption.TopDirectoryOnly);
            var nullEnd = Path.DirectorySeparatorChar + ".json";
            foreach (var filePath in jsonFiles)
            {
                try
                {
                    // Пропускаем .meta файлы
                    if (filePath.EndsWith(".meta")) continue;
                    if (filePath.EndsWith(nullEnd)) continue;

                    string jsonContent = File.ReadAllText(filePath);
                    T deserializedObject = JsonUtility.FromJson<T>(jsonContent);

                    if (deserializedObject != null)
                    {
                        results.Add(deserializedObject);
                    }
                } catch (System.Exception ex)
                {
                    Debug.LogError($"Ошибка при чтении файла {Path.GetFileName(filePath)}: {ex.Message}");
                }
            }
           
            return results;
        }

        private static void CleanMacOSMetadataFiles(string folderPath)
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, folderPath);

            if (!Directory.Exists(fullPath)) return;

            var allFiles = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories);

            foreach (var filePath in allFiles)
            {
                string fileName = Path.GetFileName(filePath);

                if (fileName.StartsWith("._") || fileName == ".DS_Store")
                {
                    try
                    {
                        File.Delete(filePath);
                        Debug.Log($"Удален служебный файл: {fileName}");
                    } catch (Exception ex)
                    {
                        Debug.LogWarning($"Не удалось удалить файл {fileName}: {ex.Message}");
                    }
                }
            }
        }

        private static readonly string[] picExtensions = { ".png", ".jpg", ".bmp" };
        private static readonly string[] soundExtensions = { ".mp3", ".wav" };
        private static readonly string[] videoRxtensions = { ".mp4", ".avi", ".mov", ".webm", ".wmv" };


        public static Texture2D LoadPicFromAssets(string relativePath)
        {
            // Если путь уже содержит расширение, используем как есть
            string existingExtension = Path.GetExtension(relativePath);
            if (!string.IsNullOrEmpty(existingExtension))
            {
                string filePath = Path.Combine(Application.streamingAssetsPath, relativePath);

                if (File.Exists(filePath))
                {
                    return LoadTextureFromFile(filePath, relativePath);
                }
            }

            // Если расширения нет или файл не найден, перебираем возможные расширения
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
            string directory = Path.GetDirectoryName(relativePath);

            foreach (string extension in picExtensions)
            {
                string fileNameWithExtension = fileNameWithoutExtension + extension;
                string fullRelativePath = string.IsNullOrEmpty(directory) ? fileNameWithExtension : Path.Combine(directory, fileNameWithExtension);
                string filePath = Path.Combine(Application.streamingAssetsPath, fullRelativePath);

                if (File.Exists(filePath))
                {
                    return LoadTextureFromFile(filePath, fullRelativePath);
                }
            }

            // Если ни один файл не найден
            Debug.LogError($"Файл не найден: {relativePath} (проверены расширения: {string.Join(", ", picExtensions)})");
            return null;
        }

        private static Texture2D LoadTextureFromFile(string filePath, string relativePath)
        {
            // Проверяем поддерживаемые форматы
            string extension = Path.GetExtension(filePath).ToLower();
            if (!picExtensions.Contains(extension))
            {
                Debug.LogError($"Файл не является поддерживаемым форматом ({string.Join(", ", picExtensions)}): {filePath}");
                return null;
            }

            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);

                if (texture.LoadImage(fileData))
                {
                    texture.name = Path.GetFileNameWithoutExtension(relativePath);
                    Debug.Log($"Текстура загружена: {texture.name} ({texture.width}x{texture.height})");
                    return texture;
                } else
                {
                    Debug.LogError($"Не удалось загрузить текстуру из файла: {filePath}");
                    return null;
                }
            } catch (System.Exception ex)
            {
                Debug.LogError($"Ошибка при загрузке текстуры {filePath}: {ex.Message}");
                return null;
            }
        }


        public static bool CheckVideoInAssets(string basePath)
        {
            // Убираем расширение если оно есть
            string pathWithoutExtension = Path.Combine(
                Path.GetDirectoryName(basePath),
                Path.GetFileNameWithoutExtension(basePath)
            );

            // Проверяем различные расширения
           

            foreach (string ext in videoRxtensions)
            {
                string testPath = pathWithoutExtension + ext;
                string fullPath = Path.Combine(Application.streamingAssetsPath, testPath);

                if (File.Exists(fullPath))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckPicInAssets(string relativePath)
        {
            if (CheckFileInAssets(relativePath))
            {
                return true;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
            string directory = Path.GetDirectoryName(relativePath);

            foreach (string extension in picExtensions)
            {
                string fileNameWithExtension = fileNameWithoutExtension + extension;
                string fullRelativePath = string.IsNullOrEmpty(directory) ? fileNameWithExtension : Path.Combine(directory, fileNameWithExtension);
                string filePath = Path.Combine(Application.streamingAssetsPath, fullRelativePath);

                if (File.Exists(filePath))
                {
                    return true;
                }
            }
            Debug.Log($"Файл не найден: {relativePath}");
            return false;
        }

        public static bool CheckSoundInAssets(string relativePath)
        {
            if (CheckFileInAssets(relativePath))
            {
                return true;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
            string directory = Path.GetDirectoryName(relativePath);

            foreach (string extension in soundExtensions)
            {
                string fileNameWithExtension = fileNameWithoutExtension + extension;
                string fullRelativePath = string.IsNullOrEmpty(directory) ? fileNameWithExtension : Path.Combine(directory, fileNameWithExtension);
                string filePath = Path.Combine(Application.streamingAssetsPath, fullRelativePath);

                if (File.Exists(filePath))
                {
                    return true;
                }
                
            }
            Debug.Log($"Файл не найден: {relativePath}");
            return false;
        }

        public static bool CheckFileInAssets(string path)
        {
            var allPath = Path.Combine(Application.streamingAssetsPath, path);
            bool isExists = File.Exists(allPath);
            return isExists;
        }
    }
}