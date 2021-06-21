using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Geek.Server.Demo
{
    public class DemoBagState : DBState
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public StateMap<int, long> ItemMap { get; set; } = new StateMap<int, long>();
    }

    public class DemoBagComp : StateComponent<DemoBagState>
    {
    }
}
