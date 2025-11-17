using System;
using System.IO;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace MusicOnEndRound.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PlayMusicCommand : ICommand
    {
        public string Command => "playendmusic";
        public string[] Aliases => new[] { "pem" };
        public string Description => "Воспроизводит музыку из папки плагина";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("musiconendround.play"))
            {
                response = "У вас нет прав для использования этой команды";
                return false;
            }

            try
            {
                string folderPath = Plugin.Instance.Config.MusicFolderPath;

                if (!Directory.Exists(folderPath))
                {
                    response = $"Папка с музыкой не существует: {folderPath}";
                    return false;
                }

                string[] supportedFormats = { "*.ogg", "*.mp3", "*.wav" };
                var musicFiles = supportedFormats
                    .SelectMany(format => Directory.GetFiles(folderPath, format, SearchOption.TopDirectoryOnly))
                    .ToArray();

                if (musicFiles.Length == 0)
                {
                    response = "В папке не найдено музыкальных файлов";
                    return false;
                }

                string trackName;
                string selectedFile;
                
                if (arguments.Count > 0)
                {
                    string searchName = string.Join(" ", arguments);
                    selectedFile = musicFiles.FirstOrDefault(f => 
                        Path.GetFileNameWithoutExtension(f).Equals(searchName, StringComparison.OrdinalIgnoreCase) ||
                        Path.GetFileName(f).Equals(searchName, StringComparison.OrdinalIgnoreCase));
                    
                    if (selectedFile == null)
                    {
                        response = $"Трек не найден: {searchName}";
                        return false;
                    }
                    
                    trackName = Path.GetFileNameWithoutExtension(selectedFile);
                }
                else
                {
                    Random random = new Random();
                    selectedFile = musicFiles[random.Next(musicFiles.Length)];
                    trackName = Path.GetFileNameWithoutExtension(selectedFile);
                }

                var trackSettings = Plugin.Instance.GetTrackSettings(trackName);

                float volume = trackSettings.Volume;
                if (volume < 0f) volume = 0f;
                if (volume > 100f) volume = 100f;
                volume = volume / 100f;

                AudioPlayer audioPlayer = AudioPlayer.CreateOrGet("EndRoundMusic", onIntialCreation: (p) =>
                {
                    p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);
                });

                audioPlayer.RemoveAllClips();
                audioPlayer.AddClip(trackName, volume: volume, loop: false, destroyOnEnd: true);

                response = $"Воспроизводится трек: {trackName} (громкость: {trackSettings.Volume})";
                return true;
            }
            catch (Exception ex)
            {
                response = $"Ошибка: {ex.Message}";
                return false;
            }
        }
    }
}
