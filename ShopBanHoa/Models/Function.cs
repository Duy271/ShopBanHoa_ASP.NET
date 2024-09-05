using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace ShopBanHoa.Models
{
    public class Function
    {

        public string CalculateMD5Hash(string mk)
        {
            string input = "&&))//" + mk + "!!((\\";
            // Chuyển đổi input thành mảng byte
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Sử dụng MD5 để tính toán hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}