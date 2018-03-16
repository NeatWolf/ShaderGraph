using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph
{
    [Serializable]
    public class CustomTextureSubShader : ICustomTextureSubShader
    {
        public string GetSubshader(IMasterNode inMasterNode, GenerationMode mode)
        {
            var templateLocation = ShaderGenerator.GetTemplatePath("CustomTextureSubshader.template");

            if (!File.Exists(templateLocation))
                return string.Empty;

            var subShaderTemplate = File.ReadAllText(templateLocation);
            var resultPass = subShaderTemplate;

            return resultPass;
        }
    }
}
