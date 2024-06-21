using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace projectLoXo.Pages
{
    public class ThongtinModel : PageModel
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
            Tendangnhap = NguoiDung.TenDangNhap;
            string query = "SELECT MatKhau, HoTen, DiaChi, SDT, TenDangNhap FROM NguoiDung WHERE TenDangNhap = @TenDangNhap";

            using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TenDangNhap", Tendangnhap);
                
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Matkhau = reader.GetString(0);
                    Hoten = reader.GetString(1);
                    Diachi = reader.GetString(2);
                    SDT = reader.GetString(3);
                }

                reader.Close();
            }
        }

        public IActionResult OnPost()
        {
            Console.WriteLine(Tendangnhap);
            Console.WriteLine(Matkhau);
            string query = "UPDATE NguoiDung SET MatKhau = @MatKhau, HoTen = @HoTen, DiaChi = @DiaChi, SDT = @SDT WHERE TenDangNhap = @TenDangNhap";

            using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TenDangNhap", Tendangnhap);
                command.Parameters.AddWithValue("@MatKhau", Matkhau);
                command.Parameters.AddWithValue("@HoTen", Hoten);
                command.Parameters.AddWithValue("@DiaChi", Diachi);
                command.Parameters.AddWithValue("@SDT", SDT);


                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Insert thành công!");

                    return RedirectToPage("/Thongtin");
                }
                else
                {
                    Console.WriteLine("Insert không thành công!");

                    return Page();
                }
            }
        }
    }
}
