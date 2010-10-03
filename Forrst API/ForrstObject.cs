using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forrst
{
    /// <summary>
    /// An object representing data from the Forrst API that contains a reference to the ForrstClient used to obtain these data.
    /// </summary>
    public abstract class ForrstObject
    {
        public ForrstObject(ForrstClient client) {
            this.Client = client;
        }

        /// <summary>
        /// The ForrstClient that has been used to obtain this object and that can be used by this object to obtain more data.
        /// </summary>
        protected ForrstClient Client { get; private set; }
    }
}
