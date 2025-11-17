using System;
using System.IO;
using System.Linq;
using Exiled.API.Features;
using MusicOnEndRound.EventHandlers;

namespace MusicOnEndRound
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "MusicOnEndRound";
        public override string Author => "Zazar";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 10, 2);

        private ServerHandlers serverHandlers;
        public static Plugin Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            if (!Directory.Exists(Config.MusicFolderPath))
            {
                Directory.CreateDirectory(Config.MusicFolderPath);
                Log.Warn($"Папка с музыкой не найдена. Создана новая папка: {Config.MusicFolderPath}");
            }

            LoadAudioClips();

            serverHandlers = new ServerHandlers();
            Exiled.Events.Handlers.Server.RoundEnded += serverHandlers.OnRoundEnded;

            base.OnEnabled();
        }

        private void LoadAudioClips()
        {
            if (!Directory.Exists(Config.MusicFolderPath))
                return;

            string[] supportedFormats = { "*.ogg", "*.mp3", "*.wav" };
            var musicFiles = supportedFormats
                .SelectMany(format => Directory.GetFiles(Config.MusicFolderPath, format, SearchOption.TopDirectoryOnly))
                .ToArray();

            if (musicFiles.Length == 0)
            {
                Log.Warn($"В папке {Config.MusicFolderPath} не найдено музыкальных файлов");
                return;
            }

            int loadedTracksCount = 0;
            System.Collections.Generic.List<string> newTracks = new System.Collections.Generic.List<string>();

            foreach (string file in musicFiles)
            {
                try
                {
                    string clipName = Path.GetFileNameWithoutExtension(file);

                    if (!Config.Tracks.ContainsKey(clipName))
                    {
                        newTracks.Add(clipName);
                    }

                    AudioClipStorage.LoadClip(file, clipName);
                    loadedTracksCount++;
                    
                    if (Config.Debug)
                        Log.Info($"Загружен аудиоклип: {clipName}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка загрузки аудиоклипа {file}: {ex.Message}");
                }
            }

            Log.Info($"Загружено треков: {loadedTracksCount}/{musicFiles.Length}");

            if (newTracks.Count > 0)
            {
                Log.Warn($"Обнаружено {newTracks.Count} треков без настроек в конфиге:");
                foreach (string track in newTracks)
                {
                    Log.Warn($"  - {track}");
                }
                Log.Warn("Добавьте их в конфиг вручную или они будут использовать настройки по умолчанию.");
                Log.Warn("Пример для конфига:");
                Log.Warn($"  {newTracks[0]}:");
                Log.Warn("    enabled: true");
                Log.Warn("    chance: 100");
                Log.Warn("    volume: 50");
            }
        }

        public Models.TrackSettings GetTrackSettings(string trackName)
        {
            if (Config.Tracks.TryGetValue(trackName, out var settings))
                return settings;

            if (Config.Debug)
                Log.Debug($"Трек {trackName} не найден в конфиге, используются настройки по умолчанию");

            return new Models.TrackSettings(true, 100, 50f);
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= serverHandlers.OnRoundEnded;
            serverHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}
