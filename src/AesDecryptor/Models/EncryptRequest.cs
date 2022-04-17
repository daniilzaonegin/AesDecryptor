using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AesDecryptor.Models
{
    public class EncryptRequest
    {
        [BindRequired, Required]
        public string TextToEncrypt { get; set; }
        [BindRequired, Required]
        [MinLength(1, ErrorMessage = "Key cannot be empty")]
        public string Key { get; set; }
    }
}
