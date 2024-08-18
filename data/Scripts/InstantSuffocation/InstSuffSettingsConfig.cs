using ProtoBuf;
using VRageMath;
using System.Xml.Serialization;
using System.Collections.Generic;
using Sandbox.Definitions;

using static Draygo.API.HudAPIv2;

namespace Suffocation
{
    [ProtoContract]
    public class InstSuffSettingsConfig
    {
        [XmlArrayItem("DefinitionId")]
        public List<SerialCharacter> Characters;

        public InstSuffSettingsConfig()
        {
            Characters = new List<SerialCharacter>();
            foreach (var charDef in MyDefinitionManager.Static.Characters)
            {
                Characters.Add(new SerialCharacter(charDef));// charDef.DamageAmountAtZeroPressure);
            }
        }
    }

    [ProtoContract]
    public class SerialCharacter
    {
        [XmlAttribute("SubtypeId")]
        [ProtoMember(1)] public string SubtypeId;

        [XmlAttribute("DamageAmountAtZeroPressure")]
        [ProtoMember(2)] public float DamageAmountAtZeroPressure;

        public SerialCharacter() { }

        public SerialCharacter(MyCharacterDefinition charDef)
        {
            SubtypeId = charDef.Id.SubtypeName;
            DamageAmountAtZeroPressure = charDef.DamageAmountAtZeroPressure;
        }
    }
}
