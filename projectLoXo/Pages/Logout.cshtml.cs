using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace projectLoXo.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            NguoiDung.TenDangNhap=String.Empty;
            NguoiDung.MatKhau = String.Empty;
            NguoiDung.HoTen = String.Empty;
            NguoiDung.DiaChi = String.Empty;
            NguoiDung.SDT = String.Empty;
            NguoiDung.QuyenHan = String.Empty;
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index"); 

        }
    }
}
