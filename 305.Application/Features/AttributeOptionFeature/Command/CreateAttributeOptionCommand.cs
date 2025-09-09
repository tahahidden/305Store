using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Command;

namespace _305.Application.Features.AttributeOptionFeature.Command
{
    public class CreateAttributeOptionCommand : CreateCommand<string>
    {
        public long attributeId { get; set; }
        public string value { get; set; }
    }
}