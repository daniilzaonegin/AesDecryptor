using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AesDecryptor.Models
{
    public class DecryptViewModel
    {
        [BindRequired, Required]
        public string TextToDecrypt { get; set; }

        [BindRequired, Required]
        [MinLength(1, ErrorMessage = "Key cannot be empty")]
        public string DecryptKey { get; set; }

        public string DecryptResult { get; set; }

        public string DecryptError { get; set; }
    }
}
