namespace studakAPI.DTO
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime Expires { get; set; }
    }
}
