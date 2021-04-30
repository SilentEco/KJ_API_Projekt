using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KJ_API_Projekt.model
{
    public class ApiToken
    {
        public int Id { get; set; }
        public MyUser User { get; set; }
        public string Value { get; set; }
    }
}
