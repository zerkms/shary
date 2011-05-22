using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Configuration;

namespace Storages
{
    public class Factory
    {
        public Interfaces.IStorage Storage(Config config)
        {
            var storages = config.Storages;

            switch (storages.Current)
            {
                case "Clipboard":
                    return new ClipboardStorage();

                default:
                    return new ClipboardStorage();
            }
        }
    }
}
