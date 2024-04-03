// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;

namespace ContainerTypes
{
    /// <summary>
    /// Structure for type of Container e.g Dewar, Canister etc.
    /// </summary>
    public struct ContainerMake
    {
        public UInt16 Ident { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Stucture for subtype Container e.g Stateborne BioTrek 5 etc.
    /// </summary>
    public struct ContainerModel
    {
        public UInt32 TagIdent { get; set; }
        public string Description { get; set; }
        public bool IsRFIDEnabled { get; set; }
    }

    /// <summary>
    /// Class for accessing container types
    /// </summary>
    public static class ContainerIdentTypes
    {
        /// <summary>
        /// List of General types of Containers e.g Dewar, Canister etc. 
        /// </summary>
        public static List<ContainerMake> Types => new List<ContainerMake>()
        {
            new ContainerMake() { Ident = GeneralTypes.NONE_TYPE_ID, Description = "Not a known type"},
            new ContainerMake() { Ident = GeneralTypes.VIAL_TYPE_ID, Description = "Vial"},
            new ContainerMake() { Ident = GeneralTypes.BOX_TYPE_ID, Description = "Box" },
            new ContainerMake() { Ident = GeneralTypes.RACK_TYPE_ID, Description = "Stack" },
            new ContainerMake() { Ident = GeneralTypes.DEWAR_TYPE_ID, Description = "Dewar" },
            new ContainerMake() { Ident = GeneralTypes.STRAW_TYPE_ID, Description = "Straw" },
            new ContainerMake() { Ident = GeneralTypes.VISOTUBE_TYPE_ID, Description = "Visotube" },
            new ContainerMake() { Ident = GeneralTypes.GOBLET_TYPE_ID, Description = "Goblet" },
            new ContainerMake() { Ident = GeneralTypes.CANISTER_TYPE_ID, Description = "Canister" },
            new ContainerMake() { Ident = GeneralTypes.POT_TYPE_ID, Description = "Pot" },
            new ContainerMake() { Ident = GeneralTypes.TEST_BENCH_TYPE_ID, Description = "Test Bench" },
            new ContainerMake() { Ident = GeneralTypes.STORAGE_RACK_TYPE_ID, Description = "Storage Rack" },
            new ContainerMake() { Ident = GeneralTypes.CANE_TYPE_ID, Description = "Cane" },
            new ContainerMake() { Ident = GeneralTypes.FREEZER_TYPE_ID, Description = "Freezer" },
            new ContainerMake() { Ident = GeneralTypes.LOCATION_TYPE_ID, Description = "Location" },
            new ContainerMake() { Ident = GeneralTypes.DRY_SHIPPER_TYPE_ID, Description = "Dry Shipper" }
        };

        /// <summary>
        /// List of container subtypes (model) e.g Wheaton 2 ml Cryo Vial. 
        /// </summary>
        public static List<ContainerModel> Subtypes => new List<ContainerModel>()
        {
            // Vials
            new ContainerModel() { TagIdent = (GeneralTypes.VIAL_TYPE_ID << 16) + (1), Description = "Reserved", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.VIAL_TYPE_ID << 16) + (2), Description = "Wheaton 2 ml CryoElite vial", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.VIAL_TYPE_ID << 16) + (3), Description = "Wheaton 5 ml CryoElite vial", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.VIAL_TYPE_ID << 16) + (4), Description = "GbO 2ml Cryo.s vial", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.VIAL_TYPE_ID << 16) + (5), Description = "GbO 5ml Cryo.s vial", IsRFIDEnabled = true },

