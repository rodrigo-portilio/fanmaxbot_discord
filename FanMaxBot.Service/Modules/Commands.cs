using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FanMaxBot.Service.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            IUser user = Context.User;
            await ReplyAsync($"Olá {user.Username} meu nome é Duffy e sou o bot e mascote do canal estou aqui para te ajudar.");
            await ReplyAsync("Digite !github para conhecer o repositorio do canal.");
            await ReplyAsync("Digite !live para saber os dias das lives.");
            await ReplyAsync("Digite !codigo para saber aonde está meu código.");
        }

        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        [Command("codigo")]
        public async Task Codigo()
        {
            await ReplyAsync("Meu código está no github caso queira me melhorar ou aprender como eu fui criado.");
            await ReplyAsync("https://github.com/fanmax/fanmaxbot_discord");
        }

        [Command("github")]
        public async Task Github()
        {
            await ReplyAsync("https://github.com/fanmax");
        }

        [Command("live")]
        public async Task Live()
        {
            await ReplyAsync("As lives do canal são na quarta-feira às 21hrs e no sábado às 15hrs");
            await ReplyAsync("https://www.twitch.tv/fanmax");
        }

        [Command("ola")]
        public async Task Ola()
        {
            IUser user = Context.User;
            await ReplyAsync($"Olá {user.Username}");
        }


    }
}
