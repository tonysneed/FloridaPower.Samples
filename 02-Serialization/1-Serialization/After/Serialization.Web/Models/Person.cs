using ProtoBuf;

namespace Serialization.Web.Models
{
    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public int Age { get; set; }

        [ProtoMember(3, AsReference = true)]
        public Person Brother { get; set; }
    }
}