using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace projectLoXo.Pages
{
    public class LichsuModel : PageModel
    {
        public static List<string> Ngays { get; set; } = new List<string>();
        public static List<string> cauHois { get; set; } = new List<string>();
        public static List<string> cauTraLois { get; set; } = new List<string>();

        public void OnGet()
        {
            Ngays.Clear();
            cauHois.Clear();
            cauTraLois.Clear();
            string query = "SELECT Id, TenDangNhap, Ngay, CauHoi, CauTraLoi FROM LichSu WHERE TenDangNhap = @TenDangNhap ORDER BY Id ASC";

            using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@TenDangNhap", NguoiDung.TenDangNhap);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ??c các giá tr? t? t?ng dòng d? li?u
                        string ngay = reader["Ngay"].ToString();
                        string cauHoi = reader["CauHoi"].ToString();
                        string cauTraLoi = reader["CauTraLoi"].ToString();

                        // Thêm vào danh sách t??ng ?ng
                        Ngays.Add(ngay);
                        cauHois.Add(cauHoi);
                        cauTraLois.Add(cauTraLoi);

                    }
                }
                foreach (var i in Ngays)
                {
                    Console.WriteLine(i);
                }
                foreach (var i in cauHois)
                {
                    Console.WriteLine(i);
                }
                foreach (var i in cauTraLois)
                {
                    Console.WriteLine(i);
                }


            }
        }
    }
}
