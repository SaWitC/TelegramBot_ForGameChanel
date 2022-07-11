using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram;
using Telegram.Bot;
using TelegramBot_GamesPc.Data;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot_GamesPc.Commands
{
    public class UserCommands
    {
        private static Message _message;
        private static ITelegramBotClient _BotClient;
        public bool IsGamesActive = false;
        public UserCommands(Message message, ITelegramBotClient botClient)
        {
            _message = message;
            _BotClient = botClient;
        }
        public async Task StartUserCommand()
        {
            await _BotClient.SendTextMessageAsync(_message.Chat, "Добро пожаловать на борт, добрый путник!");
            return;
        }

        public async Task GameUserCommand()
        {
            string GameTitle = "";
            GameTitle = _message.Text.Replace("/games", "");
            GameTitle = GameTitle.Trim();

            string Message = "";

            if (!string.IsNullOrEmpty(GameTitle))
            {
                using (AppDBContext context = new AppDBContext())
                {
                    var Game = await context.gameLinkModels.FirstOrDefaultAsync(o => o.Title.ToLower().Contains(GameTitle.ToLower()));
                    if (Game != null)
                    {
                        Message += $"{Game.ImageLink}\n{Game.Title}\n{Game.GameLink}";
                    }
                }
            }
            else
            {
                this.IsGamesActive = true;
                await _BotClient.SendTextMessageAsync(_message.Chat, "Введите название или другую команду");
                return;
            }

            if (!string.IsNullOrEmpty(Message))
            {
                await _BotClient.SendTextMessageAsync(_message.Chat, Message);
                return;
            }
            else
            {
                await _BotClient.SendTextMessageAsync(_message.Chat, "Ничего не нашел (");
                return;
            }
        }

        public async Task GameUserCommand(string Title)
        {
            string GameTitle = Title.Trim();

            string Message = "";

            using (AppDBContext context = new AppDBContext())
            {
                var Game = await context.gameLinkModels.FirstOrDefaultAsync(o => o.Title.ToLower().Contains(GameTitle.ToLower()));
                if (Game != null)
                {
                    Message += $"{Game.ImageLink}\n{Game.Title}\n{Game.GameLink}";
                }
            }

            if (!string.IsNullOrEmpty(Message))
            {
                this.IsGamesActive = false;
                await _BotClient.SendTextMessageAsync(_message.Chat, Message);
                return;
            }
            else
            {
                this.IsGamesActive = false;
                await _BotClient.SendTextMessageAsync(_message.Chat, "Ничего не нашел (");
                return;
            }
        }

        public async Task InfoUserCommand()
        {
            await _BotClient.SendTextMessageAsync(_message.Chat, "о боте\n" +
            "команды\n" +
            "/games название игры(выводит описание и ссылку на игру(если найдет))\n" +
            "/start ничего интересного\n" +
            "/info информация о боте(вы здесь)\n" +
            "/gamesList число до 10(вывоится список найденых игр максимум 10)\n" +
            "это тестовый бот\n" +
            "в будущем возможно расширение функционала.\n");
            return;
        }

        public async Task GetId()
        {
            await _BotClient.SendTextMessageAsync(_message.Chat,_message.Chat.Id.ToString());
            return;
        }

        //public static async Task StartUserCommand()
        //{
        //    await _BotClient.SendTextMessageAsync(_message.Chat, "Добро пожаловать на борт, добрый путник!");
        //    return;
        //}
    }
}
