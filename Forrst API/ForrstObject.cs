using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forrst
{
    public abstract class ForrstObject
    {
        public ForrstObject(ForrstClient client) {
            this.Client = client;
        }

        protected ForrstClient Client { get; private set; }
    }
}
