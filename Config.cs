using System.ComponentModel;
using Exiled.API.Interfaces;
using Exiled.Loader;

namespace Replacer
{
    public class Config : IConfig
    {
        [Description("Wether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Do you want to replace players that were kicked?")]
        public bool ReplaceKicked { get; set; } = true;

        [Description("Do you want to replace players that were banned?")]
        public bool ReplaceBanned { get; set; } = true;
    }
}