using System;
using System.IO;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;

namespace MusicOnEndRound.EventHandlers
{
    public class ServerHandlers
    {
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.CallDelayed(Plugin.Instance.Config.DelayBeforePlay, () =>
            {
                try
                {
                    string musicPath = GetMusicPath();
                    
                    if (string.IsNullOrEmpty(musicPath))
                    {
                        Log.Warn("Не удалось найти музыкальный файл для воспроизведения");
                        return;
                    }

                    if (Plugin.Instance.Config.Debug)
                        Log.Info($"Воспроизведение трека: {Path.GetFileName(musicPath)}");

                    PlayMusicForAll(musicPath);
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка при воспроизведении музыки: {ex.Message}");
                }
            });
        }

        private string GetMusicPath()
        {
            var enabledTracks = Plugin.Instance.Config.Tracks
                .Where(t => t.Value.Enabled)
                .ToList();

            if (enabledTracks.Count == 0)
            {
                Log.Warn("Нет включенных треков для воспроизведения");
                return null;
            }

            Random random = new Random();
            var selectedTrack = SelectTrackByChance(enabledTracks, random);

            if (selectedTrack.Key == null)
            {
                Log.Warn("Не удалось выбрать трек по шансам");
                return null;
            }

            string folderPath = Plugin.Instance.Config.MusicFolderPath;
            string[] supportedFormats = { ".ogg", ".mp3", ".wav" };
            
            foreach (string format in supportedFormats)
            {
                string fullPath = Path.Combine(folderPath, selectedTrack.Key + format);
                if (File.Exists(fullPath))
                    return fullPath;
            }

            Log.Warn($"Файл трека не найден: {selectedTrack.Key}");
            return null;
        }

        private System.Collections.Generic.KeyValuePair<string, Models.TrackSettings> SelectTrackByChance(
            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, Models.TrackSettings>> tracks, 
            Random random)
        {
            int totalChance = tracks.Sum(t => t.Value.Chance);
            
            if (totalChance <= 0)
            {
                return tracks[random.Next(tracks.Count)];
            }

            int roll = random.Next(totalChance);
            int currentSum = 0;

            foreach (var track in tracks)
            {
                currentSum += track.Value.Chance;
                if (roll < currentSum)
                    return track;
            }

            return tracks[tracks.Count - 1];
        }

        private void PlayMusicForAll(string filePath)
        {
            string trackName = Path.GetFileNameWithoutExtension(filePath);
            
            if (!Plugin.Instance.Config.Tracks.TryGetValue(trackName, out var trackSettings))
            {
                Log.Warn($"Настройки для трека {trackName} не найдены");
                return;
            }

            float volume = trackSettings.Volume;
            if (volume < 0f) volume = 0f;
            if (volume > 100f) volume = 100f;
            volume = volume / 100f;

            bool loop = Plugin.Instance.Config.LoopMusic;

            AudioPlayer audioPlayer = AudioPlayer.CreateOrGet("EndRoundMusic", onIntialCreation: (p) =>
            {
                p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);
            });

            audioPlayer.RemoveAllClips();
            audioPlayer.AddClip(trackName, volume: volume, loop: loop, destroyOnEnd: !loop);
        }
    }
}
