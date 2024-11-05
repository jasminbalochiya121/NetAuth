namespace NetAuthAssignment.Options
{
    public class JwtOptions
    {
        public const string Name = "Jwt";
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
