using System;
using System.Security.Cryptography;
using System.Text;

namespace ExecutavelGitAnalyzer.Infra
{
    class Criptografia
    {
        public static string Encrypt(string input)
        {
            using TripleDESCryptoServiceProvider tripledescryptoserviceprovider = new();
            using MD5CryptoServiceProvider md5cryptoserviceprovider = new();

            try
            {
                if (input.Trim() != "")
                {
                    string myKey = "1111111111111111";  //Aqui vc inclui uma chave qualquer para servir de base para cifrar, que deve ser a mesma no método Decodificar
                    tripledescryptoserviceprovider.Key = md5cryptoserviceprovider.ComputeHash(Encoding.ASCII.GetBytes(myKey));
                    tripledescryptoserviceprovider.Mode = CipherMode.ECB;
                    using ICryptoTransform desdencrypt = tripledescryptoserviceprovider.CreateEncryptor();
                    ASCIIEncoding MyASCIIEncoding = new();
                    byte[] buff = Encoding.ASCII.GetBytes(input);

                    return Convert.ToBase64String(desdencrypt.TransformFinalBlock(buff, 0, buff.Length));

                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao cifrar : {e.Message}");
                return null;
            }
        }

        public static string Decrypt(string input)
        {
            using TripleDESCryptoServiceProvider tripledescryptoserviceprovider = new();
            using MD5CryptoServiceProvider md5cryptoserviceprovider = new();

            try
            {
                if (input.Trim() != "")
                {
                    string myKey = "1111111111111111";  //Aqui vc inclui uma chave qualquer para servir de base para cifrar, que deve ser a mesma no método Codificar
                    tripledescryptoserviceprovider.Key = md5cryptoserviceprovider.ComputeHash(Encoding.ASCII.GetBytes(myKey));
                    tripledescryptoserviceprovider.Mode = CipherMode.ECB;
                    using ICryptoTransform desdencrypt = tripledescryptoserviceprovider.CreateDecryptor();
                    byte[] buff = Convert.FromBase64String(input);

                    return Encoding.ASCII.GetString(desdencrypt.TransformFinalBlock(buff, 0, buff.Length));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao decifrar : {e.Message}");
                return null;
            }
        }
    }
}
