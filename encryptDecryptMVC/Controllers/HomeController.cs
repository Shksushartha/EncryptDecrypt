using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using encryptDecryptMVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace encryptDecryptMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Encrypt()
    {
        return View(new encryptDecryptModel());
    }

    [HttpPost]
    public IActionResult Encrypt(encryptDecryptModel x)
    {
        x.e = EncryptString(x.d);
        return View(x);
    }

    [HttpGet]
    public IActionResult Decrypt()
    {
        return View(new encryptDecryptModel());
    }

    [HttpPost]
    public IActionResult Decrypt(encryptDecryptModel x)
    {
        x.d = DecryptString(x.e);
        return View(x);
    }


    public static string EncryptString(string a)
    {
        string key = "#32!xAz)27:zXa@3";

        try
        {

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(a);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }


            return Convert.ToBase64String(array);
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string DecryptString(string a)
    {
        string key = "#32!xAz)27:zXa@3";
        string cipherText;

        try
        {

            byte[] iv = new byte[16];


            byte[] buffer = Convert.FromBase64String(a);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            return e.Message;
        }

    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

