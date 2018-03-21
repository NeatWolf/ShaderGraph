using UnityEditor.ShaderGraph.Drawing.Controls;
using UnityEngine;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph
{
    [Title("Custom Texture", "Update Data")]
    public class CustomTextureUpdateData : AbstractMaterialNode, IGeneratesBodyCode
    {
        private const string kOutputSlotName = "data";
        
        public const int OutputSlotId = 0;

        [SerializeField]
        private CustomTextureUpdateDataChannel m_OutputChannel;

        [EnumControl("Channel")]
        public CustomTextureUpdateDataChannel uvChannel
        {
            get { return m_OutputChannel; }
            set
            {
                if (m_OutputChannel == value)
                    return;

                m_OutputChannel = value;
                Dirty(ModificationScope.Graph);
            }
        }

        public override bool hasPreview { get { return false; } }

        public CustomTextureUpdateData ()
        {
            name = "Custom Texture Update Data";
            UpdateNodeAfterDeserialization();
        }

        /*
        public override string documentationURL
        {
            get { return "https://github.com/Unity-Technologies/ShaderGraph/wiki/UV-Node"; }
        }
        */

        public override void UpdateNodeAfterDeserialization()
        {
            RemoveSlot(OutputSlotId);

            switch ( m_OutputChannel )
            {
                case CustomTextureUpdateDataChannel.globalTexcoord:
                    AddSlot(new Vector3MaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output, Vector3.zero));
                    break;
                case CustomTextureUpdateDataChannel.primitiveID:
                    AddSlot(new Vector1MaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output, 0f ));
                    break;
                case CustomTextureUpdateDataChannel.direction:
                    AddSlot(new Vector3MaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output, Vector3.zero));
                    break;
                default :
                    AddSlot(new Vector3MaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output, Vector3.zero));
                    break;
            }
        }

        public void GenerateNodeCode(ShaderGenerator visitor, GenerationMode generationMode)
        {
            if (m_OutputChannel == CustomTextureUpdateDataChannel.primitiveID)
            {
                visitor.AddShaderChunk(string.Format("{0} {1} = IN.{2};", precision, GetVariableNameForSlot(OutputSlotId), m_OutputChannel.GetCustomTextureUpdateDataName()), true);
            }
            else
            {
                visitor.AddShaderChunk(string.Format("{0}3 {1} = IN.{2};", precision, GetVariableNameForSlot(OutputSlotId), m_OutputChannel.GetCustomTextureUpdateDataName()), true);
            }
        }

        public bool RequiresMeshUV(CustomTextureUpdateDataChannel channel)
        {
            return channel == uvChannel;
        }
    }

    public enum CustomTextureUpdateDataChannel
    {
        localTexcoord = 0,
        globalTexcoord = 1,
        primitiveID = 2, 
        direction = 3
    }
}
