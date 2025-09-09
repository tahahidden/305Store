using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Command;

namespace _305.Application.Features.AttributeFeatures.Command
{
    public class CreateAttributeCommand : CreateCommand<string>
    {
        public int dataTypeInt { get; set; }

    }
}