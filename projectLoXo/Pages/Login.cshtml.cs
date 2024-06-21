using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace projectLoXo.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Tendangnhap { get; set; }
        [BindProperty]
        public string Matkhau { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
            {
                connection.Open();

                string query = "SELECT COUNT(*),TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan FROM NguoiDung WHERE TenDangNhap = @TenDangNhap " +
                    "AND MatKhau = @MatKhau GROUP BY TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenDangNhap", Tendangnhap);
                    command.Parameters.AddWithValue("@MatKhau", Matkhau);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) 
                    {
                        NguoiDung.TenDangNhap = reader.GetString(1);
                        NguoiDung.MatKhau = reader.GetString(2);
                        NguoiDung.HoTen = reader.GetString(3);
                        NguoiDung.DiaChi = reader.GetString(4);
                        NguoiDung.SDT = reader.GetString(5);
                        NguoiDung.QuyenHan = reader.GetString(6);
                    }
                    reader.Close();

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
   
                        List<Claim> lst = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, Tendangnhap),
                            new Claim(ClaimTypes.Name, Tendangnhap)
                        };
                        ClaimsIdentity ci = new ClaimsIdentity(lst, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal cp = new ClaimsPrincipal(ci);
                        HttpContext.SignInAsync(cp);

                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên tài khoản hoặc mật khẩu không chính xác.");
                        return Page();
                    }
                }
            }
        }
    }
}
