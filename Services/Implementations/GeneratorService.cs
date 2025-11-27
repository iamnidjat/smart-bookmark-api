using System.Text;

namespace SmartBookmarkApi.Services
{
    public static class GeneratorService
    {
        private const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateCode(int length) 
        {
            Random random = new();
            StringBuilder codeBuilder = new();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(CharSet.Length);
                codeBuilder.Append(CharSet[index]);
            }

            return codeBuilder.ToString();
        }
    }
}