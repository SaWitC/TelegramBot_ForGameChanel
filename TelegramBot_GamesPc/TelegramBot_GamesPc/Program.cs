using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TelegramBot_GamesPc.Data;
using Microsoft.EntityFrameworkCore;
using TelegramBot_GamesPc.Models;
using TelegramBot_GamesPc.Commands;

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
                gl.ImageLink = "https://www.savingcontent.com/wp-content/uploads/2020/08/GunfireReborn-keyart_new.png)";
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

        private static bool IsGamesActive = false;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                try
                {
                    var message = update.Message;
                    UserCommands userCommands = new UserCommands(message, botClient);
                    //wait gameTitle
                    if (!message.Text.Contains("/"))
                    {
                        await userCommands.GameUserCommand(message.Text);
                        IsGamesActive = userCommands.IsGamesActive;
                        return;
                    }
                    // games
                    if (message.Text.ToLower().Contains("/games"))
                    {
                        await userCommands.GameUserCommand();
                        return;
                    }
                    //start
                    if (message.Text.ToLower() == "/start")
                    {
                        await userCommands.StartUserCommand();
                        IsGamesActive = userCommands.IsGamesActive;
                        return;
                    }
                    //info
                    if (message.Text.ToLower() == "/info")
                    {
                        await userCommands.InfoUserCommand();
                        return;
                    }
                    //getid
                    if (message.Text.ToLower() == "/getid")
                    {
                        await userCommands.GetId();
                        return;
                    }
                    //gefault
                    await botClient.SendTextMessageAsync(message.Chat, "Прости но я тебя не понял");
                }
                catch
                {

                }
                //await botClient.SendTextMessageAsync(, "Привет-привет!!");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

    }
}
