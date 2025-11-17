using System.ComponentModel;

namespace MusicOnEndRound.Models
{
    public class TrackSettings
    {
        [Description("Включен ли этот трек?")]
        public bool Enabled { get; set; } = true;

        [Description("Шанс воспроизведения этого трека (0-100)")]
        public int Chance { get; set; } = 100;

        [Description("Громкость этого трека (0-100)")]
        public float Volume { get; set; } = 50f;

        public TrackSettings() { }

        public TrackSettings(bool enabled, int chance, float volume)
        {
            Enabled = enabled;
            Chance = chance;
            Volume = volume;
        }
    }
}
