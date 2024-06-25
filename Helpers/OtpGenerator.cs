namespace QuokkaMesh.Helpers
{
    public class OtpGenerator
    {
        public static string GenerateOtp(int length = 6)
        {
            var randomNumber = new byte[length];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);
            }
            var otp = "";
            foreach (var c in randomNumber)
            {
                otp += (c % 10).ToString();
            }
            return $"{otp}";
        }
    }
}
