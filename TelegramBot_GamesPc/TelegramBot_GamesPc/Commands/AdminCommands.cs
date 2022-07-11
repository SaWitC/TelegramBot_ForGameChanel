using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_GamesPc.Commands
{
    public class AdminCommands
    {

        private static Message _message;
        private static ITelegramBotClient _BotClient;
        public bool IsGamesActive = false;
        public AdminCommands(Message message, ITelegramBotClient botClient)
        {
            _message = message;
            _BotClient = botClient;
        }

        public async Task ActiveAdmin()
        {
            await _BotClient.SendTextMessageAsync(_message.Chat, "Добро пожаловать на борт, добрый путник!");
            return;
        }
    }
}
