using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DbTool.Core
{
    public interface IModelCodeExtractorSelector
    {
        IModelCodeExtractor GetCodeExtractorByExtension(string codeFilePath);

        IModelCodeExtractor GetCodeExtractorByCodeType(string codeType);
    }

    public sealed class DefaultModelCodeExtractorSelector : IModelCodeExtractorSelector
    {
        private readonly IModelCodeExtractor[] _codeExtractors;

        public DefaultModelCodeExtractorSelector(IEnumerable<IModelCodeExtractor> codeExtractors)
        {
            _codeExtractors = codeExtractors.ToArray();
        }

        public IModelCodeExtractor GetCodeExtractorByCodeType(string codeType)
        {
            return _codeExtractors.LastOrDefault(x => x.CodeType.Equals(codeType, System.StringComparison.OrdinalIgnoreCase))
                               ?? throw new InvalidOperationException($"No ModelCodeExtractor registered for {codeType}");
            ;
        }

        public IModelCodeExtractor GetCodeExtractorByExtension(string codeFilePath)
        {
            var fileExtension = Path.GetExtension(codeFilePath);
            return _codeExtractors.LastOrDefault(x => x.SupportedFileExtensions.ContainsKey(fileExtension))
                               ?? throw new InvalidOperationException($"No ModelCodeExtractor registered for {fileExtension}");
            ;
        }
    }
}
