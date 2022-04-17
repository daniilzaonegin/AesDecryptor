using AesDecryptor.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AesDecryptor.Controllers
{
    public class HomeController : Controller
    {
        [ActionName("Encrypt")]
        public IActionResult GetEncrypt() {
            return View("Encrypt");
        }

        [HttpPost]
        public IActionResult Encrypt(EncryptViewModel request) {
            if (!ModelState.IsValid)
            {
                return View("Encrypt", request);
            }
            byte[] encryptedMsg;
            byte[] generatedIV;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] userKey = Encoding.UTF8.GetBytes(request.EncryptKey);
                    
                    aes.Key = Enumerable.Range(0, 32-userKey.Length).Select(i => (byte)0).Concat(userKey).ToArray();
                    generatedIV = aes.IV;
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter sw = new StreamWriter(cs))
                                {
                                    sw.Write(request.TextToEncrypt);
                                }
                            }
                            encryptedMsg = ms.ToArray();
                        }
                    }
                }
            } 
            catch (Exception exc)
            {
                request.EncryptError = exc.Message;
                return View("Encrypt", request);
            }
            generatedIV ??= new byte[0];
            encryptedMsg ??= new byte[0];
            request.EncryptResult=(Convert.ToBase64String(generatedIV.Concat(encryptedMsg).ToArray()));
            return View("Encrypt", request);
        }

        [ActionName("Decrypt")]
        public IActionResult GetDecrypt() {
            return View("Decrypt");
        }

        [HttpPost]
        public IActionResult Decrypt(DecryptViewModel viewModel) {
            if (!ModelState.IsValid)
            {
                return View("Decrypt",viewModel);
            }
            byte[] input = Convert.FromBase64String(viewModel.TextToDecrypt);
            if (input.Length < 18)
            {
                viewModel.DecryptError = "Message couldn't be shorter than 16 bytes";
                return View("Decrypt", viewModel);
            }
            byte[] IV = input.Take(16).ToArray();
            byte[] textToDecryptBytes = input.TakeLast(input.Length - 16).ToArray();
            string decryptedMsg = "";
            try
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] userKey = Encoding.UTF8.GetBytes(viewModel.DecryptKey);

                    aes.Key = Enumerable.Range(0, 32 - userKey.Length).Select(i => (byte)0).Concat(userKey).ToArray();
                    aes.IV = IV;
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        using (MemoryStream ms = new MemoryStream(textToDecryptBytes))
                        {
                            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader sr = new StreamReader(cs))
                                {
                                    decryptedMsg = sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                viewModel.DecryptError = exc.Message;
                return View("Decrypt", viewModel);
            }
            viewModel.DecryptResult = decryptedMsg;
            return View("Decrypt", viewModel);
        }
    }
}