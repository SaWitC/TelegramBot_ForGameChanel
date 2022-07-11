using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TelegramBot_GamesPc.Data;
using Microsoft.EntityFrameworkCore;
using TelegramBot_GamesPc.Models;

namespace TelegramBot_GamesPc
{
    class Program
    {
        static ITelegramBotClient telegramBot = new TelegramBotClient("5580926756:AAESR41QQ3Ea0NTn3DMmANj17wuBqVMPvwU");
        static void Main(string[] args)
        {
            using (AppDBContext context = new AppDBContext())
            {
                var gl = new GameLinkModel();
                gl.Description = "1111";
                gl.Title = "Gunfire Reborn";
                gl.GameLink="https://t.me/c/1272975293/2";
                context.Add(gl);
                Console.WriteLine("Done");
                context.SaveChanges();
            }
                Console.WriteLine("Hello World!");

            Console.WriteLine("Запущен бот " + telegramBot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            telegramBot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower().Contains("/games"))
                {
                    //await botClient.SendTextMessageAsync(message.Chat, "Ищу");

                    string GameTitle = "";
                    GameTitle = message.Text.Replace("/games","");
                    GameTitle=GameTitle.Trim();

                    string Message = "";

                    if (!string.IsNullOrEmpty(GameTitle))
                    {
                        using (AppDBContext context = new AppDBContext())
                        {
                            var Game = await context.gameLinkModels.FirstOrDefaultAsync(o => o.Title.ToLower().Contains(GameTitle.ToLower()));
                            if (Game != null)
                            {
                                Message += $"{Game.Title}\n{Game.GameLink}";
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(Message))
                    {
                        await botClient.SendTextMessageAsync(message.Chat,Message);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Ничего не нашел(");
                    }

                   

                    
                }
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Прости но я тебя не понял");
                //await botClient.SendTextMessageAsync(, "Привет-привет!!");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

    }
}