            // Boxes
            new ContainerModel() { TagIdent = (GeneralTypes.BOX_TYPE_ID << 16) + (1), Description = "Nunc 10x10 storage box", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.BOX_TYPE_ID << 16) + (2), Description = "Wheaton 10x10 storage box", IsRFIDEnabled = true },

            // Racks/Stacks/Towers
            new ContainerModel() { TagIdent = (GeneralTypes.RACK_TYPE_ID << 16) + (1), Description = "Generic 11 stage rack", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.RACK_TYPE_ID << 16) + (2), Description = "Generic 15 stage rack", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.RACK_TYPE_ID << 16) + (3), Description = "Generic 10 stage rack", IsRFIDEnabled = false },

            //Dewars/Freezers
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (1), Description = "MVE model 1400 dewar", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (2), Description = "MVE model 800 dewar", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (3), Description = "CryoPlus 7400 series dewar", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (4), Description = "Sanyo Chest Freezer", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (5), Description = "Statebourne Biosystem Access 12", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (6), Description = "Statebourne Biosystem Access 24", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (7), Description = "Statebourne Biosystem Access 50", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (8), Description = "Statebourne Biosystem Archive 21", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (9), Description = "Statebourne Biosystem Archive 40", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (10), Description = "Statebourne Biosystem Archive 60", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (11), Description = "Statebourne Biosystem Archive 80", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (12), Description = "Statebourne Biosystem Archive 100", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (13), Description = "Statebourne Biosystem Archive 100+", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (14), Description = "Statebourne Bio 2", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (15), Description = "Statebourne Bio 3", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (16), Description = "Statebourne Bio 8", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (17), Description = "Statebourne Bio 10", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (18), Description = "Statebourne Bio 20", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (19), Description = "Statebourne Bio 35", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (20), Description = "Statebourne Bio 12", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (21), Description = "Statebourne Bio 21", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (22), Description = "Statebourne Bio 22", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (23), Description = "Statebourne Bio 34", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DEWAR_TYPE_ID << 16) + (24), Description = "Statebourne Bio 36", IsRFIDEnabled = true },

            //Straws
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (1), Description = "CBS straw - clear sleeve", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (2), Description = "CBS straw - white sleeve", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (3), Description = "CBS straw - red sleeve", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (4), Description = "CBS straw - orange sleeve", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (5), Description = "CBS straw - yellow sleeve", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (6), Description = "CBS straw - green sleeve", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.STRAW_TYPE_ID << 16) + (7), Description = "CBS straw - blue sleeve", IsRFIDEnabled = true },

            //Visotubes
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (1), Description = "CBS circular clear visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (2), Description = "CBS triangular black visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (3), Description = "CBS triangular brown visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (4), Description = "CBS triangular red visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (5), Description = "CBS triangular green visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (6), Description = "CBS triangular blue visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (7), Description = "CBS triangular grey visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (8), Description = "CBS triangular purple visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (9), Description = "CBS triangular yellow visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (10), Description = "CBS triangular pink visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (11), Description = "CBS triangular orange visotube", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.VISOTUBE_TYPE_ID << 16) + (12), Description = "CBS triangular pistachio visotube", IsRFIDEnabled = false },

            //Goblet 
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (1), Description = "CBS clear goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (2), Description = "CBS red goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (3), Description = "CBS green goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (4), Description = "CBS blue goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (5), Description = "CBS grey goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (6), Description = "CBS purple goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (7), Description = "CBS yellow goblet", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.GOBLET_TYPE_ID << 16) + (8), Description = "CBS pistachio goblet", IsRFIDEnabled = true },
            //Pot
            new ContainerModel() { TagIdent = (GeneralTypes.POT_TYPE_ID << 16) + (1), Description = "50 ml sperm pot", IsRFIDEnabled = true },
            
            // Test bench
            // TBD

            //Storage Rack
            // TBD

            // Cane
            new ContainerModel() { TagIdent = (GeneralTypes.CANE_TYPE_ID << 16) + (1), Description = "Cane for 6 x 2ml vials", IsRFIDEnabled = true },

            // Canister
            new ContainerModel() { TagIdent = (GeneralTypes.CANISTER_TYPE_ID << 16) + (1), Description = "CBS 2-shelf canister", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.CANISTER_TYPE_ID << 16) + (2), Description = "CBS 3-shelf canister", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.CANISTER_TYPE_ID << 16) + (3), Description = "CBS 4-shelf canister", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.CANISTER_TYPE_ID << 16) + (4), Description = "CBS 5-shelf canister", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.CANISTER_TYPE_ID << 16) + (5), Description = "CBS 6-shelf canister", IsRFIDEnabled = true },
            
            // Fridge/Freezers
            new ContainerModel() { TagIdent = (GeneralTypes.FREEZER_TYPE_ID << 16) + (1), Description = "Generic +4 deg C Fridge", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.FREEZER_TYPE_ID << 16) + (2), Description = "Generic -20 deg C Freezer", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.FREEZER_TYPE_ID << 16) + (3), Description = "Generic -80 deg C Freezer", IsRFIDEnabled = false },

            // Location
            new ContainerModel() { TagIdent = (GeneralTypes.LOCATION_TYPE_ID << 16) + (1), Description = "Organisation", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.LOCATION_TYPE_ID << 16) + (2), Description = "Site", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.LOCATION_TYPE_ID << 16) + (3), Description = "Buildings", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.LOCATION_TYPE_ID << 16) + (4), Description = "Department", IsRFIDEnabled = false },
            new ContainerModel() { TagIdent = (GeneralTypes.LOCATION_TYPE_ID << 16) + (5), Description = "Room", IsRFIDEnabled = false },

            // Dry Shipper
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (1), Description = "MVE QWick 6/9", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (2), Description = "MVE QWick 10/1000", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (3), Description = "MVE QWick 14/48", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (4), Description = "MVE QWick 62/180", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (5), Description = "MVE QWick 14/24", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (6), Description = "MVE QWick 9/500", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (7), Description = "MVE QWick 10/950", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (8), Description = "MVE SC 2/1V", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (9), Description = "MVE SC 4/2V", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (10), Description = "MVE SC 4/3V", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (11), Description = "MVE SC 20/12V", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (12), Description = "MVE XC 20/3V", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (13), Description = "MVE Mini Moover", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (14), Description = "MVE CryoShipper Mini", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (15), Description = "MVE Cryo Moover", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (16), Description = "MVE Cryo Shipper", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (17), Description = "MVE Cruo Shipper XC", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (18), Description = "MVE IATA", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (19), Description = "Statebourne Biotrek 3", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (20), Description = "Statebourne Biotrek 10", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (21), Description = "Taylor-Wharton CX100", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (22), Description = "Taylor-Wharton CXR100", IsRFIDEnabled = true },
            new ContainerModel() { TagIdent = (GeneralTypes.DRY_SHIPPER_TYPE_ID << 16) + (23), Description = "Taylor-Wharton CXR500", IsRFIDEnabled = true }
        };

        public static ContainerMake GetGeneralType(UInt16 id)
        {
            return (from gen in Types where gen.Ident.Equals(id)
                    select gen).FirstOrDefault();
        }

        /// <summary>
        /// Retreives all General types (Makes) which have RFID enabled subtypes.
        /// </summary>
        /// <returns></returns>
        public static List<ContainerMake> GetGeneralRFIDEnabledContainers()
        {
            List<ContainerMake> resp = (from sub in Subtypes where sub.IsRFIDEnabled 
                                        from gen in Types where (sub.TagIdent >> 16).Equals(gen.Ident)
                                        select gen).Distinct().ToList();
            return resp;
        }

        /// <summary>
        /// Retreives all General types (Makes) which have RFID enabled subtypes.
        /// Excludes location types.
        /// </summary>
        /// <returns></returns>
        public static List<ContainerMake> GetGeneralNonRFIDEnabledContainers()
        {
            List<ContainerMake> resp = (from sub in Subtypes where !sub.IsRFIDEnabled
                                        from gen in Types where (sub.TagIdent >> 16).Equals(gen.Ident) && !gen.Ident.Equals(GeneralTypes.LOCATION_TYPE_ID)
                                        select gen).Distinct().ToList();
            return resp;
        }

        /// <summary>
        /// Retrieves all RFID enabled subtypes. 
        /// </summary>
        /// <returns></returns>
        public static List<ContainerModel> GetSubtypeRFIDEnabledContainers()
        {
            List<ContainerModel> resp = (from sub in Subtypes where sub.IsRFIDEnabled
                                         select sub).ToList();
            return resp;
        }

        /// <summary>
        /// Retrieves all RFID enabled subtypes. 
        /// </summary>
        /// <returns></returns>
        public static List<ContainerModel> GetSubtypeNonRFIDEnabledContainers()
        {
            List<ContainerModel> resp = (from sub in Subtypes where !sub.IsRFIDEnabled
                                         select sub).ToList();
            return resp;
        }

        /// <summary>
        /// Retrieves all RFID enabled subtypes for a certain general type, Wheaton 2 ml vial for Vial etc. 
        /// </summary>
        /// <param name="GENERAL_TYPE_ID"></param>
        /// <returns></returns>
        public static List<ContainerModel> GetSubtypeRFIDEnabledContainers(ushort GENERAL_TYPE_ID)
        {
            List<ContainerModel> resp = (from gen in Types where gen.Ident.Equals(GENERAL_TYPE_ID)
                                         from sub in Subtypes where (sub.TagIdent >> 16).Equals(gen.Ident)
                                         where sub.IsRFIDEnabled select sub).ToList();
            return resp;
        }

        /// <summary>
        /// Retrieves all non RFID enabled subtypes for a certain general type, generic 11 stage rack etc. 
        /// </summary>
        /// <param name="GENERAL_TYPE_ID"></param>
        /// <returns></returns>
        public static List<ContainerModel> GetSubtypeNonRFIDEnabledContainers(ushort GENERAL_TYPE_ID)
        {
            List<ContainerModel> resp = (from gen in Types where gen.Ident.Equals(GENERAL_TYPE_ID)
                                         from sub in Subtypes where (sub.TagIdent >> 16).Equals(gen.Ident)
                                         where !sub.IsRFIDEnabled select sub).ToList();
            return resp;
        }
    }
}
