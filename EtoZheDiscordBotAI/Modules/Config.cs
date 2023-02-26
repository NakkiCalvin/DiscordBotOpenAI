using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoZheDiscordBotAI.Modules
{
    internal struct Config
    {
        public string OpenAiToken { get; set; }

        public string Token { get; set; }

        public string Prefix { get; set; }
    }
}
