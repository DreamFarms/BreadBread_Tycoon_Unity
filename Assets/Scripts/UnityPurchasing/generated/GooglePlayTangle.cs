// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("6Ouy00E7vzdhbtQaj1HTnr4AA4a36d/OpAi5lO3SYk7j90UAWUeBdqd+0hWfxP/vQXypFbQW4JMuDX619kTH5PbLwM/sQI5AMcvHx8fDxsVEx8nG9kTHzMREx8fGAIqylJzsAgxAQMatb/K16t601NZ2wnCNKLqCt42RndNeLFUkFtZhimZLYU/KhIyEIU1lCyY11mveiV7Ri+nBwi1ZYqiaK+zEvObgxZrLvg30kXYe+qntkRb6nWyh8li7BCALGKtWovUnB3FezOSXi5dtC+mgRJl2ficBpChtivh2s9OAWlN08KeHyca+oQmz9DtJIxIH9DmbKz3CBtb37vFGJos9gyb5wRgQZW3lCOD0scKFcbRXpfxWTT8TywQwa9i3U8TFx8bH");
        private static int[] order = new int[] { 7,9,10,7,9,11,7,9,8,9,11,13,13,13,14 };
        private static int key = 198;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
