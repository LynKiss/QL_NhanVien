using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp4
{
    internal class Program
    {
        struct kieunv
        {
            public int manv;
            public string tennv;
            public int tuoinv;
            public string gioitinhnv, quequannv, chucvunv;
        }
        static int soluong;
        static kieunv[] dsnv;
        static bool kiemTraMaNVDaTonTai(int maNV, string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 1)
                {
                    int existingMaNV;
                    if (int.TryParse(values[0], out existingMaNV) && existingMaNV == maNV)
                    {
                        return true; // Mã nhân viên đã tồn tại
                    }
                }
            }
            return false; // Mã nhân viên chưa tồn tại
        }

        static void nhap()
        {
            Console.WriteLine("Mời nhập số lượng nhân viên cần thêm:");
            int soluong = int.Parse(Console.ReadLine());
            kieunv[] dsnv = new kieunv[soluong];
            List<int> maNhanVienDaNhap = new List<int>(); // Danh sách lưu các mã nhân viên đã nhập

            for (int i = 0; i < dsnv.Length; i++)
            {
                Console.WriteLine("Mời nhập nhân viên thứ {0}", i + 1);
                Console.WriteLine("Nhập mã nhân viên:");

                int maNV;
                bool daTonTai;

                do
                {
                    while (!int.TryParse(Console.ReadLine(), out maNV))
                    {
                        Console.WriteLine("Mã nhân viên không hợp lệ. Vui lòng nhập lại:");
                    }

                    daTonTai = kiemTraMaNVDaTonTai(maNV, "C:\\DSnhanvien.txt");

                    if (daTonTai || maNhanVienDaNhap.Contains(maNV))
                    {
                        Console.WriteLine("Mã nhân viên đã tồn tại. Vui lòng nhập lại:");
                    }
                } while (daTonTai || maNhanVienDaNhap.Contains(maNV));

                // Lưu mã nhân viên vào danh sách đã nhập
                maNhanVienDaNhap.Add(maNV);

                kieunv nhanVien = new kieunv();
                nhanVien.manv = maNV;

                Console.WriteLine("Họ và tên nhân viên:");
                nhanVien.tennv = Console.ReadLine();

                Console.WriteLine("Mời nhập tuổi của nhân viên:");
                int tuoi;
                while (!int.TryParse(Console.ReadLine(), out tuoi) || tuoi < 18 || tuoi > 60)
                {
                    Console.WriteLine("Tuổi không hợp lệ. Vui lòng nhập lại (từ 18 đến 60):");
                }
                nhanVien.tuoinv = tuoi;

                Console.WriteLine("Mời nhập giới tính của nhân viên (Nam hoặc Nữ):");
                string gioiTinh = Console.ReadLine();
                while (gioiTinh != "Nam" && gioiTinh != "Nữ")
                {
                    Console.WriteLine("Giới tính không hợp lệ. Vui lòng nhập lại (Nam hoặc Nữ):");
                    gioiTinh = Console.ReadLine();
                }
                nhanVien.gioitinhnv = gioiTinh;

                Console.WriteLine("Mời nhập quê quán nhân viên:");
                nhanVien.quequannv = Console.ReadLine();

                Console.WriteLine("Mời nhập chức vụ (bảo vệ, thu ngân, đầu bếp, bưng bê, rửa chén):");
                string chucVu = Console.ReadLine();
                while (chucVu != "bảo vệ" && chucVu != "thu ngân" && chucVu != "đầu bếp" && chucVu != "bưng bê" && chucVu != "rửa chén")
                {
                    Console.WriteLine("Chức vụ không hợp lệ. Vui lòng nhập lại:");
                    chucVu = Console.ReadLine();
                }
                nhanVien.chucvunv = chucVu;

                dsnv[i] = nhanVien;
            }

            ghitep(dsnv); // Ghi danh sách nhân viên vào tệp tin sau khi đã nhập xong thông tin cho tất cả các nhân viên
        }



        static void ghitep(kieunv[] danhSachNhanVien)
        {
            string filePath = "C:\\DSnhanvien.txt";

            // Sử dụng StreamWriter để ghi dữ liệu vào tệp
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                foreach (kieunv nhanVien in danhSachNhanVien)
                {
                    if (!string.IsNullOrEmpty(nhanVien.tennv)) // Chỉ ghi dữ liệu nếu tên nhân viên không rỗng
                    {
                        writer.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}", nhanVien.manv, nhanVien.tennv, nhanVien.tuoinv, nhanVien.gioitinhnv, nhanVien.quequannv, nhanVien.chucvunv);
                    }
                }
            }
        }

        static void nhapthem()
        {
            kieunv newdsnv = new kieunv();
            Console.WriteLine("Mời nhập mã nhân viên:");
            int maNV;
            while (!int.TryParse(Console.ReadLine(), out maNV))
            {
                Console.WriteLine("Mã nhân viên phải là số nguyên, mời nhập lại:");
            }
            newdsnv.manv = maNV;


            Console.WriteLine("Mời nhập họ và tên nhân viên:");
            newdsnv.tennv = Console.ReadLine();
            while (newdsnv.tennv == "")
            {
                Console.WriteLine("Mời nhập lại họ và tên nhân viên:");
                newdsnv.tennv = Console.ReadLine();
            }
            Console.Write("Mời nhập tuổi của nhân viên: ");
            int tuoi;
            while (!int.TryParse(Console.ReadLine(), out tuoi))
            {
                Console.WriteLine("Giá trị nhập vào không hợp lệ. Vui lòng nhập lại.");
                Console.Write("Mời nhập tuổi của nhân viên: ");
            }
            newdsnv.tuoinv = tuoi;


            Console.WriteLine("Mời nhập giới tính của nhân viên(Nam hoặc Nữ):");
            newdsnv.gioitinhnv = Console.ReadLine();
            while (newdsnv.gioitinhnv != "Nam" && newdsnv.gioitinhnv != "Nữ")
            {
                Console.WriteLine("Mời nhập lại giới tính nhân viên(Nam hoặc Nữ):");
                newdsnv.gioitinhnv = Console.ReadLine();
            }
            Console.WriteLine("Mời nhập quê quán nhân viên:");
            newdsnv.quequannv = Console.ReadLine();
            while (newdsnv.quequannv == "")
            {
                Console.WriteLine("Mời nhập lại quê quán nhân viên:");
                newdsnv.quequannv = Console.ReadLine();
            }
            Console.WriteLine("Mời nhập chức vụ(chỉ có thể nhận giá trị là 'bảo vệ' hoặc 'thu ngân' hoặc 'đầu bếp' hoặc 'rửa chén' hoặc 'bưng bê'");
            newdsnv.chucvunv = Console.ReadLine();
            while ((newdsnv.chucvunv != "bảo vệ") & (newdsnv.chucvunv != "thu ngân") & (newdsnv.chucvunv != "đầu bếp") & (newdsnv.chucvunv != "rửa chén") & (newdsnv.chucvunv != "bưng bê"))
            {
                Console.WriteLine("Mời nhập lại chức vụ(chỉ có thể nhận giá trị là 'bảo vệ' hoặc 'thu ngân' hoặc 'đầu bếp' hoặc 'rửa chén' hoặc 'bưng bê'");
                newdsnv.chucvunv = Console.ReadLine();
            }
            ghitep(dsnv);
            Console.WriteLine("Thêm thành công!");
        }
        static void TimKiem()
        {
            string duongDanFile = "C:\\DSnhanvien.txt";
            bool tiepTuc = true;

            while (tiepTuc)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n\n");
                Console.WriteLine(" ╔════════════════════════════════════════════════════════════════════╗                                           ");
                Console.WriteLine(" ║Nhập từ khóa cần tìm :                                              ║                                           ");
                Console.WriteLine(" ╚════════════════════════════════════════════════════════════════════╝                                           ");
                Console.SetCursorPosition(30, Console.CursorTop - 2); // Đặt con trỏ chuột tới cột 33, dòng trước đó 1 dòng
                Console.ResetColor();
                string tuKhoa = Console.ReadLine();
                string[] lines;
                lines = File.ReadAllLines(duongDanFile);

                bool timThay = false;
                Console.Clear();
                Console.WriteLine("\t\t\t\t\t╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("\t\t\t\t\t║                                                     DANH SÁCH TÌM KIẾM                                                           ║");
                Console.WriteLine("\t\t\t\t\t╠═══════════╦═══════════════════════════════╦═════════════╦═══════════════════╦══════════════════════╦═════════════════════════════╣");
                Console.WriteLine("\t\t\t\t\t║   Mã NV   ║            Tên NV             ║     Tuổi    ║     Giới Tính     ║       Quê Quán       ║           Chức Vụ           ║");
                Console.WriteLine("\t\t\t\t\t╠═══════════╩═══════════════════════════════╩═════════════╩═══════════════════╩══════════════════════╩═════════════════════════════╣");

                foreach (string line in lines)
                {
                    string[] values = line.Split('|');
                    if (values.Length >= 6)
                    {
                        string maNhanVien = values[0].Trim();
                        string hoTen = values[1].Trim();
                        string tuoi = values[2].Trim();
                        string gioiTinh = values[3].Trim();
                        string queQuan = values[4].Trim();
                        string chucVu = values[5].Trim();

                        if (maNhanVien.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                            hoTen.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                            tuoi.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                            gioiTinh.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                            queQuan.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase) ||
                            chucVu.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("\t\t\t\t\t║{0,-12}{1,-32}{2,-14}{3,-20}{4,-23}{5,-29}║", maNhanVien, hoTen, tuoi, gioiTinh, queQuan, chucVu);
                            timThay = true;
                            Console.ResetColor();
                        }
                    }
                }
                Console.WriteLine("\t\t\t\t\t╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");

                if (!timThay)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("\t\t\t                                   ╔════════════════════════════════════════════════════════════════════╗ ");
                    Console.WriteLine("\t\t\t                                   ║            KHÔNG TÌM THẤY THÔNG TIN NHÂN VIÊN BẠN CẦN!             ║ ");
                    Console.WriteLine("\t\t\t                                   ╚════════════════════════════════════════════════════════════════════╝ ");
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n\n\n\t\t\t                                    BẠN CÓ MUỐN TIẾP TỤC TÌM KIẾM? (Y/N):");
                Console.SetCursorPosition(97, Console.CursorTop);
                string luaChon = Console.ReadLine();
                Console.ResetColor();
                tiepTuc = luaChon.Equals("Y", StringComparison.OrdinalIgnoreCase);
                Console.Clear();
            }
        }
        static void hienthi(string duongDanFile)
        {
            string[] lines = File.ReadAllLines(duongDanFile);
            Console.Clear();
            Console.WriteLine("\t\t\t\t\t╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("\t\t\t\t\t║                                                     DANH SÁCH NHÂN VIÊN                                                          ║");
            Console.WriteLine("\t\t\t\t\t╠═══════════╦═══════════════════════════════╦═════════════╦═══════════════════╦══════════════════════╦═════════════════════════════╣");
            Console.WriteLine("\t\t\t\t\t║   Mã NV   ║            Tên NV             ║     Tuổi    ║     Giới Tính     ║       Quê Quán       ║           Chức Vụ           ║");
            Console.WriteLine("\t\t\t\t\t╠═══════════╩═══════════════════════════════╩═════════════╩═══════════════════╩══════════════════════╩═════════════════════════════╣");

            foreach (string line in lines)
            {
                string[] values = line.Split('|');

                if (values.Length >= 6)
                {
                    string maNhanVien = values[0].Trim();
                    string hoTen = values[1].Trim();
                    string tuoi = values[2].Trim();
                    string gioiTinh = values[3].Trim();
                    string queQuan = values[4].Trim();
                    string chucVu = values[5].Trim();

                    Console.WriteLine("\t\t\t\t\t║{0,-12}{1,-32}{2,-14}{3,-20}{4,-23}{5,-29}║", maNhanVien, hoTen, tuoi, gioiTinh, queQuan, chucVu);
                }
            }
            Console.WriteLine("\t\t\t\t\t╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
        }
        static string TimChucVuNhanVien(string duongDanFile, int maNhanVien)
        {
            string[] lines = File.ReadAllLines(duongDanFile);

            foreach (string line in lines)
            {
                string[] values = line.Split('|');

                if (values.Length >= 6)
                {
                    int maNV;
                    if (int.TryParse(values[0].Trim(), out maNV))
                    {
                        if (maNV == maNhanVien)
                        {
                            return values[5].Trim(); // Trả về chức vụ tìm thấy
                        }
                    }
                }
            }

            return null; // Không tìm thấy chức vụ
        }
        static string TimTenNhanVien(string duongDanFile, int maNhanVien)
        {
            string[] lines = File.ReadAllLines(duongDanFile);



            foreach (string line in lines)
            {
                string[] values = line.Split('|');

                if (values.Length >= 6)
                {
                    int maNV;
                    if (int.TryParse(values[0].Trim(), out maNV))
                    {
                        if (maNV == maNhanVien)
                        {
                            return values[1].Trim(); // Return the found job position
                        }
                    }
                }
            }

            return null; // Chuc vu not found
        }
        static void tinhluong()
        {
            string duongDanFile = "C:\\DSnhanvien.txt";
            bool tiepTuc = true;

            while (tiepTuc)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("\t\t\t\t\t\t\t║                                       BẢNG LƯƠNG                                      ║");
                Console.WriteLine("\t\t\t\t\t\t\t╠═══════════════════════╦═══════════╦════════════════╦═══════════════════╦══════════════╣");
                Console.WriteLine("\t\t\t\t\t\t\t║   Tên chức vụ         ║ Bậc lương ║ Theo giờ (1h)  ║ Lương cả ngày (8h)║ Tăng ca (1h) ║");
                Console.WriteLine("\t\t\t\t\t\t\t╠═══════════════════════╬═══════════╬════════════════╬═══════════════════╬══════════════╣");
                Console.WriteLine("\t\t\t\t\t\t\t║  Bảo vệ               ║     1     ║   18,750 VNĐ   ║   150,000 VNĐ     ║  37,500 VNĐ  ║");
                Console.WriteLine("\t\t\t\t\t\t\t║  Thu ngân             ║     1     ║   25,000 VNĐ   ║   200,000 VNĐ     ║  50,000 VNĐ  ║");
                Console.WriteLine("\t\t\t\t\t\t\t║  Chăm sóc khách hàng  ║     1     ║   31,250 VNĐ   ║   250,000 VNĐ     ║  62,500 VNĐ  ║");
                Console.WriteLine("\t\t\t\t\t\t\t║  Hỗ trợ kỹ thuật      ║     1     ║   50,000 VNĐ   ║   400,000 VNĐ     ║ 100,000 VNĐ  ║");
                Console.WriteLine("\t\t\t\t\t\t\t║  Bán hàng             ║     1     ║   31,250 VNĐ   ║   250,000 VNĐ     ║  62,500 VNĐ  ║");
                Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════╩═══════════╩════════════════╩═══════════════════╩══════════════╝");
                int maNhanVien;
                Console.Write("\t\t\t\t\t\t\tNhập mã nhân viên: ");
                maNhanVien = int.Parse((Console.ReadLine()));
                // Tiếp tục xử lý với mã nhân viên hợp lệ

                // Sử dụng mã nhân viên đã nhập để tìm thông tin và tính lương

                Console.ResetColor();
                // Tìm chức vụ của nhân viên trong file
                string chucVu = TimChucVuNhanVien(duongDanFile, maNhanVien);
                string tenNV = TimTenNhanVien(duongDanFile, maNhanVien);
                if (chucVu != null)
                {
                    Console.WriteLine("\n\n\n\n\n");
                    Console.WriteLine("\t\t\t\t\t\t\tTên nhân viên: " + tenNV);
                    Console.WriteLine("\t\t\t\t\t\t\tChức vụ: " + chucVu);

                    int ngayCong = 0;
                    bool isValidInput = false;

                    while (!isValidInput)
                    {
                        Console.WriteLine("\t\t\t\t\t\t\tMời nhập số ngày làm việc trong tháng:");
                        Console.SetCursorPosition(52 + 28 + 14, Console.CursorTop - 1);
                        string input = Console.ReadLine();

                        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out ngayCong) && ngayCong >= 1 && ngayCong <= 30)
                        {
                            isValidInput = true;
                        }
                        else
                        {
                            Console.WriteLine("\t\t\t\t\t\t\tSố ngày làm việc không hợp lệ. Vui lòng nhập lại!");
                        }
                    }
                    int gioTangCa = 0;

                    bool isValidInputt = false;

                    while (!isValidInputt)
                    {
                        Console.WriteLine("\t\t\t\t\t\t\tMời nhập số giờ tăng ca trong tháng:");
                        Console.SetCursorPosition(52 + 28 + 14, Console.CursorTop - 1);
                        string input = Console.ReadLine();

                        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out gioTangCa))
                        {
                            isValidInputt = true;
                        }
                        else
                        {
                            Console.WriteLine("\t\t\t\t\t\t\tSố giờ tăng ca không hợp lệ. Vui lòng nhập lại!");
                        }
                    }

                    float luongTheoGio;
                    float luongCaNgay;
                    float tienTangCa;

                    switch (chucVu.ToLower())
                    {
                        case "bảo vệ":
                            luongTheoGio = 30000;
                            luongCaNgay = luongTheoGio * 8;
                            tienTangCa = luongTheoGio * 2;

                            float tienLuong = ngayCong * luongCaNgay;
                            float tienTangCaTotal = gioTangCa * tienTangCa;
                            float tongLuong = tienLuong + tienTangCaTotal;
                            Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền lương: {0,-14} VNĐ                                                         ║", tienLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền tăng ca: {0,-14} VNĐ                                                       ║", tienTangCaTotal.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tổng lương: {0,-14} VNĐ                                                         ║", tongLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════════════════════════════════════════════════════════════════════╝");
                            break;

                        case "thu ngân":
                            luongTheoGio = 25000;
                            luongCaNgay = luongTheoGio * 8;
                            tienTangCa = luongTheoGio * 2;

                            tienLuong = ngayCong * luongCaNgay;
                            tienTangCaTotal = gioTangCa * tienTangCa;
                            tongLuong = tienLuong + tienTangCaTotal;

                            Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền lương: {0,-14} VNĐ                                                         ║", tienLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền tăng ca: {0,-14} VNĐ                                                       ║", tienTangCaTotal.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tổng lương: {0,-14} VNĐ                                                         ║", tongLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════════════════════════════════════════════════════════════════════╝");
                            break;
                        case "đầu bếp":
                            luongTheoGio = 60000;
                            luongCaNgay = luongTheoGio * 8;
                            tienTangCa = luongTheoGio * 2;

                            tienLuong = ngayCong * luongCaNgay;
                            tienTangCaTotal = gioTangCa * tienTangCa;
                            tongLuong = tienLuong + tienTangCaTotal;

                            Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền lương: {0,-14} VNĐ                                                         ║", tienLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền tăng ca: {0,-14} VNĐ                                                       ║", tienTangCaTotal.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tổng lương: {0,-14} VNĐ                                                         ║", tongLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════════════════════════════════════════════════════════════════════╝");
                            break;

                        case "bưng bê":
                            luongTheoGio = 20000;
                            luongCaNgay = luongTheoGio * 8;
                            tienTangCa = luongTheoGio * 2;

                            tienLuong = ngayCong * luongCaNgay;
                            tienTangCaTotal = gioTangCa * tienTangCa;
                            tongLuong = tienLuong + tienTangCaTotal;

                            Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền lương: {0,-14} VNĐ                                                         ║", tienLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền tăng ca: {0,-14} VNĐ                                                       ║", tienTangCaTotal.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tổng lương: {0,-14} VNĐ                                                         ║", tongLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════════════════════════════════════════════════════════════════════╝");
                            break;

                        case "rửa chén":
                            luongTheoGio = 20000;
                            luongCaNgay = luongTheoGio * 8;
                            tienTangCa = luongTheoGio * 2;

                            tienLuong = ngayCong * luongCaNgay;
                            tienTangCaTotal = gioTangCa * tienTangCa;
                            tongLuong = tienLuong + tienTangCaTotal;

                            Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền lương: {0,-14} VNĐ                                                         ║", tienLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tiền tăng ca: {0,-14} VNĐ                                                       ║", tienTangCaTotal.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t║Tổng lương: {0,-14} VNĐ                                                         ║", tongLuong.ToString("N0"));
                            Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════════════════════════════════════════════════════════════════════╝");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\n\n\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\t\t\t\t\t╔═══════════════════════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("\t\t\t\t\t\t\t║ Không tìm thấy thông tin nhân viên với mã:{0,-8}                                    ║", maNhanVien);
                    Console.WriteLine("\t\t\t\t\t\t\t╚═══════════════════════════════════════════════════════════════════════════════════════╝");
                    Console.ResetColor();
                }

                // Hỏi người dùng có muốn tính lương tiếp hay không
                Console.WriteLine("\n\n\n");
                Console.WriteLine("\t\t\t\t\t\t\tBạn có muốn tính lương tiếp không? (Y/N):");
                Console.SetCursorPosition(52 + 28 + 18, Console.CursorTop - 1);
                string luaChon = Console.ReadLine();

                if (luaChon.ToLower() != "y")
                {

                    tiepTuc = false;

                }
                Console.Clear();
                Console.ResetColor();
            }
        }
        static int ChonMenu()
        {
            Console.Clear();
            Console.WriteLine("    ╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("    ║                                                                                                                                                   ║");
            Console.WriteLine("    ║                                  ╔════════════════════════════════════════════════════════════════════╗                                           ║");
            Console.WriteLine("    ║                                  ║               CHƯƠNG TRÌNH QUẢN LÝ NHÀ HÀNG SEN HỒ                 ║                                           ║");
            Console.WriteLine("    ║                                  ╚════════════════════════════════════════════════════════════════════╝                                           ║");
            Console.WriteLine("    ║                                  ╔════════════════════════════════════════════════════════════════════╗                                           ║");
            Console.WriteLine("    ║                                  ║                                 MENU                               ║                                           ║");
            Console.WriteLine("    ║                                  ╚════════════════════════════════════════════════════════════════════╝                                           ║");
            Console.WriteLine("    ║                                                                                                                                                   ║");
            Console.WriteLine("    ║                                                                                                                                                   ║");
            Console.WriteLine("    ║                            ╔════════════════════════════════════╗       ╔════════════════════════════════════╗                                    ║");
            Console.WriteLine("    ║                            ║ 1. Nhập danh sách nhân viên        ║       ║ 2. Tính lương cho nhân viên        ║                                    ║");
            Console.WriteLine("    ║                            ╚════════════════════════════════════╝       ╚════════════════════════════════════╝                                    ║");
            Console.WriteLine("    ║                            ╔════════════════════════════════════╗       ╔════════════════════════════════════╗                                    ║");
            Console.WriteLine("    ║                            ║ 3. Hiển thị danh sách nhân viên    ║       ║ 4. Thêm nhân viên                  ║                                    ║");
            Console.WriteLine("    ║                            ╚════════════════════════════════════╝       ╚════════════════════════════════════╝                                    ║");
            Console.WriteLine("    ║                            ╔════════════════════════════════════╗       ╔════════════════════════════════════╗                                    ║");
            Console.WriteLine("    ║                            ║ 5. Xóa nhân viên                   ║       ║ 6. Tìm kiếm nhân viên              ║                                    ║");
            Console.WriteLine("    ║                            ╚════════════════════════════════════╝       ╚════════════════════════════════════╝                                    ║");
            Console.WriteLine("    ║                            ╔════════════════════════════════════╗       ╔════════════════════════════════════╗                                    ║");
            Console.WriteLine("    ║                            ║ 7. Thống kê                        ║       ║ 8. Cập nhật nhân viên              ║                                    ║");
            Console.WriteLine("    ║                            ╚════════════════════════════════════╝       ╚════════════════════════════════════╝                                    ║");
            Console.WriteLine("    ║                            ╔════════════════════════════════════╗       ╔════════════════════════════════════╗                                    ║");
            Console.WriteLine("    ║                            ║ 9. Kiểm góp ý                      ║       ║ 10. Quay lại                       ║                                    ║");
            Console.WriteLine("    ║                            ╚════════════════════════════════════╝       ╚════════════════════════════════════╝                                    ║");
            Console.WriteLine("    ║                            ╔════════════════════════════════════╗       ╔════════════════════════════════════╗                                    ║");
            Console.WriteLine("    ║                            ║ 11. Thoát                          ║       ║ 12. Thêm chức năng mới             ║                                    ║");
            Console.WriteLine("    ║                            ╚════════════════════════════════════╝       ╚════════════════════════════════════╝                                    ║");
            Console.WriteLine("    ╠════════════════════════════════════════════════════════════════╦══════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine("    ║                                                                ║");
            Console.WriteLine("    ║        BẠN ĐÃ ĐĂNG NHẬP THÀNH CÔNG XIN MỜI LỰA CHỌN :          ║");
            Console.WriteLine("    ║                                                                ║");
            Console.WriteLine("    ╚════════════════════════════════════════════════════════════════╝");

            Console.SetCursorPosition(63, Console.CursorTop - 3);
            int chon;
            do
            {
                chon = int.Parse(Console.ReadLine());
            } while (!(chon >= 1 && chon <= 20));
            Console.Clear();
            return chon;
        }
        static void menu()
        {
            do
            {
                switch (ChonMenu())
                {

                    case 1:
                        Console.Clear();
                        nhap();
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        tinhluong();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;


                    case 3:
                        Console.Clear();
                        hienthi("C:\\DSnhanvien.txt");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.Clear();
                        nhapthem();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;

                    case 5:
                        xoachinh();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;

                    case 6:
                        Console.Clear();
                        TimKiem();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;
                    case 7:
                        ThongKe();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;
                    case 8:
                        capNhatchinh();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;
                    case 9:
                        DocTepGopY();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("   ╔═══════════════════════════════╗");
                        Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                        Console.WriteLine("   ╚═══════════════════════════════╝");
                        Console.ReadKey();
                        break;
                    case 10: return;
                    case 11: Environment.Exit(0); break;
                    case 12:

                        bool isBlue = false;

                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("  \n\n\n\n\n\n\n\n\n\n                                              ╔════════════════════════════════════════════════════════════════════╗                                           ");
                            Console.WriteLine("                                              ║   CHƯƠNG NĂNG CHƯA ĐƯỢC CẬP NHẬT VUI LÒNG ĐỢI PHIÊN BẢN TIẾP THEO  ║                                           ");
                            Console.WriteLine("                                              ╚════════════════════════════════════════════════════════════════════╝                                           ");

                            if (isBlue)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("\n\n\t\t\t\t\t\t\t\t\t╔═════════════╗");
                                Console.WriteLine("\t\t\t\t\t\t\t\t\t║   QUAY LẠI  ║");
                                Console.WriteLine("\t\t\t\t\t\t\t\t\t╚═════════════╝");
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("\n\n\t\t\t\t\t\t\t\t\t╔═════════════╗");
                                Console.WriteLine("\t\t\t\t\t\t\t\t\t║   QUAY LẠI  ║");
                                Console.WriteLine("\t\t\t\t\t\t\t\t\t╚═════════════╝");
                            }

                            Console.ResetColor();
                            System.Threading.Thread.Sleep(500); // Delay 500 milliseconds

                            isBlue = !isBlue;
                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    returnToMain = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        Console.Clear();

                        Console.WriteLine("  LỰA CHỌN KHÔNG HỢP LỆ !   ");
                        Console.WriteLine();
                        Console.ReadKey();
                        Console.WriteLine();
                        break;

                }
                Console.Clear();
            } while (true);
        }
        static void xoachinh()
        {
            string duongDanFile = "C:\\DSnhanvien.txt";
            bool tiepTuc = true;

            while (tiepTuc)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n\n");
                Console.WriteLine("\t\t\t                                   ╔════════════════════════════════════════════════════════════════════╗                                           ");
                Console.WriteLine("\t\t\t                                   ║Nhập mã nhân viên cần xóa:                                          ║                                           ");
                Console.WriteLine("\t\t\t                                   ╚════════════════════════════════════════════════════════════════════╝                                           ");
                Console.SetCursorPosition(86, Console.CursorTop - 2); // Đặt con trỏ chuột tới cột 79, dòng trước đó 2 dòng
                string maNV = Console.ReadLine();
                Console.ResetColor();
                // Đọc danh sách nhân viên từ file
                string[] lines;
                lines = File.ReadAllLines(duongDanFile);

                bool daXoa = false;

                // Ghi lại danh sách nhân viên sau khi xóa vào cùng file
                using (StreamWriter writer = new StreamWriter(duongDanFile))
                {
                    foreach (string line in lines)
                    {

                        string[] values = line.Split('|');

                        // Kiểm tra mã nhân viên để xác định xem có xóa hay không
                        string maNhanVien = values[0].Trim();
                        if (!maNhanVien.Equals(maNV, StringComparison.OrdinalIgnoreCase))
                        {
                            writer.WriteLine(line);
                        }
                        else
                        {
                            daXoa = true;
                        }
                    }
                }

                Console.Clear();
                if (daXoa)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t\t\t                                   ╔══════════════════════════════════════════════════════════════════╗ ");
                    Console.WriteLine("\t\t\t                                   ║                Đã xóa nhân viên thành công!                      ║ ");
                    Console.WriteLine("\t\t\t                                   ╚══════════════════════════════════════════════════════════════════╝ ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\t                                   ╔══════════════════════════════════════════════════════════════════╗ ");
                    Console.WriteLine("\t\t\t                                   ║          Không tìm thấy nhân viên với mã cần xóa!                ║ ");
                    Console.WriteLine("\t\t\t                                   ╚══════════════════════════════════════════════════════════════════╝ ");
                }
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n\n\n\t\t\t                                    BẠN CÓ MUỐN TIẾP TỤC XÓA NHÂN VIÊN? (Y/N):");
                Console.SetCursorPosition(102, Console.CursorTop);
                string luaChon = Console.ReadLine();
                Console.ResetColor();
                tiepTuc = luaChon.Equals("Y", StringComparison.OrdinalIgnoreCase);
                Console.Clear();
            }
        }
        static int chonmenutk()
        {
            int selectedOption = 1;
            bool optionSelected = false;
            while (!optionSelected)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════╗ ");
                Console.WriteLine("║           MENU THỐNG KÊ           ║ ");
                Console.WriteLine("╚═══════════════════════════════════╝ ");

                Console.WriteLine("╔═══════════════════════════════════╗ ");
                Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║       1. THỐNG KÊ THEO SỐ LƯỢNG   ║ ");
                Console.ResetColor();
                Console.WriteLine("╚═══════════════════════════════════╝ ");

                Console.WriteLine("╔═══════════════════════════════════╗ ");
                Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║       2. THỐNG KÊ GIỚI TÍNH       ║ ");
                Console.ResetColor();
                Console.WriteLine("╚═══════════════════════════════════╝ ");

                Console.WriteLine("╔═══════════════════════════════════╗ ");
                Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║       3. THỐNG KÊ QUÊ QUÁN        ║ ");
                Console.ResetColor();
                Console.WriteLine("╚═══════════════════════════════════╝ ");

                Console.WriteLine("╔═══════════════════════════════════╗ ");
                Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║       4. THỐNG KÊ CHỨC VỤ         ║ ");
                Console.ResetColor();
                Console.WriteLine("╚═══════════════════════════════════╝ ");

                Console.WriteLine("╔═══════════════════════════════════╗ ");
                Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║        5. QUAY LẠI                ║ ");
                Console.ResetColor();
                Console.WriteLine("╚═══════════════════════════════════╝ ");
                Console.WriteLine();
                Console.WriteLine("    ╔══════════════════════════════════════════════════╗");
                Console.WriteLine("    ║       DÙNG NÚT DI CHUYỂN ĐỂ CHỌN ! THANK YOU     ║");
                Console.WriteLine("    ╚══════════════════════════════════════════════════╝");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedOption > 1)
                            selectedOption--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedOption < 5)
                            selectedOption++;
                        break;
                    case ConsoleKey.Enter:
                        optionSelected = true;
                        break;
                }
            }
            return selectedOption;
        }
        static void ThongKe()
        {
            while (true)
            {
                switch (chonmenutk())
                {
                    case 1:
                        Console.Clear();
                        ThongKeSoLuong();
                        break;
                    case 2:
                        Console.Clear();
                        ThongKeGioiTinh();
                        Console.ReadKey();
                        break;

                    case 3:
                        Console.Clear();
                        ThongKeQueQuan();
                        Console.ReadKey();
                        break;

                    case 4:
                        Console.Clear();
                        ThongKeChucVu();
                        Console.ReadKey();
                        break;

                    case 5:
                        menu();
                        break;
                }
            }
        }
        static void ThongKeSoLuong()
        {
            string duongDanFile = "C:\\DSnhanvien.txt";

            // Đọc danh sách nhân viên từ file
            string[] lines = File.ReadAllLines(duongDanFile);

            // Khởi tạo các biến đếm và tổng
            int soLuongNhanVien = 0;
            int soLuongNam = 0;
            int soLuongNu = 0;
            int cv1 = 0;
            int cv2 = 0;
            int cv3 = 0;
            int cv4 = 0;
            int cv5 = 0;

            // Duyệt qua danh sách nhân viên và thực hiện thống kê
            foreach (string line in lines)
            {

                string[] values = line.Split('|');

                if (values.Length >= 6) // Kiểm tra số lượng thông tin đủ để thống kê
                {
                    soLuongNhanVien++;
                    // Thống kê theo giới tính
                    string gioiTinh = values[3].Trim().ToLower();
                    if (gioiTinh.Equals("Nam", StringComparison.OrdinalIgnoreCase))
                    {
                        soLuongNam++;
                    }
                    else if (gioiTinh.Equals("Nữ", StringComparison.OrdinalIgnoreCase))
                    {
                        soLuongNu++;
                    }

                    // Thống kê theo chức vụ
                    string chucVu = values[5].Trim();
                    if (chucVu.Equals("bảo vệ", StringComparison.OrdinalIgnoreCase))
                    {
                        cv1++;
                    }
                    else if (chucVu.Equals("thu ngân", StringComparison.OrdinalIgnoreCase))
                    {
                        cv2++;
                    }
                    else if (chucVu.Equals("đầu bếp", StringComparison.OrdinalIgnoreCase))
                    {
                        cv3++;
                    }
                    else if (chucVu.Equals("bưng bê", StringComparison.OrdinalIgnoreCase))
                    {
                        cv4++;
                    }
                    else if (chucVu.Equals("rửa chén", StringComparison.OrdinalIgnoreCase))
                    {
                        cv5++;
                    }
                }
            }

            // Hiển thị thông tin thống kê dưới dạng bảng đơn giản
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine("\t\t\t    ╔════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("\t\t\t    ║                             THỐNG KÊ SỐ LƯỢNG                              ║");
            Console.WriteLine("\t\t\t    ╠════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("\t\t\t    ║Số lượng nhân viên:{0,-57}║", soLuongNhanVien);
            Console.WriteLine("\t\t\t    ║Số lượng nam:{0,-63}║", soLuongNam);
            Console.WriteLine("\t\t\t    ║Số lượng nữ: {0,-63}║", soLuongNu);
            Console.WriteLine("\t\t\t    ║Số lượng nhân viên chức vụ BẢO VỆ:{0,-42}║", cv1);
            Console.WriteLine("\t\t\t    ║Số lượng nhân viên chức vụ THU NGÂN: {0,-38} ║", cv2);
            Console.WriteLine("\t\t\t    ║Số lượng nhân viên chức vụ ĐẦU BẾP:{0,-29}            ║", cv3);
            Console.WriteLine("\t\t\t    ║Số lượng nhân viên chức vụ BƯNG BÊ:{0,-40} ║", cv4);
            Console.WriteLine("\t\t\t    ║Số lượng nhân viên chức vụ RỬA CHÉN:{0,-33}       ║", cv5);
            Console.WriteLine("\t\t\t    ╚════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("   ╔═══════════════════════════════╗");
            Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
            Console.WriteLine("   ╚═══════════════════════════════╝");
            Console.ReadKey();
        }
        static void ThongKeGioiTinh()
        {
            string file = "C:\\DSnhanvien.txt"; // Đường dẫn đúng đến tệp tin DSnhanvien
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);

                Console.WriteLine("╔════════╦═════════════════════════╦═══════════╗");
                Console.WriteLine("║  Mã NV ║     Tên NV              ║ Giới Tính ║");
                Console.WriteLine("╠════════╬═════════════════════════╬═══════════╣");
                foreach (string line in lines)
                {
                    string[] values = line.Split('|');

                    if (values.Length >= 6)
                    {
                        string maNhanVien = values[0].Trim();
                        string tenNhanVien = values[1].Trim();
                        string gioiTinh = values[3].Trim();

                        if (gioiTinh.Equals("Nam", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"║{maNhanVien,-8}║{tenNhanVien,-25}║{gioiTinh,-11}║");
                        }
                    }
                }
                Console.WriteLine("╠════════╬═════════════════════════╬═══════════╣");
                foreach (string line in lines)
                {
                    string[] values = line.Split('|');

                    if (values.Length >= 6)
                    {
                        string maNhanVien = values[0].Trim();
                        string tenNhanVien = values[1].Trim();
                        string gioiTinh = values[3].Trim();

                        if (gioiTinh.Equals("Nữ", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"║{maNhanVien,-8}║{tenNhanVien,-25}║{gioiTinh,-11}║");
                        }
                    }
                }
                Console.WriteLine("╚════════╩═════════════════════════╩═══════════╝");
            }
            else
            {
                Console.WriteLine("Không tìm thấy tệp tin DSnhanvien.");
            }
        }
        static void ThongKeChucVu()
        {
            string file = "C:\\DSnhanvien.txt";
            string[] lines = File.ReadAllLines(file);
            Console.WriteLine("╔══════════════╦════════════════════════════════════════╦══════════════════════════╗");
            Console.WriteLine("║     Mã NV    ║                 Tên NV                 ║ Chức Vụ                  ║");
            Console.WriteLine("╠══════════════╬════════════════════════════════════════╬══════════════════════════╣");

            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 6)
                {
                    string manv = values[0].Trim();
                    string ten = values[1].Trim();
                    string chucvu = values[5].Trim();
                    if (chucvu.Equals("bảo vệ", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("║{0,-14}║{1,-40}║{2,-26}║", manv, ten, chucvu);
                    }
                }

            }
            Console.WriteLine("╠══════════════╬════════════════════════════════════════╬══════════════════════════╣");
            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 6)
                {
                    string manv = values[0].Trim();
                    string ten = values[1].Trim();
                    string chucvu = values[5].Trim();
                    if (chucvu.Equals("thu ngân", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("║{0,-14}║{1,-40}║{2,-26}║", manv, ten, chucvu);
                    }
                }

            }
            Console.WriteLine("╠══════════════╬════════════════════════════════════════╬══════════════════════════╣");
            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 6)
                {
                    string manv = values[0].Trim();
                    string ten = values[1].Trim();
                    string chucvu = values[5].Trim();
                    if (chucvu.Equals("đầu bếp", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("║{0,-14}║{1,-40}║{2,-26}║", manv, ten, chucvu);
                    }
                }

            }
            Console.WriteLine("╠══════════════╬════════════════════════════════════════╬══════════════════════════╣");
            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 6)
                {
                    string manv = values[0].Trim();
                    string ten = values[1].Trim();
                    string chucvu = values[5].Trim();
                    if (chucvu.Equals("bưng bê", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("║{0,-14}║{1,-40}║{2,-26}║", manv, ten, chucvu);
                    }
                }

            }
            Console.WriteLine("╠══════════════╬════════════════════════════════════════╬══════════════════════════╣");
            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 6)
                {
                    string manv = values[0].Trim();
                    string ten = values[1].Trim();
                    string chucvu = values[5].Trim();
                    if (chucvu.Equals("rửa chén", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("║{0,-14}║{1,-40}║{2,-26}║", manv, ten, chucvu);
                    }
                }
            }
            Console.WriteLine("╚══════════════╩════════════════════════════════════════╩══════════════════════════╝");
        }
        static void ThongKeQueQuan()
        {
            string file = "C:\\DSnhanvien.txt";
            string[] lines = File.ReadAllLines(file);

            Console.WriteLine("╔══════════════╦════════════════════════════════════════╦══════════════════════════╗");
            Console.WriteLine("║     Mã NV    ║                 Tên NV                 ║ Quê quán                 ║");
            Console.WriteLine("╠══════════════╬════════════════════════════════════════╬══════════════════════════╣");

            Dictionary<string, List<string>> queQuanDict = new Dictionary<string, List<string>>();

            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                if (values.Length >= 6)
                {
                    string maNV = values[0].Trim();
                    string tenNV = values[1].Trim();
                    string queQuan = values[4].Trim();

                    if (!queQuanDict.ContainsKey(queQuan))
                    {
                        queQuanDict[queQuan] = new List<string>();
                    }

                    queQuanDict[queQuan].Add(string.Format("║{0,-14}║{1,-40}║{2,-26}║", maNV, tenNV, queQuan));
                }
            }

            foreach (var kvp in queQuanDict)
            {
                List<string> queQuanLines = kvp.Value;
                queQuanLines.Sort();

                foreach (string queQuanLine in queQuanLines)
                {
                    Console.WriteLine(queQuanLine);
                }
            }

            Console.WriteLine("╚══════════════╩════════════════════════════════════════╩══════════════════════════╝");
        }
        static void capNhatchinh()
        {
            string duongDanFile = "C:\\DSnhanvien.txt";
            bool tiepTuc = true;

            while (tiepTuc)
            {
                Console.Clear();
                string[] lines = File.ReadAllLines(duongDanFile);
                Console.WriteLine("\n\n\t\t\t                                   ╔════════════════════════════════════════════════════════════════════╗                                           ");
                Console.WriteLine("\t\t\t                                   ║Nhập mã nhân viên cần cập nhật:                                     ║                                           ");
                Console.WriteLine("\t\t\t                                   ╚════════════════════════════════════════════════════════════════════╝                                           ");
                Console.SetCursorPosition(91, Console.CursorTop - 2); // Đặt con trỏ chuột tới cột 38, dòng trước đó 1 dòng
                string maNhanVienCanSua = Console.ReadLine().Trim();

                bool timThay = false;
                foreach (string line in lines)
                {
                    string[] values = line.Trim().Split('|');
                    if (values.Length >= 6)
                    {
                        string maNhanVien = values[0].Trim();
                        string tenNhanVien = values[1];
                        string tuoi = values[2];
                        string gioiTinh = values[3];
                        string queQuan = values[4];
                        string chucVu = values[5];

                        if (maNhanVien.Equals(maNhanVienCanSua, StringComparison.OrdinalIgnoreCase))
                        {

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t╔══════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║                        THÔNG TIN CŨ                          ║");
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t╠══════════════════════════════════════════════════════════════╣");

                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║Mã nhân viên:{0,-43}      ║", maNhanVien);
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║Tên nhân viên:{0,-43}     ║", tenNhanVien);
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║Tuổi:{0,-43}              ║", tuoi);
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║Giới tính:{0,-43}         ║", gioiTinh);
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║Quê quán:{0,-43}          ║", queQuan);
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t║Chức Vụ:{0,-43}           ║", chucVu);

                            Console.WriteLine("\t\t\t\t\t\t\t\t\t╚══════════════════════════════════════════════════════════════╝");

                            Console.ResetColor();
                            timThay = true;
                            break;
                        }
                    }
                }

                if (!timThay)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("\t\t\t                                   ╔════════════════════════════════════════════════════════════════════╗ ");
                    Console.WriteLine("\t\t\t                                   ║            KHÔNG TÌM THẤY THÔNG TIN NHÂN VIÊN BẠN CẦN!             ║ ");
                    Console.WriteLine("\t\t\t                                   ╚════════════════════════════════════════════════════════════════════╝ ");
                    Console.ResetColor();
                }
                else
                {



                    Console.WriteLine("\n\n\n");
                    Console.WriteLine("\t\t\t\t\t\t\t\t\t╔══════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("\t\t\t\t\t\t\t\t\t║               NHẬP THÔNG TIN MỚI ĐỂ CẬP NHẬT                     ║");
                    Console.WriteLine("\t\t\t\t\t\t\t\t\t╚══════════════════════════════════════════════════════════════════╝");




                    Console.Write("\t\t\t\t\t\t\t\t\tMã nhân viên: ");
                    string maNhanVienMoi = Console.ReadLine();

                    while (string.IsNullOrEmpty(maNhanVienMoi) || !int.TryParse(maNhanVienMoi, out _))
                    {
                        Console.WriteLine("\t\t\t\t\t\t\t\t\tMã nhân viên phải là số nguyên và không được để trống.");
                        Console.Write("\t\t\t\t\t\t\t\t\tNhập lại mã nhân viên: ");
                        maNhanVienMoi = Console.ReadLine();
                    }

                    Console.Write("\t\t\t\t\t\t\t\t\tTên nhân viên: ");
                    string tenNhanVienMoi = Console.ReadLine();

                    // Kiểm tra tên nhân viên không được để trống và không được chứa số
                    if (string.IsNullOrWhiteSpace(tenNhanVienMoi) || tenNhanVienMoi.Any(char.IsDigit))
                    {
                        Console.WriteLine("\t\t\t\t\t\t\t\t\tTên nhân viên không được để trống và không được chứa số.");
                        // Tiếp tục yêu cầu người dùng nhập lại giá trị
                        do
                        {
                            Console.Write("\t\t\t\t\t\t\t\t\tTên nhân viên: ");
                            tenNhanVienMoi = Console.ReadLine();
                        } while (string.IsNullOrWhiteSpace(tenNhanVienMoi) || tenNhanVienMoi.Any(char.IsDigit));
                    }

                    Console.Write("\t\t\t\t\t\t\t\t\tTuổi: ");
                    string tuoiMoiStr = Console.ReadLine();
                    int tuoiMoi;

                    // Kiểm tra tuổi phải là số nguyên
                    if (!int.TryParse(tuoiMoiStr, out tuoiMoi))
                    {
                        Console.WriteLine("\t\t\t\t\t\t\t\t\tTuổi phải là số nguyên.");
                        // Tiếp tục yêu cầu người dùng nhập lại giá trị
                        do
                        {
                            Console.Write("\t\t\t\t\t\t\t\t\tTuổi: ");
                            tuoiMoiStr = Console.ReadLine();
                        } while (!int.TryParse(tuoiMoiStr, out tuoiMoi));
                    }

                    Console.Write("\t\t\t\t\t\t\t\t\tGiới tính: ");
                    string gioiTinhMoi = Console.ReadLine();

                    // Kiểm tra giới tính phải là "nam" hoặc "nữ"
                    if (!string.Equals(gioiTinhMoi, "nam", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(gioiTinhMoi, "nữ", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("\t\t\t\t\t\t\t\t\tGiới tính phải là 'nam' hoặc 'nữ'.");
                        // Tiếp tục yêu cầu người dùng nhập lại giá trị
                        do
                        {
                            Console.Write("\t\t\t\t\t\t\t\t\tGiới tính: ");
                            gioiTinhMoi = Console.ReadLine();
                        } while (!string.Equals(gioiTinhMoi, "nam", StringComparison.OrdinalIgnoreCase) &&
                                 !string.Equals(gioiTinhMoi, "nữ", StringComparison.OrdinalIgnoreCase));
                    }

                    Console.Write("\t\t\t\t\t\t\t\t\tQuê quán: ");
                    string queQuanMoi = Console.ReadLine();

                    // Kiểm tra quê quán không được để trống và không chứa số
                    if (string.IsNullOrEmpty(queQuanMoi) || queQuanMoi.Any(char.IsDigit))
                    {
                        Console.WriteLine("\t\t\t\t\t\t\t\t\tQuê quán không được để trống và không chứa số.");
                        // Tiếp tục yêu cầu người dùng nhập lại giá trị
                        do
                        {
                            Console.Write("\t\t\t\t\t\t\t\t\tQuê quán: ");
                            queQuanMoi = Console.ReadLine();
                        } while (string.IsNullOrEmpty(queQuanMoi) || queQuanMoi.Any(char.IsDigit));
                    }


                    Console.Write("\t\t\t\t\t\t\t\t\tChức vụ: ");
                    string chucVuMoi = Console.ReadLine();

                    // Kiểm tra chức vụ phải nằm trong danh sách cho phép
                    string[] danhSachChucVu = { "bảo vệ", "thu ngân", "đầu bếp", "bưng bê", "rửa chén" };
                    if (!danhSachChucVu.Contains(chucVuMoi.ToLower()))
                    {
                        Console.WriteLine("\t\t\t\t\t\t\t\t\tChức vụ không hợp lệ(chỉ có thể nhận giá trị là 'bảo vệ' hoặc 'thu ngân' hoặc 'đầu bếp' hoặc 'bưng bê' hoặc 'rửa chén')");
                        // Tiếp tục yêu cầu người dùng nhập lại giá trị
                        do
                        {
                            Console.Write("\t\t\t\t\t\t\t\t\tChức vụ: ");
                            chucVuMoi = Console.ReadLine();
                        } while (!danhSachChucVu.Contains(chucVuMoi.ToLower()));
                    }
                    // Sử dụng giá trị mã nhân viên để cập nhật thông tin trong file                   
                    // Cập nhật thông tin trong danh sách
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];

                        string[] values = line.Split('|');

                        if (values.Length >= 6 && values[0].Trim().Equals(maNhanVienCanSua, StringComparison.OrdinalIgnoreCase))
                        {
                            string newLine = $"{maNhanVienMoi,-12}|{tenNhanVienMoi,-32}|{tuoiMoi,-14}|{gioiTinhMoi,-20}|{queQuanMoi,-23}|{chucVuMoi,-29}";
                            lines[i] = newLine; // Gán giá trị mới vào phần tử trong mảng lines
                            break;
                        }
                    }
                    // Ghi lại danh sách đã cập nhật vào file
                    File.WriteAllLines(duongDanFile, lines);

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("\t\t\t                                   ╔════════════════════════════════════════════════════════════════════╗ ");
                    Console.WriteLine("\t\t\t                                   ║               CẬP NHẬT THÔNG TIN NHÂN VIÊN THÀNH CÔNG!             ║ ");
                    Console.WriteLine("\t\t\t                                   ╚════════════════════════════════════════════════════════════════════╝ ");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n\n\n\t\t\t                                    BẠN CÓ MUỐN TIẾP TỤC CẬP NHẬT THÔNG TIN NHÂN VIÊN? (C/K): ");
                string luaChon = Console.ReadLine();
                tiepTuc = (luaChon.Equals("C", StringComparison.OrdinalIgnoreCase));
                Console.ResetColor();
            }
        }
        static Dictionary<string, string> users = new Dictionary<string, string>();
        static Dictionary<string, string> userRoles = new Dictionary<string, string>(); // Lưu trữ thông tin vai trò của từng tài khoản
        static string filePath = "users.txt";
        static bool returnToMain = false;
        static void LoadUsersFromFile()
        {
            if (File.Exists(filePath))
            {
                users.Clear(); // Xóa danh sách người dùng hiện tại để đọc lại từ file

                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');

                    // Kiểm tra xem mảng có đủ phần tử hay không
                    if (parts.Length >= 2)
                    {
                        string username = parts[0];
                        string password = parts[1];
                        users.Add(username, password);
                    }
                    else
                    {
                        Console.WriteLine("Danh sách tài khoản : " + line);
                    }
                }
            }
        }
        static void SaveUsersToFile()
        {
            List<string> lines = new List<string>();
            foreach (var user in users)
            {
                string username = user.Key;
                string password = user.Value;
                string role = userRoles[username]; // Lấy vai trò của tài khoản

                string line = $"{username};{password};{role}";
                lines.Add(line);
            }

            File.WriteAllLines(filePath, lines);
        }
        static void RegisterAdmin()
        {
            Console.Clear();

            Console.WriteLine("   ╔══════════════════╗  ");
            Console.WriteLine("   ║   ĐĂNG KÝ ADMIN  ║  ");
            Console.WriteLine("   ╚══════════════════╝");

            string username = "";
            string password = "";

            while (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   Tài khoản:                                        ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.SetCursorPosition(20, Console.CursorTop - 2);
                username = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   Mật khẩu:                                         ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.SetCursorPosition(20, Console.CursorTop - 2);
                password = Console.ReadLine();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.Clear();
                    Console.WriteLine(" TÀI KHOẢN KHÔNG HỢP LỆ !");
                    Console.WriteLine();
                    Console.WriteLine("   ╔══════════════════╗");
                    Console.WriteLine("   ║   ĐĂNG KÝ ADMIN  ║  ");
                    Console.WriteLine("   ╚══════════════════╝");
                }
            }

            users[username] = password;
            userRoles[username] = "admin"; // Thiết lập vai trò là admin
            Console.Clear();
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Đăng ký tài khoản ADMIN thành công!               ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("   ╔═══════════════════════════════╗");
            Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
            Console.WriteLine("   ╚═══════════════════════════════╝");

            Console.ReadLine();
            Console.Clear();


        }

        static void RegisterUser()
        {
            Console.Clear();

            Console.WriteLine("   ╔══════════════════╗  ");
            Console.WriteLine("   ║   ĐĂNG KÝ USER   ║  ");
            Console.WriteLine("   ╚══════════════════╝");

            string username = "";
            string password = "";

            while (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   Tài khoản:                                        ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.SetCursorPosition(20, Console.CursorTop - 2);
                username = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   Mật khẩu:                                         ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.SetCursorPosition(20, Console.CursorTop - 2);
                password = Console.ReadLine();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.Clear();
                    Console.WriteLine("TÀI KHOẢN KHÔNG HỢP LỆ !");
                    Console.WriteLine();
                    Console.WriteLine("   ╔══════════════════╗");
                    Console.WriteLine("   ║   ĐĂNG KÝ USER   ║  ");
                    Console.WriteLine("   ╚══════════════════╝");
                }
            }

            users[username] = password;
            userRoles[username] = "user"; // Thiết lập vai trò là user
            Console.Clear();
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Đăng ký tài khoản người dùng thành công!          ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("   ╔═══════════════════════════════╗");
            Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
            Console.WriteLine("   ╚═══════════════════════════════╝");

            Console.ReadLine();
            Console.Clear();


        }

        static string Login(string role)
        {
            Console.Clear();

            Console.WriteLine("   ╔═══════════════╗  ");
            Console.WriteLine("   ║      LOGIN    ║  ");
            Console.WriteLine("   ╚═══════════════╝");
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Tài khoản:                                        ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(20, Console.CursorTop - 2);
            string username = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Mật khẩu:                                         ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(20, Console.CursorTop - 2);
            string password = "";
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();

            if (users.ContainsKey(username))
            {
                if (users[username] == password && userRoles[username] == role) // Kiểm tra vai trò của tài khoản
                {
                    Console.Clear();
                    Console.WriteLine("Đăng nhập thành công!");
                    return username;
                }
            }

            return "";
        }
        static void ChangePassword(string username)
        {
            Console.Clear();
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Nhập mật khẩu cũ :                                ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(28, Console.CursorTop - 2);

            string oldPassword = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Nhập mật khẩu mới :                               ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(28, Console.CursorTop - 2);
            string newPassword = Console.ReadLine();

            if (users.ContainsKey(username) && users[username] == oldPassword)
            {
                users[username] = newPassword;
                Console.Clear();
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   ĐỔI MẬT KHẨU THÀNH CÔNG!                          ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("   ╔═══════════════════════════════╗");
                Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                Console.WriteLine("   ╚═══════════════════════════════╝");

                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   SAI MẬT KHẨU CŨ . ĐỔI MẬT KHẨU THẤT BẠI !         ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("   ╔═══════════════════════════════╗");
                Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                Console.WriteLine("   ╚═══════════════════════════════╝");

                Console.ReadLine();
                Console.Clear();
            }
        }
        static void ResetPassword()
        {
            Console.Clear();

            Console.WriteLine("   ╔══════════════════╗  ");
            Console.WriteLine("   ║  RESET MẬT KHẨU  ║  ");
            Console.WriteLine("   ╚══════════════════╝");
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Tài khoản:                                        ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(20, Console.CursorTop - 2);
            string username = Console.ReadLine();

            if (users.ContainsKey(username))
            {
                Console.WriteLine();
                Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("   ║   Mật khẩu mới :                                    ║");
                Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                Console.SetCursorPosition(20, Console.CursorTop - 2);
                string newPassword = Console.ReadLine();
                users[username] = newPassword;
                Console.Clear();
                Console.WriteLine("   ╔══════════════════════════════════════════╗");
                Console.WriteLine("   ║   Đặt lại mật khẩu thành công!           ║");
                Console.WriteLine("   ╚══════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("   ╔═══════════════════════════════╗");
                Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                Console.WriteLine("   ╚═══════════════════════════════╝");

                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("   ╔══════════════════════════════════════════╗");
                Console.WriteLine("   ║   Tài khoản không tồn tại ! Thất bại .   ║");
                Console.WriteLine("   ╚══════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("   ╔═══════════════════════════════╗");
                Console.WriteLine("   ║   Nhấn ENTER để tiếp tục ...  ║");
                Console.WriteLine("   ╚═══════════════════════════════╝");
                Console.ReadLine();
                Console.Clear();

            }
        }
        static void MenuAdmin()
        {

            Console.Clear();


            while (true)
            {
                Console.Clear();

                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                      MENU Admin!                       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  1. Thêm người dùng              ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  2. Xóa người dùng               ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  3. Danh sách người dùng         ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  4. Quay lại                     ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  5. Menu chính                   ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");

                Console.Write("  >>>   Nhập lựa chọn của bạn  : ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        SaveUsersToFile();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine("   ║  Nhập người tên TK người dùng cần xóa :             ║");
                        Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                        Console.SetCursorPosition(45, Console.CursorTop - 2);
                        string username = Console.ReadLine();
                        if (users.ContainsKey(username) && userRoles[username] == "user")
                        {
                            users.Remove(username);
                            userRoles.Remove(username);
                            Console.WriteLine();
                            Console.WriteLine("   ╔═════════════════════╗");
                            Console.WriteLine("   ║  Xóa thành công     ║");
                            Console.WriteLine("   ╚═════════════════════╝");
                            SaveUsersToFile();

                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("   ╔═══════════════════════════════════════════════════════════════╗");
                            Console.WriteLine("   ║  Không tìm thấy hoặc người này không thuộc quyền hạng của bạn ║");
                            Console.WriteLine("   ╚═══════════════════════════════════════════════════════════════╝");
                        }
                        Console.ReadKey();
                        break;
                    case "3":
                        LoadUsersFromFile();

                        foreach (var user in users)
                        {
                            Console.WriteLine("Username: {0}, Role: {1}", user.Key, userRoles[user.Key]);
                        }
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.Clear();
                        return;
                    case "5": menu(); break;
                    default:
                        Console.Clear();

                        Console.WriteLine("  LỰA CHỌN KHÔNG HỢP LỆ !   ");
                        Console.WriteLine();
                        Console.ReadKey();
                        Console.WriteLine();
                        break;
                }
            }
        }
        static void timkiemtheoma()
        {
            kieunv[] dsnv = new kieunv[100];
            int count = 0;

            // Đọc dữ liệu từ file
            StreamReader reader = new StreamReader("C:\\DSnhanvien.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                kieunv nv = new kieunv();
                string[] tokens = line.Split('|');
                if (tokens.Length >= 2)
                {
                    if (int.TryParse(tokens[0], out int manv))
                    {
                        nv.manv = manv;
                    }
                    nv.tennv = tokens[1];
                    if (tokens.Length >= 3)
                    {
                        nv.tuoinv = 0;
                        if (int.TryParse(tokens[2], out int tuoi))
                        {
                            nv.tuoinv = tuoi;
                        }
                    }
                    if (tokens.Length >= 4)
                    {
                        nv.gioitinhnv = tokens[3];
                    }
                    if (tokens.Length >= 5)
                    {
                        nv.quequannv = tokens[4];
                    }
                    if (tokens.Length >= 6)
                    {
                        nv.chucvunv = tokens[5];
                    }
                    dsnv[count] = nv;
                    count++;
                }
            }
            reader.Close();
            // Tìm kiếm nhân viên theo mã
            Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("   ║   Nhập mã :                                         ║");
            Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(25, Console.CursorTop - 2);
            int maNV = int.Parse(Console.ReadLine());
            bool timThay = false;
            foreach (kieunv nv in dsnv)
            {
                if (nv.manv == maNV)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("\n\t\tTHÔNG TIN NHÂN VIÊN");
                    Console.WriteLine("╔════════╦════════════════════════════════════╗");
                    Console.WriteLine("║  Mã NV ║              Họ tên                ║");
                    Console.WriteLine("╠════════╬════════════════════════════════════╣");
                    Console.WriteLine("║ {0,-7} ║ {1,-32}                            ║", nv.manv, nv.tennv);
                    Console.WriteLine("╠════════╬════════════════════════════════════╣");
                    Console.WriteLine("║ Tuổi: {0,-4}                                ║", nv.tuoinv);
                    Console.WriteLine("╠════════╬════════════════════════════════════╣");
                    Console.WriteLine("║ Giới tính: {0,-8}                           ║", nv.gioitinhnv);
                    Console.WriteLine("╠════════╬════════════════════════════════════╣");
                    Console.WriteLine("║ Quê quán: {0,-30}                           ║", nv.quequannv);
                    Console.WriteLine("╠════════╬════════════════════════════════════╣");
                    Console.WriteLine("║ Chức vụ: {0,-28}                            ║", nv.chucvunv);
                    Console.WriteLine("╚════════╩════════════════════════════════════╝");
                    timThay = true;
                    break;
                }
            }
            if (!timThay)
            {
                Console.WriteLine("Không tìm thấy nhân viên mã {0}", maNV);
            }
        }

        static void MenuUser(string loggedInUser) // Thêm tham số để nhận giá trị người dùng đăng nhập thành công
        {


            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                       MENU User!                       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  1. Thay đổi mật khẩu            ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  2. Quay lại                     ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  3. Menu chính                   ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.Write("  >>>   Nhập lựa chọn của bạn  : ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ChangePassword(loggedInUser); // Sử dụng giá trị người dùng đăng nhập thành công
                        SaveUsersToFile();
                        break;
                    case "2":
                        return;
                    case "3": menuuser(); break;
                    default:
                        Console.Clear();

                        Console.WriteLine("  LỰA CHỌN KHÔNG HỢP LỆ !   ");

                        Console.WriteLine();
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void GhiTepGopY(string gopY)
        {
            string filePath = @"C:\gopy.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(gopY);
                    Console.WriteLine("Góp ý đã được lưu thành công vào tệp tin.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi trong quá trình lưu góp ý: " + ex.Message);
            }
        }
        static void DocTepGopY()
        {
            string filePath = @"C:\gopy.txt";

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    Console.WriteLine("Nội dung góp ý:");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Đã xảy ra lỗi trong quá trình đọc tệp tin góp ý: " + ex.Message);
            }
        }
        static void nhapGy()
        {
            Console.Clear();
            Console.WriteLine("   ╔═══════════════╗  ");
            Console.WriteLine("   ║     GÓP Ý     ║  ");
            Console.WriteLine("   ╚═══════════════╝");
            string thongTinnhanvien;
            Console.WriteLine("Thông tin:");
            Console.WriteLine("Mã nhân viên_Chức vụ_Họ tên_Quê quán_Ngày sinh:");
            thongTinnhanvien = Console.ReadLine();

            string thongTinLienLac;
            Console.WriteLine("Thông tin liên lạc (SDT_Emaill):");
            thongTinLienLac = Console.ReadLine();
            string gopyy;
            gopyy = Console.ReadLine();

            string gopY = thongTinnhanvien + "_" + thongTinLienLac + "_" + gopyy;

            GhiTepGopY(gopY);
        }
        static void menuuser()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                     MENU User L2!                      ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  1. Ý kiến của nhân viên         ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  2. Tính lương nhân viên         ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  3. Tìm kiếm nhân viên ( mã )    ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.WriteLine("║           ║  4. Quay lại                     ║         ║");
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.Write("  >>>   Nhập lựa chọn của bạn  : ");
                int chonus = int.Parse(Console.ReadLine());
                switch (chonus)
                {
                    case 1:
                        nhapGy(); Console.ReadKey();
                        break;
                    case 2:
                        tinhluong(); Console.ReadKey();
                        break;
                    case 3:
                        timkiemtheoma(); Console.ReadKey();
                        break;
                    case 4: return;

                }
            }
        }
        static void dangnhaptk()
        {
            int selectedOption = 1;
            bool optionSelected = false;

            while (!optionSelected)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   LỰA CHỌN ĐĂNG NHẬP                   ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║           ║  1. Đăng nhập admin              ║         ║");
                Console.ResetColor();
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║           ║   2. Đăng nhập người dùng        ║         ║");
                Console.ResetColor();
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Blue : ConsoleColor.White;
                Console.WriteLine("║           ║   3. Quay lại                    ║         ║");
                Console.ResetColor();
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.WriteLine("  >>>   Sử dụng các phím mũi tên lên/xuống để di chuyển, sau đó nhấn Enter để chọn.");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedOption > 1)
                            selectedOption--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedOption < 3)
                            selectedOption++;
                        break;
                    case ConsoleKey.Enter:
                        optionSelected = true;
                        break;
                }

            }
            switch (selectedOption)
            {
                case 1:
                    string adminUsername = Login("admin");
                    if (adminUsername != "")
                    {
                        Console.Clear();
                        Console.WriteLine("   ╔═══════════════╗  ");
                        Console.WriteLine("   ║ MÃ BẢO VỆ AD! ║  ");
                        Console.WriteLine("   ╚═══════════════╝");
                        Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine("   ║   NHẬP MÃ DÀNH CHO TK ADMIN :                       ║");
                        Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                        Console.SetCursorPosition(35, Console.CursorTop - 2);

                        string code = Console.ReadLine();

                        if (code == "0000")
                        {
                            Console.WriteLine("Mã hợp lệ. Chuyển đến menu admin.");
                            loggedInUser = adminUsername;
                            isLoggedIn = true;
                            MenuAdmin();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("  SAI MÃ . ĐĂNG NHẬP THẤT BẠI !   ");
                            Console.WriteLine();
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("   ĐĂNG NHẬP THẤT BẠI !   ");
                        Console.WriteLine();
                        Console.ReadKey();
                    }
                    break;
                case 2:
                    string userUsername = Login("user");
                    if (userUsername != "")
                    {
                        Console.WriteLine("Đăng nhập thành công với tài khoản người dùng: " + userUsername);
                        loggedInUser = userUsername;
                        isLoggedIn = true;
                        MenuUser(loggedInUser);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("   ĐĂNG NHẬP THẤT BẠI !   ");
                        Console.WriteLine();
                        Console.ReadKey();
                    }
                    break;
                case 3:
                    // Quay lại
                    // Thêm mã lệnh để xử lý việc quay lại nếu cần.
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                    break;
            }


            // Sử dụng selectedOption để xử lý tùy chọn đã chọn.
            Console.WriteLine("Bạn đã chọn tùy chọn số: " + selectedOption);
        }
        static void dangkytk()
        {
            int selectedOption = 1;
            ConsoleKeyInfo keyInfo;

            do
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                     lỰA CHỌN ĐĂNG KÝ                   ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                if (selectedOption == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("║           ║  1. Đăng ký tài khoản admin      ║         ║");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("║           ║  1. Đăng ký tài khoản admin      ║         ║");
                }
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                if (selectedOption == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("║           ║  2. Đăng ký tài khoản người dùng ║         ║");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("║           ║  2. Đăng ký tài khoản người dùng ║         ║");
                }
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║           ╔══════════════════════════════════╗         ║");
                if (selectedOption == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("║           ║  3. Quay lại                     ║         ║");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("║           ║  3. Quay lại                     ║         ║");
                }
                Console.WriteLine("║           ╚══════════════════════════════════╝         ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.Write("  >>>   DÙNG NÚT DI CHUYỂN ! TKANK YOU ");

                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
                {
                    selectedOption--;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 3)
                {
                    selectedOption++;
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            // Sau khi lựa chọn được xác nhận
            switch (selectedOption)
            {
                case 1:
                    RegisterAdmin();
                    SaveUsersToFile();
                    break;
                case 2:
                    RegisterUser();
                    SaveUsersToFile();
                    break;
                case 3: returnToMain = true; break;
                default:
                    Console.Clear();
                    Console.WriteLine("  LỰA CHỌN KHÔNG HỢP LỆ !   ");

                    Console.WriteLine();

                    break;
            }
        }
        static void Main()
        {

            Console.Clear();
            LoadUsersFromFile();
            Console.InputEncoding = UnicodeEncoding.Unicode;
            Console.OutputEncoding = UnicodeEncoding.Unicode;
            bool returnToMain = false; // Biến cờ để quay lại hàm Main
            int selectedOption = 1; // Mục đã được chọn ban đầu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("   ██╗  ██╗  ██████   ███╗   ███╗███████╗       ╔══════════════════════════════════════════════════════╗           ");
                Console.WriteLine("   ██║  ██║ ██╔═══██╗ ████╗ ████║██╔════╝       ║   SINH VIÊN    : NGUYỄN HỮU LINH                     ║           ");
                Console.WriteLine("   ███████║ ██║   ██║ ██╔████╔██║█████╗         ║   MÃ SINH VIÊN : 12522056                            ║          ");
                Console.WriteLine("   ██╔══██║ ██║   ██║║██║╚██╔╝██║██╔══╝         ║   LỚP : 125223                                       ║          ");
                Console.WriteLine("   ██║  ██║ ╚██████╔╝║██║ ╚═╝ ██║███████╗       ║   IT_ CÔNG NGHỆ THÔNG TIN_KỸ THUẬT PHẦN MỀM          ║        ");
                Console.WriteLine("   ╚═════╝ ╚═╝  ╚═╝ ╚═╝     ╚═╝╚══════╝         ╚══════════════════════════════════════════════════════╝           ");


                Console.WriteLine("╔════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   LỰA CHỌN CỦA BẠN                 ║");
                Console.WriteLine("╠════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                    ║");
                Console.WriteLine("║             ╔═════════════════════════╗            ║");
                Console.WriteLine(GetMenuItem(1, selectedOption));
                Console.WriteLine("║             ╚═════════════════════════╝            ║");
                Console.WriteLine("║             ╔═════════════════════════╗            ║");
                Console.WriteLine(GetMenuItem(2, selectedOption));
                Console.WriteLine("║             ╚═════════════════════════╝            ║");
                Console.WriteLine("║             ╔═════════════════════════╗            ║");
                Console.WriteLine(GetMenuItem(3, selectedOption));
                Console.WriteLine("║             ╚═════════════════════════╝            ║");
                Console.WriteLine("║             ╔═════════════════════════╗            ║");
                Console.WriteLine(GetMenuItem(4, selectedOption));
                Console.WriteLine("║             ╚═════════════════════════╝            ║");
                Console.WriteLine("║             ╔═════════════════════════╗            ║");
                Console.WriteLine(GetMenuItem(5, selectedOption));
                Console.WriteLine("║             ╚═════════════════════════╝            ║");
                Console.WriteLine("║             ╔═════════════════════════╗            ║");
                Console.WriteLine(GetMenuItem(6, selectedOption));
                Console.WriteLine("║             ╚═════════════════════════╝            ║");
                Console.WriteLine("║                                                    ║");
                Console.WriteLine("╚════════════════════════════════════════════════════╝");

                // Di chuyển lên xuống và chọn mục
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedOption = Math.Max(1, selectedOption - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedOption = Math.Min(6, selectedOption + 1);
                        break;
                    case ConsoleKey.Enter:
                        returnToMain = HandleSelection(selectedOption);
                        break;
                }

                if (returnToMain)
                {
                    break;
                }
            }
        }
        static string GetMenuItem(int menuItem, int selectedOption)
        {
            string item = string.Empty;
            if (menuItem == selectedOption)
            {
                item = "║            >> ";
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                item = "║               ";
            }

            switch (menuItem)
            {
                case 1:
                    item += "Đăng ký";
                    break;
                case 2:
                    item += "Đăng nhập";
                    break;
                case 3:
                    item += "Đổi mật khẩu";
                    break;
                case 4:
                    item += "Đăng xuất";
                    break;
                case 5:
                    item += "Reset lại mật khẩu";
                    break;
                case 6:
                    item += "Thoát";
                    break;
            }

            Console.ResetColor();
            item = item.PadRight(51) + "  ║";
            return item;
        }
        static bool isLoggedIn = false; // Biến theo dõi trạng thái đăng nhập
        static string loggedInUser = ""; // Biến lưu tên người dùng đã đăng nhập
        static bool HandleSelection(int selectedOption)
        {

            Console.Clear();
            switch (selectedOption)
            {
                case 1:
                    Console.Clear();

                    dangkytk();

                    break;

                // Thực hiện logic cho Đăng ký

                case 2:
                    Console.Clear();
                    if (isLoggedIn)
                    {
                        Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine("   ║      BẠN ĐANG ĐĂNG NHẬP VUI LÒNG ĐĂNG XUẤT          ║");
                        Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                        Console.ReadKey();
                    }
                    else
                    {
                        dangnhaptk();
                    }
                    break;
                // Thực hiện logic cho Đăng nhập

                case 3:
                    if (isLoggedIn)
                    {
                        ChangePassword(loggedInUser);
                        SaveUsersToFile();
                    }
                    else
                    {
                        Console.Clear();

                        Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine("   ║      BẠN ĐANG ĐĂNG NHẬP TRƯỚC KHI ĐỔI MẬT KHẨU      ║");
                        Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                        Console.ReadKey();
                    }
                    break;

                case 4:
                    if (isLoggedIn)
                    {
                        Console.Clear();
                        Console.WriteLine("   ╔════════════════════════════════╗");
                        Console.WriteLine("   ║      ĐĂNG XUẤT THÀNH CÔNG !    ║");
                        Console.WriteLine("   ╚════════════════════════════════╝");
                        isLoggedIn = false;
                        loggedInUser = "";
                    }
                    else
                    {
                        Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine("   ║      BẠN CHƯA ĐĂNG NHẬP TÀI KHOẢN NÀO     !         ║");
                        Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                        Console.ReadKey();
                    }
                    Console.ReadKey();
                    break;
                case 5:
                    if (isLoggedIn)
                    {
                        Console.Clear();

                        Console.WriteLine("   ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine("   ║   CHỨC NĂNG NÀY KHÔNG KHẢ DỤNG KHI ĐANG ĐĂNG NHẬP   ║");
                        Console.WriteLine("   ╚═════════════════════════════════════════════════════╝");
                        Console.ReadKey();

                        Console.WriteLine();
                        Console.ReadKey();

                    }
                    else
                    {
                        ResetPassword();
                        SaveUsersToFile();
                    }
                    break;
                case 6:
                    Environment.Exit(0); break;
            }
            return false; // Tiếp tục lặp trong hàm Main
        }
    }
}
