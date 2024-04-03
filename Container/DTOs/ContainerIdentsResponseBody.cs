using ContainerTypes;
using System.Collections.Generic;

namespace Infrastructure.Container.DTOs
{
    /// <summary>
    ///     Response of container idents types (Dewars, Racks, etc).
    /// </summary>
    public class GeneralTypeResponse
    {
        public int TotalRecords { get; set; }

        public List<GeneralTypeResponseBody> Types { get; set; }
    }

    /// <summary>
    ///     Body of the Response including type-ident with the intention it can used as a key for filtering subtypes 
    /// </summary>
    public class GeneralTypeResponseBody
    {
        public GeneralTypeResponseBody(ContainerMake type)
        {
            Ident = type.Ident;
            Description = type.Description;
        }

        public int Ident { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    ///     Response of container idents subtypes (Standard 2 ml Wheaton vial, etc).
    /// </summary>
    public class SubtypeResponse
    {
        public int TotalRecords { get; set; }

        public List<SubtypeResponseBody> Subtypes { get; set; }
    }

    public class SubtypeResponseBody
    {
        public SubtypeResponseBody(ContainerModel subtype)
        {
            TagIdent = (int)subtype.TagIdent;
            Description = subtype.Description;
            IsRFIDEnabled = subtype.IsRFIDEnabled;
        }

        public int TagIdent { get; set; }
        public string Description { get; set; }
        public bool IsRFIDEnabled { get; set; }
    }
}
