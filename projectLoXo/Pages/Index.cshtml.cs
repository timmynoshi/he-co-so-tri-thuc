using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace projectLoXo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            NguoiDung.TenDangNhap = User.Identity.Name;
            if (NguoiDung.TenDangNhap != null) 
            {
                using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
                {
                    
                    connection.Open();

                    string query1 = "SELECT COUNT(*),TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan FROM NguoiDung WHERE TenDangNhap = @TenDangNhap " +
                        "GROUP BY TenDangNhap,MatKhau,HoTen,DiaChi,SDT,QuyenHan";

                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@TenDangNhap", NguoiDung.TenDangNhap);

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read()) 
                        {
                            NguoiDung.MatKhau = reader.GetString(2);
                            NguoiDung.HoTen = reader.GetString(3);
                            NguoiDung.DiaChi = reader.GetString(4);
                            NguoiDung.SDT = reader.GetString(5);
                            NguoiDung.QuyenHan = reader.GetString(6);
                        }
                        reader.Close();
                        // check NguoiDung có nhận được hay không
                        Console.WriteLine(NguoiDung.TenDangNhap);
                        Console.WriteLine(NguoiDung.MatKhau);

                    }
                }
            }
        }
    }
}
