using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicService.Exceptions
{
    [Serializable]
    public class KindNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Kind not found";
        private int KindId { get; }
        public KindNotFoundException(int kindId) : base(DefaultMessage) {
            KindId = kindId;
        }
    }
}