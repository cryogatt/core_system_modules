using Newtonsoft.Json;

namespace Infrastructure.Material.DTOs
{
    public class MaterialInfoResponseBody
    {
        public MaterialInfoResponseBody()
        {

        }

        public MaterialInfoResponseBody(string attributeFieldName, string attributeValueName = null)
        {
            AttributeFieldName = attributeFieldName;
            AttributeValueName = attributeValueName;
        }

        /// <summary>
        ///     Header.
        /// </summary>
        [JsonProperty]
        public string AttributeFieldName;

        /// <summary>
        ///     Value.
        /// </summary>
        [JsonProperty]
        public string AttributeValueName;

    }
}