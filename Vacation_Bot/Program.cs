using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.IO;
using System.Text;
using System.Linq;
using Pastel;
using System.Drawing;

namespace Vacation_Bot
{
    class Program
    {

        DiscordSocketClient client;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        const string BotPrefix = "!!";
        string ConsolePrefix = "[V]".Pastel("#50c878");

        private async Task MainAsync()
        {

            string HeaderText =
                @"
__      __             _   _               _____  _                       _ 
\ \    / /            | | (_)             |  __ \(_)                     | |
 \ \  / /_ _  ___ __ _| |_ _  ___  _ __   | |  | |_ ___  ___ ___  _ __ __| |
  \ \/ / _` |/ __/ _` | __| |/ _ \| '_ \  | |  | | / __|/ __/ _ \| '__/ _` |
   \  / (_| | (_| (_| | |_| | (_) | | | | | |__| | \__ \ (_| (_) | | | (_| |
    \/ \__,_|\___\__,_|\__|_|\___/|_| |_| |_____/|_|___/\___\___/|_|  \__,_|
    Welcome to bot console! (Created by Meepure)
                ";

            Console.WriteLine(HeaderText.Pastel("#50c878"));

            client = new DiscordSocketClient();
            client.MessageReceived += CommandsHandler;
            client.Log += Log;


            if (!(File.Exists("token.txt")))
            {
                Console.WriteLine("Enter Discord Bot Token Here:");
                var token = Console.ReadLine();

                try
                {
                    StreamWriter sw = new StreamWriter("token.txt", true, Encoding.ASCII);
                    sw.Write(token);
                    sw.Close();

                    Console.WriteLine($"{ConsolePrefix} Token saved to file!\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                    File.Delete("token.txt");
                }
            }

            try
            {

                StreamReader sr = new StreamReader("token.txt");
                string tokenline = sr.ReadLine();
                Console.WriteLine($"{ConsolePrefix} Current Token: {tokenline}");
                Console.WriteLine($"{ConsolePrefix} Bot prefix: \"{BotPrefix}\"\n");

                sr.Close();

                Console.WriteLine($"{ConsolePrefix} Console Log:");

                await client.LoginAsync(TokenType.Bot, tokenline);
                await client.StartAsync();

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandsHandler(SocketMessage msg)
        {
            string MessageAuthor = msg.Author.ToString().Remove(msg.Author.ToString().Length - 5);

            if (!msg.Author.IsBot)

                switch (msg.Content)
                {
                    case BotPrefix + "привет":
                        {
                            msg.Channel.SendMessageAsync($"Привет, " + MessageAuthor);
                            break;
                        }

                    case BotPrefix + "roll":
                        {
                            Random rnd = new Random();
                            int value = rnd.Next(0, 100);
                            msg.Channel.SendMessageAsync($"**{MessageAuthor}** получает число: **{value}**");
                            break;
                        }

                    case BotPrefix + "flip":
                        {
                            Random rnd = new Random();
                            int value = rnd.Next(0, 100);
                            string FlipResult;

                            if (value <= 50)
                                FlipResult = "Орел";
                            else
                                FlipResult = "Решка";

                            msg.Channel.SendMessageAsync($"**{MessageAuthor}** подбрасывает монетку: ***{FlipResult}***");
                            break;
                        }

                }

            return Task.CompletedTask;
        }
    }
}
