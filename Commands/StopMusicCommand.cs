using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace MusicOnEndRound.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StopMusicCommand : ICommand
    {
        public string Command => "stopendmusic";
        public string[] Aliases => new[] { "sem" };
        public string Description => "Останавливает музыку в конце раунда";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("musiconendround.stop"))
            {
                response = "У вас нет прав для использования этой команды";
                return false;
            }

            try
            {
                if (AudioPlayer.TryGet("EndRoundMusic", out AudioPlayer player))
                {
                    player.RemoveAllClips();
                    response = "Музыка остановлена";
                    return true;
                }

                response = "Музыка не воспроизводится";
                return false;
            }
            catch (Exception ex)
            {
                response = $"Ошибка: {ex.Message}";
                return false;
            }
        }
    }
}
