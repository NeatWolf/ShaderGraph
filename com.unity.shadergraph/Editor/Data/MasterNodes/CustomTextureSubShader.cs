using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Graphing;
using UnityEditor;

namespace UnityEditor.ShaderGraph
{
    [Serializable]
    public class CustomTextureSubShader : ICustomTextureSubShader
    {
        public string GetSubshader(IMasterNode inMasterNode, GenerationMode mode)
        {
            // Get the template file
            var templateLocation = ShaderGenerator.GetTemplatePath("CustomTextureSubshader.template");
            if (!File.Exists(templateLocation))
                return string.Empty;

            var subShaderTemplate = File.ReadAllText(templateLocation);

            var masterNode = inMasterNode as CustomTextureMasterNode;
            var subShader = new ShaderGenerator();

            var builder = new ShaderStringBuilder();
            builder.IncreaseIndent();
            builder.IncreaseIndent();

            var surfaceDescriptionFunction = new ShaderGenerator();
            var surfaceDescriptionStruct = new ShaderGenerator();
            var functionRegistry = new FunctionRegistry(builder);

            var shaderProperties = new PropertyCollector();

            var activeNodeList = ListPool<INode>.Get();
            NodeUtils.DepthFirstCollectNodesFromNode(activeNodeList, masterNode, NodeUtils.IncludeSelf.Include);

            var requirements = ShaderGraphRequirements.FromNodes(activeNodeList);

            var slots = new List<MaterialSlot>();
            slots.Add(masterNode.FindSlot<MaterialSlot>(0));

            GraphUtil.GenerateSurfaceDescriptionStruct(surfaceDescriptionStruct, slots, true);

            GraphUtil.GenerateSurfaceDescription(
                activeNodeList,
                masterNode,
                masterNode.owner as AbstractMaterialGraph,
                surfaceDescriptionFunction,
                functionRegistry,
                shaderProperties,
                requirements,
                mode);

            var graph = new ShaderGenerator();
            graph.AddShaderChunk(shaderProperties.GetPropertiesDeclaration(2), false);
            graph.AddShaderChunk(builder.ToString(), false);
            graph.AddShaderChunk(surfaceDescriptionStruct.GetShaderString(2), false);
            graph.AddShaderChunk(surfaceDescriptionFunction.GetShaderString(2), false);

            var tagsVisitor = new ShaderGenerator();
            var blendingVisitor = new ShaderGenerator();
            var cullingVisitor = new ShaderGenerator();
            var zTestVisitor = new ShaderGenerator();
            var zWriteVisitor = new ShaderGenerator();

            var materialOptions = new SurfaceMaterialOptions();
            materialOptions.GetTags(tagsVisitor);
            materialOptions.GetBlend(blendingVisitor);
            materialOptions.GetCull(cullingVisitor);
            materialOptions.GetDepthTest(zTestVisitor);
            materialOptions.GetDepthWrite(zWriteVisitor);

            var localPixelShader = new ShaderGenerator();
            var localSurfaceInputs = new ShaderGenerator();
            var surfaceOutputRemap = new ShaderGenerator();

            foreach (var channel in requirements.requiresMeshUVs.Distinct())
                localSurfaceInputs.AddShaderChunk(string.Format("surfaceInput.{0} = {1};", channel.GetUVName(), string.Format("half4(input.texCoord{0}, 0, 0)", (int)channel)), false);


            // MY CODE
            
            var properties = new ShaderGenerator();
            properties.AddShaderChunk(shaderProperties.GetPropertiesDeclaration(2), false);

            var thing = new ShaderGenerator();
            thing.AddShaderChunk(surfaceDescriptionFunction.GetShaderString(2), false);


            var subShaderOutput = subShaderTemplate;

            
            subShaderOutput = subShaderOutput.Replace("${Graph}", graph.GetShaderString(3));

            subShaderOutput = subShaderOutput.Replace("${SurfaceOutputRemap}", surfaceOutputRemap.GetShaderString(3));

            //subShaderOutput = subShaderOutput.Replace("${ShaderPropertyUsages}", "");

            //subShaderOutput = subShaderOutput.Replace("${ShaderFunctions}", "");
            //subShaderOutput = subShaderOutput.Replace("${PixelShaderBody}", localPixelShader.GetShaderString(3));
            //subShaderOutput = subShaderOutput.Replace("${PixelShaderBody}", "return float4(0,1,0,1);");

            //EditorUtility.DisplayDialog("Shader", subShaderOutput, "Close");

            return subShaderOutput;
        }
    }
}
