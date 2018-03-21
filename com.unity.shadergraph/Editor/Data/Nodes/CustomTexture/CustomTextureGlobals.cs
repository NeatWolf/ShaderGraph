using UnityEngine;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph
{
    [Title("Custom Texture", "Globals")]
    public class CustomeTextureGlobals : AbstractMaterialNode
    {
		// Doc : https://docs.unity3d.com/Manual/CustomRenderTextures.html
		/*
		_CustomRenderTextureWidth	float	Width of the Custom Texture in pixels
		_CustomRenderTextureHeight	float	Height of the Custom Texture in pixels
		_CustomRenderTextureDepth	float	Depth of the Custom Texture in pixels (only for 3D textures, otherwise will always be equal to 1).
		_CustomRenderTextureCubeFace	float	Only for Cubemaps: Index of the current cubemap face being processed (-X, +X, -Y, +Y, -Z, +Z).
		_CustomRenderTexture3DSlice	float	Only for 3D textures: Index of the current 3D slice being processed.
		_SelfTexture2D	Sampler2D	For double buffered textures: Texture containing the result of the last update before the last swap.
		_SelfTextureCube	SamplerCUBE	For double buffered textures: Texture containing the result of the last update before the last swap.
		_SelfTexture3D	Sampler3D	For double buffered textures: Texture containing the result of the last update before the last swap.
		 */

		private const string kOutputSlotName = "Texture Width";
		private const string kOutputSlot1Name = "Texture Height";
		private const string kOutputSlot2Name = "Texture Depth";
		private const string kOutputSlot3Name = "Texture Cube Face";
		private const string kOutputSlot4Name = "Texture 3D Slice";
		private const string kOutputSlot5Name = "Self Texture 2D";
		private const string kOutputSlot6Name = "Self Texture Cube";
		private const string kOutputSlot7Name = "Self Texture 3D";

		
        public const int OutputSlotId = 0;
        public const int OutputSlot1Id = 1;
        public const int OutputSlot2Id = 2;
        public const int OutputSlot3Id = 3;
        public const int OutputSlot4Id = 4;
        public const int OutputSlot5Id = 5;
        public const int OutputSlot6Id = 6;
        public const int OutputSlot7Id = 7;

        public CustomeTextureGlobals()
        {
            name = "Custome Texture Globals";
            UpdateNodeAfterDeserialization();
        }

        public sealed override void UpdateNodeAfterDeserialization()
        {
            AddSlot(new Vector1MaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output, 0));
            AddSlot(new Vector1MaterialSlot(OutputSlot1Id, kOutputSlot1Name, kOutputSlot1Name, SlotType.Output, 0));
            AddSlot(new Vector1MaterialSlot(OutputSlot2Id, kOutputSlot2Name, kOutputSlot2Name, SlotType.Output, 0));
            AddSlot(new Vector1MaterialSlot(OutputSlot3Id, kOutputSlot3Name, kOutputSlot3Name, SlotType.Output, 0));
            AddSlot(new Vector1MaterialSlot(OutputSlot4Id, kOutputSlot4Name, kOutputSlot4Name, SlotType.Output, 0));
            AddSlot(new Texture2DMaterialSlot(OutputSlot5Id, kOutputSlot5Name, kOutputSlot5Name, SlotType.Output, ShaderStage.Fragment, false));
            AddSlot(new CubemapMaterialSlot(OutputSlot6Id, kOutputSlot6Name, kOutputSlot6Name, SlotType.Output, ShaderStage.Fragment, false));
            AddSlot(new Texture2DMaterialSlot(OutputSlot7Id, kOutputSlot7Name, kOutputSlot7Name, SlotType.Output, ShaderStage.Fragment, false));
            RemoveSlotsNameNotMatching(validSlots);
        }

        protected int[] validSlots
        {
            get { return new[] { OutputSlotId, OutputSlot1Id, OutputSlot2Id, OutputSlot3Id, OutputSlot4Id, OutputSlot5Id, OutputSlot6Id, OutputSlot7Id }; }
        }

        public override string GetVariableNameForSlot(int slotId)
        {
            switch (slotId)
            {
                case OutputSlot1Id:
                    return "_CustomRenderTextureHeight";
                case OutputSlot2Id:
                    return "_CustomRenderTextureDepth";
                case OutputSlot3Id:
                    return "_CustomRenderTextureCubeFace";
                case OutputSlot4Id:
                    return "_CustomRenderTexture3DSlice";
                case OutputSlot5Id:
                    return "_SelfTexture2D";
                case OutputSlot6Id:
                    return "_SelfTextureCube";
                case OutputSlot7Id:
                    return "_SelfTexture3D";
                default:
                    return "_CustomRenderTextureWidth";
            }
        }
	}
}