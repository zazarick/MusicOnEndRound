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

            bool configUpdated = false;

            foreach (string file in musicFiles)
            {
                try
                {
                    string clipName = Path.GetFileNameWithoutExtension(file);

                    if (!Config.Tracks.ContainsKey(clipName))
                    {
                        Config.Tracks[clipName] = new Models.TrackSettings(true, 100, 50f);
                        configUpdated = true;
                        
                        if (Config.Debug)
                            Log.Info($"Добавлен новый трек в конфиг: {clipName}");
                    }

                    AudioClipStorage.LoadClip(file, clipName);
                    
                    if (Config.Debug)
                        Log.Info($"Загружен аудиоклип: {clipName}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка загрузки аудиоклипа {file}: {ex.Message}");
                }
            }

            if (configUpdated)
            {
                Log.Info("Обнаружены новые треки. Конфигурация будет обновлена при следующей перезагрузке плагина");
            }
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
