using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace projectLoXo.Pages
{
    public class DangkyModel : PageModel
    {
        [BindProperty]
        public string Tendangnhap { get; set; }
        [BindProperty]
        public string Matkhau { get; set; }
        [BindProperty]
        public string Hoten { get; set; }
        [BindProperty]
        public string Diachi { get; set; }
        [BindProperty]
        public string SDT { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            
            string query = "INSERT INTO NguoiDung (TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan) VALUES (@TenDangNhap,@MatKhau,@HoTen,@DiaChi,@SDT,@QuyenHan)";

            using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TenDangNhap", Tendangnhap);
                command.Parameters.AddWithValue("@MatKhau", Matkhau);
                command.Parameters.AddWithValue("@HoTen", Hoten);
                command.Parameters.AddWithValue("@DiaChi", Diachi);
                command.Parameters.AddWithValue("@SDT", SDT);
                command.Parameters.AddWithValue("@QuyenHan", "user");


                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return RedirectToPage("/Login");
            }
        }
    }
}
