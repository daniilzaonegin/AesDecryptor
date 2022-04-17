using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AesDecryptor.Models
{
    public class EncryptViewModel
    {
        [BindRequired, Required]
        public string TextToEncrypt { get; set; }

        [BindRequired, Required]
        [MinLength(1, ErrorMessage = "Key cannot be empty")]
        public string EncryptKey { get; set; }
        public string EncryptResult { get; set; }
        public string EncryptError { get; set; }
    }
}
