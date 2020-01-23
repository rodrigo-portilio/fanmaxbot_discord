using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace FanMaxBot.Service
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _command;
        private IServiceProvider _service;

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>();

            IConfigurationRoot configuration = builder.Build();
            var token = configuration.GetSection("discord:token").Value;

            _client = new DiscordSocketClient();

            _command = new CommandService();

            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_command)
                .BuildServiceProvider();

            _client.Log += Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await UserJoinAsync();

            await UserLeftAsync();

            await Task.Delay(-1);
        }

        public async Task UserJoinAsync()
        {
            _client.UserJoined += AnnounceJoinedUser;
        }
        public async Task UserLeftAsync()
        {
            _client.UserLeft += AnnounceUserLeft;
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var channel = _client.GetChannel(663473390962606081) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Bem-vindo {user.Username} ao canal"); //Welcomes the new user
            await user.SendMessageAsync($"Olá {user.Username} meu nome é Duffy e sou o bot e mascote do canal FanMax.");
            await user.SendMessageAsync("As lives do canal são na quarta-feira às 21hrs e no sábado às 15hrs");
        }

        public async Task AnnounceUserLeft(SocketGuildUser user)
        {
            var channel = _client.GetChannel(663473390962606081) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Que pena o {user.Username} nos deixou! :("); //Welcomes the new user
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        }

        public async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if(message.Author.IsBot) return;

            var argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _command.ExecuteAsync(context, argPos, _service);
                if(!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
