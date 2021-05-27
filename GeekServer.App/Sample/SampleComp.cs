using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geek.Server
{
    public class SampleState : DBState
    {
        /// <summary>需要回存数据库的字段需要使用属性(get set)才能检测到属性变化，及时回存数据库</summary>
        public string TestStr { get; set; }
        public long TestLong { get; set; }
        /// <summary>list一律使用StateList</summary>
        public StateList<string> TestList { get; set; } = new StateList<string>();
        /// <summary>map一律使用StateMap 且需要加入DictionaryRepresentation.ArrayOfDocuments标签，否则mongodb序列化无法自动序列化</summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public StateMap<long, string> TestMap { get; set; } = new StateMap<long, string>();

        /// <summary>
        /// 不回存的字段不需要用get/set（StateList,StateMap）
        /// 但是需要加上BsonIgnore标签
        /// </summary>
        [BsonIgnore]
        public List<long> TestNoStoreList = new List<long>();
    }

    public class SampleComp : StateComponent<SampleState>
    {

    }
}
