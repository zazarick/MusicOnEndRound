using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;

namespace MusicOnEndRound.EventHandlers
{
    public class ServerHandlers
    {
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            LeadingTeam outcome = ev.LeadingTeam;

            Timing.CallDelayed(Plugin.Instance.Config.DelayBeforePlay, () =>
            {
                try
                {
                    string musicPath = GetMusicPath(outcome);

                    if (string.IsNullOrEmpty(musicPath))
                    {
                        Log.Warn($"Не удалось найти музыкальный файл для исхода {outcome}");
                        return;
                    }

                    if (Plugin.Instance.Config.Debug)
                        Log.Info($"Исход раунда: {outcome}. Воспроизведение трека: {Path.GetFileName(musicPath)}");

                    PlayMusicForAll(musicPath);
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка при воспроизведении музыки: {ex.Message}");
                }
            });
        }

        private string GetMusicPath(LeadingTeam outcome)
        {
            string folderPath = Plugin.Instance.Config.MusicFolderPath;
            string[] supportedFormats = { "*.ogg", "*.mp3", "*.wav" };
            var allMusicFiles = supportedFormats
                .SelectMany(format => Directory.GetFiles(folderPath, format, SearchOption.TopDirectoryOnly))
                .ToArray();

            if (allMusicFiles.Length == 0)
            {
                Log.Warn("Нет музыкальных файлов для воспроизведения");
                return null;
            }

            var tracksWithSettings = new List<KeyValuePair<string, Models.TrackSettings>>();

            foreach (string file in allMusicFiles)
            {
                string trackName = Path.GetFileNameWithoutExtension(file);
                var settings = Plugin.Instance.GetTrackSettings(trackName);

                if (settings.Enabled && settings.Outcome == outcome)
                {
                    tracksWithSettings.Add(new KeyValuePair<string, Models.TrackSettings>(file, settings));
                }
            }

            if (tracksWithSettings.Count == 0)
            {
                Log.Warn($"Нет включенных треков для исхода {outcome}");
                return null;
            }

            Random random = new Random();
            var selectedTrack = SelectTrackByChance(tracksWithSettings, random);

            return selectedTrack.Key;
        }

        private KeyValuePair<string, Models.TrackSettings> SelectTrackByChance(
            List<KeyValuePair<string, Models.TrackSettings>> tracks,
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
            var trackSettings = Plugin.Instance.GetTrackSettings(trackName);

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
