using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Exiled.API.Interfaces;
using MusicOnEndRound.Models;

namespace MusicOnEndRound
{
    public class Config : IConfig
    {
        [Description("Включен ли плагин?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Показывать ли отладочные сообщения?")]
        public bool Debug { get; set; } = false;

        [Description("Путь к папке с музыкой (относительно папки EXILED)")]
        public string MusicFolderPath { get; set; } = Path.Combine(Exiled.API.Features.Paths.Configs, "MusicOnEndRound");

        [Description("Задержка перед началом воспроизведения музыки (в секундах)")]
        public float DelayBeforePlay { get; set; } = 2f;

        [Description("Зациклить воспроизведение музыки?")]
        public bool LoopMusic { get; set; } = false;

        [Description("Настройки для каждого трека. Ключ - название файла без расширения")]
        public Dictionary<string, TrackSettings> Tracks { get; set; } = new Dictionary<string, TrackSettings>
        {
            ["example_track"] = new TrackSettings(true, 100, 50f)
        };
    }
}
