using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KJ_API_Projekt.model
{
    public class GeoMessagesV2 : GeoMessages
    {
        public new Message Message { get; set; }
    }
}
