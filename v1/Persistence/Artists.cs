using System;
using Persistence.Enum;

namespace Persistence
{
    public class Artists
    {
        public int artistId {set;get;}
        public string artistFirstName {set;get;}
        public string artistLastName{set;get;}
        public Gender gender {set;get;}
        public DateTime born{set;get;}

        public bool isBand {get;set;}
        public string bandName {get;set;}
        public bool isSinger {get;set;}
        public string stageName{set;get;}

        public bool isProducer {get;set;}
        public string producerAlias {set;get;}

        public bool isWriter {get;set;}
        public string songwriterAlias {set;get;}

        public bool artistStatus{set;get;}

    }
}
