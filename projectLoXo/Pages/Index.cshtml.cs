using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace projectLoXo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public string KetQuaXuLy { get; set; }
        [BindProperty]
        public List<TruyVanEntry> TruyVanEntries { get; set; }

        public class TruyVanEntry
        {
            public string TenDangNhap { get; set; }
            public string TruyVanCauHoi { get; set; }
            public string TruyVanTraLoi { get; set; }
            public string ThoiGian { get; set; }
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

            KetQuaXuLy = "";
            TruyVanEntries = GetQueryInformation();

        }

        private List<TruyVanEntry> GetQueryInformation()
        {
            var truyVanEntries = new List<TruyVanEntry>();

            using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
            {
                connection.Open();
                string query = "SELECT TenDangNhap, Ngay, CauHoi, CauTraLoi FROM LichSu";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        truyVanEntries.Add(new TruyVanEntry
                        {
                            TenDangNhap = reader["TenDangNhap"].ToString(),
                            TruyVanCauHoi = reader["CauHoi"].ToString(),
                            TruyVanTraLoi = reader["CauTraLoi"].ToString(),
                            ThoiGian = reader["Ngay"].ToString()
                        });
                    }
                }
            }

            return truyVanEntries;
        }
        private string ProcessKetQuaXuLy(string input)
        {
            // Replace {} and _ with space
            string processedInput = input.Replace("{", " ").Replace("}", " ").Replace("_", " ").Replace("[", " ").Replace("]", " ");

            // Convert exponents to HTML
            processedInput = Regex.Replace(processedInput, @"([a-zA-Z0-9]+)\^2", m =>
            {
                var baseValue = m.Groups[1].Value;
                return $"{baseValue}<sup>2</sup>";
            });

            // Convert subscripts to HTML
            processedInput = Regex.Replace(processedInput, @"([a-zA-Z]+)(\d+)", m =>
            {
                var baseValue = m.Groups[1].Value;
                var subscript = m.Groups[2].Value;
                return $"{baseValue}<sub>{subscript}</sub>";
            });

            // Convert numeric fractions to HTML
            processedInput = Regex.Replace(processedInput, @"(\d+)/(\d+)", m =>
            {
                var numerator = m.Groups[1].Value;
                var denominator = m.Groups[2].Value;
                return $"<span class=\"fraction\"><span class=\"numerator\">{numerator}</span><span class=\"denominator\">{denominator}</span></span>";
            });

            // Convert alphabetic fractions to HTML
            processedInput = Regex.Replace(processedInput, @"([a-zA-Z]+)\/([a-zA-Z]+)", m =>
            {
                var numerator = m.Groups[1].Value;
                var denominator = m.Groups[2].Value;
                return $"<span class=\"fraction\"><span class=\"numerator\">{numerator}</span><span class=\"denominator\">{denominator}</span></span>";
            });

            // Convert mixed fractions to HTML
            processedInput = Regex.Replace(processedInput, @"([a-zA-Z]+(<sub>\d+<\/sub>)*)\/([a-zA-Z]+(<sub>\d+<\/sub>)*)", m =>
            {
                var numerator = m.Groups[1].Value;
                var denominator = m.Groups[3].Value;
                return $"<span class=\"fraction\"><span class=\"numerator\">{numerator}</span><span class=\"denominator\">{denominator}</span></span>";
            });

            // Convert fractions in equations like u^2/U0L^2+i^2/I0^2 to HTML
            processedInput = Regex.Replace(processedInput, @"([a-zA-Z0-9^]+)\/([a-zA-Z0-9^]+)", m =>
            {
                var numerator = m.Groups[1].Value;
                var denominator = m.Groups[2].Value;
                return $"<span class=\"fraction\"><span class=\"numerator\">{numerator}</span><span class=\"denominator\">{denominator}</span></span>";
            });

            return processedInput;
        }



        
        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            string H = form["GIATHIET_H"].ToString();
            string G = form["KETLUAN_G"].ToString();
            string f1, f2, f3;

            string loaiBaiTap = form["LOAIBAITAP"].ToString();
            string KnowledgePath;
            string filename = RandomNameFile();

            f3 = Path.Combine(_webHostEnvironment.ContentRootPath, "Knowledge", "Rules.txt").Replace(@"\", @"/");
            f1 = Path.Combine(_webHostEnvironment.ContentRootPath, "Knowledge", "Facts.txt").Replace(@"\", @"/");
            f2 = Path.Combine(_webHostEnvironment.ContentRootPath, "Knowledge", "Formula.txt").Replace(@"\", @"/");
            KnowledgePath = Path.Combine(_webHostEnvironment.ContentRootPath, "KB-IE").Replace(@"\", @"/");

            string ResultPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Solved", filename + "_ketqua.html").Replace(@"\", @"/");
            string DataPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Data", filename + "_debai.txt").Replace(@"\", @"/");
            string BatPath = Path.Combine(_webHostEnvironment.ContentRootPath, "BatFiles", filename + "_bat.bat").Replace(@"\", @"/");
            string H1 = H;
            string G1 = G;
            CreateFileData(KnowledgePath, DataPath, H1, G1, ResultPath, f1, f2, f3);
            CreateFileBat(BatPath, DataPath);
            GoToConnectToMaple(BatPath);
            DateTime currentTime = DateTime.Now;
            string formattedDate = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

            KetQuaXuLy = await System.IO.File.ReadAllTextAsync(ResultPath);
            KetQuaXuLy = ProcessKetQuaXuLy(KetQuaXuLy);

            if (User.Identity.Name != null)
            {
                using (SqlConnection connection = new SqlConnection(SQLCon.stringg))
                {
                    connection.Open();
                    string query = "INSERT INTO LichSu (TenDangNhap, CauHoi, CauTraLoi, Ngay) " +
                                   "VALUES (@TenDangNhap, @TruyVanCauHoi, @TruyVanTraLoi, @ThoiGian)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenDangNhap", User.Identity.Name);
                        command.Parameters.AddWithValue("@TruyVanCauHoi", "Gia Thiet : " + H + " \n Ket Luan : " + G);
                        command.Parameters.AddWithValue("@TruyVanTraLoi", KetQuaXuLy);
                        command.Parameters.AddWithValue("@ThoiGian", formattedDate);

                        command.ExecuteNonQuery();
                    }
                }
            }
            return RedirectToPage("/Index");
        }




        public static void CreateFileData(string KnowledgePath, string pathFileData, string H, string G, string ResultsPath, string f1, string f2, string f3)
        {
            using (FileStream fs = new FileStream(pathFileData, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
            {
                sw.WriteLine("restart: ");
                sw.WriteLine("libname :=libname,\"" + KnowledgePath + "\": ");
                sw.WriteLine("with(problemsolving): ");
                sw.WriteLine("f1:=\"" + f1 + "\": ");
                sw.WriteLine("f2:=\"" + f2 + "\": ");
                sw.WriteLine("f3:=\"" + f3 + "\": ");
                sw.WriteLine("H:={" + H + "}: ");
                sw.WriteLine("G:={" + G + "}: ");
                sw.WriteLine("Init(): ");
                sw.WriteLine("Initialize(f1,f2,f3): ");
                sw.WriteLine("Sol:=FindSolution(H, G): ");
                sw.WriteLine("Sol:=ReduceSolultionSteps(Sol, G): ");
                sw.WriteLine("duongdan:=\"" + ResultsPath + "\": ");
                sw.WriteLine("OutPutSolution(Sol, duongdan): ");
            }
        }

        public static void CreateFileBat(string pathFileBat, string InputPath)
        {
            using (FileStream fs = new FileStream(pathFileBat, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
            {
                sw.WriteLine("cmaple.exe \"" + InputPath + "\"");
            }
        }

        public void GoToConnectToMaple(string BatPath)
        {
            var proc = new System.Diagnostics.Process
            {
                EnableRaisingEvents = true,
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = BatPath,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                }
            };
            proc.Start();
            proc.WaitForExit();
            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(5));
        }

        public string RandomNameFile()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }
    }
}
