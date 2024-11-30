namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    // 5. Settings Table
    public class Setting
    {
        public int SettingID { get; set; }
        public string? SettingName { get; set; } = string.Empty;  // Default to an empty string
        public string? SettingValue { get; set; } = string.Empty;  // Default to an empty string
        public DateTime UpdatedAt { get; set; } = DateTime.Now;  // Default to the current date and time
    }
}
