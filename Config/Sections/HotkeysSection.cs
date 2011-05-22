using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace Configuration
{
    public class HotkeysSection : ConfigurationSection
    {
        [ConfigurationProperty("Select")]
        public HotkeyElement Select
        {
            get { return this["Select"] as HotkeyElement; }
            set { this["Select"] = value; }
        }
    }
}
