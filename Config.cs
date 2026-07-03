using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Exiled.API.Enums;
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

        [Description("Настройки для каждого трека. Ключ - название файла без расширения. Для каждого исхода раунда можно задать несколько вариаций (например, по 2). Outcome: FacilityForces (МОГ/Наука), ChaosInsurgency (ПХ), Anomalies (СЦП), Draw (Ничья). Треки не указанные здесь по умолчанию считаются треками исхода Draw (enabled: true, chance: 100, volume: 50)")]
        public Dictionary<string, TrackSettings> Tracks { get; set; } = new Dictionary<string, TrackSettings>
        {
            ["mtf_win_1"] = new TrackSettings(true, LeadingTeam.FacilityForces, 50, 50f),
            ["mtf_win_2"] = new TrackSettings(true, LeadingTeam.FacilityForces, 50, 50f),

            ["chaos_win_1"] = new TrackSettings(true, LeadingTeam.ChaosInsurgency, 50, 50f),
            ["chaos_win_2"] = new TrackSettings(true, LeadingTeam.ChaosInsurgency, 50, 50f),

            ["scp_win_1"] = new TrackSettings(true, LeadingTeam.Anomalies, 50, 50f),
            ["scp_win_2"] = new TrackSettings(true, LeadingTeam.Anomalies, 50, 50f),

            ["draw_1"] = new TrackSettings(true, LeadingTeam.Draw, 50, 50f),
            ["draw_2"] = new TrackSettings(true, LeadingTeam.Draw, 50, 50f)
        };
    }
}
