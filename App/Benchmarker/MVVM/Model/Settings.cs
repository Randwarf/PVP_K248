using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model
{
    internal class Settings
    {
        [JsonProperty("agreedToDataSharing")]
        public bool agreedToDataSharing = false;
    }
}
