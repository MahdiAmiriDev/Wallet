using System.Text.Json.Serialization;

namespace YashilCommerce.Core.DTO
{
    public class ActiveProductSalesPlanningCountGroupedDto
    {
        /// <summary>
        /// تعداد دوره های فعال یا برگذار شده ی مرکز
        /// </summary>
        [JsonPropertyName("value")]
        public int ActiveProductSalesPlanningCount { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        [JsonPropertyName("name")]
        public string KeyTitle { get; set; }
    }
}
