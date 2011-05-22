using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Configuration
{
    class StoragesSection : ConfigurationSection
    {
        [ConfigurationProperty("Current", IsRequired = true, DefaultValue = "Clipboard")]
        public string Current
        {
            get { return (string)this["Current"]; }
            set { this["Current"] = value; }
        }
    }
}
