using System.ComponentModel;
using Exiled.API.Enums;

namespace MusicOnEndRound.Models
{
    public class TrackSettings
    {
        [Description("Включен ли этот трек?")]
        public bool Enabled { get; set; } = true;

        [Description("Исход раунда, при котором играет трек: FacilityForces (МОГ/Наука), ChaosInsurgency (ПХ), Anomalies (СЦП), Draw (Ничья)")]
        public LeadingTeam Outcome { get; set; } = LeadingTeam.Draw;

        [Description("Шанс воспроизведения этого трека среди треков того же исхода (0-100)")]
        public int Chance { get; set; } = 100;

        [Description("Громкость этого трека (0-100)")]
        public float Volume { get; set; } = 50f;

        public TrackSettings() { }

        public TrackSettings(bool enabled, LeadingTeam outcome, int chance, float volume)
        {
            Enabled = enabled;
            Outcome = outcome;
            Chance = chance;
            Volume = volume;
        }
    }
}
